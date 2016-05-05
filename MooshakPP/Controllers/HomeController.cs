﻿using MooshakPP.DAL;
using MooshakPP.Models;
using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakPP.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// This function will use the IdentityManager class to initilize the database with a few test cases.
        /// The acions in this fucntion should only run once because of the "singleton pattern" used.
        /// </summary>
        private static void IdentityInitilizer()
        {
            IdentityManager manager = new IdentityManager();

            if(!manager.RoleExists("admin"))
            {
                manager.CreateRole("admin");
            }

            if (!manager.RoleExists("student"))
            {
                manager.CreateRole("student");
            }

            if (!manager.RoleExists("teacher"))
            {
                manager.CreateRole("teacher");
            }

            if (!manager.UserExists("admin@admin.com"))
            {
                ApplicationUser newAdmin = new ApplicationUser();
                newAdmin.UserName = "admin@admin.com";
                newAdmin.Email = "admin@admin.com";
                manager.CreateUser(newAdmin, "123456");
            }

            if (!manager.UserExists("teacher@teacher.com"))
            {
                ApplicationUser newUser = new ApplicationUser();
                newUser.UserName = "teacher@teacher.com";
                newUser.Email = "teacher@teacher.com";
                manager.CreateUser(newUser, "123456");
            }

            if (!manager.UserExists("kalli@faggot.com"))
            {
                ApplicationUser newUser = new ApplicationUser();
                newUser.UserName = "kalli@faggot.com";
                newUser.Email = "kalli@faggot.com";
                manager.CreateUser(newUser, "123456");
            }

            var admin = manager.GetUser("admin@admin.com");

            if(!manager.UserIsInRole(admin.Id, "admin"))
            {
                manager.AddUserToRole(admin.Id, "admin");
            }

            var teacher = manager.GetUser("teacher@teacher.com");

            if (!manager.UserIsInRole(teacher.Id, "teacher"))
            {
                manager.AddUserToRole(teacher.Id, "teacher");
            }

            var student = manager.GetUser("kalli@faggot.com");

            if (!manager.UserIsInRole(student.Id, "student"))
            {
                manager.AddUserToRole(student.Id, "student");
            }

        }

        public ActionResult Index()
        {
            IdentityInitilizer();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}