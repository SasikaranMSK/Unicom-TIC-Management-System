using System;

namespace UnicomTICManagementSystem.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public DateTime ExamDate { get; set; }
        public TimeSpan Duration { get; set; }
        public string Location { get; set; } = string.Empty;
        public int MaxMarks { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public ExamType Type { get; set; }
        public bool IsActive { get; set; }
    }

    public enum ExamType
    {
        Midterm,
        Final,
        Quiz,
        Assignment,
        Project
    }
}