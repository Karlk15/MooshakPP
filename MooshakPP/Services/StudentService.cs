using MooshakPP.Models.ViewModels;
using MooshakPP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Services
{
    public class StudentService
    {
        public IndexViewModel Index(int userId, int courseId, int assignmentId)
        {
            return null;
        }

        public SubmissionViewModel Submissions(int userId, int courseId, int assignmentId, int milestoneId)
        {
            return null;
        }

        public DescriptionViewModel Description(int userId, int courseId, int assignmentId, int milestoneId)
        {
            return null;
        }

        public DetailsViewModel Details(int userId, int courseId, int assignmentId, int milestoneId, int submissionId)
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
            return null;
        }

        protected List<Assignment> GetAssignments(int courseId)
        {
            return null;
        }

        protected Assignment GetAssignmentByID(int assignmentId)
        {
            return null;
        }

        protected List<Milestone> GetMilestones(int assignmentId)
        {
            return null;
        }

        protected Milestone GetMilestoneByID(int milestoneId)
        {
            return null;
        }

        protected List<Submission> GetSubmissions(int userId, int milestoneId)
        {
            return null;
        }

        protected Submission GetSubmissionByID(int submissionId)
        {
            return null;
        }

    }
}