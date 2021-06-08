namespace Subrip
{
    partial class FormRip
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.numericUpDownInterval = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownColorMargin = new System.Windows.Forms.NumericUpDown();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboBoxScreen = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDownDistance = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDownRatioTh = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnTextAnalysis = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnAddtoDataset = new System.Windows.Forms.Button();
            this.pictureBoxGrouped = new System.Windows.Forms.PictureBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColorMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRatioTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGrouped)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(204, 81);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 36);
            this.button2.TabIndex = 6;
            this.button2.Text = "Color";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // numericUpDownInterval
            // 
            this.numericUpDownInterval.Increment = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownInterval.Location = new System.Drawing.Point(595, 45);
            this.numericUpDownInterval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownInterval.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numericUpDownInterval.Name = "numericUpDownInterval";
            this.numericUpDownInterval.Size = new System.Drawing.Size(112, 31);
            this.numericUpDownInterval.TabIndex = 7;
            this.numericUpDownInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownInterval.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDownColorMargin
            // 
            this.numericUpDownColorMargin.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numericUpDownColorMargin.Increment = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownColorMargin.Location = new System.Drawing.Point(204, 45);
            this.numericUpDownColorMargin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownColorMargin.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numericUpDownColorMargin.Name = "numericUpDownColorMargin";
            this.numericUpDownColorMargin.Size = new System.Drawing.Size(112, 31);
            this.numericUpDownColorMargin.TabIndex = 11;
            this.numericUpDownColorMargin.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(19, 461);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "X";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(669, 100);
            this.chart1.TabIndex = 13;
            this.chart1.Text = "chart1";
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            this.chart2.Location = new System.Drawing.Point(696, 461);
            this.chart2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Name = "Y";
            this.chart2.Series.Add(series2);
            this.chart2.Size = new System.Drawing.Size(112, 100);
            this.chart2.TabIndex = 15;
            this.chart2.Text = "chart2";
            // 
            // comboBoxScreen
            // 
            this.comboBoxScreen.FormattingEnabled = true;
            this.comboBoxScreen.Items.AddRange(new object[] {
            "Main",
            "Secondary"});
            this.comboBoxScreen.Location = new System.Drawing.Point(716, 42);
            this.comboBoxScreen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxScreen.Name = "comboBoxScreen";
            this.comboBoxScreen.Size = new System.Drawing.Size(91, 33);
            this.comboBoxScreen.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(589, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 25);
            this.label5.TabIndex = 27;
            this.label5.Text = "Interval";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(199, 11);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 25);
            this.label6.TabIndex = 28;
            this.label6.Text = "Color Margin";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(711, 11);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 25);
            this.label9.TabIndex = 31;
            this.label9.Text = "Screen";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(327, 11);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(127, 25);
            this.label10.TabIndex = 33;
            this.label10.Text = "Distance Th";
            // 
            // numericUpDownDistance
            // 
            this.numericUpDownDistance.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numericUpDownDistance.Location = new System.Drawing.Point(332, 45);
            this.numericUpDownDistance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownDistance.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownDistance.Name = "numericUpDownDistance";
            this.numericUpDownDistance.Size = new System.Drawing.Size(112, 31);
            this.numericUpDownDistance.TabIndex = 32;
            this.numericUpDownDistance.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(21, 581);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 100);
            this.panel1.TabIndex = 34;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(463, 11);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 25);
            this.label11.TabIndex = 37;
            this.label11.Text = "Ratio Th";
            // 
            // numericUpDownRatioTh
            // 
            this.numericUpDownRatioTh.DecimalPlaces = 2;
            this.numericUpDownRatioTh.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numericUpDownRatioTh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownRatioTh.Location = new System.Drawing.Point(468, 45);
            this.numericUpDownRatioTh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownRatioTh.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRatioTh.Name = "numericUpDownRatioTh";
            this.numericUpDownRatioTh.Size = new System.Drawing.Size(112, 31);
            this.numericUpDownRatioTh.TabIndex = 36;
            this.numericUpDownRatioTh.Value = new decimal(new int[] {
            95,
            0,
            0,
            131072});
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(21, 689);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(787, 100);
            this.panel2.TabIndex = 42;
            // 
            // btnTextAnalysis
            // 
            this.btnTextAnalysis.Image = global::Subrip.Properties.Resources.lupa;
            this.btnTextAnalysis.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTextAnalysis.Location = new System.Drawing.Point(123, 65);
            this.btnTextAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTextAnalysis.Name = "btnTextAnalysis";
            this.btnTextAnalysis.Size = new System.Drawing.Size(53, 50);
            this.btnTextAnalysis.TabIndex = 45;
            this.btnTextAnalysis.UseVisualStyleBackColor = true;
            this.btnTextAnalysis.Click += new System.EventHandler(this.btnTextAnalysis_Click);
            // 
            // button6
            // 
            this.button6.BackgroundImage = global::Subrip.Properties.Resources.select;
            this.button6.Image = global::Subrip.Properties.Resources.select;
            this.button6.Location = new System.Drawing.Point(16, 15);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(53, 50);
            this.button6.TabIndex = 44;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // btnAddtoDataset
            // 
            this.btnAddtoDataset.Image = global::Subrip.Properties.Resources.add;
            this.btnAddtoDataset.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnAddtoDataset.Location = new System.Drawing.Point(124, 15);
            this.btnAddtoDataset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddtoDataset.Name = "btnAddtoDataset";
            this.btnAddtoDataset.Size = new System.Drawing.Size(53, 50);
            this.btnAddtoDataset.TabIndex = 43;
            this.btnAddtoDataset.UseVisualStyleBackColor = true;
            this.btnAddtoDataset.Click += new System.EventHandler(this.button4_Click);
            // 
            // pictureBoxGrouped
            // 
            this.pictureBoxGrouped.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxGrouped.Location = new System.Drawing.Point(21, 349);
            this.pictureBoxGrouped.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBoxGrouped.Name = "pictureBoxGrouped";
            this.pictureBoxGrouped.Size = new System.Drawing.Size(786, 100);
            this.pictureBoxGrouped.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxGrouped.TabIndex = 35;
            this.pictureBoxGrouped.TabStop = false;
            // 
            // btnStop
            // 
            this.btnStop.Image = global::Subrip.Properties.Resources.stop;
            this.btnStop.Location = new System.Drawing.Point(67, 64);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(53, 50);
            this.btnStop.TabIndex = 18;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.button5_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(21, 236);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(786, 100);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Image = global::Subrip.Properties.Resources.play;
            this.btnStart.Location = new System.Drawing.Point(16, 62);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(53, 50);
            this.btnStart.TabIndex = 9;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Image = global::Subrip.Properties.Resources.snapshot;
            this.button1.Location = new System.Drawing.Point(67, 15);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(53, 50);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(20, 124);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(787, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(332, 88);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(229, 29);
            this.checkBox1.TabIndex = 46;
            this.checkBox1.Text = "Load Full Dictionary";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(21, 796);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(787, 100);
            this.panel3.TabIndex = 43;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(576, 80);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 36);
            this.button3.TabIndex = 47;
            this.button3.Text = "HSK";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // FormRip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(817, 935);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnTextAnalysis);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.btnAddtoDataset);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numericUpDownRatioTh);
            this.Controls.Add(this.pictureBoxGrouped);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numericUpDownDistance);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxScreen);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.numericUpDownColorMargin);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.numericUpDownInterval);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormRip";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SubRip";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColorMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRatioTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGrouped)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NumericUpDown numericUpDownInterval;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownColorMargin;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ComboBox comboBoxScreen;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDownDistance;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxGrouped;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDownRatioTh;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAddtoDataset;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnTextAnalysis;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button3;
    }
}

