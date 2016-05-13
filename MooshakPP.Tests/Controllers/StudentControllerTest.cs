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

        /// <summary>
        /// The next 3 tests will knowingly fail because of the service funtion being called in the controller
        /// It is enough to comment that out and not take in model in the return View() parameters
        /// That will offer a fair test for the functions
        /// </summary>
        #region Tests Post ActionResult for Index
        [TestMethod]
        public void TestIndexMilestoneIdTwo()
        {
            // Arrange:
            var controller = new StudentController();
            int? courseID = 1;
            int? milestoneID = 2;
            int? assignmentID = 3;
            string action = "aritrary";

            // Act:
            var result = (RedirectToRouteResult)controller.Index(courseID,assignmentID,milestoneID,action);

            // Assert:
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }

        [TestMethod]
        public void TestIndexMilestoneIdNull()
        {
            // Arrange:
            var controller = new StudentController();
            int? courseID = 1;
            int? milestoneID = null;
            int? assignmentID = 3;
            string action = "aritrary";

            // Act:
            var result = controller.Index(courseID, assignmentID, milestoneID, action) as ViewResult;

            // Assert:
            Assert.AreEqual("Index", result.ViewName);

        }

        [TestMethod]
        public void TestIndexMilestoneIdNotInDb()
        {
            // Arrange:
            var controller = new StudentController();
            int? courseID = 1;
            int? milestoneID = 1000;
            int? assignmentID = 3;
            string action = "aritrary";

            // Act:
            var result = controller.Index(courseID, assignmentID, milestoneID, action) as ViewResult;

            // Assert:
            Assert.AreEqual("Index", result.ViewName);

        }
        #endregion
    }
}
