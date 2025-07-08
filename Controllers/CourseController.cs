using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class CourseController
    {
        private readonly DatabaseManager _databaseManager;

        public CourseController()
        {
            _databaseManager = new DatabaseManager();
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _databaseManager.GetAllCoursesAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _databaseManager.GetCourseByIdAsync(id);
        }

        public async Task<bool> AddCourseAsync(Course course)
        {
            try
            {
                return await _databaseManager.AddCourseAsync(course);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add course: {ex.Message}");
            }
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            try
            {
                return await _databaseManager.UpdateCourseAsync(course);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update course: {ex.Message}");
            }
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            try
            {
                return await _databaseManager.DeleteCourseAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete course: {ex.Message}");
            }
        }

        public bool ValidateCourse(Course course)
        {
            return !string.IsNullOrWhiteSpace(course.CourseCode) &&
                   !string.IsNullOrWhiteSpace(course.CourseName) &&
                   course.Credits > 0 &&
                   course.MaxStudents > 0;
        }
    }
}