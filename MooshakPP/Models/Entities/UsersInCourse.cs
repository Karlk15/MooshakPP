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

        public string userID { get; set; }
    
        public int courseID { get; set; }

        public int RoleID { get; set; }

        [ForeignKey("userID")]
        public virtual ApplicationUser user { get; set; }

        [ForeignKey("courseID")]
        public virtual Course course { get; set; }
    }
}