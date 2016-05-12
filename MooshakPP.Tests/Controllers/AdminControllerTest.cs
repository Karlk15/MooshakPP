using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooshakPP.Controllers;
using System.Web.Mvc;
using MooshakPP.Models.Entities;
using MooshakPP.Services;
using MooshakPP.Models.ViewModels;
using MooshakPP.Models;

namespace MooshakPP.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTest
    {
        #region Tests Post ActionResult for ManageCourse

        [TestMethod]
        public void TestManageCoursePostActionDelete1()
        {
            // Arrange:
            var controller = new AdminController();
            Course newCourse = new Course();
            string action = "delete";
            int? courseID = null;
            newCourse.name = "arbritary";

            // Act:
            var result = (RedirectToRouteResult)controller.ManageCourse(newCourse, courseID, action);

            // Assert:
            Assert.AreEqual("ManageCourse", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestManageCoursePostActionDelete2()
        {
            // Arrange:
            var controller = new AdminController();
            Course newCourse = new Course();
            string action = "delete";
            int? courseID = 1;
            newCourse.name = "arbritary";

            // Act:
            var result = (RedirectToRouteResult)controller.ManageCourse(newCourse, courseID, action);

            // Assert:
            Assert.AreEqual("ManageCourse", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestManageCoursePostActionCreate()
        {
            // Arrange:
            var controller = new AdminController();
            Course newCourse = new Course();
            string action = "create";
            int? courseID = 3;
            newCourse.name = "arbritary";

            // Act:
            var result = (RedirectToRouteResult)controller.ManageCourse(newCourse, courseID, action);

            // Assert:
            Assert.AreEqual("ManageCourse", result.RouteValues["action"]);
        }

        /// <summary>
        /// This will knowingly fail because of the service funtion being called in the controller
        /// It is enough to comment that out and not take in model in the return View() parameters
        /// That will offer a fair test for the function
        /// </summary>
        [TestMethod]
        public void TestManageCoursePostActionCreateCourseEmpty()
        {
            // Arrange:
            var controller = new AdminController();
            Course newCourse = new Course();
            string action = "create";
            int? courseID = 3;

            // Act:
            //var result = (RedirectToRouteResult)controller.ManageCourse(newCourse, courseID, action);
            var result = controller.ManageCourse(newCourse, courseID, action) as ViewResult;

            // Assert:
            Assert.AreEqual("ManageCourse", result.ViewName);
        }
        #endregion

        #region Tests Post ActionResult for ConnectUser

        [TestMethod]
        public void TestConnectUserCourseIdNull()
        {
            // Arrange:
            var controller = new AdminController();
            string[] userIDs = new string[2];
            userIDs[0] = "1";
            userIDs[1] = "2";
            string action = "arbritary";
            int? courseID = null;

            // Act:
            var result = (RedirectToRouteResult)controller.ConnectUser(courseID, userIDs, action);

            // Assert:
            Assert.AreEqual("ConnectUser", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestConnectUserActionAdd()
        {
            // Arrange:
            var controller = new AdminController();
            string[] userIDs = new string[2];
            userIDs[0] = "1";
            userIDs[1] = "2";
            string action = "add";
            int? courseID = 1;

            // Act:
            var result = (RedirectToRouteResult)controller.ConnectUser(courseID, userIDs, action);

            // Assert:
            Assert.AreEqual("ConnectUser", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestConnectUserUsersNone()
        {
            // Arrange:
            var controller = new AdminController();
            string[] userIDs = new string[2];
            string action = "remove";
            int? courseID = 1;

            // Act:
            var result = (RedirectToRouteResult)controller.ConnectUser(courseID, userIDs, action);

            // Assert:
            Assert.AreEqual("ConnectUser", result.RouteValues["action"]);
        }

        #endregion
    }
}
