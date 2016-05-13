using Microsoft.AspNet.Identity;
using MooshakPP.Models;
using MooshakPP.Models.Entities;
using MooshakPP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using MooshakPP.DAL;

namespace MooshakPP.Services
{
    public class TeacherService : StudentService
    {
        private ApplicationDbContext db;
        private IdentityManager manager;
        //private readonly IAppDataContext db;
        public TeacherService(IAppDataContext context) : base(context)
        {
            //db = context ?? new ApplicationDbContext();
            db = new ApplicationDbContext();
            manager = new IdentityManager();
        }

        public CreateAssignmentViewModel AddAssignment(string userID, int courseID, int? assignmentID)
        {
            CreateAssignmentViewModel allAssignments = new CreateAssignmentViewModel();
            allAssignments.courses = GetCourses(userID);
            allAssignments.assignments = new List<Assignment>(GetAssignments(courseID));
            allAssignments.currentCourse = GetCourseByID(courseID);
            if(assignmentID != null && assignmentID != 0)
            {
                allAssignments.currentAssignment = GetAssignmentByID((int)assignmentID);
                string startDate = allAssignments.currentAssignment.startDate.ToString();
                allAssignments.start = startDate.Substring(0, startDate.IndexOf(" "));

                string dueDate = allAssignments.currentAssignment.dueDate.ToString();
                allAssignments.due = dueDate.Substring(0, dueDate.IndexOf(" "));
            }

            return allAssignments;
        }

        public CreateMilestoneViewModel AddMilestone(int assId, int? currMilestoneId)
        {
            CreateMilestoneViewModel model = new CreateMilestoneViewModel();
            model.milestones = GetMilestones(assId);
            if(currMilestoneId == null || currMilestoneId == 0)
            {
                model.currentMilestone = new Milestone();
                model.currentMilestone.assignmentID = assId; 
            }
            else
            {
                model.currentMilestone = GetMilestoneByID((int)currMilestoneId);
            }
            model.currentAssignment = GetAssignmentByID(assId);
            
            return model;
        }

        public RecoverAssignmentsViewModel RecoverAssignments(string teacherID, int courseID, int? currentAssignmentID)
        {
            RecoverAssignmentsViewModel recoverAssignments = new RecoverAssignmentsViewModel();
            recoverAssignments.deletedAssignments = GetDeletedAssignments(teacherID);
            recoverAssignments.courses = GetCourses(teacherID);
            recoverAssignments.currentCourse = GetCourseByID(courseID);
            if (currentAssignmentID != null && currentAssignmentID != 0)
            {
                recoverAssignments.currentSelected = GetAssignmentByID((int)currentAssignmentID);
            }
            else
            {
                recoverAssignments.currentSelected = new Assignment();
                recoverAssignments.currentSelected.ID = 0;
            }
            return recoverAssignments;
        }

        public AllSubmissionsViewModel bestSubmissions(int? milestoneId, string userId)
        {
            AllSubmissionsViewModel bestSubmissions = new AllSubmissionsViewModel();
            if (milestoneId != null && milestoneId != 0)
            {
                bestSubmissions.currentMilestone = GetMilestoneByID((int)milestoneId);
                bestSubmissions.milestones = GetMilestones(bestSubmissions.currentMilestone.assignmentID);
                bestSubmissions.users = GetUsersInCourse(GetAssignmentByID(bestSubmissions.currentMilestone.assignmentID).courseID);
                bestSubmissions.submittedUser = manager.GetUserById(userId);
                if (bestSubmissions.submittedUser == null)
                {
                    bestSubmissions.submittedUser = new ApplicationUser();
                }
                if(userId != null && userId != "")
                {
                    bestSubmissions.submissions = GetBestSubmissions(bestSubmissions.submittedUser, bestSubmissions.milestones);
                }
                else
                {
                    bestSubmissions.submissions = new List<Submission>();
                }
            }
            else
            {
                bestSubmissions.submissions = new List<Submission>();
                bestSubmissions.submittedUser = new ApplicationUser();
                bestSubmissions.users = new List<ApplicationUser>();
            }
            return bestSubmissions;
        }

        public bool CreateAssignment(Assignment newAssignment)
        {
            if(newAssignment != null)
            {
                db.Assignments.Add(newAssignment);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CreateMilestone(Milestone milestone, HttpPostedFileBase upload)
        {   //Only zip uploads are accepted
            if (upload != null)
            {
                if (milestone != null && upload.FileName.EndsWith(".zip"))
                {
                    // Create milestone before test cases so you have an ID for the test cases
                    db.Milestones.Add(milestone);
                    db.SaveChanges();

                    //Creates test cases and calls multiple functions
                    CreateTests(milestone.ID, upload);

                    return true;
                }
                else
                {
                    return false;

                }
            }
            else
            {
                return false;
            }
        }

        public bool EditAssignment(Assignment assignmentToEdit)
        {
            Assignment assignment = GetAssignmentByID(assignmentToEdit.ID);
            if (assignment != null)
            {
                updateAssignment(assignmentToEdit);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditMilestone(Milestone milestone, HttpPostedFileBase upload)
        {
            if(milestone != null)
            {
                updateMilestone(milestone);
                if(upload != null)
                {

                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //soft deletes an assignment
        public bool RemoveAssignment(int assignmentID)
        {
            Assignment assignment = GetAssignmentByID(assignmentID);
            if (assignment != null)
            {
                Assignment toRemove = assignment;
                toRemove.isDeleted = true;
                toRemove.courseID = 0;
                updateAssignment(toRemove);
                return true;
            }
            else
            {
                return false;
            }
        }

        //gets a soft deleted assignment and updates it
        public bool RecoverAssignment(int courseID, int assignmentID)
        {
            Assignment assignment = GetAssignmentByID(assignmentID);
            if(assignment != null)
            {
                Assignment toRecover = assignment;
                toRecover.isDeleted = true;
                toRecover.courseID = courseID;
                updateAssignment(toRecover);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CreateTests(int milestoneID, HttpPostedFileBase upload)
        {
            // Save the zip file to the server
            SaveZip(milestoneID, ref upload);
            // Unpack the zip file on the server
            SaveUnpackedZip(milestoneID, ref upload);
            // List every unpacked test case
            List<TestCase> testCases = GenerateTestCases(milestoneID);
            // Create test cases
            foreach (TestCase test in testCases)
            {
                CreateTestCase(test);
            }
        }

        // The function assumes that upload is a zip file
        private bool SaveZip(int mileID, ref HttpPostedFileBase upload)
        {
            // Saving zip file to server
            // Get zipDirectory
            string zipDir = ConfigurationManager.AppSettings["zippedTestCases"];

            //Use a helper function that finds your subfolder
            zipDir = GetMilestonePath(zipDir, mileID);
            if (!Directory.Exists(zipDir))
                Directory.CreateDirectory(zipDir);

            // Save upload, will be saved in same format as it was uploaded
            upload.SaveAs(zipDir + upload.FileName);
            return true;
        }

        // The function assumes that upload is a zip file
        // finds the correct filepath and saves upload zip contents there
        private bool SaveUnpackedZip(int mileID, ref HttpPostedFileBase upload)
        {
            // Saving zip file to server
            // Get test case directories
            string saveDir = ConfigurationManager.AppSettings["TestCases"];
            string zipDir = ConfigurationManager.AppSettings["ZippedTestCases"];
            // Use a helper function that finds your subfolder
            saveDir = GetMilestonePath(saveDir, mileID);
            zipDir = GetMilestonePath(zipDir, mileID);
            // Zip file doesn't exist
            if (!Directory.Exists(zipDir))
                return false;

            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            // Call the unpacker which saves the file
            return unpackZip(zipDir + upload.FileName, saveDir);

        }

        private List<TestCase> GenerateTestCases(int mileID)
        {
            // Load path to relevant test case directory
            string testCaseDir = ConfigurationManager.AppSettings["testCases"];
            testCaseDir = GetMilestonePath(testCaseDir, mileID);

            // Find all relevant test cases
            string[] directories = Directory.GetDirectories(testCaseDir);

            // List of function results
            List<TestCase> testCases = new List<TestCase>();
            foreach (string dir in directories)
            {   // Get test case information
                TestCase newCase = new TestCase();
                newCase.inputUrl = dir + "\\input.txt";
                newCase.outputUrl = dir + "\\output.txt";
                newCase.milestoneID = mileID;
                testCases.Add(newCase);
            }
            return testCases;
        }

        private bool CreateTestCase(TestCase testcase)
        {
            if (testcase != null)
            {
                db.Testcases.Add(testcase);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        //updates an assignment in the database, updatedAssignment contains information about the update
        private void updateAssignment(Assignment updatedAssignment)
        {
            //finds the previous assignment so it exists in the context
            Assignment prev = (from a in db.Assignments
                               where a.ID == updatedAssignment.ID
                               select a).FirstOrDefault();

            //updates the assignment
            db.Entry(prev).CurrentValues.SetValues(updatedAssignment);
            db.SaveChanges();
        }

        //updates a milestone in the database, updatedMilestone contains information about the update
        private void updateMilestone(Milestone updatedMilestone)
        {
            //finds the previous milestone so it exists in the context
            Milestone prev = (from m in db.Milestones
                              where m.ID == updatedMilestone.ID
                              select m).FirstOrDefault();

            //updates the milestone
            db.Entry(prev).CurrentValues.SetValues(updatedMilestone);
            db.SaveChanges();
        }

        private List<Assignment> GetDeletedAssignments(string teacherID)
        {
            List<Assignment> deletedAssignments = (from a in db.Assignments
                                                   where a.teacherID == teacherID && a.courseID == 0
                                                   select a).ToList();
            return deletedAssignments;
        }

        private List<ApplicationUser> GetUsersInCourse(int courseId)
        {
            List<ApplicationUser> users = (from u in db.UsersInCourses
                                                  where u.courseID == courseId
                                                  select u.user).ToList();
            return users;
        }
        private List<Submission> GetSubmissionsForAssignment(int assignmentId)
        {
            List<Milestone> milestones = (from m in db.Milestones
                                          where m.assignmentID == assignmentId
                                          select m).ToList();
            List<Submission> best = new List<Submission>();
            foreach(Milestone m in milestones)
            {
                
            }
            return null;

        }

        private List<Submission> GetBestSubmissions(ApplicationUser user, List<Milestone> milestones)
        {
            List<Submission> bestSubmissions = new List<Submission>();
            foreach(Milestone milestone in milestones)
            {
                // Get current users submissions
                List<Submission> usersSubmissions = GetSubmissions(user.Id, milestone.ID);
                // Rate submissions and find highest one

                int highScore = 0;
                Submission highestScoring = null;
                Submission newestCompError = null;
                foreach (Submission submission in usersSubmissions)
                {
                    // Check for accepted
                    if (submission.status == result.Accepted)
                    {
                        bestSubmissions.Add(submission);
                        break;
                    }
                    // Count wrong answers and runErrors
                    if (submission.passCount > highScore)
                    {
                        // filter highestScore
                        highScore = submission.passCount;
                        highestScoring = submission;
                    }
                    // Can only happen once
                    else if (newestCompError != null && submission.status == result.compError)
                    {
                        // Catch first compError
                        newestCompError = submission;
                    }
                }
                if (highScore > 0)
                {
                    bestSubmissions.Add(highestScoring);
                }
                else if (newestCompError != null)
                {
                    bestSubmissions.Add(newestCompError);
                }
                else
                {
                    // Can only happen user has no submissions
                    // Placeholder for missing submissions
                    Submission tempSub = new Submission();
                    tempSub.status = result.none;
                    bestSubmissions.Add(tempSub);
                }

            }
            return bestSubmissions;
        }
    }
}