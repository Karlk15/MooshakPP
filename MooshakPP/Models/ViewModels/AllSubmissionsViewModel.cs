using MooshakPP.Models.Entities;
using System.Collections.Generic;

namespace MooshakPP.Models.ViewModels
{
    public class AllSubmissionsViewModel
    {
        public List<ApplicationUser> users { get; set; }
        public List<Submission> submissions { get; set; }
        public List<Milestone> milestones { get; set; }
        public Assignment currentAssignment { get; set; }
        public ApplicationUser submittedUser { get; set; }
        public List<string> downloadPath { get; set; }
    }
}