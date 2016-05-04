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
            return null;
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
            return null;
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