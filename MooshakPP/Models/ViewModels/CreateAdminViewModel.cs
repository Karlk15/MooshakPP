using System.Collections.Generic;

namespace MooshakPP.Models.ViewModels
{
    public class CreateAdminViewModel
    {
        public List<ApplicationUser> allAdmins { get; set; }
        public ApplicationUser currentlySelected { get; set; }
        public ApplicationUser newAdmin { get; set; }
    }
}