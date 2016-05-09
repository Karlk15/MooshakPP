using MooshakPP.Models.Entities;//   ÞARF AÐ TAKA ÚT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"!#$%&/(&%$#"!"#$%&/(&%$#"!#$%&/()&%$#"!#$%&/()/&%$#"
using MooshakPP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MooshakPP.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace MooshakPP.Controllers
{
    [Authorize(Roles = "student")]
    public class StudentController : BaseController
    {
        private StudentService service = new StudentService();

        // GET: Student
        [HttpGet]
        public ActionResult Index(int? courseID, int? assignmentID)
        {
            IndexViewModel model = new IndexViewModel();

            model = service.Index(User.Identity.GetUserId(), (int)courseID, (int)assignmentID);
           
            return View(model);
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