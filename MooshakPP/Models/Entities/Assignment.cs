using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.Entities
{
    public class Assignment
    {
        public int ID { get; set; }
        public int courseID { get; set; }
        [Required(ErrorMessage = "You must specify a title!")]
        public string title { get; set; }
        public DateTime startDate { get; set; }
        public DateTime dueDate { get; set; }
        public bool isDeleted { get; set; }
        public string teacherID { get; set; }
    }
}