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
    public partial class 产品外观和尺寸检验记录 : BaseForm
    {
        // TODO   需要从Parameter 中读取生产指令ID或编号，这里假装填写当前生产指令编号和ID
        string CODE = "1";
        int ID = 11;
        // TODO : 注意处理生产指令的状态
        // TODO： 审核时要调用赵梦的函数
        // TODO: 打印
        // TODO：要加到mainform中去，现在连按钮都没有

        // 需要保存的状态
        /// <summary>
        /// 0:操作员，1：审核员，2：管理员
        /// </summary>
        int _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        int _formState;
        // 当前数据在自己表中的id
        int _id;
        String _code;

        // 显示界面需要的信息
        String str产品代码;
        String str产品批号;
        String str生产指令编号;
        Int32 i生产指令ID;
        List<String> ls操作员;
        List<String> ls审核员;

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


        public 产品外观和尺寸检验记录()
        {
            InitializeComponent();
            variableInit();
            getOtherData();
            getPeople();
            readOuterData();
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData();
                outerBind();
            }
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumn();
            innerBind();

            setFormVariable(Convert.ToInt32(dtOuter.Rows[0]["ID"]));

            setUseState();
            setFormState();
            setEnableReadOnly();

            addComputerEventHandler();
            addOtherEvenHandler();
        }

        public 产品外观和尺寸检验记录(int id)
        {
            // 待显示
            InitializeComponent();
            variableInit(id);
            getOtherData();
            getPeople();
            

            // 读取数据
            readOuterData(id);
            outerBind();
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumn();
            innerBind();

            // 获取和显示内容有关的变量
            setFormVariable(id);

            // 设置状态和控件可用性
            setUseState();
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();
        }

        void variableInit()
        {
            conn = new OleDbConnection(strConn);
            conn.Open();
            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            i生产指令ID = ID;
            str生产指令编号 = CODE;
        }

        void variableInit(int id)
        {
            conn = new OleDbConnection(strConn);
            conn.Open();
            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录 where ID=" + id, conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            i生产指令ID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
             da = new OleDbDataAdapter("select * from 生产指令 where ID=" + i生产指令ID, conn);
             dt = new DataTable("temp");
            da.Fill(dt);
            str生产指令编号 = dt.Rows[0]["生产指令编号"].ToString();
        }

        void getOtherData()
        {
            // 读取用于显示界面的重要信息
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + i生产指令ID, conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            str产品代码 = dt.Rows[0]["产品代码"].ToString();
            str产品批号 = dt.Rows[0]["产品批号"].ToString();

        }

        void setUseState()
        {
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState = 0;
            else if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState = 1;
            else _userState = 2;
        }

        // 读取数据，根据自己表的ID
        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录 where ID=" + id, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("产品外观和尺寸检验记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        // 读取数据，无参数表示从Paramter中读取数据
        void readOuterData()
        {
            daOuter = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录 where 生产指令ID=" + ID, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("产品外观和尺寸检验记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = i生产指令ID;
            dr["产品代码"] = str产品代码;
            dr["产品批号"] = str产品批号;
            dr["生产日期"] = DateTime.Now;
            dr["操作员"] = mySystem.Parameter.userName;
            dr["操作日期"] = DateTime.Now;
            dr["审核日期"] = DateTime.Now;
            dr["抽检量合计"] = 0;
            dr["游离异物合计"] = 0;
            dr["内含黑点晶点合计"] = 0;
            dr["热封线不良合计"] = 0;
            dr["其他合计"] = 0;
            dr["不良合计"] = 0;
            return dr;
        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("tb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(2));
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
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                }
            }
        }
        void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录详细信息 where T产品外观和尺寸检验记录ID=" + dtOuter.Rows[0]["ID"], conn);
            dtInner = new DataTable("产品外观和尺寸检验记录详细信息");
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }
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
                if (dc.ColumnName == "判定外观检查")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("合格");
                    cbc.Items.Add("不合格");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "判定尺寸检测")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("合格");
                    cbc.Items.Add("不合格");
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

            // 然后修改其他特殊属性
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            
            dataGridView1.Columns[2].HeaderText = "抽样时间（外观）";
            dataGridView1.Columns[3].HeaderText = "抽样量（外观）";
            dataGridView1.Columns[8].ReadOnly = true;
            dataGridView1.Columns[9].HeaderText = "判定（外观）";
            dataGridView1.Columns[10].HeaderText = "抽样时间（尺寸）";
            dataGridView1.Columns[14].HeaderText = "判定（尺寸）";
        }


        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T产品外观和尺寸检验记录ID"] = dtOuter.Rows[0]["ID"];
            dr["抽样时间外观检查"] = DateTime.Now;
            dr["抽检量外观检查"] = 0;
            dr["游离异物"] = 0;
            dr["内含黑点晶点"] = 0;
            dr["热封线不良"] = 0;
            dr["其他"] = 0;
            dr["不良合计"] = 0;
            dr["判定外观检查"] = "合格";
            dr["抽检时间尺寸检测"] = DateTime.Now;
            dr["抽检量尺寸检测"] = 0;
            dr["宽"] = 0;
            dr["长"] = 0;
            dr["判定尺寸检测"] = "合格";
            return dr;
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;
        }

        // 获取和显示内容有关的变量
        void setFormVariable(int id)
        {
            _id = id;
        }

        // 设置状态和控件可用性
        void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='产品外观和尺寸检验记录'", conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();

            ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();

        }

        void setFormState()
        {
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = 1;
            else
            {
                if (b) _formState = 2;
                else _formState = 3;
            }
        }
        void setEnableReadOnly()
        {
            
            if (2 == _userState)
            {
                setControlTrue();
            }
            if (1 == _userState)
            {
                if (1 == _formState)
                {
                    setControlTrue();
                    btn审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (0 == _userState)
            {
                if (0 == _formState || 3 == _formState) setControlTrue();
                else setControlFalse();
            }
        }

        void setControlTrue()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
            foreach (Control c in this.Controls)
            {
                
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

        void setControlFalse()
        {
            foreach (Control c in this.Controls)
            {
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

        // 事件部分
        void addComputerEventHandler()
        {
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int sum;
            switch (e.ColumnIndex)
            {
                // 抽检量合计
                case 3:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["抽检量外观检查"]);
                    }
                    dtOuter.Rows[0]["抽检量合计"] = sum;
                    break;
                // 游离异物合计
                case 4:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["游离异物"]);
                    }
                    dtOuter.Rows[0]["游离异物合计"] = sum;
                    break;                
                // 内含黑点晶点合计
                case 5:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["内含黑点晶点"]);
                    }
                    dtOuter.Rows[0]["内含黑点晶点合计"] = sum;
                    break;
                // 热封线不良合计
                case 6:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["热封线不良"]);
                    }
                    dtOuter.Rows[0]["热封线不良合计"] = sum;
                    break;
                // 其他合计
                case 7:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["其他"]);
                    }
                    dtOuter.Rows[0]["其他合计"] = sum;
                    break;
                //// 不良合计
                //case 8:
                //    sum = 0;
                //    foreach (DataRow dr in dtInner.Rows)
                //    {
                //        sum += Convert.ToInt32(dr["不良合计"]);
                //    }
                //    dtOuter.Rows[0]["不良合计"] = sum;
                //    break;

            }
            if (e.ColumnIndex >= 3 && e.ColumnIndex <= 7)
            {
                sum = 0;
                for (int i = 3; i <= 7; ++i)
                {
                    sum += Convert.ToInt32(dtInner.Rows[e.RowIndex][i]);
                }
                dtInner.Rows[e.RowIndex]["不良合计"] = sum;
                // 为什么DataGridVew中的值不会及时刷新？
                dataGridView1.Rows[e.RowIndex].Cells["不良合计"].Value = sum;
                sum = 0;
                foreach (DataRow dr in dtInner.Rows)
                {
                    sum += Convert.ToInt32(dr["不良合计"]);
                }
                dtOuter.Rows[0]["不良合计"] = sum;
            }
        }
        void addOtherEvenHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
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

            if (_userState == 0) btn提交审核.Enabled = true;
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);
        }

        private void btn上移_Click(object sender, EventArgs e)
        {
            int count = dtInner.Rows.Count;
            if (dataGridView1.SelectedCells.Count == 0) return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (0 == index)
            {
                return;
            }
            DataRow currRow = dtInner.Rows[index];
            DataRow desRow = dtInner.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dtInner.Rows.Add(desRow);

            for (int i = index - 1; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dtInner.Rows[i];
                DataRow tdesRow = dtInner.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dtInner.Rows.Add(tdesRow);
            }
            daInner.Update((DataTable)bsInner.DataSource);
            dtInner.Clear();
            daInner.Fill(dtInner);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index - 1].Selected = true;
        }

        private void btn下移_Click(object sender, EventArgs e)
        {
            int count = dtInner.Rows.Count;
            if (dataGridView1.SelectedCells.Count == 0) return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (count - 1 == index)
            {
                return;
            }
            DataRow currRow = dtInner.Rows[index];
            DataRow desRow = dtInner.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dtInner.Rows.Add(desRow);

            for (int i = index + 2; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dtInner.Rows[i];
                DataRow tdesRow = dtInner.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dtInner.Rows.Add(tdesRow);
            }
            daInner.Update((DataTable)bsInner.DataSource);
            dtInner.Clear();
            daInner.Fill(dtInner);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index + 1].Selected = true;
        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0) return;
            dtInner.Rows[dataGridView1.SelectedCells[0].RowIndex].Delete();
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            // 重新计算合计
            int sum = 0;
            foreach (DataRow dr in dtInner.Rows)
            {
                sum += Convert.ToInt32(dr["不良合计"]);
            }
            dtOuter.Rows[0]["不良合计"] = sum;

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

            da = new OleDbDataAdapter("select * from 待审核 where 表名='产品外观和尺寸检验记录' and 对应ID=" + _id, conn);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "产品外观和尺寸检验记录";
            dr["对应ID"] = _id;
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = "__待审核";
            String log = "===================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            log += "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        bool dataValidate()
        {
            return true;
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(dtOuter.Rows[0]["日志"].ToString());
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

            da = new OleDbDataAdapter("select * from 待审核 where 表名='产品外观和尺寸检验记录' and 对应ID=" + _id, conn);
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


    }
}
