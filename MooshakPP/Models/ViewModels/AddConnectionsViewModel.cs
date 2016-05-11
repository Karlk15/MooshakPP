using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class AddConnectionsViewModel
    {
        public List<Course> courses{ get; set; }
        public List<ApplicationUser> connectedTeachers { get; set; }
        public List<ApplicationUser> connectedStudents { get; set; }
        public List<ApplicationUser> notConnectedTeachers { get; set; }
        public List<ApplicationUser> notConnectedStudents { get; set; }

        //currently selected
        public Course currentCourse { get; set; } 
    }
}