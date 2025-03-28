namespace AddinArtama
{
    partial class FrmManutProps
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManutProps));
      this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
      this.lblInfo = new LmCorbieUI.Controls.LmLabel();
      this.btnIniciar = new LmCorbieUI.Controls.LmButton();
      this.lmPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // lmPanel1
      // 
      this.lmPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel1.Controls.Add(this.lblInfo);
      this.lmPanel1.Controls.Add(this.btnIniciar);
      this.lmPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lmPanel1.IsPanelMenu = false;
      this.lmPanel1.Location = new System.Drawing.Point(0, 30);
      this.lmPanel1.Name = "lmPanel1";
      this.lmPanel1.Size = new System.Drawing.Size(255, 596);
      this.lmPanel1.TabIndex = 0;
      // 
      // lblInfo
      // 
      this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.lblInfo.FontWeight = LmCorbieUI.Design.LmLabelWeight.Bold;
      this.lblInfo.Location = new System.Drawing.Point(-3, 264);
      this.lblInfo.Margin = new System.Windows.Forms.Padding(3);
      this.lblInfo.Name = "lblInfo";
      this.lblInfo.Size = new System.Drawing.Size(255, 104);
      this.lblInfo.TabIndex = 87;
      this.lblInfo.Text = "Esta manutenção removerá as propriedades \"PINTURA\" e \"CHECK\" da aba \"Específico d" +
    "a configuração\" da tela \"Propriedades do arquivo\".";
      this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.lblInfo.WrapToLine = true;
      // 
      // btnIniciar
      // 
      this.btnIniciar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.btnIniciar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnIniciar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnIniciar.BorderRadius = 15;
      this.btnIniciar.BorderSize = 0;
      this.btnIniciar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnIniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnIniciar.Image = ((System.Drawing.Image)(resources.GetObject("btnIniciar.Image")));
      this.btnIniciar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnIniciar.Location = new System.Drawing.Point(11, 209);
      this.btnIniciar.Margin = new System.Windows.Forms.Padding(1);
      this.btnIniciar.Name = "btnIniciar";
      this.btnIniciar.Size = new System.Drawing.Size(232, 31);
      this.btnIniciar.TabIndex = 4;
      this.btnIniciar.Tag = "Avançar";
      this.btnIniciar.Text = "   Iniciar Manutenção";
      this.btnIniciar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnIniciar.UseVisualStyleBackColor = false;
      this.btnIniciar.Click += new System.EventHandler(this.BtnIniciar_Click);
      // 
      // FrmManutProps
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(255, 626);
      this.Controls.Add(this.lmPanel1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Movimentar = false;
      this.Name = "FrmManutProps";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.Resizable = false;
      this.Text = "Manutenção Packlist e Plano de Pintura";
      this.lmPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
        private LmCorbieUI.Controls.LmPanel lmPanel1;
    private LmCorbieUI.Controls.LmButton btnIniciar;
    private LmCorbieUI.Controls.LmLabel lblInfo;
  }
}