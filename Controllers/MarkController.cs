using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicornTICManagementSystem.Models;
using UnicornTICManagementSystem.Repositories;

namespace UnicornTICManagementSystem.Controllers
{
    public class MarkController
    {
        private readonly DatabaseManager _databaseManager;

        public MarkController()
        {
            _databaseManager = new DatabaseManager();
        }

        public async Task<List<Mark>> GetAllMarksAsync()
        {
            return await _databaseManager.GetAllMarksAsync();
        }

        public async Task<List<Mark>> GetMarksByStudentAsync(int studentId)
        {
            return await _databaseManager.GetMarksByStudentAsync(studentId);
        }

        public async Task<List<Mark>> GetMarksByCourseAsync(int courseId)
        {
            return await _databaseManager.GetMarksByCourseAsync(courseId);
        }

        public async Task<bool> AddMarkAsync(Mark mark)
        {
            try
            {
                mark.Grade = CalculateGrade(mark.Percentage);
                mark.DateRecorded = DateTime.Now;
                return await _databaseManager.AddMarkAsync(mark);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add mark: {ex.Message}");
            }
        }

        public async Task<bool> UpdateMarkAsync(Mark mark)
        {
            try
            {
                mark.Grade = CalculateGrade(mark.Percentage);
                return await _databaseManager.UpdateMarkAsync(mark);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update mark: {ex.Message}");
            }
        }

        private string CalculateGrade(decimal percentage)
        {
            if (percentage >= 90) return "A+";
            if (percentage >= 85) return "A";
            if (percentage >= 80) return "A-";
            if (percentage >= 75) return "B+";
            if (percentage >= 70) return "B";
            if (percentage >= 65) return "B-";
            if (percentage >= 60) return "C+";
            if (percentage >= 55) return "C";
            if (percentage >= 50) return "C-";
            return "F";
        }

        public bool ValidateMark(Mark mark)
        {
            return mark.StudentId > 0 &&
                   mark.ExamId > 0 &&
                   mark.MarksObtained >= 0 &&
                   mark.MarksObtained <= mark.MaxMarks;
        }

        public string GetGradeDescription(string grade)
        {
            switch (grade)
            {
                case "A+": return "Excellent (90-100%)";
                case "A": return "Very Good (85-89%)";
                case "A-": return "Good (80-84%)";
                case "B+": return "Above Average (75-79%)";
                case "B": return "Average (70-74%)";
                case "B-": return "Below Average (65-69%)";
                case "C+": return "Satisfactory (60-64%)";
                case "C": return "Pass (55-59%)";
                case "C-": return "Marginal Pass (50-54%)";
                case "F": return "Fail (Below 50%)";
                default: return "Unknown Grade";
            }
        }

        public decimal CalculateGPA(List<Mark> marks)
        {
            if (marks == null || marks.Count == 0)
                return 0;

            decimal totalPoints = 0;
            int totalCredits = 0;

            foreach (var mark in marks)
            {
                decimal gradePoints = GetGradePoints(mark.Grade);
                // Assuming each course has 3 credits (you might want to get this from the course)
                int credits = 3;

                totalPoints += gradePoints * credits;
                totalCredits += credits;
            }

            return totalCredits > 0 ? Math.Round(totalPoints / totalCredits, 2) : 0;
        }

        private decimal GetGradePoints(string grade)
        {
            switch (grade)
            {
                case "A+": return 4.0m;
                case "A": return 3.7m;
                case "A-": return 3.3m;
                case "B+": return 3.0m;
                case "B": return 2.7m;
                case "B-": return 2.3m;
                case "C+": return 2.0m;
                case "C": return 1.7m;
                case "C-": return 1.3m;
                case "F": return 0.0m;
                default: return 0.0m;
            }
        }
    }
}