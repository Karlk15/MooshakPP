using MooshakPP.Models.Entities;//   ÞARF AÐ TAKA ÚT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"!#$%&/(&%$#"!"#$%&/(&%$#"!#$%&/()&%$#"!#$%&/()/&%$#"
using MooshakPP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using MooshakPP.Models.ViewModels;
using Microsoft.AspNet.Identity;
// move following to service when service is ready
using System.Configuration;
using System.IO;


namespace MooshakPP.Controllers
{
    [Authorize(Roles = "student")]
    public class StudentController : BaseController
    {
        private StudentService service = new StudentService();

        // GET: Student
        [HttpGet]
        public ActionResult Index(int? courseID, int? assignmentID, int? milestoneID)
        {
            IndexViewModel model = new IndexViewModel();

            if (courseID == null)
            {
                courseID = service.GetFirstCourse(User.Identity.GetUserId());
            }

            if(assignmentID == null)
            {
                assignmentID = service.GetFirstAssignment((int)courseID);
            }

            if (milestoneID == null)
            {
                milestoneID = service.GetFirstMilestone((int)assignmentID);
            }

            model = service.Index(User.Identity.GetUserId(), (int)courseID, (int)assignmentID/*, (int)milestoneID*/);

            Course usingThisCourse = service.GetCourse((int)courseID);

            ViewBag.selectedCourseName = usingThisCourse.name;

            return View(model);
        }


        [HttpPost]
        public ActionResult Submit(FormCollection collection)
        {
            //placeholder code
            var code = "#include <iostream>\n" +
                    "using namespace std;\n" +
                    "int main()\n" +
                    "{\n" +
                    "cout << \"Hello world\" << endl;\n" +
                    "cout << \"The output should contain two lines\" << endl;\n" +
                    "return 0;\n" +
                    "}";

            // Set up our working folder, and the file names/paths.
            // In this example, this is all hardcoded, but in a
            string submissionDir = ConfigurationManager.AppSettings["SubmissionDir"];
            //Make relative path absolute
            submissionDir = HttpContext.Server.MapPath(submissionDir);
            //Add working directory
            submissionDir += "\\Forritun\\" + "\\Assignment 1\\" + "\\Milestone 1\\";
            string userSubmission = submissionDir + User.Identity.GetUserName() + "\\Submission ";

            int i = 1;
            while (Directory.Exists(userSubmission + i))
            {
                i++;
            }
            // the "\\" is vital
            userSubmission += i + "\\";

            

            Directory.CreateDirectory(userSubmission);

            var workingFolder = userSubmission;
            var cppFileName = "Hello.cpp";
            var exeFilePath = workingFolder + "Hello.exe";

            // Write the code to a file, such that the compiler
            // can find it:
            System.IO.File.WriteAllText(workingFolder + cppFileName, code);

            // In this case, we use the C++ compiler (cl.exe) which ships
            // with Visual Studio. It is located in this folder:
            var compilerFolder = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\bin\\";


            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = workingFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;

            compiler.Start();
            compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
            compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + cppFileName);
            compiler.StandardInput.WriteLine("exit");
            string output = compiler.StandardOutput.ReadToEnd();
            compiler.WaitForExit();
            compiler.Close();
            return RedirectToAction("Index");
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