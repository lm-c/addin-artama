namespace AddinArtama {
  partial class FrmPerfil {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPerfil));
      this.lblEspessura = new LmCorbieUI.Controls.LmLabel();
      this.txtDescricao = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.lblID = new LmCorbieUI.Controls.LmLabel();
      this.lmGroupBox1 = new LmCorbieUI.Controls.LmGroupBox();
      this.flpUsuarios = new LmCorbieUI.Controls.LmPanelFlow();
      this.btnPermissao = new LmCorbieUI.Controls.LmButton();
      this.txtID = new LmCorbieUI.Controls.LmTextBox();
      this.txtObservacao = new LmCorbieUI.Controls.LmTextBox();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.btnLimpar = new LmCorbieUI.Controls.LmButton();
      this.btnExcluir = new LmCorbieUI.Controls.LmButton();
      this.lmGroupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblEspessura
      // 
      this.lblEspessura.AutoSize = true;
      this.lblEspessura.BackColor = System.Drawing.Color.Transparent;
      this.lblEspessura.Location = new System.Drawing.Point(6, 98);
      this.lblEspessura.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lblEspessura.Name = "lblEspessura";
      this.lblEspessura.Size = new System.Drawing.Size(77, 19);
      this.lblEspessura.TabIndex = 364;
      this.lblEspessura.Text = "Descrição *";
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
      this.txtDescricao.Location = new System.Drawing.Point(6, 121);
      this.txtDescricao.MaxLength = 80;
      this.txtDescricao.Name = "txtDescricao";
      this.txtDescricao.PasswordChar = '\0';
      this.txtDescricao.Propriedade = null;
      this.txtDescricao.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtDescricao.SelectedText = "";
      this.txtDescricao.SelectionLength = 0;
      this.txtDescricao.SelectionStart = 0;
      this.txtDescricao.ShortcutsEnabled = true;
      this.txtDescricao.Size = new System.Drawing.Size(264, 30);
      this.txtDescricao.TabIndex = 1;
      this.txtDescricao.UnderlinedStyle = false;
      this.txtDescricao.UseSelectable = true;
      this.txtDescricao.Valor_Decimais = ((short)(4));
      this.txtDescricao.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtDescricao.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel1
      // 
      this.lmLabel1.AutoSize = true;
      this.lmLabel1.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel1.Location = new System.Drawing.Point(6, 157);
      this.lmLabel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel1.Name = "lmLabel1";
      this.lmLabel1.Size = new System.Drawing.Size(81, 19);
      this.lmLabel1.TabIndex = 363;
      this.lmLabel1.Text = "Observação";
      this.lmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblID
      // 
      this.lblID.AutoSize = true;
      this.lblID.BackColor = System.Drawing.Color.Transparent;
      this.lblID.Location = new System.Drawing.Point(6, 37);
      this.lblID.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lblID.Name = "lblID";
      this.lblID.Size = new System.Drawing.Size(53, 19);
      this.lblID.TabIndex = 362;
      this.lblID.Text = "Código";
      this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lmGroupBox1
      // 
      this.lmGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lmGroupBox1.Controls.Add(this.flpUsuarios);
      this.lmGroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(41)))), ((int)(((byte)(38)))));
      this.lmGroupBox1.Location = new System.Drawing.Point(3, 413);
      this.lmGroupBox1.Name = "lmGroupBox1";
      this.lmGroupBox1.Size = new System.Drawing.Size(270, 188);
      this.lmGroupBox1.TabIndex = 368;
      this.lmGroupBox1.TabStop = false;
      this.lmGroupBox1.Text = "Usuários";
      // 
      // flpUsuarios
      // 
      this.flpUsuarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.flpUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flpUsuarios.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flpUsuarios.Location = new System.Drawing.Point(3, 20);
      this.flpUsuarios.Name = "flpUsuarios";
      this.flpUsuarios.Size = new System.Drawing.Size(264, 165);
      this.flpUsuarios.TabIndex = 0;
      this.flpUsuarios.WrapContents = false;
      // 
      // btnPermissao
      // 
      this.btnPermissao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnPermissao.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnPermissao.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnPermissao.BorderRadius = 13;
      this.btnPermissao.BorderSize = 0;
      this.btnPermissao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnPermissao.Image = ((System.Drawing.Image)(resources.GetObject("btnPermissao.Image")));
      this.btnPermissao.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnPermissao.Location = new System.Drawing.Point(6, 381);
      this.btnPermissao.Name = "btnPermissao";
      this.btnPermissao.Size = new System.Drawing.Size(264, 26);
      this.btnPermissao.TabIndex = 6;
      this.btnPermissao.Text = " &Definir Permissões";
      this.btnPermissao.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnPermissao.UseVisualStyleBackColor = false;
      this.btnPermissao.Click += new System.EventHandler(this.BtnPermissao_Click);
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
      this.txtID.Location = new System.Drawing.Point(6, 60);
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
      // txtObservacao
      // 
      this.txtObservacao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtObservacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtObservacao.BorderRadius = 15;
      this.txtObservacao.BorderSize = 2;
      this.txtObservacao.F7ToolTipText = null;
      this.txtObservacao.F8ToolTipText = null;
      this.txtObservacao.F9ToolTipText = null;
      this.txtObservacao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtObservacao.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtObservacao.IconF7")));
      this.txtObservacao.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtObservacao.IconF8")));
      this.txtObservacao.IconToolTipText = null;
      this.txtObservacao.Lines = new string[0];
      this.txtObservacao.Location = new System.Drawing.Point(6, 180);
      this.txtObservacao.MaxLength = 250;
      this.txtObservacao.Multiline = true;
      this.txtObservacao.Name = "txtObservacao";
      this.txtObservacao.PasswordChar = '\0';
      this.txtObservacao.Propriedade = null;
      this.txtObservacao.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtObservacao.SelectedText = "";
      this.txtObservacao.SelectionLength = 0;
      this.txtObservacao.SelectionStart = 0;
      this.txtObservacao.ShortcutsEnabled = true;
      this.txtObservacao.Size = new System.Drawing.Size(264, 99);
      this.txtObservacao.TabIndex = 2;
      this.txtObservacao.UnderlinedStyle = false;
      this.txtObservacao.UseSelectable = true;
      this.txtObservacao.Valor_Decimais = ((short)(0));
      this.txtObservacao.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtObservacao.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
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
      this.btnSalvar.Location = new System.Drawing.Point(6, 285);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(264, 26);
      this.btnSalvar.TabIndex = 3;
      this.btnSalvar.Text = " &Salvar";
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
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
      this.btnLimpar.Location = new System.Drawing.Point(6, 349);
      this.btnLimpar.Name = "btnLimpar";
      this.btnLimpar.Size = new System.Drawing.Size(264, 26);
      this.btnLimpar.TabIndex = 5;
      this.btnLimpar.Text = " &Limpar";
      this.btnLimpar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnLimpar.UseVisualStyleBackColor = false;
      this.btnLimpar.Click += new System.EventHandler(this.BtnLimpar_Click);
      // 
      // btnExcluir
      // 
      this.btnExcluir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExcluir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnExcluir.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnExcluir.BorderRadius = 13;
      this.btnExcluir.BorderSize = 0;
      this.btnExcluir.Enabled = false;
      this.btnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnExcluir.Image = ((System.Drawing.Image)(resources.GetObject("btnExcluir.Image")));
      this.btnExcluir.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnExcluir.Location = new System.Drawing.Point(6, 317);
      this.btnExcluir.Name = "btnExcluir";
      this.btnExcluir.Size = new System.Drawing.Size(264, 26);
      this.btnExcluir.TabIndex = 4;
      this.btnExcluir.Text = " E&xcluir";
      this.btnExcluir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnExcluir.UseVisualStyleBackColor = false;
      this.btnExcluir.Click += new System.EventHandler(this.BtnExcluir_Click);
      // 
      // FrmPerfil
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(276, 604);
      this.Controls.Add(this.lmGroupBox1);
      this.Controls.Add(this.btnPermissao);
      this.Controls.Add(this.txtID);
      this.Controls.Add(this.lblEspessura);
      this.Controls.Add(this.txtDescricao);
      this.Controls.Add(this.txtObservacao);
      this.Controls.Add(this.lmLabel1);
      this.Controls.Add(this.btnSalvar);
      this.Controls.Add(this.btnLimpar);
      this.Controls.Add(this.btnExcluir);
      this.Controls.Add(this.lblID);
      this.HelpButton = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmPerfil";
      this.Padding = new System.Windows.Forms.Padding(3, 30, 3, 3);
      this.ShowInTaskbar = false;
      this.Text = "Cadastro de Perfil do Usuário";
      this.ClickHelp += new LmCorbieUI.LmForms.LmSingleForm.ButClick(this.FrmPerfil_ClickHelp);
      this.Load += new System.EventHandler(this.FrmPerfil_Load);
      this.lmGroupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private LmCorbieUI.Controls.LmTextBox txtID;
    private LmCorbieUI.Controls.LmLabel lblEspessura;
    private LmCorbieUI.Controls.LmTextBox txtDescricao;
    private LmCorbieUI.Controls.LmTextBox txtObservacao;
    private LmCorbieUI.Controls.LmLabel lmLabel1;
    private LmCorbieUI.Controls.LmButton btnSalvar;
    private LmCorbieUI.Controls.LmButton btnLimpar;
    private LmCorbieUI.Controls.LmButton btnExcluir;
    private LmCorbieUI.Controls.LmLabel lblID;
    private LmCorbieUI.Controls.LmButton btnPermissao;
    private LmCorbieUI.Controls.LmGroupBox lmGroupBox1;
    private LmCorbieUI.Controls.LmPanelFlow flpUsuarios;
  }
}