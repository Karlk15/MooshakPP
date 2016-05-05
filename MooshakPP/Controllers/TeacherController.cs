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

            //all uncommented code in this ActionResult is temporary
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

                model.title = collection["newAssignment.title"];

                //adding a default time to the due date of the assignment
                string tempDueDate = collection["newAssignment.dueDate"];
                tempDueDate = tempDueDate + " 23:59:59.42";
                model.dueDate = Convert.ToDateTime(tempDueDate);

                //adding the new assignment to the database through the TeacherService
                service.CreateAssignment(model);

                //getting the new list of assignments with the new assignment added ton the database
                allAssignments = service.AddAssignment(model.courseID);

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