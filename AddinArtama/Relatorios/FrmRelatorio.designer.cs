namespace AddinArtama
{
    partial class FrmRelatorio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRelatorio));
            this.rpv1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // rpv1
            // 
            this.rpv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rpv1.Location = new System.Drawing.Point(2, 30);
            this.rpv1.Name = "rpv1";
            this.rpv1.ServerReport.BearerToken = null;
            this.rpv1.Size = new System.Drawing.Size(689, 280);
            this.rpv1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Tag = "0";
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FrmRelatorio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 312);
            this.Controls.Add(this.rpv1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FrmRelatorio";
            this.Padding = new System.Windows.Forms.Padding(2, 30, 2, 2);
            this.ShowIcon = false;
            this.Text = "Relatorio";
            this.Load += new System.EventHandler(this.FrmRelatorio_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmRelatorio_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rpv1;
        private System.Windows.Forms.Timer timer1;
    }
}