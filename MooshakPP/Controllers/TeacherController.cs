using Microsoft.AspNet.Identity;
using MooshakPP.Models.Entities;
using MooshakPP.Models.ViewModels;
using MooshakPP.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ActionResult Index(int? courseID, int? assignmentID, int? milestoneID)
        {
            IndexViewModel model = new IndexViewModel();

            if (courseID == null)
            {
                courseID = service.GetFirstCourse(User.Identity.GetUserId());
            }

            if (assignmentID == null)
            {
                assignmentID = service.GetFirstAssignment((int)courseID);
            }

            if(milestoneID == null)
            {
                milestoneID = service.GetFirstMilestone(assignmentID);
            }

            model = service.Index(User.Identity.GetUserId(), (int)courseID, assignmentID/*, (int)milestoneID*/);

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? courseID, int? assignmentID)
        {

            if(courseID == null)
            {
                courseID = service.GetFirstCourse(User.Identity.GetUserId());
            }
           
            if(assignmentID == null)
            {
                assignmentID = service.GetFirstAssignment((int)courseID);
            }

            CreateAssignmentViewModel model = service.AddAssignment(User.Identity.GetUserId(), (int)courseID, (int)assignmentID);
            return View(model);
        
        }

        [HttpPost]
        public ActionResult Create(Assignment newAssignment, string action)
        {
            CreateAssignmentViewModel allAssignments = new CreateAssignmentViewModel();

            if (ModelState.IsValid)
            {

                if (action == "delete")
                {
                    //TODO
                }
                //Assignment model = new Assignment();

                //string tempCourseID = collection["courseID"];
                //model.courseID = Int32.Parse(tempCourseID);

                //model.title = collection["newAssignment.title"];

                //adding a default time to the due date of the assignment and parsing the right format to avoid errors
                //string tempDueDate = collection["newAssignment.dueDate"];
                string tempDueDate = newAssignment.dueDate + " 23:59:59";
                //tempDueDate = tempDueDate + " 23:59:59";
                //model.dueDate = DateTime.ParseExact(tempDueDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                newAssignment.dueDate = DateTime.ParseExact(tempDueDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //adding the new assignment to the database through the TeacherService
                service.CreateAssignment(newAssignment);

                //getting the new list of assignments with the new assignment added ton the database
                allAssignments = service.AddAssignment(User.Identity.GetUserId(), newAssignment.courseID, newAssignment.ID);

                Course usingThisCourse = service.GetCourse(newAssignment.courseID);

                ViewBag.selectedCourseName = usingThisCourse.name;

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

        [HttpGet]
        public ActionResult AddMilestones(int? assignmentID)
        {
            CreateMilestoneViewModel model = new CreateMilestoneViewModel();
            model = service.AddMilestone((int)assignmentID);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMilestone(FormCollection collection)
    {
            return View();
        }
    }
}