using MooshakPP.Models.Entities;
using System.Collections.Generic;

namespace MooshakPP.Models.ViewModels
{
    public class CreateAssignmentViewModel
    {
        public List<Course> courses { get; set; }
        public List<Assignment> assignments { get; set; }
        //public Assignment newAssignment { get; set; }

        //currently used
        public Course currentCourse { get; set; }
        public Assignment currentAssignment { get; set; }

        //temporary string for duedate and startDate
        public string due { get; set; }
        public string start { get; set; }
    }
}