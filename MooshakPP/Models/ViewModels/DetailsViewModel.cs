using MooshakPP.Models.Entities;
using System.Collections.Generic;


namespace MooshakPP.Models.ViewModels
{
    public class DetailsViewModel
    {
        public List<ComparisonViewModel> tests { get; set; }
        public Submission submission { get; set; }
    }
}