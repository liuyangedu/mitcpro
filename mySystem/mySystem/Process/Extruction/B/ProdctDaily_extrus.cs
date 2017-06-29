﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.OleDb;

namespace mySystem
{
    /// <summary>
    ///  吹膜生产日报表
    /// </summary>
    public partial class ProdctDaily_extrus : mySystem.BaseForm
    {
        public ProdctDaily_extrus(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            Init();
            add_instrucode();
            //query_by_instru("E-2017-005");
            //connToServer();
            //queryAndShow();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //自定义变量
        private bool isOk;//是否连接数据库成功
        string prodCode;//生产指令
        string strCon;
        string sql;

        SqlConnection conn;
        DataTable dt;
        DataRow[] dr;

        DataTable dt_by_instru;
        

        //针对查询
        DateTime date1;//起始时间
        DateTime date2;//结束时间
        string ddan;//订单号
        string instr;//生产指令
        string writer;//填报人
        string checker;//审核人
        bool flag;//标志查询的条件是否可见


        //自定义函数
        private void Init()
        {
            strCon = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            sql = "select * from test_2";
            prodCode = "0x34222fds";
            isOk = false;
            lastRow= new List<object[]>();

            dataGridView1.Font = new Font("宋体",12);

            comboBox1.Items.Add("(空)");
            comboBox2.Items.Add("(空)");
            comboBox3.Items.Add("(空)");
            comboBox4.Items.Add("(空)");

            comboBox1.Text = "(空)".Trim();
            comboBox2.Text = "(空)".Trim();
            comboBox3.Text = "(空)".Trim();
            comboBox4.Text = "(空)".Trim();
            date1 = DateTime.Parse("2016/10/20");

            label2.Visible = false;
            dateTimePicker1.Visible = false;
            label3.Visible = false;
            dateTimePicker2.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            comboBox1.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;

            dt = new DataTable();
            //dt.Columns.Add("id",System.Type.GetType("System.Int32"));
            dt.Columns.Add("date", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("classes", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("prodcode", System.Type.GetType("System.String"));
            dt.Columns.Add("prodbatch", System.Type.GetType("System.String"));
            dt.Columns.Add("number", System.Type.GetType("System.String"));
            dt.Columns.Add("count", System.Type.GetType("System.String"));
            dt.Columns.Add("weight", System.Type.GetType("System.String"));
            dt.Columns.Add("mA", System.Type.GetType("System.String"));
            dt.Columns.Add("mB1C", System.Type.GetType("System.String"));
            dt.Columns.Add("mB2", System.Type.GetType("System.String"));
            dt.Columns.Add("time", System.Type.GetType("System.String"));
            dt.Columns.Add("rec", System.Type.GetType("System.String"));
            dt.Columns.Add("rev", System.Type.GetType("System.String"));

            flag = false;//查询条件不可见
        }
        private void add_instrucode()
        {
            if (mainform.isSqlOk)
            {

            }
            else
            {
                string acsql = "select production_instruction_code from production_instruction";
                OleDbCommand comm = new OleDbCommand(acsql, mainform.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);
                DataTable dtemp = new DataTable();
                da.Fill(dtemp);

                for (int i = 0; i < dtemp.Rows.Count; i++)
                {
                    comboBox2.Items.Add(dtemp.Rows[i][0].ToString());
                }
                dtemp.Dispose();
                comm.Dispose();
                da.Dispose();
            }
        }

        //通过批号id查找批号代码
        private string batchcode_findby_batchid(int btid)
        {
            string code = "";

            string acsql = "select product_batch_code from product_batch where product_batch_id=" + btid;
            OleDbCommand comm = new OleDbCommand(acsql, mainform.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);

            if (dt1.Rows.Count == 0)
                return "***";
            code = dt1.Rows[0][0].ToString();
            da.Dispose();
            dt1.Dispose();
            return code;
        }

        //通过批号id查找产品代码
        private string prodcode_findby_batchid(int btid)
        {
            string code = "";

            string acsql = "select product_code from product_aoxing where product_id in (select product_id from product_batch where product_batch_id="+btid+")";
            OleDbCommand comm = new OleDbCommand(acsql, mainform.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);

            if (dt1.Rows.Count == 0)
                return "***";
            code = dt1.Rows[0][0].ToString();
            da.Dispose();
            dt1.Dispose();
            return code;
        }

        //通过用户id查找用户名字
        private string name_findby_userid(int uid)
        {
            string name = "";
            string acsql = "select user_name from user_aoxing where user_id=" + uid;
            OleDbCommand comm = new OleDbCommand(acsql, mainform.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);

            if (dt1.Rows.Count == 0)
                return "***";
            name = dt1.Rows[0][0].ToString();
            da.Dispose();
            dt1.Dispose();
            return name;
        }

        //查找同一条生产指令下的数据 select [A].*,[C].* from [A] left join ( select * from [B] ) as [C] on [A].id = [C].id
        private void query_by_instru(string instru_code)
        {
            if (mainform.isSqlOk)
            {
    
            }
            else
            {
                //通过生产指令代码找到对应的生产指令id
                string acsql = "select production_instruction_id from production_instruction where production_instruction_code='"+instru_code+"'";
                OleDbCommand comm = new OleDbCommand(acsql, mainform.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);
                DataTable dt1 = new DataTable();
                da.Fill(dt1);
                
                int id = int.Parse(dt1.Rows[0][0].ToString());//获得生产指令id
                da.Dispose();

                //通过生产指令id查找相应的子集
                //acsql = "select s6.production_instruction_id,s6.s6_production_date,s6.s6_flight,s6.product_batch_id,s6.s6_mojuan_length,s6.s6_mojuan_weight,s5.production_instruction_id,s5.s5_feeding_info,s6.s6_time,s6.s6_recorder_id,s6.s6_reviewer_id from extrusion_s6_production_check s6,extrusion_s5_feeding s5 where s6.production_instruction_id=s5.production_instruction_id";
                //acsql="select s6.s6_production_date,s6.s6_flight,s6.product_batch_id,s6.s6_mojuan_length,s6.s6_mojuan_weigh,s6.s6_time,s6.s6_recorder_id,s6.s6_reviewer_id from extrusion_s6_production_check where production_instruction_id="
                acsql = "select product_batch_id,s6_production_date,s6_flight,s6_mojuan_number,s6_mojuan_length,s6_mojuan_weight,s6_time,s6_recorder_id,s6_reviewer_id from extrusion_s6_production_check where production_instruction_id="+id;
                comm.CommandText = acsql;
                da = new OleDbDataAdapter(comm);
                DataTable dt2 = new DataTable();
                da.Fill(dt2);
                da.Dispose();

                acsql = "select product_batch_id,s5_feeding_info from extrusion_s5_feeding where production_instruction_id="+id;
                comm.CommandText = acsql;
                da = new OleDbDataAdapter(comm);
                DataTable dt3 = new DataTable();
                da.Fill(dt3);

                //子集通过连接生成最后的联合表
                var query = from r in dt2.AsEnumerable()
                            join s in dt3.AsEnumerable()
                            on r.Field<int>("product_batch_id") equals s.Field<int>("product_batch_id") into temp
                            from t in temp.DefaultIfEmpty()
                            select new
                            {
                                batch_id = r.Field<int>("product_batch_id").ToString(),
                                date=r.Field<DateTime>("s6_production_date").ToShortDateString(),
                                flight = r.Field<bool>("s6_flight").ToString(),
                                number=r.Field<string>("s6_mojuan_number"),
                                length = r.Field<int>("s6_mojuan_length").ToString(),
                                weight = r.Field<int>("s6_mojuan_weight").ToString(),
                                time = r.Field<DateTime>("s6_time").ToString(),
                                recid = r.Field<int>("s6_recorder_id").ToString(),
                                revid = r.Field<int>("s6_reviewer_id").ToString(),
                                feedinfo=t!=null ? t.Field<string>("s5_feeding_info") : "***"
                            };
                var query_r = from r in dt3.AsEnumerable()
                            join s in dt2.AsEnumerable()
                            on r.Field<int>("product_batch_id") equals s.Field<int>("product_batch_id") into temp
                            from t in temp.DefaultIfEmpty()
                            select new
                            {
                                batch_id = r.Field<int>("product_batch_id").ToString(),
                                date=t!=null?t.Field<DateTime>("s6_production_date").ToShortDateString():"***",
                                flight = t!=null? t.Field<bool>("s6_flight").ToString():"***",
                                number=t!=null? t.Field<string>("s6_mojuan_number"):"***",
                                length = t!=null? t.Field<int>("s6_mojuan_length").ToString():"***",
                                weight = t!=null? t.Field<int>("s6_mojuan_weight").ToString():"***",
                                time = t!=null? t.Field<DateTime>("s6_time").ToString():"***",
                                recid = t!=null? t.Field<int>("s6_recorder_id").ToString():"***",
                                revid = t!=null? t.Field<int>("s6_reviewer_id").ToString():"***",
                                feedinfo = r.Field<string>("s5_feeding_info")
                            };
                var fullquery = query.Union(query_r);//最后查找的结果
                
                //将结果填入表格中和dt中
                dt.Clear();
                int index = 0;
                foreach (var item in fullquery)
                {
                    Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", item.batch_id, item.date, item.flight, item.number,item.length, item.weight, item.time, item.recid, item.revid, item.feedinfo);
                    DataGridViewRow dr = new DataGridViewRow();
                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                    }
                    
                    dr.Cells[0].Value = index+1;
                    dr.Cells[1].Value = item.date;
                    dr.Cells[2].Value = item.flight=="True";
                    dr.Cells[3].Value = prodcode_findby_batchid(int.Parse(item.batch_id));//产品代码
                    dr.Cells[4].Value = batchcode_findby_batchid(int.Parse(item.batch_id));//批号代码
                    dr.Cells[5].Value = item.number;//卷号
                    dr.Cells[6].Value = item.length;
                    dr.Cells[7].Value = item.weight;
                    dr.Cells[8].Value = "";//废品重量
                    dr.Cells[9].Value = item.feedinfo;//加料A
                    dr.Cells[10].Value = item.feedinfo;//加料B1+C
                    dr.Cells[11].Value = item.feedinfo;//加料B2
                    dr.Cells[12].Value = item.time;//工时
                    if (item.recid == "***")
                        dr.Cells[13].Value = item.recid;
                    else
                        dr.Cells[13].Value = name_findby_userid(int.Parse(item.recid));
                    if (item.revid == "***")
                        dr.Cells[14].Value = item.revid;
                    else
                        dr.Cells[14].Value = name_findby_userid(int.Parse(item.revid));

                    dataGridView1.Rows.Add(dr);

                    DataRow drow = dt.NewRow();
                    if (item.date == "***")
                        drow["date"] = DateTime.Parse("1900-1-1 0:00:00");
                    else
                        drow["date"] = DateTime.Parse(item.date);
                    drow["classes"] = item.flight == "True";
                    drow["prodcode"] = dr.Cells[3].Value;
                    drow["prodbatch"] = dr.Cells[4].Value;
                    drow["number"] = item.number;
                    drow["count"] = item.length;
                    drow["weight"] = item.weight;
                    drow["mA"] = item.feedinfo;
                    drow["mB1C"] = item.feedinfo;
                    drow["mB2"] = item.feedinfo;
                    drow["time"] = item.time;
                    drow["rec"] = dr.Cells[13].Value;
                    drow["rev"] = dr.Cells[14].Value;
                    dt.Rows.Add(drow);

                    index = index + 1;
                }
                    
                
                //查询条件可见
                setvisible(true);
                flag = true;

                //往查询条件中添加目录
                comboBox1.Items.Clear();
                comboBox3.Items.Clear();
                comboBox4.Items.Clear();
                comboBox1.Items.Add("(空)");
                comboBox3.Items.Add("(空)");
                comboBox4.Items.Add("(空)");

                DistinctValueCount(dt, "prodcode", comboBox1);
                DistinctValueCount(dt, "rec", comboBox3);
                DistinctValueCount(dt, "rev", comboBox4);
            }
        }

        //统计datatable中某一列不同值，并将其填入控件中
        private int DistinctValueCount(DataTable dt, string colname,ComboBox combox)
        {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object tmp = dt.Rows[i][colname];
                if (!ht.Contains(tmp))
                {
                    ht.Add(tmp, tmp);
                    combox.Items.Add(tmp.ToString());
                }
            }

            return ht.Count;
        }

        //通过查询条件来查
        private void query_by_condt()
        {
            DateTime date1 = dateTimePicker1.Value.Date;
            DateTime date2 = dateTimePicker2.Value.Date;
            TimeSpan delt = date2 - date1;
            if (delt.TotalDays < 0)
            {
                MessageBox.Show("起止时间有误，请重新输入");
                return;
            }

            string prodcode = comboBox1.Text;//产品代码
            string writer = comboBox3.Text;//填报人
            string checker = comboBox4.Text;//复核人

            ////删除合计行
            //dt.Rows.RemoveAt(dt.Rows.Count - 1);

            string sql = "date>=" + "'" + date1 + "'" + " and " + "date<=" + "'" + date2 + "'";

            if (prodcode != "(空)")
                sql += " and " + "prodcode like" + "'%" + prodcode + "%'";
            if (writer!= "(空)")
                sql += " and " + "rec like" + "'%" + writer + "%'";
            if (checker != "(空)")
                sql += " and " + "rev like" + "'%" + checker + "%'";
            DataRow[] arrayDR=dt.Select(sql);

            //清空表格
            while (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            //填充
            int i=0;
            foreach(DataRow drow in arrayDR)
            {
                 DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = i + 1;
                dr.Cells[1].Value = drow[0];
                dr.Cells[2].Value = drow[1];
                dr.Cells[3].Value = drow[2];
                dr.Cells[4].Value = drow[3];
                dr.Cells[5].Value = drow[4];
                dr.Cells[6].Value = drow[5];
                dr.Cells[7].Value = drow[6];
                dr.Cells[8].Value = "";//废品重量
                dr.Cells[9].Value = drow[7]; //加料A
                dr.Cells[10].Value = drow[8]; //加料B1+C
                dr.Cells[11].Value = drow[9]; //加料B2
                dr.Cells[12].Value = drow[10]; //工时
                dr.Cells[13].Value = drow[11];
                dr.Cells[14].Value = drow[12];
                dataGridView1.Rows.Add(dr);
            }

        }

        /*仅用来来测试，实际早已在上一步登陆*/
        private void connToServer()
        {             
            conn = new SqlConnection(strCon);
            conn.Open();
            isOk = true;
        }
        private void queryAndShow()
        {
            //if (!isOk)
            //{
            //    MessageBox.Show("连接数据库失败", "error");
            //    return;
            //}

            //显示生产指令
            //label3.Text = prodCode;

            //查询
            if (mainform.isSqlOk)
            {
                
                SqlCommand comm = new SqlCommand(sql, mainform.conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);

                //DataSet ds = new DataSet();
                dt = new DataTable();
                //da.Fill(dt);

                ///添加一列
                DataColumn col = new DataColumn("编号");
                dt.Columns.Add(col);
                da.Fill(dt);
            }
            else
            {
                string sql = "select s6_production_date,s6_flight,production_instruction_id,product_batch_id,s6_mojuan_length,s6_mojuan_weight from extrusion_s6_production_check";
                OleDbCommand comm = new OleDbCommand(sql, mainform.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                //DataSet ds = new DataSet();
                dt = new DataTable();
                //da.Fill(dt);

                ///添加一列
                DataColumn col = new DataColumn("编号");
                dt.Columns.Add(col);
                da.Fill(dt);
            }

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                dt.Rows[row][0] = (row+1).ToString();
            }


            //统计列值
            
            
            //DataView dv = dt.DefaultView;
            //DataTable tempdt=dv.ToTable(true, "订单");

            //for (int i = 0; i < tempdt.Rows.Count; i++)
            //{
            //    comboBox1.Items.Add(tempdt.Rows[i]["订单"].ToString().Trim());
            //}
            //tempdt = dv.ToTable(true, "生产指令");
            //for (int i = 0; i < tempdt.Rows.Count; i++)
            //{
            //    comboBox2.Items.Add(tempdt.Rows[i]["生产指令"].ToString().Trim());
            //}
            //tempdt = dv.ToTable(true, "填报人");
            //for (int i = 0; i < tempdt.Rows.Count; i++)
            //{
            //    comboBox3.Items.Add(tempdt.Rows[i]["填报人"].ToString().Trim());
            //}
            //tempdt = dv.ToTable(true, "复核人");
            //for (int i = 0; i < tempdt.Rows.Count; i++)
            //{
            //    comboBox4.Items.Add(tempdt.Rows[i]["复核人"].ToString().Trim());
            //}

            ////添加合计
            //DataRow row1;
            //row1=dt.NewRow();
            //row1[0] = "合计";
            //row1[7] = dt.Compute("sum("+dt.Columns[7].ColumnName+")", "TRUE");
            //row1[8] = dt.Compute("sum(" + dt.Columns[8].ColumnName + ")", "TRUE");
            //row1[9] = dt.Compute("sum(" + dt.Columns[9].ColumnName + ")", "TRUE");
            //dt.Rows.Add(row1);

            dataGridView1.DataSource = dt;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //System.Console.WriteLine("**************************************************************************8");
            //if (e.RowIndex >= 0 || dataGridView1.Rows.Count == 0)
            //    return;

            //if (lastRow.Count == 0)
            //{
            //    colindex = e.ColumnIndex;
            //    int index = dataGridView1.Rows.Count - 1;
            //    System.Console.WriteLine(colindex.ToString());
            //    System.Console.WriteLine(index.ToString());
            //    lastRow.Add(((DataTable)dataGridView1.DataSource).Rows[index].ItemArray);
            //    System.Console.WriteLine(lastRow[0].ToString());
            //    dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.Rows.Count - 1]);

            //    DataTable dt = ((DataTable)dataGridView1.DataSource);
            //    DataView dv = dt.DefaultView;
            //    dv.Sort = dt.Columns[colindex].ColumnName;
            //    dt = dv.ToTable();
            //    dt.Rows.Add(lastRow[0]);
            //    lastRow.Clear();
            //    dataGridView1.DataSource = dt;
            //}
        }
        List<object[]> lastRow;
        int colindex;

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string s=comboBox2.SelectedValue.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //date1 = dateTimePicker1.Value.Date;
            ////System.Console.WriteLine(date1);
            //date2 = dateTimePicker2.Value.Date;
            //System.Console.WriteLine(date2);
            //TimeSpan delt = date2 - date1;
            //if (delt.TotalDays < 0)
            //{
            //    MessageBox.Show("起止时间有误，请重新输入");
            //    return;
            //}

            //ddan = comboBox1.Text.Trim();
            //instr = comboBox2.Text.Trim();
            //writer = comboBox3.Text.Trim();
            //checker = comboBox4.Text.Trim();

            ////删除合计行
            //dt.Rows.RemoveAt(dt.Rows.Count - 1);

            //string sql = "生产时间>=" + "'" + date1 + "'" + " and " + "生产时间<=" + "'" + date2 + "'";

            //if (ddan != "(空)")
            //    sql += " and " + "订单 like" + "'%" + ddan + "%'";
            //if (instr != "(空)")
            //    sql += " and " + "生产指令 like" + "'%" + instr + "%'";

            //if (writer != "(空)")
            //    sql += " and " + "填报人 like" + "'%" + writer + "%'";
            //if (checker != "(空)")
            //    sql += " and " + "复核人 like" + "'%" + checker + "%'";


            //dr = dt.Select(sql);
            ////添加合计行
            //DataRow rowtemp;
            //rowtemp = dt.NewRow();
            //rowtemp[0] = "合计";
            //rowtemp[7] = dt.Compute("sum(" + dt.Columns[7].ColumnName + ")", "TRUE");
            //rowtemp[8] = dt.Compute("sum(" + dt.Columns[8].ColumnName + ")", "TRUE");
            //rowtemp[9] = dt.Compute("sum(" + dt.Columns[9].ColumnName + ")", "TRUE");
            //dt.Rows.Add(rowtemp);

            //if (dr.Length == 0)
            //{               
            //    dataGridView1.DataSource = null;
            //    return;
            //}
                
            //DataTable temp = dr.CopyToDataTable();
            ////改变序号
            //for (int row = 0; row < temp.Rows.Count; row++)
            //{
            //    temp.Rows[row][0] = (row + 1).ToString();
            //}
            //DataRow row1;
            //row1 = temp.NewRow();
            //row1[0] = "合计";
            //row1[7] = temp.Compute("sum(" + dt.Columns[7].ColumnName + ")", "TRUE");
            //row1[8] = temp.Compute("sum(" + dt.Columns[8].ColumnName + ")", "TRUE");
            //row1[9] = temp.Compute("sum(" + dt.Columns[9].ColumnName + ")", "TRUE");
            //temp.Rows.Add(row1);

            //dataGridView1.DataSource = temp;

            //清空表格
            while (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            if (mainform.isSqlOk)
            {

            }
            else
            {
                instr = comboBox2.Text;
                if (instr == "(空)")
                {
                    MessageBox.Show("选择一条生产指令");
                    return;
                }
                //判断通过指令查找还是进行模糊查找
                if (!flag)
                    query_by_instru(instr);
                else
                    query_by_condt();

            }

        }

        private bool setvisible(bool v)
        {
            label2.Visible = v;
            dateTimePicker1.Visible = v;
            label3.Visible = v;
            dateTimePicker2.Visible = v;
            label5.Visible = v;
            label6.Visible = v;
            label7.Visible = v;
            comboBox1.Visible = v;
            comboBox3.Visible = v;
            comboBox4.Visible = v;
            return v;
        }
        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            setvisible(false);
            flag = false;
        }

    }
}
