using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class CreateAdminViewModel
    {
        public List<ApplicationUser> allAdmins { get; set; }

        public ApplicationUser currentlySelected { get; set; }
        public ApplicationUser newAdmin { get; set; }
    }
}