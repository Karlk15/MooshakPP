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

        public CreateAssignmentViewModel AddAssignment(string userId, int courseId)
        {
            CreateAssignmentViewModel allAssignments = new CreateAssignmentViewModel();
            allAssignments.courses = GetCourses(userId);
            allAssignments.assignments = new List<Assignment>(base.GetAssignments(courseId));

            //created a single assignment for the Post request in the Teacher controller
            //so now we have a courseID for our new assignment
            allAssignments.newAssignment = new Assignment();
            allAssignments.newAssignment.courseID = courseId;

            return allAssignments;
        }
        
        public void CreateAssignment(Assignment newAssignment)
        {
            db.Assignments.Add(newAssignment);
            db.SaveChanges();
        }

        public CreateMilestoneViewModel AddMilestone(int assId)
        {
            CreateMilestoneViewModel model = new CreateMilestoneViewModel();
            model.milestones = GetMilestones(assId);
            return model;
        }

        public bool CreateMilestones(List<Milestone> milestones)
        {
            return true;
        }

        /// <summary>
        /// The "new" in this function is to get rid of the "hide inherited" warning
        /// </summary>
        

        

    }
}