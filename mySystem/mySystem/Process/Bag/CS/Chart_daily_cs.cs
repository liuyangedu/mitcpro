﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace mySystem.Process.CleanCut
{
    public partial class Chart_daily_cs : Form
    {
        private OleDbConnection connOle = Parameter.connOle;
        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;

        private DataTable dt日报表详细信息, dt日报表, dt生产指令, dt生产指令详细信息, dt领料, dt领料详细信息, dt内包装;
        private BindingSource bs日报表详细信息, bs日报表, bs生产指令, bs生产指令详细信息, bs领料, bs领料详细信息, bs内包装;
        private OleDbDataAdapter da日报表详细信息, da日报表, da生产指令, da生产指令详细信息, da领料, da领料详细信息, da内包装;
        private OleDbCommandBuilder cb日报表详细信息, cb日报表, cb生产指令, cb生产指令详细信息, cb领料, cb领料详细信息, cb内包装;
       
        int ID = mySystem.Parameter.csbagInstruID;
        string strID = mySystem.Parameter.csbagInstruction;

        public Chart_daily_cs()
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();

            readOuterData();
            outerBind();
            readInnerData();
            innerBind();

            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();
        }

        private void Chart_daily_cs_Load(object sender, EventArgs e)
        {

        }

        // 获取其他需要的数据，比如产品代码，产生废品原因等
        private void getOtherData()
        {
            //读取生产指令外表
            da生产指令 = new OleDbDataAdapter("select * from 生产指令 where ID="+ID, mySystem.Parameter.connOle);
            cb生产指令 = new OleDbCommandBuilder(da生产指令);
            dt生产指令 = new DataTable("生产指令");
            bs生产指令 = new BindingSource();
            da生产指令.Fill(dt生产指令);
            DataTable dt生产指令所需信息 = dt生产指令.DefaultView.ToTable(false, new string[] {"ID", "计划生产日期", "生产指令编号" });

            //读取生产指令内表
            da生产指令详细信息 = new OleDbDataAdapter("select * from 生产指令详细信息", mySystem.Parameter.connOle);
            cb生产指令详细信息 = new OleDbCommandBuilder(da生产指令);
            dt生产指令详细信息 = new DataTable("生产指令详细信息");
            bs生产指令详细信息 = new BindingSource();
            da生产指令详细信息.Fill(dt生产指令详细信息);
            DataTable dt生产指令所需信息详细 = dt生产指令详细信息.DefaultView.ToTable(false, new string[] { "产品代码", "产品批号","客户或订单号" });

            //读取领料量外表
            da领料 = new OleDbDataAdapter("select * from CS制袋领料记录 where 生产指令ID=" + ID, mySystem.Parameter.connOle);
            cb领料 = new OleDbCommandBuilder(da领料);
            dt领料 = new DataTable("领料");
            bs领料 = new BindingSource();
            da领料.Fill(dt领料);
            DataTable dt领料所需信息 = dt领料.DefaultView.ToTable(false, new string[] { "ID"});

            //读取领料量内表
            da领料详细信息 = new OleDbDataAdapter("select * from CS制袋领料记录详细记录",mySystem.Parameter.connOle);
            cb领料详细信息 = new OleDbCommandBuilder(da领料详细信息);
            dt领料详细信息 = new DataTable("领料详细信息");
            bs领料详细信息 = new BindingSource();
            da领料详细信息.Fill(dt领料详细信息);
            DataTable dt领料所需信息详细 = dt领料详细信息.DefaultView.ToTable(false, new string[] { "TCS制袋领料记录ID","物料简称","使用数量C"});

            //读取内包装
            da内包装 = new OleDbDataAdapter("select * from 产品内包装记录 where 生产指令ID="+ID, mySystem.Parameter.connOle);
            cb内包装 = new OleDbCommandBuilder(da内包装);
            dt内包装 = new DataTable("内包装");
            bs内包装 = new BindingSource();
            da内包装.Fill(dt内包装);
            DataTable dt内包装所需信息 = dt内包装.DefaultView.ToTable(false, new string[] {"ID", "产品数量只数合计B"});

            MessageBox.Show(strID);

            //添加打印机
            fill_printer();
        }
        // 根据条件从数据库中读取一行外表的数据
        private void readOuterData()
        {
            da日报表 = new OleDbDataAdapter("select * from CS制袋日报表", mySystem.Parameter.connOle);
            cb日报表 = new OleDbCommandBuilder(da日报表);
            dt日报表 = new DataTable("CS制袋日报表");
            bs日报表 = new BindingSource();
            da日报表.Fill(dt日报表);
        }
        // 外表和控件的绑定
        private void outerBind()
        {
            bs日报表.DataSource = dt日报表;

        }
        // 根据条件从数据库中读取多行内表数据
        private void readInnerData()
        {
            //            String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
            //                                Data Source=../../database/miejun.mdb;Persist Security Info=False";
            //            OleDbConnection connOle = new OleDbConnection(strConn);
            //            connOle.Open();
            dt日报表详细信息 = new DataTable("CS制袋日报表详细信息");
            bs日报表详细信息 = new BindingSource();
            da日报表详细信息 = new OleDbDataAdapter(@"select * from CS制袋日报表详细信息", mySystem.Parameter.connOle);
            cb日报表详细信息 = new OleDbCommandBuilder(da日报表详细信息);
            da日报表详细信息.Fill(dt日报表详细信息);
        }
        // 内表和控件的绑定
        private void innerBind()
        {
            // bs委托单.DataSource = dt委托单;

            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewColumns();
            setDataGridViewFormat();
            bs日报表详细信息.DataSource = dt日报表详细信息;
            dataGridView1.DataSource = bs日报表详细信息.DataSource;

        }
        // 设置自动计算类事件
        private void addComputerEventHandler()
        {

        }

        // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        //         如果审核结果为『通过』，则为『审核通过』
        //         如果审核结果为『不通过』，则为『审核未通过』
        private void setFormState()
        {

        }
        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过
        private void setEnableReadOnly()
        {

        }
        // 其他事件，比如按钮的点击，数据有效性判断
        private void addOtherEventHandler()
        {
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }
        // 获取操作员和审核员
        private void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='辐照灭菌台帐'", connOle);
            dt = new DataTable("temp");
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                string[] s = Regex.Split(dt.Rows[0]["操作员"].ToString(), ",|，");
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != "")
                        ls操作员.Add(s[i]);
                }
                string[] s1 = Regex.Split(dt.Rows[0]["审核员"].ToString(), ",|，");
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != "")
                        ls审核员.Add(s1[i]);
                }
            }
        }

        // 根据登录人，设置stat_user
        private void setUserState()
        {
            _userState = Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (Parameter.UserState.NoBody == _userState)
            {
                _userState = Parameter.UserState.管理员;
                label角色.Text = "管理员";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = "操作员";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = "审核员";
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            }
        }

        // 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
        private void setDataGridViewColumns()
        {

            DataGridViewTextBoxColumn c1;
            foreach (DataColumn dc in dt日报表详细信息.Columns)
            {
                switch (dc.ColumnName)
                {
                      default:
                        c1 = new DataGridViewTextBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.Automatic;
                        c1.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c1);
                        break;
                }
            }

        }

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewFormat()
        {
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["入库量只A"].HeaderText = "入库量(只)";
            dataGridView1.Columns["工时B"].HeaderText = "工时/h";
            dataGridView1.Columns["系数C"].HeaderText = "系数";
            dataGridView1.Columns["成品宽D"].HeaderText = "宽/mm";
            dataGridView1.Columns["成品长E"].HeaderText = "长/mm";
            dataGridView1.Columns["成品数量W"].HeaderText = "成品数量/㎡";
            dataGridView1.Columns["膜材1规格F"].HeaderText = "膜材1规格/mm";
            dataGridView1.Columns["膜材1用量G"].HeaderText = "膜材1用量/mm";
            dataGridView1.Columns["膜材1用量E"].HeaderText = "膜材1用量/㎡";
            dataGridView1.Columns["膜材2规格H"].HeaderText = "膜材2规格/mm";
            dataGridView1.Columns["膜材2用量K"].HeaderText = "膜材2用量/mm";
            dataGridView1.Columns["膜材2用量R"].HeaderText = "膜材2用量/㎡";
            dataGridView1.Columns["制袋收率"].HeaderText = "制袋收率（%）";
            //第一列ID不显示
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns["TCS制袋日报表ID"].Visible = false;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            int columnindex = ((DataGridView)sender).SelectedCells[0].ColumnIndex;
            String Columnsname = ((DataGridView)sender).Columns[columnindex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString();
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");

            if (Columnsname == "登记员" || Columnsname == "审核员")
            {
                string str人员 = dt日报表详细信息.Rows[columnindex][rowsname].ToString();
                if (mySystem.Parameter.NametoID(str人员) <= 0)
                    MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
            }

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        { }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        { }

        //写默认行数据
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["班次"] = "";
            dr["工时B"] = 0;
            dr["系数C"] = 0;
            dr["膜材1规格F"] = 0;
            dr["膜材2规格H"] = 0;

            return dr;
        }

        //设置序号递增
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        private void bt添加_Click(object sender, EventArgs e)
        {
            DataRow dr新行 = dt日报表详细信息.NewRow();
            dr新行 = writeInnerDefault(dr新行);
            dt日报表详细信息.Rows.Add(dr新行);
            setDataGridViewRowNums();
        }

        private void bt删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.SelectedCells[0].RowIndex < 0)
                    return;
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            }

            da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
            innerBind();
            //刷新序号
            setDataGridViewRowNums();
        }

        private void bt保存_Click(object sender, EventArgs e)
        {
            bool is填满 = is_filled();
            if (is填满)
            {
                bs日报表详细信息.EndEdit();
                da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
                readInnerData();
                innerBind();
            }
            else
                MessageBox.Show("信息填写不完整");
        }

        //某行数据是否填满
        private bool is_filled()
        {
            int index = dataGridView1.Rows.Count - 1;
            DataGridViewRow dgvr最后一行 = dataGridView1.Rows[index];
            DataRow dr最后一行 = dt日报表详细信息.NewRow();
            dr最后一行 = (dgvr最后一行.DataBoundItem as DataRowView).Row;

            int sum = 0;//空白单元格个数
            for (int i = 0; i < dr最后一行.ItemArray.Length; i++)
            {
                //string suibian = dr[i].ToString();
                //if (dr[i] != dr["审核意见"] && dr[i] != dr["审核是否通过"])
                //if (dr[i].Equals(dr["审核意见"]) || dr[i].Equals(dr["审核是否通过"]))
                if (i != 0 && i != 1)
                {
                    if (dr最后一行[i].ToString() == "")
                        sum += 1;
                }
                else
                {
                    sum += 0;
                }
            }
            if (sum != 0)
                return false;
            else
                return true;
        }

        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\miejun\SOP-MFG-106-R03A 辐照灭菌台帐.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];


            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 修改Sheet中某行某列的值
                my = printValue(my, wb);
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                //false->打印
                // 设置该进程是否可见
                oXL.Visible = false;
                // 修改Sheet中某行某列的值
                my = printValue(my, wb);
                try
                {
                    // 直接用默认打印机打印该Sheet
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch
                { }
                finally
                {
                    // 关闭文件，false表示不保存
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                }
            }
        }

        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            int rownum = dt日报表详细信息.Rows.Count;
            for (int i = 0; i < rownum; i++)
            {
                mysheet.Cells[i + 4, 2].Value = dt日报表详细信息.Rows[i]["委托单号"].ToString();
                mysheet.Cells[i + 4, 3].Value = Convert.ToDateTime(dt日报表详细信息.Rows[i]["委托日期"]).ToString("D");//去掉时分秒，且显示为****年**月**日
                mysheet.Cells[i + 4, 4].Value = dt日报表详细信息.Rows[i]["产品数量箱"].ToString();
                mysheet.Cells[i + 4, 5].Value = dt日报表详细信息.Rows[i]["产品数量只"].ToString();
                mysheet.Cells[i + 4, 6].Value = dt日报表详细信息.Rows[i]["送去产品托盘数量个"].ToString();
                mysheet.Cells[i + 4, 7].Value = dt日报表详细信息.Rows[i]["拉回产品托盘数量个"].ToString();
                mysheet.Cells[i + 4, 8].Value = dt日报表详细信息.Rows[i]["备注"].ToString();
                mysheet.Cells[i + 4, 9].Value = dt日报表详细信息.Rows[i]["登记员"].ToString();
                mysheet.Cells[i + 4, 10].Value = dt日报表详细信息.Rows[i]["审核员"].ToString();
            }
            //加页脚
            int sheetnum;
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 辐照灭菌台帐详细信息", connOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dt日报表详细信息.Rows[0]["ID"])) + 1;
            // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            mysheet.PageSetup.RightFooter = mySystem.Parameter.proInstruction + " - 09 - " + sheetnum.ToString() + " / &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString();
            //返回
            return mysheet;
        }

        private void bt打印_Click(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            print(true);
            //写日志
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 打印文档\n";
            dt日报表详细信息.Rows[0]["日志"] = dt日报表详细信息.Rows[0]["日志"].ToString() + log;

            bs日报表详细信息.EndEdit();
            da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
        }

        private void bt查看日志_Click(object sender, EventArgs e)
        {
            (new mySystem.Other.LogForm()).setLog(dt日报表.Rows[0]["日志"].ToString()).Show();
        }

    }
}
