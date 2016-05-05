using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class DescriptionViewModel
    {
        public Milestone milestone { get; set; }
        public IndexViewModel indexView { get; set; }
    }
}