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
            if(assignmentID != null)
                allAssignments.currentAssignment = GetAssignmentByID((int)assignmentID);

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
                model.currentMilestone = (from Milestone m in model.milestones
                                          where m.ID == currMilestoneId
                                          select m).FirstOrDefault();
            }
            model.currentAssignment = GetAssignmentByID(assId);
            
            return model;
        }

        public bool CreateMilestone(Milestone milestone, HttpPostedFileBase upload)
        {   //Only zip uploads are accepted
            if (milestone != null && upload.FileName.EndsWith(".zip"))
            {
                SaveZip(milestone, ref upload);
                SaveUnpackedZip(milestone, ref upload);
                db.Milestones.Add(milestone);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // The function assumes that upload is a zip file
        public bool SaveZip(Milestone milestone, ref HttpPostedFileBase upload)
        {
            // Saving zip file to server
            // Get zipDirectory
            string zipDir = ConfigurationManager.AppSettings["zippedTestCases"];

            //Use a helper function that finds your subfolder
            zipDir = GetTestCasePath(zipDir, milestone);
            if (!Directory.Exists(zipDir))
                Directory.CreateDirectory(zipDir);

            // Save upload, will be saved in same format as it was uploaded
            upload.SaveAs(zipDir + upload.FileName);
            return true;
        }

        // The function assumes that upload is a zip file
        // finds the correct filepath and saves upload zip contents there
        public bool SaveUnpackedZip(Milestone milestone, ref HttpPostedFileBase upload)
        {
            // Saving zip file to server
            // Get zipDirectory
            string saveDir = ConfigurationManager.AppSettings["TestCases"];
            string zipDir = ConfigurationManager.AppSettings["ZippedTestCases"];
            // Use a helper function that finds your subfolder
            saveDir = GetTestCasePath(saveDir, milestone);
            zipDir = GetTestCasePath(zipDir, milestone);
            // Zip file doesn't exist
            if (!Directory.Exists(zipDir))
                return false;

            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            // Call the unpacker which saves the file
            return unpackZip(zipDir + upload.FileName, saveDir);
            
        }

        public bool CreateTestCase(TestCase testcase)
        {
            if(testcase != null)
            {
                db.Testcases.Add(testcase);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveAssignment(int assignmentID)
        {
            Assignment assignment = GetAssignmentByID(assignmentID);
            if (assignment != null)
            {
                assignment.isDeleted = true;
                assignment.courseID = 0;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}