using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class CreateAssignmentViewModel
    {
        public List<Course> courses { get; set; }
        public List<Assignment> assignments { get; set; }
        public Assignment newAssignment { get; set; }
    }
}