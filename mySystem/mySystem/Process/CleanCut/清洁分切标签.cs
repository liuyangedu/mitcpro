﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace mySystem.Process.CleanCut
{
    public partial class 清洁分切标签 : Form
    {
        public 清洁分切标签()
        {
            InitializeComponent();
            fill_printer();
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                c打印机.Items.Add(sPrint);
            }
            c打印机.SelectedItem = print.PrinterSettings.PrinterName;
            getData();
        }

        private void cb白班_CheckedChanged(object sender, EventArgs e)
        {
            if (cb白班.Checked)
                cb夜班.Checked = false;
        }

        private void cb夜班_CheckedChanged(object sender, EventArgs e)
        {
            if (cb夜班.Checked)
                cb白班.Checked = false;
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);

        private void c打印机_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDefaultPrinter(c打印机.SelectedItem.ToString());
            
        }
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                c打印机.Items.Add(sPrint);
            }
            c打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            String sql = "select * from 标签 where 生产指令ID={0}";
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da = new OleDbDataAdapter(String.Format(sql, mySystem.Parameter.cleancutInstruID), mySystem.Parameter.connOle);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["生产指令"] = mySystem.Parameter.cleancutInstruction;
                    dr["生产指令ID"] = mySystem.Parameter.cleancutInstruID;
                    dr["膜代码"] = cb膜代码.Text;
                    dr["批号_卷号"] = tb批号.Text;
                    dr["合格数量米"] = tb米.Text;
                    dr["合格数量千克"] = tbKg.Text;
                    dr["原膜代码"] = cb原膜代码.Text;
                    dr["分切日期"] = dtp分切日期.Value.ToString("yyyy/MM/dd");
                    dr["分切班次"] = cb白班.Checked ? "白班" : "夜班";
                    dt.Rows.Add(dr);
                    da.Update(dt);
                }
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter(String.Format(sql, mySystem.Parameter.cleancutInstruID), mySystem.Parameter.conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["生产指令"] = mySystem.Parameter.cleancutInstruction;
                    dr["生产指令ID"] = mySystem.Parameter.cleancutInstruID;
                    dr["膜代码"] = cb膜代码.Text;
                    dr["批号_卷号"] = tb批号.Text;
                    dr["合格数量米"] = tb米.Text;
                    dr["合格数量千克"] = tbKg.Text;
                    dr["原膜代码"] = cb原膜代码.Text;
                    dr["分切日期"] = dtp分切日期.Value.ToString("yyyy/MM/dd");
                    dr["分切班次"] = cb白班.Checked ? "白班" : "夜班";
                    dt.Rows.Add(dr);
                    da.Update(dt);
                }
            }
           
            printLable();
            GC.Collect();
        }

        HashSet<String> hs原膜代码, hs膜代码;
        void getData()
        {
            System.Data.DataTable dt;
            dt = new System.Data.DataTable();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;

                da = new OleDbDataAdapter("select * from 清洁分切工序生产指令详细信息 where T生产指令表ID = " + mySystem.Parameter.cleancutInstruID, mySystem.Parameter.connOle);

                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;

                da = new SqlDataAdapter("select * from 清洁分切工序生产指令详细信息 where T生产指令表ID = " + mySystem.Parameter.cleancutInstruID, mySystem.Parameter.conn);

                da.Fill(dt);
            }
            
            hs原膜代码 = new HashSet<string>();
            foreach (DataRow dr in dt.Rows)
            {
                hs原膜代码.Add(dr["清洁前产品代码"].ToString());
            }
            foreach (String s in hs原膜代码)
            {
                cb原膜代码.Items.Add(s);
            }
        }

        private void cb原膜代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str原膜代码 = cb原膜代码.SelectedItem.ToString();
            System.Data.DataTable dt;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;

                da = new OleDbDataAdapter("select * from 清洁分切工序生产指令详细信息 where T生产指令表ID = " + mySystem.Parameter.cleancutInstruID + " and 清洁前产品代码 = '" + str原膜代码 + "'", mySystem.Parameter.connOle);
                dt = new System.Data.DataTable();
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;

                da = new SqlDataAdapter("select * from 清洁分切工序生产指令详细信息 where T生产指令表ID = " + mySystem.Parameter.cleancutInstruID + " and 清洁前产品代码 = '" + str原膜代码 + "'", mySystem.Parameter.conn);
                dt = new System.Data.DataTable();
                da.Fill(dt);
            }
            
            cb膜代码.Text = "";
            cb膜代码.Items.Clear();
            hs膜代码 = new HashSet<string>();
            foreach (DataRow dr in dt.Rows)
            {
                hs膜代码.Add(dr["清洁后产品代码"].ToString());
            }
            foreach (String s in hs膜代码)
            {
                cb膜代码.Items.Add(s);
            }
        }

        public static void printLable(int id)
        {
            String sql = "select * from 标签 where ID={0}";
            System.Data.DataTable dt = new System.Data.DataTable();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da = new OleDbDataAdapter(String.Format(sql, id), mySystem.Parameter.connOle);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);

                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter(String.Format(sql, id), mySystem.Parameter.conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);

                da.Fill(dt);
            }
           
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("无法找到标签信息");
                return;
            }
            DataRow dr = dt.Rows[0];
            string path = Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            wb = oXL.Workbooks.Open(path + @"/../../xls/cleancut/7 标签-清洁分切.xlsx");
            _Worksheet my = wb.Worksheets[wb.Worksheets.Count];

            //dr["生产指令"] = mySystem.Parameter.cleancutInstruction;
            //dr["生产指令ID"] = mySystem.Parameter.cleancutInstruID;
            //dr["膜代码"] = cb膜代码.Text;
            //dr["批号-卷号 "] = tb批号.Text;
            //dr["合格数量米"] = tb米.Text;
            //dr["合格数量千克"] = tbKg.Text;
            //dr["原膜代码"] = cb原膜代码.Text;
            //dr["分切日期"] = dtp分切日期.Value.ToString("yyyy/MM/dd");
            //dr["分切班次"] = cb白班.Checked ? "白班" : "夜班";

            my.Select();
            my.Cells[2, 2].Value = dr["膜代码"];
            my.Cells[3, 2].Value = dr["批号_卷号"];
            my.Cells[4, 2].Value = dr["合格数量米"] + "米；  " + dr["合格数量千克"] + "Kg";
            my.Cells[5, 2].Value = dr["原膜代码"];
            my.Cells[6, 2].Value = String.Format("{0} {1}", Convert.ToDateTime(dr["分切日期"]).ToString("yyyy/MM/dd"),
                dr["分切班次"].ToString() == "白班" ? "白班☑ 夜班□" : "白班□ 夜班☑");

            my = wb.Worksheets[1];
            my.Select();
            oXL.Visible = false;
            my.PrintOut();
            // 关闭文件，false表示不保存
            wb.Close(false);
            // 关闭Excel进程
            oXL.Quit();
            // 释放COM资源
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
            wb = null;
            oXL = null;
        }

        void printLable()
        {
            string path = Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            wb = oXL.Workbooks.Open(path + @"/../../xls/cleancut/7 标签-清洁分切.xlsx");
            _Worksheet my = wb.Worksheets[wb.Worksheets.Count];

            my.Select();
            my.Cells[2, 2].Value = cb膜代码.Text;
            my.Cells[3, 2].Value = tb批号.Text;
            my.Cells[4, 2].Value = tb米.Text + "米；  " + tbKg.Text + "Kg";
            my.Cells[5, 2].Value = cb原膜代码.Text;
            my.Cells[6, 2].Value = String.Format("{0} {1}", dtp分切日期.Value.ToString("yyyy/MM/dd"),
                cb白班.Checked ? "白班☑ 夜班□" : "白班□ 夜班☑"); 

            my = wb.Worksheets[1];
            my.Select();
            oXL.Visible = false;
            my.PrintOut();
            // 关闭文件，false表示不保存
            wb.Close(false);
            // 关闭Excel进程
            oXL.Quit();
            // 释放COM资源
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
            wb = null;
            oXL = null;
        }

        


    }
}
