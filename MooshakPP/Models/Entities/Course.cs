using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.Entities
{
    //[Table(Name = "Course")]
    public class Course 
    {
        public int ID { get; set; }
        public string name { get; set; }
    }
}