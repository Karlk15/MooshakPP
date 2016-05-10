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

            if (assignmentID == null && courseID != null)
            {
                assignmentID = service.GetFirstAssignment((int)courseID);
            }

            if (milestoneID == null && assignmentID != null)
            {
                milestoneID = service.GetFirstMilestone((int)assignmentID);
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

            CreateAssignmentViewModel model = service.AddAssignment(User.Identity.GetUserId(), (int)courseID, assignmentID);
            if(assignmentID == null)
            {
                Assignment noAssignment = new Assignment();
                noAssignment.title = "";
                noAssignment.ID = 0;
                noAssignment.courseID = (int)courseID;
                model.currentAssignment = noAssignment;
            }
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
                        service.RemoveAssignment((int)assignmentID);
                    }

                    return RedirectToAction("Create");

                }
                else if (action == "create")
                {
                    model.courseID = (int)courseID;

                    model.title = collection.currentAssignment.title;

                    //adding a default time to the due date of the assignment and parsing the right format to avoid errors
                    string tempDueDate = collection.due;
                    tempDueDate = tempDueDate + " 23:59:59";
                    model.dueDate = DateTime.ParseExact(tempDueDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    //adding a default time to the start date of the assignment and parsing the right format to avoid errors
                    string tempStartDate = collection.start;
                    tempStartDate = tempStartDate + " 00:01:00";
                    model.startDate = DateTime.ParseExact(tempStartDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    model.teacherID = User.Identity.GetUserId();

                    //adding the new assignment to the database through the TeacherService
                    service.CreateAssignment(model);

                    
                    return RedirectToAction("Create", new { courseid = courseID, assignmentid = model.ID});
                }
                else if(action == "edit")
                {
                    if(assignmentID != null)
                    {
                        //adding a default time to the start date of the assignment and parsing the right format to avoid errors
                        string tempDueDate = collection.due;
                        tempDueDate = tempDueDate + " 23:59:59";
                        //model.dueDate = DateTime.ParseExact(tempDueDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        model.dueDate = Convert.ToDateTime(tempDueDate);

                        //adding a default time to the start date of the assignment and parsing the right format to avoid errors
                        string tempStartDate = collection.start;
                        tempStartDate = tempStartDate + " 00:01:00";
                        //model.startDate = DateTime.ParseExact(tempStartDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        model.startDate = Convert.ToDateTime(tempStartDate);

                        service.EditAssignment((int)courseID, (int)assignmentID, collection.currentAssignment.title ,model.startDate, model.dueDate);

                        return RedirectToAction("Create", new { courseid = courseID, assignmentid = assignmentID });
                    }
                }
                else if(action == "recover")
                {
                    service.RecoverAssignment((int)courseID, (int)assignmentID);
                    return RedirectToAction("Create", new { courseid = courseID, assignmentid = assignmentID });
                }
            }

            return View("Error");
        }

        [HttpGet]
        public ActionResult RecoverAssignments(int? courseID, int? assignmentID)
        {
            if (courseID == null)
                courseID = service.GetFirstCourse(User.Identity.GetUserId());
            if (assignmentID == null)
                assignmentID = service.GetFirstAssignment(0);
            RecoverAssignmentsViewModel recover = service.RecoverAssignments(User.Identity.GetUserId(),(int)courseID ,assignmentID);
            return View(recover);
        }

        [HttpPost]
        public ActionResult RecoverAssignment(int? courseID, int? assignmentID)
        {
            return RedirectToAction("Create", "Teacher");
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