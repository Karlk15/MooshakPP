using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class DetailsViewModel
    {
        List<TestCase> testcases { get; set; }
        Submission submission { get; set; }
    }
}