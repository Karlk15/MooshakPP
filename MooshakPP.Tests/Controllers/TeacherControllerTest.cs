using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooshakPP.Controllers;
using System.Web.Mvc;
using MooshakPP.Models.ViewModels;

namespace MooshakPP.Tests.Controllers
{
    [TestClass]
    public class TeacherControllerTest
    {
        /// <summary>
        /// The next 3 tests will knowingly fail because of the service funtion being called in the controller
        /// It is enough to comment that out and not take in model in the return View() parameters
        /// That will offer a fair test for the functions
        /// </summary>
        #region Tests Get ActionResult for Create

        [TestMethod]
        public void TestCreateCourseIdNull()
        {
            // Arrange:
            var controller = new TeacherController();
            int? courseID = null;
            int? assID = 2;

            // Act:
            var result = controller.Create(courseID, assID) as ViewResult;

            // Assert:
            Assert.AreEqual("Create", result.ViewName);

        }

        [TestMethod]
        public void TestCreateAssIdNull()
        {
            // Arrange:
            var controller = new TeacherController();
            int? courseID = 1;
            int? assID = null;

            // Act:
            var result = controller.Create(courseID, assID) as ViewResult;

            // Assert:
            Assert.AreEqual("Create", result.ViewName);

        }

        [TestMethod]
        public void TestCreateAssIdAndCourseIdNull()
        {
            // Arrange:
            var controller = new TeacherController();
            int? courseID = null;
            int? assID = null;

            // Act:
            var result = controller.Create(courseID, assID) as ViewResult;

            // Assert:
            Assert.AreEqual("Create", result.ViewName);

        }
        #endregion


        /// <summary>
        /// The next few tests will knowingly fail because of the service funtion being called in the controller
        /// It is enough to comment that out and not take in model in the return View() parameters
        /// That will offer a fair test for the functions
        /// </summary>
        #region Tests Post ActionResult for Create

        [TestMethod]
        public void TestCreateActionDelete1()
        {
            // Arrange:
            var controller = new TeacherController();
            CreateAssignmentViewModel viewModel = new CreateAssignmentViewModel();
            string action = "delete";
            int? courseID = 1;
            int? assID = null;


            // Act:
            var result = (RedirectToRouteResult)controller.Create(viewModel,courseID,assID,action);

            // Assert:
            Assert.AreEqual("Create", result.RouteValues["action"]);

        }

        [TestMethod]
        public void TestCreateActionDelete2()
        {
            // Arrange:
            var controller = new TeacherController();
            CreateAssignmentViewModel viewModel = new CreateAssignmentViewModel();
            string action = "delete";
            int? courseID = 1;
            int? assID = 1;


            // Act:
            var result = (RedirectToRouteResult)controller.Create(viewModel, courseID, assID, action);

            // Assert:
            Assert.AreEqual("Create", result.RouteValues["action"]);

        }

        [TestMethod]
        public void TestCreateActionEditAssIdNull()
        {
            // Arrange:
            var controller = new TeacherController();
            CreateAssignmentViewModel viewModel = new CreateAssignmentViewModel();
            string action = "edit";
            int? courseID = 1;
            int? assID = null;

            // Act:
            var result = controller.Create(viewModel, courseID, assID, action) as ViewResult;

            // Assert:
            Assert.AreEqual("Error", result.ViewName);

        }

        [TestMethod]
        public void TestCreateActionRecover()
        {
            // Arrange:
            var controller = new TeacherController();
            CreateAssignmentViewModel viewModel = new CreateAssignmentViewModel();
            string action = "recover";
            int? courseID = 1;
            int? assID = 1;


            // Act:
            var result = (RedirectToRouteResult)controller.Create(viewModel, courseID, assID, action);

            // Assert:
            Assert.AreEqual("Create", result.RouteValues["action"]);

        }
        #endregion
    }
}
