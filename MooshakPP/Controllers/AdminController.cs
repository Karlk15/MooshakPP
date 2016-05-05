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
    [Authorize(Roles = "admin")]
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
            for(int i = 0; i < 10; i++)
            {
                var result = Request.Form["newUser.email"][i];          
            }
            return View();
        }

        /** Connects users to courses
         *  ID is course ID
            nullable since no course is selected automatically
         **/
        [HttpGet]
        public ActionResult ConnectUser(int? ID)
        {
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