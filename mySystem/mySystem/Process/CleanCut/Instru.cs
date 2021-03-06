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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace mySystem.Process.CleanCut
{
    public partial class Instru : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;

        private DataTable dt_prodinstr, dt_prodlist;
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;
        private string person_操作员;
        private string person_审核员;
        private List<string> list_操作员;
        private List<string> list_审核员;

        private HashSet<string> hs产品代码;

        private SqlDataAdapter da_prodinstrsql, da_prodlistsql;
        private SqlCommandBuilder cb_prodinstrsql, cb_prodlistsql;

        //private int stat_user;//登录人状态，0 操作员， 1 审核员， 2管理员
        //private int stat_form;//窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过

        // 需要保存的状态
        /// <summary>
        /// 1:操作员，2：审核员，4：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;

        private string instrcode;//指令编号
        //用于带id参数构造函数，存储已存在记录的相关信息
        int instrid;

        HashSet<string> hs_产品代码;//存储内表产品代码下拉框内容
        bool isFirstBind = true;

        public Instru(mySystem.MainForm mainform)
            : base(mainform)
        {
            hs_产品代码 = new HashSet<string>();
            InitializeComponent();

            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            foreach(Control c in this.Controls)
                c.Enabled=false;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            tb指令编号.Enabled = true;
            bt查询插入.Enabled = true;

            tb指令编号.Text = mySystem.Parameter.cleancutInstruction;
            instrid = mySystem.Parameter.cleancutInstruID;
            instrcode = mySystem.Parameter.cleancutInstruction;
        }

        public Instru(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            hs_产品代码 = new HashSet<string>();
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            string asql = "select * from 清洁分切工序生产指令 where ID=" + id;
            SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            instrcode = tempdt.Rows[0]["生产指令编号"].ToString();
            //instrid = (int)tempdt.Rows[0]["生产指令ID"];
            instrid = id;

            readOuterData(instrcode);
            removeOuterBinding();
            outerBind();

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            getInnerOtherData();
            innerBind();

            setFormState();
            setEnableReadOnly();
            addOtherEvnetHandler();

            bt查询插入.Enabled = false;
            tb指令编号.Enabled = false;

        }

        void getInnerOtherData()
        {
            DataTable dt;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                hs产品代码 = new HashSet<string>();
                //　产品代码
                string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
                OleDbConnection Tconn = new OleDbConnection(strConnect);
                Tconn.Open();
                da = new OleDbDataAdapter("select * from 设置存货档案 where 类型 like '组件' and 属于工序 like '%清洁分切%'", Tconn);
                dt = new DataTable("temp");
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;
                
                hs产品代码 = new HashSet<string>();
                //　产品代码
                string strConnect = "server=" + Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection Tconn = new SqlConnection(strConnect);
                Tconn.Open();
                da = new SqlDataAdapter("select * from 设置存货档案 where 类型 like '组件' and 属于工序 like '%清洁分切%'", Tconn);
                dt = new DataTable("temp");
                da.Fill(dt);
            }
            
            foreach (DataRow dr in dt.Rows)
            {
                hs产品代码.Add(dr["存货代码"].ToString());
            }

            // 自定义数据
            foreach (DataRow dr in dt_prodlist.Rows)
            {
                hs产品代码.Add(dr["清洁前产品代码"].ToString());
            }
        }

        // 设置读取数据的事件
        void addDataEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
            //dataGridView1.EditingControlShowing +=new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
            //dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
        }
        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }
        void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (e.ColumnIndex == 3)
            //{
            //    object eFV = e.FormattedValue;
            //    DataGridViewComboBoxColumn cbc = dataGridView1.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
            //    if (!cbc.Items.Contains(eFV))
            //    {
            //        cbc.Items.Add(eFV);
            //        dataGridView1.SelectedCells[0].Value = eFV;
            //    }
            //}
        }

        private void dataGridView1_EditingControlShowing(object sender,DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (sender as DataGridView);
            if (dgv.SelectedCells.Count == 0) return;
            int colIdx = dgv.SelectedCells[0].ColumnIndex;

            if (colIdx == 3)//分切前产品代码
            {
                TextBox tb = (e.Control as TextBox);
                tb.AutoCompleteCustomSource = null;
                AutoCompleteStringCollection acsc;
                if (tb == null) return;
                acsc = new AutoCompleteStringCollection();
                acsc.AddRange(hs产品代码.ToArray());
                tb.AutoCompleteCustomSource = acsc;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }

            //if (((DataGridView)sender).SelectedCells.Count <= 0)
            //    return;
            //if (((DataGridView)sender).SelectedCells[0].ColumnIndex == 3)
            //{
            //    //ComboBox c = e.Control as ComboBox;
            //    //if (c != null) c.DropDownStyle = ComboBoxStyle.DropDown;
            //    TextBox tb = (e.Control as TextBox);
            //    tb.AutoCompleteCustomSource = null;
            //    AutoCompleteStringCollection acsc;
            //    if (tb == null) return;
            //    acsc = new AutoCompleteStringCollection();
            //    acsc.AddRange(hs产品代码.ToArray());
            //    tb.AutoCompleteCustomSource = acsc;
            //    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //}
        }

        void setUserState()
        {
            //if (mySystem.Parameter.userName == person_操作员)
            //    stat_user = 0;
            //else if (mySystem.Parameter.userName == person_审核员)
            //    stat_user = 1;
            //else
            //    stat_user = 2;

            _userState = Parameter.UserState.NoBody;
            if (list_操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (list_审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
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

        //// 获取操作员和审核员
        void getPeople()
        {
            list_操作员 = new List<string>();
            list_审核员 = new List<string>();

            DataTable dt = new DataTable("用户权限");
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='清洁分切工序生产指令'", mySystem.Parameter.connOle);
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter(@"select * from 用户权限 where 步骤='清洁分切工序生产指令'", mySystem.Parameter.conn);
                da.Fill(dt);
            }
            

            if (dt.Rows.Count > 0)
            {
                person_操作员 = dt.Rows[0]["操作员"].ToString();
                person_审核员 = dt.Rows[0]["审核员"].ToString();
                string[] s = Regex.Split(person_操作员, ",|，");
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != "")
                        list_操作员.Add(s[i]);
                }
                string[] s1 = Regex.Split(person_审核员, ",|，");
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != "")
                        list_审核员.Add(s1[i]);
                }
            }

        }

        
        private void getOtherData()
        {
            //获取设置中产品代码
            DataTable tdt = new DataTable("产品编码");
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter tda = new OleDbDataAdapter("select 产品编码 from 设置清洁分切产品编码", mySystem.Parameter.connOle);
                
                tda.Fill(tdt);
            }
            else
            {
                SqlDataAdapter tda = new SqlDataAdapter("select 产品编码 from 设置清洁分切产品编码", mySystem.Parameter.conn);
                tda.Fill(tdt);
            }
            
            foreach (DataRow tdr in tdt.Rows)
            {
                hs_产品代码.Add(tdr["产品编码"].ToString());
            }
            //添加打印机
            fill_printer();
        }

        //public Instru()
        //{ }
        private void Init()
        {
           
        }

        //表格填写错误提示
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        bool input_Judge()
        {
            //判断合法性
            //编制人
            if (mySystem.Parameter.NametoID(tb编制人.Text) <= 0)
            {
                MessageBox.Show("编制人ID不存在");
                return false;
            }
            //接收人
            if (mySystem.Parameter.NametoID(tb接收人.Text) <= 0)
            {
                MessageBox.Show("接受人ID不存在");
                return false;
            }
            //产品代码是否重复
            //HashSet<string> hs_temp = new HashSet<string>();
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (hs_temp.Contains(dataGridView1.Rows[i].Cells[3].Value.ToString()))
            //    {
            //        MessageBox.Show("产品编码不能重复");
            //        return false;
            //    }                    
            //    hs_temp.Add(dataGridView1.Rows[i].Cells[3].Value.ToString());
            //}
            return true;


        }

        //确认按钮
        private void button1_Click(object sender, EventArgs e)
        {
            //bool rt=save();

            ////控件可见性
            //if (rt && _userState == Parameter.UserState.操作员)
            //    bt发送审核.Enabled = true;
            save();
            if (_userState == Parameter.UserState.操作员)
                bt发送审核.Enabled = true;
        }

        private bool save()
        {
            //外表保存
            bs_prodinstr.EndEdit();
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            }
            else
            {
                da_prodinstrsql.Update((DataTable)bs_prodinstr.DataSource);
            }
            
            readOuterData(instrcode);
            removeOuterBinding();
            outerBind();

            //内表保存
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();

            return true;
        }

        private void bt查询插入_Click(object sender, EventArgs e)
        {
            readOuterData(tb指令编号.Text);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0 && _userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员可以新建指令");
                return;
            }
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                if (!mySystem.Parameter.isSqlOk)
                {
                    da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                }
                else
                {
                    da_prodinstrsql.Update((DataTable)bs_prodinstr.DataSource);
                }
                
                readOuterData(tb指令编号.Text);
                removeOuterBinding();
                outerBind();
            }

            instrcode = tb指令编号.Text;
            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            getInnerOtherData();
            innerBind();

            setFormState();
            setEnableReadOnly();
            addOtherEvnetHandler();

            tb指令编号.Enabled = false;
            bt查询插入.Enabled = false;

        }

        // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        //         如果审核结果为『通过』，则为『审核通过』
        //         如果审核结果为『不通过』，则为『审核未通过』
        void setFormState()
        {
            //if (dt_prodinstr.Rows[0]["审核人"].ToString() == "")
            //    stat_form = 0;
            //else if(dt_prodinstr.Rows[0]["审核人"].ToString() =="__待审核")
            //    stat_form = 1;
            //else if((bool)dt_prodinstr.Rows[0]["审核是否通过"])
            //    stat_form = 2;
            //else
            //    stat_form = 3;

            string s = dt_prodinstr.Rows[0]["审核人"].ToString();
            bool b = Convert.ToBoolean(dt_prodinstr.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }

        // 其他事件，比如按钮的点击，数据有效性判断
        void addOtherEvnetHandler()
        {
            dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);

            setDataGridViewCombox();
        }

        private void setControlFalse()
        {
            foreach (Control c in this.Controls)
                c.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            bt日志.Enabled = true;
            bt打印.Enabled = true;
            cb打印机.Enabled = true;
        }
        private void setControlTrue()
        {
            foreach (Control c in this.Controls)
                c.Enabled = true;
            dataGridView1.ReadOnly = false;
            bt发送审核.Enabled = false;
            bt审核.Enabled = false;
            btn更改.Enabled = false;
        }
        void setEnableReadOnly()
        {
            //if (stat_user == 2)//管理员
            //{
            //    //控件都能点
            //    foreach (Control c in this.Controls)
            //        c.Enabled = true;
            //}
            //else if (stat_user == 1)//审核人
            //{
            //    if (stat_form == 0 || stat_form == 3 || stat_form == 2)//草稿,审核不通过，审核通过
            //    {
            //        //空间都不能点
            //        setControlFalse();

            //    }
            //    else//待审核
            //    {
            //        //发送审核不可点，其他都可点
            //        setControlTrue();
            //        bt审核.Enabled = true;
            //    }

            //}
            //else//操作员
            //{
            //    if (stat_form == 1 || stat_form == 2)//待接收，审核通过
            //    {
            //        //空间都不能点
            //        setControlFalse();
            //    }
            //    else//未审核与审核不通过
            //    {
            //        //发送审核，审核不能点
            //        setControlTrue();
            //    }
            //}

            if (Parameter.UserState.管理员 == _userState)
            {
                setControlTrue();
            }
            if (Parameter.UserState.审核员 == _userState)
            {
                if (Parameter.FormState.待审核 == _formState)
                {
                    setControlTrue();
                    bt审核.Enabled = true;
                }
                else if (Parameter.FormState.审核通过 == _formState)
                {
                    setControlFalse();
                    if (Convert.ToInt32(dt_prodinstr.Rows[0]["状态"]) == 2)
                        btn更改.Enabled = true;
                }
                else setControlFalse();
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
            }


        }

        // 设置自动计算类事件
        void addComputerEventHandler()
        { }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令编号"] = tb指令编号.Text;
            dr["生产设备"] = "分切机AA-EQU-035";
            dr["计划生产日期"] = DateTime.Now;

            dr["编制人"] = mySystem.Parameter.userName;
            dr["编制时间"] = DateTime.Now;
            dr["审批时间"] = DateTime.Now;
            dr["接收时间"] = DateTime.Now;
            dr["备注"] = "";

            dr["状态"] = 0;
            dr["审核是否通过"] = false;
            dr["审批人"] = "";
            dr["审核人"] = "";

            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 新建记录\n";
            dr["日志"] = log;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T生产指令表ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["序号"] = 0;
            dr["数量卷"] = 0;
            dr["数量米"] = 0;
            dr["分切后卷数"] = 1;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(string code)
        {
            dt_prodinstr = new DataTable("清洁分切工序生产指令");
            bs_prodinstr = new BindingSource();
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodinstr = new OleDbDataAdapter(@"select * from 清洁分切工序生产指令 where 生产指令编号='" + code + "'", mySystem.Parameter.connOle);
                cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
                da_prodinstr.Fill(dt_prodinstr);
            }
            else
            {
                da_prodinstrsql = new SqlDataAdapter(@"select * from 清洁分切工序生产指令 where 生产指令编号='" + code + "'", mySystem.Parameter.conn);
                cb_prodinstrsql = new SqlCommandBuilder(da_prodinstrsql);
                da_prodinstrsql.Fill(dt_prodinstr);
            }
            
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("清洁分切工序生产指令详细信息");
            bs_prodlist = new BindingSource();
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist = new OleDbDataAdapter("select * from 清洁分切工序生产指令详细信息 where T生产指令表ID=" + id, mySystem.Parameter.connOle);
                cb_prodlist = new OleDbCommandBuilder(da_prodlist);
                da_prodlist.Fill(dt_prodlist);
            }
            else
            {
                da_prodlistsql = new SqlDataAdapter("select * from 清洁分切工序生产指令详细信息 where T生产指令表ID=" + id, mySystem.Parameter.conn);
                cb_prodlistsql = new SqlCommandBuilder(da_prodlistsql);
                da_prodlistsql.Fill(dt_prodlist);
            }
            
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            tb指令编号.DataBindings.Clear();
            tb设备编号.DataBindings.Clear();
            dtp计划生产日期.DataBindings.Clear();

            tb备注.DataBindings.Clear();
            tb编制人.DataBindings.Clear();
            tb审批人.DataBindings.Clear();
            tb接收人.DataBindings.Clear();

            dtp编制日期.DataBindings.Clear();
            dtp审批日期.DataBindings.Clear();
            dtp接收日期.DataBindings.Clear();

        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;

            tb指令编号.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编号");
            tb设备编号.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产设备");
            dtp计划生产日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "计划生产日期");
            tb备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "备注");
            tb编制人.DataBindings.Add("Text", bs_prodinstr.DataSource, "编制人");
            tb审批人.DataBindings.Add("Text", bs_prodinstr.DataSource, "审批人");
            tb接收人.DataBindings.Add("Text", bs_prodinstr.DataSource, "接收人");
            dtp编制日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "编制时间");
            dtp审批日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "审批时间");
            dtp接收日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "接收时间");
        }
        // 内表和控件的绑定
        void innerBind()
        {
            //移除所有列
            //while (dataGridView1.Columns.Count > 0)
            //    dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            //setDataGridViewCombox();
            bs_prodlist.DataSource = dt_prodlist;
            dataGridView1.DataSource = bs_prodlist.DataSource;
            
            //Utility.setDataGridViewAutoSizeMode(dataGridView1);
            setDataGridViewColumns();
        }

        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            dataGridView1.Columns["数量卷"].HeaderText = "数量（卷）";
            dataGridView1.Columns["数量米"].HeaderText = "数量（米）"; 
        }
        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[6].ReadOnly = true;//数量米

            foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
            {
                //dgvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        private void bt添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr = writeInnerDefault(dr);

            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        private void bt删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.SelectedCells[0].RowIndex < 0)
                    return;
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            }
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            
            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            //刷新序号
            setDataGridViewRowNums();
        }
        //TODO：对于其他有上移下移的表都这样处理*************
        private void bt上移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count == 0)
                return;
            if (dataGridView1.SelectedCells.Count <= 0)
                return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (0 == index)
            {
                return;
            }
            DataRow currRow = dt_prodlist.Rows[index];
            DataRow desRow = dt_prodlist.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dt_prodlist.Rows.Add(desRow);

            for (int i = index - 1; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dt_prodlist.Rows[i];
                DataRow tdesRow = dt_prodlist.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dt_prodlist.Rows.Add(tdesRow);
            }
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            
            dt_prodlist.Clear();
            da_prodlist.Fill(dt_prodlist);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index - 1].Selected = true;

            //刷新序号
            setDataGridViewRowNums();
        }

        private void bt下移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count == 0)
                return;
            if (dataGridView1.SelectedCells.Count <= 0)
                return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (count - 1 == index)
            {
                return;
            }
            DataRow currRow = dt_prodlist.Rows[index];
            DataRow desRow = dt_prodlist.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dt_prodlist.Rows.Add(desRow);

            for (int i = index + 2; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dt_prodlist.Rows[i];
                DataRow tdesRow = dt_prodlist.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dt_prodlist.Rows.Add(tdesRow);
            }
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            
            dt_prodlist.Clear();
            da_prodlist.Fill(dt_prodlist);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index + 1].Selected = true;

            //刷新序号
            setDataGridViewRowNums();
        }

        public override void CheckResult()
        {
            //获得审核信息
            //dtp审批日期.Value = checkform.time;
            dt_prodinstr.Rows[0]["审批人"] = mySystem.Parameter.userName;
            dt_prodinstr.Rows[0]["审批时间"] = checkform.time;

            dt_prodinstr.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            if(checkform.ischeckOk)
                dt_prodinstr.Rows[0]["状态"] = 1;//待接收
            else
                dt_prodinstr.Rows[0]["状态"] = 0;//草稿

            //状态
            setControlFalse();


            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='清洁分切工序生产指令' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);
            }
            else
            {
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='清洁分切工序生产指令' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);
            }
            

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion;
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            bs_prodinstr.EndEdit();
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            }
            else
            {
                da_prodinstrsql.Update((DataTable)bs_prodinstr.DataSource);
            }
            

            base.CheckResult();
        }

        private void bt审核_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.Show();
        }

        private void bt发送审核_Click(object sender, EventArgs e)
        {
            //判断合法性
            if (!input_Judge())
            {
                bt发送审核.Enabled = false;
                return;
            }
            //写待审核表
            DataTable dt_temp= new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='清洁分切工序生产指令' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);

                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "清洁分切工序生产指令";
                    dr["对应ID"] = (int)dt_prodinstr.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                bs_temp.DataSource = dt_temp;
                da_temp.Update((DataTable)bs_temp.DataSource);
            }
            else
            {
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='清洁分切工序生产指令' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);

                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "清洁分切工序生产指令";
                    dr["对应ID"] = (int)dt_prodinstr.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                bs_temp.DataSource = dt_temp;
                da_temp.Update((DataTable)bs_temp.DataSource);
            }
            

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString()+log;

            dt_prodinstr.Rows[0]["审核人"] = "__待审核";
            dt_prodinstr.Rows[0]["审批时间"] = DateTime.Now;
            dt_prodinstr.Rows[0]["审批人"] = "__待审核";
            
            save();

            //空间都不能点
            setControlFalse();
            
        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dt_prodinstr.Rows[0]["日志"].ToString());
            (new mySystem.Other.LogForm()).setLog(dt_prodinstr.Rows[0]["日志"].ToString()).Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bt复制_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count <= 0)
                return;
            DataRow dr = dt_prodlist.NewRow();
            dr.ItemArray = dt_prodlist.Rows[dataGridView1.SelectedCells[0].RowIndex].ItemArray.Clone() as object[];
            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
              if (e.RowIndex < 0)
                return;

            //产品编码
            if (e.ColumnIndex == 3)
            {
                string str = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                str += "C";
                dataGridView1.Rows[e.RowIndex].Cells[7].Value = str;
            }
            if (e.ColumnIndex == 5)
            {
                float num_卷=float.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                dataGridView1.Rows[e.RowIndex].Cells[6].Value = num_卷 * 1000;
            }

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
            cb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }
        private void bt打印_Click(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            print(false);
            GC.Collect();
        }

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            int ind = 0;
            if (dt_prodlist.Rows.Count > 3)
            {
                //在第7行插入
                for (int i = 0; i < dt_prodlist.Rows.Count - 3; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[7, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
                ind = dt_prodlist.Rows.Count - 3;
            }

            my.Cells[3, 1].Value = "指令编号：" + dt_prodinstr.Rows[0]["生产指令编号"].ToString();
            my.Cells[3, 3].Value = "生产设备：" + dt_prodinstr.Rows[0]["生产设备"].ToString();
            my.Cells[3, 6].Value = Convert.ToDateTime(dt_prodinstr.Rows[0]["计划生产日期"]).ToString("yyyy年MM月dd日");
            for (int i = 0; i < dt_prodlist.Rows.Count; i++)
            {
                my.Cells[6 + i, 1] = i + 1;
                my.Cells[6 + i, 2] = dt_prodlist.Rows[i]["清洁前产品代码"].ToString();
                my.Cells[6 + i, 3] = dt_prodlist.Rows[i]["清洁前批号"].ToString();
                my.Cells[6 + i, 4] = dt_prodlist.Rows[i]["数量卷"].ToString();
                my.Cells[6 + i, 5] = dt_prodlist.Rows[i]["数量米"].ToString();
                my.Cells[6 + i, 6] = dt_prodlist.Rows[i]["清洁后产品代码"].ToString();
            }

            my.Cells[10 + ind, 1].Value = "备注：" + dt_prodinstr.Rows[0]["备注"].ToString();
            my.Cells[11 + ind, 1].Value = String.Format("编制人：{0}  {1}        审批人：{2}  {3}        接收人：{4}  {5}",
                dt_prodinstr.Rows[0]["编制人"].ToString(), Convert.ToDateTime(dt_prodinstr.Rows[0]["编制时间"]).ToString("yyyy年MM月dd日"),
                dt_prodinstr.Rows[0]["审批人"].ToString(), Convert.ToDateTime(dt_prodinstr.Rows[0]["审批时间"]).ToString("yyyy年MM月dd日"),
                dt_prodinstr.Rows[0]["接收人"].ToString(), Convert.ToDateTime(dt_prodinstr.Rows[0]["接收时间"]).ToString("yyyy年MM月dd日"));
        }

        //查找打印的表序号
        private int find_indexofprint()
        {
            List<int> list_id = new List<int>();
            string asql = "select * from 清洁分切工序生产指令 where 生产指令编号 = '" + instrcode + "'";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            for (int i = 0; i < tempdt.Rows.Count; i++)
                list_id.Add((int)tempdt.Rows[i]["ID"]);
            return list_id.IndexOf((int)dt_prodinstr.Rows[0]["ID"]) + 1;

        }

        public int print(bool b)
        {
            int label_打印成功 = 1;
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/cleancut/SOP-MFG-302-R01A 清洁分切生产指令.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = instrcode + "-01-" + find_indexofprint().ToString("D3") + " &P/" + wb.ActiveSheet.PageSetup.Pages.Count;  // &P 是页码

            if (b)
            {
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
                return 0;
            }
            else
            {
                int pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
                // 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch
                {
                    label_打印成功 = 0;
                }
                finally
                {
                    if (1 == label_打印成功)
                    {
                        string log = "\n=====================================\n";
                        log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 完成打印\n";
                        dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;
                        bs_prodinstr.EndEdit();
                        if (!mySystem.Parameter.isSqlOk)
                        {
                            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                        }
                        else
                        {
                            da_prodinstrsql.Update((DataTable)bs_prodinstr.DataSource);
                        }
                        
                    }
                    // 关闭文件，false表示不保存
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                    wb = null;
                    oXL = null;
                    my = null;
                }
                return pageCount;
            }  
        }

        private void btn更改_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dt_prodinstr.Rows[0]["状态"]) == 1)
            {
                MessageBox.Show("该指令还未被接收，无法更改!");
                return;
            }
            DataRow dr = dt_prodinstr.NewRow();
            dr.ItemArray = dt_prodinstr.Rows[0].ItemArray.Clone() as object[];
            dt_prodinstr.Rows[0]["审核人"] = "";
            string newcode = dr["生产指令编号"].ToString() + " 更改" + DateTime.Now.ToString("yyyyMMdd");
            dr["生产指令编号"] = newcode;
            dr["状态"] = 4;
            //写日志
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 更改生产指令计划\n";
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            dt_prodinstr.Rows.Add(dr);
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            }
            else
            {
                da_prodinstrsql.Update((DataTable)bs_prodinstr.DataSource);
            }
           
            readOuterData(newcode);

            int newid = (int)dt_prodinstr.Rows[dt_prodinstr.Rows.Count - 1]["ID"];
            int count = dt_prodlist.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                DataRow dr_list = dt_prodlist.NewRow();
                dr_list.ItemArray = dt_prodlist.Rows[i].ItemArray.Clone() as object[];
                dr_list["T生产指令表ID"] = newid;
                dt_prodlist.Rows.Add(dr_list);
            }
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            
            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            MessageBox.Show("更改成功");
            setFormState();
            setEnableReadOnly();
        }

        private void Instru_FormClosing(object sender, FormClosingEventArgs e)
        {
            writeDGVWidthToSetting(dataGridView1);
        }
    }
}
