using System;
using System.Collections.Generic;

namespace UnicomTICManagementSystem.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; }
        public List<Course> EnrolledCourses { get; set; } = new List<Course>();

        public string FullName => $"{FirstName} {LastName}";
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }
}