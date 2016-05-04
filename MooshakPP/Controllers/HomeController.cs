using MooshakPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakPP.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// This function will use the IdentityManager class to initilize the database with a few test cases.
        /// The acions in this fucntion should only run once because of the "singleton pattern" used.
        /// </summary>
        private static void DatabaseInitilizer()
        {
            IdentityManager manager = new IdentityManager();

            if(!manager.RoleExists("admin"))
            {
                manager.CreateRole("admin");
            }

            if(!manager.UserExists("admin@admin.com"))
            {
                ApplicationUser newAdmin = new ApplicationUser();
                newAdmin.UserName = "admin@admin.com";
                manager.CreateUser(newAdmin, "123456");
            }

            var user = manager.GetUser("admin@admin.com");

            if(!manager.UserIsInRole(user.Id, "admin"))
            {
                manager.AddUserToRole(user.Id, "admin");
            }

        }

        public ActionResult Index()
        {
            DatabaseInitilizer();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}