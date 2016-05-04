using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.Entities
{
    public class TestCase
    {
        public int ID { get; set; }
        public int milestoneID { get; set; }
        public string inputUrl { get; set; }
        public string outputUrl { get; set; }
    }
}