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
    public partial class MarkEditForm : Form
    {
        private readonly MarkController _markController;
        private readonly StudentController _studentController;
        private readonly ExamController _examController;
        private readonly Mark _editingMark;

        private ComboBox cmbStudent;
        private ComboBox cmbExam;
        private TextBox txtMarksObtained;
        private TextBox txtMaxMarks;
        private TextBox txtGrade;
        private TextBox txtComments;
        private Button btnSave;
        private Button btnCancel;

        private List<Student> _students;
        private List<Exam> _exams;

        public MarkEditForm(Mark mark = null)
        {
            _markController = new MarkController();
            _studentController = new StudentController();
            _examController = new ExamController();
            _editingMark = mark;

            InitializeComponent();
            LoadData();

            if (_editingMark != null)
            {
                LoadMarkData();
                this.Text = "Edit Mark";
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Add Mark";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int yPos = 30;
            int labelWidth = 120;
            int controlWidth = 300;
            int spacing = 40;

            // Student
            var lblStudent = new Label();
            lblStudent.Text = "Student:";
            lblStudent.Location = new Point(30, yPos);
            lblStudent.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblStudent);

            cmbStudent = new ComboBox();
            cmbStudent.Location = new Point(160, yPos - 2);
            cmbStudent.Size = new Size(controlWidth, 25);
            cmbStudent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cmbStudent);

            yPos += spacing;

            // Exam
            var lblExam = new Label();
            lblExam.Text = "Exam:";
            lblExam.Location = new Point(30, yPos);
            lblExam.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblExam);

            cmbExam = new ComboBox();
            cmbExam.Location = new Point(160, yPos - 2);
            cmbExam.Size = new Size(controlWidth, 25);
            cmbExam.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbExam.SelectedIndexChanged += CmbExam_SelectedIndexChanged;
            this.Controls.Add(cmbExam);

            yPos += spacing;

            // Marks Obtained
            var lblMarksObtained = new Label();
            lblMarksObtained.Text = "Marks Obtained:";
            lblMarksObtained.Location = new Point(30, yPos);
            lblMarksObtained.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblMarksObtained);

            txtMarksObtained = new TextBox();
            txtMarksObtained.Location = new Point(160, yPos - 2);
            txtMarksObtained.Size = new Size(controlWidth, 25);
            txtMarksObtained.TextChanged += TxtMarksObtained_TextChanged;
            this.Controls.Add(txtMarksObtained);

            yPos += spacing;

            // Max Marks
            var lblMaxMarks = new Label();
            lblMaxMarks.Text = "Max Marks:";
            lblMaxMarks.Location = new Point(30, yPos);
            lblMaxMarks.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblMaxMarks);

            txtMaxMarks = new TextBox();
            txtMaxMarks.Location = new Point(160, yPos - 2);
            txtMaxMarks.Size = new Size(controlWidth, 25);
            txtMaxMarks.ReadOnly = true;
            txtMaxMarks.BackColor = Color.LightGray;
            this.Controls.Add(txtMaxMarks);

            yPos += spacing;

            // Grade
            var lblGrade = new Label();
            lblGrade.Text = "Grade:";
            lblGrade.Location = new Point(30, yPos);
            lblGrade.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblGrade);

            txtGrade = new TextBox();
            txtGrade.Location = new Point(160, yPos - 2);
            txtGrade.Size = new Size(controlWidth, 25);
            txtGrade.ReadOnly = true;
            txtGrade.BackColor = Color.LightGray;
            this.Controls.Add(txtGrade);

            yPos += spacing;

            // Comments
            var lblComments = new Label();
            lblComments.Text = "Comments:";
            lblComments.Location = new Point(30, yPos);
            lblComments.Size = new Size(labelWidth, 20);
            this.Controls.Add(lblComments);

            txtComments = new TextBox();
            txtComments.Location = new Point(160, yPos - 2);
            txtComments.Size = new Size(controlWidth, 50);
            txtComments.Multiline = true;
            this.Controls.Add(txtComments);

            yPos += 70;

            // Buttons
            btnSave = new Button();
            btnSave.Text = _editingMark != null ? "Update" : "Save";
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
                // Load students
                _students = await _studentController.GetAllStudentsAsync();
                cmbStudent.DisplayMember = "FullName";
                cmbStudent.ValueMember = "Id";
                cmbStudent.DataSource = _students;

                // Load exams
                _exams = await _examController.GetAllExamsAsync();
                cmbExam.DisplayMember = "ExamName";
                cmbExam.ValueMember = "Id";
                cmbExam.DataSource = _exams;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMarkData()
        {
            if (_editingMark != null)
            {
                // Set student
                var student = _students?.FirstOrDefault(s => s.Id == _editingMark.StudentId);
                if (student != null)
                    cmbStudent.SelectedValue = student.Id;

                // Set exam
                var exam = _exams?.FirstOrDefault(e => e.Id == _editingMark.ExamId);
                if (exam != null)
                    cmbExam.SelectedValue = exam.Id;

                txtMarksObtained.Text = _editingMark.MarksObtained.ToString();
                txtMaxMarks.Text = _editingMark.MaxMarks.ToString();
                txtGrade.Text = _editingMark.Grade;
                txtComments.Text = _editingMark.Comments;
            }
        }

        private void CmbExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExam.SelectedItem is Exam selectedExam)
            {
                txtMaxMarks.Text = selectedExam.MaxMarks.ToString();
                CalculateGrade();
            }
        }

        private void TxtMarksObtained_TextChanged(object sender, EventArgs e)
        {
            CalculateGrade();
        }

        private void CalculateGrade()
        {
            if (decimal.TryParse(txtMarksObtained.Text, out decimal marksObtained) &&
                decimal.TryParse(txtMaxMarks.Text, out decimal maxMarks) &&
                maxMarks > 0)
            {
                decimal percentage = (marksObtained / maxMarks) * 100;
                string grade = CalculateGradeFromPercentage(percentage);
                txtGrade.Text = grade;
            }
            else
            {
                txtGrade.Text = "";
            }
        }

        private string CalculateGradeFromPercentage(decimal percentage)
        {
            if (percentage >= 90) return "A+";
            if (percentage >= 85) return "A";
            if (percentage >= 80) return "A-";
            if (percentage >= 75) return "B+";
            if (percentage >= 70) return "B";
            if (percentage >= 65) return "B-";
            if (percentage >= 60) return "C+";
            if (percentage >= 55) return "C";
            if (percentage >= 50) return "C-";
            return "F";
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbStudent.SelectedItem == null || cmbExam.SelectedItem == null)
                {
                    MessageBox.Show("Please select both student and exam.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtMarksObtained.Text, out decimal marksObtained))
                {
                    MessageBox.Show("Please enter valid marks obtained.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedStudent = (Student)cmbStudent.SelectedItem;
                var selectedExam = (Exam)cmbExam.SelectedItem;

                var mark = new Mark
                {
                    Id = _editingMark?.Id ?? 0,
                    StudentId = selectedStudent.Id,
                    StudentName = selectedStudent.FullName,
                    ExamId = selectedExam.Id,
                    ExamName = selectedExam.ExamName,
                    CourseId = selectedExam.CourseId,
                    CourseName = selectedExam.CourseName,
                    MarksObtained = marksObtained,
                    MaxMarks = selectedExam.MaxMarks,
                    Grade = txtGrade.Text,
                    DateRecorded = _editingMark?.DateRecorded ?? DateTime.Now,
                    Comments = txtComments.Text.Trim()
                };

                if (!_markController.ValidateMark(mark))
                {
                    MessageBox.Show("Please check the mark values. Marks obtained cannot exceed maximum marks.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool success;
                if (_editingMark != null)
                {
                    success = await _markController.UpdateMarkAsync(mark);
                }
                else
                {
                    success = await _markController.AddMarkAsync(mark);
                }

                if (success)
                {
                    MessageBox.Show($"Mark {(_editingMark != null ? "updated" : "added")} successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Failed to {(_editingMark != null ? "update" : "add")} mark.",
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