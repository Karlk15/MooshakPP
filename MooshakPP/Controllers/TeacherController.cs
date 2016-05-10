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

            CreateAssignmentViewModel model = service.AddAssignment(User.Identity.GetUserId(), (int)courseID, assignmentID);
            return View(model);
        
        }

        [HttpPost]
        public ActionResult Create(CreateAssignmentViewModel collection, int? courseID,  int? assignmentID, string action)
        {
            Assignment model = new Assignment();

            if (ModelState.IsValid)
            {

                if (action == "delete")
                {
                    if (assignmentID != null)
                    {
                        //service.RemoveAssignment((int)assignmentID);
                    }

                    return RedirectToAction("Create");

                }
                else if (action == "Create")
                {
                    model.courseID = (int)courseID;

                    model.title = collection.newAssignment.title;

                    //adding a default time to the due date of the assignment and parsing the right format to avoid errors
                    string tempDueDate = collection.due;
                    tempDueDate = tempDueDate + " 23:59:59";
                    model.dueDate = DateTime.ParseExact(tempDueDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    //adding the new assignment to the database through the TeacherService
                    service.CreateAssignment(model);

                    
                    return RedirectToAction("Create", new { courseid = (int)courseID, assignmentid = model.ID});
                }

                
            }

            return View("Error");
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
        public ActionResult AddMilestones(int? assignmentID, int? currMilestoneID)
        {
            if (assignmentID == null)
                return RedirectToAction("Create");

            CreateMilestoneViewModel model = service.AddMilestone((int)assignmentID, currMilestoneID);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMilestones(CreateMilestoneViewModel model, int? assignmentID)
        {


            //collection.currentAssignment



            return RedirectToAction("AddMilestones");
        }
    }
}