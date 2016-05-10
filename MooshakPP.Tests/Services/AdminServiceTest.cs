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
         
        [TestMethod]
        public void TestManageCourseIdOne()
        {
            // Arrange:
            const int courseID = 1;

            // Act:
            var result = service.ManageCourse(courseID);

            // Assert:
            Assert.AreEqual("Gagnaskipan", result.currentCourse.name);

        }
    }
}
