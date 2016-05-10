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
                model.currentMilestone = (from Milestone m in model.milestones
                                          where m.ID == currMilestoneId
                                          select m).FirstOrDefault();
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
            if (currentAssignmentID != null)
                recoverAssignments.currentSelected = GetAssignmentByID((int)currentAssignmentID);
            return recoverAssignments;
        }

        public bool CreateMilestones(List<Milestone> milestones)
        {
            return true;
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