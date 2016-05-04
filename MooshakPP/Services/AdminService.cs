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
        public CreateCourseViewModel CreateCourse()
        {
            CreateCourseViewModel allCourses = new CreateCourseViewModel();
            /*foreach(Course c in GetAllCourses())
            {
                allCourses.courses.Add(c);
            }*/
            allCourses.courses = new List<Course>(GetAllCourses());
            return allCourses;
        }

        public bool CreateCourse(string name)
        {
            return true;
        }

        public bool CreateUsers()
        {
            return true;
        }

        public AddConnectionsViewModel AddConnections()
        {
            AddConnectionsViewModel connections = new AddConnectionsViewModel();

            connections.courses = new List<Course>(GetAllCourses());
            connections.connectedUser = new List<User>(GetConnectedUsers(0));
            connections.connectedUser = new List<User>(GetNotConnected(0));

            return connections;
        }

        private List<Course> GetAllCourses()
        {
            return null;
        }

        private List<User> GetConnectedUsers(int courseID)
        {
            return null;
        }

        private List<User> GetNotConnected(int courseID)
        {
            return null;
        }
    }
}