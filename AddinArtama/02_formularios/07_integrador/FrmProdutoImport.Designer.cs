namespace AddinArtama
{
    partial class FrmProdutoImport
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProdutoImport));
      this.lblCodMat = new LmCorbieUI.Controls.LmLabel();
      this.cmxLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.btnProximo = new LmCorbieUI.Controls.LmButton();
      this.btnVoltar = new LmCorbieUI.Controls.LmButton();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.btnCarrProcess = new LmCorbieUI.Controls.LmButton();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.btnAtualizarProcesso = new LmCorbieUI.Controls.LmButton();
      this.pnlDados = new LmCorbieUI.Controls.LmPanel();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
      this.txtDescricao = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.lmPanel3 = new LmCorbieUI.Controls.LmPanel();
      this.lmLabel7 = new LmCorbieUI.Controls.LmLabel();
      this.txtSmCompr = new LmCorbieUI.Controls.LmTextBox();
      this.lmPanel2 = new LmCorbieUI.Controls.LmPanel();
      this.lmLabel6 = new LmCorbieUI.Controls.LmLabel();
      this.txtSmLarg = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.lblPeso = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel5 = new LmCorbieUI.Controls.LmLabel();
      this.lblDescMat = new LmCorbieUI.Controls.LmLabel();
      this.lblEspess = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel8 = new LmCorbieUI.Controls.LmLabel();
      this.tbcOperacoes = new LmCorbieUI.Controls.LmTabControl();
      this.tbpLista = new LmCorbieUI.Controls.LmTabPage();
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.tbpOperacoes = new LmCorbieUI.Controls.LmTabPage();
      this.lmPanelOP = new LmCorbieUI.Controls.LmPanel();
      this.lmLabel3 = new LmCorbieUI.Controls.LmLabel();
      this.btnInserir = new LmCorbieUI.Controls.LmButton();
      this.lmLabel4 = new LmCorbieUI.Controls.LmLabel();
      this.txtMaquina = new LmCorbieUI.Controls.LmTextBox();
      this.txtOperacao = new LmCorbieUI.Controls.LmTextBox();
      this.flpOperacoes = new LmCorbieUI.Controls.LmPanelFlow();
      this.tbpEngenharia = new LmCorbieUI.Controls.LmTabPage();
      this.trvProduto = new System.Windows.Forms.TreeView();
      this.pnlDados.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.lmPanel1.SuspendLayout();
      this.lmPanel3.SuspendLayout();
      this.lmPanel2.SuspendLayout();
      this.tbcOperacoes.SuspendLayout();
      this.tbpLista.SuspendLayout();
      this.tbpOperacoes.SuspendLayout();
      this.lmPanelOP.SuspendLayout();
      this.tbpEngenharia.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblCodMat
      // 
      this.lblCodMat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblCodMat.BackColor = System.Drawing.Color.Transparent;
      this.lblCodMat.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblCodMat.ForeColor = System.Drawing.Color.Red;
      this.lblCodMat.Location = new System.Drawing.Point(118, 24);
      this.lblCodMat.Margin = new System.Windows.Forms.Padding(3);
      this.lblCodMat.Name = "lblCodMat";
      this.lblCodMat.Size = new System.Drawing.Size(177, 15);
      this.lblCodMat.TabIndex = 18;
      this.lblCodMat.Text = "---";
      this.lblCodMat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // cmxLabel2
      // 
      this.cmxLabel2.BackColor = System.Drawing.Color.Transparent;
      this.cmxLabel2.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.cmxLabel2.Location = new System.Drawing.Point(12, 24);
      this.cmxLabel2.Margin = new System.Windows.Forms.Padding(3);
      this.cmxLabel2.Name = "cmxLabel2";
      this.cmxLabel2.Size = new System.Drawing.Size(104, 15);
      this.cmxLabel2.TabIndex = 15;
      this.cmxLabel2.Text = "Código Material:";
      this.cmxLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // btnProximo
      // 
      this.btnProximo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnProximo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnProximo.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnProximo.BorderRadius = 13;
      this.btnProximo.BorderSize = 0;
      this.btnProximo.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnProximo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnProximo.Image = ((System.Drawing.Image)(resources.GetObject("btnProximo.Image")));
      this.btnProximo.Location = new System.Drawing.Point(237, 126);
      this.btnProximo.Margin = new System.Windows.Forms.Padding(1);
      this.btnProximo.Name = "btnProximo";
      this.btnProximo.Size = new System.Drawing.Size(26, 26);
      this.btnProximo.TabIndex = 4;
      this.btnProximo.Tag = "Avançar";
      this.btnProximo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnProximo, "Próxima peça");
      this.btnProximo.UseVisualStyleBackColor = false;
      this.btnProximo.Click += new System.EventHandler(this.BtnProximo_Click);
      // 
      // btnVoltar
      // 
      this.btnVoltar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnVoltar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnVoltar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnVoltar.BorderRadius = 13;
      this.btnVoltar.BorderSize = 0;
      this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnVoltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnVoltar.Image = ((System.Drawing.Image)(resources.GetObject("btnVoltar.Image")));
      this.btnVoltar.Location = new System.Drawing.Point(209, 126);
      this.btnVoltar.Margin = new System.Windows.Forms.Padding(1);
      this.btnVoltar.Name = "btnVoltar";
      this.btnVoltar.Size = new System.Drawing.Size(26, 26);
      this.btnVoltar.TabIndex = 3;
      this.btnVoltar.Tag = "Voltar";
      this.btnVoltar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnVoltar, "Peça anterior");
      this.btnVoltar.UseVisualStyleBackColor = false;
      this.btnVoltar.Click += new System.EventHandler(this.BtnVoltar_Click);
      // 
      // btnSalvar
      // 
      this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSalvar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnSalvar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnSalvar.BorderRadius = 13;
      this.btnSalvar.BorderSize = 0;
      this.btnSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSalvar.Image = ((System.Drawing.Image)(resources.GetObject("btnSalvar.Image")));
      this.btnSalvar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnSalvar.Location = new System.Drawing.Point(154, 113);
      this.btnSalvar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(146, 26);
      this.btnSalvar.TabIndex = 4;
      this.btnSalvar.Text = " Salvar";
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnSalvar, "Salvar/Atualizar Produto e Engenharia");
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
      // 
      // btnCarrProcess
      // 
      this.btnCarrProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCarrProcess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnCarrProcess.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnCarrProcess.BorderRadius = 13;
      this.btnCarrProcess.BorderSize = 0;
      this.btnCarrProcess.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCarrProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCarrProcess.Image = ((System.Drawing.Image)(resources.GetObject("btnCarrProcess.Image")));
      this.btnCarrProcess.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnCarrProcess.Location = new System.Drawing.Point(3, 113);
      this.btnCarrProcess.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.btnCarrProcess.Name = "btnCarrProcess";
      this.btnCarrProcess.Size = new System.Drawing.Size(145, 26);
      this.btnCarrProcess.TabIndex = 3;
      this.btnCarrProcess.Text = " Carregar";
      this.btnCarrProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnCarrProcess, "Carregar componentes");
      this.btnCarrProcess.UseVisualStyleBackColor = false;
      this.btnCarrProcess.Click += new System.EventHandler(this.BtnCarrProcess_Click);
      // 
      // btnAtualizarProcesso
      // 
      this.btnAtualizarProcesso.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAtualizarProcesso.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnAtualizarProcesso.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnAtualizarProcesso.BorderRadius = 13;
      this.btnAtualizarProcesso.BorderSize = 0;
      this.btnAtualizarProcesso.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnAtualizarProcesso.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnAtualizarProcesso.Image = ((System.Drawing.Image)(resources.GetObject("btnAtualizarProcesso.Image")));
      this.btnAtualizarProcesso.Location = new System.Drawing.Point(265, 126);
      this.btnAtualizarProcesso.Margin = new System.Windows.Forms.Padding(1);
      this.btnAtualizarProcesso.Name = "btnAtualizarProcesso";
      this.btnAtualizarProcesso.Size = new System.Drawing.Size(26, 26);
      this.btnAtualizarProcesso.TabIndex = 5;
      this.btnAtualizarProcesso.Tag = "";
      this.btnAtualizarProcesso.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnAtualizarProcesso, "Atualizar Processos");
      this.btnAtualizarProcesso.UseVisualStyleBackColor = false;
      this.btnAtualizarProcesso.Click += new System.EventHandler(this.BtnAtualizarProcesso_Click);
      // 
      // pnlDados
      // 
      this.pnlDados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.pnlDados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pnlDados.Controls.Add(this.tableLayoutPanel1);
      this.pnlDados.Controls.Add(this.lmLabel1);
      this.pnlDados.Controls.Add(this.lblPeso);
      this.pnlDados.Controls.Add(this.lblCodMat);
      this.pnlDados.Controls.Add(this.lmLabel5);
      this.pnlDados.Controls.Add(this.cmxLabel2);
      this.pnlDados.Controls.Add(this.lblDescMat);
      this.pnlDados.Controls.Add(this.lblEspess);
      this.pnlDados.Controls.Add(this.lmLabel8);
      this.pnlDados.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlDados.IsPanelMenu = false;
      this.pnlDados.Location = new System.Drawing.Point(0, 30);
      this.pnlDados.Name = "pnlDados";
      this.pnlDados.Size = new System.Drawing.Size(305, 231);
      this.pnlDados.TabIndex = 0;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.btnSalvar, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.lmPanel1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.btnCarrProcess, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.lmPanel3, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.lmPanel2, 0, 1);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 87);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(303, 140);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // lmPanel1
      // 
      this.lmPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lmPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.tableLayoutPanel1.SetColumnSpan(this.lmPanel1, 2);
      this.lmPanel1.Controls.Add(this.txtDescricao);
      this.lmPanel1.Controls.Add(this.lmLabel2);
      this.lmPanel1.IsPanelMenu = false;
      this.lmPanel1.Location = new System.Drawing.Point(0, 0);
      this.lmPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.lmPanel1.Name = "lmPanel1";
      this.lmPanel1.Size = new System.Drawing.Size(303, 55);
      this.lmPanel1.TabIndex = 0;
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
      this.txtDescricao.Location = new System.Drawing.Point(3, 21);
      this.txtDescricao.MaxLength = 50;
      this.txtDescricao.Name = "txtDescricao";
      this.txtDescricao.PasswordChar = '\0';
      this.txtDescricao.Propriedade = null;
      this.txtDescricao.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtDescricao.SelectedText = "";
      this.txtDescricao.SelectionLength = 0;
      this.txtDescricao.SelectionStart = 0;
      this.txtDescricao.ShortcutsEnabled = true;
      this.txtDescricao.Size = new System.Drawing.Size(297, 31);
      this.txtDescricao.TabIndex = 0;
      this.txtDescricao.UnderlinedStyle = false;
      this.txtDescricao.UseSelectable = true;
      this.txtDescricao.Valor_Decimais = ((short)(0));
      this.txtDescricao.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtDescricao.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtDescricao.Leave += new System.EventHandler(this.TxtDenominacao_Leave);
      // 
      // lmLabel2
      // 
      this.lmLabel2.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel2.Location = new System.Drawing.Point(3, 0);
      this.lmLabel2.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel2.Name = "lmLabel2";
      this.lmLabel2.Size = new System.Drawing.Size(113, 21);
      this.lmLabel2.TabIndex = 24;
      this.lmLabel2.Text = "Denominação";
      this.lmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmPanel3
      // 
      this.lmPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lmPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel3.Controls.Add(this.lmLabel7);
      this.lmPanel3.Controls.Add(this.txtSmCompr);
      this.lmPanel3.IsPanelMenu = false;
      this.lmPanel3.Location = new System.Drawing.Point(151, 55);
      this.lmPanel3.Margin = new System.Windows.Forms.Padding(0);
      this.lmPanel3.Name = "lmPanel3";
      this.lmPanel3.Size = new System.Drawing.Size(152, 55);
      this.lmPanel3.TabIndex = 2;
      // 
      // lmLabel7
      // 
      this.lmLabel7.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel7.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel7.Location = new System.Drawing.Point(3, 0);
      this.lmLabel7.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel7.Name = "lmLabel7";
      this.lmLabel7.Size = new System.Drawing.Size(107, 21);
      this.lmLabel7.TabIndex = 76;
      this.lmLabel7.Text = "SM Comprimento";
      this.lmLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // txtSmCompr
      // 
      this.txtSmCompr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSmCompr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtSmCompr.BorderRadius = 15;
      this.txtSmCompr.BorderSize = 2;
      this.txtSmCompr.F7ToolTipText = null;
      this.txtSmCompr.F8ToolTipText = null;
      this.txtSmCompr.F9ToolTipText = null;
      this.txtSmCompr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtSmCompr.IconF7 = null;
      this.txtSmCompr.IconToolTipText = null;
      this.txtSmCompr.Lines = new string[0];
      this.txtSmCompr.Location = new System.Drawing.Point(3, 21);
      this.txtSmCompr.MaxLength = 50;
      this.txtSmCompr.Name = "txtSmCompr";
      this.txtSmCompr.PasswordChar = '\0';
      this.txtSmCompr.Propriedade = null;
      this.txtSmCompr.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtSmCompr.SelectedText = "";
      this.txtSmCompr.SelectionLength = 0;
      this.txtSmCompr.SelectionStart = 0;
      this.txtSmCompr.ShortcutsEnabled = true;
      this.txtSmCompr.ShowClearButton = true;
      this.txtSmCompr.Size = new System.Drawing.Size(146, 31);
      this.txtSmCompr.TabIndex = 0;
      this.txtSmCompr.UnderlinedStyle = false;
      this.txtSmCompr.UseSelectable = true;
      this.txtSmCompr.Valor = LmCorbieUI.Design.LmValueType.Num_Inteiro;
      this.txtSmCompr.Valor_Decimais = ((short)(0));
      this.txtSmCompr.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtSmCompr.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtSmCompr.Leave += new System.EventHandler(this.TxtSmCompr_Leave);
      // 
      // lmPanel2
      // 
      this.lmPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lmPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel2.Controls.Add(this.lmLabel6);
      this.lmPanel2.Controls.Add(this.txtSmLarg);
      this.lmPanel2.IsPanelMenu = false;
      this.lmPanel2.Location = new System.Drawing.Point(0, 55);
      this.lmPanel2.Margin = new System.Windows.Forms.Padding(0);
      this.lmPanel2.Name = "lmPanel2";
      this.lmPanel2.Size = new System.Drawing.Size(151, 55);
      this.lmPanel2.TabIndex = 1;
      // 
      // lmLabel6
      // 
      this.lmLabel6.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel6.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel6.Location = new System.Drawing.Point(3, 0);
      this.lmLabel6.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel6.Name = "lmLabel6";
      this.lmLabel6.Size = new System.Drawing.Size(107, 21);
      this.lmLabel6.TabIndex = 74;
      this.lmLabel6.Text = "SM Largura";
      this.lmLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // txtSmLarg
      // 
      this.txtSmLarg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSmLarg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtSmLarg.BorderRadius = 15;
      this.txtSmLarg.BorderSize = 2;
      this.txtSmLarg.F7ToolTipText = null;
      this.txtSmLarg.F8ToolTipText = null;
      this.txtSmLarg.F9ToolTipText = null;
      this.txtSmLarg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtSmLarg.IconF7 = null;
      this.txtSmLarg.IconToolTipText = null;
      this.txtSmLarg.Lines = new string[0];
      this.txtSmLarg.Location = new System.Drawing.Point(3, 21);
      this.txtSmLarg.MaxLength = 50;
      this.txtSmLarg.Name = "txtSmLarg";
      this.txtSmLarg.PasswordChar = '\0';
      this.txtSmLarg.Propriedade = null;
      this.txtSmLarg.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtSmLarg.SelectedText = "";
      this.txtSmLarg.SelectionLength = 0;
      this.txtSmLarg.SelectionStart = 0;
      this.txtSmLarg.ShortcutsEnabled = true;
      this.txtSmLarg.ShowClearButton = true;
      this.txtSmLarg.Size = new System.Drawing.Size(145, 31);
      this.txtSmLarg.TabIndex = 0;
      this.txtSmLarg.UnderlinedStyle = false;
      this.txtSmLarg.UseSelectable = true;
      this.txtSmLarg.Valor = LmCorbieUI.Design.LmValueType.Num_Inteiro;
      this.txtSmLarg.Valor_Decimais = ((short)(0));
      this.txtSmLarg.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtSmLarg.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtSmLarg.Leave += new System.EventHandler(this.TxtSmLarg_Leave);
      // 
      // lmLabel1
      // 
      this.lmLabel1.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel1.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel1.ForeColor = System.Drawing.Color.Red;
      this.lmLabel1.Location = new System.Drawing.Point(3, 3);
      this.lmLabel1.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel1.Name = "lmLabel1";
      this.lmLabel1.Size = new System.Drawing.Size(113, 15);
      this.lmLabel1.TabIndex = 65;
      this.lmLabel1.Text = "Dimensão:";
      this.lmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblPeso
      // 
      this.lblPeso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblPeso.BackColor = System.Drawing.Color.Transparent;
      this.lblPeso.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblPeso.ForeColor = System.Drawing.Color.Red;
      this.lblPeso.Location = new System.Drawing.Point(118, 66);
      this.lblPeso.Margin = new System.Windows.Forms.Padding(3);
      this.lblPeso.Name = "lblPeso";
      this.lblPeso.Size = new System.Drawing.Size(177, 15);
      this.lblPeso.TabIndex = 72;
      this.lblPeso.Text = "---";
      this.lblPeso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmLabel5
      // 
      this.lmLabel5.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel5.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel5.ForeColor = System.Drawing.Color.Red;
      this.lmLabel5.Location = new System.Drawing.Point(3, 66);
      this.lmLabel5.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel5.Name = "lmLabel5";
      this.lmLabel5.Size = new System.Drawing.Size(113, 15);
      this.lmLabel5.TabIndex = 71;
      this.lmLabel5.Text = "Peso:";
      this.lmLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblDescMat
      // 
      this.lblDescMat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblDescMat.BackColor = System.Drawing.Color.Transparent;
      this.lblDescMat.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblDescMat.ForeColor = System.Drawing.Color.Red;
      this.lblDescMat.Location = new System.Drawing.Point(118, 45);
      this.lblDescMat.Margin = new System.Windows.Forms.Padding(3);
      this.lblDescMat.Name = "lblDescMat";
      this.lblDescMat.Size = new System.Drawing.Size(177, 15);
      this.lblDescMat.TabIndex = 70;
      this.lblDescMat.Text = "---";
      this.lblDescMat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lblEspess
      // 
      this.lblEspess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblEspess.BackColor = System.Drawing.Color.Transparent;
      this.lblEspess.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblEspess.ForeColor = System.Drawing.Color.Red;
      this.lblEspess.Location = new System.Drawing.Point(118, 3);
      this.lblEspess.Margin = new System.Windows.Forms.Padding(3);
      this.lblEspess.Name = "lblEspess";
      this.lblEspess.Size = new System.Drawing.Size(177, 15);
      this.lblEspess.TabIndex = 68;
      this.lblEspess.Text = "---";
      this.lblEspess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmLabel8
      // 
      this.lmLabel8.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel8.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel8.ForeColor = System.Drawing.Color.Red;
      this.lmLabel8.Location = new System.Drawing.Point(3, 45);
      this.lmLabel8.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel8.Name = "lmLabel8";
      this.lmLabel8.Size = new System.Drawing.Size(113, 15);
      this.lmLabel8.TabIndex = 67;
      this.lmLabel8.Text = "Descrição Material:";
      this.lmLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tbcOperacoes
      // 
      this.tbcOperacoes.Controls.Add(this.tbpLista);
      this.tbcOperacoes.Controls.Add(this.tbpOperacoes);
      this.tbcOperacoes.Controls.Add(this.tbpEngenharia);
      this.tbcOperacoes.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbcOperacoes.Location = new System.Drawing.Point(0, 261);
      this.tbcOperacoes.Name = "tbcOperacoes";
      this.tbcOperacoes.SelectedIndex = 1;
      this.tbcOperacoes.Size = new System.Drawing.Size(305, 331);
      this.tbcOperacoes.TabIndex = 1;
      this.tbcOperacoes.UseSelectable = true;
      // 
      // tbpLista
      // 
      this.tbpLista.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpLista.Controls.Add(this.dgv);
      this.tbpLista.Location = new System.Drawing.Point(4, 38);
      this.tbpLista.Name = "tbpLista";
      this.tbpLista.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
      this.tbpLista.Size = new System.Drawing.Size(297, 289);
      this.tbpLista.TabIndex = 1;
      this.tbpLista.Text = "Componentes";
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
      this.dgv.EnabledCsvButton = false;
      this.dgv.EnabledFind = false;
      this.dgv.EnabledHideColumnsButton = false;
      this.dgv.EnabledPdfButton = false;
      this.dgv.EnabledRefreshButton = false;
      this.dgv.LimparSelecaoAposCarregar = false;
      this.dgv.Location = new System.Drawing.Point(0, 9);
      this.dgv.Margin = new System.Windows.Forms.Padding(0);
      this.dgv.MostrarRodapeBotoes = true;
      this.dgv.MostrarTotalizador = false;
      this.dgv.Name = "dgv";
      this.dgv.PermiteAutoDimensionarLinha = false;
      this.dgv.PermiteDimensionarColuna = false;
      this.dgv.PermiteOrdenarColunas = false;
      this.dgv.PermiteOrdenarLinhas = false;
      this.dgv.PermiteQuebrarLinhaCabecalho = false;
      this.dgv.PermiteSelecaoMultipla = false;
      this.dgv.PosColunasGrid = "";
      this.dgv.Size = new System.Drawing.Size(295, 278);
      this.dgv.TabIndex = 98;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      this.dgv.RowIndexChanged += new LmCorbieUI.Controls.LmDataGridView.RowEvent(this.Dgv_RowIndexChanged);
      // 
      // tbpOperacoes
      // 
      this.tbpOperacoes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpOperacoes.Controls.Add(this.lmPanelOP);
      this.tbpOperacoes.Controls.Add(this.flpOperacoes);
      this.tbpOperacoes.Location = new System.Drawing.Point(4, 38);
      this.tbpOperacoes.Name = "tbpOperacoes";
      this.tbpOperacoes.Padding = new System.Windows.Forms.Padding(3, 162, 3, 3);
      this.tbpOperacoes.Size = new System.Drawing.Size(297, 289);
      this.tbpOperacoes.TabIndex = 0;
      this.tbpOperacoes.Text = "Operações";
      // 
      // lmPanelOP
      // 
      this.lmPanelOP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lmPanelOP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanelOP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lmPanelOP.Controls.Add(this.btnAtualizarProcesso);
      this.lmPanelOP.Controls.Add(this.btnProximo);
      this.lmPanelOP.Controls.Add(this.btnVoltar);
      this.lmPanelOP.Controls.Add(this.lmLabel3);
      this.lmPanelOP.Controls.Add(this.btnInserir);
      this.lmPanelOP.Controls.Add(this.lmLabel4);
      this.lmPanelOP.Controls.Add(this.txtMaquina);
      this.lmPanelOP.Controls.Add(this.txtOperacao);
      this.lmPanelOP.IsPanelMenu = false;
      this.lmPanelOP.Location = new System.Drawing.Point(-1, -1);
      this.lmPanelOP.Name = "lmPanelOP";
      this.lmPanelOP.Size = new System.Drawing.Size(297, 157);
      this.lmPanelOP.TabIndex = 362;
      // 
      // lmLabel3
      // 
      this.lmLabel3.AutoSize = true;
      this.lmLabel3.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel3.Location = new System.Drawing.Point(3, 3);
      this.lmLabel3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel3.Name = "lmLabel3";
      this.lmLabel3.Size = new System.Drawing.Size(78, 19);
      this.lmLabel3.TabIndex = 360;
      this.lmLabel3.Text = "Operação *";
      this.lmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // btnInserir
      // 
      this.btnInserir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnInserir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnInserir.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnInserir.BorderRadius = 13;
      this.btnInserir.BorderSize = 0;
      this.btnInserir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnInserir.Image = ((System.Drawing.Image)(resources.GetObject("btnInserir.Image")));
      this.btnInserir.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnInserir.Location = new System.Drawing.Point(3, 126);
      this.btnInserir.Name = "btnInserir";
      this.btnInserir.Size = new System.Drawing.Size(202, 26);
      this.btnInserir.TabIndex = 2;
      this.btnInserir.Text = " Inserir Processo";
      this.btnInserir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnInserir.UseVisualStyleBackColor = false;
      this.btnInserir.Click += new System.EventHandler(this.BtnInserir_Click);
      // 
      // lmLabel4
      // 
      this.lmLabel4.AutoSize = true;
      this.lmLabel4.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel4.Location = new System.Drawing.Point(3, 62);
      this.lmLabel4.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel4.Name = "lmLabel4";
      this.lmLabel4.Size = new System.Drawing.Size(73, 19);
      this.lmLabel4.TabIndex = 359;
      this.lmLabel4.Text = "Máquina *";
      this.lmLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtMaquina
      // 
      this.txtMaquina.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtMaquina.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtMaquina.BorderRadius = 15;
      this.txtMaquina.BorderSize = 2;
      this.txtMaquina.F7ToolTipText = null;
      this.txtMaquina.F8ToolTipText = null;
      this.txtMaquina.F9ToolTipText = null;
      this.txtMaquina.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtMaquina.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtMaquina.IconF7")));
      this.txtMaquina.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtMaquina.IconF8")));
      this.txtMaquina.IconToolTipText = null;
      this.txtMaquina.Lines = new string[0];
      this.txtMaquina.Location = new System.Drawing.Point(3, 85);
      this.txtMaquina.MaxLength = 250;
      this.txtMaquina.Name = "txtMaquina";
      this.txtMaquina.PasswordChar = '\0';
      this.txtMaquina.Propriedade = null;
      this.txtMaquina.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtMaquina.SelectedText = "";
      this.txtMaquina.SelectionLength = 0;
      this.txtMaquina.SelectionStart = 0;
      this.txtMaquina.ShortcutsEnabled = true;
      this.txtMaquina.ShowButtonF7 = true;
      this.txtMaquina.ShowClearButton = true;
      this.txtMaquina.Size = new System.Drawing.Size(290, 30);
      this.txtMaquina.TabIndex = 1;
      this.txtMaquina.UnderlinedStyle = false;
      this.txtMaquina.UseSelectable = true;
      this.txtMaquina.Valor = LmCorbieUI.Design.LmValueType.ComboBox;
      this.txtMaquina.Valor_Decimais = ((short)(4));
      this.txtMaquina.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtMaquina.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // txtOperacao
      // 
      this.txtOperacao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtOperacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtOperacao.BorderRadius = 15;
      this.txtOperacao.BorderSize = 2;
      this.txtOperacao.F7ToolTipText = null;
      this.txtOperacao.F8ToolTipText = null;
      this.txtOperacao.F9ToolTipText = null;
      this.txtOperacao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtOperacao.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtOperacao.IconF7")));
      this.txtOperacao.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtOperacao.IconF8")));
      this.txtOperacao.IconToolTipText = null;
      this.txtOperacao.Lines = new string[0];
      this.txtOperacao.Location = new System.Drawing.Point(3, 26);
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
      this.txtOperacao.ShowClearButton = true;
      this.txtOperacao.Size = new System.Drawing.Size(290, 30);
      this.txtOperacao.TabIndex = 0;
      this.txtOperacao.UnderlinedStyle = false;
      this.txtOperacao.UseSelectable = true;
      this.txtOperacao.Valor = LmCorbieUI.Design.LmValueType.ComboBox;
      this.txtOperacao.Valor_Decimais = ((short)(4));
      this.txtOperacao.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtOperacao.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtOperacao.SelectedValueChanched += new LmCorbieUI.Controls.LmTextBox.ValChange(this.TxtOperacao_SelectedValueChanched);
      // 
      // flpOperacoes
      // 
      this.flpOperacoes.AllowDrop = true;
      this.flpOperacoes.AutoScroll = true;
      this.flpOperacoes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.flpOperacoes.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flpOperacoes.Location = new System.Drawing.Point(3, 162);
      this.flpOperacoes.Name = "flpOperacoes";
      this.flpOperacoes.Padding = new System.Windows.Forms.Padding(0, 5, 0, 9);
      this.flpOperacoes.Size = new System.Drawing.Size(289, 122);
      this.flpOperacoes.TabIndex = 6;
      this.flpOperacoes.SizeChanged += new System.EventHandler(this.FlpProcess_SizeChanged);
      this.flpOperacoes.DragDrop += new System.Windows.Forms.DragEventHandler(this.FlpOperacoes_DragDrop);
      this.flpOperacoes.DragEnter += new System.Windows.Forms.DragEventHandler(this.FlpOperacoes_DragEnter);
      // 
      // tbpEngenharia
      // 
      this.tbpEngenharia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpEngenharia.Controls.Add(this.trvProduto);
      this.tbpEngenharia.Location = new System.Drawing.Point(4, 38);
      this.tbpEngenharia.Name = "tbpEngenharia";
      this.tbpEngenharia.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
      this.tbpEngenharia.Size = new System.Drawing.Size(297, 289);
      this.tbpEngenharia.TabIndex = 2;
      this.tbpEngenharia.Text = "Engenharia";
      // 
      // trvProduto
      // 
      this.trvProduto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.trvProduto.Dock = System.Windows.Forms.DockStyle.Fill;
      this.trvProduto.Location = new System.Drawing.Point(0, 9);
      this.trvProduto.Name = "trvProduto";
      this.trvProduto.Size = new System.Drawing.Size(295, 278);
      this.trvProduto.TabIndex = 7;
      // 
      // FrmProdutoImport
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(305, 592);
      this.Controls.Add(this.tbcOperacoes);
      this.Controls.Add(this.pnlDados);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.KeyPreview = true;
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Movimentar = false;
      this.Name = "FrmProdutoImport";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.Resizable = false;
      this.Text = "Cadastro de Produto/Engenharia";
      this.Loaded += new LmCorbieUI.LmForms.LmSingleForm.FormLoad(this.FrmProcessoAplicacao_Loaded);
      this.Load += new System.EventHandler(this.FrmProcessoAplicacao_Load);
      this.pnlDados.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.lmPanel1.ResumeLayout(false);
      this.lmPanel3.ResumeLayout(false);
      this.lmPanel2.ResumeLayout(false);
      this.tbcOperacoes.ResumeLayout(false);
      this.tbpLista.ResumeLayout(false);
      this.tbpOperacoes.ResumeLayout(false);
      this.lmPanelOP.ResumeLayout(false);
      this.lmPanelOP.PerformLayout();
      this.tbpEngenharia.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
        private LmCorbieUI.Controls.LmLabel lblCodMat;
        private LmCorbieUI.Controls.LmLabel cmxLabel2;
        private LmCorbieUI.Controls.LmButton btnProximo;
        private LmCorbieUI.Controls.LmButton btnVoltar;
        private LmCorbieUI.Controls.LmButton btnSalvar;
        private LmCorbieUI.Controls.LmButton btnCarrProcess;
        private System.Windows.Forms.ToolTip cmxToolTip1;
    private LmCorbieUI.Controls.LmPanel pnlDados;
    private LmCorbieUI.Controls.LmLabel lmLabel1;
    private LmCorbieUI.Controls.LmLabel lmLabel2;
    private LmCorbieUI.Controls.LmTextBox txtDescricao;
    private LmCorbieUI.Controls.LmLabel lblPeso;
    private LmCorbieUI.Controls.LmLabel lmLabel5;
    private LmCorbieUI.Controls.LmLabel lblDescMat;
    private LmCorbieUI.Controls.LmLabel lblEspess;
    private LmCorbieUI.Controls.LmLabel lmLabel8;
    private LmCorbieUI.Controls.LmTabControl tbcOperacoes;
    private LmCorbieUI.Controls.LmTabPage tbpOperacoes;
    private LmCorbieUI.Controls.LmPanelFlow flpOperacoes;
    private LmCorbieUI.Controls.LmTabPage tbpLista;
    private LmCorbieUI.Controls.LmDataGridView dgv;
    private LmCorbieUI.Controls.LmTextBox txtMaquina;
    private LmCorbieUI.Controls.LmLabel lmLabel3;
    private LmCorbieUI.Controls.LmTextBox txtOperacao;
    private LmCorbieUI.Controls.LmLabel lmLabel4;
    private LmCorbieUI.Controls.LmButton btnInserir;
    private LmCorbieUI.Controls.LmPanel lmPanelOP;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private LmCorbieUI.Controls.LmPanel lmPanel1;
    private LmCorbieUI.Controls.LmPanel lmPanel3;
    private LmCorbieUI.Controls.LmPanel lmPanel2;
    private LmCorbieUI.Controls.LmLabel lmLabel7;
    private LmCorbieUI.Controls.LmTextBox txtSmCompr;
    private LmCorbieUI.Controls.LmLabel lmLabel6;
    private LmCorbieUI.Controls.LmTextBox txtSmLarg;
    private LmCorbieUI.Controls.LmTabPage tbpEngenharia;
    private System.Windows.Forms.TreeView trvProduto;
    private LmCorbieUI.Controls.LmButton btnAtualizarProcesso;
  }
}