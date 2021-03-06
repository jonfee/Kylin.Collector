﻿namespace ProductCollector
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDownloadCategory = new System.Windows.Forms.Button();
            this.btnLoadLocationCategory = new System.Windows.Forms.Button();
            this.lbStatistics = new System.Windows.Forms.Label();
            this.panelCategory = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSourceSite = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtxtMsg = new System.Windows.Forms.RichTextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDownloadCategory);
            this.groupBox1.Controls.Add(this.btnLoadLocationCategory);
            this.groupBox1.Controls.Add(this.lbStatistics);
            this.groupBox1.Controls.Add(this.panelCategory);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbSourceSite);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(875, 336);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数设置";
            // 
            // btnDownloadCategory
            // 
            this.btnDownloadCategory.Location = new System.Drawing.Point(472, 27);
            this.btnDownloadCategory.Name = "btnDownloadCategory";
            this.btnDownloadCategory.Size = new System.Drawing.Size(85, 23);
            this.btnDownloadCategory.TabIndex = 18;
            this.btnDownloadCategory.Text = "下载最新分类";
            this.btnDownloadCategory.UseVisualStyleBackColor = true;
            this.btnDownloadCategory.Click += new System.EventHandler(this.btnDownloadCategory_Click);
            // 
            // btnLoadLocationCategory
            // 
            this.btnLoadLocationCategory.Location = new System.Drawing.Point(368, 27);
            this.btnLoadLocationCategory.Name = "btnLoadLocationCategory";
            this.btnLoadLocationCategory.Size = new System.Drawing.Size(87, 23);
            this.btnLoadLocationCategory.TabIndex = 17;
            this.btnLoadLocationCategory.Text = "本地加载分类";
            this.btnLoadLocationCategory.UseVisualStyleBackColor = true;
            this.btnLoadLocationCategory.Click += new System.EventHandler(this.btnLoadLocationCategory_Click);
            // 
            // lbStatistics
            // 
            this.lbStatistics.AutoSize = true;
            this.lbStatistics.Location = new System.Drawing.Point(375, 300);
            this.lbStatistics.Name = "lbStatistics";
            this.lbStatistics.Size = new System.Drawing.Size(0, 12);
            this.lbStatistics.TabIndex = 16;
            // 
            // panelCategory
            // 
            this.panelCategory.AutoScroll = true;
            this.panelCategory.Location = new System.Drawing.Point(133, 73);
            this.panelCategory.Name = "panelCategory";
            this.panelCategory.Size = new System.Drawing.Size(725, 203);
            this.panelCategory.TabIndex = 15;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(232, 294);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 14;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(134, 294);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 13;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "需要采集的分类：";
            // 
            // cbSourceSite
            // 
            this.cbSourceSite.FormattingEnabled = true;
            this.cbSourceSite.Location = new System.Drawing.Point(134, 29);
            this.cbSourceSite.Name = "cbSourceSite";
            this.cbSourceSite.Size = new System.Drawing.Size(217, 20);
            this.cbSourceSite.TabIndex = 1;
            this.cbSourceSite.SelectedIndexChanged += new System.EventHandler(this.cbSourceSite_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "采集站点：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtxtMsg);
            this.groupBox2.Location = new System.Drawing.Point(13, 397);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(875, 334);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "采集结果";
            // 
            // rtxtMsg
            // 
            this.rtxtMsg.Location = new System.Drawing.Point(7, 21);
            this.rtxtMsg.Name = "rtxtMsg";
            this.rtxtMsg.Size = new System.Drawing.Size(862, 307);
            this.rtxtMsg.TabIndex = 0;
            this.rtxtMsg.Text = "";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 366);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(875, 13);
            this.progressBar.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 743);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "产品采集器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSourceSite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panelCategory;
        private System.Windows.Forms.RichTextBox rtxtMsg;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lbStatistics;
        private System.Windows.Forms.Button btnDownloadCategory;
        private System.Windows.Forms.Button btnLoadLocationCategory;
    }
}

