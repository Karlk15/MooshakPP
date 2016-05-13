using MooshakPP.Models.Entities;
using System.Collections.Generic;

namespace MooshakPP.Models.ViewModels
{
    public class SubmissionViewModel
    {
        public List<Submission> submissions { get; set;}
        public Milestone currentMilestone { get; set; }
        public List<ApplicationUser> submittedUser { get; set; }
        public ApplicationUser loggedInUser { get; set; }
    }
}