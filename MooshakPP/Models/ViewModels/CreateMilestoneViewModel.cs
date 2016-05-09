using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class CreateMilestoneViewModel
    {
        public List<Milestone> milestones { get; set; }
        public Milestone newMilestone { get; set; }
    }
}