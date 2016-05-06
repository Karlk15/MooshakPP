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
        public ActionResult ManageCourse(Course newCourse, int ID, string action)
        {
            if(action =="delete")
            {
                //delete course matching ID
                return RedirectToAction("ManageCourse");
            }
            ManageCourseViewModel model = service.ManageCourse();
            if (ModelState.IsValid)
            {
                service.ManageCourse(newCourse);
                return RedirectToAction("ManageCourse", model);
            }
            return View(model);
        }

        

        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(FormCollection collection)
        {
            for(int i = 0; i < 10; i++)
            {
                var result = Request.Form["newUser.email"][i];          
            }
            return View();
        }

        //ID is the course.ID
        [HttpGet]
        public ActionResult ConnectUser(int? ID)
        {
            AddConnectionsViewModel model = service.AddConnections();
            return View(model);
        }

        //ID is the course.ID
        //users is a string array of users you are performing an action on
        //action specifies whether you are adding or removing students, defined by which button you pressed
        [HttpPost]
        public ActionResult ConnectUser(int? ID, int[] users, string action)
        {
            return RedirectToAction("ConnectUser");
        }

        [HttpPost]
        public ActionResult DeleteCourse(int? ID)
        {
            //TODO delete course with course.ID == ID
            return RedirectToAction("ManageCourses");
        }
    }
}