using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public partial class ExamEditForm : Form
    {
   
        private readonly ExamController _examController;
        private readonly CourseController _courseController;
        private readonly Exam _editingExam;

        private TextBox txtExamName;
        private ComboBox cmbCourse;
        private DateTimePicker dtpExamDate;
        private DateTimePicker dtpExamTime;
        private NumericUpDown nudDurationHours;
        private NumericUpDown nudDurationMinutes;
        private TextBox txtLocation;
        private NumericUpDown nudMaxMarks;
        private TextBox txtInstructions;
        private ComboBox cmbExamType;
        private Button btnSave;
        private Button btnCancel;

        private List<Course> _courses;

        public ExamEditForm(Exam exam = null)
        {
            _examController = new ExamController();
            _courseController = new CourseController();
            _editingExam = exam;

            InitializeComponent();
            LoadData();

            if (_editingExam != null)
            {
                LoadExamData();
                this.Text = "Edit Exam";
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Add Exam";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int yPos = 30;
            int labelWidth = 120;
            int controlWidth = 300;
            int spacing = 40;

            // Exam Name
            var lblExamName = new Label();
            lblExamName.Text = "Exam Name:";
            lblExamName.Location = new Point(30, yPos);
            lblExamName.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblExamName);

            txtExamName = new TextBox();
            txtExamName.Location = new Point(160, yPos - 2);
            txtExamName.Size = new Size(controlWidth, 25);
            this.Controls.Add(txtExamName);

            yPos += spacing;

            // Course
            var lblCourse = new Label();
            lblCourse.Text = "Course:";
            lblCourse.Location = new Point(30, yPos);
            lblCourse.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblCourse);

            cmbCourse = new ComboBox();
            cmbCourse.Location = new Point(160, yPos - 2);
            cmbCourse.Size = new Size(controlWidth, 25);
            cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cmbCourse);

            yPos += spacing;

            // Exam Date
            var lblExamDate = new Label();
            lblExamDate.Text = "Exam Date:";
            lblExamDate.Location = new Point(30, yPos);
            lblExamDate.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblExamDate);

            dtpExamDate = new DateTimePicker();
            dtpExamDate.Location = new Point(160, yPos - 2);
            dtpExamDate.Size = new Size(150, 25);
            dtpExamDate.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dtpExamDate);

            // Exam Time
            dtpExamTime = new DateTimePicker();
            dtpExamTime.Location = new Point(320, yPos - 2);
            dtpExamTime.Size = new Size(140, 25);
            dtpExamTime.Format = DateTimePickerFormat.Time;
            dtpExamTime.ShowUpDown = true;
            this.Controls.Add(dtpExamTime);

            yPos += spacing;

            // Duration
            var lblDuration = new Label();
            lblDuration.Text = "Duration:";
            lblDuration.Location = new Point(30, yPos);
            lblDuration.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblDuration);

            nudDurationHours = new NumericUpDown();
            nudDurationHours.Location = new Point(160, yPos - 2);
            nudDurationHours.Size = new Size(60, 25);
            nudDurationHours.Minimum = 0;
            nudDurationHours.Maximum = 12;
            nudDurationHours.Value = 2;
            this.Controls.Add(nudDurationHours);

            var lblHours = new Label();
            lblHours.Text = "hrs";
            lblHours.Location = new Point(225, yPos);
            lblHours.Size = new Size(25, 20);
            this.Controls.Add(lblHours);

            nudDurationMinutes = new NumericUpDown();
            nudDurationMinutes.Location = new Point(255, yPos - 2);
            nudDurationMinutes.Size = new Size(60, 25);
            nudDurationMinutes.Minimum = 0;
            nudDurationMinutes.Maximum = 59;
            nudDurationMinutes.Increment = 15;
            this.Controls.Add(nudDurationMinutes);

            var lblMinutes = new Label();
            lblMinutes.Text = "mins";
            lblMinutes.Location = new Point(320, yPos);
            lblMinutes.Size = new Size(30, 20);
            this.Controls.Add(lblMinutes);

            yPos += spacing;

            // Location
            var lblLocation = new Label();
            lblLocation.Text = "Location:";
            lblLocation.Location = new Point(30, yPos);
            lblLocation.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblLocation);

            txtLocation = new TextBox();
            txtLocation.Location = new Point(160, yPos - 2);
            txtLocation.Size = new Size(controlWidth, 25);
            this.Controls.Add(txtLocation);

            yPos += spacing;

            // Max Marks
            var lblMaxMarks = new Label();
            lblMaxMarks.Text = "Max Marks:";
            lblMaxMarks.Location = new Point(30, yPos);
            lblMaxMarks.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblMaxMarks);

            nudMaxMarks = new NumericUpDown();
            nudMaxMarks.Location = new Point(160, yPos - 2);
            nudMaxMarks.Size = new Size(100, 25);
            nudMaxMarks.Minimum = 1;
            nudMaxMarks.Maximum = 1000;
            nudMaxMarks.Value = 100;
            this.Controls.Add(nudMaxMarks);

            yPos += spacing;

            // Exam Type
            var lblExamType = new Label();
            lblExamType.Text = "Exam Type:";
            lblExamType.Location = new Point(30, yPos);
            lblExamType.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblExamType);

            cmbExamType = new ComboBox();
            cmbExamType.Location = new Point(160, yPos - 2);
            cmbExamType.Size = new Size(controlWidth, 25);
            cmbExamType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbExamType.Items.AddRange(Enum.GetNames(typeof(ExamType)));
            this.Controls.Add(cmbExamType);

            yPos += spacing;

            // Instructions
            var lblInstructions = new Label();
            lblInstructions.Text = "Instructions:";
            lblInstructions.Location = new Point(30, yPos);
            lblInstructions.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblInstructions);

            txtInstructions = new TextBox();
            txtInstructions.Location = new Point(160, yPos - 2);
            txtInstructions.Size = new Size(controlWidth, 60);
            txtInstructions.Multiline = true;
            txtInstructions.ScrollBars = ScrollBars.Vertical;
            this.Controls.Add(txtInstructions);

            yPos += 80;

            // Buttons
            btnSave = new Button();
            btnSave.Text = _editingExam != null ? "Update" : "Save";
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
        }

        private async void LoadData()
        {
            try
            {
                // Load courses
                var modelCourses = await _courseController.GetAllCoursesAsync();
                _courses = modelCourses.Select(c => new Course
                {
                    Id = c.Id,
                    CourseCode = c.CourseCode,
                    CourseName = c.CourseName,
                    Description = c.Description,
                    Credits = c.Credits,
                    TeacherId = c.TeacherId,
                    TeacherName = c.TeacherName,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    MaxStudents = c.MaxStudents,
                    IsActive = c.IsActive
                }).ToList();

                cmbCourse.DisplayMember = "CourseName";
                cmbCourse.ValueMember = "Id";
                cmbCourse.DataSource = _courses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExamData()
        {
            if (_editingExam != null)
            {
                txtExamName.Text = _editingExam.ExamName;

                // Set course
                var course = _courses?.FirstOrDefault(c => c.Id == _editingExam.CourseId);
                if (course != null)
                    cmbCourse.SelectedValue = course.Id;

                dtpExamDate.Value = _editingExam.ExamDate.Date;
                dtpExamTime.Value = DateTime.Today.Add(_editingExam.ExamDate.TimeOfDay);

                nudDurationHours.Value = _editingExam.Duration.Hours;
                nudDurationMinutes.Value = _editingExam.Duration.Minutes;

                txtLocation.Text = _editingExam.Location;
                nudMaxMarks.Value = _editingExam.MaxMarks;
                txtInstructions.Text = _editingExam.Instructions;
                cmbExamType.SelectedItem = _editingExam.Type.ToString();
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtExamName.Text))
                {
                    MessageBox.Show("Please enter exam name.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbCourse.SelectedItem == null)
                {
                    MessageBox.Show("Please select a course.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedCourse = (Course)cmbCourse.SelectedItem;
                var examDateTime = dtpExamDate.Value.Date.Add(dtpExamTime.Value.TimeOfDay);
                var duration = new TimeSpan((int)nudDurationHours.Value, (int)nudDurationMinutes.Value, 0);

                var exam = new Exam
                {
                    Id = _editingExam?.Id ?? 0,
                    ExamName = txtExamName.Text.Trim(),
                    CourseId = selectedCourse.Id,
                    CourseName = selectedCourse.CourseName,
                    ExamDate = examDateTime,
                    Duration = duration,
                    Location = txtLocation.Text.Trim(),
                    MaxMarks = (int)nudMaxMarks.Value,
                    Instructions = txtInstructions.Text.Trim(),
                    Type = (ExamType)Enum.Parse(typeof(ExamType), cmbExamType.SelectedItem.ToString()),
                    IsActive = true
                };

                if (!_examController.ValidateExam(exam))
                {
                    MessageBox.Show("Please check the exam details. Exam date must be in the future.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool success;
                if (_editingExam != null)
                {
                    success = await _examController.UpdateExamAsync(exam);
                }
                else
                {
                    success = await _examController.AddExamAsync(exam);
                }

                if (success)
                {
                    MessageBox.Show($"Exam {(_editingExam != null ? "updated" : "added")} successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Failed to {(_editingExam != null ? "update" : "add")} exam.",
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