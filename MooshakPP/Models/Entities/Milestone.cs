using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.Entities
{
    public class Milestone
    {
        public int ID { get; set; }
        public int assignmentID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}