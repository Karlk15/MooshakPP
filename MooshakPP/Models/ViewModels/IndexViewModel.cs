using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<Course> studentCourses { get; set; }
        public List<Assignment> courseAssignments { get; set; }
        public List<Submission> studentSubmissions { get; set; }
        public Submission newSubmission { get; set; }
    }
}