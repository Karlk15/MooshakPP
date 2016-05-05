using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<Course> studentCourses;
        public List<Assignment> courseAssignments;
        public List<Submission> studentSubmissions;
        public Submission newSubmission;
    }
}