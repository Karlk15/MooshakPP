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
        private StudentService service = new StudentService(null);

        // GET: Student
        [HttpGet]
        public ActionResult Index(int? courseID, int? assignmentID, int? milestoneID)
        {
            IndexViewModel model = new IndexViewModel();

            if (courseID == null)
            {
                courseID = service.GetFirstCourse(User.Identity.GetUserId());
            }

            model = service.Index(User.Identity.GetUserId(), courseID, assignmentID, milestoneID);

            return View(model);
        }


        [HttpPost]
        public ActionResult Index(int? courseID, int? assignmentID, int? milestoneID, string action)
        {
            HttpPostedFileBase file = null;
            //if file submission is valid

            if (milestoneID == null || milestoneID == 0)
            {
                ModelState.AddModelError("", "You need to pick a milestone!");
                IndexViewModel model = new IndexViewModel();
                model = service.Index(User.Identity.GetUserId(), courseID, assignmentID, milestoneID);
                return View(model);
            }

            if (Request.Files.Count >= 0 && Request.Files[0].FileName != "")
            {
                file = Request.Files[0];

                //userID, mileID, HttpPostedFileBase
                //username must be passed because User is tied to http
                service.CreateSubmission(User.Identity.GetUserId(), User.Identity.Name, (int)milestoneID, file);
            }

            return RedirectToAction("Index", new { courseid = courseID, assignmentid = assignmentID, milestoneid = milestoneID });
        }

        [HttpGet]
        public ActionResult ViewDetails()
        {
            // Submission ID placeholder // Username placeholder
            service.GetDetails(70, "kristofer15@ru.is");
            return View();
        }

        [HttpGet]
        public ActionResult ViewSubmission()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewDescription(int milestoneId)
        {
            DescriptionViewModel model = new DescriptionViewModel();
            return View();
        }
    }
}