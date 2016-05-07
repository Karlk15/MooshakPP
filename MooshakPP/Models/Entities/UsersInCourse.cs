using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.Entities
{
    public class UsersInCourse
    {
        public int ID { get; set; }

        [ForeignKey("AspNetUsers")]
        public int userID { get; set; }
    
        [ForeignKey("Course")]
        public int courseID { get; set; }

        public int RoleID { get; set; }
    }
}