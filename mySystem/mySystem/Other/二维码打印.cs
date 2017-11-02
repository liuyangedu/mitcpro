﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OleDb;

namespace mySystem.Other
{
    public partial class 二维码打印 : Form
    {
        public static 二维码打印 create(String daima, string pihao)
        {
            二维码打印 ret = new 二维码打印();
            ret.tb产品代码.Text = daima;
            ret.tb产品批号.Text = pihao;
            ret.tb二维码.ReadOnly = true;
            return ret;
        }

        string strConnect;
        OleDbConnection conn;

        public 二维码打印()
        {
            InitializeComponent();
            fill_printer();
            strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
            conn = new OleDbConnection(strConnect);
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cmb打印机.Items.Add(sPrint);
            }
            cmb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }

        private void btn关闭_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            // TODO
            // 打印一张二维码，就在右边扫描一次，所以不用保存
        }

        private void btn生成二维码_Click(object sender, EventArgs e)
        {
            string prefix = tb产品代码.Text + "@" + tb产品批号.Text + "@";
            int num = 1;
            string sql = "select top 1 二维码 from 二维码信息 where 二维码 like '%{0}%' order by ID DESC";
            OleDbDataAdapter da = new OleDbDataAdapter(String.Format(sql, prefix), conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                String[] s = dt.Rows[0]["二维码"].ToString().Split('@');
                num = Convert.ToInt32(s[2]);
                num++;
            }
            tb二维码.Text = prefix + num.ToString("D3");
        }
    }
}
