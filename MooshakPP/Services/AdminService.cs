using MooshakPP.Models;
using MooshakPP.Models.Entities;
using MooshakPP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Services
{
    public class AdminService
    {
        private ApplicationDbContext db;

        public AdminService()
        {
            db = new ApplicationDbContext();
        }

        public CreateCourseViewModel CreateCourse()
        {
            CreateCourseViewModel allCourses = new CreateCourseViewModel();
         
            allCourses.courses = new List<Course>(GetAllCourses());

            return allCourses;
        }

        public void CreateCourse(string newName)
        {
            Course newCourse = new Course();
            newCourse.name = newName;
            db.Courses.Add(newCourse);
            db.SaveChanges();
        }

        public bool CreateUsers(List<User> newUsers)
        {
            return true;
        }

        public AddConnectionsViewModel AddConnections()
        {
            AddConnectionsViewModel connections = new AddConnectionsViewModel();

            connections.courses = new List<Course>(GetAllCourses());
            connections.connectedUser = new List<User>(GetConnectedUsers(0));
            connections.notConnectedUser = new List<User>(GetNotConnected(0));

            return connections;
        }

        private List<Course> GetAllCourses()
        {
            var courses = (from course in db.Courses
                           select course).ToList();
            return courses;
        }

        private List<User> GetConnectedUsers(int courseID)
        {
            User tu1 = new User();
            User tu2 = new User();
            User tu3 = new User();

            tu1.ID = 1;
            tu2.ID = 2;
            tu3.ID = 3;

            tu1.email = "jon15@ru.is";
            tu2.email = "dickbutt13@ru.is";
            tu3.email = "stalin<3@ru.is";

            tu1.passwordhash = "thisisapassword";
            tu2.passwordhash = "thisisalsoapassword";
            tu3.passwordhash = "thisisnotapassword";

            tu1.securitystamp = "dumdum";
            tu2.securitystamp = "canttuchthis";
            tu3.securitystamp = "racecarisapalindrome";

            List<User> tempUsers = new List<User>();

            tempUsers.Add(tu1);
            tempUsers.Add(tu2);
            tempUsers.Add(tu3);
            return tempUsers;
        }

        private List<User> GetNotConnected(int courseID)
        {
            User tu1 = new User();
            User tu2 = new User();
            User tu3 = new User();

            tu1.ID = 1;
            tu2.ID = 2;
            tu3.ID = 3;

            tu1.email = "NCjon15@ru.is";
            tu2.email = "NCdickbutt13@ru.is";
            tu3.email = "NCstalin<3@ru.is";

            tu1.passwordhash = "thisisapassword";
            tu2.passwordhash = "thisisalsoapassword";
            tu3.passwordhash = "thisisnotapassword";

            tu1.securitystamp = "dumdum";
            tu2.securitystamp = "canttuchthis";
            tu3.securitystamp = "racecarisapalindrome";

            List<User> tempUsers = new List<User>();

            tempUsers.Add(tu1);
            tempUsers.Add(tu2);
            tempUsers.Add(tu3);
            return tempUsers;
        }
    }
}