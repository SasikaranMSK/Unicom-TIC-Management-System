using System;

namespace UnicornTICManagementSystem.Models
{
    public class Timetable
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Classroom { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string DayName { get; set; } = string.Empty;
    }
}