using System;
using System.Drawing;
using System.Windows.Forms;
using UnicornTICManagementSystem.Controllers;

namespace UnicornTICManagementSystem.Views
{
    public partial class CourseForm : Form
    {
        private readonly CourseController _courseController;
        private DataGridView dgvCourses;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        public CourseForm()
        {
            _courseController = new CourseController();
            InitializeComponent();
            LoadCourses();
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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add Course functionality will be implemented here.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Edit Course functionality will be implemented here.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Delete Course functionality will be implemented here.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadCourses();
        }
    }
}