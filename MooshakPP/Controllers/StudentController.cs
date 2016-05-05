using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakPP.Controllers
{
    [Authorize(Roles = "student")]
    public class StudentController : BaseController
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit(FormCollection collection)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewSubmission()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewDescription()
        {
            return View();
        }
    }
}