namespace AddinArtama {
  partial class FrmUsuarioCad {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUsuarioCad));
      this.ckbSituacao = new LmCorbieUI.Controls.LmCheckBox();
      this.lblSituacao = new LmCorbieUI.Controls.LmLabel();
      this.lblEspessura = new LmCorbieUI.Controls.LmLabel();
      this.txtNome = new LmCorbieUI.Controls.LmTextBox();
      this.lblID = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.tbcUsuario = new LmCorbieUI.Controls.LmTabControl();
      this.tbpCadastro = new LmCorbieUI.Controls.LmTabPage();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.btnExcluir = new LmCorbieUI.Controls.LmButton();
      this.txtID = new LmCorbieUI.Controls.LmTextBox();
      this.btnLimpar = new LmCorbieUI.Controls.LmButton();
      this.txtSenha = new LmCorbieUI.Controls.LmTextBox();
      this.txtLogin = new LmCorbieUI.Controls.LmTextBox();
      this.tbpPerfil = new LmCorbieUI.Controls.LmTabPage();
      this.dgvPermis = new LmCorbieUI.Controls.LmDataGridView();
      this.lmPanel2 = new LmCorbieUI.Controls.LmPanel();
      this.btnSelPerfil = new LmCorbieUI.Controls.LmButton();
      this.btnAltPerfil = new LmCorbieUI.Controls.LmButton();
      this.btnAddPerfil = new LmCorbieUI.Controls.LmButton();
      this.dgvPerfil = new LmCorbieUI.Controls.LmDataGridView();
      this.tbcUsuario.SuspendLayout();
      this.tbpCadastro.SuspendLayout();
      this.tbpPerfil.SuspendLayout();
      this.lmPanel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // ckbSituacao
      // 
      this.ckbSituacao.AutoSize = true;
      this.ckbSituacao.BackColor = System.Drawing.Color.Transparent;
      this.ckbSituacao.Checked = true;
      this.ckbSituacao.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckbSituacao.Location = new System.Drawing.Point(78, 241);
      this.ckbSituacao.Name = "ckbSituacao";
      this.ckbSituacao.Propriedade = "Ativo";
      this.ckbSituacao.Size = new System.Drawing.Size(57, 19);
      this.ckbSituacao.TabIndex = 4;
      this.ckbSituacao.Text = "Ativo";
      this.ckbSituacao.UseSelectable = true;
      // 
      // lblSituacao
      // 
      this.lblSituacao.BackColor = System.Drawing.Color.Transparent;
      this.lblSituacao.Location = new System.Drawing.Point(3, 241);
      this.lblSituacao.Margin = new System.Windows.Forms.Padding(3);
      this.lblSituacao.Name = "lblSituacao";
      this.lblSituacao.Size = new System.Drawing.Size(69, 19);
      this.lblSituacao.TabIndex = 350;
      this.lblSituacao.Text = "Situação";
      this.lblSituacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblEspessura
      // 
      this.lblEspessura.AutoSize = true;
      this.lblEspessura.BackColor = System.Drawing.Color.Transparent;
      this.lblEspessura.Location = new System.Drawing.Point(3, 64);
      this.lblEspessura.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lblEspessura.Name = "lblEspessura";
      this.lblEspessura.Size = new System.Drawing.Size(56, 19);
      this.lblEspessura.TabIndex = 349;
      this.lblEspessura.Text = "Nome *";
      this.lblEspessura.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtNome
      // 
      this.txtNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtNome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtNome.BorderRadius = 15;
      this.txtNome.BorderSize = 2;
      this.txtNome.CampoObrigatorio = true;
      this.txtNome.F7ToolTipText = null;
      this.txtNome.F8ToolTipText = null;
      this.txtNome.F9ToolTipText = null;
      this.txtNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtNome.IconF7 = null;
      this.txtNome.IconToolTipText = null;
      this.txtNome.Lines = new string[0];
      this.txtNome.Location = new System.Drawing.Point(3, 87);
      this.txtNome.MaxLength = 250;
      this.txtNome.Name = "txtNome";
      this.txtNome.PasswordChar = '\0';
      this.txtNome.Propriedade = null;
      this.txtNome.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtNome.SelectedText = "";
      this.txtNome.SelectionLength = 0;
      this.txtNome.SelectionStart = 0;
      this.txtNome.ShortcutsEnabled = true;
      this.txtNome.Size = new System.Drawing.Size(256, 30);
      this.txtNome.TabIndex = 1;
      this.txtNome.UnderlinedStyle = false;
      this.txtNome.UseSelectable = true;
      this.txtNome.Valor_Decimais = ((short)(4));
      this.txtNome.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtNome.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lblID
      // 
      this.lblID.AutoSize = true;
      this.lblID.BackColor = System.Drawing.Color.Transparent;
      this.lblID.Location = new System.Drawing.Point(3, 3);
      this.lblID.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lblID.Name = "lblID";
      this.lblID.Size = new System.Drawing.Size(53, 19);
      this.lblID.TabIndex = 345;
      this.lblID.Text = "Código";
      this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lmLabel1
      // 
      this.lmLabel1.AutoSize = true;
      this.lmLabel1.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel1.Location = new System.Drawing.Point(3, 123);
      this.lmLabel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel1.Name = "lmLabel1";
      this.lmLabel1.Size = new System.Drawing.Size(53, 19);
      this.lmLabel1.TabIndex = 348;
      this.lmLabel1.Text = "Login *";
      this.lmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lmLabel2
      // 
      this.lmLabel2.AutoSize = true;
      this.lmLabel2.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel2.Location = new System.Drawing.Point(3, 182);
      this.lmLabel2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel2.Name = "lmLabel2";
      this.lmLabel2.Size = new System.Drawing.Size(56, 19);
      this.lmLabel2.TabIndex = 353;
      this.lmLabel2.Text = "Senha *";
      this.lmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tbcUsuario
      // 
      this.tbcUsuario.Controls.Add(this.tbpCadastro);
      this.tbcUsuario.Controls.Add(this.tbpPerfil);
      this.tbcUsuario.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbcUsuario.Location = new System.Drawing.Point(2, 30);
      this.tbcUsuario.Name = "tbcUsuario";
      this.tbcUsuario.SelectedIndex = 0;
      this.tbcUsuario.Size = new System.Drawing.Size(272, 498);
      this.tbcUsuario.TabIndex = 355;
      this.tbcUsuario.UseSelectable = true;
      // 
      // tbpCadastro
      // 
      this.tbpCadastro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpCadastro.Controls.Add(this.btnSalvar);
      this.tbpCadastro.Controls.Add(this.btnExcluir);
      this.tbpCadastro.Controls.Add(this.txtID);
      this.tbpCadastro.Controls.Add(this.btnLimpar);
      this.tbpCadastro.Controls.Add(this.lblID);
      this.tbpCadastro.Controls.Add(this.txtSenha);
      this.tbpCadastro.Controls.Add(this.lmLabel1);
      this.tbpCadastro.Controls.Add(this.lmLabel2);
      this.tbpCadastro.Controls.Add(this.txtLogin);
      this.tbpCadastro.Controls.Add(this.txtNome);
      this.tbpCadastro.Controls.Add(this.ckbSituacao);
      this.tbpCadastro.Controls.Add(this.lblEspessura);
      this.tbpCadastro.Controls.Add(this.lblSituacao);
      this.tbpCadastro.Location = new System.Drawing.Point(4, 38);
      this.tbpCadastro.Name = "tbpCadastro";
      this.tbpCadastro.Size = new System.Drawing.Size(264, 456);
      this.tbpCadastro.TabIndex = 0;
      this.tbpCadastro.Text = "Cadastro";
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
      this.btnSalvar.Location = new System.Drawing.Point(3, 276);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(256, 26);
      this.btnSalvar.TabIndex = 5;
      this.btnSalvar.Text = " &Salvar";
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
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
      this.btnExcluir.Location = new System.Drawing.Point(3, 308);
      this.btnExcluir.Name = "btnExcluir";
      this.btnExcluir.Size = new System.Drawing.Size(256, 26);
      this.btnExcluir.TabIndex = 6;
      this.btnExcluir.Text = " E&xcluir";
      this.btnExcluir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnExcluir.UseVisualStyleBackColor = false;
      this.btnExcluir.Click += new System.EventHandler(this.BtnExcluir_Click);
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
      this.txtID.Location = new System.Drawing.Point(3, 28);
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
      this.btnLimpar.Location = new System.Drawing.Point(3, 340);
      this.btnLimpar.Name = "btnLimpar";
      this.btnLimpar.Size = new System.Drawing.Size(256, 26);
      this.btnLimpar.TabIndex = 7;
      this.btnLimpar.Text = " &Limpar";
      this.btnLimpar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnLimpar.UseVisualStyleBackColor = false;
      this.btnLimpar.Click += new System.EventHandler(this.BtnLimpar_Click);
      // 
      // txtSenha
      // 
      this.txtSenha.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSenha.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtSenha.BorderRadius = 15;
      this.txtSenha.BorderSize = 2;
      this.txtSenha.CampoObrigatorio = true;
      this.txtSenha.F7ToolTipText = null;
      this.txtSenha.F8ToolTipText = null;
      this.txtSenha.F9ToolTipText = null;
      this.txtSenha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtSenha.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtSenha.IconF7")));
      this.txtSenha.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtSenha.IconF8")));
      this.txtSenha.IconToolTipText = null;
      this.txtSenha.Lines = new string[0];
      this.txtSenha.Location = new System.Drawing.Point(3, 205);
      this.txtSenha.MaxLength = 100;
      this.txtSenha.Name = "txtSenha";
      this.txtSenha.PasswordChar = '●';
      this.txtSenha.Propriedade = null;
      this.txtSenha.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtSenha.SelectedText = "";
      this.txtSenha.SelectionLength = 0;
      this.txtSenha.SelectionStart = 0;
      this.txtSenha.ShortcutsEnabled = true;
      this.txtSenha.Size = new System.Drawing.Size(256, 30);
      this.txtSenha.TabIndex = 3;
      this.txtSenha.UnderlinedStyle = false;
      this.txtSenha.UseSelectable = true;
      this.txtSenha.UseSystemPasswordChar = true;
      this.txtSenha.Valor_Decimais = ((short)(0));
      this.txtSenha.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtSenha.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // txtLogin
      // 
      this.txtLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtLogin.BorderRadius = 15;
      this.txtLogin.BorderSize = 2;
      this.txtLogin.CampoObrigatorio = true;
      this.txtLogin.F7ToolTipText = null;
      this.txtLogin.F8ToolTipText = null;
      this.txtLogin.F9ToolTipText = null;
      this.txtLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtLogin.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtLogin.IconF7")));
      this.txtLogin.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtLogin.IconF8")));
      this.txtLogin.IconToolTipText = null;
      this.txtLogin.Lines = new string[0];
      this.txtLogin.Location = new System.Drawing.Point(3, 146);
      this.txtLogin.MaxLength = 10;
      this.txtLogin.Name = "txtLogin";
      this.txtLogin.PasswordChar = '\0';
      this.txtLogin.Propriedade = null;
      this.txtLogin.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtLogin.SelectedText = "";
      this.txtLogin.SelectionLength = 0;
      this.txtLogin.SelectionStart = 0;
      this.txtLogin.ShortcutsEnabled = true;
      this.txtLogin.Size = new System.Drawing.Size(256, 30);
      this.txtLogin.TabIndex = 2;
      this.txtLogin.UnderlinedStyle = false;
      this.txtLogin.UseSelectable = true;
      this.txtLogin.Valor_Decimais = ((short)(0));
      this.txtLogin.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtLogin.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtLogin.Leave += new System.EventHandler(this.txtLogin_Leave);
      // 
      // tbpPerfil
      // 
      this.tbpPerfil.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpPerfil.Controls.Add(this.dgvPermis);
      this.tbpPerfil.Controls.Add(this.lmPanel2);
      this.tbpPerfil.Controls.Add(this.dgvPerfil);
      this.tbpPerfil.Location = new System.Drawing.Point(4, 38);
      this.tbpPerfil.Name = "tbpPerfil";
      this.tbpPerfil.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
      this.tbpPerfil.Size = new System.Drawing.Size(264, 456);
      this.tbpPerfil.TabIndex = 1;
      this.tbpPerfil.Text = "Perfil";
      // 
      // dgvPermis
      // 
      this.dgvPermis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.dgvPermis.Botao1Largura = 100;
      this.dgvPermis.Botao1Texto = "";
      this.dgvPermis.Botao2Largura = 100;
      this.dgvPermis.Botao2Texto = "";
      this.dgvPermis.ColunaOrdenacaoGrid = "";
      this.dgvPermis.ColunasBloqueadasGrid = "";
      this.dgvPermis.ColunasOcultasGrid = "";
      this.dgvPermis.ColunasOcultasImpressGrid = "";
      this.dgvPermis.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgvPermis.EnabledCsvButton = true;
      this.dgvPermis.EnabledFind = true;
      this.dgvPermis.EnabledHideColumnsButton = true;
      this.dgvPermis.EnabledPdfButton = true;
      this.dgvPermis.EnabledRefreshButton = true;
      this.dgvPermis.LimparSelecaoAposCarregar = false;
      this.dgvPermis.Location = new System.Drawing.Point(0, 218);
      this.dgvPermis.Margin = new System.Windows.Forms.Padding(0);
      this.dgvPermis.MostrarRodapeBotoes = false;
      this.dgvPermis.MostrarTotalizador = false;
      this.dgvPermis.Name = "dgvPermis";
      this.dgvPermis.PermiteAutoDimensionarLinha = false;
      this.dgvPermis.PermiteDimensionarColuna = true;
      this.dgvPermis.PermiteOrdenarColunas = true;
      this.dgvPermis.PermiteOrdenarLinhas = true;
      this.dgvPermis.PermiteQuebrarLinhaCabecalho = false;
      this.dgvPermis.PermiteSelecaoMultipla = false;
      this.dgvPermis.PosColunasGrid = "";
      this.dgvPermis.Size = new System.Drawing.Size(262, 236);
      this.dgvPermis.TabIndex = 358;
      this.dgvPermis.Texto = "";
      this.dgvPermis.TituloRelatorio = "";
      this.dgvPermis.UseSelectable = true;
      // 
      // lmPanel2
      // 
      this.lmPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel2.Controls.Add(this.btnSelPerfil);
      this.lmPanel2.Controls.Add(this.btnAltPerfil);
      this.lmPanel2.Controls.Add(this.btnAddPerfil);
      this.lmPanel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.lmPanel2.IsPanelMenu = false;
      this.lmPanel2.Location = new System.Drawing.Point(0, 118);
      this.lmPanel2.Name = "lmPanel2";
      this.lmPanel2.Size = new System.Drawing.Size(262, 100);
      this.lmPanel2.TabIndex = 357;
      // 
      // btnSelPerfil
      // 
      this.btnSelPerfil.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSelPerfil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnSelPerfil.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnSelPerfil.BorderRadius = 13;
      this.btnSelPerfil.BorderSize = 0;
      this.btnSelPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSelPerfil.Image = ((System.Drawing.Image)(resources.GetObject("btnSelPerfil.Image")));
      this.btnSelPerfil.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnSelPerfil.Location = new System.Drawing.Point(2, 3);
      this.btnSelPerfil.Name = "btnSelPerfil";
      this.btnSelPerfil.Size = new System.Drawing.Size(259, 26);
      this.btnSelPerfil.TabIndex = 5;
      this.btnSelPerfil.Text = " Selecionar";
      this.btnSelPerfil.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnSelPerfil.UseVisualStyleBackColor = false;
      this.btnSelPerfil.Click += new System.EventHandler(this.BtnSelPerfil_Click);
      // 
      // btnAltPerfil
      // 
      this.btnAltPerfil.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAltPerfil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnAltPerfil.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnAltPerfil.BorderRadius = 13;
      this.btnAltPerfil.BorderSize = 0;
      this.btnAltPerfil.Enabled = false;
      this.btnAltPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnAltPerfil.Image = ((System.Drawing.Image)(resources.GetObject("btnAltPerfil.Image")));
      this.btnAltPerfil.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnAltPerfil.Location = new System.Drawing.Point(2, 35);
      this.btnAltPerfil.Name = "btnAltPerfil";
      this.btnAltPerfil.Size = new System.Drawing.Size(259, 26);
      this.btnAltPerfil.TabIndex = 6;
      this.btnAltPerfil.Text = " Alterar";
      this.btnAltPerfil.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnAltPerfil.UseVisualStyleBackColor = false;
      this.btnAltPerfil.Click += new System.EventHandler(this.BtnAltPerfil_Click);
      // 
      // btnAddPerfil
      // 
      this.btnAddPerfil.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddPerfil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnAddPerfil.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnAddPerfil.BorderRadius = 13;
      this.btnAddPerfil.BorderSize = 0;
      this.btnAddPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnAddPerfil.Image = ((System.Drawing.Image)(resources.GetObject("btnAddPerfil.Image")));
      this.btnAddPerfil.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnAddPerfil.Location = new System.Drawing.Point(2, 67);
      this.btnAddPerfil.Name = "btnAddPerfil";
      this.btnAddPerfil.Size = new System.Drawing.Size(259, 26);
      this.btnAddPerfil.TabIndex = 7;
      this.btnAddPerfil.Text = " Perfil";
      this.btnAddPerfil.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnAddPerfil.UseVisualStyleBackColor = false;
      this.btnAddPerfil.Click += new System.EventHandler(this.BtnAddPerfil_Click);
      // 
      // dgvPerfil
      // 
      this.dgvPerfil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.dgvPerfil.Botao1Largura = 100;
      this.dgvPerfil.Botao1Texto = "";
      this.dgvPerfil.Botao2Largura = 100;
      this.dgvPerfil.Botao2Texto = "";
      this.dgvPerfil.ColunaOrdenacaoGrid = "";
      this.dgvPerfil.ColunasBloqueadasGrid = "";
      this.dgvPerfil.ColunasOcultasGrid = "";
      this.dgvPerfil.ColunasOcultasImpressGrid = "";
      this.dgvPerfil.Dock = System.Windows.Forms.DockStyle.Top;
      this.dgvPerfil.EnabledCsvButton = true;
      this.dgvPerfil.EnabledFind = true;
      this.dgvPerfil.EnabledHideColumnsButton = true;
      this.dgvPerfil.EnabledPdfButton = true;
      this.dgvPerfil.EnabledRefreshButton = true;
      this.dgvPerfil.LimparSelecaoAposCarregar = false;
      this.dgvPerfil.Location = new System.Drawing.Point(0, 6);
      this.dgvPerfil.Margin = new System.Windows.Forms.Padding(0);
      this.dgvPerfil.MostrarRodapeBotoes = false;
      this.dgvPerfil.MostrarTotalizador = false;
      this.dgvPerfil.Name = "dgvPerfil";
      this.dgvPerfil.PermiteAutoDimensionarLinha = false;
      this.dgvPerfil.PermiteDimensionarColuna = true;
      this.dgvPerfil.PermiteOrdenarColunas = true;
      this.dgvPerfil.PermiteOrdenarLinhas = true;
      this.dgvPerfil.PermiteQuebrarLinhaCabecalho = false;
      this.dgvPerfil.PermiteSelecaoMultipla = false;
      this.dgvPerfil.PosColunasGrid = "";
      this.dgvPerfil.Size = new System.Drawing.Size(262, 112);
      this.dgvPerfil.TabIndex = 0;
      this.dgvPerfil.Texto = "";
      this.dgvPerfil.TituloRelatorio = "";
      this.dgvPerfil.UseSelectable = true;
      // 
      // FrmUsuarioCad
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(276, 530);
      this.Controls.Add(this.tbcUsuario);
      this.HelpButton = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmUsuarioCad";
      this.Padding = new System.Windows.Forms.Padding(2, 30, 2, 2);
      this.Resizable = false;
      this.ShowInTaskbar = false;
      this.Text = "Cadastro de Usuário";
      this.TopMost = true;
      this.ClickHelp += new LmCorbieUI.LmForms.LmSingleForm.ButClick(this.FrmUsuarioCad_ClickHelp);
      this.Load += new System.EventHandler(this.FrmUsuarioCad_Load);
      this.tbcUsuario.ResumeLayout(false);
      this.tbpCadastro.ResumeLayout(false);
      this.tbpCadastro.PerformLayout();
      this.tbpPerfil.ResumeLayout(false);
      this.lmPanel2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion
    private LmCorbieUI.Controls.LmCheckBox ckbSituacao;
    private LmCorbieUI.Controls.LmLabel lblSituacao;
    private LmCorbieUI.Controls.LmLabel lblEspessura;
    private LmCorbieUI.Controls.LmTextBox txtNome;
    private LmCorbieUI.Controls.LmButton btnSalvar;
    private LmCorbieUI.Controls.LmButton btnLimpar;
    private LmCorbieUI.Controls.LmButton btnExcluir;
    private LmCorbieUI.Controls.LmLabel lblID;
    private LmCorbieUI.Controls.LmLabel lmLabel1;
    private LmCorbieUI.Controls.LmTextBox txtLogin;
    private LmCorbieUI.Controls.LmTextBox txtID;
    private LmCorbieUI.Controls.LmTextBox txtSenha;
    private LmCorbieUI.Controls.LmLabel lmLabel2;
    private LmCorbieUI.Controls.LmTabControl tbcUsuario;
    private LmCorbieUI.Controls.LmTabPage tbpCadastro;
    private LmCorbieUI.Controls.LmTabPage tbpPerfil;
    private LmCorbieUI.Controls.LmDataGridView dgvPermis;
    private LmCorbieUI.Controls.LmPanel lmPanel2;
    private LmCorbieUI.Controls.LmButton btnSelPerfil;
    private LmCorbieUI.Controls.LmButton btnAltPerfil;
    private LmCorbieUI.Controls.LmButton btnAddPerfil;
    private LmCorbieUI.Controls.LmDataGridView dgvPerfil;
  }
}