using System;
using System.Drawing;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public partial class StudentForm : Form
    {
        private readonly StudentController _studentController;
        private DataGridView dgvStudents;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private TextBox txtSearch;
        private Label lblSearch;

        public StudentForm()
        {
            _studentController = new StudentController();
            InitializeComponent();
            LoadStudents();
        }

        private void InitializeComponent()
        {
            this.Text = "Student Management";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Search controls
            lblSearch = new Label();
            lblSearch.Text = "Search:";
            lblSearch.Location = new Point(20, 20);
            lblSearch.Size = new Size(60, 20);
            this.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(90, 18);
            txtSearch.Size = new Size(200, 25);
            txtSearch.TextChanged += TxtSearch_TextChanged;
            this.Controls.Add(txtSearch);

            // Buttons
            btnAdd = new Button();
            btnAdd.Text = "Add Student";
            btnAdd.Location = new Point(320, 15);
            btnAdd.Size = new Size(100, 30);
            btnAdd.BackColor = Color.LightGreen;
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Edit Student";
            btnEdit.Location = new Point(430, 15);
            btnEdit.Size = new Size(100, 30);
            btnEdit.BackColor = Color.LightBlue;
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "Delete Student";
            btnDelete.Location = new Point(540, 15);
            btnDelete.Size = new Size(100, 30);
            btnDelete.BackColor = Color.LightCoral;
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(650, 15);
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            // DataGridView
            dgvStudents = new DataGridView();
            dgvStudents.Location = new Point(20, 60);
            dgvStudents.Size = new Size(950, 500);
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.MultiSelect = false;
            dgvStudents.ReadOnly = true;
            dgvStudents.AllowUserToAddRows = false;
            this.Controls.Add(dgvStudents);
        }

        private async void LoadStudents()
        {
            try
            {
                var students = await _studentController.GetAllStudentsAsync();
                dgvStudents.DataSource = students;

                // Hide ID column
                if (dgvStudents.Columns["Id"] != null)
                    dgvStudents.Columns["Id"].Visible = false;

                // Set column headers
                if (dgvStudents.Columns["StudentNumber"] != null)
                    dgvStudents.Columns["StudentNumber"].HeaderText = "Student Number";
                if (dgvStudents.Columns["FirstName"] != null)
                    dgvStudents.Columns["FirstName"].HeaderText = "First Name";
                if (dgvStudents.Columns["LastName"] != null)
                    dgvStudents.Columns["LastName"].HeaderText = "Last Name";
                if (dgvStudents.Columns["Email"] != null)
                    dgvStudents.Columns["Email"].HeaderText = "Email";
                if (dgvStudents.Columns["DateOfBirth"] != null)
                    dgvStudents.Columns["DateOfBirth"].HeaderText = "Date of Birth";
                if (dgvStudents.Columns["EnrollmentDate"] != null)
                    dgvStudents.Columns["EnrollmentDate"].HeaderText = "Enrollment Date";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var addForm = new RegisterForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadStudents();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                var selectedStudent = (Student)dgvStudents.SelectedRows[0].DataBoundItem;
                var editForm = new RegisterForm(selectedStudent);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadStudents();
                }
            }
            else
            {
                MessageBox.Show("Please select a student to edit.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                var selectedStudent = (Student)dgvStudents.SelectedRows[0].DataBoundItem;
                var result = MessageBox.Show($"Are you sure you want to delete {selectedStudent.FullName}?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        await _studentController.DeleteStudentAsync(selectedStudent.Id);
                        LoadStudents();
                        MessageBox.Show("Student deleted successfully.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting student: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a student to delete.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadStudents();
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            // Implement search functionality
            // This would filter the DataGridView based on the search text
        }
    }
}