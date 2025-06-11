namespace AddinArtama
{
    partial class FrmMateriaPrimaApl
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMateriaPrimaApl));
      this.txtComponente = new LmCorbieUI.Controls.LmTextBox();
      this.cmxLabel5 = new LmCorbieUI.Controls.LmLabel();
      this.lblCodMat = new LmCorbieUI.Controls.LmLabel();
      this.cmxLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.pnlControl = new LmCorbieUI.Controls.LmPanel();
      this.btnProximo = new LmCorbieUI.Controls.LmButton();
      this.btnVoltar = new LmCorbieUI.Controls.LmButton();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.btnCarrProcess = new LmCorbieUI.Controls.LmButton();
      this.lblPecasProc = new LmCorbieUI.Controls.LmLabel();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.ckbAddDenom = new LmCorbieUI.Controls.LmCheckBox();
      this.pnlDados = new LmCorbieUI.Controls.LmPanel();
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.lblMaterial = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel4 = new LmCorbieUI.Controls.LmLabel();
      this.txtDescricao = new LmCorbieUI.Controls.LmTextBox();
      this.lblPeso = new LmCorbieUI.Controls.LmLabel();
      this.txtMaterial = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel5 = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel6 = new LmCorbieUI.Controls.LmLabel();
      this.lblDescMat = new LmCorbieUI.Controls.LmLabel();
      this.lblProcess = new LmCorbieUI.Controls.LmLabel();
      this.lblEspess = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel8 = new LmCorbieUI.Controls.LmLabel();
      this.tbcOperacoes = new LmCorbieUI.Controls.LmTabControl();
      this.tbpOperacoes = new LmCorbieUI.Controls.LmTabPage();
      this.flpOperacoes = new LmCorbieUI.Controls.LmPanelFlow();
      this.txtProcurar = new LmCorbieUI.Controls.LmTextBox();
      this.tbpLista = new LmCorbieUI.Controls.LmTabPage();
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.tmrInicioLocalizar = new System.Windows.Forms.Timer(this.components);
      this.pnlControl.SuspendLayout();
      this.pnlDados.SuspendLayout();
      this.tbcOperacoes.SuspendLayout();
      this.tbpOperacoes.SuspendLayout();
      this.tbpLista.SuspendLayout();
      this.SuspendLayout();
      // 
      // txtComponente
      // 
      this.txtComponente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtComponente.BorderRadius = 15;
      this.txtComponente.BorderSize = 2;
      this.txtComponente.F7ToolTipText = "Atualiza Componente";
      this.txtComponente.F8ToolTipText = "Salvar Como, Peça e Montagem";
      this.txtComponente.F9ToolTipText = null;
      this.txtComponente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtComponente.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtComponente.IconF7")));
      this.txtComponente.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtComponente.IconF8")));
      this.txtComponente.IconToolTipText = null;
      this.txtComponente.Lines = new string[0];
      this.txtComponente.Location = new System.Drawing.Point(0, 186);
      this.txtComponente.MaxLength = 32767;
      this.txtComponente.Name = "txtComponente";
      this.txtComponente.PasswordChar = '\0';
      this.txtComponente.Propriedade = null;
      this.txtComponente.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtComponente.SelectedText = "";
      this.txtComponente.SelectionLength = 0;
      this.txtComponente.SelectionStart = 0;
      this.txtComponente.ShortcutsEnabled = true;
      this.txtComponente.ShowButtonF7 = true;
      this.txtComponente.ShowButtonF8 = true;
      this.txtComponente.Size = new System.Drawing.Size(134, 31);
      this.txtComponente.TabIndex = 1;
      this.txtComponente.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.txtComponente.UnderlinedStyle = false;
      this.txtComponente.UseSelectable = true;
      this.txtComponente.Valor_Decimais = ((short)(0));
      this.txtComponente.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtComponente.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtComponente.ButtonClickF7 += new LmCorbieUI.Controls.LmTextBox.ButClick(this.TxtComponente_ButtonClickF7);
      this.txtComponente.ButtonClickF8 += new LmCorbieUI.Controls.LmTextBox.ButClick(this.TxtComponente_ButtonClickF8);
      // 
      // cmxLabel5
      // 
      this.cmxLabel5.BackColor = System.Drawing.Color.Transparent;
      this.cmxLabel5.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.cmxLabel5.Location = new System.Drawing.Point(0, 170);
      this.cmxLabel5.Margin = new System.Windows.Forms.Padding(3);
      this.cmxLabel5.Name = "cmxLabel5";
      this.cmxLabel5.Size = new System.Drawing.Size(107, 15);
      this.cmxLabel5.TabIndex = 22;
      this.cmxLabel5.Text = "COMPONENTE:";
      this.cmxLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lblCodMat
      // 
      this.lblCodMat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblCodMat.BackColor = System.Drawing.Color.Transparent;
      this.lblCodMat.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblCodMat.ForeColor = System.Drawing.Color.Red;
      this.lblCodMat.Location = new System.Drawing.Point(123, 45);
      this.lblCodMat.Margin = new System.Windows.Forms.Padding(3);
      this.lblCodMat.Name = "lblCodMat";
      this.lblCodMat.Size = new System.Drawing.Size(210, 15);
      this.lblCodMat.TabIndex = 18;
      this.lblCodMat.Text = "---";
      this.lblCodMat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // cmxLabel2
      // 
      this.cmxLabel2.BackColor = System.Drawing.Color.Transparent;
      this.cmxLabel2.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.cmxLabel2.Location = new System.Drawing.Point(2, 45);
      this.cmxLabel2.Margin = new System.Windows.Forms.Padding(3);
      this.cmxLabel2.Name = "cmxLabel2";
      this.cmxLabel2.Size = new System.Drawing.Size(119, 15);
      this.cmxLabel2.TabIndex = 15;
      this.cmxLabel2.Text = "Código Material:";
      this.cmxLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // pnlControl
      // 
      this.pnlControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.pnlControl.Controls.Add(this.btnProximo);
      this.pnlControl.Controls.Add(this.btnVoltar);
      this.pnlControl.Controls.Add(this.btnSalvar);
      this.pnlControl.Controls.Add(this.btnCarrProcess);
      this.pnlControl.Controls.Add(this.lblPecasProc);
      this.pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlControl.IsPanelMenu = false;
      this.pnlControl.Location = new System.Drawing.Point(0, 308);
      this.pnlControl.Name = "pnlControl";
      this.pnlControl.Size = new System.Drawing.Size(340, 43);
      this.pnlControl.TabIndex = 3;
      // 
      // btnProximo
      // 
      this.btnProximo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnProximo.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnProximo.BorderRadius = 15;
      this.btnProximo.BorderSize = 0;
      this.btnProximo.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnProximo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnProximo.Image = ((System.Drawing.Image)(resources.GetObject("btnProximo.Image")));
      this.btnProximo.Location = new System.Drawing.Point(105, 5);
      this.btnProximo.Margin = new System.Windows.Forms.Padding(1);
      this.btnProximo.Name = "btnProximo";
      this.btnProximo.Size = new System.Drawing.Size(31, 31);
      this.btnProximo.TabIndex = 6;
      this.btnProximo.Tag = "Avançar";
      this.btnProximo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnProximo, "Próxima peça");
      this.btnProximo.UseVisualStyleBackColor = false;
      this.btnProximo.Click += new System.EventHandler(this.BtnProximo_Click);
      // 
      // btnVoltar
      // 
      this.btnVoltar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnVoltar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnVoltar.BorderRadius = 15;
      this.btnVoltar.BorderSize = 0;
      this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnVoltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnVoltar.Image = ((System.Drawing.Image)(resources.GetObject("btnVoltar.Image")));
      this.btnVoltar.Location = new System.Drawing.Point(72, 5);
      this.btnVoltar.Margin = new System.Windows.Forms.Padding(1);
      this.btnVoltar.Name = "btnVoltar";
      this.btnVoltar.Size = new System.Drawing.Size(31, 31);
      this.btnVoltar.TabIndex = 5;
      this.btnVoltar.Tag = "Voltar";
      this.btnVoltar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnVoltar, "Peça anterior");
      this.btnVoltar.UseVisualStyleBackColor = false;
      this.btnVoltar.Click += new System.EventHandler(this.BtnVoltar_Click);
      // 
      // btnSalvar
      // 
      this.btnSalvar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnSalvar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnSalvar.BorderRadius = 15;
      this.btnSalvar.BorderSize = 0;
      this.btnSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSalvar.Image = ((System.Drawing.Image)(resources.GetObject("btnSalvar.Image")));
      this.btnSalvar.Location = new System.Drawing.Point(36, 5);
      this.btnSalvar.Margin = new System.Windows.Forms.Padding(1);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(31, 31);
      this.btnSalvar.TabIndex = 4;
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnSalvar, "Salvar Processos");
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
      // 
      // btnCarrProcess
      // 
      this.btnCarrProcess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnCarrProcess.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnCarrProcess.BorderRadius = 15;
      this.btnCarrProcess.BorderSize = 0;
      this.btnCarrProcess.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCarrProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCarrProcess.Image = ((System.Drawing.Image)(resources.GetObject("btnCarrProcess.Image")));
      this.btnCarrProcess.Location = new System.Drawing.Point(3, 5);
      this.btnCarrProcess.Margin = new System.Windows.Forms.Padding(1);
      this.btnCarrProcess.Name = "btnCarrProcess";
      this.btnCarrProcess.Size = new System.Drawing.Size(31, 31);
      this.btnCarrProcess.TabIndex = 0;
      this.cmxToolTip1.SetToolTip(this.btnCarrProcess, "Carregar componentes\r\npara inserir processos");
      this.btnCarrProcess.UseVisualStyleBackColor = false;
      this.btnCarrProcess.Click += new System.EventHandler(this.BtnCarrProcess_Click);
      // 
      // lblPecasProc
      // 
      this.lblPecasProc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblPecasProc.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblPecasProc.Location = new System.Drawing.Point(140, 13);
      this.lblPecasProc.Margin = new System.Windows.Forms.Padding(3);
      this.lblPecasProc.Name = "lblPecasProc";
      this.lblPecasProc.Size = new System.Drawing.Size(194, 15);
      this.lblPecasProc.TabIndex = 12;
      this.lblPecasProc.Text = "Peça 0 de 0 - %";
      this.lblPecasProc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // ckbAddDenom
      // 
      this.ckbAddDenom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ckbAddDenom.AutoSize = true;
      this.ckbAddDenom.BackColor = System.Drawing.Color.Transparent;
      this.ckbAddDenom.Checked = true;
      this.ckbAddDenom.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckbAddDenom.Location = new System.Drawing.Point(177, 222);
      this.ckbAddDenom.Name = "ckbAddDenom";
      this.ckbAddDenom.Propriedade = null;
      this.ckbAddDenom.Size = new System.Drawing.Size(156, 19);
      this.ckbAddDenom.TabIndex = 25;
      this.ckbAddDenom.Text = "Add em Todas Config";
      this.cmxToolTip1.SetToolTip(this.ckbAddDenom, "Adicionar descrição em todas as configurações");
      this.ckbAddDenom.UseSelectable = true;
      this.ckbAddDenom.CheckedChanged += new System.EventHandler(this.CkbAddDenom_CheckedChanged);
      // 
      // pnlDados
      // 
      this.pnlDados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.pnlDados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pnlDados.Controls.Add(this.lmLabel1);
      this.pnlDados.Controls.Add(this.lblMaterial);
      this.pnlDados.Controls.Add(this.lmLabel2);
      this.pnlDados.Controls.Add(this.txtComponente);
      this.pnlDados.Controls.Add(this.lmLabel4);
      this.pnlDados.Controls.Add(this.txtDescricao);
      this.pnlDados.Controls.Add(this.lblPeso);
      this.pnlDados.Controls.Add(this.cmxLabel5);
      this.pnlDados.Controls.Add(this.lblCodMat);
      this.pnlDados.Controls.Add(this.txtMaterial);
      this.pnlDados.Controls.Add(this.lmLabel5);
      this.pnlDados.Controls.Add(this.lmLabel6);
      this.pnlDados.Controls.Add(this.cmxLabel2);
      this.pnlDados.Controls.Add(this.lblDescMat);
      this.pnlDados.Controls.Add(this.lblProcess);
      this.pnlDados.Controls.Add(this.lblEspess);
      this.pnlDados.Controls.Add(this.lmLabel8);
      this.pnlDados.Controls.Add(this.ckbAddDenom);
      this.pnlDados.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlDados.IsPanelMenu = false;
      this.pnlDados.Location = new System.Drawing.Point(0, 30);
      this.pnlDados.Name = "pnlDados";
      this.pnlDados.Size = new System.Drawing.Size(340, 278);
      this.pnlDados.TabIndex = 99;
      // 
      // lmLabel1
      // 
      this.lmLabel1.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel1.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel1.ForeColor = System.Drawing.Color.Red;
      this.lmLabel1.Location = new System.Drawing.Point(2, 3);
      this.lmLabel1.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel1.Name = "lmLabel1";
      this.lmLabel1.Size = new System.Drawing.Size(119, 15);
      this.lmLabel1.TabIndex = 65;
      this.lmLabel1.Text = "Dimensão:";
      this.lmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblMaterial
      // 
      this.lblMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblMaterial.BackColor = System.Drawing.Color.Transparent;
      this.lblMaterial.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblMaterial.ForeColor = System.Drawing.Color.Red;
      this.lblMaterial.Location = new System.Drawing.Point(123, 24);
      this.lblMaterial.Margin = new System.Windows.Forms.Padding(3);
      this.lblMaterial.Name = "lblMaterial";
      this.lblMaterial.Size = new System.Drawing.Size(210, 15);
      this.lblMaterial.TabIndex = 74;
      this.lblMaterial.Text = "---";
      this.lblMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmLabel2
      // 
      this.lmLabel2.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel2.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel2.Location = new System.Drawing.Point(0, 223);
      this.lmLabel2.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel2.Name = "lmLabel2";
      this.lmLabel2.Size = new System.Drawing.Size(107, 15);
      this.lmLabel2.TabIndex = 24;
      this.lmLabel2.Text = "DENOMINAÇÃO";
      this.lmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmLabel4
      // 
      this.lmLabel4.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel4.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel4.ForeColor = System.Drawing.Color.Red;
      this.lmLabel4.Location = new System.Drawing.Point(2, 24);
      this.lmLabel4.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel4.Name = "lmLabel4";
      this.lmLabel4.Size = new System.Drawing.Size(119, 15);
      this.lmLabel4.TabIndex = 73;
      this.lmLabel4.Text = "Material:";
      this.lmLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtDescricao
      // 
      this.txtDescricao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDescricao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtDescricao.BorderRadius = 15;
      this.txtDescricao.BorderSize = 2;
      this.txtDescricao.F7ToolTipText = null;
      this.txtDescricao.F8ToolTipText = null;
      this.txtDescricao.F9ToolTipText = null;
      this.txtDescricao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtDescricao.IconF7 = null;
      this.txtDescricao.IconToolTipText = null;
      this.txtDescricao.Lines = new string[0];
      this.txtDescricao.Location = new System.Drawing.Point(0, 239);
      this.txtDescricao.MaxLength = 50;
      this.txtDescricao.Name = "txtDescricao";
      this.txtDescricao.PasswordChar = '\0';
      this.txtDescricao.Propriedade = null;
      this.txtDescricao.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtDescricao.SelectedText = "";
      this.txtDescricao.SelectionLength = 0;
      this.txtDescricao.SelectionStart = 0;
      this.txtDescricao.ShortcutsEnabled = true;
      this.txtDescricao.Size = new System.Drawing.Size(333, 31);
      this.txtDescricao.TabIndex = 5;
      this.txtDescricao.UnderlinedStyle = false;
      this.txtDescricao.UseSelectable = true;
      this.txtDescricao.Valor_Decimais = ((short)(0));
      this.txtDescricao.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtDescricao.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtDescricao.Leave += new System.EventHandler(this.TxtDenominacao_Leave);
      // 
      // lblPeso
      // 
      this.lblPeso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblPeso.BackColor = System.Drawing.Color.Transparent;
      this.lblPeso.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblPeso.ForeColor = System.Drawing.Color.Red;
      this.lblPeso.Location = new System.Drawing.Point(123, 87);
      this.lblPeso.Margin = new System.Windows.Forms.Padding(3);
      this.lblPeso.Name = "lblPeso";
      this.lblPeso.Size = new System.Drawing.Size(210, 15);
      this.lblPeso.TabIndex = 72;
      this.lblPeso.Text = "---";
      this.lblPeso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // txtMaterial
      // 
      this.txtMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtMaterial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtMaterial.BorderRadius = 15;
      this.txtMaterial.BorderSize = 2;
      this.txtMaterial.F7ToolTipText = "Selecionar Material Chapa Metálica";
      this.txtMaterial.F8ToolTipText = null;
      this.txtMaterial.F9ToolTipText = null;
      this.txtMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtMaterial.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtMaterial.IconF7")));
      this.txtMaterial.IconToolTipText = null;
      this.txtMaterial.Lines = new string[0];
      this.txtMaterial.Location = new System.Drawing.Point(0, 133);
      this.txtMaterial.MaxLength = 50;
      this.txtMaterial.Name = "txtMaterial";
      this.txtMaterial.PasswordChar = '\0';
      this.txtMaterial.Propriedade = null;
      this.txtMaterial.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtMaterial.SelectedText = "";
      this.txtMaterial.SelectionLength = 0;
      this.txtMaterial.SelectionStart = 0;
      this.txtMaterial.ShortcutsEnabled = true;
      this.txtMaterial.ShowButtonF7 = true;
      this.txtMaterial.Size = new System.Drawing.Size(333, 31);
      this.txtMaterial.TabIndex = 0;
      this.txtMaterial.UnderlinedStyle = false;
      this.txtMaterial.UseSelectable = true;
      this.txtMaterial.Valor = LmCorbieUI.Design.LmValueType.ComboBox;
      this.txtMaterial.Valor_Decimais = ((short)(0));
      this.txtMaterial.WaterMark = "Selecionar Material";
      this.txtMaterial.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtMaterial.WaterMarkFont = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      // 
      // lmLabel5
      // 
      this.lmLabel5.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel5.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel5.ForeColor = System.Drawing.Color.Red;
      this.lmLabel5.Location = new System.Drawing.Point(2, 87);
      this.lmLabel5.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel5.Name = "lmLabel5";
      this.lmLabel5.Size = new System.Drawing.Size(119, 15);
      this.lmLabel5.TabIndex = 71;
      this.lmLabel5.Text = "Peso:";
      this.lmLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lmLabel6
      // 
      this.lmLabel6.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel6.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel6.Location = new System.Drawing.Point(2, 108);
      this.lmLabel6.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel6.Name = "lmLabel6";
      this.lmLabel6.Size = new System.Drawing.Size(119, 15);
      this.lmLabel6.TabIndex = 26;
      this.lmLabel6.Text = "Processos:";
      this.lmLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblDescMat
      // 
      this.lblDescMat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblDescMat.BackColor = System.Drawing.Color.Transparent;
      this.lblDescMat.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblDescMat.ForeColor = System.Drawing.Color.Red;
      this.lblDescMat.Location = new System.Drawing.Point(123, 66);
      this.lblDescMat.Margin = new System.Windows.Forms.Padding(3);
      this.lblDescMat.Name = "lblDescMat";
      this.lblDescMat.Size = new System.Drawing.Size(210, 15);
      this.lblDescMat.TabIndex = 70;
      this.lblDescMat.Text = "---";
      this.lblDescMat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lblProcess
      // 
      this.lblProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblProcess.BackColor = System.Drawing.Color.Transparent;
      this.lblProcess.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblProcess.ForeColor = System.Drawing.Color.Red;
      this.lblProcess.Location = new System.Drawing.Point(123, 108);
      this.lblProcess.Margin = new System.Windows.Forms.Padding(3);
      this.lblProcess.Name = "lblProcess";
      this.lblProcess.Size = new System.Drawing.Size(210, 15);
      this.lblProcess.TabIndex = 27;
      this.lblProcess.Text = "---";
      this.lblProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lblEspess
      // 
      this.lblEspess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblEspess.BackColor = System.Drawing.Color.Transparent;
      this.lblEspess.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblEspess.ForeColor = System.Drawing.Color.Red;
      this.lblEspess.Location = new System.Drawing.Point(123, 3);
      this.lblEspess.Margin = new System.Windows.Forms.Padding(3);
      this.lblEspess.Name = "lblEspess";
      this.lblEspess.Size = new System.Drawing.Size(210, 15);
      this.lblEspess.TabIndex = 68;
      this.lblEspess.Text = "---";
      this.lblEspess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmLabel8
      // 
      this.lmLabel8.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel8.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel8.ForeColor = System.Drawing.Color.Red;
      this.lmLabel8.Location = new System.Drawing.Point(2, 66);
      this.lmLabel8.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel8.Name = "lmLabel8";
      this.lmLabel8.Size = new System.Drawing.Size(119, 15);
      this.lmLabel8.TabIndex = 67;
      this.lmLabel8.Text = "Descrição Material:";
      this.lmLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tbcOperacoes
      // 
      this.tbcOperacoes.Controls.Add(this.tbpOperacoes);
      this.tbcOperacoes.Controls.Add(this.tbpLista);
      this.tbcOperacoes.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbcOperacoes.Location = new System.Drawing.Point(0, 351);
      this.tbcOperacoes.Name = "tbcOperacoes";
      this.tbcOperacoes.SelectedIndex = 0;
      this.tbcOperacoes.Size = new System.Drawing.Size(340, 241);
      this.tbcOperacoes.TabIndex = 100;
      this.tbcOperacoes.UseSelectable = true;
      // 
      // tbpOperacoes
      // 
      this.tbpOperacoes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpOperacoes.Controls.Add(this.flpOperacoes);
      this.tbpOperacoes.Controls.Add(this.txtProcurar);
      this.tbpOperacoes.Location = new System.Drawing.Point(4, 38);
      this.tbpOperacoes.Name = "tbpOperacoes";
      this.tbpOperacoes.Padding = new System.Windows.Forms.Padding(3, 9, 3, 3);
      this.tbpOperacoes.Size = new System.Drawing.Size(332, 199);
      this.tbpOperacoes.TabIndex = 0;
      this.tbpOperacoes.Text = "Operações";
      // 
      // flpOperacoes
      // 
      this.flpOperacoes.AutoScroll = true;
      this.flpOperacoes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.flpOperacoes.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flpOperacoes.Location = new System.Drawing.Point(3, 39);
      this.flpOperacoes.Name = "flpOperacoes";
      this.flpOperacoes.Padding = new System.Windows.Forms.Padding(0, 5, 0, 9);
      this.flpOperacoes.Size = new System.Drawing.Size(324, 155);
      this.flpOperacoes.TabIndex = 6;
      this.flpOperacoes.SizeChanged += new System.EventHandler(this.FlpProcess_SizeChanged);
      // 
      // txtProcurar
      // 
      this.txtProcurar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtProcurar.BorderRadius = 15;
      this.txtProcurar.BorderSize = 2;
      this.txtProcurar.Dock = System.Windows.Forms.DockStyle.Top;
      this.txtProcurar.F7ToolTipText = null;
      this.txtProcurar.F8ToolTipText = null;
      this.txtProcurar.F9ToolTipText = null;
      this.txtProcurar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtProcurar.Icon = ((System.Drawing.Image)(resources.GetObject("txtProcurar.Icon")));
      this.txtProcurar.IconF7 = null;
      this.txtProcurar.IconToolTipText = null;
      this.txtProcurar.Lines = new string[0];
      this.txtProcurar.Location = new System.Drawing.Point(3, 9);
      this.txtProcurar.MaxLength = 30;
      this.txtProcurar.Name = "txtProcurar";
      this.txtProcurar.PasswordChar = '\0';
      this.txtProcurar.Propriedade = null;
      this.txtProcurar.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtProcurar.SelectedText = "";
      this.txtProcurar.SelectionLength = 0;
      this.txtProcurar.SelectionStart = 0;
      this.txtProcurar.ShortcutsEnabled = true;
      this.txtProcurar.ShowClearButton = true;
      this.txtProcurar.ShowIcon = true;
      this.txtProcurar.Size = new System.Drawing.Size(324, 30);
      this.txtProcurar.TabIndex = 5;
      this.txtProcurar.UnderlinedStyle = false;
      this.txtProcurar.UseSelectable = true;
      this.txtProcurar.Valor_Decimais = ((short)(4));
      this.txtProcurar.WaterMark = "Procurar Por...";
      this.txtProcurar.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtProcurar.WaterMarkFont = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtProcurar.TextChanged += new System.EventHandler(this.TxtProcurar_TextChanged);
      // 
      // tbpLista
      // 
      this.tbpLista.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpLista.Controls.Add(this.dgv);
      this.tbpLista.Location = new System.Drawing.Point(4, 38);
      this.tbpLista.Name = "tbpLista";
      this.tbpLista.Padding = new System.Windows.Forms.Padding(3, 9, 3, 3);
      this.tbpLista.Size = new System.Drawing.Size(332, 199);
      this.tbpLista.TabIndex = 1;
      this.tbpLista.Text = "Lista Componentes";
      // 
      // dgv
      // 
      this.dgv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(222)))), ((int)(((byte)(251)))));
      this.dgv.Botao1Largura = 100;
      this.dgv.Botao1Texto = "";
      this.dgv.Botao2Largura = 100;
      this.dgv.Botao2Texto = "";
      this.dgv.ColunaOrdenacaoGrid = "";
      this.dgv.ColunasBloqueadasGrid = "";
      this.dgv.ColunasOcultasGrid = "";
      this.dgv.ColunasOcultasImpressGrid = "";
      this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgv.EnabledCsvButton = true;
      this.dgv.EnabledFind = true;
      this.dgv.EnabledHideColumnsButton = true;
      this.dgv.EnabledPdfButton = true;
      this.dgv.EnabledRefreshButton = true;
      this.dgv.LimparSelecaoAposCarregar = false;
      this.dgv.Location = new System.Drawing.Point(3, 9);
      this.dgv.Margin = new System.Windows.Forms.Padding(0);
      this.dgv.MostrarRodapeBotoes = false;
      this.dgv.MostrarTotalizador = false;
      this.dgv.Name = "dgv";
      this.dgv.PermiteAutoDimensionarLinha = false;
      this.dgv.PermiteDimensionarColuna = false;
      this.dgv.PermiteOrdenarColunas = false;
      this.dgv.PermiteOrdenarLinhas = false;
      this.dgv.PermiteQuebrarLinhaCabecalho = false;
      this.dgv.PermiteSelecaoMultipla = false;
      this.dgv.PosColunasGrid = "";
      this.dgv.Size = new System.Drawing.Size(324, 185);
      this.dgv.TabIndex = 98;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      this.dgv.RowIndexChanged += new LmCorbieUI.Controls.LmDataGridView.RowEvent(this.Dgv_RowIndexChanged);
      // 
      // tmrInicioLocalizar
      // 
      this.tmrInicioLocalizar.Tag = "0";
      this.tmrInicioLocalizar.Tick += new System.EventHandler(this.TmrInicioLocalizar_Tick);
      // 
      // FrmMateriaPrimaApl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(340, 592);
      this.Controls.Add(this.tbcOperacoes);
      this.Controls.Add(this.pnlControl);
      this.Controls.Add(this.pnlDados);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Movimentar = false;
      this.Name = "FrmMateriaPrimaApl";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.Resizable = false;
      this.Text = "Aplicação de Matéria Prima";
      this.Loaded += new LmCorbieUI.LmForms.LmSingleForm.FormLoad(this.FrmProcesso_Loaded);
      this.pnlControl.ResumeLayout(false);
      this.pnlDados.ResumeLayout(false);
      this.pnlDados.PerformLayout();
      this.tbcOperacoes.ResumeLayout(false);
      this.tbpOperacoes.ResumeLayout(false);
      this.tbpLista.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
        private LmCorbieUI.Controls.LmTextBox txtComponente;
        private LmCorbieUI.Controls.LmLabel cmxLabel5;
        private LmCorbieUI.Controls.LmLabel lblCodMat;
        private LmCorbieUI.Controls.LmLabel cmxLabel2;
        private LmCorbieUI.Controls.LmPanel pnlControl;
        private LmCorbieUI.Controls.LmLabel lblPecasProc;
        private LmCorbieUI.Controls.LmButton btnProximo;
        private LmCorbieUI.Controls.LmButton btnVoltar;
        private LmCorbieUI.Controls.LmButton btnSalvar;
        private LmCorbieUI.Controls.LmButton btnCarrProcess;
        private System.Windows.Forms.ToolTip cmxToolTip1;
    private LmCorbieUI.Controls.LmPanel pnlDados;
    private LmCorbieUI.Controls.LmLabel lmLabel1;
    private LmCorbieUI.Controls.LmLabel lblMaterial;
    private LmCorbieUI.Controls.LmLabel lmLabel2;
    private LmCorbieUI.Controls.LmLabel lmLabel4;
    private LmCorbieUI.Controls.LmTextBox txtDescricao;
    private LmCorbieUI.Controls.LmLabel lblPeso;
    private LmCorbieUI.Controls.LmTextBox txtMaterial;
    private LmCorbieUI.Controls.LmLabel lmLabel5;
    private LmCorbieUI.Controls.LmLabel lmLabel6;
    private LmCorbieUI.Controls.LmLabel lblDescMat;
    private LmCorbieUI.Controls.LmLabel lblProcess;
    private LmCorbieUI.Controls.LmLabel lblEspess;
    private LmCorbieUI.Controls.LmLabel lmLabel8;
    private LmCorbieUI.Controls.LmCheckBox ckbAddDenom;
    private LmCorbieUI.Controls.LmTabControl tbcOperacoes;
    private LmCorbieUI.Controls.LmTabPage tbpOperacoes;
    private LmCorbieUI.Controls.LmPanelFlow flpOperacoes;
    private LmCorbieUI.Controls.LmTextBox txtProcurar;
    private LmCorbieUI.Controls.LmTabPage tbpLista;
    private LmCorbieUI.Controls.LmDataGridView dgv;
    private System.Windows.Forms.Timer tmrInicioLocalizar;
  }
}