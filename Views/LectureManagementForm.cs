using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Views
{
    public class LectureManagementForm : Form
    {
        private DataGridView dgvLectures;
        private Button btnEdit, btnDelete, btnRefresh;
        private DatabaseManager _db = new DatabaseManager();

        public LectureManagementForm()
        {
            this.Text = "Manage Lectures";
            this.Size = new System.Drawing.Size(800, 400);

            dgvLectures = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 300,
                ReadOnly = true,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            btnEdit = new Button { Text = "Edit", Left = 100, Top = 320, Width = 100 };
            btnDelete = new Button { Text = "Delete", Left = 220, Top = 320, Width = 100 };
            btnRefresh = new Button { Text = "Refresh", Left = 340, Top = 320, Width = 100 };

            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += (s, e) => LoadLectures();

            this.Controls.Add(dgvLectures);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnRefresh);

            LoadLectures();
        }

        private async void LoadLectures()
        {
            var lectures = await _db.GetAllLecturesAsync();
            dgvLectures.DataSource = lectures;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvLectures.CurrentRow?.DataBoundItem is Lecture lecture)
            {
                // You can implement a LectureEditForm for editing if needed
                MessageBox.Show("Edit functionality can be implemented here.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLectures.CurrentRow?.DataBoundItem is Lecture lecture)
            {
                var confirm = MessageBox.Show($"Delete lecture {lecture.LectureName}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    await _db.DeleteLectureAsync(lecture.Id);
                    LoadLectures();
                }
            }
        }
    }
}