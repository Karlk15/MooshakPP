using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class SubmissionViewModel
    {
        public List<Submission> mySubmissions { get; set;}
        public IndexViewModel indexView;
    }
}