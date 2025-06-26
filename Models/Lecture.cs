using System;

namespace UnicomTICManagementSystem.Models
{
    public class Lecture
    {
        public int Id { get; set; }
        public string LectureName { get; set; }
        public string Email { get; set; }
       
        public bool IsActive { get; set; }
    }
}