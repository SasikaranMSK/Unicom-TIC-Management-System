using System;

namespace UnicomTICManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public enum UserRole
    {
        Student = 0,
        Lecture = 1,
        Administrator = 2,
        Staff = 3
    }
}