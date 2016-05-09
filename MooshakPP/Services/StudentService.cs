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

        public IndexViewModel Index(string userId, int courseId, int? assignmentId/*, int milestoneId*/)
        {
            IndexViewModel newIndex = new IndexViewModel();
            newIndex.courses = GetCourses(userId);
            newIndex.assignments = GetAssignments(courseId);
            if(assignmentId != null)
                newIndex.milestones = GetMilestones((int)assignmentId);
            //newIndex.studentSubmissions = GetSubmissions(userId);
            return newIndex;
        }

        public SubmissionViewModel Submissions(string userId, int milestoneId)
        {
            SubmissionViewModel mySubmissions = new SubmissionViewModel();
            mySubmissions.mySubmissions = GetSubmissions(userId, milestoneId);
            return mySubmissions;
        }

        public DescriptionViewModel Description(int milestoneId)
        {
            DescriptionViewModel description = new DescriptionViewModel();
            description.milestone = GetMilestoneByID(milestoneId);
            return description;
        }

        public DetailsViewModel Details(int submissionId)
        {
            return null;
        } 

        public bool CreateSubmission(Submission studentSubmission)
        {
            return true;
        }

        public int? GetFirstCourse(string userId)
        {
            List<Course> courses = GetCourses(userId);
            if (courses != null)
            {
                return courses[0].ID;
            }
            return null;
        }

        public int? GetFirstAssignment(int courseId)
        {
            List<Assignment> assignments = GetAssignments(courseId);
            if (assignments.Count != 0)
            {
                return assignments[0].ID;
            }
            return null;
        }

        public int? GetFirstMilestone(int? assignmentId)
        {
            if(assignmentId != null)
            {
                List<Milestone> milestones = GetMilestones((int)assignmentId);
                if (milestones.Count != 0)
                {
                    return milestones[0].ID;
                }
            }
            return null;
        }

        public Course GetCourse(int courseID)
        {
            Course theCourse = GetCourseByID(courseID);
            return theCourse;
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