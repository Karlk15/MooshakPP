using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class AllSubmissionsViewModel
    {
        List<User> users { get; set; }
        List<Submission> submissions { get; set; }
        IndexViewModel viewModels;
    }
}