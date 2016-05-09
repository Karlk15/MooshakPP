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
    [Authorize(Roles = "admin")]
    public class AdminController : BaseController
    {
        private AdminService service = new AdminService();
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageCourse(int? courseID)
        {
            if(courseID == null)
            {
                courseID = service.GetFirstCourse().ID;
            }
            ManageCourseViewModel model = service.ManageCourse((int)courseID);
            return View(model);
        }

        //The action variable is passed by the button pressed in the view to determine what action was requested 
        //Currently the delete button is the only one who can pass a value, others use actionlinks
        [HttpPost]
        public ActionResult ManageCourse(Course newCourse, int? courseID, string action)
        {
            if (action == "delete")
            {
                if (courseID != null)
                {
                    service.RemoveCourse((int)courseID);
                    
                }
                return RedirectToAction("ManageCourse");
            }
            ManageCourseViewModel model = service.ManageCourse((int)courseID);
            if (!string.IsNullOrEmpty(newCourse.name))
            {
                service.CreateCourse(newCourse);
                return RedirectToAction("ManageCourse", model);
            }
            return View(model);
        }

        

        [HttpGet]
        public ActionResult CreateUser(int? ID)
        {
            // 10 is how many new users can be entered
            CreateUserViewModel model = service.GetUserViewModel(10);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateUser(CreateUserViewModel collection, string action, string userID)
        {
            if (action == "delete")
            {
                if (!string.IsNullOrEmpty(userID))
                    service.RemoveUser(userID);
            }
            else if (collection.newUsers.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (!string.IsNullOrEmpty(collection.newUsers[i].Email))
                    {
                        service.CreateUser(collection.newUsers[i].Email, collection.isTeacher[i]);
                    }
                }
            }

            return RedirectToAction("CreateUser");
        }

        [HttpGet]
        public ActionResult ConnectUser(int? courseID)
        {
            if (courseID == null)
            {   
                //ID is made 0 so the connected user list will be empty but not connected and course lists will still be full
                courseID = 0;

                //This is not an error message
                ViewData["selectedCourse"] = "No course selected";

                //This is an error message, only appears after a POST on course.ID == null
                ViewData["error"] = TempData["connError"];         
            }

            AddConnectionsViewModel model = service.GetConnections((int)courseID);
            return View(model);
        }

        //ID is the course.ID
        //users is an int array of users you are performing an action on
        //action specifies whether you are adding or removing students, defined by which button you pressed
        [HttpPost]
        public ActionResult ConnectUser(int? ID, string[] users, string action)
        {
            if(ID == null)
            {
                TempData["connError"] = "No course is selected";
                return RedirectToAction("ConnectUser");
            }

            int courseID = Convert.ToInt32(ID); //int? to int
            List<string> userIDs = users.ToList(); //string[] to List<string>
            if (action == "add")
            {
                service.AddConnections(courseID, userIDs);
            }
            else if (action == "remove")
            {
                service.RemoveConnections(courseID, userIDs);
            }
            return RedirectToAction("ConnectUser");
        }
    }
}