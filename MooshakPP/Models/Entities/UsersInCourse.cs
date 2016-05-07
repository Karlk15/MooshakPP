using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.Entities
{
    public class UsersInCourse
    {
        public int userID { get; set; }
        public int courseID { get; set; }
    }
}