using MooshakPP.Models.Entities;
using System.Collections.Generic;

namespace MooshakPP.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<Course> courses { get; set; }
        public List<Assignment> assignments { get; set; }
        public List<Milestone> milestones { get; set; }
        public List<Submission> submissions { get; set; }
        public Submission newSubmission { get; set; }

        //currently selected
        public Course currentCourse { get; set; }
        public Assignment currentAssignment { get; set; }
        public Milestone currentMilestone { get; set; }

        //partial viewModels 
        public SubmissionViewModel mySubmissions { get; set; }
        public SubmissionViewModel allSubmissions { get; set; }
        public AllSubmissionsViewModel bestSubmissions { get; set; }
    }
}