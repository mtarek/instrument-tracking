namespace InstrumentTracking
{
    partial class ThorMotorController
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThorMotorController));
            this.axMG17Motor1 = new AxMG17MotorLib.AxMG17Motor();
            ((System.ComponentModel.ISupportInitialize)(this.axMG17Motor1)).BeginInit();
            this.SuspendLayout();
            // 
            // axMG17Motor1
            // 
            this.axMG17Motor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMG17Motor1.Enabled = true;
            this.axMG17Motor1.Location = new System.Drawing.Point(0, 0);
            this.axMG17Motor1.Name = "axMG17Motor1";
            this.axMG17Motor1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMG17Motor1.OcxState")));
            this.axMG17Motor1.Size = new System.Drawing.Size(360, 337);
            this.axMG17Motor1.TabIndex = 0;
            // 
            // ThorMotorController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 337);
            this.Controls.Add(this.axMG17Motor1);
            this.Name = "ThorMotorController";
            this.Text = "ThorMotorController";
            ((System.ComponentModel.ISupportInitialize)(this.axMG17Motor1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMG17MotorLib.AxMG17Motor axMG17Motor1;
    }
}