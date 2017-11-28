﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using mySystem.Process.CleanCut;
using mySystem.Process.Bag.CS;

namespace mySystem.Process.Bag
{
    public partial class CSBagMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;

        public CSBagMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            comboInit();
            InitBtn();
        }

        //下拉框获取生产指令
        public void comboInit()
        {
            HashSet<String> hash = new HashSet<String>();
            if (Parameter.isSqlOk)
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = mySystem.Parameter.conn;
                comm.CommandText = "select * from 生产指令 where 状态 = 2 ";
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        hash.Add(reader["生产指令编号"].ToString());
                    }
                    foreach (String code in hash)
                    {
                        comboBox1.Items.Add(code);
                    }

                }
                comm.Dispose();
            }
            else
            {

            }
            //默认下拉框选最后一个
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                Parameter.csbagInstruction = comboBox1.SelectedItem.ToString();
                String tblName = "生产指令";
                List<String> queryCols = new List<String>(new String[] { "ID" });
                List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
                List<Object> whereVals = new List<Object>(new Object[] { Parameter.csbagInstruction });
                List<List<Object>> res;
                if (mySystem.Parameter.isSqlOk)
                {
                    res = Utility.selectAccess(Parameter.conn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                }
                else
                {
                    res = Utility.selectAccess(mySystem.Parameter.conn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                }
                Parameter.csbagInstruID = Convert.ToInt32(res[0][0]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            instruction = comboBox1.SelectedItem.ToString();
            Parameter.csbagInstruction = instruction;
            String tblName = "生产指令";
            List<String> queryCols = new List<String>(new String[] { "ID" });
            List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
            List<Object> whereVals = new List<Object>(new Object[] { instruction });
            List<List<Object>> res;
            if (mySystem.Parameter.isSqlOk)
            {
                res = Utility.selectAccess(Parameter.conn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            }
            else
            {
                res = Utility.selectAccess(mySystem.Parameter.conn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            }
            instruID = Convert.ToInt32(res[0][0]);
            Parameter.csbagInstruID = instruID;
            InitBtn();

        }

        //初始化按钮状态
        public void InitBtn()
        {
            if (comboBox1.SelectedIndex == -1)
            { otherBtnInit(false); }
            else
            { otherBtnInit(true); }
        }

        private void otherBtnInit(bool b)
        {
            Btn领料记录.Enabled = b;
            Btn内包装.Enabled = b;
            Btn日报表.Enabled = b;
            Btn内标签.Enabled = b;
            Btn外观及检验.Enabled = b;
            Btn开机确认.Enabled = b;
            Btn运行记录.Enabled = b;
            Btn清场.Enabled = b;
            Btn批生产.Enabled = b;
            Btn热合强度.Enabled = b;
            Btn外包装.Enabled = b;
            Btn洁净.Enabled = b;
            Btn退料.Enabled = b;
            Btn交接班.Enabled = b;
            Btn结束.Enabled = b;
            Btn外标签.Enabled = b;
        }


        private void A1Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "CS制袋生产领料记录");
            if (b)
            {
                MaterialRecord form1 = new MaterialRecord(mainform);
                form1.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品内包装记录");
            if (b)
            {
                CSBag_InnerPackaging form2 = new CSBag_InnerPackaging(mainform);
                form2.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            Chart_daily_cs form3 = new Chart_daily_cs(mainform);           
            form3.ShowDialog();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "CS制袋生产指令");
            if (b)
            {
                CS.CS制袋生产指令 form = new CS.CS制袋生产指令(mainform);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "制袋机开机前确认表");
            if (b)
            {
                CSBag_CheckBeforePower form6 = new CSBag_CheckBeforePower(mainform);
                form6.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "制袋机运行记录");
            if (b)
            {
                RunningRecord form7 = new RunningRecord(mainform);
                form7.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "清场记录");
            if (b)
            {
                CS.清场记录 myform = new CS.清场记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void A4Btn_Click(object sender, EventArgs e)
        {
            标签 form = new 标签(true);
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "制袋工序批生产记录");
            if (b)
            {
                Record_batch_bag form9 = new Record_batch_bag(mainform);
                form9.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
                       
        }

        private void Btn外观及检验_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品外观和尺寸检验记录");
            if (b)
            {
                CS.产品外观和尺寸检验记录 myform = new 产品外观和尺寸检验记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void Btn热合强度_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品热合强度检验记录");
            if (b)
            {
                CS.产品热合强度检验记录 myform = new 产品热合强度检验记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
            
        }

        private void Btn外包装_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品外包装记录表");
            if (b)
            {
                CS.CS产品外包装记录 myform = new CS产品外包装记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
            
        }

        private void Btn退料_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "生产退料记录表");
            if (b)
            {
                CS.CS生产退料记录 myform = new CS生产退料记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
            
        }

        private void Btn洁净_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "洁净区温湿度记录表");
            if (b)
            {
                CS.CS洁净区温湿度记录 myform = new CS洁净区温湿度记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
            
        }

        private void Btn交接班_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "岗位交接班记录");
            if (b)
            {
                CS.HandOver myform = new CS.HandOver(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
            

        }

        private void Btn结束_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否确认结束工序？", "提示", MessageBoxButtons.YesNo))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from 生产指令 where ID="+mySystem.Parameter.csbagInstruID, mySystem.Parameter.conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dt.Rows[0]["状态"] = 4;
                da.Update(dt);
            }
        }



        //判断是否能查看
        private Boolean checkUser(String user, int role, String tblName)
        {
            Boolean b = false;
            String[] name操作员 = null;
            String[] name审核员 = null;
            SqlCommand comm = new SqlCommand();
            comm.Connection = mySystem.Parameter.conn;
            comm.CommandText = "select * from 用户权限 where 步骤 = " + "'" + tblName + "' ";
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                name操作员 = reader["操作员"].ToString().Split("，,".ToCharArray());
                name审核员 = reader["审核员"].ToString().Split("，,".ToCharArray());
            }

            if (role == 3)
            {
                return b = true;
            }
            else
            {
                foreach (String name in name操作员)
                {
                    if (user == name)
                    { return b = true; }
                }
                foreach (String name in name审核员)
                {
                    if (user == name)
                    { return b = true; }
                }

            }
            return b = false;
        }

        private void Btn外标签_Click(object sender, EventArgs e)
        {
            标签 form = new 标签(false);
            form.Show();
        }

        private void btn生产领料申请单_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "生产领料申请单表");
            if (b)
            {
                CS生产领料申请单 form生产领料申请单 = new CS生产领料申请单(mainform);
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
        }

    }
}
