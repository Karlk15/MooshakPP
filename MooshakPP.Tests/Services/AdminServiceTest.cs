using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooshakPP.Services;
using MooshakPP.Models.Entities;

namespace MooshakPP.Tests.Services
{
    [TestClass]
    public class AdminServiceTest
    {
        private AdminService service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDatabase();

            //initilizing the fake DB for ManageCourse and RemoveCourse testing
            var c1 = new Course
            {
                ID = 1,
                name = "Gagnaskipan",
            };
            mockDb.Courses.Add(c1);

            var c2 = new Course
            {
                ID = 3,
                name = "Forritun",
            };
            mockDb.Courses.Add(c2);

            var c3 = new Course
            {
                ID = 33,
                name = "Vefforitun 2",
            };
            mockDb.Courses.Add(c3);

            service = new AdminService(mockDb);
        }

        #region Tests for ManageCourse

        [TestMethod]
        public void TestManageCourseIdOne()
        {
            // Arrange:
            const int courseID = 1;

            // Act:
            var result = service.ManageCourse(courseID);

            // Assert:
            Assert.AreEqual("Gagnaskipan", result.currentCourse.name);
            Assert.AreEqual(3, result.courses.Count);

        }

        [TestMethod]
        public void TestManageCourseIdNull()
        {
            // Arrange:
            int? courseID = null;

            // Act:
            var result = service.ManageCourse(courseID);

            // Assert:
            Assert.AreEqual(null, result.currentCourse.name);
            Assert.AreEqual(3, result.courses.Count);
        }

        [TestMethod]
        public void TestManageCourseIdNotInDB()
        {
            // Arrange:
            int? courseID = 4;

            // Act:
            var result = service.ManageCourse(courseID);

            // Assert:
            Assert.AreEqual(null, result.currentCourse.name);
            Assert.AreEqual(3, result.courses.Count);
        }

        #endregion

        #region Test for RemoveCourse

        [TestMethod]
        public void RemoveCourseIdNotInDb()
        {
            // Arrange:
            int courseID = 4;

            // Act:
            var result = service.RemoveCourse(courseID);

            // Assert:
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void RemoveCourseIdOne()
        {
            // Arrange:
            int courseID = 1;

            // Act:
            var result = service.RemoveCourse(courseID);

            // Assert:
            Assert.AreEqual(true, result);
        }

        #endregion

        #region Tests for CreateCourse

        [TestMethod]
        public void TestCreateCourseForDummyCourse()
        {
            // Arrange:
            Course dummyCourse = new Course();
            dummyCourse.name = "arbritary";

            // Act:
            var result = service.CreateCourse(dummyCourse);

            // Assert:
            Assert.AreEqual(true, result);

        }

        [TestMethod]
        public void TestCreateCourseForCourseNull()
        {
            // Arrange:
            Course dummyCourse = new Course();

            // Act:
            var result = service.CreateCourse(dummyCourse);

            // Assert:
            Assert.AreEqual(false, result);

        }

        #endregion
    }
}
