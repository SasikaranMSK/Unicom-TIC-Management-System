using System;
using System.Drawing;
using System.Windows.Forms;
using UnicornTICManagementSystem.Controllers;
using UnicornTICManagementSystem.Models;

namespace UnicornTICManagementSystem.Views
{
    public partial class TimetableForm : Form
    {
        private readonly TimetableController _timetableController;
        private DataGridView dgvTimetable;
        private ComboBox cmbDay;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Label lblDay;

        public TimetableForm()
        {
            _timetableController = new TimetableController();
            InitializeComponent();
            LoadTimetable();
        }

        private void InitializeComponent()
        {
            this.Text = "Timetable Management";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Day filter
            lblDay = new Label();
            lblDay.Text = "Day:";
            lblDay.Location = new Point(20, 20);
            lblDay.Size = new Size(40, 20);
            this.Controls.Add(lblDay);

            cmbDay = new ComboBox();
            cmbDay.Location = new Point(70, 18);
            cmbDay.Size = new Size(120, 25);
            cmbDay.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDay.Items.AddRange(new string[] { "All Days", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
            cmbDay.SelectedIndex = 0;
            cmbDay.SelectedIndexChanged += CmbDay_SelectedIndexChanged;
            this.Controls.Add(cmbDay);

            // Buttons
            btnAdd = new Button();
            btnAdd.Text = "Add Entry";
            btnAdd.Location = new Point(220, 15);
            btnAdd.Size = new Size(100, 30);
            btnAdd.BackColor = Color.LightGreen;
            btnAdd.Click += BtnAdd_ClickAsync;
            this.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Edit Entry";
            btnEdit.Location = new Point(330, 15);
            btnEdit.Size = new Size(100, 30);
            btnEdit.BackColor = Color.LightBlue;
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "Delete Entry";
            btnDelete.Location = new Point(440, 15);
            btnDelete.Size = new Size(100, 30);
            btnDelete.BackColor = Color.LightCoral;
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(550, 15);
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            // DataGridView
            dgvTimetable = new DataGridView();
            dgvTimetable.Location = new Point(20, 60);
            dgvTimetable.Size = new Size(950, 500);
            dgvTimetable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTimetable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTimetable.MultiSelect = false;
            dgvTimetable.ReadOnly = true;
            dgvTimetable.AllowUserToAddRows = false;
            this.Controls.Add(dgvTimetable);
        }

        private async void LoadTimetable()
        {
            try
            {
                dgvTimetable.AutoGenerateColumns = true;
                dgvTimetable.DataSource = await _timetableController.GetAllTimetablesAsync();

                // Hide unnecessary columns
                if (dgvTimetable.Columns["Id"] != null)
                    dgvTimetable.Columns["Id"].Visible = false;
                if (dgvTimetable.Columns["CourseId"] != null)
                    dgvTimetable.Columns["CourseId"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading timetable: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbDay_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Filter timetable by selected day
            LoadTimetable();
        }

        private async void BtnAdd_ClickAsync(object? sender, EventArgs e)
        {
            using (var editForm = new TimetableEditForm())
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    var newEntry = new Models.Timetable
                    {
                        CourseId = editForm.TimetableEntry.CourseId,
                        CourseName = editForm.TimetableEntry.CourseName,
                        DayOfWeek = editForm.TimetableEntry.DayOfWeek,
                        StartTime = editForm.TimetableEntry.StartTime,
                        EndTime = editForm.TimetableEntry.EndTime,
                        Classroom = editForm.TimetableEntry.Classroom,
                        TeacherName = editForm.TimetableEntry.TeacherName,
                        EffectiveDate = editForm.TimetableEntry.EffectiveDate,
                        EndDate = editForm.TimetableEntry.EndDate,
                        IsActive = editForm.TimetableEntry.IsActive
                    };

                    bool success = await _timetableController.AddTimetableEntryAsync(newEntry);
                    if (success)
                        LoadTimetable();
                    else
                        MessageBox.Show("Failed to add timetable entry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvTimetable.CurrentRow?.DataBoundItem is Timetable selected)
            {
                using (var editForm = new TimetableEditForm(selected))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        var updatedEntry = editForm.TimetableEntry;
                        updatedEntry.Id = selected.Id; // Ensure ID is set for update
                                                       // Implement an UpdateTimetableEntryAsync in your controller if needed
                                                       // bool success = await _timetableController.UpdateTimetableEntryAsync(updatedEntry);
                                                       // if (success)
                                                       //     LoadTimetable();
                                                       // else
                                                       //     MessageBox.Show("Failed to update timetable entry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvTimetable.CurrentRow?.DataBoundItem is Timetable selected)
            {
                var confirm = MessageBox.Show(
                    $"Are you sure you want to delete the timetable entry for {selected.CourseName} on {selected.DayOfWeek}?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    bool success = await _timetableController.DeleteTimetableEntryAsync(selected.Id);
                    if (success)
                        LoadTimetable();
                    else
                        MessageBox.Show("Failed to delete timetable entry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a timetable entry to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadTimetable();
        }
    }
}