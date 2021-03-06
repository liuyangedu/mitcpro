﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionCheck : Form
    {
        //private ExtructionProcess extructionformfather = null;
        private Color greencolor = Color.FromArgb(0, 255, 0);
        private Color redcolor = Color.FromArgb(255, 100, 100);

        private SqlConnection conn = null;
        private string sql = "Select * From extrusion";

        //5个报表
        public bool page7finished = false;
        public String page7recorder = "记录人";
        public String page7checker = "审核人";
        public bool page8finished = false;
        public String page8recorder = "记录人";
        public String page8checker = "审核人";
        public bool page10finished = false;
        public String page10recorder = "记录人";
        public String page10checker = "审核人";
        public bool page11finished = false;
        public String page11recorder = "记录人";
        public String page11checker = "审核人";
        public bool page13finished = false;
        public String page13recorder = "记录人";
        public String page13checker = "审核人";
        //6个工序
        public String step1recorder = "张三";
        public String step1checker = "李四";
        public String step2recorder = "张三";
        public String step2checker = "李四";
        public String step3recorder = "张三";
        public String step3checker = "李四";
        public String step4recorder = "张三";
        public String step4checker = "李四";
        public String step5recorder = "张三";
        public String step5checker = "李四";
        public String step6recorder = "张三";
        public String step6checker = "李四";

        //public ExtructionCheck(ExtructionProcess winMain, SqlConnection Mainconn)
        //{
        //    InitializeComponent();
        //    extructionformfather = winMain;

        //    conn = Mainconn;

        //    ShowNameandColor();
        //}

        private void ShowNameandColor()
        {
            //7个报表           
            if (page7finished)
            { 
                Page7Recorder.Text = page7recorder; Page7Checker.Text = page7checker;
                this.Page7Label.BackColor = greencolor;
            }
            else
            { 
                Page7Recorder.Text = "暂未完成"; Page7Checker.Text = "暂未完成";
                this.Page7Label.BackColor = redcolor;
            }
            if (page8finished)
            { 
                Page8Recorder.Text = page8recorder; Page8Checker.Text = page8checker;
                this.Page8Label.BackColor = greencolor;
            }
            else
            { 
                Page8Recorder.Text = "暂未完成"; Page8Checker.Text = "暂未完成";
                this.Page8Label.BackColor = redcolor;
            }
            if (page10finished)
            { 
                Page10Recorder.Text = page10recorder; Page10Checker.Text = page10checker;
                this.Page10Label.BackColor = greencolor;
            }
            else
            { 
                Page10Recorder.Text = "暂未完成"; Page10Checker.Text = "暂未完成";
                this.Page10Label.BackColor = redcolor;
            }
            if (page11finished)
            { 
                Page11Recorder.Text = page11recorder; Page11Checker.Text = page11checker;
                this.Page11Label.BackColor = greencolor;
            }
            else
            { 
                Page11Recorder.Text = "暂未完成"; Page11Checker.Text = "暂未完成";
                this.Page11Label.BackColor = redcolor;
            }
            if (page13finished)
            { 
                Page13Recorder.Text = page13recorder; Page13Checker.Text = page13checker;
                this.Page13Label.BackColor = greencolor;
            }
            else
            { 
                Page13Recorder.Text = "暂未完成"; Page13Checker.Text = "暂未完成";
                this.Page13Label.BackColor = redcolor;
            }
            //6个工序
            Step1Recorder.Text = step1recorder;
            Step1Checker.Text = step1checker;
            this.Step1Label.BackColor = greencolor;
            Step2Recorder.Text = step2recorder;
            Step2Checker.Text = step2checker;
            this.Step2Label.BackColor = greencolor;
            Step3Recorder.Text = step3recorder;
            Step3Checker.Text = step3checker;
            this.Step3Label.BackColor = greencolor;
            Step4Recorder.Text = step4recorder;
            Step4Checker.Text = step4checker;
            this.Step4Label.BackColor = greencolor;
            Step5Recorder.Text = step5recorder;
            Step5Checker.Text = step5checker;
            this.Step5Label.BackColor = greencolor;
            Step6Recorder.Text = step6recorder;
            Step6Checker.Text = step6checker;
            this.Step6Label.BackColor = greencolor;
        }
    }
}
