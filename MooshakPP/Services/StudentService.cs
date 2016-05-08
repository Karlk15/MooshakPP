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

        public IndexViewModel Index(string userId, int courseId, int assignmentId)
        {
            IndexViewModel newIndex = new IndexViewModel();
            newIndex.studentCourses = GetCourses(userId);
            newIndex.courseAssignments = GetAssignments(courseId);
            //newIndex.studentSubmissions = GetSubmissions(userId);
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

        protected List<Course> GetCourses(string userId)
        {
            List<Course> courses = (from c in db.UsersInCourses
                           where c.userID == userId
                           select c.course).ToList();
            return courses;
        }

        protected Course GetCourseByID(int courseId)
        {
            Course theCourse = (from c in db.Courses
                             where c.ID == courseId
                             select c).SingleOrDefault();
            
            return theCourse;
        }

        protected List<Assignment> GetAssignments(int courseId)
        {
            List<Assignment> assignments = (from a in db.Assignments
                               where a.courseID == courseId
                               select a).ToList();

            return assignments;
        }

        protected Assignment GetAssignmentByID(int assignmentId)
        {
            Assignment assignment = (from a in db.Assignments
                              where a.ID == assignmentId
                              select a).FirstOrDefault();
            return assignment;
        }

        protected List<Milestone> GetMilestones(int assignmentId)
        {
            List<Milestone> milestones = (from m in db.Milestones
                              where m.assignmentID == assignmentId
                              select m).ToList();
            return milestones;
        }

        protected Milestone GetMilestoneByID(int milestoneId)
        {
            Milestone milestone = (from m in db.Milestones
                             where m.ID == milestoneId
                             select m).FirstOrDefault();
            return milestone;
        }

        protected List<Submission> GetSubmissions(string userId, int milestoneId)
        {
            List<Submission> submissions = (from s in db.Submissions
                               where s.userID == userId && s.milestoneID == milestoneId
                               select s).ToList();
            return submissions;
        }

        protected Submission GetSubmissionByID(int submissionId)
        {
            Submission submission = (from s in db.Submissions
                              where s.ID == submissionId
                              select s).FirstOrDefault();
            return submission;
        }

        
    }
}