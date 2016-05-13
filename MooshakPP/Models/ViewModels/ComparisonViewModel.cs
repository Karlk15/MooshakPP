using System.Collections.Generic;

namespace MooshakPP.Models.ViewModels
{
    public class ComparisonViewModel
    {
        public string input { get; set; }
        public List<string> expectedOut { get; set; }
        public List<string> obtainedOut { get; set; }
    }
}