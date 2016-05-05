using MooshakPP.Models.Entities;
using MooshakPP.Models.ViewModels;
using MooshakPP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakPP.Controllers
{
    public class AdminController : BaseController
    {
        private AdminService service = new AdminService();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageCourse(int? ID)
        {
            ManageCourseViewModel model = service.ManageCourse();
            ViewBag.selectedCourse = ID;
            return View(model);
        }

        [HttpPost]
        public ActionResult ManageCourse(Course newCourse)
        {
            ManageCourseViewModel model = service.ManageCourse();
            if (ModelState.IsValid)
            {
                service.ManageCourse(newCourse.name);
                return RedirectToAction("ManageCourse", model);
            }
            return View(model);
        }

        

        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(FormCollection collection)
        {
            User newUser = new User();
            List<User> newUsers = new List<User>();
            for(int i = 0; i < collection.Count; i++)
            {
                newUser.email = collection["username"];
                newUsers.Add(newUser);
            }
            service.CreateUsers(newUsers);
            return View();
        }

        /** Connects users to courses
         *  ID is course ID
         **/
        [HttpGet]
        public ActionResult ConnectUser(int? ID)
        {
            if(ID == null)
            {   //Connect requires course ID, if none is provided in the url, redirect to ManageCourse
                return RedirectToAction("ManageCourse");
            }
            AddConnectionsViewModel model = service.AddConnections();
            return View(model);
        }

        [HttpPost]
        public ActionResult ConnectUser(FormCollection collection)
        {
            return View();
        }
    }
}