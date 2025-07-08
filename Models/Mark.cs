using System;

namespace UnicomTICManagementSystem.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int ExamId { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public decimal MarksObtained { get; set; }
        public decimal MaxMarks { get; set; }
        public string Grade { get; set; } = string.Empty;
        public DateTime DateRecorded { get; set; }
        public string Comments { get; set; } = string.Empty;

        public decimal Percentage => MaxMarks > 0 ? (MarksObtained / MaxMarks) * 100 : 0;
        public bool IsPassed => Percentage >= 50;
       
    }
    
}