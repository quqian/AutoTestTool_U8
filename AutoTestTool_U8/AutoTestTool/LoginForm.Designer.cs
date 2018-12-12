namespace AutoTestTool
{
    partial class LoginForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.textBox_UserName = new System.Windows.Forms.TextBox();
            this.textBox_LoginCode = new System.Windows.Forms.TextBox();
            this.skinButton_Login = new CCWin.SkinControl.SkinButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinLabel1
            // 
            this.skinLabel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.ForeColor = System.Drawing.Color.Black;
            this.skinLabel1.Location = new System.Drawing.Point(145, 110);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(91, 29);
            this.skinLabel1.TabIndex = 0;
            this.skinLabel1.Text = "用户名:";
            this.skinLabel1.Click += new System.EventHandler(this.skinLabel1_Click);
            // 
            // skinLabel2
            // 
            this.skinLabel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.skinLabel2.ForeColorSuit = true;
            this.skinLabel2.Location = new System.Drawing.Point(169, 154);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(67, 29);
            this.skinLabel2.TabIndex = 1;
            this.skinLabel2.Text = "密码:";
            this.skinLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.skinLabel2.Click += new System.EventHandler(this.skinLabel2_Click);
            // 
            // textBox_UserName
            // 
            this.textBox_UserName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox_UserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_UserName.Location = new System.Drawing.Point(263, 105);
            this.textBox_UserName.MaxLength = 16;
            this.textBox_UserName.Name = "textBox_UserName";
            this.textBox_UserName.Size = new System.Drawing.Size(210, 34);
            this.textBox_UserName.TabIndex = 2;
            this.textBox_UserName.TextChanged += new System.EventHandler(this.textBox_UserName_TextChanged);
            // 
            // textBox_LoginCode
            // 
            this.textBox_LoginCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox_LoginCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_LoginCode.Location = new System.Drawing.Point(263, 151);
            this.textBox_LoginCode.MaxLength = 16;
            this.textBox_LoginCode.Name = "textBox_LoginCode";
            this.textBox_LoginCode.PasswordChar = '*';
            this.textBox_LoginCode.Size = new System.Drawing.Size(210, 34);
            this.textBox_LoginCode.TabIndex = 3;
            this.textBox_LoginCode.Text = "123456";
            this.textBox_LoginCode.TextChanged += new System.EventHandler(this.textBox_LoginCode_TextChanged);
            this.textBox_LoginCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_LoginCode_KeyPress);
            // 
            // skinButton_Login
            // 
            this.skinButton_Login.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.skinButton_Login.BackColor = System.Drawing.Color.Transparent;
            this.skinButton_Login.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton_Login.DownBack = null;
            this.skinButton_Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton_Login.ForeColor = System.Drawing.Color.Fuchsia;
            this.skinButton_Login.Location = new System.Drawing.Point(263, 226);
            this.skinButton_Login.MouseBack = null;
            this.skinButton_Login.Name = "skinButton_Login";
            this.skinButton_Login.NormlBack = null;
            this.skinButton_Login.Size = new System.Drawing.Size(210, 43);
            this.skinButton_Login.TabIndex = 4;
            this.skinButton_Login.Text = "登录";
            this.skinButton_Login.UseVisualStyleBackColor = false;
            this.skinButton_Login.Click += new System.EventHandler(this.skinButton_Login_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_UserName);
            this.groupBox1.Controls.Add(this.skinButton_Login);
            this.groupBox1.Controls.Add(this.textBox_LoginCode);
            this.groupBox1.Controls.Add(this.skinLabel2);
            this.groupBox1.Controls.Add(this.skinLabel1);
            this.groupBox1.ForeColor = System.Drawing.Color.Transparent;
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(755, 373);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(292, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 36);
            this.label1.TabIndex = 5;
            this.label1.Text = "登陆页面";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(779, 397);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "LoginForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "用户登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private System.Windows.Forms.TextBox textBox_UserName;
        private System.Windows.Forms.TextBox textBox_LoginCode;
        private CCWin.SkinControl.SkinButton skinButton_Login;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
    }
}

