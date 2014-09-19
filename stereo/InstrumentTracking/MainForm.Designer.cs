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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnInitializeStage = new System.Windows.Forms.ToolStripButton();
            this.btnHomeStage = new System.Windows.Forms.ToolStripButton();
            this.btnCalibrateCameras = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnInitializeStage,
            this.btnHomeStage,
            this.btnCalibrateCameras});
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 616);
            this.Controls.Add(this.toolStrip1);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnInitializeStage;
        private System.Windows.Forms.ToolStripButton btnCalibrateCameras;
        private System.Windows.Forms.ToolStripButton btnHomeStage;
    }
}