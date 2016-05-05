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
    [Authorize(Roles = "teacher")]
    public class TeacherController : BaseController
    {
        private TeacherService service = new TeacherService();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //int? id <--- vantar sem parameter í Create frá dropdown lista í Index view
        [HttpGet]
        public ActionResult Create()
        {
            //if (id.HasValue)
            //{
            //int courseID = id.Value;
            int courseID = 1;
            CreateAssignmentViewModel model = service.AddAssignment(courseID);
            return View(model);
            //}
            //return View("Error");
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            CreateAssignmentViewModel allAssignments = new CreateAssignmentViewModel();

            if (ModelState.IsValid)
            {
                Assignment model = new Assignment();
                
                string tempCourseID = collection["courseID"];
                model.courseID = Int32.Parse(tempCourseID);

                allAssignments = service.AddAssignment(model.courseID);

                model.title = collection["title"];

                string tempDueDate = collection["dueDate"];
                tempDueDate = tempDueDate + " 23:59:59.42";
                model.dueDate = Convert.ToDateTime(tempDueDate);

                service.CreateAssignment(model);

                RedirectToAction("Create", allAssignments);
            }

            return View(allAssignments);
        }

        [HttpPost]
        public ActionResult Submit(FormCollection collection)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Submissions()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AllSubmissions()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Description()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMilestones(int assignmentID)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMilestone(FormCollection collection)
    {
            return View();
        }
    }
}