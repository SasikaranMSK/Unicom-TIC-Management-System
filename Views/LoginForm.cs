using System;
using System.Drawing;
using System.Windows.Forms;
using UnicornTICManagementSystem.Controllers;

namespace UnicornTICManagementSystem.Views
{
    public partial class LoginForm : Form
    {
        private readonly LoginController _loginController;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private PictureBox picLogo;

        public LoginForm()
        {
            _loginController = new LoginController();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Unicorn TIC Management System - Login";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Logo
            picLogo = new PictureBox();
            picLogo.Size = new Size(64, 64);
            picLogo.Location = new Point(190, 20);
            picLogo.BackColor = Color.LightBlue;
            picLogo.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(picLogo);

            // Title
            lblTitle = new Label();
            lblTitle.Text = "Unicorn TIC Management System";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkBlue;
            lblTitle.Size = new Size(400, 30);
            lblTitle.Location = new Point(25, 100);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            // Username Label
            lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(50, 150);
            lblUsername.Size = new Size(80, 20);
            lblUsername.Font = new Font("Arial", 10);
            this.Controls.Add(lblUsername);

            // Username TextBox
            txtUsername = new TextBox();
            txtUsername.Location = new Point(140, 148);
            txtUsername.Size = new Size(250, 25);
            txtUsername.Font = new Font("Arial", 10);
            this.Controls.Add(txtUsername);

            // Password Label
            lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new Point(50, 185);
            lblPassword.Size = new Size(80, 20);
            lblPassword.Font = new Font("Arial", 10);
            this.Controls.Add(lblPassword);

            // Password TextBox
            txtPassword = new TextBox();
            txtPassword.Location = new Point(140, 183);
            txtPassword.Size = new Size(250, 25);
            txtPassword.Font = new Font("Arial", 10);
            txtPassword.UseSystemPasswordChar = true;
            this.Controls.Add(txtPassword);

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new Point(140, 230);
            btnLogin.Size = new Size(100, 35);
            btnLogin.BackColor = Color.LightBlue;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Arial", 10, FontStyle.Bold);
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // Exit Button
            btnExit = new Button();
            btnExit.Text = "Exit";
            btnExit.Location = new Point(250, 230);
            btnExit.Size = new Size(100, 35);
            btnExit.BackColor = Color.LightCoral;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Arial", 10);
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);

            // Set default button and focus
            this.AcceptButton = btnLogin;
            txtUsername.Focus();
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_loginController.ValidateCredentials(txtUsername.Text, txtPassword.Text))
                {
                    MessageBox.Show("Please enter both username and password.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnLogin.Enabled = false;
                btnLogin.Text = "Logging in...";

                var user = await _loginController.AuthenticateUserAsync(txtUsername.Text, txtPassword.Text);

                if (user != null)
                {
                    MessageBox.Show($"Welcome, {user.FullName}!", "Login Successful",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open main form and hide login form
                    var mainForm = new MainForm(user);
                    this.Hide();
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Login";
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}