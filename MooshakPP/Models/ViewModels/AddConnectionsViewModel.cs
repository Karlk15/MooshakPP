using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class AddConnectionsViewModel
    {
        public List<Course> courses{ get; set; }
        public List<ApplicationUser> connectedUser { get; set; }
        public List<ApplicationUser> notConnectedUser { get; set; }
    }
}