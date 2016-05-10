using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MooshakPP.Models.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;

namespace MooshakPP.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public interface IAppDataContext
    {
        IDbSet<Assignment> Assignments { get; set; }
        IDbSet<Course> Courses { get; set; }
        IDbSet<Milestone> Milestones { get; set; }
        IDbSet<Submission> Submissions { get; set; }
        IDbSet<TestCase> Testcases { get; set; }
        IDbSet<UsersInCourse> UsersInCourses { get; set; }

        int SaveChanges();
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IAppDataContext
    {
        /*public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<TestCase> Testcases { get; set; }
        public DbSet<UsersInCourse> UsersInCourses { get; set; }*/

        public IDbSet<Assignment> Assignments { get; set; }
        public IDbSet<Course> Courses { get; set; }
        public IDbSet<Milestone> Milestones { get; set; }
        public IDbSet<Submission> Submissions { get; set; }
        public IDbSet<TestCase> Testcases { get; set; }
        public IDbSet<UsersInCourse> UsersInCourses { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}