using System;
using System.Windows.Forms;
using UnicornTICManagementSystem.Repositories;
using UnicornTICManagementSystem.Views;

namespace UnicornTICManagementSystem
{
    internal static class Program
    {
        /// <summary>  
        /// The main entry point for the application.  
        /// </summary>  
        [STAThread]
        static void Main()
        {
            var dbManager = new DatabaseManager();
            dbManager.InitializeDatabase();
            Application.EnableVisualStyles();
            //ApplicationConfiguration.Initialize();  
            Application.Run(new LoginForm());
        }
    }
}