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

namespace mySystem.Process.CleanCut
{
    public partial class CleanCutMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;

        public CleanCutMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select instruction_code from production_instruction";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["instruction_code"]);  //下拉框获取生产指令
                    }
                }
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select production_instruction_code from production_instruction";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["production_instruction_code"]);
                    }
                }

            }
        }


        //定义各窗体变量
        CleanCut_Productrecord form1 = null;
        DailyRecord form2 = null;
        Instru form4 = null;
        CleanCut_CheckBeforePower form5 = null;
        Record_cleansite_cut form6 = null;



        private void A1Btn_Click(object sender, EventArgs e)
        {
            form4 = new Instru();             
            form4.ShowDialog();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            form6 = new Record_cleansite_cut(base.mainform);            
            form6.ShowDialog();
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            form5 = new CleanCut_CheckBeforePower(mainform);            
            form5.ShowDialog();
        }

        private void A4Btn_Click(object sender, EventArgs e)
        {
            form1 = new CleanCut_Productrecord(mainform);           
            form1.ShowDialog();
        }

        private void A5Btn_Click(object sender, EventArgs e)
        {
            form2 = new DailyRecord();
            form2.ShowDialog();
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            instruction = comboBox1.SelectedItem.ToString();
            Parameter.cleancutInstruction = instruction;
            String tblName = "production_instruction";
            List<String> queryCols = new List<String>(new String[] { "instruction_id" });
            List<String> whereCols = new List<String>(new String[] { "instruction_code" });
            List<Object> whereVals = new List<Object>(new Object[] { instruction });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            instruID = Convert.ToInt32(res[0][0]);
            Parameter.cleancutInstruID = instruID;

        }

        private void A6Btn_Click(object sender, EventArgs e)
        {

        }

        private void A7Btn_Click(object sender, EventArgs e)
        {

        }




    }
}
