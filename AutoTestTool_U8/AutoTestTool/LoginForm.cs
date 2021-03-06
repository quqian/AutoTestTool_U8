﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTestTool
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void skinButton_Login_Click(object sender, EventArgs e)
        {
            if (textBox_UserName.Text == null)
            {
                MessageBox.Show("用户名不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox_LoginCode.Text == null)
            {
                MessageBox.Show("密码不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < ProcTestData.Account.Count; i++)
            {
                if (textBox_UserName.Text == ProcTestData.Account[i])
                {
                    if (textBox_LoginCode.Text == ProcTestData.Password[i])
                    {
                        ProcTestData.PresentAccount = ProcTestData.Account[i];
                        ProcTestData.WriteLastUserName(ProcTestData.lastLoginUserFile, ProcTestData.PresentAccount);

                        this.Hide();
                  //      if ((radioButton1.Checked == true) && (radioButton2.Checked == false))
                        {
                            MainForm fm = new MainForm();
                            fm.Show();
                        }
                //        else {
                //            U8_SubForms fm = new U8_SubForms();
                 //           fm.Show();
                  //      }
                        return;
                    }
                }
            }
            
            MessageBox.Show("请检查用户名与密码是否正确", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            textBox_UserName.Text = "";
            textBox_LoginCode.Text = "";
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
            //    radioButton1.Checked = true;
                ProcTestData.Account = ProcTestData.GetMysqlUserInfo("user");
                ProcTestData.Password = ProcTestData.GetMysqlUserInfo("password");

                if (ProcTestData.Account == null)
                {
                    MessageBox.Show("用户信息读取失败", "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
     

                textBox_UserName.Text = ProcTestData.ReadLastUserName(ProcTestData.lastLoginUserFile);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void textBox_LoginCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                skinButton_Login_Click(sender, e);
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
        //    radioButton2.Checked = false;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
        //    radioButton1.Checked = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel_LoginForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox_UserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_LoginCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void skinLabel2_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
