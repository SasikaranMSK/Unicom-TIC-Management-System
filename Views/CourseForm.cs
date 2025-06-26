using System;
using System.Drawing;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public partial class CourseForm : Form
    {
        private readonly CourseController _courseController;
        private readonly UserRole _role;
        private DataGridView dgvCourses;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        public CourseForm(UserRole role)
        {
            _role = role;
            _courseController = new CourseController();
            InitializeComponent();
            LoadCourses();
            ApplyRolePermissions();
        }

        private void InitializeComponent()
        {
            this.Text = "Course Management";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Buttons
            btnAdd = new Button();
            btnAdd.Text = "Add Course";
            btnAdd.Location = new Point(20, 15);
            btnAdd.Size = new Size(100, 30);
            btnAdd.BackColor = Color.LightGreen;
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Edit Course";
            btnEdit.Location = new Point(130, 15);
            btnEdit.Size = new Size(100, 30);
            btnEdit.BackColor = Color.LightBlue;
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "Delete Course";
            btnDelete.Location = new Point(240, 15);
            btnDelete.Size = new Size(100, 30);
            btnDelete.BackColor = Color.LightCoral;
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(350, 15);
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            // DataGridView
            dgvCourses = new DataGridView();
            dgvCourses.Location = new Point(20, 60);
            dgvCourses.Size = new Size(950, 500);
            dgvCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCourses.MultiSelect = false;
            dgvCourses.ReadOnly = true;
            dgvCourses.AllowUserToAddRows = false;
            this.Controls.Add(dgvCourses);
        }

        private async void LoadCourses()
        {
            try
            {
                var courses = await _courseController.GetAllCoursesAsync();
                dgvCourses.DataSource = courses;

                // Hide unnecessary columns
                if (dgvCourses.Columns["Id"] != null)
                    dgvCourses.Columns["Id"].Visible = false;
                if (dgvCourses.Columns["TeacherId"] != null)
                    dgvCourses.Columns["TeacherId"].Visible = false;
                if (dgvCourses.Columns["EnrolledStudents"] != null)
                    dgvCourses.Columns["EnrolledStudents"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyRolePermissions()
        {
            if (_role == UserRole.Student)
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
            }
        }

        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new CourseEditForm())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var newCourse = dialog.Course;
                    if (_courseController.ValidateCourse(newCourse))
                    {
                        var success = await _courseController.AddCourseAsync(newCourse);
                        if (success)
                        {
                            MessageBox.Show("Course added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCourses();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add course.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid course data.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private async void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow == null)
            {
                MessageBox.Show("Please select a course to edit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var course = dgvCourses.CurrentRow.DataBoundItem as Course;
            if (course == null) return;

            using (var dialog = new CourseEditForm(course))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var updatedCourse = dialog.Course;
                    if (_courseController.ValidateCourse(updatedCourse))
                    {
                        var success = await _courseController.UpdateCourseAsync(updatedCourse);
                        if (success)
                        {
                            MessageBox.Show("Course updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCourses();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update course.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid course data.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow == null)
            {
                MessageBox.Show("Please select a course to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var course = dgvCourses.CurrentRow.DataBoundItem as Course;
            if (course == null) return;

            var confirm = MessageBox.Show($"Are you sure you want to delete course '{course.CourseName}'?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                var success = await _courseController.DeleteCourseAsync(course.Id);
                if (success)
                {
                    MessageBox.Show("Course deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCourses();
                }
                else
                {
                    MessageBox.Show("Failed to delete course.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadCourses();
        }
    }
}