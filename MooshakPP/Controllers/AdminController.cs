using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakPP.Controllers
{
    public class AdminController : BaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCourse(FormCollection collection)
        {
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(FormCollection collection)
        {
            return View();
        }

        public ActionResult ConnectUser(int courseID)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConnectUser(FormCollection collection)
        {
            return View();
        }
    }
}