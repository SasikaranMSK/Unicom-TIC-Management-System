using System;
using System.Drawing;
using System.Windows.Forms;
using UnicornTICManagementSystem.Controllers;
using UnicornTICManagementSystem.Models;

namespace UnicornTICManagementSystem.Views
{
    public partial class ExamForm : Form
    {
        private readonly ExamController _examController;
        private DataGridView dgvExams;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        public ExamForm()
        {
            _examController = new ExamController();
            InitializeComponent();
            LoadExams();

        }

        private void InitializeComponent()
        {
            this.Text = "Exam Management";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Buttons
            btnAdd = new Button();
            btnAdd.Text = "Add Exam";
            btnAdd.Location = new Point(20, 15);
            btnAdd.Size = new Size(100, 30);
            btnAdd.BackColor = Color.LightGreen;
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Edit Exam";
            btnEdit.Location = new Point(130, 15);
            btnEdit.Size = new Size(100, 30);
            btnEdit.BackColor = Color.LightBlue;
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "Delete Exam";
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
            dgvExams = new DataGridView();
            dgvExams.Location = new Point(20, 60);
            dgvExams.Size = new Size(950, 500);
            dgvExams.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExams.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExams.MultiSelect = false;
            dgvExams.ReadOnly = true;
            dgvExams.AllowUserToAddRows = false;
            this.Controls.Add(dgvExams);
        }

        private async void LoadExams()
        {
            try
            {
                var exams = await _examController.GetAllExamsAsync();
                dgvExams.DataSource = exams;

                // Hide unnecessary columns
                if (dgvExams.Columns["Id"] != null)
                    dgvExams.Columns["Id"].Visible = false;
                if (dgvExams.Columns["CourseId"] != null)
                    dgvExams.Columns["CourseId"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading exams: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new ExamEditForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadExams();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvExams.SelectedRows.Count > 0)
            {
                var selectedExam = (Exam)dgvExams.SelectedRows[0].DataBoundItem;
                var editForm = new ExamEditForm(selectedExam);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadExams();
                }
            }
            else
            {
                MessageBox.Show("Please select an exam to edit.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvExams.SelectedRows.Count > 0)
            {
                var selectedExam = (Exam)dgvExams.SelectedRows[0].DataBoundItem;
                var result = MessageBox.Show($"Are you sure you want to delete the exam '{selectedExam.ExamName}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Set exam as inactive instead of deleting
                        selectedExam.IsActive = false;
                        await _examController.UpdateExamAsync(selectedExam);
                        LoadExams();
                        MessageBox.Show("Exam deleted successfully.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting exam: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an exam to delete.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadExams();
        }
    }
}