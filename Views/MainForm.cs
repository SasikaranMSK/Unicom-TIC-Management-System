using System;
using System.Drawing;
using System.Windows.Forms;
using UnicornTICManagementSystem.Models;

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

            // File Menu
            var fileMenu = new ToolStripMenuItem("&File");
            fileMenu.DropDownItems.Add("&Logout", null, Logout_Click);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("E&xit", null, Exit_Click);

            // Students Menu
            var studentsMenu = new ToolStripMenuItem("&Students");
            studentsMenu.DropDownItems.Add("&Manage Students", null, ManageStudents_Click);
            studentsMenu.DropDownItems.Add("&Register Student", null, RegisterStudent_Click);

            // Courses Menu
            var coursesMenu = new ToolStripMenuItem("&Courses");
            coursesMenu.DropDownItems.Add("&Manage Courses", null, ManageCourses_Click);
            coursesMenu.DropDownItems.Add("&Add Course", null, AddCourse_Click);

            // Exams Menu
            var examsMenu = new ToolStripMenuItem("&Exams");
            examsMenu.DropDownItems.Add("&Manage Exams", null, ManageExams_Click);
            examsMenu.DropDownItems.Add("&Schedule Exam", null, ScheduleExam_Click);

            // Marks Menu
            var marksMenu = new ToolStripMenuItem("&Marks");
            marksMenu.DropDownItems.Add("&Enter Marks", null, EnterMarks_Click);
            marksMenu.DropDownItems.Add("&View Reports", null, ViewReports_Click);

            // Timetable Menu
            var timetableMenu = new ToolStripMenuItem("&Timetable");
            timetableMenu.DropDownItems.Add("&View Timetable", null, ViewTimetable_Click);
            timetableMenu.DropDownItems.Add("&Manage Schedule", null, ManageSchedule_Click);

            // Help Menu
            var helpMenu = new ToolStripMenuItem("&Help");
            helpMenu.DropDownItems.Add("&About", null, About_Click);

            menuStrip.Items.AddRange(new ToolStripItem[]
            {
                fileMenu, studentsMenu, coursesMenu, examsMenu, marksMenu, timetableMenu, helpMenu
            });

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
            lblWelcome.Text = $"Welcome to Unicorn TIC Management System, {_currentUser.FirstName}!";
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
            MessageBox.Show("Add Course functionality will be implemented here.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            MessageBox.Show("Manage Schedule functionality will be implemented here.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unicorn TIC Management System\nVersion 1.0.0\n\nDeveloped for educational institution management.",
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}