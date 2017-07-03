﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem
{
    public partial class CheckForm : BaseForm
    {
        bool isSqlOk;
        public int userID;
        public string userName;
        public string opinion;
        public bool ischeckOk; //审核是否通过
        public DateTime time;

        BaseForm bs;
        public CheckForm(BaseForm mainform)
        {
            bs = mainform;
            InitializeComponent();
            Parameter.InitConnUser();

        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (CheckerIDTextBox.Text.Trim() == "" || CheckerPWTextBox.Text.Trim() == "")
            {
                MessageBox.Show("提示：请输入审核员ID和密码！", "警告");
                CheckerIDTextBox.Focus();
            }
            else
            {
                String myID = this.CheckerIDTextBox.Text;
                String mypassword = this.CheckerPWTextBox.Text;

                if (isSqlOk)
                {
                    userID = CheckUser(Parameter.connUser, myID, mypassword);
                }
                else
                {
                    userID = CheckUser(Parameter.connOleUser, myID, mypassword);
                }
                opinion = OpTextBox.Text;
                ischeckOk = true;
                time = DateTime.Now;
                bs.CheckResult();
                this.Dispose();

            }

        }




        private void NotOKBtn_Click(object sender, EventArgs e)
        {
            if (CheckerIDTextBox.Text.Trim() == "" || CheckerPWTextBox.Text.Trim() == "")
            {
                MessageBox.Show("提示：请输入审核员ID和密码！", "警告");
                CheckerIDTextBox.Focus();
            }
            else
            {
                String myID = this.CheckerIDTextBox.Text;
                String mypassword = this.CheckerPWTextBox.Text;

                if (isSqlOk)
                {
                    userID = CheckUser(Parameter.connUser, myID, mypassword);
                }
                else
                {
                    userID = CheckUser(Parameter.connOleUser, myID, mypassword);
                }

                opinion = OpTextBox.Text;
                ischeckOk = false;
                time = DateTime.Now;
                bs.CheckResult();
                this.Dispose();
            }
        }


        private int CheckUser(SqlConnection Connection, string ID, string password)
        {
            string searchsql = "select * from user_aoxing where user_id='" + ID + "'";
            SqlCommand comm = new SqlCommand(searchsql, Connection);
            SqlDataReader sdr = comm.ExecuteReader();//执行查询
            if (sdr.Read())  //如果该用户存在
            {
                if (sdr.GetString(5).Trim() == password) //密码正确
                {
                    //MessageBox.Show("登录成功！", "提示");
                    userID = sdr.GetInt32(3);
                    userName = sdr.GetString(4);
                    comm.Dispose();
                    sdr.Close();
                    sdr.Dispose();
                    //this.Hide();
                    return userID;
                }
                else         //密码错误
                {
                    MessageBox.Show("您输入的密码有误，请重新输入！", "警告");
                    this.CheckerPWTextBox.Text = null;
                    this.CheckerPWTextBox.Focus();
                    sdr.Close();
                    sdr.Dispose();
                    return 0;
                }
            }
            else
            {
                MessageBox.Show("该用户不存在，请检查后重新输入！", "警告");
                this.CheckerIDTextBox.Text = null;
                this.CheckerPWTextBox.Text = null;
                CheckerIDTextBox.Focus();
                sdr.Close();
                sdr.Dispose();
                return 0;

            }


        }

        private int CheckUser(OleDbConnection Connection, string ID, string password)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Connection;
            comm.CommandText = "select * from user_aoxing where user_id= @ID";
            comm.Parameters.AddWithValue("@ID", ID);

            OleDbDataReader sdr = comm.ExecuteReader();//执行查询
            if (sdr.Read())  //如果该用户存在
            {
                if (sdr.GetString(5).Trim() == password) //密码正确
                {
                    //MessageBox.Show("登录成功！", "提示");
                    userID = sdr.GetInt32(3);
                    userName = sdr.GetString(4);
                    comm.Dispose();
                    sdr.Close();
                    sdr.Dispose();
                    //this.Hide();
                    return userID;
                }
                else         //密码错误
                {
                    MessageBox.Show("您输入的密码有误，请重新输入！", "警告");
                    this.CheckerPWTextBox.Text = null;
                    this.CheckerPWTextBox.Focus();
                    sdr.Close();
                    sdr.Dispose();
                    return 0;
                }
            }
            else
            {
                MessageBox.Show("该用户不存在，请检查后重新输入！", "警告");
                this.CheckerIDTextBox.Text = null;
                this.CheckerPWTextBox.Text = null;
                CheckerIDTextBox.Focus();
                sdr.Close();
                sdr.Dispose();
                return 0;

            }
        }



        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckerIDTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CheckerPWTextBox.Focus();
            }
        }

        private void CheckerPWTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                OKBtn.Focus();
            }
        }





    }
}
