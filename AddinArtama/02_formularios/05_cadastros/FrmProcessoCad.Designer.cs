namespace AddinArtama {
  partial class FrmProcessoCad {
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProcessoCad));
      this.ckbSituacao = new LmCorbieUI.Controls.LmCheckBox();
      this.lblSituacao = new LmCorbieUI.Controls.LmLabel();
      this.lblID = new LmCorbieUI.Controls.LmLabel();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.txtID = new LmCorbieUI.Controls.LmTextBox();
      this.btnLimpar = new LmCorbieUI.Controls.LmButton();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.txtMascaraMaquina = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.txtOperacao = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel3 = new LmCorbieUI.Controls.LmLabel();
      this.txtCentroCusto = new LmCorbieUI.Controls.LmTextBox();
      this.SuspendLayout();
      // 
      // ckbSituacao
      // 
      this.ckbSituacao.AutoSize = true;
      this.ckbSituacao.BackColor = System.Drawing.Color.Transparent;
      this.ckbSituacao.Checked = true;
      this.ckbSituacao.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckbSituacao.Location = new System.Drawing.Point(76, 268);
      this.ckbSituacao.Name = "ckbSituacao";
      this.ckbSituacao.Propriedade = "Ativo";
      this.ckbSituacao.Size = new System.Drawing.Size(57, 19);
      this.ckbSituacao.TabIndex = 4;
      this.ckbSituacao.Text = "Ativo";
      this.ckbSituacao.UseSelectable = true;
      // 
      // lblSituacao
      // 
      this.lblSituacao.AutoSize = true;
      this.lblSituacao.BackColor = System.Drawing.Color.Transparent;
      this.lblSituacao.Location = new System.Drawing.Point(10, 268);
      this.lblSituacao.Margin = new System.Windows.Forms.Padding(3);
      this.lblSituacao.Name = "lblSituacao";
      this.lblSituacao.Size = new System.Drawing.Size(60, 19);
      this.lblSituacao.TabIndex = 5;
      this.lblSituacao.Text = "Situação";
      this.lblSituacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblID
      // 
      this.lblID.AutoSize = true;
      this.lblID.BackColor = System.Drawing.Color.Transparent;
      this.lblID.Location = new System.Drawing.Point(10, 33);
      this.lblID.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lblID.Name = "lblID";
      this.lblID.Size = new System.Drawing.Size(53, 19);
      this.lblID.TabIndex = 345;
      this.lblID.Text = "Código";
      this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // btnSalvar
      // 
      this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSalvar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnSalvar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnSalvar.BorderRadius = 13;
      this.btnSalvar.BorderSize = 0;
      this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSalvar.Image = ((System.Drawing.Image)(resources.GetObject("btnSalvar.Image")));
      this.btnSalvar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnSalvar.Location = new System.Drawing.Point(10, 294);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(256, 26);
      this.btnSalvar.TabIndex = 5;
      this.btnSalvar.Text = " &Salvar";
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
      // 
      // txtID
      // 
      this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtID.BorderRadius = 15;
      this.txtID.BorderSize = 2;
      this.txtID.F7ToolTipText = "Pesquisar [F7]";
      this.txtID.F8ToolTipText = "Item Anterior [F8]";
      this.txtID.F9ToolTipText = "Próximo Item [F9]";
      this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtID.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtID.IconF7")));
      this.txtID.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtID.IconF8")));
      this.txtID.IconF9 = ((System.Drawing.Image)(resources.GetObject("txtID.IconF9")));
      this.txtID.IconToolTipText = null;
      this.txtID.Lines = new string[0];
      this.txtID.Location = new System.Drawing.Point(10, 58);
      this.txtID.MaxLength = 32767;
      this.txtID.Name = "txtID";
      this.txtID.PasswordChar = '\0';
      this.txtID.Propriedade = "ID";
      this.txtID.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtID.SelectedText = "";
      this.txtID.SelectionLength = 0;
      this.txtID.SelectionStart = 0;
      this.txtID.ShortcutsEnabled = true;
      this.txtID.ShowButtonF7 = true;
      this.txtID.Size = new System.Drawing.Size(152, 30);
      this.txtID.TabIndex = 0;
      this.txtID.UnderlinedStyle = false;
      this.txtID.UseSelectable = true;
      this.txtID.Valor = LmCorbieUI.Design.LmValueType.Num_Inteiro;
      this.txtID.Valor_Decimais = ((short)(0));
      this.txtID.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtID.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtID.ButtonClickF7 += new LmCorbieUI.Controls.LmTextBox.ButClick(this.TxtID_ButtonClickF7);
      this.txtID.Leave += new System.EventHandler(this.TxtID_Leave);
      // 
      // btnLimpar
      // 
      this.btnLimpar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnLimpar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnLimpar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnLimpar.BorderRadius = 13;
      this.btnLimpar.BorderSize = 0;
      this.btnLimpar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnLimpar.Image = ((System.Drawing.Image)(resources.GetObject("btnLimpar.Image")));
      this.btnLimpar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnLimpar.Location = new System.Drawing.Point(10, 326);
      this.btnLimpar.Name = "btnLimpar";
      this.btnLimpar.Size = new System.Drawing.Size(256, 26);
      this.btnLimpar.TabIndex = 5;
      this.btnLimpar.Text = " &Limpar";
      this.btnLimpar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnLimpar.UseVisualStyleBackColor = false;
      this.btnLimpar.Click += new System.EventHandler(this.BtnLimpar_Click);
      // 
      // lmLabel2
      // 
      this.lmLabel2.AutoSize = true;
      this.lmLabel2.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel2.Location = new System.Drawing.Point(10, 153);
      this.lmLabel2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel2.Name = "lmLabel2";
      this.lmLabel2.Size = new System.Drawing.Size(128, 19);
      this.lmLabel2.TabIndex = 354;
      this.lmLabel2.Text = "Máscara Máquina *";
      this.lmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtMascaraMaquina
      // 
      this.txtMascaraMaquina.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtMascaraMaquina.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtMascaraMaquina.BorderRadius = 15;
      this.txtMascaraMaquina.BorderSize = 2;
      this.txtMascaraMaquina.CampoObrigatorio = true;
      this.txtMascaraMaquina.F7ToolTipText = null;
      this.txtMascaraMaquina.F8ToolTipText = null;
      this.txtMascaraMaquina.F9ToolTipText = null;
      this.txtMascaraMaquina.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtMascaraMaquina.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtMascaraMaquina.IconF7")));
      this.txtMascaraMaquina.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtMascaraMaquina.IconF8")));
      this.txtMascaraMaquina.IconToolTipText = null;
      this.txtMascaraMaquina.Lines = new string[0];
      this.txtMascaraMaquina.Location = new System.Drawing.Point(10, 176);
      this.txtMascaraMaquina.MaxLength = 250;
      this.txtMascaraMaquina.Name = "txtMascaraMaquina";
      this.txtMascaraMaquina.PasswordChar = '\0';
      this.txtMascaraMaquina.Propriedade = null;
      this.txtMascaraMaquina.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtMascaraMaquina.SelectedText = "";
      this.txtMascaraMaquina.SelectionLength = 0;
      this.txtMascaraMaquina.SelectionStart = 0;
      this.txtMascaraMaquina.ShortcutsEnabled = true;
      this.txtMascaraMaquina.Size = new System.Drawing.Size(256, 30);
      this.txtMascaraMaquina.TabIndex = 2;
      this.txtMascaraMaquina.UnderlinedStyle = false;
      this.txtMascaraMaquina.UseSelectable = true;
      this.txtMascaraMaquina.Valor_Decimais = ((short)(4));
      this.txtMascaraMaquina.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtMascaraMaquina.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel1
      // 
      this.lmLabel1.AutoSize = true;
      this.lmLabel1.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel1.Location = new System.Drawing.Point(10, 94);
      this.lmLabel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel1.Name = "lmLabel1";
      this.lmLabel1.Size = new System.Drawing.Size(78, 19);
      this.lmLabel1.TabIndex = 356;
      this.lmLabel1.Text = "Operação *";
      this.lmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtOperacao
      // 
      this.txtOperacao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtOperacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtOperacao.BorderRadius = 15;
      this.txtOperacao.BorderSize = 2;
      this.txtOperacao.CampoObrigatorio = true;
      this.txtOperacao.F7ToolTipText = null;
      this.txtOperacao.F8ToolTipText = null;
      this.txtOperacao.F9ToolTipText = null;
      this.txtOperacao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtOperacao.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtOperacao.IconF7")));
      this.txtOperacao.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtOperacao.IconF8")));
      this.txtOperacao.IconToolTipText = null;
      this.txtOperacao.Lines = new string[0];
      this.txtOperacao.Location = new System.Drawing.Point(10, 117);
      this.txtOperacao.MaxLength = 250;
      this.txtOperacao.Name = "txtOperacao";
      this.txtOperacao.PasswordChar = '\0';
      this.txtOperacao.Propriedade = null;
      this.txtOperacao.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtOperacao.SelectedText = "";
      this.txtOperacao.SelectionLength = 0;
      this.txtOperacao.SelectionStart = 0;
      this.txtOperacao.ShortcutsEnabled = true;
      this.txtOperacao.ShowButtonF7 = true;
      this.txtOperacao.Size = new System.Drawing.Size(256, 30);
      this.txtOperacao.TabIndex = 1;
      this.txtOperacao.UnderlinedStyle = false;
      this.txtOperacao.UseSelectable = true;
      this.txtOperacao.Valor = LmCorbieUI.Design.LmValueType.ComboBox;
      this.txtOperacao.Valor_Decimais = ((short)(4));
      this.txtOperacao.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtOperacao.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel3
      // 
      this.lmLabel3.AutoSize = true;
      this.lmLabel3.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel3.Location = new System.Drawing.Point(10, 212);
      this.lmLabel3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel3.Name = "lmLabel3";
      this.lmLabel3.Size = new System.Drawing.Size(120, 19);
      this.lmLabel3.TabIndex = 358;
      this.lmLabel3.Text = "Centro de Custo *";
      this.lmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtCentroCusto
      // 
      this.txtCentroCusto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtCentroCusto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtCentroCusto.BorderRadius = 15;
      this.txtCentroCusto.BorderSize = 2;
      this.txtCentroCusto.CampoObrigatorio = true;
      this.txtCentroCusto.F7ToolTipText = null;
      this.txtCentroCusto.F8ToolTipText = null;
      this.txtCentroCusto.F9ToolTipText = null;
      this.txtCentroCusto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtCentroCusto.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtCentroCusto.IconF7")));
      this.txtCentroCusto.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtCentroCusto.IconF8")));
      this.txtCentroCusto.IconToolTipText = null;
      this.txtCentroCusto.Lines = new string[0];
      this.txtCentroCusto.Location = new System.Drawing.Point(10, 235);
      this.txtCentroCusto.MaxLength = 250;
      this.txtCentroCusto.Name = "txtCentroCusto";
      this.txtCentroCusto.PasswordChar = '\0';
      this.txtCentroCusto.Propriedade = null;
      this.txtCentroCusto.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtCentroCusto.SelectedText = "";
      this.txtCentroCusto.SelectionLength = 0;
      this.txtCentroCusto.SelectionStart = 0;
      this.txtCentroCusto.ShortcutsEnabled = true;
      this.txtCentroCusto.Size = new System.Drawing.Size(256, 30);
      this.txtCentroCusto.TabIndex = 3;
      this.txtCentroCusto.UnderlinedStyle = false;
      this.txtCentroCusto.UseSelectable = true;
      this.txtCentroCusto.Valor_Decimais = ((short)(4));
      this.txtCentroCusto.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtCentroCusto.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // FrmProcessoCad
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(276, 530);
      this.Controls.Add(this.lmLabel3);
      this.Controls.Add(this.txtCentroCusto);
      this.Controls.Add(this.lmLabel1);
      this.Controls.Add(this.txtOperacao);
      this.Controls.Add(this.lmLabel2);
      this.Controls.Add(this.txtMascaraMaquina);
      this.Controls.Add(this.btnSalvar);
      this.Controls.Add(this.lblID);
      this.Controls.Add(this.txtID);
      this.Controls.Add(this.lblSituacao);
      this.Controls.Add(this.btnLimpar);
      this.Controls.Add(this.ckbSituacao);
      this.HelpButton = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmProcessoCad";
      this.Padding = new System.Windows.Forms.Padding(2, 30, 2, 2);
      this.Resizable = false;
      this.ShowInTaskbar = false;
      this.Text = "Cadastro de Processo";
      this.TopMost = true;
      this.ClickHelp += new LmCorbieUI.LmForms.LmSingleForm.ButClick(this.FrmProcessoCad_ClickHelp);
      this.Load += new System.EventHandler(this.FrmProcessoCad_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private LmCorbieUI.Controls.LmCheckBox ckbSituacao;
    private LmCorbieUI.Controls.LmLabel lblSituacao;
    private LmCorbieUI.Controls.LmButton btnSalvar;
    private LmCorbieUI.Controls.LmButton btnLimpar;
    private LmCorbieUI.Controls.LmLabel lblID;
    private LmCorbieUI.Controls.LmTextBox txtID;
    private LmCorbieUI.Controls.LmLabel lmLabel2;
    private LmCorbieUI.Controls.LmTextBox txtMascaraMaquina;
    private LmCorbieUI.Controls.LmLabel lmLabel1;
    private LmCorbieUI.Controls.LmTextBox txtOperacao;
    private LmCorbieUI.Controls.LmLabel lmLabel3;
    private LmCorbieUI.Controls.LmTextBox txtCentroCusto;
  }
}