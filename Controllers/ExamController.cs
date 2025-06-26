using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class ExamController
    {
        private readonly DatabaseManager _databaseManager;

        public ExamController()
        {
            _databaseManager = new DatabaseManager();
        }

        public async Task<List<Exam>> GetAllExamsAsync()
        {
            return await _databaseManager.GetAllExamsAsync();
        }

        public async Task<List<Exam>> GetExamsByCourseAsync(int courseId)
        {
            return await _databaseManager.GetExamsByCourseAsync(courseId);
        }

        public async Task<bool> AddExamAsync(Exam exam)
        {
            try
            {
                return await _databaseManager.AddExamAsync(exam);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add exam: {ex.Message}");
            }
        }

        public async Task<bool> UpdateExamAsync(Exam exam)
        {
            try
            {
                return await _databaseManager.UpdateExamAsync(exam);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update exam: {ex.Message}");
            }
        }

        public bool ValidateExam(Exam exam)
        {
            return !string.IsNullOrWhiteSpace(exam.ExamName) &&
                   exam.CourseId > 0 &&
                   exam.ExamDate > DateTime.Now &&
                   exam.MaxMarks > 0;
        }
    }
}