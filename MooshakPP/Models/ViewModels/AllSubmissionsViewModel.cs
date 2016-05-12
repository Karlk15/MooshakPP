using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class AllSubmissionsViewModel
    {
        public List<ApplicationUser> users { get; set; }
        public List<Submission> submissions { get; set; }

        public Milestone currentMilestone { get; set; }
        public ApplicationUser submittedUser { get; set; }
    }
}