using MooshakPP.Models.Entities;//   ÞARF AÐ TAKA ÚT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"!#$%&/(&%$#"!"#$%&/(&%$#"!#$%&/()&%$#"!#$%&/()/&%$#"
using MooshakPP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MooshakPP.Models.ViewModels;

namespace MooshakPP.Controllers
{
    [Authorize(Roles = "student")]
    public class StudentController : BaseController
    {
        private StudentService service = new StudentService();

        // GET: Student
        [HttpGet]
        public ActionResult Index(int? ID)
        {
            IndexViewModel model = new IndexViewModel();
            List<Course> courses = new List<Course>();
            List<Assignment> assignments = new List<Assignment>();
            List<Submission> submissions = new List<Submission>();
            Submission newSub = new Submission();

            Course course1 = new Course();
            course1.ID = 1;
            course1.name = "gagnaskipan";

            Course course2 = new Course();
            course2.ID = 2;
            course2.name = "vefforritun";

            courses.Add(course1);
            courses.Add(course2);

            model.courseAssignments = assignments;
            model.newSubmission = newSub;
            model.studentCourses = courses;
            model.studentSubmissions = submissions;
            
           
            return View(model);
        }

        [HttpPost]
        public ActionResult Submit(FormCollection collection)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewSubmission()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewDescription()
        {
            return View();
        }
    }
}