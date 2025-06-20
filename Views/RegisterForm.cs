using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using UnicornTICManagementSystem.Controllers;
using UnicornTICManagementSystem.Models;

namespace UnicornTICManagementSystem.Views
{
    public partial class RegisterForm : Form
    {
        private readonly StudentController _studentController;
        private readonly Student? _editingStudent;
        private TextBox txtStudentNumber;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private DateTimePicker dtpDateOfBirth;
        private TextBox txtAddress;
        private TextBox txtPhoneNumber;
        private Button btnSave;
        private Button btnCancel;

        public RegisterForm(Student? student = null)
        {
            _studentController = new StudentController();
            _editingStudent = student;
            InitializeComponent();
            
            if (_editingStudent != null)
            {
                LoadStudentData();
                this.Text = "Edit Student";
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Register Student";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int yPos = 30;
            int labelWidth = 120;
            int textBoxWidth = 300;
            int spacing = 40;

            // Student Number
            var lblStudentNumber = new Label();
            lblStudentNumber.Text = "Student Number:";
            lblStudentNumber.Location = new Point(30, yPos);
            lblStudentNumber.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblStudentNumber);

            txtStudentNumber = new TextBox();
            txtStudentNumber.Location = new Point(160, yPos - 2);
            txtStudentNumber.Size = new Size(textBoxWidth, 25);
            this.Controls.Add(txtStudentNumber);
     
            yPos += spacing;

            // First Name
            var lblFirstName = new Label();
            lblFirstName.Text = "First Name:";
            lblFirstName.Location = new Point(30, yPos);
            lblFirstName.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblFirstName);

            txtFirstName = new TextBox();
            txtFirstName.Location = new Point(160, yPos - 2);
            txtFirstName.Size = new Size(textBoxWidth, 25);
            this.Controls.Add(txtFirstName);

            yPos += spacing;

            // Last Name
            var lblLastName = new Label();
            lblLastName.Text = "Last Name:";
            lblLastName.Location = new Point(30, yPos);
            lblLastName.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblLastName);

            txtLastName = new TextBox();
            txtLastName.Location = new Point(160, yPos - 2);
            txtLastName.Size = new Size(textBoxWidth, 25);
            this.Controls.Add(txtLastName);

            yPos += spacing;

            // Email
            var lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(30, yPos);
            lblEmail.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Location = new Point(160, yPos - 2);
            txtEmail.Size = new Size(textBoxWidth, 25);
            this.Controls.Add(txtEmail);

            yPos += spacing;

            // Date of Birth
            var lblDateOfBirth = new Label();
            lblDateOfBirth.Text = "Date of Birth:";
            lblDateOfBirth.Location = new Point(30, yPos);
            lblDateOfBirth.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblDateOfBirth);

            dtpDateOfBirth = new DateTimePicker();
            dtpDateOfBirth.Location = new Point(160, yPos - 2);
            dtpDateOfBirth.Size = new Size(textBoxWidth, 25);
            dtpDateOfBirth.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dtpDateOfBirth);

            yPos += spacing;

            // Address
            var lblAddress = new Label();
            lblAddress.Text = "Address:";
            lblAddress.Location = new Point(30, yPos);
            lblAddress.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblAddress);

            txtAddress = new TextBox();
            txtAddress.Location = new Point(160, yPos - 2);
            txtAddress.Size = new Size(textBoxWidth, 25);
            this.Controls.Add(txtAddress);

            yPos += spacing;

            // Phone Number
            var lblPhoneNumber = new Label();
            lblPhoneNumber.Text = "Phone Number:";
            lblPhoneNumber.Location = new Point(30, yPos);
            lblPhoneNumber.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblPhoneNumber);

            txtPhoneNumber = new TextBox();
            txtPhoneNumber.Location = new Point(160, yPos - 2);
            txtPhoneNumber.Size = new Size(textBoxWidth, 25);
            this.Controls.Add(txtPhoneNumber);

            yPos += 60;

            // Buttons
            btnSave = new Button();
            btnSave.Text = _editingStudent != null ? "Update" : "Save";
            btnSave.Location = new Point(280, yPos);
            btnSave.Size = new Size(80, 35);
            btnSave.BackColor = Color.LightGreen;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(380, yPos);
            btnCancel.Size = new Size(80, 35);
            btnCancel.BackColor = Color.LightCoral;
            btnCancel.DialogResult = DialogResult.Cancel;
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Load += RegisterForm_Load;
        }

        private void LoadStudentData()
        {
            if (_editingStudent != null)
            {
                txtStudentNumber.Text = _editingStudent.StudentNumber;
                txtFirstName.Text = _editingStudent.FirstName;
                txtLastName.Text = _editingStudent.LastName;
                txtEmail.Text = _editingStudent.Email;
                dtpDateOfBirth.Value = _editingStudent.DateOfBirth;
                txtAddress.Text = _editingStudent.Address;
                txtPhoneNumber.Text = _editingStudent.PhoneNumber;
            }
        }

        private async void RegisterForm_Load(object sender, EventArgs e)
        {
            var nextStudentNumber = await _studentController.GetNextStudentNumberAsync();
            txtStudentNumber.Text = nextStudentNumber; // txtStudentNumber is your TextBox for student number
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                string studentNumber = string.IsNullOrWhiteSpace(txtStudentNumber.Text)
                    ? await _studentController.GetNextStudentNumberAsync()
                    : txtStudentNumber.Text.Trim();

                var student = new Student
                {
                    Id = _editingStudent?.Id ?? 0,
                    StudentNumber = studentNumber,
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    DateOfBirth = dtpDateOfBirth.Value,
                    Address = txtAddress.Text.Trim(),
                    PhoneNumber = txtPhoneNumber.Text.Trim(),
                    EnrollmentDate = _editingStudent?.EnrollmentDate ?? DateTime.Now,
                    IsActive = true
                };

                if (!_studentController.ValidateStudent(student))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool success;
                if (_editingStudent != null)
                {
                    success = await _studentController.UpdateStudentAsync(student);
                }
                else
                {
                    success = await _studentController.AddStudentAsync(student);
                }

                if (success)
                {
                    MessageBox.Show($"Student {(_editingStudent != null ? "updated" : "registered")} successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Failed to {(_editingStudent != null ? "update" : "register")} student.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}