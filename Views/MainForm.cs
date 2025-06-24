using System;
using System.Drawing;
using System.Windows.Forms;
using UnicornTICManagementSystem.Models;
using UnicornTICManagementSystem.Repositories;

namespace UnicornTICManagementSystem.Views
{
    public partial class MainForm : Form
    {
        private readonly User _currentUser;
        private MenuStrip menuStrip;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private Panel mainPanel;
        private Label lblWelcome;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        public MainForm(User currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            UpdateWelcomeMessage();
        }

        private void InitializeComponent()
        {
            this.Text = "Unicorn TIC Management System";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            InitializeMenu();
            InitializeStatusBar();
            InitializeMainPanel();
        }

        private void InitializeMenu()
        {
            menuStrip = new MenuStrip();

            // File Menu (all roles)
            var fileMenu = new ToolStripMenuItem("&File");
            fileMenu.DropDownItems.Add("&Logout", null, Logout_Click);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("E&xit", null, Exit_Click);

            // Students Menu
            var studentsMenu = new ToolStripMenuItem("&Students");
            if (_currentUser.Role == UserRole.Administrator)
            {
                studentsMenu.DropDownItems.Add("&Manage Students", null, ManageStudents_Click);
                studentsMenu.DropDownItems.Add("&Register Student", null, RegisterStudent_Click);
            }

            // Courses Menu
            var coursesMenu = new ToolStripMenuItem("&Courses");
            if (_currentUser.Role == UserRole.Administrator)
            {
                coursesMenu.DropDownItems.Add("&Manage Courses", null, ManageCourses_Click);
                coursesMenu.DropDownItems.Add("&Add Course", null, AddCourse_Click);
            }

            // Exams Menu
            var examsMenu = new ToolStripMenuItem("&Exams");
            if (_currentUser.Role == UserRole.Administrator || _currentUser.Role == UserRole.Staff)
            {
                examsMenu.DropDownItems.Add("&Manage Exams", null, ManageExams_Click);
                examsMenu.DropDownItems.Add("&Schedule Exam", null, ScheduleExam_Click);
            }

            // Marks Menu
            var marksMenu = new ToolStripMenuItem("&Marks");
            if (_currentUser.Role == UserRole.Administrator || _currentUser.Role == UserRole.Staff || _currentUser.Role == UserRole.Lecture)
            {
                marksMenu.DropDownItems.Add("&Enter Marks", null, EnterMarks_Click);
            }
            if (_currentUser.Role == UserRole.Administrator || _currentUser.Role == UserRole.Staff || _currentUser.Role == UserRole.Student)
            {
                marksMenu.DropDownItems.Add("&View Reports", null, ViewReports_Click);
            }

            // Timetable Menu
            var timetableMenu = new ToolStripMenuItem("&Timetable");
            if (_currentUser.Role != UserRole.Administrator) // All except admin can view timetable
                timetableMenu.DropDownItems.Add("&View Timetable", null, ViewTimetable_Click);
            if (_currentUser.Role == UserRole.Administrator || _currentUser.Role == UserRole.Staff || _currentUser.Role == UserRole.Lecture)
                timetableMenu.DropDownItems.Add("&Manage Schedule", null, ManageSchedule_Click);

            // Help Menu (all roles)
            var helpMenu = new ToolStripMenuItem("&Help");
            helpMenu.DropDownItems.Add("&About", null, About_Click);

            // Lectures Menu (admin only)
            if (_currentUser.Role == UserRole.Administrator)
            {
                var lecturesMenu = new ToolStripMenuItem("&Lectures");
                lecturesMenu.DropDownItems.Add("&Manage Lectures", null, ManageLectures_Click);
                menuStrip.Items.Add(lecturesMenu);

                var usersMenu = new ToolStripMenuItem("&Users");
                usersMenu.DropDownItems.Add("&Manage Users", null, ManageUsers_Click);
                usersMenu.DropDownItems.Add("&Create User", null, CreateUser_Click);
                usersMenu.DropDownItems.Add("&Change Password", null, ChangePassword_Click);
                usersMenu.DropDownItems.Add("&Change Role", null, ChangeRole_Click);
                menuStrip.Items.Add(usersMenu);
            }

            // Add menus to menuStrip
            menuStrip.Items.Add(fileMenu);
            if (studentsMenu.DropDownItems.Count > 0) menuStrip.Items.Add(studentsMenu);
            if (coursesMenu.DropDownItems.Count > 0) menuStrip.Items.Add(coursesMenu);
            if (examsMenu.DropDownItems.Count > 0) menuStrip.Items.Add(examsMenu);
            if (marksMenu.DropDownItems.Count > 0) menuStrip.Items.Add(marksMenu);
            if (timetableMenu.DropDownItems.Count > 0) menuStrip.Items.Add(timetableMenu);
            menuStrip.Items.Add(helpMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void InitializeStatusBar()
        {
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = $"Logged in as: {_currentUser.FullName} ({_currentUser.Role})";
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
        }

        private void InitializeMainPanel()
        {
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.FromArgb(245, 245, 245);

            lblWelcome = new Label();
            lblWelcome.Font = new Font("Arial", 24, FontStyle.Bold);
            lblWelcome.ForeColor = Color.DarkBlue;
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(50, 50);

            mainPanel.Controls.Add(lblWelcome);
            this.Controls.Add(mainPanel);
        }

        private void UpdateWelcomeMessage()
        {
            lblWelcome.Text = $"Welcome to Unicom TIC Management System, {_currentUser.FirstName}!";
        }

        // Menu Event Handlers
        private void Logout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = new LoginForm();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ManageStudents_Click(object sender, EventArgs e)
        {
            var studentForm = new StudentForm();
            studentForm.ShowDialog();
        }

        private void RegisterStudent_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void ManageCourses_Click(object sender, EventArgs e)
        {
            var courseForm = new CourseForm();
            courseForm.ShowDialog();
        }

        private void AddCourse_Click(object sender, EventArgs e)
        {
            if (_currentUser.Role != UserRole.Administrator)
            {
                MessageBox.Show("You are not allowed to add courses.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // ... actual add course logic ...
        }

        private void ManageExams_Click(object sender, EventArgs e)
        {
            var examForm = new ExamForm();
            examForm.ShowDialog();
        }

        private void ScheduleExam_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Schedule Exam functionality will be implemented here.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EnterMarks_Click(object sender, EventArgs e)
        {
            var markForm = new MarkForm();
            markForm.ShowDialog();
        }

        private void ViewReports_Click(object sender, EventArgs e)
        {
            MessageBox.Show("View Reports functionality will be implemented here.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ViewTimetable_Click(object sender, EventArgs e)
        {
            var timetableForm = new TimetableForm();
            timetableForm.ShowDialog();
        }

        private void ManageSchedule_Click(object sender, EventArgs e)
        {
            var timetableForm = new TimetableForm();
            timetableForm.ShowDialog();
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unicorn TIC Management System\nVersion 1.0.0\n\nDeveloped for educational institution management.",
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ManageUsers_Click(object sender, EventArgs e)
        {
            using (var form = new UserManagementForm())
                form.ShowDialog();
        }

        private void CreateUser_Click(object sender, EventArgs e)
        {
            using (var form = new UserEditForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var db = new DatabaseManager();
                    db.AddUserAsync(form.User);
                    MessageBox.Show("User created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ChangePassword_Click(object sender, EventArgs e)
        {
            using (var form = new UserManagementForm())
                form.ShowDialog(); // You can add a password change dialog in UserManagementForm
        }

        private void ChangeRole_Click(object sender, EventArgs e)
        {
            using (var form = new UserManagementForm())
                form.ShowDialog(); // You can add a role change dialog in UserManagementForm
        }

        private void ManageLectures_Click(object sender, EventArgs e)
        {
            using (var form = new LectureManagementForm())
                form.ShowDialog();
        }

    }

}