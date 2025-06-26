using System;
using System.Drawing;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public class CourseEditForm : Form
    {
        private TextBox txtCourseCode;
        private TextBox txtCourseName;
        private TextBox txtDescription;
        private NumericUpDown nudCredits;
        private TextBox txtTeacherName;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private NumericUpDown nudMaxStudents;
        private CheckBox chkIsActive;
        private Button btnOK;
        private Button btnCancel;

        public Course Course { get; private set; }

        public CourseEditForm()
            : this(null)
        {
        }

        public CourseEditForm(Course course)
        {
            this.Text = course == null ? "Add Course" : "Edit Course";
            this.Size = new Size(450, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            int labelX = 20, controlX = 150, y = 30, yStep = 40;

            Label lblCourseCode = new Label { Text = "Course Code:", Location = new Point(labelX, y), AutoSize = true };
            txtCourseCode = new TextBox { Location = new Point(controlX, y - 5), Width = 250 };
            y += yStep;

            Label lblCourseName = new Label { Text = "Course Name:", Location = new Point(labelX, y), AutoSize = true };
            txtCourseName = new TextBox { Location = new Point(controlX, y - 5), Width = 250 };
            y += yStep;

            Label lblDescription = new Label { Text = "Description:", Location = new Point(labelX, y), AutoSize = true };
            txtDescription = new TextBox { Location = new Point(controlX, y - 5), Width = 250, Height = 50, Multiline = true, ScrollBars = ScrollBars.Vertical };
            y += yStep + 20;

            Label lblCredits = new Label { Text = "Credits:", Location = new Point(labelX, y), AutoSize = true };
            nudCredits = new NumericUpDown { Location = new Point(controlX, y - 5), Width = 80, Minimum = 0, Maximum = 100 };
            y += yStep;

            Label lblTeacherName = new Label { Text = "Teacher Name:", Location = new Point(labelX, y), AutoSize = true };
            txtTeacherName = new TextBox { Location = new Point(controlX, y - 5), Width = 250 };
            y += yStep;

            Label lblStartDate = new Label { Text = "Start Date:", Location = new Point(labelX, y), AutoSize = true };
            dtpStartDate = new DateTimePicker { Location = new Point(controlX, y - 5), Width = 150, Format = DateTimePickerFormat.Short };
            y += yStep;

            Label lblEndDate = new Label { Text = "End Date:", Location = new Point(labelX, y), AutoSize = true };
            dtpEndDate = new DateTimePicker { Location = new Point(controlX, y - 5), Width = 150, Format = DateTimePickerFormat.Short };
            y += yStep;

            Label lblMaxStudents = new Label { Text = "Max Students:", Location = new Point(labelX, y), AutoSize = true };
            nudMaxStudents = new NumericUpDown { Location = new Point(controlX, y - 5), Width = 80, Minimum = 1, Maximum = 1000 };
            y += yStep;

            chkIsActive = new CheckBox { Text = "Is Active", Location = new Point(controlX, y - 5), AutoSize = true };
            y += yStep;

            btnOK = new Button { Text = "OK", Location = new Point(controlX, y), DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Cancel", Location = new Point(controlX + 100, y), DialogResult = DialogResult.Cancel };

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblCourseCode, txtCourseCode,
                lblCourseName, txtCourseName,
                lblDescription, txtDescription,
                lblCredits, nudCredits,
                lblTeacherName, txtTeacherName,
                lblStartDate, dtpStartDate,
                lblEndDate, dtpEndDate,
                lblMaxStudents, nudMaxStudents,
                chkIsActive,
                btnOK, btnCancel
            });

            if (course != null)
            {
                txtCourseCode.Text = course.CourseCode;
                txtCourseName.Text = course.CourseName;
                txtDescription.Text = course.Description;
                nudCredits.Value = course.Credits;
                txtTeacherName.Text = course.TeacherName;
                dtpStartDate.Value = course.StartDate == default ? DateTime.Today : course.StartDate;
                dtpEndDate.Value = course.EndDate == default ? DateTime.Today : course.EndDate;
                nudMaxStudents.Value = course.MaxStudents > 0 ? course.MaxStudents : 1;
                chkIsActive.Checked = course.IsActive;
                Course = new Course
                {
                    Id = course.Id
                };
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCourseCode.Text))
            {
                MessageBox.Show("Course code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                MessageBox.Show("Course name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Course == null)
                Course = new Course();

            Course.CourseCode = txtCourseCode.Text.Trim();
            Course.CourseName = txtCourseName.Text.Trim();
            Course.Description = txtDescription.Text.Trim();
            Course.Credits = (int)nudCredits.Value;
            Course.TeacherName = txtTeacherName.Text.Trim();
            Course.StartDate = dtpStartDate.Value.Date;
            Course.EndDate = dtpEndDate.Value.Date;
            Course.MaxStudents = (int)nudMaxStudents.Value;
            Course.IsActive = chkIsActive.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
