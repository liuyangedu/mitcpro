﻿namespace mySystem.Process.CleanCut
{
    partial class Instru
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.tb备注 = new System.Windows.Forms.TextBox();
            this.bt查询插入 = new System.Windows.Forms.Button();
            this.tb指令编号 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb设备编号 = new System.Windows.Forms.TextBox();
            this.dtp计划生产日期 = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp编制日期 = new System.Windows.Forms.DateTimePicker();
            this.dtp审批日期 = new System.Windows.Forms.DateTimePicker();
            this.dtp接收日期 = new System.Windows.Forms.DateTimePicker();
            this.tb编制人 = new System.Windows.Forms.TextBox();
            this.tb审批人 = new System.Windows.Forms.TextBox();
            this.tb接收人 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.bt确认 = new System.Windows.Forms.Button();
            this.bt审核 = new System.Windows.Forms.Button();
            this.bt打印 = new System.Windows.Forms.Button();
            this.bt添加 = new System.Windows.Forms.Button();
            this.bt删除 = new System.Windows.Forms.Button();
            this.bt上移 = new System.Windows.Forms.Button();
            this.bt下移 = new System.Windows.Forms.Button();
            this.bt发送审核 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(294, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "清洁/分切 工序生产指令";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 107);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(838, 162);
            this.dataGridView1.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(712, 32);
            this.label4.TabIndex = 5;
            this.label4.Text = "标签：中文标签\r\n产品包装：单层洁净包装，标识产品编码、批号、数量、生产日期，完成后放置于车间内指定位置。";
            // 
            // tb备注
            // 
            this.tb备注.Location = new System.Drawing.Point(12, 407);
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(838, 26);
            this.tb备注.TabIndex = 6;
            // 
            // bt查询插入
            // 
            this.bt查询插入.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt查询插入.Location = new System.Drawing.Point(272, 66);
            this.bt查询插入.Name = "bt查询插入";
            this.bt查询插入.Size = new System.Drawing.Size(84, 23);
            this.bt查询插入.TabIndex = 26;
            this.bt查询插入.Text = "查询/插入";
            this.bt查询插入.UseVisualStyleBackColor = true;
            this.bt查询插入.Click += new System.EventHandler(this.bt查询插入_Click);
            // 
            // tb指令编号
            // 
            this.tb指令编号.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb指令编号.Location = new System.Drawing.Point(137, 64);
            this.tb指令编号.Name = "tb指令编号";
            this.tb指令编号.Size = new System.Drawing.Size(96, 23);
            this.tb指令编号.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(28, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 14);
            this.label6.TabIndex = 24;
            this.label6.Text = "生产指令编号";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(410, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 28;
            this.label7.Text = "生产设备";
            // 
            // tb设备编号
            // 
            this.tb设备编号.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb设备编号.Location = new System.Drawing.Point(479, 64);
            this.tb设备编号.Name = "tb设备编号";
            this.tb设备编号.Size = new System.Drawing.Size(110, 23);
            this.tb设备编号.TabIndex = 27;
            // 
            // dtp计划生产日期
            // 
            this.dtp计划生产日期.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp计划生产日期.Location = new System.Drawing.Point(703, 66);
            this.dtp计划生产日期.Name = "dtp计划生产日期";
            this.dtp计划生产日期.Size = new System.Drawing.Size(136, 23);
            this.dtp计划生产日期.TabIndex = 30;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(606, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 14);
            this.label8.TabIndex = 29;
            this.label8.Text = "计划生产日期";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 379);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 31;
            this.label2.Text = "备  注：";
            // 
            // dtp编制日期
            // 
            this.dtp编制日期.Location = new System.Drawing.Point(31, 492);
            this.dtp编制日期.Name = "dtp编制日期";
            this.dtp编制日期.Size = new System.Drawing.Size(158, 26);
            this.dtp编制日期.TabIndex = 8;
            // 
            // dtp审批日期
            // 
            this.dtp审批日期.Location = new System.Drawing.Point(244, 489);
            this.dtp审批日期.Name = "dtp审批日期";
            this.dtp审批日期.Size = new System.Drawing.Size(157, 26);
            this.dtp审批日期.TabIndex = 9;
            // 
            // dtp接收日期
            // 
            this.dtp接收日期.Location = new System.Drawing.Point(477, 489);
            this.dtp接收日期.Name = "dtp接收日期";
            this.dtp接收日期.Size = new System.Drawing.Size(156, 26);
            this.dtp接收日期.TabIndex = 10;
            // 
            // tb编制人
            // 
            this.tb编制人.Location = new System.Drawing.Point(106, 460);
            this.tb编制人.Name = "tb编制人";
            this.tb编制人.Size = new System.Drawing.Size(83, 26);
            this.tb编制人.TabIndex = 11;
            // 
            // tb审批人
            // 
            this.tb审批人.Location = new System.Drawing.Point(319, 456);
            this.tb审批人.Name = "tb审批人";
            this.tb审批人.Size = new System.Drawing.Size(82, 26);
            this.tb审批人.TabIndex = 12;
            // 
            // tb接收人
            // 
            this.tb接收人.Location = new System.Drawing.Point(552, 456);
            this.tb接收人.Name = "tb接收人";
            this.tb接收人.Size = new System.Drawing.Size(80, 26);
            this.tb接收人.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 463);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 32;
            this.label3.Text = "编制人：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(241, 463);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 33;
            this.label5.Text = "审批人：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(474, 463);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 16);
            this.label9.TabIndex = 34;
            this.label9.Text = "接收人：";
            // 
            // bt确认
            // 
            this.bt确认.Location = new System.Drawing.Point(686, 456);
            this.bt确认.Name = "bt确认";
            this.bt确认.Size = new System.Drawing.Size(63, 23);
            this.bt确认.TabIndex = 35;
            this.bt确认.Text = "确认";
            this.bt确认.UseVisualStyleBackColor = true;
            this.bt确认.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt审核
            // 
            this.bt审核.Location = new System.Drawing.Point(776, 456);
            this.bt审核.Name = "bt审核";
            this.bt审核.Size = new System.Drawing.Size(63, 23);
            this.bt审核.TabIndex = 36;
            this.bt审核.Text = "审核";
            this.bt审核.UseVisualStyleBackColor = true;
            this.bt审核.Click += new System.EventHandler(this.bt审核_Click);
            // 
            // bt打印
            // 
            this.bt打印.Location = new System.Drawing.Point(776, 492);
            this.bt打印.Name = "bt打印";
            this.bt打印.Size = new System.Drawing.Size(63, 23);
            this.bt打印.TabIndex = 37;
            this.bt打印.Text = "打印";
            this.bt打印.UseVisualStyleBackColor = true;
            // 
            // bt添加
            // 
            this.bt添加.Location = new System.Drawing.Point(21, 285);
            this.bt添加.Name = "bt添加";
            this.bt添加.Size = new System.Drawing.Size(63, 23);
            this.bt添加.TabIndex = 38;
            this.bt添加.Text = "添加";
            this.bt添加.UseVisualStyleBackColor = true;
            this.bt添加.Click += new System.EventHandler(this.bt添加_Click);
            // 
            // bt删除
            // 
            this.bt删除.Location = new System.Drawing.Point(106, 285);
            this.bt删除.Name = "bt删除";
            this.bt删除.Size = new System.Drawing.Size(63, 23);
            this.bt删除.TabIndex = 39;
            this.bt删除.Text = "删除";
            this.bt删除.UseVisualStyleBackColor = true;
            this.bt删除.Click += new System.EventHandler(this.bt删除_Click);
            // 
            // bt上移
            // 
            this.bt上移.Location = new System.Drawing.Point(193, 285);
            this.bt上移.Name = "bt上移";
            this.bt上移.Size = new System.Drawing.Size(63, 23);
            this.bt上移.TabIndex = 40;
            this.bt上移.Text = "上移";
            this.bt上移.UseVisualStyleBackColor = true;
            this.bt上移.Click += new System.EventHandler(this.bt上移_Click);
            // 
            // bt下移
            // 
            this.bt下移.Location = new System.Drawing.Point(283, 285);
            this.bt下移.Name = "bt下移";
            this.bt下移.Size = new System.Drawing.Size(63, 23);
            this.bt下移.TabIndex = 41;
            this.bt下移.Text = "下移";
            this.bt下移.UseVisualStyleBackColor = true;
            this.bt下移.Click += new System.EventHandler(this.bt下移_Click);
            // 
            // bt发送审核
            // 
            this.bt发送审核.Location = new System.Drawing.Point(674, 493);
            this.bt发送审核.Name = "bt发送审核";
            this.bt发送审核.Size = new System.Drawing.Size(89, 23);
            this.bt发送审核.TabIndex = 42;
            this.bt发送审核.Text = "发送审核";
            this.bt发送审核.UseVisualStyleBackColor = true;
            this.bt发送审核.Click += new System.EventHandler(this.bt发送审核_Click);
            // 
            // Instru
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 533);
            this.Controls.Add(this.bt发送审核);
            this.Controls.Add(this.bt下移);
            this.Controls.Add(this.bt上移);
            this.Controls.Add(this.bt删除);
            this.Controls.Add(this.bt添加);
            this.Controls.Add(this.bt打印);
            this.Controls.Add(this.bt审核);
            this.Controls.Add(this.bt确认);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtp计划生产日期);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tb设备编号);
            this.Controls.Add(this.bt查询插入);
            this.Controls.Add(this.tb指令编号);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb接收人);
            this.Controls.Add(this.tb审批人);
            this.Controls.Add(this.tb编制人);
            this.Controls.Add(this.dtp接收日期);
            this.Controls.Add(this.dtp审批日期);
            this.Controls.Add(this.dtp编制日期);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Instru";
            this.Text = "Instru";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb备注;
        private System.Windows.Forms.Button bt查询插入;
        private System.Windows.Forms.TextBox tb指令编号;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb设备编号;
        private System.Windows.Forms.DateTimePicker dtp计划生产日期;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp编制日期;
        private System.Windows.Forms.DateTimePicker dtp审批日期;
        private System.Windows.Forms.DateTimePicker dtp接收日期;
        private System.Windows.Forms.TextBox tb编制人;
        private System.Windows.Forms.TextBox tb审批人;
        private System.Windows.Forms.TextBox tb接收人;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button bt确认;
        private System.Windows.Forms.Button bt审核;
        private System.Windows.Forms.Button bt打印;
        private System.Windows.Forms.Button bt添加;
        private System.Windows.Forms.Button bt删除;
        private System.Windows.Forms.Button bt上移;
        private System.Windows.Forms.Button bt下移;
        private System.Windows.Forms.Button bt发送审核;
    }
}