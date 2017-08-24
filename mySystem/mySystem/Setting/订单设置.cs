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
    public partial class 订单设置 : Form
    {

        private OleDbDataAdapter da组件存货档案;
        private DataTable dt组件存货档案;
        private BindingSource bs组件存货档案;
        private OleDbCommandBuilder cb组件存货档案;
        private OleDbDataAdapter da产成品存货档案;
        private DataTable dt产成品存货档案;
        private BindingSource bs产成品存货档案;
        private OleDbCommandBuilder cb产成品存货档案;


        private OleDbDataAdapter da人员;
        private DataTable dt人员;
        private BindingSource bs人员;
        private OleDbCommandBuilder cb人员;
        private OleDbDataAdapter da权限;
        private DataTable dt权限;
        private BindingSource bs权限;
        private OleDbCommandBuilder cb权限;

        private OleDbDataAdapter da业务类型;
        private DataTable dt业务类型;
        private BindingSource bs业务类型;
        private OleDbCommandBuilder cb业务类型;
        private OleDbDataAdapter da销售类型;
        private DataTable dt销售类型;
        private BindingSource bs销售类型;
        private OleDbCommandBuilder cb销售类型;
        private OleDbDataAdapter da客户简称;
        private DataTable dt客户简称;
        private BindingSource bs客户简称;
        private OleDbCommandBuilder cb客户简称;
        private OleDbDataAdapter da销售部门;
        private DataTable dt销售部门;
        private BindingSource bs销售部门;
        private OleDbCommandBuilder cb销售部门;
        private OleDbDataAdapter da币种;
        private DataTable dt币种;
        private BindingSource bs币种;
        private OleDbCommandBuilder cb币种;

        public 订单设置()
        {
            InitializeComponent();
            Initdgv();
            Bind();
            dgv产成品存货档案.CellDoubleClick += new DataGridViewCellEventHandler(dgv产成品存货档案_CellDoubleClick);
        }

        void dgv产成品存货档案_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv产成品存货档案.Columns[e.ColumnIndex].Name == "BOM列表")
            {
                // 弹出对话框，选择组件
                //MessageBox.Show("dian");
                OleDbDataAdapter da = new OleDbDataAdapter("select ID, 存货编码,存货名称,规格型号 from 设置组件存货档案", mySystem.Parameter.connOle);
                DataTable dt = new DataTable();
                da.Fill(dt);
                try
                {
                    String ids = mySystem.Other.InputDataGridView.getIDs(dgv产成品存货档案[e.ColumnIndex, e.RowIndex].Value.ToString(), dt);
                    dgv产成品存货档案[e.ColumnIndex, e.RowIndex].Value = ids;
                }
                catch (Exception ee)
                {
                }
                
            }
        }

        private void Initdgv()
        {
            bs组件存货档案 = new BindingSource();
            EachInitdgv(dgv组件存货档案);

            bs产成品存货档案 = new BindingSource();
            EachInitdgv(dgv产成品存货档案);

            bs人员 = new BindingSource();
            EachInitdgv(dgv人员);

            bs权限 = new BindingSource();
            EachInitdgv(dgv权限);

            bs业务类型 = new BindingSource();
            EachInitdgv(dgv业务类型);

            bs销售类型 = new BindingSource();
            EachInitdgv(dgv销售类型);

            bs客户简称 = new BindingSource();
            EachInitdgv(dgv客户简称);

            bs销售部门 = new BindingSource();
            EachInitdgv(dgv销售部门);

            bs币种 = new BindingSource();
            EachInitdgv(dgv币种);

        }


        private void Bind()
        {
            //**************************   供应商    ***********************************
            dt组件存货档案 = new DataTable("设置组件存货档案"); //""中的是表名
            da组件存货档案 = new OleDbDataAdapter("select * from 设置组件存货档案", mySystem.Parameter.connOle);
            cb组件存货档案 = new OleDbCommandBuilder(da组件存货档案);
            dt组件存货档案.Columns.Add("序号", System.Type.GetType("System.String"));
            da组件存货档案.Fill(dt组件存货档案);
            bs组件存货档案.DataSource = dt组件存货档案;
            this.dgv组件存货档案.DataSource = bs组件存货档案.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv组件存货档案);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv组件存货档案.Columns["ID"].Visible = false;
            

            //**************************   检验标准    ***********************************
            dt产成品存货档案 = new DataTable("设置产成品存货档案"); //""中的是表名
            da产成品存货档案 = new OleDbDataAdapter("select * from 设置产成品存货档案", mySystem.Parameter.connOle);
            cb产成品存货档案 = new OleDbCommandBuilder(da产成品存货档案);
            dt产成品存货档案.Columns.Add("序号", System.Type.GetType("System.String"));
            da产成品存货档案.Fill(dt产成品存货档案);
            bs产成品存货档案.DataSource = dt产成品存货档案;
            this.dgv产成品存货档案.DataSource = bs产成品存货档案.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产成品存货档案);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产成品存货档案.Columns["ID"].Visible = false;
            this.dgv产成品存货档案.Columns["BOM列表"].ReadOnly = true;

            //**************************   人员设置    ***********************************
            dt人员 = new DataTable("订单用户"); //""中的是表名
            da人员 = new OleDbDataAdapter("select * from 订单用户", mySystem.Parameter.connOle);
            cb人员 = new OleDbCommandBuilder(da人员);
            dt人员.Columns.Add("序号", System.Type.GetType("System.String"));
            da人员.Fill(dt人员);
            bs人员.DataSource = dt人员;
            this.dgv人员.DataSource = bs人员.DataSource;
            //显示序号
            //dgv人员下拉框设置();
            setDataGridViewRowNums(this.dgv人员);
            this.dgv人员.Columns["用户名"].MinimumWidth = 150;
            //this.dgv人员.Columns["班次"].MinimumWidth = 100;
            this.dgv人员.Columns["用户名"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv人员.Columns["ID"].Visible = false;

            //************************    人员权限     *******************************************
            dt权限 = new DataTable("订单用户权限"); //""中的是表名
            da权限 = new OleDbDataAdapter("select * from 订单用户权限", mySystem.Parameter.connOle);
            cb权限 = new OleDbCommandBuilder(da权限);
            dt权限.Columns.Add("序号", System.Type.GetType("System.String"));
            da权限.Fill(dt权限);
            bs权限.DataSource = dt权限;
            this.dgv权限.DataSource = bs权限.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv权限);
            this.dgv权限.Columns["步骤"].MinimumWidth = 250;
            this.dgv权限.Columns["操作员"].MinimumWidth = 150;
            this.dgv权限.Columns["审核员"].MinimumWidth = 150;
            this.dgv权限.Columns["步骤"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv权限.Columns["步骤"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv权限.Columns["ID"].Visible = false;


            //**************************   业务类型    ***********************************
            dt业务类型 = new DataTable("设置业务类型"); //""中的是表名
            da业务类型 = new OleDbDataAdapter("select * from 设置业务类型", mySystem.Parameter.connOle);
            cb业务类型 = new OleDbCommandBuilder(da业务类型);
            dt业务类型.Columns.Add("序号", System.Type.GetType("System.String"));
            da业务类型.Fill(dt业务类型);
            bs业务类型.DataSource = dt业务类型;
            this.dgv业务类型.DataSource = bs业务类型.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv业务类型);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv业务类型.Columns["ID"].Visible = false;


            //**************************   销售类型    ***********************************
            dt销售类型 = new DataTable("设置销售类型"); //""中的是表名
            da销售类型 = new OleDbDataAdapter("select * from 设置销售类型", mySystem.Parameter.connOle);
            cb销售类型 = new OleDbCommandBuilder(da销售类型);
            dt销售类型.Columns.Add("序号", System.Type.GetType("System.String"));
            da销售类型.Fill(dt销售类型);
            bs销售类型.DataSource = dt销售类型;
            this.dgv销售类型.DataSource = bs销售类型.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv销售类型);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv销售类型.Columns["ID"].Visible = false;


            //**************************   客户简称    ***********************************
            dt客户简称 = new DataTable("设置客户简称"); //""中的是表名
            da客户简称 = new OleDbDataAdapter("select * from 设置客户简称", mySystem.Parameter.connOle);
            cb客户简称 = new OleDbCommandBuilder(da客户简称);
            dt客户简称.Columns.Add("序号", System.Type.GetType("System.String"));
            da客户简称.Fill(dt客户简称);
            bs客户简称.DataSource = dt客户简称;
            this.dgv客户简称.DataSource = bs客户简称.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv客户简称);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv客户简称.Columns["ID"].Visible = false;


            //**************************   销售部门    ***********************************
            dt销售部门 = new DataTable("设置销售部门"); //""中的是表名
            da销售部门 = new OleDbDataAdapter("select * from 设置销售部门", mySystem.Parameter.connOle);
            cb销售部门 = new OleDbCommandBuilder(da销售部门);
            dt销售部门.Columns.Add("序号", System.Type.GetType("System.String"));
            da销售部门.Fill(dt销售部门);
            bs销售部门.DataSource = dt销售部门;
            this.dgv销售部门.DataSource = bs销售部门.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv销售部门);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv销售部门.Columns["ID"].Visible = false;


            //**************************   币种    ***********************************
            dt币种 = new DataTable("设置币种"); //""中的是表名
            da币种 = new OleDbDataAdapter("select * from 设置币种", mySystem.Parameter.connOle);
            cb币种 = new OleDbCommandBuilder(da币种);
            dt币种.Columns.Add("序号", System.Type.GetType("System.String"));
            da币种.Fill(dt币种);
            bs币种.DataSource = dt币种;
            this.dgv币种.DataSource = bs币种.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv币种);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv币种.Columns["ID"].Visible = false;

        }

        private void setDataGridViewRowNums(DataGridView dgv)
        {
            int coun = dgv.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }

        private void EachInitdgv(DataGridView dgv)
        {
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToResizeRows = false;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Font = new Font("宋体", 12);
        }

        private void add组件存货档案_Click(object sender, EventArgs e)
        {
            DataRow dr = dt组件存货档案.NewRow();
            dt组件存货档案.Rows.InsertAt(dt组件存货档案.NewRow(), dt组件存货档案.Rows.Count);
            setDataGridViewRowNums(this.dgv组件存货档案);
        }

        private void del组件存货档案_Click(object sender, EventArgs e)
        {
            int idx = this.dgv组件存货档案.CurrentRow.Index;
            dt组件存货档案.Rows[idx].Delete();
            da组件存货档案.Update((DataTable)bs组件存货档案.DataSource);
            dt组件存货档案.Clear();
            da组件存货档案.Fill(dt组件存货档案);
            setDataGridViewRowNums(this.dgv组件存货档案);
        }

        private void add产成品存货档案_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产成品存货档案.NewRow();
            dt产成品存货档案.Rows.InsertAt(dt产成品存货档案.NewRow(), dt产成品存货档案.Rows.Count);
            setDataGridViewRowNums(this.dgv产成品存货档案);
        }

        private void del产成品存货档案_Click(object sender, EventArgs e)
        {
            int idx = this.dgv产成品存货档案.CurrentRow.Index;
            dt产成品存货档案.Rows[idx].Delete();
            da产成品存货档案.Update((DataTable)bs产成品存货档案.DataSource);
            dt产成品存货档案.Clear();
            da产成品存货档案.Fill(dt产成品存货档案);
            setDataGridViewRowNums(this.dgv产成品存货档案);
        }

        private void Btn保存组件设置_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {
                    da组件存货档案.Update((DataTable)bs组件存货档案.DataSource);
                    dt组件存货档案.Clear();
                    da组件存货档案.Fill(dt组件存货档案);
                    setDataGridViewRowNums(this.dgv组件存货档案);

                    da产成品存货档案.Update((DataTable)bs产成品存货档案.DataSource);
                    dt产成品存货档案.Clear();
                    da产成品存货档案.Fill(dt产成品存货档案);
                    setDataGridViewRowNums(this.dgv产成品存货档案);

                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

        private void Btn保存人员_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {
                    Boolean b = checkPeopleExist(); //判断用户是否在用户大表中
                    if (b)
                    {
                        da人员.Update((DataTable)bs人员.DataSource);
                        dt人员.Clear();
                        da人员.Fill(dt人员);
                        setDataGridViewRowNums(this.dgv人员);

                        Boolean c = checkPeopleRight(); //判断用户是否在库存管理用户表中
                        if (c)
                        {
                            da权限.Update((DataTable)bs权限.DataSource);
                            dt权限.Clear();
                            da权限.Fill(dt权限);
                            setDataGridViewRowNums(this.dgv权限);

                            MessageBox.Show("保存成功！");
                        }
                    }
                }

            }
            catch (Exception ee)
            { MessageBox.Show("保存失败！", "错误"); }
        }

        private void add人员_Click(object sender, EventArgs e)
        {
            DataRow dr = dt人员.NewRow();
            dt人员.Rows.InsertAt(dt人员.NewRow(), dt人员.Rows.Count);
            setDataGridViewRowNums(this.dgv人员);
        }

        private void del人员_Click(object sender, EventArgs e)
        {
            int idx = this.dgv人员.SelectedCells[0].RowIndex;
            dt人员.Rows[idx].Delete();
            da人员.Update((DataTable)bs人员.DataSource);
            dt人员.Clear();
            da人员.Fill(dt人员);
            setDataGridViewRowNums(this.dgv人员);
        }

        private Boolean checkPeopleExist()
        {
            Boolean b = true;
            string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/user.mdb;Persist Security Info=False";
            OleDbConnection conn = new OleDbConnection(strCon);
            conn.Open();
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conn;

            foreach (DataRow dr in dt人员.Rows)
            {
                //判断行状态
                DataRowState state = dr.RowState;
                if (state.Equals(DataRowState.Deleted))
                { continue; }
                else
                {
                    String name = dr["用户名"].ToString();
                    comm.CommandText = "select * from [users] where 姓名 = " + "'" + name + "'";
                    OleDbDataReader reader = comm.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        b = false;
                        MessageBox.Show("员工" + "“" + name + "”" + "不存在！");
                    }
                    reader.Dispose();
                }
            }

            comm.Dispose();
            conn.Dispose();
            return b;
        }


        private Boolean checkPeopleRight()
        {
            Boolean b;
            String[] list操作员;
            String[] list审核员;
            foreach (DataRow dr in dt权限.Rows)
            {
                list操作员 = dr["操作员"].ToString().Split(new char[] { '，', ',' });
                foreach (String name in list操作员)
                {
                    Boolean eachbool = EachPeopleRightCheck(name);
                    if (!eachbool)
                    { return b = false; }
                }
                list审核员 = dr["审核员"].ToString().Split(new char[] { '，', ',' });
                foreach (String name in list审核员)
                {
                    Boolean eachbool = EachPeopleRightCheck(name);
                    if (!eachbool)
                    { return b = false; }
                }
            }

            return b = true;
        }


        private Boolean EachPeopleRightCheck(String name)
        {
            Boolean b;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Parameter.connOle;
            comm.CommandText = "select * from 订单用户 where 用户名 = " + "'" + name + "' ";
            OleDbDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            { b = true; }
            else
            {
                b = false;
                MessageBox.Show("员工" + "“" + name + "”" + "无订单管理权限，保存失败！");
            }

            reader.Dispose();
            comm.Dispose();
            return b;
        }

        private void add业务类型_Click(object sender, EventArgs e)
        {
            DataRow dr = dt业务类型.NewRow();
            dt业务类型.Rows.InsertAt(dt业务类型.NewRow(), dt业务类型.Rows.Count);
            setDataGridViewRowNums(this.dgv业务类型);
        }

        private void add销售类型_Click(object sender, EventArgs e)
        {
            DataRow dr = dt销售类型.NewRow();
            dt销售类型.Rows.InsertAt(dt销售类型.NewRow(), dt销售类型.Rows.Count);
            setDataGridViewRowNums(this.dgv销售类型);
        }

        private void add客户简称_Click(object sender, EventArgs e)
        {
            DataRow dr = dt客户简称.NewRow();
            dt客户简称.Rows.InsertAt(dt客户简称.NewRow(), dt客户简称.Rows.Count);
            setDataGridViewRowNums(this.dgv客户简称);
        }

        private void add销售部门_Click(object sender, EventArgs e)
        {
            DataRow dr = dt销售部门.NewRow();
            dt销售部门.Rows.InsertAt(dt销售部门.NewRow(), dt销售部门.Rows.Count);
            setDataGridViewRowNums(this.dgv销售部门);
        }

        private void add币种_Click(object sender, EventArgs e)
        {
            DataRow dr = dt币种.NewRow();
            dt币种.Rows.InsertAt(dt币种.NewRow(), dt币种.Rows.Count);
            setDataGridViewRowNums(this.dgv币种);
        }

        private void del业务类型_Click(object sender, EventArgs e)
        {
            int idx = this.dgv业务类型.SelectedCells[0].RowIndex;
            dt业务类型.Rows[idx].Delete();
            da业务类型.Update((DataTable)bs业务类型.DataSource);
            dt业务类型.Clear();
            da业务类型.Fill(dt业务类型);
            setDataGridViewRowNums(this.dgv业务类型);
        }

        private void del销售类型_Click(object sender, EventArgs e)
        {
            int idx = this.dgv销售类型.SelectedCells[0].RowIndex;
            dt销售类型.Rows[idx].Delete();
            da销售类型.Update((DataTable)bs销售类型.DataSource);
            dt销售类型.Clear();
            da销售类型.Fill(dt销售类型);
            setDataGridViewRowNums(this.dgv销售类型);
        }

        private void del客户简称_Click(object sender, EventArgs e)
        {
            int idx = this.dgv客户简称.SelectedCells[0].RowIndex;
            dt客户简称.Rows[idx].Delete();
            da客户简称.Update((DataTable)bs客户简称.DataSource);
            dt客户简称.Clear();
            da客户简称.Fill(dt客户简称);
            setDataGridViewRowNums(this.dgv客户简称);
        }

        private void del销售部门_Click(object sender, EventArgs e)
        {
            int idx = this.dgv销售部门.SelectedCells[0].RowIndex;
            dt销售部门.Rows[idx].Delete();
            da销售部门.Update((DataTable)bs销售部门.DataSource);
            dt销售部门.Clear();
            da销售部门.Fill(dt销售部门);
            setDataGridViewRowNums(this.dgv销售部门);
        }

        private void del币种_Click(object sender, EventArgs e)
        {
            int idx = this.dgv币种.SelectedCells[0].RowIndex;
            dt币种.Rows[idx].Delete();
            da币种.Update((DataTable)bs币种.DataSource);
            dt币种.Clear();
            da币种.Fill(dt币种);
            setDataGridViewRowNums(this.dgv币种);
        }

        private void save基本信息设置_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {   
                    da业务类型.Update((DataTable)bs业务类型.DataSource);
                    dt业务类型.Clear();
                    da业务类型.Fill(dt业务类型);
                    setDataGridViewRowNums(this.dgv业务类型);

                    da销售类型.Update((DataTable)bs销售类型.DataSource);
                    dt销售类型.Clear();
                    da销售类型.Fill(dt销售类型);
                    setDataGridViewRowNums(this.dgv销售类型);

                    da客户简称.Update((DataTable)bs客户简称.DataSource);
                    dt客户简称.Clear();
                    da客户简称.Fill(dt客户简称);
                    setDataGridViewRowNums(this.dgv客户简称);


                    da销售部门.Update((DataTable)bs销售部门.DataSource);
                    dt销售部门.Clear();
                    da销售部门.Fill(dt销售部门);
                    setDataGridViewRowNums(this.dgv销售部门);

                    da币种.Update((DataTable)bs币种.DataSource);
                    dt币种.Clear();
                    da币种.Fill(dt币种);
                    setDataGridViewRowNums(this.dgv币种);
                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }



    }
}
