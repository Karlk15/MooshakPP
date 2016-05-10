using Microsoft.AspNet.Identity;
using MooshakPP.Models;
using MooshakPP.Models.Entities;
using MooshakPP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public bool CreateMilestones(Milestone milestone)
        {
            if(milestone != null)
            {
                db.Milestones.Add(milestone);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
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