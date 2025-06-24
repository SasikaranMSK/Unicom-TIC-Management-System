using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicornTICManagementSystem.Models;
using UnicornTICManagementSystem.Repositories;

public class UserManagementForm : Form
{
    private DataGridView dgvUsers;
    private Button btnEdit, btnDelete, btnRefresh;
    private DatabaseManager _db = new DatabaseManager();

    public UserManagementForm()
    {
        this.Text = "Manage Users";
        this.Size = new System.Drawing.Size(800, 400);

        dgvUsers = new DataGridView { Dock = DockStyle.Top, Height = 300, ReadOnly = true, AutoGenerateColumns = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
        btnEdit = new Button { Text = "Edit", Left = 100, Top = 320, Width = 100 };
        btnDelete = new Button { Text = "Delete", Left = 220, Top = 320, Width = 100 };
        btnRefresh = new Button { Text = "Refresh", Left = 340, Top = 320, Width = 100 };

        btnEdit.Click += BtnEdit_Click;
        btnDelete.Click += BtnDelete_Click;
        btnRefresh.Click += (s, e) => LoadUsers();

        this.Controls.Add(dgvUsers);
        this.Controls.Add(btnEdit);
        this.Controls.Add(btnDelete);
        this.Controls.Add(btnRefresh);

        LoadUsers();
    }

    private async void LoadUsers()
    {
        var users = await _db.GetAllUsersAsync();
        dgvUsers.DataSource = users;
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dgvUsers.CurrentRow?.DataBoundItem is User user)
        {
            using (var editForm = new UserEditForm(user))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    _db.UpdateUserAsync(editForm.User);
                    LoadUsers();
                }
            }
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (dgvUsers.CurrentRow?.DataBoundItem is User user)
        {
            var confirm = MessageBox.Show($"Delete user {user.Username}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                _db.DeleteUserAsync(user.Id);
                LoadUsers();
            }
        }
    }
}