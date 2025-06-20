using System;
using System.Drawing;
using System.Windows.Forms;
using UnicornTICManagementSystem.Controllers;
using UnicornTICManagementSystem.Models;

namespace UnicornTICManagementSystem.Views
{
    public partial class MarkForm : Form
    {
        private readonly MarkController _markController;
        private DataGridView dgvMarks;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnRefresh;
        private ComboBox cmbFilterType;
        private Label lblFilter;

        public MarkForm()
        {
            _markController = new MarkController();
            InitializeComponent();
            LoadMarks();
        }

        private void InitializeComponent()
        {
            this.Text = "Mark Management";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Filter controls
            lblFilter = new Label();
            lblFilter.Text = "Filter by:";
            lblFilter.Location = new Point(20, 20);
            lblFilter.Size = new Size(60, 20);
            this.Controls.Add(lblFilter);

            cmbFilterType = new ComboBox();
            cmbFilterType.Location = new Point(90, 18);
            cmbFilterType.Size = new Size(150, 25);
            cmbFilterType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterType.Items.AddRange(new string[] { "All Marks", "By Student", "By Course" });
            cmbFilterType.SelectedIndex = 0;
            this.Controls.Add(cmbFilterType);

            // Buttons
            btnAdd = new Button();
            btnAdd.Text = "Add Mark";
            btnAdd.Location = new Point(270, 15);
            btnAdd.Size = new Size(100, 30);
            btnAdd.BackColor = Color.LightGreen;
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Edit Mark";
            btnEdit.Location = new Point(380, 15);
            btnEdit.Size = new Size(100, 30);
            btnEdit.BackColor = Color.LightBlue;
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(490, 15);
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            // DataGridView
            dgvMarks = new DataGridView();
            dgvMarks.Location = new Point(20, 60);
            dgvMarks.Size = new Size(950, 500);
            dgvMarks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMarks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMarks.MultiSelect = false;
            dgvMarks.ReadOnly = true;
            dgvMarks.AllowUserToAddRows = false;
            this.Controls.Add(dgvMarks);
        }

        private async void LoadMarks()
        {
            try
            {
                var marks = await _markController.GetAllMarksAsync();
                dgvMarks.DataSource = marks;

                // Hide ID columns
                if (dgvMarks.Columns["Id"] != null)
                    dgvMarks.Columns["Id"].Visible = false;
                if (dgvMarks.Columns["StudentId"] != null)
                    dgvMarks.Columns["StudentId"].Visible = false;
                if (dgvMarks.Columns["ExamId"] != null)
                    dgvMarks.Columns["ExamId"].Visible = false;
                if (dgvMarks.Columns["CourseId"] != null)
                    dgvMarks.Columns["CourseId"].Visible = false;

                // Set column headers
                if (dgvMarks.Columns["StudentName"] != null)
                    dgvMarks.Columns["StudentName"].HeaderText = "Student";
                if (dgvMarks.Columns["ExamName"] != null)
                    dgvMarks.Columns["ExamName"].HeaderText = "Exam";
                if (dgvMarks.Columns["CourseName"] != null)
                    dgvMarks.Columns["CourseName"].HeaderText = "Course";
                if (dgvMarks.Columns["MarksObtained"] != null)
                    dgvMarks.Columns["MarksObtained"].HeaderText = "Marks";
                if (dgvMarks.Columns["MaxMarks"] != null)
                    dgvMarks.Columns["MaxMarks"].HeaderText = "Max Marks";
                if (dgvMarks.Columns["DateRecorded"] != null)
                    dgvMarks.Columns["DateRecorded"].HeaderText = "Date Recorded";
                if (dgvMarks.Columns["Percentage"] != null)
                    dgvMarks.Columns["Percentage"].HeaderText = "Percentage";
                if (dgvMarks.Columns["IsPassed"] != null)
                    dgvMarks.Columns["IsPassed"].HeaderText = "Passed";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading marks: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new MarkEditForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadMarks();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMarks.SelectedRows.Count > 0)
            {
                var selectedMark = (Mark)dgvMarks.SelectedRows[0].DataBoundItem;
                var editForm = new MarkEditForm(selectedMark);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadMarks();
                }
            }
            else
            {
                MessageBox.Show("Please select a mark to edit.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadMarks();
        }
    }
}