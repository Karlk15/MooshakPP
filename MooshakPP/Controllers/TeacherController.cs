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
    public class TeacherController : BaseController
    {
        private TeacherService service = new TeacherService();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //int? id <--- vantar sem parameter í Create
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
            return View();
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