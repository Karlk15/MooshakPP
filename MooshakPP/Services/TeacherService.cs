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
            Assignment a1 = new Assignment
            {
                title = "Lab1",
                courseID = courseId,
                dueDate = new DateTime(2016,5,5)
            };
            Assignment a2 = new Assignment
            {
                title = "Lab2",
                courseID = courseId,
                dueDate = new DateTime(2016, 5, 5)
            };
            List<Assignment> assignments = new List<Assignment>();

            assignments.Add(a1);
            assignments.Add(a2);

            return assignments;
        }

        public CreateAssignmentViewModel AddAssignment(int courseID)
        {
            CreateAssignmentViewModel allAssignments = new CreateAssignmentViewModel();

            allAssignments.assignments = new List<Assignment>(GetAllAssignments(courseID));

            return allAssignments;
        }
        
        public void CreateAssignment(Assignment newAssignment)
        {
            db.Assignments.Add(newAssignment);
            db.SaveChanges();
        }

    }
}