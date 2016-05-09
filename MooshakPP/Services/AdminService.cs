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
        private static IdentityManager manager;

        public AdminService()
        {
            db = new ApplicationDbContext();
            manager = new IdentityManager();
        }

        public ManageCourseViewModel ManageCourse(int courseId)
        {
            ManageCourseViewModel allCourses = new ManageCourseViewModel();
         
            allCourses.courses = new List<Course>(GetAllCourses());
            allCourses.currentCourse = GetCourseByID(courseId);

            return allCourses;
        }

        public bool CreateCourse(Course newCourse)
        {
            try
            { 
                db.Courses.Add(newCourse);
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool RemoveCourse(int ID)
        {
            Course course = (from c in GetAllCourses()
                            where c.ID == ID
                            select c).FirstOrDefault();
            if (course != null)
            {
                db.Courses.Remove(course);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // n represents how many new users can be accepted
        public CreateUserViewModel GetUserViewModel(int n)
        {
            CreateUserViewModel newUserView = new CreateUserViewModel();
            newUserView.allUsers = GetAllUsers();
            newUserView.newUsers = new List<ApplicationUser>();

            for (int i = 0; i < n && i >= 0; i++)
            {
                newUserView.newUsers.Add(new ApplicationUser());
            }
            
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
                bool result = manager.CreateUser(nUser, password);

                if (result)
                {
                    if (isTeacher == true)
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


        }

        // hashed IDs are strings
        public void RemoveUser(string ID)
        {
            ApplicationUser user = (from u in GetAllUsers()
                                    where u.Id == ID
                                    select u).FirstOrDefault();
            if(user != null)
                manager.RemoveUser(user);   //FIX ME
        }

        public AddConnectionsViewModel GetConnections(int courseID)
        {
            AddConnectionsViewModel connections = new AddConnectionsViewModel();

            connections.courses = new List<Course>(GetAllCourses());
            connections.connectedUser = new List<ApplicationUser>(GetConnectedUsers(courseID));
            connections.notConnectedUser = new List<ApplicationUser>(GetNotConnected(courseID));
            connections.currentCourse = GetCourseByID(courseID);

            return connections;
        }

        public void AddConnections(int courseID, List<string> userIDs)
        {
            foreach (string ID in userIDs)
            {
                if(!IsConnected(courseID, ID))
                {
                    UsersInCourse entry = new UsersInCourse();
                    entry.courseID = courseID;
                    entry.userID = ID;
                    db.UsersInCourses.Add(entry);
                }
            }
            db.SaveChanges();
        }

        public void RemoveConnections(int courseID, List<string> userIDs)
        {
            foreach (string ID in userIDs)
            {
                UsersInCourse connection = GetConnectionByID(courseID, ID);
                if(connection != null)
                {
                    db.UsersInCourses.Remove(connection);
                    db.SaveChanges();
                }
                
            }
        }

        public Course GetFirstCourse()
        {
            List<Course> allCourses = GetAllCourses();
            return allCourses[0];
        }

        private List<Course> GetAllCourses()
        {
            var courses = (from course in db.Courses
                           select course).ToList();
            return courses;
        }

        private Course GetCourseByID(int courseId)
        {
            Course theCourse = (from c in db.Courses
                                where c.ID == courseId
                                select c).SingleOrDefault();

            return theCourse;
        }

        private List<ApplicationUser> GetConnectedUsers(int courseID)
        {
            var connectedUsers = (from users in db.UsersInCourses
                                  where users.courseID == courseID
                                  select users.user).ToList();
            return connectedUsers;
        }

        private List<ApplicationUser> GetNotConnected(int courseID)
        {
            List<ApplicationUser> allUsers = GetAllUsers();
            List<ApplicationUser> connectedUsers = GetConnectedUsers(courseID);
            List<ApplicationUser> notConnectedUsers = new List<ApplicationUser>();

            foreach (ApplicationUser user in allUsers)
            {
                if(!connectedUsers.Exists(x => x.Email == user.Email) && !manager.UserIsInRole(user.Id, "admin"))
                {
                    notConnectedUsers.Add(user);
                }
            }
            return notConnectedUsers;
        }

        private List<ApplicationUser> GetAllUsers()
        {
            List<ApplicationUser> result = manager.GetAllUsers();
            return result;
        }

        private bool IsConnected(int courseID, string userID)
        {
            var getConnected = (from user in db.UsersInCourses
                                where user.userID == userID && user.courseID == courseID
                                select user).FirstOrDefault();
            if (getConnected != null)
                return true;
            else
                return false;
        }

        private UsersInCourse GetConnectionByID(int courseID, string userID)
        {
            UsersInCourse connection = (from u in db.UsersInCourses
                                    where u.courseID == courseID && u.userID == userID
                                    select u).FirstOrDefault();
            return connection;
        }
    }
}