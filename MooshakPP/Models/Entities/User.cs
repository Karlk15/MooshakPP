using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string email { get; set; }
        public string passwordhash { get; set; }
        public string securitystamp { get; set; }
    }
}