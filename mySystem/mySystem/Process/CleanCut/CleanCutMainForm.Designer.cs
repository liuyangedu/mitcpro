﻿namespace mySystem.Process.CleanCut
{
    partial class CleanCutMainForm
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.A1Btn = new System.Windows.Forms.Button();
            this.A2Btn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.A5Btn = new System.Windows.Forms.Button();
            this.A4Btn = new System.Windows.Forms.Button();
            this.A3Btn = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(97, 22);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(290, 27);
            this.comboBox1.TabIndex = 18;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 17;
            this.label1.Text = "生产指令：";
            // 
            // A1Btn
            // 
            this.A1Btn.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.A1Btn.Location = new System.Drawing.Point(15, 40);
            this.A1Btn.Name = "A1Btn";
            this.A1Btn.Size = new System.Drawing.Size(269, 38);
            this.A1Btn.TabIndex = 12;
            this.A1Btn.Text = "清洁分切生产指令";
            this.A1Btn.UseVisualStyleBackColor = true;
            this.A1Btn.Click += new System.EventHandler(this.A1Btn_Click);
            // 
            // A2Btn
            // 
            this.A2Btn.Font = new System.Drawing.Font("SimSun", 12F);
            this.A2Btn.Location = new System.Drawing.Point(15, 110);
            this.A2Btn.Name = "A2Btn";
            this.A2Btn.Size = new System.Drawing.Size(269, 38);
            this.A2Btn.TabIndex = 16;
            this.A2Btn.Text = "清场记录";
            this.A2Btn.UseVisualStyleBackColor = true;
            this.A2Btn.Click += new System.EventHandler(this.A2Btn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.A5Btn);
            this.groupBox2.Controls.Add(this.A1Btn);
            this.groupBox2.Controls.Add(this.A2Btn);
            this.groupBox2.Controls.Add(this.A4Btn);
            this.groupBox2.Controls.Add(this.A3Btn);
            this.groupBox2.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(97, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox2.Size = new System.Drawing.Size(302, 395);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            // 
            // A5Btn
            // 
            this.A5Btn.Font = new System.Drawing.Font("SimSun", 12F);
            this.A5Btn.Location = new System.Drawing.Point(15, 320);
            this.A5Btn.Name = "A5Btn";
            this.A5Btn.Size = new System.Drawing.Size(269, 38);
            this.A5Btn.TabIndex = 17;
            this.A5Btn.Text = "清洁分切日报表";
            this.A5Btn.UseVisualStyleBackColor = true;
            this.A5Btn.Click += new System.EventHandler(this.A5Btn_Click);
            // 
            // A4Btn
            // 
            this.A4Btn.Font = new System.Drawing.Font("SimSun", 12F);
            this.A4Btn.Location = new System.Drawing.Point(15, 250);
            this.A4Btn.Name = "A4Btn";
            this.A4Btn.Size = new System.Drawing.Size(269, 38);
            this.A4Btn.TabIndex = 15;
            this.A4Btn.Text = "清洁分切生产记录表";
            this.A4Btn.UseVisualStyleBackColor = true;
            this.A4Btn.Click += new System.EventHandler(this.A4Btn_Click);
            // 
            // A3Btn
            // 
            this.A3Btn.Font = new System.Drawing.Font("SimSun", 12F);
            this.A3Btn.Location = new System.Drawing.Point(15, 180);
            this.A3Btn.Name = "A3Btn";
            this.A3Btn.Size = new System.Drawing.Size(269, 38);
            this.A3Btn.TabIndex = 11;
            this.A3Btn.Text = "清洁分切机开机确认及运行记录";
            this.A3Btn.UseVisualStyleBackColor = true;
            this.A3Btn.Click += new System.EventHandler(this.A3Btn_Click);
            // 
            // CleanCutMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 615);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("SimSun", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CleanCutMainForm";
            this.Text = "CleanCutMainForm";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button A1Btn;
        private System.Windows.Forms.Button A2Btn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button A4Btn;
        private System.Windows.Forms.Button A3Btn;
        private System.Windows.Forms.Button A5Btn;
    }
}