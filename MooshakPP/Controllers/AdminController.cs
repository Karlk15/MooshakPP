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
        public ActionResult CreateCourse()
        {
           CreateCourseViewModel model = service.CreateCourse();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCourse(string input)
        {
            CreateCourseViewModel model = service.CreateCourse();
            if (ModelState.IsValid)
            {
                service.CreateCourse(input);
                return RedirectToAction("CreateCourse", model);
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
            return View();
        }

        [HttpGet]
        public ActionResult ConnectUser()
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