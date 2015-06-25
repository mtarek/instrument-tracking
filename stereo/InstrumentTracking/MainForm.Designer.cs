namespace InstrumentTracking
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnInitializeStage = new System.Windows.Forms.ToolStripButton();
            this.btnHomeStage = new System.Windows.Forms.ToolStripButton();
            this.btnCalibrateCameras = new System.Windows.Forms.ToolStripButton();
            this.btnCalibInstrument = new System.Windows.Forms.ToolStripButton();
            this.btnSetOrigin = new System.Windows.Forms.ToolStripButton();
            this.btnTrack = new System.Windows.Forms.ToolStripButton();
            this.timerTracking = new System.Windows.Forms.Timer(this.components);
            this.timerPhase = new System.Windows.Forms.Timer(this.components);
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.timerCalib = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnInitializeStage,
            this.btnHomeStage,
            this.btnCalibrateCameras,
            this.btnCalibInstrument,
            this.btnSetOrigin,
            this.btnTrack});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(929, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnInitializeStage
            // 
            this.btnInitializeStage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnInitializeStage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInitializeStage.Name = "btnInitializeStage";
            this.btnInitializeStage.Size = new System.Drawing.Size(86, 22);
            this.btnInitializeStage.Text = "Initialize Stage";
            this.btnInitializeStage.Click += new System.EventHandler(this.btnInitializeStage_Click);
            // 
            // btnHomeStage
            // 
            this.btnHomeStage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnHomeStage.Image = ((System.Drawing.Image)(resources.GetObject("btnHomeStage.Image")));
            this.btnHomeStage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHomeStage.Name = "btnHomeStage";
            this.btnHomeStage.Size = new System.Drawing.Size(76, 22);
            this.btnHomeStage.Text = "Home Stage";
            this.btnHomeStage.Click += new System.EventHandler(this.btnHomeStage_Click);
            // 
            // btnCalibrateCameras
            // 
            this.btnCalibrateCameras.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCalibrateCameras.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCalibrateCameras.Name = "btnCalibrateCameras";
            this.btnCalibrateCameras.Size = new System.Drawing.Size(107, 22);
            this.btnCalibrateCameras.Text = "Calibrate Cameras";
            this.btnCalibrateCameras.Click += new System.EventHandler(this.btnCalibrateCameras_Click);
            // 
            // btnCalibInstrument
            // 
            this.btnCalibInstrument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCalibInstrument.Image = ((System.Drawing.Image)(resources.GetObject("btnCalibInstrument.Image")));
            this.btnCalibInstrument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCalibInstrument.Name = "btnCalibInstrument";
            this.btnCalibInstrument.Size = new System.Drawing.Size(119, 22);
            this.btnCalibInstrument.Text = "Calibrate Instrument";
            this.btnCalibInstrument.Click += new System.EventHandler(this.btnCalibInstrument_Click);
            // 
            // btnSetOrigin
            // 
            this.btnSetOrigin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSetOrigin.Image = ((System.Drawing.Image)(resources.GetObject("btnSetOrigin.Image")));
            this.btnSetOrigin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetOrigin.Name = "btnSetOrigin";
            this.btnSetOrigin.Size = new System.Drawing.Size(63, 22);
            this.btnSetOrigin.Text = "Set Origin";
            this.btnSetOrigin.Click += new System.EventHandler(this.btnSetOrigin_Click);
            // 
            // btnTrack
            // 
            this.btnTrack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTrack.Image = ((System.Drawing.Image)(resources.GetObject("btnTrack.Image")));
            this.btnTrack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(84, 22);
            this.btnTrack.Text = "Start Tracking";
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            // 
            // timerTracking
            // 
            this.timerTracking.Interval = 5;
            this.timerTracking.Tick += new System.EventHandler(this.timerTracking_Tick);
            // 
            // timerPhase
            // 
            this.timerPhase.Interval = 3;
            this.timerPhase.Tick += new System.EventHandler(this.timerPhase_Tick);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 1;
            this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(687, 142);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            52,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 1;
            this.numericUpDown2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown2.Location = new System.Drawing.Point(687, 55);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown2.TabIndex = 5;
            this.numericUpDown2.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(687, 82);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(115, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Log Tracking Data";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // timerCalib
            // 
            this.timerCalib.Interval = 10;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 616);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.toolStrip1);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnInitializeStage;
        private System.Windows.Forms.ToolStripButton btnCalibrateCameras;
        private System.Windows.Forms.ToolStripButton btnHomeStage;
        private System.Windows.Forms.ToolStripButton btnTrack;
        private System.Windows.Forms.Timer timerTracking;
        private System.Windows.Forms.Timer timerPhase;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ToolStripButton btnSetOrigin;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolStripButton btnCalibInstrument;
        private System.Windows.Forms.Timer timerCalib;
    }
}