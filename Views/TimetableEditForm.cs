using System;
using System.Drawing;
using System.Windows.Forms;
using UnicornTICManagementSystem.Models;

namespace UnicornTICManagementSystem.Views
{
    public class TimetableEditForm : Form
    {
        private TextBox txtCourseName;
        private ComboBox cmbDayOfWeek;
        private DateTimePicker dtpStartTime;
        private DateTimePicker dtpEndTime;
        private TextBox txtClassroom;
        private TextBox txtTeacherName;
        private DateTimePicker dtpEffectiveDate;
        private DateTimePicker dtpEndDate;
        private CheckBox chkIsActive;
        private Button btnOK;
        private Button btnCancel;

        public Timetable TimetableEntry { get; private set; }

        public TimetableEditForm(Timetable timetable = null)
        {
            this.Text = timetable == null ? "Add Timetable Entry" : "Edit Timetable Entry";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            if (timetable != null)
                LoadTimetable(timetable);
        }

        private void InitializeComponent()
        {
            Label lblCourseName = new Label { Text = "Course Name:", Location = new Point(20, 20), Size = new Size(100, 20) };
            txtCourseName = new TextBox { Location = new Point(130, 20), Size = new Size(220, 20) };

            Label lblDayOfWeek = new Label { Text = "Day of Week:", Location = new Point(20, 60), Size = new Size(100, 20) };
            cmbDayOfWeek = new ComboBox { Location = new Point(130, 60), Size = new Size(220, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbDayOfWeek.Items.AddRange(Enum.GetNames(typeof(DayOfWeek)));

            Label lblStartTime = new Label { Text = "Start Time:", Location = new Point(20, 100), Size = new Size(100, 20) };
            dtpStartTime = new DateTimePicker { Location = new Point(130, 100), Size = new Size(220, 20), Format = DateTimePickerFormat.Time, ShowUpDown = true };

            Label lblEndTime = new Label { Text = "End Time:", Location = new Point(20, 140), Size = new Size(100, 20) };
            dtpEndTime = new DateTimePicker { Location = new Point(130, 140), Size = new Size(220, 20), Format = DateTimePickerFormat.Time, ShowUpDown = true };

            Label lblClassroom = new Label { Text = "Classroom:", Location = new Point(20, 180), Size = new Size(100, 20) };
            txtClassroom = new TextBox { Location = new Point(130, 180), Size = new Size(220, 20) };

            Label lblTeacherName = new Label { Text = "Teacher Name:", Location = new Point(20, 220), Size = new Size(100, 20) };
            txtTeacherName = new TextBox { Location = new Point(130, 220), Size = new Size(220, 20) };

            Label lblEffectiveDate = new Label { Text = "Effective Date:", Location = new Point(20, 260), Size = new Size(100, 20) };
            dtpEffectiveDate = new DateTimePicker { Location = new Point(130, 260), Size = new Size(220, 20), Format = DateTimePickerFormat.Short };

            Label lblEndDate = new Label { Text = "End Date:", Location = new Point(20, 300), Size = new Size(100, 20) };
            dtpEndDate = new DateTimePicker { Location = new Point(130, 300), Size = new Size(220, 20), Format = DateTimePickerFormat.Short, ShowCheckBox = true };

            chkIsActive = new CheckBox { Text = "Is Active", Location = new Point(130, 340), Size = new Size(100, 20) };

            btnOK = new Button { Text = "OK", Location = new Point(80, 400), Size = new Size(100, 30) };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button { Text = "Cancel", Location = new Point(200, 400), Size = new Size(100, 30) };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] {
                lblCourseName, txtCourseName,
                lblDayOfWeek, cmbDayOfWeek,
                lblStartTime, dtpStartTime,
                lblEndTime, dtpEndTime,
                lblClassroom, txtClassroom,
                lblTeacherName, txtTeacherName,
                lblEffectiveDate, dtpEffectiveDate,
                lblEndDate, dtpEndDate,
                chkIsActive,
                btnOK, btnCancel
            });
        }

        private void LoadTimetable(Timetable timetable)
        {
            txtCourseName.Text = timetable.CourseName;
            cmbDayOfWeek.SelectedItem = timetable.DayOfWeek.ToString();
            dtpStartTime.Value = DateTime.Today.Add(timetable.StartTime);
            dtpEndTime.Value = DateTime.Today.Add(timetable.EndTime);
            txtClassroom.Text = timetable.Classroom;
            txtTeacherName.Text = timetable.TeacherName;
            dtpEffectiveDate.Value = timetable.EffectiveDate;
            if (timetable.EndDate.HasValue)
            {
                dtpEndDate.Value = timetable.EndDate.Value;
                dtpEndDate.Checked = true;
            }
            else
            {
                dtpEndDate.Checked = false;
            }
            chkIsActive.Checked = timetable.IsActive;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCourseName.Text) ||
                cmbDayOfWeek.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(txtClassroom.Text) ||
                string.IsNullOrWhiteSpace(txtTeacherName.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var timetable = new Timetable
            {
                CourseName = txtCourseName.Text.Trim(),
                DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), cmbDayOfWeek.SelectedItem.ToString()),
                StartTime = dtpStartTime.Value.TimeOfDay,
                EndTime = dtpEndTime.Value.TimeOfDay,
                Classroom = txtClassroom.Text.Trim(),
                TeacherName = txtTeacherName.Text.Trim(),
                EffectiveDate = dtpEffectiveDate.Value.Date,
                EndDate = dtpEndDate.Checked ? dtpEndDate.Value.Date : (DateTime?)null,
                IsActive = chkIsActive.Checked
            };

            TimetableEntry = timetable;
            this.DialogResult = DialogResult.OK;
        }
    }
}