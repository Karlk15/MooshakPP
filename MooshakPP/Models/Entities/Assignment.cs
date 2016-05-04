using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.Entities
{
    public class Assignment
    {
        public int ID { get; set; }
        public int courseID { get; set; }
        public string title { get; set; }
        public DateTime dueDate { get; set; }
    }
}