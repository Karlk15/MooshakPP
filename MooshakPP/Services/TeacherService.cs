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

        /// <summary>
        /// NEEDS TO BE MOVED.
        /// This function should be a protected function in "StudentService" which this class inherites from.
        /// So basically a temporary function for testing.
        /// </summary>
        protected List<Assignment> GetAllAssignments(int courseId)
        {
        
            var assignments = (from a in db.Assignments
                               where a.courseID == courseId
                               select a).ToList();

            return assignments;
        }

        public CreateAssignmentViewModel AddAssignment(int courseId)
        {
            CreateAssignmentViewModel allAssignments = new CreateAssignmentViewModel();

            allAssignments.assignments = new List<Assignment>(GetAllAssignments(courseId));

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

    }
}