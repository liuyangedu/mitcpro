﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.OleDb;

namespace mySystem
{
    /// <summary>
    ///  吹膜生产日报表
    /// </summary>
    public partial class ProdctDaily_extrus : mySystem.BaseForm
    {
        public ProdctDaily_extrus(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            Init();
            //connToServer();
            queryAndShow();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //自定义变量
        private bool isOk;//是否连接数据库成功
        string prodCode;//生产指令
        string strCon;
        string sql;

        SqlConnection conn;
        DataTable dt;
        DataRow[] dr;
        

        //针对查询
        DateTime date1;//起始时间
        DateTime date2;//结束时间
        string ddan;//订单号
        string instr;//生产指令
        string writer;//填报人
        string checker;//审核人


        //自定义函数
        private void Init()
        {
            strCon = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            sql = "select * from test_2";
            prodCode = "0x34222fds";
            isOk = false;
            lastRow= new List<object[]>();

            dataGridView1.Font = new Font("宋体",12);

            comboBox1.Items.Add("(空)".Trim());
            comboBox2.Items.Add("(空)".Trim());
            comboBox3.Items.Add("(空)".Trim());
            comboBox4.Items.Add("(空)".Trim());

            comboBox1.Text = "(空)".Trim();
            comboBox2.Text = "(空)".Trim();
            comboBox3.Text = "(空)".Trim();
            comboBox4.Text = "(空)".Trim();
            date1 = DateTime.Parse("2016/10/20");
        }
        /*仅用来来测试，实际早已在上一步登陆*/
        private void connToServer()
        {             
            conn = new SqlConnection(strCon);
            conn.Open();
            isOk = true;
        }
        private void queryAndShow()
        {
            //if (!isOk)
            //{
            //    MessageBox.Show("连接数据库失败", "error");
            //    return;
            //}

            //显示生产指令
            //label3.Text = prodCode;

            //查询
            if (mainform.isSqlOk)
            {
                
                SqlCommand comm = new SqlCommand(sql, mainform.conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);

                //DataSet ds = new DataSet();
                dt = new DataTable();
                //da.Fill(dt);

                ///添加一列
                DataColumn col = new DataColumn("编号");
                dt.Columns.Add(col);
                da.Fill(dt);
            }
            else
            {
                string sql = "select s6_production_date,s6_flight,production_instruction_id,product_batch_id,s6_mojuan_length,s6_mojuan_weight from extrusion_s6_production_check";
                OleDbCommand comm = new OleDbCommand(sql, mainform.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                //DataSet ds = new DataSet();
                dt = new DataTable();
                //da.Fill(dt);

                ///添加一列
                DataColumn col = new DataColumn("编号");
                dt.Columns.Add(col);
                da.Fill(dt);
            }

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                dt.Rows[row][0] = (row+1).ToString();
            }


            //统计列值
            
            
            //DataView dv = dt.DefaultView;
            //DataTable tempdt=dv.ToTable(true, "订单");

            //for (int i = 0; i < tempdt.Rows.Count; i++)
            //{
            //    comboBox1.Items.Add(tempdt.Rows[i]["订单"].ToString().Trim());
            //}
            //tempdt = dv.ToTable(true, "生产指令");
            //for (int i = 0; i < tempdt.Rows.Count; i++)
            //{
            //    comboBox2.Items.Add(tempdt.Rows[i]["生产指令"].ToString().Trim());
            //}
            //tempdt = dv.ToTable(true, "填报人");
            //for (int i = 0; i < tempdt.Rows.Count; i++)
            //{
            //    comboBox3.Items.Add(tempdt.Rows[i]["填报人"].ToString().Trim());
            //}
            //tempdt = dv.ToTable(true, "复核人");
            //for (int i = 0; i < tempdt.Rows.Count; i++)
            //{
            //    comboBox4.Items.Add(tempdt.Rows[i]["复核人"].ToString().Trim());
            //}

            ////添加合计
            //DataRow row1;
            //row1=dt.NewRow();
            //row1[0] = "合计";
            //row1[7] = dt.Compute("sum("+dt.Columns[7].ColumnName+")", "TRUE");
            //row1[8] = dt.Compute("sum(" + dt.Columns[8].ColumnName + ")", "TRUE");
            //row1[9] = dt.Compute("sum(" + dt.Columns[9].ColumnName + ")", "TRUE");
            //dt.Rows.Add(row1);

            dataGridView1.DataSource = dt;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //System.Console.WriteLine("**************************************************************************8");
            //if (e.RowIndex >= 0 || dataGridView1.Rows.Count == 0)
            //    return;

            //if (lastRow.Count == 0)
            //{
            //    colindex = e.ColumnIndex;
            //    int index = dataGridView1.Rows.Count - 1;
            //    System.Console.WriteLine(colindex.ToString());
            //    System.Console.WriteLine(index.ToString());
            //    lastRow.Add(((DataTable)dataGridView1.DataSource).Rows[index].ItemArray);
            //    System.Console.WriteLine(lastRow[0].ToString());
            //    dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.Rows.Count - 1]);

            //    DataTable dt = ((DataTable)dataGridView1.DataSource);
            //    DataView dv = dt.DefaultView;
            //    dv.Sort = dt.Columns[colindex].ColumnName;
            //    dt = dv.ToTable();
            //    dt.Rows.Add(lastRow[0]);
            //    lastRow.Clear();
            //    dataGridView1.DataSource = dt;
            //}
        }
        List<object[]> lastRow;
        int colindex;

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            //date1 = dateTimePicker1.Value.Date;
            ////System.Console.WriteLine(date1);
            //date2 = dateTimePicker2.Value.Date;
            //System.Console.WriteLine(date2);
            //TimeSpan delt = date2 - date1;
            //if (delt.TotalDays < 0)
            //{
            //    MessageBox.Show("起止时间有误，请重新输入");
            //    return;
            //}
            ////DataTable temp;
            ////dataGridView1.Rows.Clear();//清空
            //string select="生产时间>="+"'"+date1+"'"+" and "+"生产时间<"+"'"+date2+"'";
            //System.Console.WriteLine(select);
            //dr = dt.Select(select);
            
            //DataTable temp = dr.CopyToDataTable();
            //dataGridView1.DataSource = temp;

            ////DataRow[] tempdr = temp.Select("distict 订单");
            //comboBox1.Items.Clear();
            //for(int i=0;i<dr.Length;i++)
            //{
            //    //tempdr[i][2]
            //    comboBox1.Items.Add(dr[i][2]);
            //}
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string s=comboBox2.SelectedValue.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            date1 = dateTimePicker1.Value.Date;
            //System.Console.WriteLine(date1);
            date2 = dateTimePicker2.Value.Date;
            System.Console.WriteLine(date2);
            TimeSpan delt = date2 - date1;
            if (delt.TotalDays < 0)
            {
                MessageBox.Show("起止时间有误，请重新输入");
                return;
            }

            ddan = comboBox1.Text.Trim();
            instr = comboBox2.Text.Trim();
            writer = comboBox3.Text.Trim();
            checker = comboBox4.Text.Trim();

            //删除合计行
            dt.Rows.RemoveAt(dt.Rows.Count - 1);

            string sql = "生产时间>=" + "'" + date1 + "'" + " and " + "生产时间<=" + "'" + date2 + "'";

            if (ddan != "(空)")
                sql += " and " + "订单 like" + "'%" + ddan + "%'";
            if (instr != "(空)")
                sql += " and " + "生产指令 like" + "'%" + instr + "%'";

            if (writer != "(空)")
                sql += " and " + "填报人 like" + "'%" + writer + "%'";
            if (checker != "(空)")
                sql += " and " + "复核人 like" + "'%" + checker + "%'";


            dr = dt.Select(sql);
            //添加合计行
            DataRow rowtemp;
            rowtemp = dt.NewRow();
            rowtemp[0] = "合计";
            rowtemp[7] = dt.Compute("sum(" + dt.Columns[7].ColumnName + ")", "TRUE");
            rowtemp[8] = dt.Compute("sum(" + dt.Columns[8].ColumnName + ")", "TRUE");
            rowtemp[9] = dt.Compute("sum(" + dt.Columns[9].ColumnName + ")", "TRUE");
            dt.Rows.Add(rowtemp);

            if (dr.Length == 0)
            {               
                dataGridView1.DataSource = null;
                return;
            }
                
            DataTable temp = dr.CopyToDataTable();
            //改变序号
            for (int row = 0; row < temp.Rows.Count; row++)
            {
                temp.Rows[row][0] = (row + 1).ToString();
            }
            DataRow row1;
            row1 = temp.NewRow();
            row1[0] = "合计";
            row1[7] = temp.Compute("sum(" + dt.Columns[7].ColumnName + ")", "TRUE");
            row1[8] = temp.Compute("sum(" + dt.Columns[8].ColumnName + ")", "TRUE");
            row1[9] = temp.Compute("sum(" + dt.Columns[9].ColumnName + ")", "TRUE");
            temp.Rows.Add(row1);

            dataGridView1.DataSource = temp;

        }

    }
}
