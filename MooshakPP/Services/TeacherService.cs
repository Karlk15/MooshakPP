using MooshakPP.Models;
using MooshakPP.Models.Entities;
using MooshakPP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Services
{
    public class TeacherService
    {
        private ApplicationDbContext db;

        /// <summary>
        /// NEEDS TO BE MOVED.
        /// This function should be a protected function in "StudentService" which this class inherites from.
        /// So basically a temporary function for testing.
        /// </summary>
        protected List<Assignment> GetAllAssignments(int courseID)
        {

            return null;
        }

        public TeacherService()
        {
            db = new ApplicationDbContext();
        }

        public CreateAssignmentViewModel AddAssignment(int courseID)
        {
            CreateAssignmentViewModel allAssignments = new CreateAssignmentViewModel();

            allAssignments.assignments = new List<Assignment>(GetAllAssignments(courseID));

            return allAssignments;
        }
        

    }
}