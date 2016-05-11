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
        public ActionResult Index(int? courseID, int? assignmentID, int? milestoneID)
        {
            IndexViewModel model = new IndexViewModel();

            if (courseID == null)
            {
                courseID = service.GetFirstCourse(User.Identity.GetUserId());
            }

            if (assignmentID == null && courseID != null)
            {
                assignmentID = service.GetFirstAssignment((int)courseID);
            }

            if (milestoneID == null && assignmentID != null)
            {
                milestoneID = service.GetFirstMilestone((int)assignmentID);
            }

            model = service.Index(User.Identity.GetUserId(), courseID, assignmentID, milestoneID);

            return View(model);
        }


        [HttpPost]
        public ActionResult Submit()
        {
            HttpPostedFileBase file = null;
            //if file submission is valid
            if (Request.Files.Count >= 0 && Request.Files[0].FileName != "")
            {
                file = Request.Files[0];

                // PLACEHOLDER MILESTONE ID
                service.CreateSubmission(User.Identity.GetUserId(), User.Identity.Name, 19, file);
            }

            return RedirectToAction("Index");
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
        public ActionResult ViewDescription(int milestoneId)
        {
            DescriptionViewModel model = new DescriptionViewModel();
            return View();
        }
    }
}