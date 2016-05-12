using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooshakPP.Controllers;
using System.Web.Mvc;
using MooshakPP.Models;

namespace MooshakPP.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTest
    {
        /// <summary>
        /// The next 3 tests will knowingly fail because of the service funtion being called in the controller
        /// It is enough to comment that out and not take in model in the return View() parameters
        /// That will offer a fair test for the functions
        /// </summary>
        #region Tests Get ActionResult for Index

        [TestMethod]
        public void TestIndexCourseIdNull()
        {
            // Arrange:
            var controller = new StudentController();
            int? courseID = null;
            int? assignmentID = 1;
            int? mileStoneID = 2;

            // Act:
            var result = controller.Index(courseID, assignmentID, mileStoneID) as ViewResult;

            // Assert:
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestIndexAssIdNull()
        {
            // Arrange:
            var controller = new StudentController();
            int? courseID = 1;
            int? assignmentID = null;
            int? mileStoneID = 2;

            // Act:
            var result = controller.Index(courseID, assignmentID, mileStoneID) as ViewResult;

            // Assert:
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestIndexMileIdNull()
        {
            // Arrange:
            var controller = new StudentController();
            int? courseID = 1;
            int? assignmentID = 2;
            int? mileStoneID = null;

            // Act:
            var result = controller.Index(courseID, assignmentID, mileStoneID) as ViewResult;

            // Assert:
            Assert.AreEqual("Index", result.ViewName);
        }

        #endregion

        #region Tests Post ActionResult for Submit
        [TestMethod]
        public void TestSubmitMilestoneIdTwo()
        {
            // Arrange:
            var controller = new StudentController();
            int? milestoneID = 2;

            // Act:
            //var result = (RedirectToRouteResult)controller.Submit(milestoneID);

            // Assert:
            //Assert.AreEqual("Index", result.RouteValues["action"]);

        }

        [TestMethod]
        public void TestSubmitMilestoneIdNull()
        {
            // Arrange:
            var controller = new StudentController();
            int? milestoneID = null;

            // Act:
            //var result = (RedirectToRouteResult)controller.Submit(milestoneID);

            // Assert:
            //Assert.AreNotEqual("Index", result.RouteValues["action"]);

        }

        [TestMethod]
        public void TestSubmitMilestoneIdNotInDb()
        {
            // Arrange:
            var controller = new StudentController();
            int? milestoneID = 100;

            // Act:
            //var result = (RedirectToRouteResult)controller.Submit(milestoneID);

            // Assert:
            //Assert.AreNotEqual("Index", result.RouteValues["action"]);

        }
        #endregion
    }
}
