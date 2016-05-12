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
        private TeacherService service = new TeacherService(null);

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
        public ActionResult Create(CreateAssignmentViewModel collection, int? courseID, int? assignmentID, string action)
        {
            Assignment model = new Assignment();
            bool hasErrors = false;

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

                if (collection.currentAssignment.title == "" || collection.currentAssignment.title == null)
                {
                    hasErrors = true;
                    ModelState.AddModelError("currentAssignment.title", "You must give a title");
                }

                if (collection.start == "" || collection.start == null)
                {
                    hasErrors = true;
                    ModelState.AddModelError("start", "You must give a start date");
                }

                if (collection.due == "" || collection.due == null)
                {
                    hasErrors = true;
                    ModelState.AddModelError("due", "You must give a due date");
                }

                if (hasErrors == true)
                {
                    if (courseID == null)
                    {
                        courseID = service.GetFirstCourse(User.Identity.GetUserId());
                    }

                    CreateAssignmentViewModel emptyModel = service.AddAssignment(User.Identity.GetUserId(), (int)courseID, assignmentID);
                    
                    Assignment noAssignment = new Assignment();
                    noAssignment.title = "";
                    noAssignment.ID = 0;
                    noAssignment.courseID = (int)courseID;
                    emptyModel.currentAssignment = noAssignment;
                    emptyModel.currentCourse.ID = (int)courseID;
                    emptyModel.currentAssignment.ID = (int)assignmentID;
                    
                    return View(emptyModel);
                }

                model.courseID = (int)courseID;

                model.title = collection.currentAssignment.title;

                //adding a default time to the start date of the assignment and parsing the right format to avoid errors
                string tempStartDate = collection.start;
                tempStartDate = tempStartDate + " 00:01:00";
                model.startDate = DateTime.ParseExact(tempStartDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                //adding a default time to the due date of the assignment and parsing the right format to avoid errors
                string tempDueDate = collection.due;
                tempDueDate = tempDueDate + " 23:59:59";
                model.dueDate = DateTime.ParseExact(tempDueDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                model.teacherID = User.Identity.GetUserId();

                //adding the new assignment to the database through the TeacherService
                service.CreateAssignment(model);

                    
                return RedirectToAction("Create", new { courseid = courseID, assignmentid = model.ID});
            }
            else if(action == "edit")
            {
                if(assignmentID != null)
                {
                    model.courseID = (int)courseID;
                    model.ID = (int)assignmentID;
                    model.title = collection.currentAssignment.title;
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

                    service.EditAssignment(model);

                    return RedirectToAction("Create", new { courseid = courseID, assignmentid = assignmentID });
                }
            }
            else if(action == "recover")
            {
                service.RecoverAssignment((int)courseID, (int)assignmentID);
                return RedirectToAction("Create", new { courseid = courseID, assignmentid = assignmentID });
            }
            return View("Error");

        }

        [HttpGet]
        public ActionResult RecoverAssignment(int? courseID, int? assignmentID)
        {
            if (courseID == null)
                courseID = service.GetFirstCourse(User.Identity.GetUserId());
            
            RecoverAssignmentsViewModel recover = service.RecoverAssignments(User.Identity.GetUserId(),(int)courseID ,assignmentID);
            return View(recover);
        }

        [HttpPost]
        public ActionResult RecoverAssignment(int? courseID, int? assignmentID,string action)
        {
            if (assignmentID != null && assignmentID != 0)
            {
                service.RecoverAssignment((int)courseID, (int)assignmentID);
                return RedirectToAction("Create", new { courseid = courseID, assignmentid = assignmentID });
            }
            else
            {
                ModelState.AddModelError("", "No assignment selected!");
            }
            RecoverAssignmentsViewModel model = service.RecoverAssignments(User.Identity.GetUserId(), (int)courseID, (int)assignmentID);
            return View(model);
            //return RedirectToAction("RecoverAssignment", new { courseid = courseID, assignmentid = assignmentID });
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
        public ActionResult AddMilestones(int? assignmentID, int? milestoneID)
        {
            if (assignmentID == null || assignmentID == 0)
                return RedirectToAction("Create");

            CreateMilestoneViewModel model = service.AddMilestone((int)assignmentID, milestoneID);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMilestones(CreateMilestoneViewModel model, int? assignmentID, int? milestoneID ,string action)
        {
            Milestone newMilestone = new Milestone();

            if (assignmentID == null || assignmentID == 0)
                return RedirectToAction("Create");

            if (ModelState.IsValid)
            {
                   if(action == "create")
                   {
                       newMilestone.assignmentID = (int)assignmentID;
                       newMilestone.name = model.currentMilestone.name;
                       newMilestone.description = model.currentMilestone.description;

                       service.CreateMilestone(newMilestone, model.testCaseZip);
                   }
                   else if(action == "edit")
                   {
                       newMilestone.ID = (int)milestoneID;
                       newMilestone.assignmentID = (int)assignmentID;
                       newMilestone.description = model.currentMilestone.description;
                       newMilestone.name = model.currentMilestone.name;

                       service.EditMilestone(newMilestone, model.testCaseZip);
                   }

                return RedirectToAction("AddMilestones", new { assignmentid = assignmentID, milestoneid = newMilestone.ID });
            }
            return View("Error");
        }
    }
}