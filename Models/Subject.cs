using System;
using System.Collections.Generic;

namespace UnicornTICManagementSystem.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Department { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}