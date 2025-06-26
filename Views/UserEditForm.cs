using System;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;

public class UserEditForm : Form
{
    public User User { get; private set; }
    private TextBox txtUsername, txtPassword, txtEmail, txtFirstName, txtLastName;
    private ComboBox cmbRole;
    private Button btnSave, btnCancel;

    public UserEditForm(User user = null)
    {
        this.Text = user == null ? "Create User" : "Edit User";
        this.Size = new System.Drawing.Size(400, 350);

        txtUsername = new TextBox { Left = 120, Top = 20, Width = 200 };
        txtPassword = new TextBox { Left = 120, Top = 60, Width = 200, PasswordChar = '*' };
        txtEmail = new TextBox { Left = 120, Top = 100, Width = 200 };
        txtFirstName = new TextBox { Left = 120, Top = 140, Width = 200 };
        txtLastName = new TextBox { Left = 120, Top = 180, Width = 200 };
        cmbRole = new ComboBox { Left = 120, Top = 220, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbRole.Items.AddRange(Enum.GetNames(typeof(UserRole)));
        cmbRole.SelectedIndex = 0;

        btnSave = new Button { Text = "Save", Left = 120, Top = 260, Width = 80 };
        btnCancel = new Button { Text = "Cancel", Left = 240, Top = 260, Width = 80 };

        btnSave.Click += (s, e) =>
        {
            User = user ?? new User();
            User.Username = txtUsername.Text.Trim();
            User.Password = txtPassword.Text.Trim();
            User.Email = txtEmail.Text.Trim();
            User.FirstName = txtFirstName.Text.Trim();
            User.LastName = txtLastName.Text.Trim();
            User.Role = (UserRole)Enum.Parse(typeof(UserRole), cmbRole.SelectedItem.ToString());
            User.IsActive = true;
            this.DialogResult = DialogResult.OK;
        };
        btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

        this.Controls.AddRange(new Control[]
        {
            new Label { Text = "Username:", Left = 20, Top = 20, Width = 100 },
            txtUsername,
            new Label { Text = "Password:", Left = 20, Top = 60, Width = 100 },
            txtPassword,
            new Label { Text = "Email:", Left = 20, Top = 100, Width = 100 },
            txtEmail,
            new Label { Text = "First Name:", Left = 20, Top = 140, Width = 100 },
            txtFirstName,
            new Label { Text = "Last Name:", Left = 20, Top = 180, Width = 100 },
            txtLastName,
            new Label { Text = "Role:", Left = 20, Top = 220, Width = 100 },
            cmbRole,
            btnSave, btnCancel
        });

        if (user != null)
        {
            txtUsername.Text = user.Username;
            txtPassword.Text = user.Password;
            txtEmail.Text = user.Email;
            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            cmbRole.SelectedItem = user.Role.ToString();
        }
    }
}