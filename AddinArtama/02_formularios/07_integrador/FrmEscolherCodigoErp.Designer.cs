namespace AddinArtama {
  partial class FrmEscolherCodigoErp {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.flpChosen = new LmCorbieUI.Controls.LmPanelFlow();
      this.lmLabel3 = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel12 = new LmCorbieUI.Controls.LmLabel();
      this.flpChosen.SuspendLayout();
      this.SuspendLayout();
      // 
      // lmLabel1
      // 
      this.lmLabel1.BackColor = System.Drawing.Color.Silver;
      this.lmLabel1.FontSize = LmCorbieUI.Design.LmLabelSize.Tall;
      this.lmLabel1.Location = new System.Drawing.Point(102, 12);
      this.lmLabel1.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel1.Name = "lmLabel1";
      this.lmLabel1.Size = new System.Drawing.Size(151, 39);
      this.lmLabel1.TabIndex = 77;
      this.lmLabel1.Text = "300101526";
      this.lmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.lmLabel1.UseCustomColor = true;
      // 
      // lmLabel2
      // 
      this.lmLabel2.BackColor = System.Drawing.Color.Silver;
      this.lmLabel2.FontSize = LmCorbieUI.Design.LmLabelSize.Tall;
      this.lmLabel2.Location = new System.Drawing.Point(102, 57);
      this.lmLabel2.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel2.Name = "lmLabel2";
      this.lmLabel2.Size = new System.Drawing.Size(151, 39);
      this.lmLabel2.TabIndex = 78;
      this.lmLabel2.Text = "300101527";
      this.lmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.lmLabel2.UseCustomColor = true;
      // 
      // flpChosen
      // 
      this.flpChosen.AllowDrop = true;
      this.flpChosen.AutoScroll = true;
      this.flpChosen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.flpChosen.Controls.Add(this.lmLabel1);
      this.flpChosen.Controls.Add(this.lmLabel2);
      this.flpChosen.Controls.Add(this.lmLabel3);
      this.flpChosen.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flpChosen.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flpChosen.Location = new System.Drawing.Point(2, 90);
      this.flpChosen.Name = "flpChosen";
      this.flpChosen.Padding = new System.Windows.Forms.Padding(99, 9, 0, 30);
      this.flpChosen.Size = new System.Drawing.Size(340, 265);
      this.flpChosen.TabIndex = 79;
      this.flpChosen.WrapContents = false;
      // 
      // lmLabel3
      // 
      this.lmLabel3.BackColor = System.Drawing.Color.Silver;
      this.lmLabel3.FontSize = LmCorbieUI.Design.LmLabelSize.Tall;
      this.lmLabel3.Location = new System.Drawing.Point(102, 102);
      this.lmLabel3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
      this.lmLabel3.Name = "lmLabel3";
      this.lmLabel3.Size = new System.Drawing.Size(151, 39);
      this.lmLabel3.TabIndex = 79;
      this.lmLabel3.Text = "300101527";
      this.lmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.lmLabel3.UseCustomColor = true;
      // 
      // lmLabel12
      // 
      this.lmLabel12.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel12.Dock = System.Windows.Forms.DockStyle.Top;
      this.lmLabel12.Location = new System.Drawing.Point(2, 30);
      this.lmLabel12.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel12.Name = "lmLabel12";
      this.lmLabel12.Size = new System.Drawing.Size(340, 60);
      this.lmLabel12.TabIndex = 80;
      this.lmLabel12.Text = "Analise no ERP os códigos abaixo.\r\nSelecione o código a manter.\r\nDemais serão \"In" +
    "ativados\".";
      this.lmLabel12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // FrmEscolherCodigoErp
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(344, 357);
      this.Controls.Add(this.flpChosen);
      this.Controls.Add(this.lmLabel12);
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(732, 550);
      this.MinimizeBox = false;
      this.Name = "FrmEscolherCodigoErp";
      this.Padding = new System.Windows.Forms.Padding(2, 30, 2, 2);
      this.Resizable = false;
      this.Text = "Escolher Código Produto";
      this.flpChosen.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion
    private LmCorbieUI.Controls.LmLabel lmLabel1;
    private LmCorbieUI.Controls.LmLabel lmLabel2;
    private LmCorbieUI.Controls.LmPanelFlow flpChosen;
    private LmCorbieUI.Controls.LmLabel lmLabel12;
    private LmCorbieUI.Controls.LmLabel lmLabel3;
  }
}