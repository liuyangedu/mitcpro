﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace mySystem.Process.Bag.BTV
{
    public partial class BPV批生产记录 :mySystem.BaseForm
    {
        List<string> record = null;
        List<Int32> noid = null;
        int totalPage = 4;
        int _生产指令ID;
        string _生产指令;

        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;


        OleDbDataAdapter daOuter;
        OleDbCommandBuilder cbOuter;
        DataTable dtOuter;
        BindingSource bsOuter;

        List<String> ls操作员, ls审核员;
        List<String> tableName;
        CheckForm ckform;

        public BPV批生产记录(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            _生产指令ID = mySystem.Parameter.bpvbagInstruID;
            _生产指令 = mySystem.Parameter.bpvbagInstruction;
            tb生产指令编号.Text = _生产指令;
            init();

            getPeople();
            setUseState();
            readOuterData(_生产指令ID);
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(_生产指令ID);
                outerBind();
            }
            setFormState();
            setEnableReadOnly();

        }


        public BPV批生产记录(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 批生产记录表 where ID=" + id, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            _生产指令 = dt.Rows[0]["生产指令编号"].ToString();
            _生产指令ID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            tb生产指令编号.Text = _生产指令;
            init();

            getPeople();
            setUseState();
            readOuterData(_生产指令ID);
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(_生产指令ID);
                outerBind();
            }
            setFormState();
            setEnableReadOnly();

        }


        private void init()
        {
            record = new List<string>(); tableName = new List<string>();
            //record.Add("SOP-MFG-105-R01 制袋工序批生产记录封面.xlsx");
            //record.Add("SOP-MFG-303-R01A 2#生产指令.xlsx");
            record.Add("SOP-MFG-303-R02A SOP-MFG-102-R01A  生产领料使用记录.xlsx"); tableName.Add("生产领料使用记录");
            record.Add("SOP-MFG-102-R01A SOP-MFG-412-R01A  底封机运行记录.xlsx"); tableName.Add("底封机运行记录");
            record.Add("SOP-MFG-109-R01A 产品内包装记录.xlsx"); tableName.Add("产品内包装记录");
            record.Add("SOP-MFG-110-R01A 清场记录.xlsx"); tableName.Add("清场记录");
            record.Add("SOP-MFG-306-R02A  BPV生产前确认记录.xlsx"); tableName.Add("BPV生产前确认记录");
            record.Add("SOP-MFG-306-R04A  BPV切管记录.xlsx"); tableName.Add("BPV切管记录");
            record.Add("SOP-MFG-306-R05A  BPV装配确认记录.xlsx"); tableName.Add("BPV装配确认记录");
            record.Add("SOP-MFG-306-R06A  2D袋体生产记录.xlsx"); tableName.Add("2D袋体生产记录");
            record.Add("SOP-MFG-306-R08A  关键尺寸确认记录.xlsx"); tableName.Add("BPV关键尺寸确认记录");
            record.Add("SOP-MFG-306-R09A  原材料分装记录.xlsx"); tableName.Add("原材料分装记录");
            record.Add("SOP-MFG-415-R01A  泄漏测试记录.xlsx"); tableName.Add("泄漏测试记录");
            record.Add("SOP-MFG-417-R01A  2D袋体与船型接口热合记录.xlsx"); tableName.Add("2DBag袋体与船型接口热合记录");
            record.Add("SOP-MFG-418-R01A  瓶口焊接机运行记录.xlsx"); tableName.Add("瓶口焊接机运行记录");
            record.Add("SOP-MFG-501-R01A  多功能热合机运行记录.xlsx"); tableName.Add("多功能热合机运行记录");
            record.Add("SOP-MFG-502-R01A  3D袋体生产记录.xlsx"); tableName.Add("3D袋体生产记录");
            record.Add("SOP-MFG-503-R01A  单管口热合机运行记录.xlsx"); tableName.Add("单管口热合机运行记录");
            record.Add("SOP-MFG-505-R01A  90度热合机运行记录.xlsx"); tableName.Add("90度热合机运行记录");
            record.Add("SOP-MFG-506-R01A  封口热合机运行记录.xlsx"); tableName.Add("封口热合机运行记录");
            record.Add("SOP-MFG-508-R01A  打孔及与图纸确认记录.xlsx"); tableName.Add("打孔及与图纸确认记录");
            record.Add("SOP-MFG-108-R01A  制袋岗位交接班记录.xlsx"); tableName.Add("岗位交接班记录");
            record.Add("SOP-MFG-111-R01A 产品外包装记录.xlsx"); tableName.Add("产品外包装记录表");
            record.Add("SOP-MFG-102-R02A 生产退料记录.xlsx"); tableName.Add("生产退料记录表");
            record.Add("SOP-MFG-201-R01A 洁净区温湿度记录.xlsx"); tableName.Add("洁净区温湿度记录表");
            record.Add("QB-PA-PP-03-R01A 产品外观和尺寸检验记录.xlsx"); tableName.Add("产品外观和尺寸检验记录");
            record.Add("QB-PA-PP-03-R02A 产品热合强度检验记录.xlsx"); tableName.Add("产品热合强度检验记录");
                
              
            
            //tableName.Add("生产指令");
            
            
            

            initrecord();
            initly();
        }

        private void initly()
        {
            // 读取各表格数据，并显示页数

            OleDbDataAdapter da;
            DataTable dt;
            noid = new List<int>();

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                dgvr.Cells[0].Value = false;
            }

            dataGridView1.Columns[totalPage].ReadOnly = true;
           
            //dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            //dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            // 生产指令
            int temp;
            int idx = 0;
            int tempid;

            
            for (int i = 0; i < tableName.Count; i++)
            {
                da = new OleDbDataAdapter("select * from "+tableName[i]+" where  生产指令ID=" + _生产指令ID + "", mySystem.Parameter.connOle);
                dt = new DataTable("生产指令");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                    for (int j = 0; j < temp; ++j)
                    {
                        dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                    }
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                idx++;
            }

            


            // 另外一个datagridview
            // 读内包装
            da = new OleDbDataAdapter("select 产品代码,生产批号,产品数量只数合计B as 生产数量 from 产品内包装记录 where 生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable();
            da.Fill(dt);

            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.DataSource = dt;
            dataGridView2.ReadOnly = true;
            Utility.setDataGridViewAutoSizeMode(dataGridView2);
        }
        private void initrecord()
        {
            for (int i = 0; i < record.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[2].Value = i + 1;
                dr.Cells[3].Value = record[i];
                dataGridView1.Rows.Add(dr);
            }
        }

        

        void disableRow(int rowidx)
        {
            dataGridView1.Rows[rowidx].Cells[totalPage].Value = 0;
            dataGridView1.Rows[rowidx].ReadOnly = true;
            noid.Add(rowidx);
            dataGridView1.Rows[rowidx].DefaultCellStyle.BackColor = Color.Gray;
        }


        private void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='制袋工序批生产记录'", mySystem.Parameter.connOle);
            dt = new DataTable("temp");
            da.Fill(dt);

            //ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();
            //ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();

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

        void setUseState()
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

        void readOuterData(int pid)
        {
            daOuter = new OleDbDataAdapter("select * from 批生产记录表 where 生产指令ID=" + pid, mySystem.Parameter.connOle);
            dtOuter = new DataTable("批生产记录表");
            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        void outerBind()
        {

            bsOuter.DataSource = dtOuter;
            // clear
            tb生产指令编号.DataBindings.Clear();
            tb汇总人.DataBindings.Clear();
            tb批准人.DataBindings.Clear();
            tb审核人.DataBindings.Clear();
            dtp汇总时间.DataBindings.Clear();
            dtp结束生产时间.DataBindings.Clear();
            dtp开始生产时间.DataBindings.Clear();
            dtp批准时间.DataBindings.Clear();
            dtp审核时间.DataBindings.Clear();

            // bind
            tb生产指令编号.DataBindings.Add("Text", bsOuter.DataSource, "生产指令编号");
            tb汇总人.DataBindings.Add("Text", bsOuter.DataSource, "汇总人");
            tb批准人.DataBindings.Add("Text", bsOuter.DataSource, "批准人");
            tb审核人.DataBindings.Add("Text", bsOuter.DataSource, "审核人");
            dtp汇总时间.DataBindings.Add("Value", bsOuter.DataSource, "汇总时间");
            dtp结束生产时间.DataBindings.Add("Value", bsOuter.DataSource, "结束生产时间");
            dtp开始生产时间.DataBindings.Add("Value", bsOuter.DataSource, "开始生产时间");
            dtp批准时间.DataBindings.Add("Value", bsOuter.DataSource, "批准时间");
            dtp审核时间.DataBindings.Add("Value", bsOuter.DataSource, "审核时间");
        }

        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = _生产指令ID;
            dr["生产指令编号"] = _生产指令;
            dr["开始生产时间"] = DateTime.Now;
            dr["结束生产时间"] = DateTime.Now;
            dr["汇总人"] = mySystem.Parameter.userName;
            dr["汇总时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["批准时间"] = DateTime.Now;
            return dr;
        }

        void setFormState()
        {
           
            string s = dtOuter.Rows[0]["审核人"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }

        void setEnableReadOnly()
        {

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
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
            }


        }

        /// <summary>
        /// 默认控件可用状态
        /// </summary>
        void setControlTrue()
        {
            //// 查询插入，审核，提交审核，生产指令编码 在这里不用管
            //foreach (Control c in this.Controls)
            //{
            //    if (c is TextBox)
            //    {
            //        (c as TextBox).ReadOnly = false;
            //    }
            //    else if (c is DataGridView)
            //    {
            //        (c as DataGridView).ReadOnly = false;
            //    }
            //    else
            //    {
            //        c.Enabled = true;
            //    }
            //}
            //// 保证这两个按钮一直是false
            //btn审核.Enabled = false;
            //btn提交审核.Enabled = false;
            tb生产指令编号.Enabled = true;
            tb生产指令编号.ReadOnly = true;
            tb汇总人.Enabled = true;
            tb批准人.Enabled = true;
            tb审核人.Enabled = true;

            dtp批准时间.Enabled = true;
            dtp审核时间.Enabled = true;
            dtp结束生产时间.Enabled = true;
            dtp开始生产时间.Enabled = true;
            dtp汇总时间.Enabled = true;

            btn保存.Enabled = true;
            btn打印.Enabled = true;
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            comboBox打印机选择.Enabled = true;
        }

        /// <summary>
        /// 默认控件不可用状态
        /// </summary>
        void setControlFalse()
        {
            //// 查询插入，审核，提交审核，生产指令编码 在这里不用管
            //foreach (Control c in this.Controls)
            //{
            //    if (c.Name == "btn查询插入") continue;
            //    if (c.Name == "tb生产指令编码") continue;
            //    if (c is TextBox)
            //    {
            //        (c as TextBox).ReadOnly = true;
            //    }
            //    else if (c is DataGridView)
            //    {
            //        (c as DataGridView).ReadOnly = true;
            //    }
            //    else
            //    {
            //        c.Enabled = false;
            //    }
            //}
            //btn查看日志.Enabled = true;
            //btn打印.Enabled = true;

            tb生产指令编号.Enabled = true;
            tb生产指令编号.ReadOnly = true;
            tb汇总人.Enabled = false;
            tb批准人.Enabled = false;
            tb审核人.Enabled = false;

            dtp批准时间.Enabled = false;
            dtp审核时间.Enabled = false;
            dtp结束生产时间.Enabled = false;
            dtp开始生产时间.Enabled = false;
            dtp汇总时间.Enabled = false;

            btn保存.Enabled = false;
            btn打印.Enabled = true;
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            comboBox打印机选择.Enabled = true;

        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            save();
            if (_userState == Parameter.UserState.操作员)
            {
                btn提交审核.Enabled = true;
            }
        }

        void save()
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_生产指令ID);
            outerBind();
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            

            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;

            da = new OleDbDataAdapter("select * from 待审核 where 表名='批生产记录表' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "批生产记录表";
            dr["对应ID"] = dtOuter.Rows[0]["ID"];
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核人"] = "__待审核";
            save();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            if (dtOuter.Rows[0]["汇总人"].ToString() == mySystem.Parameter.userName)
            {
                MessageBox.Show("操作员和审核员不能是同一个人！");
                return;
            }
            ckform = new CheckForm(this);
            ckform.Show();
        }

        public override void CheckResult()
        {
            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;

            da = new OleDbDataAdapter("select * from 待审核 where 表名='批生产记录表' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核人"] = ckform.userName;
            dtOuter.Rows[0]["审核是否通过"] = ckform.ischeckOk;
            dtOuter.Rows[0]["审核意见"] = ckform.opinion;


            save();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;
            base.CheckResult();
        }

        private void fillPrinter()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                comboBox打印机选择.Items.Add(sPrint);
            }
            comboBox打印机选择.SelectedItem = print.PrinterSettings.PrinterName;
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);


        private List<Int32> getIntList(String str)
        {
            try
            {
                List<Int32> ret = new List<int>();
                String[] strs = str.Split('-');
                if (1 == strs.Length)
                {
                    ret.Add(Convert.ToInt32(strs[0]));
                }
                else if (2 == strs.Length)
                {
                    int a = Convert.ToInt32(strs[0]);
                    int b = Convert.ToInt32(strs[1]);
                    if (a > b)
                    {
                        throw new Exception("后面数字比前面的大");
                    }
                    for (int i = a; i <= b; ++i)
                    {
                        ret.Add(i);
                    }
                }
                else
                {
                    throw new Exception("打印页码解析失败！");
                }
                return ret;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new List<int>();
            }

        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            List<Int32> checkedRows = new List<int>();
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value))
                {
                    checkedRows.Add(i);
                }
            }

            foreach (Int32 r in checkedRows)
            {
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令 where ID=" + _生产指令ID, mySystem.Parameter.connOle);
                DataTable dt = new DataTable("生产指令");
                int id;
                List<Int32> pages = getIntList(dataGridView1.Rows[r].Cells[1].Value.ToString());
                switch (r)
                {
                    case 0: // 生产指令
                        da = new OleDbDataAdapter("select * from 生产指令 where  ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("生产指令");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.CS.CS制袋生产指令(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 1: // 开机确认
                        da = new OleDbDataAdapter("select * from 制袋机组开机前确认表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("制袋机组开机前确认表");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.CSBag_CheckBeforePower(mainform, id)).print(false);
                            GC.Collect();
                        }

                        break;
                    case 2: // CS制袋领料记录
                        da = new OleDbDataAdapter("select * from CS制袋领料记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("CS制袋领料记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.MaterialRecord(mainform, id)).print(false);
                            GC.Collect();
                        }

                        break;
                    case 3://制袋机组运行记录
                        da = new OleDbDataAdapter("select * from 制袋机组运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("制袋机组运行记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.RunningRecord(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 4:// 内包                                    
                        da = new OleDbDataAdapter("select * from 产品内包装记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品内包装记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            // TODO
                            (new mySystem.Process.Bag.CSBag_InnerPackaging(mainform, id)).print(false);
                            GC.Collect();

                        }
                        break;
                    case 5://外包
                        da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品外包装记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.CS.CS产品外包装记录(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 6://清场
                        da = new OleDbDataAdapter("select * from 清场记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("清场记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.CS.清场记录(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 7:// 日报

                        //da = new OleDbDataAdapter("select * from 吹膜机组运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        //dt = new DataTable("吹膜机组运行记录");
                        //da.Fill(dt);
                        //foreach (int page in pages)
                        //{
                        //    id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                        //    (new mySystem.Process.Extruction.B.Running(mainform, id)).print(false);
                        //    GC.Collect();
                        //}
                        break;
                    case 8:// 交接
                        da = new OleDbDataAdapter("select * from 岗位交接班记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("岗位交接班记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.CS.HandOver(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 9:// 内标签

                        //da = new OleDbDataAdapter("select * from 吹膜工序废品记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        //dt = new DataTable("吹膜工序废品记录");
                        //da.Fill(dt);
                        //foreach (int page in pages)
                        //{
                        //    id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                        //    (new mySystem.Process.Extruction.B.Waste(mainform, id)).print(false);
                        //    GC.Collect();
                        //}
                        break;
                    case 10:
                        // 外标签
                        //da = new OleDbDataAdapter("select * from 吹膜工序清场记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        //dt = new DataTable("吹膜工序清场记录");
                        //da.Fill(dt);
                        //foreach (int page in pages)
                        //{
                        //    id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                        //    (new mySystem.Extruction.Process.Record_extrusSiteClean(mainform, id)).print(false);
                        //    GC.Collect();
                        //}
                        break;
                    case 11:// 尺寸检验
                        da = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品外观和尺寸检验记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.CS.产品外观和尺寸检验记录(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 12:// 热合产品热合强度检验记录
                        da = new OleDbDataAdapter("select * from 产品热合强度检验记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品热合强度检验记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.CS.产品热合强度检验记录(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    //case 13: // 岗位交接班
                    //    da = new OleDbDataAdapter("select * from 吹膜岗位交接班记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                    //    dt = new DataTable("吹膜岗位交接班记录");
                    //    da.Fill(dt);
                    //    foreach (int page in pages)
                    //    {
                    //        id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                    //        (new mySystem.Process.Extruction.A.HandOver(mainform, id)).print(false);
                    //        GC.Collect();
                    //    }
                    //    break;
                    //case 14:// 内包装
                    //    da = new OleDbDataAdapter("select * from 产品内包装记录表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                    //    dt = new DataTable("产品内包装记录表");
                    //    da.Fill(dt);
                    //    foreach (int page in pages)
                    //    {
                    //        id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                    //        (new mySystem.Extruction.Process.ProductInnerPackagingRecord(mainform, id)).print(false);
                    //        GC.Collect();
                    //    }
                    //    break;
                    //case 15:// 外包装
                    //    da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                    //    dt = new DataTable("产品外包装记录表");
                    //    da.Fill(dt);
                    //    foreach (int page in pages)
                    //    {
                    //        id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                    //        (new mySystem.Extruction.Chart.outerpack(mainform, id)).print(false);
                    //        GC.Collect();
                    //    }
                    //    break;
                }
            }
        }
    }
}
