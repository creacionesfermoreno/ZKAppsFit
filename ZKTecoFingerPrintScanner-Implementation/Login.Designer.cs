namespace ZKTecoFingerPrintScanner_Implementation
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtDkey = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUneg = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.txtSede = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.txtName = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.button1 = new System.Windows.Forms.Button();
            this.PicLogo = new System.Windows.Forms.PictureBox();
            this.BtnCloseLogin = new System.Windows.Forms.PictureBox();
            this.PLoading = new System.Windows.Forms.ProgressBar();
            this.btnPase = new System.Windows.Forms.Button();
            this.SbMessage = new ZKTecoFingerPrintScanner_Implementation.Controls.StatusBar();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnCloseLogin)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(124)))), ((int)(((byte)(204)))));
            this.panel1.Controls.Add(this.PicLogo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 330);
            this.panel1.TabIndex = 0;
            // 
            // txtDkey
            // 
            this.txtDkey.Depth = 0;
            this.txtDkey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDkey.ForeColor = System.Drawing.Color.DimGray;
            this.txtDkey.Hint = "";
            this.txtDkey.Location = new System.Drawing.Point(308, 63);
            this.txtDkey.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtDkey.Name = "txtDkey";
            this.txtDkey.PasswordChar = '\0';
            this.txtDkey.SelectedText = "";
            this.txtDkey.SelectionLength = 0;
            this.txtDkey.SelectionStart = 0;
            this.txtDkey.Size = new System.Drawing.Size(419, 23);
            this.txtDkey.TabIndex = 1;
            this.txtDkey.UseSystemPasswordChar = false;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(421, 24);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(189, 17);
            this.materialLabel1.TabIndex = 2;
            this.materialLabel1.Text = "INGRESE KEY EMPRESA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Yu Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(304, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Unidad negocio";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Yu Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(304, 223);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Sede";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Yu Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(304, 260);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Empresa";
            // 
            // txtUneg
            // 
            this.txtUneg.Depth = 0;
            this.txtUneg.Enabled = false;
            this.txtUneg.Hint = "";
            this.txtUneg.Location = new System.Drawing.Point(457, 179);
            this.txtUneg.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtUneg.Name = "txtUneg";
            this.txtUneg.PasswordChar = '\0';
            this.txtUneg.SelectedText = "";
            this.txtUneg.SelectionLength = 0;
            this.txtUneg.SelectionStart = 0;
            this.txtUneg.Size = new System.Drawing.Size(223, 23);
            this.txtUneg.TabIndex = 8;
            this.txtUneg.UseSystemPasswordChar = false;
            // 
            // txtSede
            // 
            this.txtSede.Depth = 0;
            this.txtSede.Enabled = false;
            this.txtSede.Hint = "";
            this.txtSede.Location = new System.Drawing.Point(457, 220);
            this.txtSede.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtSede.Name = "txtSede";
            this.txtSede.PasswordChar = '\0';
            this.txtSede.SelectedText = "";
            this.txtSede.SelectionLength = 0;
            this.txtSede.SelectionStart = 0;
            this.txtSede.Size = new System.Drawing.Size(223, 23);
            this.txtSede.TabIndex = 9;
            this.txtSede.UseSystemPasswordChar = false;
            // 
            // txtName
            // 
            this.txtName.Depth = 0;
            this.txtName.Enabled = false;
            this.txtName.Hint = "";
            this.txtName.Location = new System.Drawing.Point(457, 257);
            this.txtName.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtName.Name = "txtName";
            this.txtName.PasswordChar = '\0';
            this.txtName.SelectedText = "";
            this.txtName.SelectionLength = 0;
            this.txtName.SelectionStart = 0;
            this.txtName.Size = new System.Drawing.Size(223, 23);
            this.txtName.TabIndex = 10;
            this.txtName.UseSystemPasswordChar = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.DimGray;
            this.button1.Location = new System.Drawing.Point(308, 107);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 11;
            this.button1.Text = "VALIDAR KEY";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PicLogo
            // 
            this.PicLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.PicLogo.Image = ((System.Drawing.Image)(resources.GetObject("PicLogo.Image")));
            this.PicLogo.Location = new System.Drawing.Point(0, 0);
            this.PicLogo.Name = "PicLogo";
            this.PicLogo.Size = new System.Drawing.Size(250, 330);
            this.PicLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicLogo.TabIndex = 0;
            this.PicLogo.TabStop = false;
            // 
            // BtnCloseLogin
            // 
            this.BtnCloseLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCloseLogin.Image = ((System.Drawing.Image)(resources.GetObject("BtnCloseLogin.Image")));
            this.BtnCloseLogin.Location = new System.Drawing.Point(743, 0);
            this.BtnCloseLogin.Name = "BtnCloseLogin";
            this.BtnCloseLogin.Size = new System.Drawing.Size(40, 30);
            this.BtnCloseLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BtnCloseLogin.TabIndex = 12;
            this.BtnCloseLogin.TabStop = false;
            this.BtnCloseLogin.Click += new System.EventHandler(this.BtnCloseLogin_Click);
            // 
            // PLoading
            // 
            this.PLoading.Location = new System.Drawing.Point(308, 143);
            this.PLoading.Name = "PLoading";
            this.PLoading.Size = new System.Drawing.Size(419, 23);
            this.PLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.PLoading.TabIndex = 13;
            this.PLoading.Visible = false;
            // 
            // btnPase
            // 
            this.btnPase.BackColor = System.Drawing.Color.Transparent;
            this.btnPase.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPase.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnPase.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btnPase.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnPase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPase.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPase.ForeColor = System.Drawing.Color.DimGray;
            this.btnPase.Location = new System.Drawing.Point(424, 107);
            this.btnPase.Name = "btnPase";
            this.btnPase.Size = new System.Drawing.Size(100, 30);
            this.btnPase.TabIndex = 15;
            this.btnPase.Text = "INGRESAR";
            this.btnPase.UseVisualStyleBackColor = false;
            this.btnPase.Click += new System.EventHandler(this.btnPase_Click);
            // 
            // SbMessage
            // 
            this.SbMessage.BackColor = System.Drawing.Color.Black;
            this.SbMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SbMessage.Location = new System.Drawing.Point(308, 295);
            this.SbMessage.Message = "";
            this.SbMessage.MessageType = false;
            this.SbMessage.Name = "SbMessage";
            this.SbMessage.Size = new System.Drawing.Size(419, 23);
            this.SbMessage.StatusBarBackColor = System.Drawing.Color.Gainsboro;
            this.SbMessage.StatusBarForeColor = System.Drawing.Color.DimGray;
            this.SbMessage.TabIndex = 14;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(780, 330);
            this.Controls.Add(this.btnPase);
            this.Controls.Add(this.SbMessage);
            this.Controls.Add(this.PLoading);
            this.Controls.Add(this.BtnCloseLogin);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtSede);
            this.Controls.Add(this.txtUneg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.txtDkey);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Login";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnCloseLogin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialSingleLineTextField txtDkey;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private MaterialSkin.Controls.MaterialSingleLineTextField txtUneg;
        private MaterialSkin.Controls.MaterialSingleLineTextField txtSede;
        private MaterialSkin.Controls.MaterialSingleLineTextField txtName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox PicLogo;
        private System.Windows.Forms.PictureBox BtnCloseLogin;
        private System.Windows.Forms.ProgressBar PLoading;
        private Controls.StatusBar SbMessage;
        private System.Windows.Forms.Button btnPase;
    }
}