using System;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class LoginController
    {
        private readonly DatabaseManager _databaseManager;

        public LoginController()
        {
            _databaseManager = new DatabaseManager();
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                // In a real application, password should be hashed
                var user = await _databaseManager.GetUserByCredentialsAsync(username, password);
                return user;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Authentication failed: {ex.Message}");
            }
        }

        public bool ValidateCredentials(string username, string password)
        {
            return !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password);
        }
    }
}