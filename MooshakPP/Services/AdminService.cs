using MooshakPP.DAL;
using System.Net.Mail;
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
        //private ApplicationDbContext db;
        private static IdentityManager manager;
        private readonly IAppDataContext db;

        public AdminService(IAppDataContext context)
        {
            //db = new ApplicationDbContext();
            db = context ?? new ApplicationDbContext();
             
            manager = new IdentityManager();
        }

        public ManageCourseViewModel ManageCourse(int? courseId)
        {
            ManageCourseViewModel allCourses = new ManageCourseViewModel();
         
            allCourses.courses = new List<Course>(GetAllCourses());
            if(courseId == null)
            {
                allCourses.currentCourse = new Course();
            }
            else
                allCourses.currentCourse = GetCourseByID((int)courseId);

            return allCourses;
        }
        
        // n represents how many new users can be accepted
        public CreateUserViewModel GetUserViewModel(int number, string currentUserId)
        {
            CreateUserViewModel newUserView = new CreateUserViewModel();
            newUserView.allUsers = GetAllExceptAdmin();
            newUserView.newUsers = new List<ApplicationUser>();
            newUserView.currentUser = GetUserByID(currentUserId);
            for (int i = 0; i < number && i >= 0; i++)
            {
                newUserView.newUsers.Add(new ApplicationUser());
            }
            
            return newUserView;
        }
        
        public AddConnectionsViewModel GetConnections(int? courseID)
        {
            AddConnectionsViewModel connections = new AddConnectionsViewModel();

            connections.courses = new List<Course>(GetAllCourses());
            if (courseID == null)
            {
                connections.notConnectedTeachers = new List<ApplicationUser>(GetNotConnectedTeachers(0));
                connections.notConnectedStudents = new List<ApplicationUser>(GetNotConnectedStudents(0));
                connections.connectedTeachers = new List<ApplicationUser>();
                connections.connectedStudents = new List<ApplicationUser>();
                connections.currentCourse = new Course();
                connections.currentCourse.name = "No course selected";
                return connections;
            }
            connections.connectedTeachers = new List<ApplicationUser>(GetConnectedTeachers((int)courseID));
            connections.connectedStudents = new List<ApplicationUser>(GetConnectedStudents((int)courseID));
            connections.notConnectedTeachers = new List<ApplicationUser>(GetNotConnectedTeachers((int)courseID));
            connections.notConnectedStudents = new List<ApplicationUser>(GetNotConnectedStudents((int)courseID));
            connections.currentCourse = GetCourseByID((int)courseID);

            return connections;
        }

        public CreateAdminViewModel GetAdmins(string adminId)
        {
            CreateAdminViewModel model = new CreateAdminViewModel();
            model.allAdmins = GetAllAdmins();
            if(!string.IsNullOrEmpty(adminId))
            {
                model.currentlySelected = GetUserByID(adminId);
            }
            else
            {
                model.currentlySelected = manager.GetUser("admin@admin.com");
            }
            return model;
        }

        public bool CreateCourse(Course newCourse)
        {
            try
            {
                db.Courses.Add(newCourse);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
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

        public bool CreateUser(string name, string role)
        {

            if (!manager.UserExists(name))
            {
                ApplicationUser nUser = new ApplicationUser();

                nUser.Email = name;
                string password = Membership.GeneratePassword(8, 0);
                nUser.UserName = name;
                bool wasCreated = manager.CreateUser(nUser, password);
                if (wasCreated)
                {
                    SendUserEmail(name, password);
                    if (role == "teacher" || role == "student" || role == "admin")
                    {
                        var teacher = manager.GetUser(nUser.UserName);
                        if (!manager.UserIsInRole(teacher.Id, role))
                        {
                            manager.AddUserToRole(teacher.Id, role);
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
        public void RemoveUser(string userID)
        {
            ApplicationUser user = GetUserByID(userID);
            if (user != null)
                RemoveUser(user);   //FIX ME
        }

        public void AddConnections(int courseID, List<string> userIDs)
        {
            foreach (string ID in userIDs)
            {
                if(!IsConnected(courseID, ID) && courseID != 0)
                {
                    UsersInCourse entry = new UsersInCourse();
                    entry.courseID = courseID;
                    entry.userID = ID;
                    db.UsersInCourses.Add(entry);
                    db.SaveChanges();
                }
            }
            
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

        public ApplicationUser GetFirstUser()
        {
            List<ApplicationUser> allUsers = GetAllUsers();
            return allUsers.FirstOrDefault();
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

        private List<ApplicationUser> GetConnectedTeachers(int courseID)
        {
            List<ApplicationUser> connectedUsers = (from users in db.UsersInCourses
                                  where users.courseID == courseID
                                  select users.user).ToList();

            List<ApplicationUser> connectedTeachers = new List<ApplicationUser>();
            foreach(ApplicationUser user in connectedUsers)
            {
                if(manager.UserIsInRole(user.Id, "teacher"))
                {
                    connectedTeachers.Add(user);
                }
            }
            return connectedTeachers;
        }

        private List<ApplicationUser> GetConnectedStudents(int courseID)
        {
            List<ApplicationUser> connectedUsers = (from users in db.UsersInCourses
                                                    where users.courseID == courseID
                                                    select users.user).ToList();

            List<ApplicationUser> connectedStudents = new List<ApplicationUser>();
            foreach (ApplicationUser user in connectedUsers)
            {
                if (manager.UserIsInRole(user.Id, "student"))
                {
                    connectedStudents.Add(user);
                }
            }
            return connectedStudents;
        }

        private List<ApplicationUser> GetNotConnectedTeachers(int courseID)
        {
            List<ApplicationUser> allUsers = GetAllUsers();
            List<ApplicationUser> connectedTeachers = GetConnectedTeachers(courseID);
            List<ApplicationUser> notConnectedTeachers = new List<ApplicationUser>();

            foreach (ApplicationUser user in allUsers)
            {
                if(!connectedTeachers.Exists(x => x.Email == user.Email) && !manager.UserIsInRole(user.Id, "admin"))
                {
                    if(manager.UserIsInRole(user.Id, "teacher"))
                    {
                        notConnectedTeachers.Add(user);
                    }
                }
            }
            return notConnectedTeachers;
        }

        private List<ApplicationUser> GetNotConnectedStudents(int courseID)
        {
            List<ApplicationUser> allUsers = GetAllUsers();
            List<ApplicationUser> connectedStudents = GetConnectedTeachers(courseID);
            List<ApplicationUser> notConnectedStudents = new List<ApplicationUser>();

            foreach (ApplicationUser user in allUsers)
            {
                if (!connectedStudents.Exists(x => x.Email == user.Email) && !manager.UserIsInRole(user.Id, "admin"))
                {
                    if (manager.UserIsInRole(user.Id, "student"))
                    {
                        notConnectedStudents.Add(user);
                    }
                }
            }
            return notConnectedStudents;
        }

        private List<ApplicationUser> GetAllUsers()
        {
            List<ApplicationUser> result = manager.GetAllUsers();
            return result;
        }

        private List<ApplicationUser> GetAllExceptAdmin()
        {
            List<ApplicationUser> ex = manager.GetAllUsers();

            List<ApplicationUser> all = new List<ApplicationUser>();
            foreach(ApplicationUser user in ex)
            {
                if(!manager.UserIsInRole(user.Id, "admin"))
                {
                    all.Add(user);
                }
            }
            return all;
        }

        private ApplicationUser GetUserByID(string userID)
        {
            ApplicationUser user = (from u in GetAllUsers()
                                    where u.Id == userID
                                    select u).FirstOrDefault();
            return user;
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
        
        private bool RemoveUser(ApplicationUser userToRemove)
        {
            List<UsersInCourse> selectedUserCourses = (from con in db.UsersInCourses
                                                       where con.userID == userToRemove.Id
                                                       select con).ToList();
            
            manager.ClearUserRoles(userToRemove.Id);
            if(selectedUserCourses.Count == 0)
            {
                manager.RemoveUser(userToRemove);
                return true;
            }
            else
            {
                for(int i = 0; i < selectedUserCourses.Count(); i++)
                {
                    
                    if(selectedUserCourses[i] != null)
                    {
                        db.UsersInCourses.Remove(selectedUserCourses[i]);
                        db.SaveChanges();
                    }
                    manager.RemoveUser(userToRemove);
                }
                return true;
            }
        }

        private List<ApplicationUser> GetAllAdmins()
        {
            List<ApplicationUser> allUsers = GetAllUsers();

            List<ApplicationUser> allAdmins = new List<ApplicationUser>();
            foreach(ApplicationUser user in allUsers)
            {
                if(manager.UserIsInRole(user.Id, "admin"))
                {
                    allAdmins.Add(user);
                }
            }
            return allAdmins;
        }

        private void SendUserEmail(string userEmail, string userPassword)
        {
            MailMessage mail = new MailMessage("mooshakpp@gmail.com", userEmail);
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = "smtp.gmail.com";
            mail.Subject = "Mooshak++ login credentials";
            mail.Body = "You can login with Mooshak++ using the following login information: " + Environment.NewLine
                        + "UserName: " + userEmail + Environment.NewLine
                        + "Password: " + userPassword + Environment.NewLine
                        + "You can change your password later when logged in. ";
            client.Credentials = new System.Net.NetworkCredential("mooshakpp@gmail.com", "ArnarErBestur123");
            client.EnableSsl = true;
            client.Send(mail);
        }
    }
}