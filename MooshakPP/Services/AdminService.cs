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

        public ManageCourseViewModel ManageCourse()
        {
            ManageCourseViewModel allCourses = new ManageCourseViewModel();
         
            allCourses.courses = new List<Course>(GetAllCourses());

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

        public AddConnectionsViewModel GetConnections(int ID)
        {
            AddConnectionsViewModel connections = new AddConnectionsViewModel();

            connections.courses = new List<Course>(GetAllCourses());
            connections.connectedUser = new List<ApplicationUser>(GetConnectedUsers(ID));
            connections.notConnectedUser = new List<ApplicationUser>(GetNotConnected(ID));


            return connections;
        }

        public void AddConnections(int courseID, List<string> userIDs)
        {
            foreach (string ID in userIDs)
            {
                UsersInCourse entry = new UsersInCourse();
                entry.courseID = courseID;
                entry.userID = ID;
                db.UsersInCourses.Add(entry);
            }
            db.SaveChanges();
        }

        public void RemoveConnections(int courseID, List<string> userIDs)
        {
            foreach (string ID in userIDs)
            {
                //remove ID from courseID in relation table
            }
            //save
        }
        private List<Course> GetAllCourses()
        {
            var courses = (from course in db.Courses
                           select course).ToList();
            return courses;
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
            var allUsers = GetAllUsers();
            var connectedUsers = GetConnectedUsers(courseID);
            var notConnectedUsers = allUsers.Except(connectedUsers).ToList();
            return notConnectedUsers;
        }

        private List<ApplicationUser> GetAllUsers()
        {
            List<ApplicationUser> result = manager.GetAllUsers();
            return result;
        }

    }
}