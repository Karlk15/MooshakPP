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
        public ActionResult ManageCourse(int? ID)
        {
            ManageCourseViewModel model = service.ManageCourse();
            ViewBag.selectedCourse = ID;
            return View(model);
        }



        //The action variable is passed by the button pressed in the view to determine what action was requested 
        //Currently the delete button is the only one who can pass a value, others use actionlinks
        [HttpPost]
        public ActionResult ManageCourse(Course newCourse, int? ID, string action)
        {
            if (action == "delete")
            {
                if (ID != null)
                {
                    int courseID = Convert.ToInt32(ID);
                    service.RemoveCourse(courseID);
                    return RedirectToAction("ManageCourse");
                }
            }
            ManageCourseViewModel model = service.ManageCourse();
            if (!string.IsNullOrEmpty(newCourse.name))
            {
                service.CreateCourse(newCourse);
                return RedirectToAction("ManageCourse", model);
            }
            return View(model);
        }

        

        [HttpGet]
        public ActionResult CreateUser()
        {
            CreateUserViewModel allUsers = service.GetUserViewModel();
            return View(allUsers);
        }
        /// <summary>
        /// collection[1] seeks 
        /// collection[2] seeks 
        /// </summary>
        [HttpPost]
        public ActionResult CreateUser(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                string temp = collection[1];
                string b = collection[2];
                string[] userName = temp.Split(',');
                string[] isTeacher = b.Split(',');

                for (int i = 0; i < 10; i++)
                {
                    if (userName[i] == "")
                    {

                    }
                    else
                    {
                        if (isTeacher[i] == "true")
                        {
                            service.CreateUser(userName[i], true);
                        }
                        else if (isTeacher[i] == "false")
                        {
                            service.CreateUser(userName[i], false);
                        }
                    }
                }
            }

            CreateUserViewModel newModel = service.GetUserViewModel();
            return RedirectToAction("CreateUser", newModel);
        }

        //ID is the course.ID
        [HttpGet]
        public ActionResult ConnectUser(int? ID)
        {
            if (ID == null)
            {   //ID is made 0 so the connected user list will be empty but not connected and course lists will still be full
                ID = 0;
                ViewData["selectedCourse"] = "No course selected"; //This is not an error message
                ViewData["error"] = TempData["connError"];         //This is an error message, only appears after a POST on course.ID == null
            }

            int courseID = Convert.ToInt32(ID);
            AddConnectionsViewModel model = service.GetConnections(courseID);
            if (ID != null)
            {
                foreach (Course course in model.courses)
                {
                    if (course.ID == ID)
                        ViewData["selectedCourse"] = course.name;
                }
            }
            return View(model);
        }

        //ID is the course.ID
        //users is an int array of users you are performing an action on
        //action specifies whether you are adding or removing students, defined by which button you pressed
        [HttpPost]
        public ActionResult ConnectUser(int? ID, int[] users, string action)
        {   //TODO if ID is null, do nothing but return an error message
            if(ID == null)
            {
                TempData["connError"] = "No course is selected";
                return RedirectToAction("ConnectUser");
            }

            int courseID = Convert.ToInt32(ID); //int? to int
            List<int> userIDs = users.ToList(); //int[] to List<int>
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