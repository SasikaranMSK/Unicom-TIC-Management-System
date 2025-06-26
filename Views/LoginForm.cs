using System;
using System.Drawing;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;
using UnicornTICManagementSystem.Properties;

namespace UnicomTICManagementSystem.Views
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
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.LightBlue;
            this.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogo.Image = global::UnicornTICManagementSystem.Properties.Resources.UNICOM_TIC_LOGO_1536x580;
            this.picLogo.Location = new System.Drawing.Point(190, 20);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(64, 64);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            this.picLogo.WaitOnLoad = true;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(25, 100);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 30);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Unicom TIC Management System";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUsername
            // 
            this.lblUsername.Font = new System.Drawing.Font("Arial", 10F);
            this.lblUsername.Location = new System.Drawing.Point(50, 150);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(80, 20);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Arial", 10F);
            this.txtUsername.Location = new System.Drawing.Point(140, 148);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(250, 23);
            this.txtUsername.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.Font = new System.Drawing.Font("Arial", 10F);
            this.lblPassword.Location = new System.Drawing.Point(50, 185);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(80, 20);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Arial", 10F);
            this.txtPassword.Location = new System.Drawing.Point(140, 183);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(250, 23);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.LightBlue;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Location = new System.Drawing.Point(140, 230);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 35);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.LightCoral;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Arial", 10F);
            this.btnExit.Location = new System.Drawing.Point(250, 230);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 35);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(434, 311);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unicom TIC Management System - Login";

            // Wire up event handlers
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);

            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
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