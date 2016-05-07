using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MooshakPP.DAL;
using MooshakPP.Models;
using MooshakPP.Models.Entities;
using MooshakPP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MooshakPP.Services
{
    public class AdminService
    {
        private ApplicationDbContext db;
        private static IdentityManager manager = new IdentityManager();

        public AdminService()
        {
            db = new ApplicationDbContext();
        }

        //GetCourses is a public function, GetAllCourses is private, both provide a list of all courses
        public ManageCourseViewModel ManageCourse()
        {
            ManageCourseViewModel allCourses = new ManageCourseViewModel();
         
            allCourses.courses = new List<Course>(GetAllCourses());

            return allCourses;
        }

        public void CreateCourse(Course newCourse)
        {
            db.Courses.Add(newCourse);
            db.SaveChanges();
        }

        public void RemoveCourse(int ID)
        {   //could be converted to return bool
            Course course = (from c in GetAllCourses()
                            where c.ID == ID
                            select c).FirstOrDefault();
            if (course != null)
            {
                db.Courses.Remove(course);
                db.SaveChanges();
            }
        }

        public CreateUserViewModel GetUserViewModel()
        {
            CreateUserViewModel newUserView = new CreateUserViewModel();
            newUserView.allUsers = GetAllUsers();
            return newUserView;
        }

        public bool CreateUser(string name, bool isTeacher)
        {

            if (!manager.UserExists(name))
            {
                ApplicationUser nUser = new ApplicationUser();

                nUser.Email = name;
                string password = Membership.GeneratePassword(8, 0);
                nUser.UserName = name;
                manager.CreateUser(nUser, password);

                if(isTeacher == true)
                {
                    var teacher = manager.GetUser(nUser.UserName);
                    if (!manager.UserIsInRole(teacher.Id, "teacher"))
                    {
                        manager.AddUserToRole(teacher.Id, "teacher");
                    }
                }
                else
                {
                    var student = manager.GetUser(nUser.UserName);
                    if (!manager.UserIsInRole(student.Id, "student"))
                    {
                        manager.AddUserToRole(student.Id, "student");
                    }
                }
            }
            return true;
        }

        public AddConnectionsViewModel GetConnections(int ID)
        {
            AddConnectionsViewModel connections = new AddConnectionsViewModel();

            connections.courses = new List<Course>(GetAllCourses());
            connections.connectedUser = new List<User>(GetConnectedUsers(ID));
            connections.notConnectedUser = new List<User>(GetNotConnected(ID));


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

        private List<ApplicationUser> GetAllUsers()
        {
            List<ApplicationUser> result = manager.GetAllUsers();
            return result;
        }

    }
}