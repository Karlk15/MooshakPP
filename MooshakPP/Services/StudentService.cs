using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MooshakPP.Models.ViewModels;
using MooshakPP.Models.Entities;

namespace MooshakPP.Services
{
    public class StudentService
    {
        private Models.ApplicationDbContext db;

        public StudentService()
        {
            db = new Models.ApplicationDbContext();
        }

        public IndexViewModel Index(int userId, int courseId, int assignmentId)
        {
            return null;
        }

        public SubmissionViewModel Submissions(int userId, int milestoneId)
        {
            return null;
        }

        public DescriptionViewModel Description(int milestoneId)
        {
            return null;
        }

        public DetailsViewModel Details(int submissionId)
        {
            return null;
        } 

        public bool CreateSubmission(Submission studentSubmission)
        {
            return true;
        }

        protected List<Course> GetCourses(int userId)
        {
            return null;
        }

        protected Course GetCourseByID(int courseId)
        {
            var theCourse = (from c in db.Courses
                             where c.ID == courseId
                             select c).SingleOrDefault();
            
            return theCourse;
        }

        protected List<Assignment> GetAssignments(int courseId)
        {
            var assignments = (from a in db.Assignments
                               where a.courseID == courseId
                               select a).ToList();

            return assignments;
        }

        protected Assignment GetAssignmentByID(int assignmentId)
        {
            var assignment = (from a in db.Assignments
                              where a.ID == assignmentId
                              select a).FirstOrDefault();
            return assignment;
        }

        protected List<Milestone> GetMilestones(int assignmentId)
        {
            var milestones = (from m in db.Milestones
                              where m.assignmentID == assignmentId
                              select m).ToList();
            return milestones;
        }

        protected Milestone GetMilestoneByID(int milestoneId)
        {
            var milestone = (from m in db.Milestones
                             where m.ID == milestoneId
                             select m).FirstOrDefault();
            return milestone;
        }

        protected List<Submission> GetSubmissions(string userId, int milestoneId)
        {
            var submissions = (from s in db.Submissions
                               where s.userID == userId && s.milestoneID == milestoneId
                               select s).ToList();
            return submissions;
        }

        protected Submission GetSubmissionByID(int submissionId)
        {
            var submission = (from s in db.Submissions
                              where s.ID == submissionId
                              select s).FirstOrDefault();
            return submission;
        }

        
    }
}