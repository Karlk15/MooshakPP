using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MooshakPP.Models.ViewModels;
using MooshakPP.Models.Entities;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;


namespace MooshakPP.Services
{
    public class StudentService
    {
        private Models.ApplicationDbContext db;

        public StudentService()
        {
            db = new Models.ApplicationDbContext();
        }

        public IndexViewModel Index(string userId, int? courseId, int? assignmentId, int? milestoneId)
        {
            IndexViewModel newIndex = new IndexViewModel();
            if (courseId != null)
            {
                newIndex.courses = GetCourses(userId);
                newIndex.assignments = GetAssignments((int)courseId);
                if (assignmentId != null)
                {
                    newIndex.milestones = GetMilestones((int)assignmentId);
                    newIndex.currentAssignment = GetAssignmentByID((int)assignmentId);
                    if(milestoneId != null)
                        newIndex.currentMilestone = GetMilestoneByID((int)milestoneId);
                }
                newIndex.currentCourse = GetCourseByID((int)courseId);
                
                //newIndex.studentSubmissions = GetSubmissions(userId);
            }
            return newIndex;
        }

        public SubmissionViewModel Submissions(string userId, int milestoneId)
        {
            SubmissionViewModel mySubmissions = new SubmissionViewModel();
            mySubmissions.mySubmissions = GetSubmissions(userId, milestoneId);
            return mySubmissions;
        }

        public DescriptionViewModel Description(int milestoneId)
        {
            DescriptionViewModel description = new DescriptionViewModel();
            description.milestone = GetMilestoneByID(milestoneId);
            return description;
        }

        public DetailsViewModel Details(int submissionId)
        {
            return null;
        }

        //mileID is milestone ID
        public bool CreateSubmission(string userID, string userName, int mileID, HttpPostedFileBase file)
        {

            string code;
            string fileName = file.FileName;

            //produce uploaded code
            using (StreamReader sr = new StreamReader(file.InputStream))
            {
                code = sr.ReadToEnd();
            }

            //Get the submission directory relative location from AppSettings
            string submissionDir = ConfigurationManager.AppSettings["SubmissionDir"];

            //Get working directory information
            submissionDir = GetMilestonePath(submissionDir, mileID);

            List<TestCase> testCases = GetTestCasesByMilestoneID(mileID);

            string userSubmission = submissionDir + userName + "\\Submission ";

            //Find an unused submission number
            int i = 1;
            while (Directory.Exists(userSubmission + i))
            {
                i++;
            }
            // the "\\" is vital
            userSubmission += i + "\\";

            Directory.CreateDirectory(userSubmission);

            var workingFolder = userSubmission;
            var cppFileName = fileName;
            var exeFilePath = workingFolder + fileName;

            // Write the code to a file, such that the compiler
            // can find it:
            System.IO.File.WriteAllText(workingFolder + cppFileName, code);

            var compilerFolder = ConfigurationManager.AppSettings["compilerFolder"];


            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = workingFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;

            bool failed = false;
            foreach(TestCase test in testCases)
            {
                compiler.Start();
                compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
                compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + cppFileName);
                compiler.StandardInput.WriteLine("exit");
                string output = compiler.StandardOutput.ReadToEnd();
                compiler.WaitForExit();
                compiler.Close();

                //Read the expected output of current test case
                using (StreamReader sr = new StreamReader(test.outputUrl))
                {
                    string expected = sr.ReadToEnd();
                    if(expected != output)
                    {
                        failed = true;
                    }
                }
            }

            //TODO, MAKE SURE CRASHES ARE NOT ACCEPTED, PROBABLY WITH TRY/CATCH
            Submission submission = new Submission();
            submission.fileURL = userSubmission;
            submission.milestoneID = mileID;
            //not yet rated
            submission.status = result.none;
            submission.userID = userID;
            if(testCases.Count > 0)
            {
                if (!failed)
                {
                    submission.status = result.Accepted;

                }
                else
                {
                    submission.status = result.wrongAnswer;
                }
                //save submission
                db.Submissions.Add(submission);
                db.SaveChanges();
            }

            return true;
        }

        public List<TestCase> GetTestCasesByMilestoneID(int milestoneID)
        {
            List<TestCase> testCases = (from c in GetTestCases()
                                        where c.milestoneID == milestoneID
                                        select c).ToList();
            return testCases;
        }

        public List<TestCase> GetTestCases()
        {
            List<TestCase> testCases = (from c in db.Testcases
                                        select c).ToList();
            return testCases;
        }

        // Enter a folder root from ConfigurationManager.AppSettings and a milestone ID to generate an absolute
        // directory path to the milestone folder
        public string GetMilestonePath(string testCaseRoot, int mileID)
        {
            // Assign subdirectories
            Milestone milestone = GetMilestoneByID(mileID);
            Assignment assignment = GetAssignmentByID(milestone.assignmentID);
            Course course = GetCourseByID(assignment.courseID);
            string caseDir = testCaseRoot + "\\" + course.name + "\\" + assignment.title + "\\" + milestone.name + "\\";

            // Make the path absolute
            caseDir = HttpContext.Current.Server.MapPath(caseDir);
            return caseDir;

        }

        public int? GetFirstCourse(string userId)
        {
            List<Course> courses = GetCourses(userId);
            if (courses.Count != 0)
            {
                return courses[0].ID;
            }
            return null;
        }

        public int? GetFirstAssignment(int courseId)
        {
            List<Assignment> assignments = GetAssignments(courseId);
            if (assignments.Count != 0)
            {
                return assignments[0].ID;
            }
            return null;
        }

        public int? GetFirstMilestone(int? assignmentId)
        {
            if (assignmentId != null)
            {
                List<Milestone> milestones = GetMilestones((int)assignmentId);
                if (milestones.Count != 0)
                {
                    return milestones.FirstOrDefault().ID;

                }
            }
            return null;
        }

        public Course GetCourse(int courseID)
        {
            Course theCourse = GetCourseByID(courseID);
            return theCourse;
        }

        public bool unpackZip(string zipPath, string extractPath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        // create the subdirectories inside the zip
                        string newDir = Directory.GetParent(extractPath + "\\" + entry.FullName).FullName;
                        if (!Directory.Exists(newDir))
                        {
                            Directory.CreateDirectory(newDir);
                        }
                        // Save the zipped file
                        entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
                    }
                }
            }
            return true;

        }

        protected List<Course> GetCourses(string userId)
        {
            List<Course> courses = (from c in db.UsersInCourses
                           where c.userID == userId
                           select c.course).ToList();
            return courses;
        }

        protected Course GetCourseByID(int courseId)
        {
            Course theCourse = (from c in db.Courses
                             where c.ID == courseId
                             select c).SingleOrDefault();
            
            return theCourse;
        }

        protected List<Assignment> GetAssignments(int courseId)
        {
            List<Assignment> assignments = (from a in db.Assignments
                               where a.courseID == courseId
                               select a).ToList();

            return assignments;
        }

        protected Assignment GetAssignmentByID(int assignmentId)
        {
            Assignment assignment = (from a in db.Assignments
                              where a.ID == assignmentId
                              select a).FirstOrDefault();
            return assignment;
        }

        protected List<Milestone> GetMilestones(int assignmentId)
        {
            List<Milestone> milestones = (from m in db.Milestones
                              where m.assignmentID == assignmentId
                              select m).ToList();
            return milestones;
        }

        protected Milestone GetMilestoneByID(int milestoneId)
        {
            Milestone milestone = (from m in db.Milestones
                             where m.ID == milestoneId
                             select m).FirstOrDefault();
            return milestone;
        }

        protected List<Submission> GetSubmissions(string userId, int milestoneId)
        {
            List<Submission> submissions = (from s in db.Submissions
                               where s.userID == userId && s.milestoneID == milestoneId
                               select s).ToList();
            return submissions;
        }

        protected Submission GetSubmissionByID(int submissionId)
        {
            Submission submission = (from s in db.Submissions
                              where s.ID == submissionId
                              select s).FirstOrDefault();
            return submission;
        }
    }
}