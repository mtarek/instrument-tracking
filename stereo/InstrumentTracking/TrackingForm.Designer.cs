namespace InstrumentTracking
{
    partial class TrackingForm
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
            this.pbTracking = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbTracking)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTracking
            // 
            this.pbTracking.BackColor = System.Drawing.Color.Black;
            this.pbTracking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbTracking.Location = new System.Drawing.Point(0, 0);
            this.pbTracking.Name = "pbTracking";
            this.pbTracking.Size = new System.Drawing.Size(485, 435);
            this.pbTracking.TabIndex = 0;
            this.pbTracking.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(376, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // TrackingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 435);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbTracking);
            this.Name = "TrackingForm";
            this.Text = "TrackingForm";
            ((System.ComponentModel.ISupportInitialize)(this.pbTracking)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbTracking;
        private System.Windows.Forms.Label label1;
    }
}