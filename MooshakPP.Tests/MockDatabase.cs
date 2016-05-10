using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeDbSet;
using System.Data.Entity;
using MooshakPP.Models;
using MooshakPP.Models.Entities;

namespace MooshakPP.Tests
{
    /// <summary>
    /// This is an example of how we'd create a fake database by implementing the 
    /// same interface that the BookeStoreEntities class implements.
    /// </summary>
    public class MockDatabase : IAppDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDatabase()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            this.Assignments = new InMemoryDbSet<Assignment>();
            this.Courses = new InMemoryDbSet<Course>();
            this.Milestones = new InMemoryDbSet<Milestone>();
            this.Submissions = new InMemoryDbSet<Submission>();
            this.Testcases = new InMemoryDbSet<TestCase>();
            this.UsersInCourses = new InMemoryDbSet<UsersInCourse>();
        }

        public IDbSet<Assignment> Assignments { get; set; }
        public IDbSet<Course> Courses { get; set; }
        public IDbSet<Milestone> Milestones { get; set; }
        public IDbSet<Submission> Submissions { get; set; }
        public IDbSet<TestCase> Testcases { get; set; }
        public IDbSet<UsersInCourse> UsersInCourses { get; set; }

        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;
            //changes += DbSetHelper.IncrementPrimaryKey<Author>(x => x.AuthorId, this.Authors);
            //changes += DbSetHelper.IncrementPrimaryKey<Book>(x => x.BookId, this.Books);

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }
}