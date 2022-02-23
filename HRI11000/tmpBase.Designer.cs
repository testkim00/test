namespace SMM3200
{
    partial class SMM3200
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
            this.panBase = new JPlatform.Client.Controls6.PanelEx();
            ((System.ComponentModel.ISupportInitialize)(this.FormMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FoemComboInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseTextEditEx.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).BeginInit();
            this.SuspendLayout();
            // 
            // BaseTextEditEx
            // 
            this.BaseTextEditEx.Properties.Appearance.Font = new System.Drawing.Font("돋움체", 9F);
            this.BaseTextEditEx.Properties.Appearance.Options.UseFont = true;
            // 
            // panBase
            // 
            this.panBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBase.Location = new System.Drawing.Point(0, 0);
            this.panBase.Name = "panBase";
            this.panBase.Size = new System.Drawing.Size(1020, 591);
            this.panBase.TabIndex = 0;
            // 
            // SMM3200
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 591);
            this.Controls.Add(this.panBase);
            this.Name = "SMM3200";
            this.Text = "SMM3200";
            ((System.ComponentModel.ISupportInitialize)(this.FormMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FoemComboInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseTextEditEx.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panBase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private JPlatform.Client.Controls6.PanelEx panBase;
    }
}

