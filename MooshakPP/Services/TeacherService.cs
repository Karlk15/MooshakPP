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

namespace MooshakPP.Services
{
    public class TeacherService : StudentService
    {
        private ApplicationDbContext db;

        public TeacherService()
        {
            db = new ApplicationDbContext();
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
        
        public bool CreateAssignment(Assignment newAssignment)
        {
            try
            {
                db.Assignments.Add(newAssignment);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public CreateMilestoneViewModel AddMilestone(int assId, int? currMilestoneId)
        {
            CreateMilestoneViewModel model = new CreateMilestoneViewModel();
            model.milestones = GetMilestones(assId);
            if(currMilestoneId == null)
            {
                model.currentMilestone = new Milestone();
                model.currentMilestone.assignmentID = 8; 
            }
            else
            {
                model.currentMilestone = GetMilestoneByID((int)currMilestoneId);
            }
            model.currentAssignment = GetAssignmentByID(assId);
            
            return model;
        }

        public bool CreateMilestone(Milestone milestone, HttpPostedFileBase upload)
        {   //Only zip uploads are accepted
            if (milestone != null && upload.FileName.EndsWith(".zip"))
            {
                // Create milestone before test cases so you have an ID for the test cases
                db.Milestones.Add(milestone);
                db.SaveChanges();
                // Save the zip file to the server
                SaveZip(milestone.ID, ref upload);
                // Unpack the zip file on the server
                SaveUnpackedZip(milestone.ID, ref upload);
                // List every unpacked test case
                List<TestCase> testCases = GenerateTestCases(milestone.ID);
                // Create test cases
                foreach (TestCase test in testCases)
                {
                    CreateTestCase(test);
                }

                
                return true;
            }
            else
            {
                return false;
            }
        }

        // The function assumes that upload is a zip file
        public bool SaveZip(int mileID, ref HttpPostedFileBase upload)
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
        public bool SaveUnpackedZip(int mileID, ref HttpPostedFileBase upload)
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

        public List<TestCase> GenerateTestCases(int mileID)
        {
            // Load path to relevant test case directory
            string testCaseDir = ConfigurationManager.AppSettings["testCases"];
            testCaseDir = GetMilestonePath (testCaseDir, mileID);

            // Find all relevant test cases
            string[] directories = Directory.GetDirectories(testCaseDir);

            // List of function results
            List<TestCase> testCases = new List<TestCase>();
            foreach (string dir in directories )
            {   // Get test case information
                TestCase newCase = new TestCase();
                newCase.inputUrl = dir + "\\input.txt";
                newCase.outputUrl = dir + "\\output.txt";
                newCase.milestoneID = mileID;
                testCases.Add(newCase);
            }
            return testCases;
        }

        public bool CreateTestCase(TestCase testcase)
        {
            if (testcase != null)
        {
                db.Testcases.Add(testcase);
                db.SaveChanges();
            return true;
        }

            return false;
        }
        public RecoverAssignmentsViewModel RecoverAssignments(string teacherID, int courseID, int? currentAssignmentID)
        {
            RecoverAssignmentsViewModel recoverAssignments = new RecoverAssignmentsViewModel();
            recoverAssignments.deletedAssignments = GetDeletedAssignments(teacherID);
            recoverAssignments.courses = GetCourses(teacherID);
            recoverAssignments.currentCourse = GetCourseByID(courseID);
            if (currentAssignmentID != null)
                recoverAssignments.currentSelected = GetAssignmentByID((int)currentAssignmentID);
            return recoverAssignments;
        }
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

        public bool EditAssignment(int courseID, int assignmentID, string assignmentName, DateTime startDate, DateTime dueDate)
        {
            Assignment assignment = GetAssignmentByID(assignmentID);
            if(assignment != null)
            {
                Assignment toEdit = assignment;
                toEdit.title = assignmentName;
                toEdit.startDate = startDate;
                toEdit.dueDate = dueDate;
                updateAssignment(toEdit);
                return true;
            }
            else
            {
                return false;
            }
        }

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

        

        private void updateAssignment (Assignment updatedAssignment)
        {
            Assignment prev = (from a in db.Assignments
                               where a.ID == updatedAssignment.ID
                               select a).FirstOrDefault();
            db.Entry(prev).CurrentValues.SetValues(updatedAssignment);
            db.SaveChanges();
        }

        private List<Assignment> GetDeletedAssignments(string teacherID)
        {
            List<Assignment> deletedAssignments = (from a in db.Assignments
                                                   where a.teacherID == teacherID && a.courseID == 0
                                                   select a).ToList();
            return deletedAssignments;
        }
    }
}