using System.Collections.Generic;

namespace MooshakPP.Models.ViewModels
{
    public class CreateUserViewModel
    {
        public List<ApplicationUser> allUsers { get; set; }
        public List<ApplicationUser> newUsers { get; set; }
        public bool[] isTeacher { get; set; }
        public ApplicationUser currentUser { get; set; }
    }
}