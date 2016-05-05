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
        public ActionResult CreateCourse(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                string name = collection[1].ToString();
                service.CreateCourse(name);
                return RedirectToAction("CreateCourse");
            }
            return View();
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