using Microsoft.AspNet.Identity;
using MooshakPP.DAL;
using MooshakPP.Models;
using System.Web.Mvc;

namespace MooshakPP.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        private static IdentityManager manager = new IdentityManager();

        /// <summary>
        /// This function will use the IdentityManager class to initilize the database with a few test cases.
        /// The acions in this fucntion should only run once because of the "singleton pattern" used.
        /// </summary>
        private static void IdentityInitilizer()
        {

            if(!manager.RoleExists("admin"))
            {
                manager.CreateRole("admin");
            }
            if (!manager.RoleExists("student"))
            {
                manager.CreateRole("student");
            }
            if (!manager.RoleExists("teacher"))
            {
                manager.CreateRole("teacher");
            }
            if (!manager.UserExists("admin@admin.com"))
            {
                ApplicationUser newAdmin = new ApplicationUser();
                newAdmin.UserName = "admin@admin.com";
                newAdmin.Email = "admin@admin.com";
                manager.CreateUser(newAdmin, "Admin-123");
            }
            if (!manager.UserExists("teacher@teacher.com"))
            {
                ApplicationUser newUser = new ApplicationUser();
                newUser.UserName = "teacher@teacher.com";
                newUser.Email = "teacher@teacher.com";
                manager.CreateUser(newUser, "123456");
            }

            var admin = manager.GetUser("admin@admin.com");

            if (!manager.UserIsInRole(admin.Id, "admin"))
            {
                manager.AddUserToRole(admin.Id, "admin");
            }

            var teacher = manager.GetUser("teacher@teacher.com");

            if (!manager.UserIsInRole(teacher.Id, "teacher"))
            {
                manager.AddUserToRole(teacher.Id, "teacher");
            }

        }

        [Authorize]
        public ActionResult Index()
        {
            IdentityInitilizer();

            var userId = User.Identity.GetUserId();

            if (manager.UserIsInRole(userId, "admin"))
            {
                return RedirectToAction("index", "admin");
            }
            else if (manager.UserIsInRole(userId, "teacher"))
            {
                return RedirectToAction("index", "teacher");
            }
            else if (manager.UserIsInRole(userId, "student"))
            {
                return RedirectToAction("index", "student");
            }
            else
            {
                //ToDo throw exception, should not go into this view under normal circumstances
                return View();
            } 
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