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
        private AdminService service = new AdminService(null);
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageCourse(int? courseID)
        {
            ManageCourseViewModel model = service.ManageCourse(courseID);
            return View(model);
        }

        //The action variable is passed by the button pressed in the view to determine what action was requested 
        //Currently the delete button is the only one who can pass a value, others use actionlinks
        [HttpPost]
        public ActionResult ManageCourse(Course newCourse, int? courseID, string action)
        {
            
            if (action == "delete")
            {
                if (courseID != null && courseID != 0)
                {
                    service.RemoveCourse((int)courseID); 
                }
                return RedirectToAction("ManageCourse");
            }

            if (!string.IsNullOrEmpty(newCourse.name))
            {
                service.CreateCourse(newCourse);
            }

            else
            {
                ModelState.AddModelError("newCourse.name", "You must enter a title");
                ManageCourseViewModel model = service.ManageCourse(courseID);
                return View(model);
            }

            return RedirectToAction("ManageCourse");
        }

        

        [HttpGet]
        public ActionResult CreateUser(string userID)
        {
            // 10 is how many new users can be entered
            if(userID == null)
            {
                userID = service.GetFirstUser().Id;
            }
            CreateUserViewModel model = service.GetUserViewModel(10, userID);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateUser(CreateUserViewModel collection, string action, string userID)
        {
            bool hasErrors = false;
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
                        if(collection.newUsers[i].Email.IndexOf("@") != -1)
                        {
                            if(collection.isTeacher[i])
                            {
                                service.CreateUser(collection.newUsers[i].Email, "teacher");
                            }
                            else
                            {
                                service.CreateUser(collection.newUsers[i].Email, "student");
                            }

                        }
                        else
                        {
                            hasErrors = true;
                            ModelState.AddModelError("newUsers[i].Email", "Invalid email account for input: " + (i+1));
                            if (userID == null)
                            {
                                userID = service.GetFirstUser().Id;
                            }
                        }
                    }
                }
                if(hasErrors == true)
                { 
                    CreateUserViewModel model = service.GetUserViewModel(10, userID);
                    return View(model);
                }
            }

            return RedirectToAction("CreateUser");
        }

        [HttpGet]
        public ActionResult ConnectUser(int? courseID)
        {
            AddConnectionsViewModel model = service.GetConnections(courseID);
            return View(model);
        }

        //ID is the course.ID
        //users is an int array of users you are performing an action on
        //action specifies whether you are adding or removing students, defined by which button you pressed
        [HttpPost]
        public ActionResult ConnectUser(int? courseID, string[] users, string action)
        {
            if(users== null)
            {
                ModelState.AddModelError("", "Please select a user.");
                AddConnectionsViewModel model = service.GetConnections(courseID);
                return View(model);
            }

            //string[] to List<string>
            List<string> userIDs = users.ToList(); 
            if (action == "add")
            {
                service.AddConnections((int)courseID, userIDs);
            }
            else if (action == "remove")
            {
                service.RemoveConnections((int)courseID, userIDs);
            }
            return RedirectToAction("ConnectUser", new { courseid = courseID });
        }

        [HttpGet]
        public ActionResult CreateAdmin(string adminId)
        {
            CreateAdminViewModel model = service.GetAdmins(adminId);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateAdmin(CreateAdminViewModel model, string action, string userId)
        {

            if (action == "delete")
            {
                if (!string.IsNullOrEmpty(userId))
                    service.RemoveUser(userId);
            }
            else
            {
                if (!string.IsNullOrEmpty(model.newAdmin.Email))
                {
                    if (model.newAdmin.Email.IndexOf("@") != -1)
                    {
                        service.CreateUser(model.newAdmin.Email, "admin");
                    }
                    else
                    {
                        ModelState.AddModelError("newAdmin[i].Email", "Invalid email account: ");
                        CreateAdminViewModel m = service.GetAdmins(userId);
                        return View(m);
                    }
                }
            }
            return RedirectToAction("CreateAdmin");
        }
    }
}