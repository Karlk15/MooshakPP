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
using MooshakPP.DAL;

namespace MooshakPP.Services
{
    public class StudentService
    {
        private IdentityManager manager = new IdentityManager();
        //private Models.ApplicationDbContext db;
        private SubmissionTester st;
        private readonly Models.IAppDataContext db;

        //Models.IAppDataContext context <----- should be a parameter for unit testing
        public StudentService(Models.IAppDataContext context)
        {
            //db = new Models.ApplicationDbContext();
            db = context ?? new Models.ApplicationDbContext();
            st = new SubmissionTester();
        }

        public IndexViewModel Index(string userId, int? courseId, int? assignmentId, int? milestoneId)
        {
            IndexViewModel newIndex = new IndexViewModel();
            if (courseId != null)
            {
                newIndex.courses = GetCourses(userId);
                newIndex.assignments = GetAssignments((int)courseId);
                if (assignmentId != null && assignmentId != 0)
                {
                    newIndex.milestones = GetMilestones((int)assignmentId);
                    newIndex.currentAssignment = GetAssignmentByID((int)assignmentId);
                    if (milestoneId != null && milestoneId != 0)
                    {
                        newIndex.currentMilestone = GetMilestoneByID((int)milestoneId);
                        List<Submission> tempSubmissions = GetSubmissions(userId, (int)milestoneId);
                        if ( tempSubmissions != null && tempSubmissions.Count != 0)
                        {
                            newIndex.mySubmissions = mySubmissions(userId, (int)milestoneId);
                        }
                        else
                        {
                            newIndex.mySubmissions = new SubmissionViewModel();
                            newIndex.mySubmissions.submissions = new List<Submission>();
                        }
                        List<Submission> tempAll = GetAllSubmissions((int)milestoneId);
                        if(tempAll != null && tempAll.Count != 0)
                        {
                            newIndex.allSubmissions = allSubmissions((int)milestoneId);
                        }
                        else
                        {
                            newIndex.allSubmissions = new SubmissionViewModel();
                            newIndex.allSubmissions.submissions = new List<Submission>();
                        }
                    }
                    else
                    {
                        newIndex.currentMilestone = new Milestone();
                    }
                }
                else
                {
                    newIndex.currentAssignment = new Assignment();
                    newIndex.milestones = new List<Milestone>();
                    newIndex.currentMilestone = new Milestone();
                    newIndex.bestSubmissions = new AllSubmissionsViewModel();
                }
                
                newIndex.currentCourse = GetCourseByID((int)courseId);
            }
            return newIndex;
        }

        // Generate the path to a specific user submission
        public DownloadModel GetDownloadModel(int? submissionID)
        {
            DownloadModel model = new DownloadModel();
            if (submissionID == null)
            {
                throw new FileNotFoundException("Download attempt on null submission");
            }

            Submission submission = GetSubmissionByID((int)submissionID);
            // Get the directory containing the submission
            string filePath = submission.fileURL;
            // Find one file in the specified language
            model.filePath = Directory.GetFiles(filePath, "*.cs").FirstOrDefault();
            // Keep it's name sperately
            model.filename = Path.GetFileName(model.filePath);
            // file encoding
            model.mimetype = "text/x-c";
            return model;
        }

        // Load the current submission, all of it's wrong outputs, expected outputs and inputs
        public DetailsViewModel GetDetails(int submissionID, string userName)
        {
            DetailsViewModel details = new DetailsViewModel();
            details.submission = GetSubmissionByID(submissionID);

            // Get all testcases if. details.submission is still null, an exception will be thrown
            List<TestCase> testcases = GetTestCasesByMilestoneID(details.submission.milestoneID);

            // Get all wrong outputs and match them with their test cases
            details.tests = new List<ComparisonViewModel>();
            foreach (string file in Directory.GetFiles(details.submission.fileURL + "\\Wrong outputs\\", "*.txt"))
            {
                // Get the test case index from the output filename (ex: "2.txt")
                int index = Convert.ToInt32(Path.GetFileNameWithoutExtension(file)) - 1;
                ComparisonViewModel comp = new ComparisonViewModel();
                using (StreamReader sr = new StreamReader(testcases[index].inputUrl))
                {
                    // Get input used in current test case
                    comp.input = sr.ReadToEnd();
                }
                using (StreamReader sr = new StreamReader(testcases[index].outputUrl))
                {
                    comp.expectedOut = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        // Get expected output
                        comp.expectedOut.Add(sr.ReadLine());
                    }
                }
                using (StreamReader sr = new StreamReader(file))
                {
                    comp.obtainedOut = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        // Get obtained output
                        comp.obtainedOut.Add(sr.ReadLine());
                    }
                }
                details.tests.Add(comp);
            }
            return details;
        }

        public SubmissionViewModel mySubmissions(string userId, int milestoneId)
        {
            SubmissionViewModel mySubmissions = new SubmissionViewModel();
            mySubmissions.submissions = GetSubmissions(userId, milestoneId);
            mySubmissions.currentMilestone = GetMilestoneByID(milestoneId);
            return mySubmissions;
        }

        public SubmissionViewModel allSubmissions(int milestoneId)
        {
            SubmissionViewModel mySubmissions = new SubmissionViewModel();
            mySubmissions.submissions = GetAllSubmissions(milestoneId);
            mySubmissions.currentMilestone = GetMilestoneByID(milestoneId);
            mySubmissions.submittedUser = new List<Models.ApplicationUser>();
            foreach (Submission s in mySubmissions.submissions)
            {
                mySubmissions.submittedUser.Add(manager.GetUser(manager.GetUserById(s.userID).Email));
            }
            
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

        // Save the given file, compile the code, run test cases on it and return the result
        public result CreateSubmission(string userID, string userName, int mileID, HttpPostedFileBase file)
        {
            List<TestCase> testCases = GetTestCasesByMilestoneID(mileID);
            //Get the submission directory relative location from AppSettings
            string submissionDir = ConfigurationManager.AppSettings["SubmissionDir"];

            //Get working directory information
            string userSubmission = GetMilestonePath(submissionDir, mileID) + userName + "\\Submission ";
            //Find an unused submission number
            int i = 1;
            while (Directory.Exists(userSubmission + i))
            {
                i++;
            }
            // Append submission number and "\\" to end directory path
            userSubmission += i + "\\";
            Directory.CreateDirectory(userSubmission);

            string workingFolder = userSubmission;
            // Save submission
            file.SaveAs(workingFolder + file.FileName);

            // Run tests on the given file using testCases
            if (testCases.Count > 0)
            {
                Submission submission = new Submission();
                // Pass submission by reference so that the passCount can be assigned by the tester
                result testResult = TestSubmission(workingFolder, file.FileName, ref testCases, ref submission);

                submission.fileURL = userSubmission;
                submission.milestoneID = mileID;
                submission.status = testResult;
                submission.userID = userID;

                //save submission
                db.Submissions.Add(submission);
                db.SaveChanges();
                return testResult;
            }

            // Can only happen if the milestone has no test cases
            return result.none;
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

        // Run the entire testing process on a compiled program, pass submission to assign the passCount
        protected result TestSubmission(string workingFolder, string fileName, ref List<TestCase> testCases, ref Submission submission)
        {
            // Create a new compiler process
            Process compiler = new Process();
            // Initialize compiler in workingFolder
            st.InitCompiler(ref compiler, workingFolder);

            result testResult = result.none;
            // find out if program is C++ or C#
            if(fileName.EndsWith(".cpp"))
            {   // Compile c++
                testResult = st.CompileCPP(ref compiler, fileName);
            }
            else if(fileName.EndsWith(".cs"))
            {   // Compile C#
                testResult = st.CompileCS(ref compiler, fileName);
            }
            else
            {   //unsupported file format or language
                return result.compError;
            }

            // Get .exe file path if it exists
            string exeFilePath = Directory.GetFiles(workingFolder, "*.exe").FirstOrDefault();
            // If compiler didn't throw an exception and it's .exe has been found
            if (testResult != result.compError && !string.IsNullOrEmpty(exeFilePath))
            {
                // Initialize executable process
                ProcessStartInfo processInfoExe = new ProcessStartInfo(exeFilePath, "");
                st.InitTester(ref processInfoExe);
                // Run program on test cases
                testResult = st.TestSubmission(ref processInfoExe, ref testCases, workingFolder, ref submission);
            }
            else
            {   // Compiler did not successfully create a .exe
                return result.compError;
            }
            return testResult;
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
                               orderby s.ID descending
                               select s).ToList();
            return submissions;
        }

        protected List<Submission> GetAllSubmissions(int milestoneId)
        {
            List<Submission> submissions = (from s in db.Submissions
                                            where s.milestoneID == milestoneId
                                            orderby s.ID descending
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