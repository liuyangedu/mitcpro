﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.Stock
{
    public partial class 原料入库管理 : Form
    {

        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        DataTable dt物资验收记录, dt物资请验单, dt检验记录, dt不合格品处理记录, dt取样记录;

        public 原料入库管理()
        {
            InitializeComponent();
            conn = new OleDbConnection(strConn);
            conn.Open();

            read物资验收记录Data();
            物资验收记录Bind();
            read物资请验单Data();
            物资请验单Bind();
            read检验记录Data();
            检验记录Bind();
            read不合格品记录Data();
            不合格品记录Bind();
            read取样记录Data();
            取样记录Bind();
            addOtherEventHandler();
        }

        private void btn增加物资验收记录_Click(object sender, EventArgs e)
        {
            物资验收记录 form = new 物资验收记录();
            form.Show();
        }

        void read物资验收记录Data()
        {

            OleDbDataAdapter da = new OleDbDataAdapter("select * from 物资验收记录", conn);
            dt物资验收记录 = new DataTable("物资验收记录");
            da.Fill(dt物资验收记录);
        }

        void 物资验收记录Bind()
        {
            dataGridView1.DataSource = dt物资验收记录;
        }

        void addOtherEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView3.AllowUserToAddRows = false;
            dataGridView4.AllowUserToAddRows = false;
            dataGridView5.AllowUserToAddRows = false;
            // TODO  加一个绑定完成事件，把需要审核的行标记
            dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellDoubleClick);
            dataGridView2.CellDoubleClick += dataGridView2_CellDoubleClick;
            dataGridView3.CellDoubleClick += dataGridView3_CellDoubleClick;
            dataGridView4.CellDoubleClick += dataGridView4_CellDoubleClick;
            dataGridView5.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView5_CellDoubleClick);

            // 隐藏ID等列
            dataGridView1.Columns["ID"].Visible = false;
            //dataGridView1.Columns["物资验收记录ID"].Visible = false;

            dataGridView2.Columns["ID"].Visible = false;
            dataGridView2.Columns["物资验收记录ID"].Visible = false;

            dataGridView3.Columns["ID"].Visible = false;
            dataGridView3.Columns["物资验收记录ID"].Visible = false;

            dataGridView4.Columns["ID"].Visible = false;
            dataGridView4.Columns["物资验收记录ID"].Visible = false;

            dataGridView5.Columns["ID"].Visible = false;
            dataGridView5.Columns["物资验收记录ID"].Visible = false;
        }

        void dataGridView5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView5.Rows[e.RowIndex].Cells[0].Value);
            取样记录 form = new 取样记录(id);
            form.Show();
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            物资验收记录 form = new 物资验收记录(id);
            form.Show();
        }

        void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView4.Rows[e.RowIndex].Cells[0].Value);
            不合格品处理记录 form = new 不合格品处理记录(id);
            form.Show();
        }

        void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells[0].Value);
            检验记录 form = new 检验记录(id);
            form.Show();
        }

        void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //双击 显示请验单
            int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value);
            物资请验单 form = new 物资请验单(id);
            form.Show();
        }

        private void btn读取_Click(object sender, EventArgs e)
        {
            read物资请验单Data();
            物资请验单Bind();
            
            
        }

        void read物资请验单Data()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 物资请验单", conn);
            dt物资请验单 = new DataTable("物资请验单");
            da.Fill(dt物资请验单);
        }

        void 物资请验单Bind()
        {
            dataGridView2.DataSource = dt物资请验单;
        }

        private void btn读取检验记录_Click(object sender, EventArgs e)
        {
            read检验记录Data();
            检验记录Bind();
        }

        void read检验记录Data()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 检验记录", conn);
            dt检验记录 = new DataTable("检验记录");
            da.Fill(dt检验记录);
        }

        void 检验记录Bind()
        {
            dataGridView3.DataSource = dt检验记录;
        }

       


        void read不合格品记录Data()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 不合格品处理记录", conn);
            dt不合格品处理记录 = new DataTable("不合格品处理记录");
            da.Fill(dt不合格品处理记录);
        }

        void 不合格品记录Bind()
        {
            dataGridView4.DataSource = dt不合格品处理记录;
        }

        private void btn读取不合格品记录_Click(object sender, EventArgs e)
        {
            read不合格品记录Data();
            不合格品记录Bind();
        }

        private void btn读取验收记录_Click(object sender, EventArgs e)
        {
            read物资验收记录Data();
            物资验收记录Bind();
        }

        void read取样记录Data()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 取样记录", conn);
            dt取样记录 = new DataTable("取样记录");
            da.Fill(dt取样记录);
        }

        void 取样记录Bind()
        {
            dataGridView5.DataSource = dt取样记录;
        }

        private void btn读取取样记录_Click(object sender, EventArgs e)
        {
            read取样记录Data();
            取样记录Bind();
        }
    }
}
