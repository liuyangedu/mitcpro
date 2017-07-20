﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Setting
{
    public partial class 吹膜设置 : Form
    {
        public 吹膜设置()
        {
            InitializeComponent();
            Initdgv();
            Bind();
            FillNum();
        }

        int para1 = 0;
        double para2 = 0;
        double para3 = 0;
        double para4 = 0;
        private OleDbDataAdapter da清洁;
        private DataTable dt清洁;
        private BindingSource bs清洁;
        private OleDbCommandBuilder cb清洁;
        private OleDbDataAdapter da供料清场;
        private DataTable dt供料清场;
        private BindingSource bs供料清场;
        private OleDbCommandBuilder cb供料清场;
        private OleDbDataAdapter da吹膜清场;
        private DataTable dt吹膜清场;
        private BindingSource bs吹膜清场;
        private OleDbCommandBuilder cb吹膜清场;
        private OleDbDataAdapter da开机;
        private DataTable dt开机;
        private BindingSource bs开机;
        private OleDbCommandBuilder cb开机;
        private OleDbDataAdapter da交接班;
        private DataTable dt交接班;
        private BindingSource bs交接班;
        private OleDbCommandBuilder cb交接班;
        private OleDbDataAdapter da产品;
        private DataTable dt产品;
        private BindingSource bs产品;
        private OleDbCommandBuilder cb产品;
        private OleDbDataAdapter da产品编码;
        private DataTable dt产品编码;
        private BindingSource bs产品编码;
        private OleDbCommandBuilder cb产品编码;
        private OleDbDataAdapter da物料代码;
        private DataTable dt物料代码;
        private BindingSource bs物料代码;
        private OleDbCommandBuilder cb物料代码;
        private OleDbDataAdapter da工艺;
        private DataTable dt工艺;
        private BindingSource bs工艺;
        private OleDbCommandBuilder cb工艺;
        private OleDbDataAdapter da废品;
        private DataTable dt废品;
        private BindingSource bs废品;
        private OleDbCommandBuilder cb废品;

        //dgv样式初始化
        private void Initdgv()
        {
            bs清洁 = new BindingSource();
            dgv清洁.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv清洁.AllowUserToAddRows = false;
            dgv清洁.ReadOnly = false;
            dgv清洁.RowHeadersVisible = false;
            dgv清洁.AllowUserToResizeColumns = true;
            dgv清洁.AllowUserToResizeRows = false;
            dgv清洁.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv清洁.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv清洁.Font = new Font("宋体", 12);

            bs供料清场 = new BindingSource();
            dgv供料清场.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv供料清场.AllowUserToAddRows = false;
            dgv供料清场.ReadOnly = false;
            dgv供料清场.RowHeadersVisible = false;
            dgv供料清场.AllowUserToResizeColumns = true;
            dgv供料清场.AllowUserToResizeRows = false;
            dgv供料清场.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv供料清场.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv供料清场.Font = new Font("宋体", 12);

            bs吹膜清场 = new BindingSource();
            dgv吹膜清场.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv吹膜清场.AllowUserToAddRows = false;
            dgv吹膜清场.ReadOnly = false;
            dgv吹膜清场.RowHeadersVisible = false;
            dgv吹膜清场.AllowUserToResizeColumns = true;
            dgv吹膜清场.AllowUserToResizeRows = false;
            dgv吹膜清场.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv吹膜清场.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv吹膜清场.Font = new Font("宋体", 12);

            bs开机 = new BindingSource();
            dgv开机.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv开机.AllowUserToAddRows = false;
            dgv开机.ReadOnly = false;
            dgv开机.RowHeadersVisible = false;
            dgv开机.AllowUserToResizeColumns = true;
            dgv开机.AllowUserToResizeRows = false;
            dgv开机.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv开机.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv开机.Font = new Font("宋体", 12);

            bs交接班 = new BindingSource();
            dgv交接班.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv交接班.AllowUserToAddRows = false;
            dgv交接班.ReadOnly = false;
            dgv交接班.RowHeadersVisible = false;
            dgv交接班.AllowUserToResizeColumns = true;
            dgv交接班.AllowUserToResizeRows = false;
            dgv交接班.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv交接班.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv交接班.Font = new Font("宋体", 12);

            bs产品 = new BindingSource();
            dgv产品.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv产品.AllowUserToAddRows = false;
            dgv产品.ReadOnly = false;
            dgv产品.RowHeadersVisible = false;
            dgv产品.AllowUserToResizeColumns = true;
            dgv产品.AllowUserToResizeRows = false;
            dgv产品.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品.Font = new Font("宋体", 12);

            bs产品编码 = new BindingSource();
            dgv产品编码.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv产品编码.AllowUserToAddRows = false;
            dgv产品编码.ReadOnly = false;
            dgv产品编码.RowHeadersVisible = false;
            dgv产品编码.AllowUserToResizeColumns = true;
            dgv产品编码.AllowUserToResizeRows = false;
            dgv产品编码.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品编码.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品编码.Font = new Font("宋体", 12);

            bs物料代码 = new BindingSource();
            dgv物料代码.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv物料代码.AllowUserToAddRows = false;
            dgv物料代码.ReadOnly = false;
            dgv物料代码.RowHeadersVisible = false;
            dgv物料代码.AllowUserToResizeColumns = true;
            dgv物料代码.AllowUserToResizeRows = false;
            dgv物料代码.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv物料代码.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv物料代码.Font = new Font("宋体", 12);

            bs工艺 = new BindingSource();
            dgv工艺.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv工艺.AllowUserToAddRows = false;
            dgv工艺.ReadOnly = false;
            dgv工艺.RowHeadersVisible = false;
            dgv工艺.AllowUserToResizeColumns = true;
            dgv工艺.AllowUserToResizeRows = false;
            dgv工艺.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv工艺.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv工艺.Font = new Font("宋体", 12);

            bs废品 = new BindingSource();
            dgv废品.AllowUserToAddRows = false;
            dgv废品.ReadOnly = false;
            dgv废品.RowHeadersVisible = false;
            dgv废品.AllowUserToResizeColumns = true;
            dgv废品.AllowUserToResizeRows = false;
            dgv废品.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv废品.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv废品.Font = new Font("宋体", 12);
        }

        private void Bind()
        {
            //**************************   清洁    ***********************************
            dt清洁 = new DataTable("设置吹膜机组清洁项目"); //""中的是表名
            da清洁 = new OleDbDataAdapter("select * from 设置吹膜机组清洁项目", mySystem.Parameter.connOle);
            cb清洁 = new OleDbCommandBuilder(da清洁);
            dt清洁.Columns.Add("序号", System.Type.GetType("System.String"));
            da清洁.Fill(dt清洁);
            bs清洁.DataSource = dt清洁;
            this.dgv清洁.DataSource = bs清洁.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv清洁);
            this.dgv清洁.Columns["清洁区域"].MinimumWidth = 200;
            this.dgv清洁.Columns["清洁内容"].MinimumWidth = 250;
            this.dgv清洁.Columns["清洁内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv清洁.Columns["清洁内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv清洁.Columns["ID"].Visible = false;

            //**************************   供料清场    ***********************************
            dt供料清场 = new DataTable("设置供料工序清场项目"); //""中的是表名
            da供料清场 = new OleDbDataAdapter("select * from 设置供料工序清场项目", mySystem.Parameter.connOle);
            cb供料清场 = new OleDbCommandBuilder(da供料清场);
            dt供料清场.Columns.Add("序号", System.Type.GetType("System.String"));
            da供料清场.Fill(dt供料清场);
            bs供料清场.DataSource = dt供料清场;
            this.dgv供料清场.DataSource = bs供料清场.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv供料清场);
            this.dgv供料清场.Columns["清场内容"].MinimumWidth = 200;
            this.dgv供料清场.Columns["清场内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv供料清场.Columns["清场内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv供料清场.Columns["ID"].Visible = false;

            //************************    吹膜清场     *******************************************
            dt吹膜清场 = new DataTable("设置吹膜工序清场项目"); //""中的是表名
            da吹膜清场 = new OleDbDataAdapter("select * from 设置吹膜工序清场项目", mySystem.Parameter.connOle);
            cb吹膜清场 = new OleDbCommandBuilder(da吹膜清场);
            dt吹膜清场.Columns.Add("序号", System.Type.GetType("System.String"));
            da吹膜清场.Fill(dt吹膜清场);
            bs吹膜清场.DataSource = dt吹膜清场;
            this.dgv吹膜清场.DataSource = bs吹膜清场.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv吹膜清场);
            this.dgv吹膜清场.Columns["清场内容"].MinimumWidth = 200;
            this.dgv吹膜清场.Columns["清场内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv吹膜清场.Columns["清场内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv吹膜清场.Columns["ID"].Visible = false;

            //**************************   开机    ***********************************
            dt开机 = new DataTable("设置吹膜机组开机前确认项目"); //""中的是表名
            da开机 = new OleDbDataAdapter("select * from 设置吹膜机组开机前确认项目", mySystem.Parameter.connOle);
            cb开机 = new OleDbCommandBuilder(da开机);
            dt开机.Columns.Add("序号", System.Type.GetType("System.String"));
            da开机.Fill(dt开机);
            bs开机.DataSource = dt开机;
            this.dgv开机.DataSource = bs开机.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv开机);
            this.dgv开机.Columns["确认项目"].MinimumWidth = 160;
            this.dgv开机.Columns["确认内容"].MinimumWidth = 200;
            this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv开机.Columns["ID"].Visible = false;

            //************************    交接班     *******************************************
            dt交接班 = new DataTable("设置岗位交接班项目"); //""中的是表名
            da交接班 = new OleDbDataAdapter("select * from 设置岗位交接班项目", mySystem.Parameter.connOle);
            cb交接班 = new OleDbCommandBuilder(da交接班);
            dt交接班.Columns.Add("序号", System.Type.GetType("System.String"));
            da交接班.Fill(dt交接班);
            bs交接班.DataSource = dt交接班;
            this.dgv交接班.DataSource = bs交接班.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv交接班);
            this.dgv交接班.Columns["确认项目"].MinimumWidth = 250;
            this.dgv交接班.Columns["确认项目"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv交接班.Columns["确认项目"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv交接班.Columns["ID"].Visible = false;

            //************************    产品     *******************************************
            dt产品 = new DataTable("设置吹膜产品"); //""中的是表名
            da产品 = new OleDbDataAdapter("select * from 设置吹膜产品", mySystem.Parameter.connOle);
            cb产品 = new OleDbCommandBuilder(da产品);
            dt产品.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品.Fill(dt产品);
            bs产品.DataSource = dt产品;
            this.dgv产品.DataSource = bs产品.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品);
            this.dgv产品.Columns["产品名称"].MinimumWidth = 150;
            this.dgv产品.Columns["产品名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品.Columns["产品名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品.Columns["ID"].Visible = false;

            //**************************   产品编码    ***********************************
            dt产品编码 = new DataTable("设置吹膜产品编码"); //""中的是表名
            da产品编码 = new OleDbDataAdapter("select * from 设置吹膜产品编码", mySystem.Parameter.connOle);
            cb产品编码 = new OleDbCommandBuilder(da产品编码);
            dt产品编码.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品编码.Fill(dt产品编码);
            bs产品编码.DataSource = dt产品编码;
            this.dgv产品编码.DataSource = bs产品编码.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品编码);
            this.dgv产品编码.Columns["产品编码"].MinimumWidth = 150;
            this.dgv产品编码.Columns["产品编码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品编码.Columns["产品编码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品编码.Columns["ID"].Visible = false;

            //************************    物料代码     *******************************************
            dt物料代码 = new DataTable("设置物料代码"); //""中的是表名
            da物料代码 = new OleDbDataAdapter("select * from 设置物料代码", mySystem.Parameter.connOle);
            cb物料代码 = new OleDbCommandBuilder(da物料代码);
            dt物料代码.Columns.Add("序号", System.Type.GetType("System.String"));
            da物料代码.Fill(dt物料代码);
            bs物料代码.DataSource = dt物料代码;
            this.dgv物料代码.DataSource = bs物料代码.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv物料代码);
            this.dgv物料代码.Columns["物料代码"].MinimumWidth = 150;
            this.dgv物料代码.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv物料代码.Columns["物料代码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv物料代码.Columns["ID"].Visible = false;

            //**************************   工艺    ***********************************
            dt工艺 = new DataTable("设置吹膜工艺"); //""中的是表名
            da工艺 = new OleDbDataAdapter("select * from 设置吹膜工艺", mySystem.Parameter.connOle);
            cb工艺 = new OleDbCommandBuilder(da工艺);
            dt工艺.Columns.Add("序号", System.Type.GetType("System.String"));
            da工艺.Fill(dt工艺);
            bs工艺.DataSource = dt工艺;
            this.dgv工艺.DataSource = bs工艺.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv工艺);
            this.dgv工艺.Columns["工艺名称"].MinimumWidth = 150;
            this.dgv工艺.Columns["工艺名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv工艺.Columns["工艺名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv工艺.Columns["ID"].Visible = false;

            //**************************   废品    ***********************************
            dt废品 = new DataTable("设置废品产生原因"); //""中的是表名
            da废品 = new OleDbDataAdapter("select * from 设置废品产生原因", mySystem.Parameter.connOle);
            cb废品 = new OleDbCommandBuilder(da废品);
            dt废品.Columns.Add("序号", System.Type.GetType("System.String"));
            da废品.Fill(dt废品);
            bs废品.DataSource = dt废品;
            this.dgv废品.DataSource = bs废品.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv废品);
            this.dgv废品.Columns["废品产生原因"].MinimumWidth = 250;
            this.dgv废品.Columns["废品产生原因"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv废品.Columns["废品产生原因"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv废品.Columns["ID"].Visible = false;

        }

        private void setDataGridViewRowNums(DataGridView dgv)
        {
            int coun = dgv.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }

        //初始填预热参数textbox
        private void FillNum()
        {
            string tblName = "设置吹膜机组预热参数记录表";
            List<String> readqueryCols = new List<String>(new String[] { "换网预热参数设定1", "流道预热参数设定1", "模颈预热参数设定1", "机头1预热参数设定1", "机头2预热参数设定1", 
                    "口模预热参数设定1", "加热保温时间1", "一区预热参数设定1", "二区预热参数设定1", "三区预热参数设定1", "四区预热参数设定1", "换网预热参数设定2", 
                    "流道预热参数设定2", "模颈预热参数设定2", "机头1预热参数设定2", "机头2预热参数设定2", "口模预热参数设定2", "加热保温时间2", "一区预热参数设定2", "二区预热参数设定2", 
                    "三区预热参数设定2", "四区预热参数设定2", "加热保温时间3" ,"温度公差"});
            List<String> whereCols = new List<String>(new String[] { "ID" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            List<List<Object>> queryValsList = Utility.selectAccess(Parameter.connOle, tblName, readqueryCols, whereCols, whereVals, null, null, null, null, null);

            List<String> data = new List<String> { };
            for (int i = 0; i < queryValsList[0].Count; i++)
            {
                data.Add(queryValsList[0][i].ToString());
            }
            List<Control> textboxes = new List<Control> { hw1, ld1, mj1, jt11, jt21, km1, duration1, region11, region21, region31, region41, hw2, 
                    ld2, mj2, jt12, jt22, km2, duration2, region12, region22, region32, region42, duration3, tolerance};
            Utility.fillControl(textboxes, data);


            //读取数据库并显示
            String tblName2 = "设置生产指令参数";
            List<String> readqueryCols2 = new List<String>(new String[] { "面", "密度", "系数1", "系数2" });
            List<String> whereCols2 = new List<String>(new String[] { "ID" });
            List<Object> whereVals2 = new List<Object>(new Object[] { 1 });
            List<List<Object>> queryValsList2 = Utility.selectAccess(Parameter.connOle, tblName2, readqueryCols2, whereCols2, whereVals2, null, null, null, null, null);

            List<String> data2 = new List<String> { };
            for (int i = 0; i < queryValsList2[0].Count; i++)
            {
                data2.Add(queryValsList2[0][i].ToString());
            }
            List<Control> textboxes2 = new List<Control> { tB面数, tB厚度密度, tB参数1, tB参数2 };
            Utility.fillControl(textboxes2, data2);

        }


        private void DataSave()
        {
            try
            {
                string tblName = "设置吹膜机组预热参数记录表";
                List<String> queryCols = new List<String>(new String[] { "温度公差" , "换网预热参数设定1", "流道预热参数设定1", "模颈预热参数设定1", "机头1预热参数设定1", "机头2预热参数设定1", 
                    "口模预热参数设定1", "加热保温时间1", "一区预热参数设定1", "二区预热参数设定1", "三区预热参数设定1", "四区预热参数设定1", "换网预热参数设定2", 
                    "流道预热参数设定2", "模颈预热参数设定2", "机头1预热参数设定2", "机头2预热参数设定2", "口模预热参数设定2", "加热保温时间2", "一区预热参数设定2", "二区预热参数设定2", 
                    "三区预热参数设定2", "四区预热参数设定2", "加热保温时间3" });
                List<Object> queryVals = new List<Object>(new Object[] { Convert.ToDouble(tolerance.Text), Convert.ToDouble(hw1.Text), Convert.ToDouble(ld1.Text), 
                Convert.ToDouble(mj1.Text), Convert.ToDouble(jt11.Text), Convert.ToDouble(jt21.Text), Convert.ToDouble(km1.Text), 
                Convert.ToDouble(duration1.Text), Convert.ToDouble(region11.Text), Convert.ToDouble(region21.Text), 
                Convert.ToDouble(region31.Text), Convert.ToDouble(region41.Text), Convert.ToDouble(hw2.Text), Convert.ToDouble(ld2.Text), 
                Convert.ToDouble(mj2.Text), Convert.ToDouble(jt12.Text), Convert.ToDouble(jt22.Text), Convert.ToDouble(km2.Text), 
                Convert.ToDouble(duration2.Text), Convert.ToDouble(region12.Text), Convert.ToDouble(region22.Text), 
                Convert.ToDouble(region32.Text), Convert.ToDouble(region42.Text), Convert.ToDouble(duration3.Text)});
                List<String> whereCols = new List<String>(new String[] { "ID" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                Boolean b = Utility.updateAccess(Parameter.connOle, tblName, queryCols, queryVals, whereCols, whereVals);
                if (!b)
                {
                    MessageBox.Show("预热参数设置保存失败！", "错误");
                    return;
                }

                para1 = Convert.ToInt32(tB面数.Text.Trim());
                para2 = Convert.ToDouble(tB厚度密度.Text.Trim());
                para3 = Convert.ToDouble(tB参数1.Text.Trim());
                para4 = Convert.ToDouble(tB参数2.Text.Trim());
                String tblName2 = "设置生产指令参数";
                List<String> updateCols2 = new List<String>(new String[] { "面", "密度", "系数1", "系数2" });
                List<Object> updateVals2 = new List<Object>(new Object[] { para1, para2, para3, para4 });
                List<String> whereCols2 = new List<String>(new String[] { "ID" });
                List<Object> whereVals2 = new List<Object>(new Object[] { 1 });
                Boolean b2 = Utility.updateAccess(Parameter.connOle, tblName2, updateCols2, updateVals2, whereCols2, whereVals2);
                if (!b2)
                {
                    MessageBox.Show("参数保存失败", "错误");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("参数保存失败", "错误");
                return; 
            }
            

            MessageBox.Show("保存成功");          
        }


        private void add清洁_Click(object sender, EventArgs e)
        {
            DataRow dr = dt清洁.NewRow();
            dt清洁.Rows.InsertAt(dt清洁.NewRow(), dt清洁.Rows.Count);
            setDataGridViewRowNums(this.dgv清洁);
        }

        private void del清洁_Click(object sender, EventArgs e)
        {
            int idx = this.dgv清洁.CurrentRow.Index;
            dt清洁.Rows[idx].Delete();
            da清洁.Update((DataTable)bs清洁.DataSource);
            dt清洁.Clear();
            da清洁.Fill(dt清洁);
            setDataGridViewRowNums(this.dgv清洁);
        }

        private void add供料清场_Click(object sender, EventArgs e)
        {
            DataRow dr = dt供料清场.NewRow();
            dt供料清场.Rows.InsertAt(dt供料清场.NewRow(), dt供料清场.Rows.Count);
            setDataGridViewRowNums(this.dgv供料清场);
        }

        private void del供料清场_Click(object sender, EventArgs e)
        {
            int idx = this.dgv供料清场.CurrentRow.Index;
            dt供料清场.Rows[idx].Delete();
            da供料清场.Update((DataTable)bs供料清场.DataSource);
            dt供料清场.Clear();
            da供料清场.Fill(dt供料清场);
            setDataGridViewRowNums(this.dgv供料清场);
        }

        private void add吹膜清场_Click(object sender, EventArgs e)
        {
            DataRow dr = dt吹膜清场.NewRow();
            dt吹膜清场.Rows.InsertAt(dt吹膜清场.NewRow(), dt吹膜清场.Rows.Count);
            setDataGridViewRowNums(this.dgv吹膜清场);
        }

        private void del吹膜清场_Click(object sender, EventArgs e)
        {
            int idx = this.dgv吹膜清场.CurrentRow.Index;
            dt吹膜清场.Rows[idx].Delete();
            da吹膜清场.Update((DataTable)bs吹膜清场.DataSource);
            dt吹膜清场.Clear();
            da吹膜清场.Fill(dt吹膜清场);
            setDataGridViewRowNums(this.dgv吹膜清场);
        }

        private void Btn保存区域设置_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {
                    da清洁.Update((DataTable)bs清洁.DataSource);
                    dt清洁.Clear();
                    da清洁.Fill(dt清洁);
                    setDataGridViewRowNums(this.dgv清洁);

                    da供料清场.Update((DataTable)bs供料清场.DataSource);
                    dt供料清场.Clear();
                    da供料清场.Fill(dt供料清场);
                    setDataGridViewRowNums(this.dgv供料清场);

                    da吹膜清场.Update((DataTable)bs吹膜清场.DataSource);
                    dt吹膜清场.Clear();
                    da吹膜清场.Fill(dt吹膜清场);
                    setDataGridViewRowNums(this.dgv吹膜清场);

                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

        private void add开机_Click(object sender, EventArgs e)
        {
            DataRow dr = dt开机.NewRow();
            dt开机.Rows.InsertAt(dt开机.NewRow(), dt开机.Rows.Count);
            setDataGridViewRowNums(this.dgv开机);
        }

        private void del开机_Click(object sender, EventArgs e)
        {
            int idx = this.dgv开机.CurrentRow.Index;
            dt开机.Rows[idx].Delete();
            da开机.Update((DataTable)bs开机.DataSource);
            dt开机.Clear();
            da开机.Fill(dt开机);
            setDataGridViewRowNums(this.dgv开机);
        }

        private void add交接班_Click(object sender, EventArgs e)
        {
            DataRow dr = dt交接班.NewRow();
            dt交接班.Rows.InsertAt(dt交接班.NewRow(), dt交接班.Rows.Count);
            setDataGridViewRowNums(this.dgv交接班);
        }

        private void del交接班_Click(object sender, EventArgs e)
        {
            int idx = this.dgv交接班.CurrentRow.Index;
            dt交接班.Rows[idx].Delete();
            da交接班.Update((DataTable)bs开机.DataSource);
            dt交接班.Clear();
            da交接班.Fill(dt交接班);
            setDataGridViewRowNums(this.dgv交接班);
        }

        private void Btn保存项目设置_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {
                    da开机.Update((DataTable)bs开机.DataSource);
                    dt开机.Clear();
                    da开机.Fill(dt开机);
                    setDataGridViewRowNums(this.dgv开机);

                    da交接班.Update((DataTable)bs交接班.DataSource);
                    dt交接班.Clear();
                    da交接班.Fill(dt交接班);
                    setDataGridViewRowNums(this.dgv交接班);
                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

        private void Btn保存参数设置_Click(object sender, EventArgs e)
        {
            DataSave();
        }

        private void add产品_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品.NewRow();
            dt产品.Rows.InsertAt(dt产品.NewRow(), dt产品.Rows.Count);
            setDataGridViewRowNums(this.dgv产品);
        }

        private void del产品_Click(object sender, EventArgs e)
        {
            int idx = this.dgv产品.CurrentRow.Index;
            dt产品.Rows[idx].Delete();
            da产品.Update((DataTable)bs产品.DataSource);
            dt产品.Clear();
            da产品.Fill(dt产品);
            setDataGridViewRowNums(this.dgv产品);
        }

        private void add产品编码_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品编码.NewRow();
            dt产品编码.Rows.InsertAt(dt产品编码.NewRow(), dt产品编码.Rows.Count);
            setDataGridViewRowNums(this.dgv产品编码);
        }

        private void del产品编码_Click(object sender, EventArgs e)
        {
            int idx = this.dgv产品编码.CurrentRow.Index;
            dt产品编码.Rows[idx].Delete();
            da产品编码.Update((DataTable)bs产品编码.DataSource);
            dt产品编码.Clear();
            da产品编码.Fill(dt产品编码);
            setDataGridViewRowNums(this.dgv产品编码);
        }

        private void add物料代码_Click(object sender, EventArgs e)
        {
            DataRow dr = dt物料代码.NewRow();
            dt物料代码.Rows.InsertAt(dt物料代码.NewRow(), dt物料代码.Rows.Count);
            setDataGridViewRowNums(this.dgv物料代码);
        }

        private void del物料代码_Click(object sender, EventArgs e)
        {
            int idx = this.dgv物料代码.CurrentRow.Index;
            dt物料代码.Rows[idx].Delete();
            da物料代码.Update((DataTable)bs物料代码.DataSource);
            dt物料代码.Clear();
            da物料代码.Fill(dt物料代码);
            setDataGridViewRowNums(this.dgv物料代码);
        }

        private void add工艺_Click(object sender, EventArgs e)
        {
            DataRow dr = dt工艺.NewRow();
            dt工艺.Rows.InsertAt(dt工艺.NewRow(), dt工艺.Rows.Count);
            setDataGridViewRowNums(this.dgv工艺);
        }

        private void del工艺_Click(object sender, EventArgs e)
        {
            int idx = this.dgv工艺.CurrentRow.Index;
            dt工艺.Rows[idx].Delete();
            da工艺.Update((DataTable)bs开机.DataSource);
            dt工艺.Clear();
            da工艺.Fill(dt工艺);
            setDataGridViewRowNums(this.dgv工艺);
        }

        private void add废品_Click(object sender, EventArgs e)
        {
            DataRow dr = dt废品.NewRow();
            dt废品.Rows.InsertAt(dt废品.NewRow(), dt废品.Rows.Count);
            setDataGridViewRowNums(this.dgv废品);
        }

        private void del废品_Click(object sender, EventArgs e)
        {
            int idx = this.dgv废品.CurrentRow.Index;
            dt废品.Rows[idx].Delete();
            da废品.Update((DataTable)bs废品.DataSource);
            dt废品.Clear();
            da废品.Fill(dt废品);
            setDataGridViewRowNums(this.dgv废品);
        }

        private void Btn保存产品设置_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {
                    da产品.Update((DataTable)bs产品.DataSource);
                    dt产品.Clear();
                    da产品.Fill(dt产品);
                    setDataGridViewRowNums(this.dgv产品);

                    da产品编码.Update((DataTable)bs产品编码.DataSource);
                    dt产品编码.Clear();
                    da产品编码.Fill(dt产品编码);
                    setDataGridViewRowNums(this.dgv产品编码);

                    da物料代码.Update((DataTable)bs物料代码.DataSource);
                    dt物料代码.Clear();
                    da物料代码.Fill(dt物料代码);
                    setDataGridViewRowNums(this.dgv物料代码);

                    da工艺.Update((DataTable)bs工艺.DataSource);
                    dt工艺.Clear();
                    da工艺.Fill(dt工艺);
                    setDataGridViewRowNums(this.dgv工艺);

                    da废品.Update((DataTable)bs废品.DataSource);
                    dt废品.Clear();
                    da废品.Fill(dt废品);
                    setDataGridViewRowNums(this.dgv废品);

                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

    }
}
