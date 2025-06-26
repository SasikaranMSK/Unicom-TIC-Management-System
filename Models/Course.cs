using System;
using System.Collections.Generic;

namespace UnicomTICManagementSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxStudents { get; set; }
        public bool IsActive { get; set; }
        public List<Student> EnrolledStudents { get; set; } = new List<Student>();

        public int EnrolledCount => EnrolledStudents.Count;
        public bool HasAvailableSpots => EnrolledCount < MaxStudents;
    }
}