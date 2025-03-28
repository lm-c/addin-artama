namespace AddinArtama {
  partial class FrmMateriaPrimaCad {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMateriaPrimaCad));
      this.ckbSituacao = new LmCorbieUI.Controls.LmCheckBox();
      this.lblSituacao = new LmCorbieUI.Controls.LmLabel();
      this.lblEspessura = new LmCorbieUI.Controls.LmLabel();
      this.txtDescricao = new LmCorbieUI.Controls.LmTextBox();
      this.lblID = new LmCorbieUI.Controls.LmLabel();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.txtID = new LmCorbieUI.Controls.LmTextBox();
      this.btnLimpar = new LmCorbieUI.Controls.LmButton();
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.txtEspessura = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.txtMaterial = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel3 = new LmCorbieUI.Controls.LmLabel();
      this.txtCodigoChapa = new LmCorbieUI.Controls.LmTextBox();
      this.SuspendLayout();
      // 
      // ckbSituacao
      // 
      this.ckbSituacao.AutoSize = true;
      this.ckbSituacao.BackColor = System.Drawing.Color.Transparent;
      this.ckbSituacao.Checked = true;
      this.ckbSituacao.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckbSituacao.Location = new System.Drawing.Point(85, 336);
      this.ckbSituacao.Name = "ckbSituacao";
      this.ckbSituacao.Propriedade = "Ativo";
      this.ckbSituacao.Size = new System.Drawing.Size(57, 19);
      this.ckbSituacao.TabIndex = 6;
      this.ckbSituacao.Text = "Ativo";
      this.ckbSituacao.UseSelectable = true;
      // 
      // lblSituacao
      // 
      this.lblSituacao.BackColor = System.Drawing.Color.Transparent;
      this.lblSituacao.Location = new System.Drawing.Point(10, 335);
      this.lblSituacao.Margin = new System.Windows.Forms.Padding(3);
      this.lblSituacao.Name = "lblSituacao";
      this.lblSituacao.Size = new System.Drawing.Size(69, 19);
      this.lblSituacao.TabIndex = 5;
      this.lblSituacao.Text = "Situação";
      this.lblSituacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblEspessura
      // 
      this.lblEspessura.AutoSize = true;
      this.lblEspessura.BackColor = System.Drawing.Color.Transparent;
      this.lblEspessura.Location = new System.Drawing.Point(10, 212);
      this.lblEspessura.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lblEspessura.Name = "lblEspessura";
      this.lblEspessura.Size = new System.Drawing.Size(120, 19);
      this.lblEspessura.TabIndex = 349;
      this.lblEspessura.Text = "Descrição Chapa *";
      this.lblEspessura.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtDescricao
      // 
      this.txtDescricao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDescricao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtDescricao.BorderRadius = 15;
      this.txtDescricao.BorderSize = 2;
      this.txtDescricao.CampoObrigatorio = true;
      this.txtDescricao.F7ToolTipText = null;
      this.txtDescricao.F8ToolTipText = null;
      this.txtDescricao.F9ToolTipText = null;
      this.txtDescricao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtDescricao.IconF7 = null;
      this.txtDescricao.IconToolTipText = null;
      this.txtDescricao.Lines = new string[0];
      this.txtDescricao.Location = new System.Drawing.Point(10, 235);
      this.txtDescricao.MaxLength = 250;
      this.txtDescricao.Name = "txtDescricao";
      this.txtDescricao.PasswordChar = '\0';
      this.txtDescricao.Propriedade = null;
      this.txtDescricao.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtDescricao.SelectedText = "";
      this.txtDescricao.SelectionLength = 0;
      this.txtDescricao.SelectionStart = 0;
      this.txtDescricao.ShortcutsEnabled = true;
      this.txtDescricao.Size = new System.Drawing.Size(256, 30);
      this.txtDescricao.TabIndex = 3;
      this.txtDescricao.UnderlinedStyle = false;
      this.txtDescricao.UseSelectable = true;
      this.txtDescricao.Valor_Decimais = ((short)(4));
      this.txtDescricao.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtDescricao.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
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
      this.btnSalvar.Location = new System.Drawing.Point(10, 365);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(256, 26);
      this.btnSalvar.TabIndex = 7;
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
      this.btnLimpar.Location = new System.Drawing.Point(10, 397);
      this.btnLimpar.Name = "btnLimpar";
      this.btnLimpar.Size = new System.Drawing.Size(256, 26);
      this.btnLimpar.TabIndex = 8;
      this.btnLimpar.Text = " &Limpar";
      this.btnLimpar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnLimpar.UseVisualStyleBackColor = false;
      this.btnLimpar.Click += new System.EventHandler(this.BtnLimpar_Click);
      // 
      // lmLabel1
      // 
      this.lmLabel1.AutoSize = true;
      this.lmLabel1.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel1.Location = new System.Drawing.Point(10, 94);
      this.lmLabel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel1.Name = "lmLabel1";
      this.lmLabel1.Size = new System.Drawing.Size(79, 19);
      this.lmLabel1.TabIndex = 352;
      this.lmLabel1.Text = "Espessura *";
      this.lmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtEspessura
      // 
      this.txtEspessura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtEspessura.BorderRadius = 15;
      this.txtEspessura.BorderSize = 2;
      this.txtEspessura.CampoObrigatorio = true;
      this.txtEspessura.F7ToolTipText = null;
      this.txtEspessura.F8ToolTipText = null;
      this.txtEspessura.F9ToolTipText = null;
      this.txtEspessura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtEspessura.IconF7 = null;
      this.txtEspessura.IconToolTipText = null;
      this.txtEspessura.Lines = new string[0];
      this.txtEspessura.Location = new System.Drawing.Point(10, 117);
      this.txtEspessura.MaxLength = 250;
      this.txtEspessura.Name = "txtEspessura";
      this.txtEspessura.PasswordChar = '\0';
      this.txtEspessura.Propriedade = null;
      this.txtEspessura.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtEspessura.SelectedText = "";
      this.txtEspessura.SelectionLength = 0;
      this.txtEspessura.SelectionStart = 0;
      this.txtEspessura.ShortcutsEnabled = true;
      this.txtEspessura.Size = new System.Drawing.Size(152, 30);
      this.txtEspessura.TabIndex = 1;
      this.txtEspessura.UnderlinedStyle = false;
      this.txtEspessura.UseSelectable = true;
      this.txtEspessura.Valor = LmCorbieUI.Design.LmValueType.Num_Real;
      this.txtEspessura.Valor_Decimais = ((short)(4));
      this.txtEspessura.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtEspessura.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel2
      // 
      this.lmLabel2.AutoSize = true;
      this.lmLabel2.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel2.Location = new System.Drawing.Point(10, 276);
      this.lmLabel2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel2.Name = "lmLabel2";
      this.lmLabel2.Size = new System.Drawing.Size(69, 19);
      this.lmLabel2.TabIndex = 354;
      this.lmLabel2.Text = "Material *";
      this.lmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtMaterial
      // 
      this.txtMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtMaterial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtMaterial.BorderRadius = 15;
      this.txtMaterial.BorderSize = 2;
      this.txtMaterial.CampoObrigatorio = true;
      this.txtMaterial.F7ToolTipText = null;
      this.txtMaterial.F8ToolTipText = null;
      this.txtMaterial.F9ToolTipText = null;
      this.txtMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtMaterial.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtMaterial.IconF7")));
      this.txtMaterial.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtMaterial.IconF8")));
      this.txtMaterial.IconToolTipText = null;
      this.txtMaterial.Lines = new string[0];
      this.txtMaterial.Location = new System.Drawing.Point(10, 299);
      this.txtMaterial.MaxLength = 250;
      this.txtMaterial.Name = "txtMaterial";
      this.txtMaterial.PasswordChar = '\0';
      this.txtMaterial.Propriedade = null;
      this.txtMaterial.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtMaterial.SelectedText = "";
      this.txtMaterial.SelectionLength = 0;
      this.txtMaterial.SelectionStart = 0;
      this.txtMaterial.ShortcutsEnabled = true;
      this.txtMaterial.ShowButtonF7 = true;
      this.txtMaterial.Size = new System.Drawing.Size(256, 30);
      this.txtMaterial.TabIndex = 4;
      this.txtMaterial.UnderlinedStyle = false;
      this.txtMaterial.UseSelectable = true;
      this.txtMaterial.Valor = LmCorbieUI.Design.LmValueType.ComboBox;
      this.txtMaterial.Valor_Decimais = ((short)(4));
      this.txtMaterial.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtMaterial.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel3
      // 
      this.lmLabel3.AutoSize = true;
      this.lmLabel3.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel3.Location = new System.Drawing.Point(10, 153);
      this.lmLabel3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel3.Name = "lmLabel3";
      this.lmLabel3.Size = new System.Drawing.Size(106, 19);
      this.lmLabel3.TabIndex = 356;
      this.lmLabel3.Text = "Código Chapa *";
      this.lmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtCodigoChapa
      // 
      this.txtCodigoChapa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtCodigoChapa.BorderRadius = 15;
      this.txtCodigoChapa.BorderSize = 2;
      this.txtCodigoChapa.CampoObrigatorio = true;
      this.txtCodigoChapa.F7ToolTipText = null;
      this.txtCodigoChapa.F8ToolTipText = null;
      this.txtCodigoChapa.F9ToolTipText = null;
      this.txtCodigoChapa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtCodigoChapa.IconF7 = null;
      this.txtCodigoChapa.IconToolTipText = null;
      this.txtCodigoChapa.Lines = new string[0];
      this.txtCodigoChapa.Location = new System.Drawing.Point(10, 176);
      this.txtCodigoChapa.MaxLength = 250;
      this.txtCodigoChapa.Name = "txtCodigoChapa";
      this.txtCodigoChapa.PasswordChar = '\0';
      this.txtCodigoChapa.Propriedade = null;
      this.txtCodigoChapa.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtCodigoChapa.SelectedText = "";
      this.txtCodigoChapa.SelectionLength = 0;
      this.txtCodigoChapa.SelectionStart = 0;
      this.txtCodigoChapa.ShortcutsEnabled = true;
      this.txtCodigoChapa.Size = new System.Drawing.Size(152, 30);
      this.txtCodigoChapa.TabIndex = 2;
      this.txtCodigoChapa.UnderlinedStyle = false;
      this.txtCodigoChapa.UseSelectable = true;
      this.txtCodigoChapa.Valor = LmCorbieUI.Design.LmValueType.Num_Inteiro;
      this.txtCodigoChapa.Valor_Decimais = ((short)(4));
      this.txtCodigoChapa.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtCodigoChapa.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // FrmMateriaPrimaCad
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(276, 530);
      this.Controls.Add(this.lmLabel3);
      this.Controls.Add(this.txtCodigoChapa);
      this.Controls.Add(this.lmLabel2);
      this.Controls.Add(this.txtMaterial);
      this.Controls.Add(this.lmLabel1);
      this.Controls.Add(this.txtEspessura);
      this.Controls.Add(this.btnSalvar);
      this.Controls.Add(this.lblID);
      this.Controls.Add(this.txtID);
      this.Controls.Add(this.lblSituacao);
      this.Controls.Add(this.btnLimpar);
      this.Controls.Add(this.lblEspessura);
      this.Controls.Add(this.ckbSituacao);
      this.Controls.Add(this.txtDescricao);
      this.HelpButton = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmMateriaPrimaCad";
      this.Padding = new System.Windows.Forms.Padding(2, 30, 2, 2);
      this.Resizable = false;
      this.ShowInTaskbar = false;
      this.Text = "Cadastro de Matéria Prima";
      this.TopMost = true;
      this.ClickHelp += new LmCorbieUI.LmForms.LmSingleForm.ButClick(this.FrmMateriaPrimaCad_ClickHelp);
      this.Load += new System.EventHandler(this.FrmMateriaPrimaCad_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private LmCorbieUI.Controls.LmCheckBox ckbSituacao;
    private LmCorbieUI.Controls.LmLabel lblSituacao;
    private LmCorbieUI.Controls.LmLabel lblEspessura;
    private LmCorbieUI.Controls.LmTextBox txtDescricao;
    private LmCorbieUI.Controls.LmButton btnSalvar;
    private LmCorbieUI.Controls.LmButton btnLimpar;
    private LmCorbieUI.Controls.LmLabel lblID;
    private LmCorbieUI.Controls.LmTextBox txtID;
    private LmCorbieUI.Controls.LmLabel lmLabel1;
    private LmCorbieUI.Controls.LmTextBox txtEspessura;
    private LmCorbieUI.Controls.LmLabel lmLabel2;
    private LmCorbieUI.Controls.LmTextBox txtMaterial;
    private LmCorbieUI.Controls.LmLabel lmLabel3;
    private LmCorbieUI.Controls.LmTextBox txtCodigoChapa;
  }
}