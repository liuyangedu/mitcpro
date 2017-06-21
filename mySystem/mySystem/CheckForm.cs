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
        SqlConnection conn = null;
        OleDbConnection connOle = null;
        bool isSqlOk;
        public int userID;
        public string opinion;
        

        public CheckForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;
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
                    userID = CheckUser(conn, myID, mypassword);
                }
                else
                {
                    userID = CheckUser(connOle, myID, mypassword);
                }   
          
            }
            opinion = OpTextBox.Text;
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
                    userID = CheckUser(conn, myID, mypassword);
                }
                else
                {
                    userID = CheckUser(connOle, myID, mypassword);
                }  

            }
            opinion = OpTextBox.Text;
        }

        private int CheckUser(SqlConnection Connection, string ID, string password)
        {
            string searchsql = "select * from [user] where user_id='" + ID + "'and user_password='" + password + "'";
            SqlCommand comm = new SqlCommand(searchsql, Connection);
            SqlDataReader sdr = comm.ExecuteReader();//执行查询
            if (sdr.Read())  //如果该用户存在
            {
                userID = sdr.GetInt32(3);
                comm.Dispose();
                sdr.Close();
                sdr.Dispose();
                this.Hide();
                return userID;

            }
            else
            {
                MessageBox.Show("输入登录信息不正确，请重新输入！", "警告");
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
            comm.CommandText = "select * from [user] where user_id= @ID and user_password= @password";
            comm.Parameters.AddWithValue("@ID", ID);
            comm.Parameters.AddWithValue("@password", password);

            OleDbDataReader sdr = comm.ExecuteReader();//执行查询
            if (sdr.Read())  //如果该用户存在
            {
                userID = sdr.GetInt32(3);
                comm.Dispose();
                sdr.Close();
                sdr.Dispose();
                this.Hide();
                return userID;

            }
            else
            {
                MessageBox.Show("输入登录信息不正确，请重新输入！", "警告");
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
