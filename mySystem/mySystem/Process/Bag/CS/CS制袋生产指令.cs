﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.Bag.CS
{
    public partial class CS制袋生产指令 : BaseForm
    {


        // TODO : 注意处理生产指令的状态（是否接收等状态）
        // TODO ：要加到Mainform中去
        // TODO： 审核时要调用赵梦的函数
        // TODO: 打印  选打印机
        // TODO：构造函数添加参数mainform
        // TODO: 用正则表达式获取操作员和审核员姓名

        // 需要保存的状态
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        int _id;
        String _code;

        // 显示界面需要的信息
        List<String> ls产品名称, ls工艺, ls负责人, ls操作员, ls审核员;
        HashSet<String> hs产品代码,hs封边;
        HashSet<String> hs制袋内包白班负责人, hs制袋内包夜班负责人, hs外包白班负责人, hs外包夜班负责人;

        // DataGridView 中用到的一些变量
        List<Int32> li可选可输的列;


        // 数据库连接
        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/csbag.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;


        public CS制袋生产指令()
        {
            InitializeComponent();
            variableInit();
            getOuterOtherData();
            getPeople();
            setUseState();
            setFormState(true);
            setEnableReadOnly();
        }

        public CS制袋生产指令(int id)
        {
            // 显示前的准备工作
            InitializeComponent();
            variableInit();
            getOuterOtherData();
            getPeople();
            setUseState();

            // 读取数据并显示
            readOuterData(id);
            outerBind();
            setKeyInfoFromDataTable(id);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            getInnerOtherData();
            setDataGridViewColumn();
            innerBind();
            getPeople();
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();

            // 禁用
            btn查询插入.Enabled = false;
            tb生产指令编号.Enabled = false;
        }

        private void btn查询插入_Click(object sender, EventArgs e)
        {
            // 读取数据
            _code = tb生产指令编号.Text;
            readOuterData(_code);
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(dtOuter.Rows[0]["生产指令编号"].ToString());
                outerBind();
            }
            setKeyInfoFromDataTable(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            getInnerOtherData();
            setDataGridViewColumn();
            innerBind();
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();

            // 禁用自己
            btn查询插入.Enabled = false;
            tb生产指令编号.Enabled = false;
        }

        /// <summary>
        /// 所有变量实例化，一些固定的变量赋值
        /// </summary>
        void variableInit()
        {
            conn = new OleDbConnection(strConn);

            ls产品名称 = new List<string>();
            ls负责人 = new List<string>();
            ls工艺 = new List<string>();
            hs外包白班负责人 = new HashSet<string>();
            hs外包夜班负责人 = new HashSet<string>();
            hs制袋内包白班负责人 = new HashSet<string>();
            hs制袋内包夜班负责人 = new HashSet<string>();

            li可选可输的列 = new List<int>();
            li可选可输的列.Add(2);
            li可选可输的列.Add(8);
        }

        /// <summary>
        /// 获取当期显示的数据的关键信息，包括但不限于ID
        /// </summary>
        /// <param name="id"></param>
        void setKeyInfoFromDataTable(int id)
        {

            _id = id;
            _code = dtOuter.Rows[0]["生产指令编号"].ToString();
        }

        /// <summary>
        /// 获取操作员和审核员名单
        /// </summary>
        void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='CS制袋生产指令'", conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();

            ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();

        }

        /// <summary>
        /// 和外表显示相关的数据赋值，主要是下拉框的Items
        /// </summary>
        void getOuterOtherData()
        {
            OleDbDataAdapter da;
            DataTable dt;
            
            da = new OleDbDataAdapter("select * from 用户", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls负责人.Add(dr["用户名"].ToString());
                cmb负责人.Items.Add(dr["用户名"].ToString());
            }
            //　产品名称
            da = new OleDbDataAdapter("select * from 设置CS制袋产品", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach(DataRow dr in dt.Rows){
                ls产品名称.Add(dr["产品名称"].ToString());
                cmb产品名称.Items.Add(dr["产品名称"].ToString());
            }
            
            
            // 工艺
            da = new OleDbDataAdapter("select * from 设置CS制袋工艺", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls工艺.Add(dr["工艺名称"].ToString());
                cmb生产工艺.Items.Add(dr["工艺名称"].ToString());
            }


        }

        /// <summary>
        /// 和内表显示相关的数据赋值，主要是下拉框的Items
        /// 对可选可输的控件要记得将DataTable中的值也加入Items
        /// </summary>
        void getInnerOtherData()
        {
            OleDbDataAdapter da;
            DataTable dt;
            hs产品代码 = new HashSet<string>();
            hs封边 = new HashSet<string>();
            //　产品代码
            da = new OleDbDataAdapter("select * from 设置CS制袋产品代码", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                hs产品代码.Add(dr["产品代码"].ToString());
            }
            
            // 封边
            da = new OleDbDataAdapter("select * from 设置CS制袋封边", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                hs封边.Add(dr["封边名称"].ToString());
            }

            // 自定义数据
            foreach (DataRow dr in dtInner.Rows)
            {
                hs产品代码.Add(dr["产品代码"].ToString());
                hs封边.Add(dr["封边"].ToString());
            }
        }

        /// <summary>
        /// 根据外表ID读取外表数据
        /// </summary>
        /// <param name="id"></param>
        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 生产指令 where ID=" + id + "", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("生产指令");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        /// <summary>
        /// 根据其他条件读取外表数据
        /// </summary>
        /// <param name="code"></param>
        void readOuterData(String code)
        {
            
            daOuter = new OleDbDataAdapter("select * from 生产指令 where 生产指令编号='" + code + "'", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("生产指令");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }


        /// <summary>
        /// 外表写入默认值
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令编号"] = _code;
            dr["生产设备"] = "制袋机 AA-EQU-001";
            dr["计划生产日期"] = DateTime.Now;
            dr["制袋物料名称1"] = "Tyvek印刷卷材";
            dr["制袋物料名称2"] = "药品包装用聚乙烯膜（XP1）";
            dr["制袋物料名称3"] = "蒸汽灭菌指示剂";
            dr["内包物料名称1"] = "内包装袋";
            dr["内包物料名称2"] = "内标签";
            dr["外包物料名称1"] = "外标签";
            dr["外包物料名称2"] = "纸箱";
            dr["外包物料批号2"] = "————————";
            dr["外包物料名称3"] = "内衬袋";
            dr["外包物料代码3"] = "专用袋";
            dr["外包物料批号3"] = "————————";
            dr["操作员"] = mySystem.Parameter.userName;
            dr["操作时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["接收时间"] = DateTime.Now;
            dr["状态"] = 0;
            return dr;
        }


        /// <summary>
        /// 外表和控件的绑定
        /// 注意变量名命名规则，区分该绑定的和不该绑定控件
        /// </summary>
        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            foreach (Control c in this.Controls)
            {
                if (c.Name == "cmb负责人") continue;
                if (c.Name.StartsWith("tb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(2),false, DataSourceUpdateMode.OnPropertyChanged);
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings.Clear();
                    (c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    (c as ComboBox).DataBindings.Clear();
                    (c as ComboBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                    ControlUpdateMode cm = (c as ComboBox).DataBindings["Text"].ControlUpdateMode;
                    DataSourceUpdateMode dm = (c as ComboBox).DataBindings["Text"].DataSourceUpdateMode;
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                    ControlUpdateMode cm = (c as DateTimePicker).DataBindings["Value"].ControlUpdateMode;
                    DataSourceUpdateMode dm = (c as DateTimePicker).DataBindings["Value"].DataSourceUpdateMode;
                }
            }
        }

        /// <summary>
        /// 根据外表ID读取内表信息
        /// </summary>
        /// <param name="outerID"></param>
        void readInnerData(int outerID)
        {
            daInner = new OleDbDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + outerID, conn);
            dtInner = new DataTable("生产指令详细信息");
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }


        /// <summary>
        /// 内表写默认值
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T生产指令ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            dr["计划产量只"] = 0;
            dr["内包装规格每包只数"] = 0;
            dr["外包规格"] = 0;
            dr["封边"] = "底封";
            return dr;
        }

        /// <summary>
        /// 内表绑定
        /// </summary>
        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;

        }

        /// <summary>
        /// 设置DataGridView的列
        /// 该函数主要设置列的类型，尤其要注意变成下拉框的列
        /// 列的可见性，只读性，HeadText都不要在这里设置，放在DataBindComplete事件中处理
        /// </summary>
        void setDataGridViewColumn()
        {
            dataGridView1.Columns.Clear();
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            DataGridViewCheckBoxColumn ckbc;
  
            // 先把所有的列都加好，基本属性附上
            foreach (DataColumn dc in dtInner.Columns)
            {
                // 要下拉框的特殊处理
                if (dc.ColumnName == "产品代码")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    foreach (String s in hs产品代码)
                    {
                        cbc.Items.Add(s);
                    }
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "封边")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    foreach (String s in hs封边)
                    {
                        cbc.Items.Add(s);
                    }
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "内标签")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("中文");
                    cbc.Items.Add("英文");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "外标签")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("中文");
                    cbc.Items.Add("英文");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                // 根据数据类型自动生成列的关键信息
                switch (dc.DataType.ToString())
                {
                        
                    case "System.Int32":
                    case "System.String":
                    case "System.Double":
                    case "System.DateTime":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView1.Columns.Add(tbc);
                        break;
                    case "System.Boolean":
                        ckbc = new DataGridViewCheckBoxColumn();
                        ckbc.HeaderText = dc.ColumnName;
                        ckbc.Name = dc.ColumnName;
                        ckbc.ValueType = dc.DataType;
                        ckbc.DataPropertyName = dc.ColumnName;
                        ckbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView1.Columns.Add(ckbc);
                        break;
                }
            }
        }

        /// <summary>
        /// 设置用户状态
        /// </summary>
        void setUseState()
        {
            _userState = Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |=Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |=Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if ( Parameter.UserState.NoBody== _userState)
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

        /// <summary>
        /// 设置窗体状态，传true表示无数据状态，不传参数则根据 审核员 信息自动判断
        /// 如果点开窗体后，需要操作控件才能显示数据的，增加一个 无数据 状态，例如生产指令
        /// 点开窗体就能显示值的，没有这个状态，例如运行记录
        /// </summary>
        /// <param name="newForm"></param>
        
        void setFormState(bool newForm = false)
        {
            if (newForm)
            {

                _formState = Parameter.FormState.无数据;
                return;
            }
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }

        /// <summary>
        /// 根据用户和窗体状态，设置控件的Enable和ReadOnly
        /// </summary>
        void setEnableReadOnly()
        {

            if (Parameter.FormState.无数据 == _formState)
            {
                setControlFalse();
                btn查询插入.Enabled = true;
                tb生产指令编号.ReadOnly = false;
                return ;
            }
            if (Parameter.UserState.管理员 == _userState)
            {
                setControlTrue();
            }
            if (Parameter.UserState.审核员 == _userState)
            {
                if (Parameter.FormState.待审核 == _formState)
                {
                    setControlTrue();
                    btn审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核通过 == _formState) setControlTrue();
                else setControlFalse();
            }

  
        }

        /// <summary>
        /// 默认控件可用状态
        /// </summary>
        void setControlTrue()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
            foreach (Control c in this.Controls)
            {
                if (c.Name == "btn查询插入") continue;
                if (c.Name == "tb生产指令编码") continue;
                if (c is TextBox)
                {
                    (c as TextBox).ReadOnly = false;
                }
                else if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = false;
                }
                else
                {
                    c.Enabled = true;
                }
            }
            // 保证这两个按钮一直是false
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            
        }

        /// <summary>
        /// 默认控件不可用状态
        /// </summary>
        void setControlFalse()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
            foreach (Control c in this.Controls)
            {
                if (c.Name == "btn查询插入") continue;
                if (c.Name == "tb生产指令编码") continue;
                if (c is TextBox)
                {
                    (c as TextBox).ReadOnly = true;
                }
                else if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = true;
                }
                else
                {
                    c.Enabled = false;
                }
            }
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;

        }

        /// <summary>
        /// 不好归类的属性设置或者事件注册
        /// </summary>
        void addOtherEvenHandler()
        {
            // TODO 其他无法分类的代码放在这里
            dataGridView1.AllowUserToAddRows = false;
            // 实现下拉框可选可输
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.CellValidating += dataGridView1_CellValidating;

            // 设置DataGridVew的可见性和只读属性等都放在绑定结束之后
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
        }

        /// <summary>
        /// 计算类事件的注册
        /// </summary>
        void addComputerEventHandler()
        {
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }


        /// <summary>
        /// 确保控件和DataTable中的数据能同步的方法
        /// 凡是需要在程序中通过代码来改变控件值时，请用本方法避免不同步的情况
        /// 第一个参数是控件的变量名，第二个参数是要填入的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        void outerDataSync(String name, String val)
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name == name)
                {
                    c.Text = val;
                    c.DataBindings[0].WriteValue();
                }
            }
        }

        /// <summary>
        /// 数据有效性验证
        /// </summary>
        /// <returns></returns>
        private bool dataValidate()
        {
            dataGridView1.DataError += dataGridView1_DataError;

            // TODO 更多条件有待补充
            if (cmb产品名称.Text == "") return false;
            if (cmb生产工艺.Text == "") return false;
            if (dataGridView1.Rows.Count == 0 || dataGridView1.Rows.Count > 1) return false;
            if (tb接收人.Text == "") return false;
            return true;
        }


        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "产品代码（规格型号）";
            dataGridView1.Columns[3].HeaderText = "计划产量（只）";
            dataGridView1.Columns[4].HeaderText = "内包装规格（只/包）";
            dataGridView1.Columns[9].HeaderText = "外包装规格（只/箱）";
        }

        void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (li可选可输的列.IndexOf(e.ColumnIndex) >= 0)
            {
                object eFV = e.FormattedValue;
                DataGridViewComboBoxColumn cbc = dataGridView1.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
                if (!cbc.Items.Contains(eFV))
                {
                    cbc.Items.Add(eFV);
                    dataGridView1.SelectedCells[0].Value = eFV;
                }
            }
        }

        void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv =  (sender as DataGridView);

            if (dgv.SelectedCells.Count == 0) return;
            int colIdx = dgv.SelectedCells[0].ColumnIndex;
            if (li可选可输的列.IndexOf(colIdx) >= 0)
            {
                ComboBox c = e.Control as ComboBox;
                if (c != null) c.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }
       

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            int i计划产量只 = Convert.ToInt32(dataGridView1[3, e.RowIndex].Value);
            int i内包装规格 = Convert.ToInt32(dataGridView1[4, e.RowIndex].Value);
            int i外包装规格 = Convert.ToInt32(dataGridView1[9, e.RowIndex].Value);
            try
            {


                switch (e.ColumnIndex)
                {       
                    // 计划产量
                    case 3:
                        //灭菌指示剂
                        //dtOuter.Rows[0]["制袋物料领料量3"] = i计划产量只.ToString();
                        outerDataSync("tb制袋物料领料量3", i计划产量只.ToString());
                        //内包装
                        //tb内包物料领料量1.Text = (i计划产量只 / i内包装规格 * 2).ToString();
                        //dtOuter.Rows[0]["内包物料领料量1"] = (i计划产量只 / i内包装规格 * 2).ToString();
                        outerDataSync("tb内包物料领料量1", (i计划产量只 / i内包装规格 * 2).ToString());
                        // 内标签
                        //tb内包物料领料量2.Text = (i计划产量只 / i内包装规格).ToString();
                        //dtOuter.Rows[0]["内包物料领料量2"] = (i计划产量只 / i内包装规格).ToString();
                        outerDataSync("tb内包物料领料量2", (i计划产量只 / i内包装规格).ToString());
                        // 外标签
                        //tb外包物料领料量1.Text = (i计划产量只 / i外包装规格 * 2).ToString();
                        //dtOuter.Rows[0]["外包物料领料量1"] = (i计划产量只 / i外包装规格 * 2).ToString();
                        outerDataSync("tb外包物料领料量1", (i计划产量只 / i外包装规格 * 2).ToString());
                        // 纸箱
                        //tb外包物料领料量2.Text = (i计划产量只 / i外包装规格).ToString();
                        //dtOuter.Rows[0]["外包物料领料量2"] = (i计划产量只 / i外包装规格).ToString();
                        outerDataSync("tb外包物料领料量2", (i计划产量只 / i外包装规格).ToString());
                        // 内衬袋
                        //dtOuter.Rows[0]["外包物料领料量3"] = (i计划产量只 / i外包装规格).ToString();
                        outerDataSync("tb外包物料领料量3", (i计划产量只 / i外包装规格).ToString());
                        //tb外包物料领料量3.Text = (i计划产量只 / i外包装规格).ToString();
                        break;
                    // 内包装规格
                    case 4:
                        //内包装
                        //tb内包物料领料量1.Text = (i计划产量只 / i内包装规格 * 2).ToString();
                        //dtOuter.Rows[0]["内包物料领料量1"] = (i计划产量只 / i内包装规格 * 2).ToString();
                        outerDataSync("tb内包物料领料量1", (i计划产量只 / i内包装规格 * 2).ToString());
                        // 内标签
                        //tb内包物料领料量2.Text = (i计划产量只 / i内包装规格).ToString();
                        //dtOuter.Rows[0]["内包物料领料量2"] = (i计划产量只 / i内包装规格).ToString();
                        outerDataSync("tb内包物料领料量2", (i计划产量只 / i内包装规格).ToString());
                        break;
                    // 外包装规格
                    case 9:
                        // 外标签
                        //tb外包物料领料量1.Text = (i计划产量只 / i外包装规格 * 2).ToString();
                        //dtOuter.Rows[0]["外包物料领料量1"] = (i计划产量只 / i外包装规格 * 2).ToString();
                        outerDataSync("tb外包物料领料量1", (i计划产量只 / i外包装规格 * 2).ToString());
                        // 纸箱
                        //tb外包物料领料量2.Text = (i计划产量只 / i外包装规格).ToString();
                        //dtOuter.Rows[0]["外包物料领料量2"] = (i计划产量只 / i外包装规格).ToString();
                        outerDataSync("tb外包物料领料量2", (i计划产量只 / i外包装规格).ToString());
                        // 内衬袋
                        //tb外包物料领料量3.Text = (i计划产量只 / i外包装规格).ToString();
                        //dtOuter.Rows[0]["外包物料领料量3"] = (i计划产量只 / i外包装规格).ToString();
                        outerDataSync("tb外包物料领料量3", (i计划产量只 / i外包装规格).ToString());
                        break;
                }
                //String a = cmb产品名称.Text;
            }
            catch (System.DivideByZeroException)
            {

            }
        }


        

       

        private void btn保存_Click(object sender, EventArgs e)
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            outerBind();

            
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            if (_userState == Parameter.UserState.操作员) btn提交审核.Enabled = true;
            
        }

        private void btn外包白班_Click(object sender, EventArgs e)
        {
            hs外包白班负责人.Add(cmb负责人.SelectedItem.ToString());
            //dtOuter.Rows[0]["外包白班负责人"] = String.Join(",", hs外包白班负责人.ToList<String>().ToArray());
            outerDataSync("tb外包白班负责人", String.Join(",", hs外包白班负责人.ToList<String>().ToArray()));
            //tb外包白班负责人.Text = 
        }

        private void btn外包夜班_Click(object sender, EventArgs e)
        {
            hs外包夜班负责人.Add(cmb负责人.SelectedItem.ToString());
            //dtOuter.Rows[0]["外包夜班负责人"] = String.Join(",", hs外包夜班负责人.ToList<String>().ToArray());
            outerDataSync("tb外包夜班负责人", String.Join(",", hs外包夜班负责人.ToList<String>().ToArray()));
            //tb外包夜班负责人.Text = String.Join(",", hs外包夜班负责人.ToList<String>().ToArray());
        }

        private void btn制袋内包白班_Click(object sender, EventArgs e)
        {
            hs制袋内包白班负责人.Add(cmb负责人.SelectedItem.ToString());
            //dtOuter.Rows[0]["制袋内包白班负责人"] = String.Join(",", hs制袋内包白班负责人.ToList<String>().ToArray());
            outerDataSync("tb制袋内包白班负责人", String.Join(",", hs制袋内包白班负责人.ToList<String>().ToArray()));
            //tb制袋内包白班负责人.Text = String.Join(",", hs制袋内包白班负责人.ToList<String>().ToArray());
        }

        private void btn制袋内包夜班_Click(object sender, EventArgs e)
        {
            hs制袋内包夜班负责人.Add(cmb负责人.SelectedItem.ToString());
            //dtOuter.Rows[0]["制袋内包夜班负责人"] = String.Join(",", hs制袋内包夜班负责人.ToList<String>().ToArray());
            outerDataSync("tb制袋内包夜班负责人", String.Join(",", hs制袋内包夜班负责人.ToList<String>().ToArray()));
            //tb制袋内包夜班负责人.Text = String.Join(",", hs制袋内包夜班负责人.ToList<String>().ToArray());
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {

            if (!dataValidate())
            {
                MessageBox.Show("数据填写不完整，请仔细检查！");
                return;
            }

            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;
            
            da = new OleDbDataAdapter("select * from 待审核 where 表名='生产指令' and 对应ID=" + _id, conn);
            cb = new OleDbCommandBuilder(da);
            
            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "生产指令";
            dr["对应ID"] = _id;
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = "__待审核";
            String log = "===================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            log+= "\n操作员："+mySystem.Parameter.userName+" 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            {
                mySystem.Other.LogForm logForm = new Other.LogForm();
                logForm.setLog(dtOuter.Rows[0]["日志"].ToString()).Show();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
            
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            // TODO 弹出赵梦的窗口

            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;

            da = new OleDbDataAdapter("select * from 待审核 where 表名='生产指令' and 对应ID=" + _id, conn);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核是否通过"] = true;
            String log = "===================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            log += "\n审核员：" + mySystem.Parameter.userName + " 审核完毕\n";
            log += "审核结果为：通过\n";
            log += "审核意见为：无\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;
        }



        private void btn添加_Click(object sender, EventArgs e)
        {
            if (dtInner.Rows.Count >= 1)
            {
                MessageBox.Show("请勿添加多行！");
                return;
            }
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);
        }


        // 验证数据有效性


        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }
 
    }
}
