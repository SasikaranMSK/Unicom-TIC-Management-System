using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class StudentController
    {
        private readonly DatabaseManager _databaseManager;

        public StudentController()
        {
            _databaseManager = new DatabaseManager();
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _databaseManager.GetAllStudentsAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _databaseManager.GetStudentByIdAsync(id);
        }

        public async Task<bool> AddStudentAsync(Student student)
        {
            try
            {
                return await _databaseManager.AddStudentAsync(student);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add student: {ex.Message}");
            }
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                return await _databaseManager.UpdateStudentAsync(student);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update student: {ex.Message}");
            }
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                return await _databaseManager.DeleteStudentAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete student: {ex.Message}");
            }
        }

        public bool ValidateStudent(Student student)
        {
            return !string.IsNullOrWhiteSpace(student.FirstName) &&
                   !string.IsNullOrWhiteSpace(student.LastName) &&
                   !string.IsNullOrWhiteSpace(student.Email) &&
                   !string.IsNullOrWhiteSpace(student.StudentNumber);
        }

        public async Task<string> GetNextStudentNumberAsync()
        {
            return await _databaseManager.GetNextStudentNumberAsync();
        }
    }
}