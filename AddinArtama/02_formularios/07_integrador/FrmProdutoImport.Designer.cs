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
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.lblCodigoProduto = new LmCorbieUI.Controls.LmLabel();
      this.ptbMaterialError = new System.Windows.Forms.PictureBox();
      this.btnCarrProcess = new LmCorbieUI.Controls.LmButton();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.pnlDados = new LmCorbieUI.Controls.LmPanel();
      this.lblPesoNbr = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel9 = new LmCorbieUI.Controls.LmLabel();
      this.lblPesoBrut = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel6 = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel10 = new LmCorbieUI.Controls.LmLabel();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.lmPanel3 = new LmCorbieUI.Controls.LmPanel();
      this.txtSmCompr = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel5 = new LmCorbieUI.Controls.LmLabel();
      this.lmPanel2 = new LmCorbieUI.Controls.LmPanel();
      this.txtSmLarg = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel4 = new LmCorbieUI.Controls.LmLabel();
      this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
      this.txtDescricao = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.lblDescMat = new LmCorbieUI.Controls.LmLabel();
      this.lblEspess = new LmCorbieUI.Controls.LmLabel();
      this.lmLabel8 = new LmCorbieUI.Controls.LmLabel();
      this.tbcOperacoes = new LmCorbieUI.Controls.LmTabControl();
      this.tbpLista = new LmCorbieUI.Controls.LmTabPage();
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.tbpEngenharia = new LmCorbieUI.Controls.LmTabPage();
      this.trvProduto = new System.Windows.Forms.TreeView();
      this.tmr = new System.Windows.Forms.Timer(this.components);
      this.lblWarning = new LmCorbieUI.Controls.LmLabel();
      ((System.ComponentModel.ISupportInitialize)(this.ptbMaterialError)).BeginInit();
      this.pnlDados.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.lmPanel3.SuspendLayout();
      this.lmPanel2.SuspendLayout();
      this.lmPanel1.SuspendLayout();
      this.tbcOperacoes.SuspendLayout();
      this.tbpLista.SuspendLayout();
      this.tbpEngenharia.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblCodMat
      // 
      this.lblCodMat.BackColor = System.Drawing.Color.Transparent;
      this.lblCodMat.Cursor = System.Windows.Forms.Cursors.Hand;
      this.lblCodMat.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblCodMat.ForeColor = System.Drawing.Color.Red;
      this.lblCodMat.Location = new System.Drawing.Point(118, 24);
      this.lblCodMat.Margin = new System.Windows.Forms.Padding(3);
      this.lblCodMat.Name = "lblCodMat";
      this.lblCodMat.Size = new System.Drawing.Size(177, 15);
      this.lblCodMat.TabIndex = 18;
      this.lblCodMat.Text = "---";
      this.lblCodMat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.cmxToolTip1.SetToolTip(this.lblCodMat, "Clique para Copiar");
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
      // lblCodigoProduto
      // 
      this.lblCodigoProduto.AutoSize = true;
      this.lblCodigoProduto.BackColor = System.Drawing.Color.Transparent;
      this.lblCodigoProduto.Cursor = System.Windows.Forms.Cursors.Hand;
      this.lblCodigoProduto.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblCodigoProduto.ForeColor = System.Drawing.Color.Red;
      this.lblCodigoProduto.Location = new System.Drawing.Point(118, 66);
      this.lblCodigoProduto.Margin = new System.Windows.Forms.Padding(3);
      this.lblCodigoProduto.Name = "lblCodigoProduto";
      this.lblCodigoProduto.Size = new System.Drawing.Size(22, 15);
      this.lblCodigoProduto.TabIndex = 77;
      this.lblCodigoProduto.Text = "---";
      this.lblCodigoProduto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.cmxToolTip1.SetToolTip(this.lblCodigoProduto, "Clique para Copiar");
      this.lblCodigoProduto.Click += new System.EventHandler(this.LblCodigoProduto_Click);
      // 
      // ptbMaterialError
      // 
      this.ptbMaterialError.Cursor = System.Windows.Forms.Cursors.Hand;
      this.ptbMaterialError.Image = global::AddinArtama.Properties.Resources.error;
      this.ptbMaterialError.Location = new System.Drawing.Point(201, 21);
      this.ptbMaterialError.Name = "ptbMaterialError";
      this.ptbMaterialError.Size = new System.Drawing.Size(20, 20);
      this.ptbMaterialError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.ptbMaterialError.TabIndex = 78;
      this.ptbMaterialError.TabStop = false;
      this.cmxToolTip1.SetToolTip(this.ptbMaterialError, "Alterar Material");
      this.ptbMaterialError.Visible = false;
      this.ptbMaterialError.VisibleChanged += new System.EventHandler(this.PtbMaterialError_VisibleChanged);
      this.ptbMaterialError.Click += new System.EventHandler(this.PtbMaterialError_Click);
      // 
      // btnCarrProcess
      // 
      this.btnCarrProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCarrProcess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnCarrProcess.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnCarrProcess.BorderRadius = 13;
      this.btnCarrProcess.BorderSize = 0;
      this.btnCarrProcess.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCarrProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCarrProcess.Image = global::AddinArtama.Properties.Resources.carregar;
      this.btnCarrProcess.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnCarrProcess.Location = new System.Drawing.Point(3, 118);
      this.btnCarrProcess.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.btnCarrProcess.Name = "btnCarrProcess";
      this.btnCarrProcess.Size = new System.Drawing.Size(145, 30);
      this.btnCarrProcess.TabIndex = 3;
      this.btnCarrProcess.Text = " Carregar";
      this.btnCarrProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnCarrProcess, "Carregar componentes");
      this.btnCarrProcess.UseVisualStyleBackColor = false;
      this.btnCarrProcess.Click += new System.EventHandler(this.BtnCarrProcess_Click);
      // 
      // btnSalvar
      // 
      this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSalvar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnSalvar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnSalvar.BorderRadius = 13;
      this.btnSalvar.BorderSize = 0;
      this.btnSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSalvar.Image = ((System.Drawing.Image)(resources.GetObject("btnSalvar.Image")));
      this.btnSalvar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnSalvar.Location = new System.Drawing.Point(154, 118);
      this.btnSalvar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(146, 30);
      this.btnSalvar.TabIndex = 4;
      this.btnSalvar.Text = " Salvar";
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnSalvar, "Salvar/Atualizar Produto e Engenharia");
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
      // 
      // pnlDados
      // 
      this.pnlDados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.pnlDados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pnlDados.Controls.Add(this.lblPesoNbr);
      this.pnlDados.Controls.Add(this.lmLabel9);
      this.pnlDados.Controls.Add(this.lblPesoBrut);
      this.pnlDados.Controls.Add(this.lmLabel6);
      this.pnlDados.Controls.Add(this.ptbMaterialError);
      this.pnlDados.Controls.Add(this.lblCodigoProduto);
      this.pnlDados.Controls.Add(this.lmLabel10);
      this.pnlDados.Controls.Add(this.tableLayoutPanel1);
      this.pnlDados.Controls.Add(this.lmLabel1);
      this.pnlDados.Controls.Add(this.lblCodMat);
      this.pnlDados.Controls.Add(this.cmxLabel2);
      this.pnlDados.Controls.Add(this.lblDescMat);
      this.pnlDados.Controls.Add(this.lblEspess);
      this.pnlDados.Controls.Add(this.lmLabel8);
      this.pnlDados.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlDados.IsPanelMenu = false;
      this.pnlDados.Location = new System.Drawing.Point(0, 30);
      this.pnlDados.Name = "pnlDados";
      this.pnlDados.Size = new System.Drawing.Size(305, 289);
      this.pnlDados.TabIndex = 0;
      // 
      // lblPesoNbr
      // 
      this.lblPesoNbr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblPesoNbr.BackColor = System.Drawing.Color.Transparent;
      this.lblPesoNbr.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblPesoNbr.ForeColor = System.Drawing.Color.Red;
      this.lblPesoNbr.Location = new System.Drawing.Point(118, 108);
      this.lblPesoNbr.Margin = new System.Windows.Forms.Padding(3);
      this.lblPesoNbr.Name = "lblPesoNbr";
      this.lblPesoNbr.Size = new System.Drawing.Size(177, 15);
      this.lblPesoNbr.TabIndex = 82;
      this.lblPesoNbr.Text = "---";
      this.lblPesoNbr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmLabel9
      // 
      this.lmLabel9.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel9.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel9.ForeColor = System.Drawing.Color.Red;
      this.lmLabel9.Location = new System.Drawing.Point(3, 108);
      this.lmLabel9.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel9.Name = "lmLabel9";
      this.lmLabel9.Size = new System.Drawing.Size(113, 15);
      this.lmLabel9.TabIndex = 81;
      this.lmLabel9.Text = "Peso NBR:";
      this.lmLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblPesoBrut
      // 
      this.lblPesoBrut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblPesoBrut.BackColor = System.Drawing.Color.Transparent;
      this.lblPesoBrut.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblPesoBrut.ForeColor = System.Drawing.Color.Red;
      this.lblPesoBrut.Location = new System.Drawing.Point(118, 87);
      this.lblPesoBrut.Margin = new System.Windows.Forms.Padding(3);
      this.lblPesoBrut.Name = "lblPesoBrut";
      this.lblPesoBrut.Size = new System.Drawing.Size(177, 15);
      this.lblPesoBrut.TabIndex = 80;
      this.lblPesoBrut.Text = "---";
      this.lblPesoBrut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmLabel6
      // 
      this.lmLabel6.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel6.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel6.ForeColor = System.Drawing.Color.Red;
      this.lmLabel6.Location = new System.Drawing.Point(3, 87);
      this.lmLabel6.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel6.Name = "lmLabel6";
      this.lmLabel6.Size = new System.Drawing.Size(113, 15);
      this.lmLabel6.TabIndex = 79;
      this.lmLabel6.Text = "Peso Bruto:";
      this.lmLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lmLabel10
      // 
      this.lmLabel10.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel10.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lmLabel10.ForeColor = System.Drawing.Color.Red;
      this.lmLabel10.Location = new System.Drawing.Point(3, 66);
      this.lmLabel10.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel10.Name = "lmLabel10";
      this.lmLabel10.Size = new System.Drawing.Size(113, 15);
      this.lmLabel10.TabIndex = 76;
      this.lmLabel10.Text = "Código Produto:";
      this.lmLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.lmPanel3, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.lmPanel2, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.lmPanel1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.btnCarrProcess, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.btnSalvar, 1, 2);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 127);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(303, 155);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // lmPanel3
      // 
      this.lmPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lmPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel3.Controls.Add(this.txtSmCompr);
      this.lmPanel3.Controls.Add(this.lmLabel5);
      this.lmPanel3.IsPanelMenu = false;
      this.lmPanel3.Location = new System.Drawing.Point(151, 55);
      this.lmPanel3.Margin = new System.Windows.Forms.Padding(0);
      this.lmPanel3.Name = "lmPanel3";
      this.lmPanel3.Size = new System.Drawing.Size(152, 55);
      this.lmPanel3.TabIndex = 2;
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
      this.txtSmCompr.Size = new System.Drawing.Size(146, 31);
      this.txtSmCompr.TabIndex = 0;
      this.txtSmCompr.UnderlinedStyle = false;
      this.txtSmCompr.UseSelectable = true;
      this.txtSmCompr.Valor_Decimais = ((short)(0));
      this.txtSmCompr.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtSmCompr.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtSmCompr.Leave += new System.EventHandler(this.TxtSmCompr_Leave);
      // 
      // lmLabel5
      // 
      this.lmLabel5.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel5.Location = new System.Drawing.Point(3, 0);
      this.lmLabel5.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel5.Name = "lmLabel5";
      this.lmLabel5.Size = new System.Drawing.Size(113, 21);
      this.lmLabel5.TabIndex = 24;
      this.lmLabel5.Text = "SM Compr.";
      this.lmLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmPanel2
      // 
      this.lmPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lmPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel2.Controls.Add(this.txtSmLarg);
      this.lmPanel2.Controls.Add(this.lmLabel4);
      this.lmPanel2.IsPanelMenu = false;
      this.lmPanel2.Location = new System.Drawing.Point(0, 55);
      this.lmPanel2.Margin = new System.Windows.Forms.Padding(0);
      this.lmPanel2.Name = "lmPanel2";
      this.lmPanel2.Size = new System.Drawing.Size(151, 55);
      this.lmPanel2.TabIndex = 1;
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
      this.txtSmLarg.Size = new System.Drawing.Size(145, 31);
      this.txtSmLarg.TabIndex = 0;
      this.txtSmLarg.UnderlinedStyle = false;
      this.txtSmLarg.UseSelectable = true;
      this.txtSmLarg.Valor_Decimais = ((short)(0));
      this.txtSmLarg.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtSmLarg.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtSmLarg.Leave += new System.EventHandler(this.TxtSmLarg_Leave);
      // 
      // lmLabel4
      // 
      this.lmLabel4.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel4.Location = new System.Drawing.Point(3, 0);
      this.lmLabel4.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel4.Name = "lmLabel4";
      this.lmLabel4.Size = new System.Drawing.Size(113, 21);
      this.lmLabel4.TabIndex = 24;
      this.lmLabel4.Text = "SM Larg.";
      this.lmLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
      this.tbcOperacoes.Controls.Add(this.tbpEngenharia);
      this.tbcOperacoes.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbcOperacoes.Location = new System.Drawing.Point(0, 352);
      this.tbcOperacoes.Name = "tbcOperacoes";
      this.tbcOperacoes.SelectedIndex = 0;
      this.tbcOperacoes.Size = new System.Drawing.Size(305, 240);
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
      this.tbpLista.Size = new System.Drawing.Size(297, 198);
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
      this.dgv.EnabledFind = true;
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
      this.dgv.Size = new System.Drawing.Size(295, 187);
      this.dgv.TabIndex = 98;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      this.dgv.ProcurarTextChanged += new LmCorbieUI.Controls.LmDataGridView.TxtChange(this.Dgv_ProcurarTextChanged);
      this.dgv.Sorted += new LmCorbieUI.Controls.LmDataGridView.GridEvent(this.Dgv_Sorted);
      this.dgv.RowIndexChanged += new LmCorbieUI.Controls.LmDataGridView.RowEvent(this.Dgv_RowIndexChanged);
      // 
      // tbpEngenharia
      // 
      this.tbpEngenharia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpEngenharia.Controls.Add(this.trvProduto);
      this.tbpEngenharia.Location = new System.Drawing.Point(4, 38);
      this.tbpEngenharia.Name = "tbpEngenharia";
      this.tbpEngenharia.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
      this.tbpEngenharia.Size = new System.Drawing.Size(297, 231);
      this.tbpEngenharia.TabIndex = 2;
      this.tbpEngenharia.Text = "Engenharia";
      // 
      // trvProduto
      // 
      this.trvProduto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.trvProduto.Dock = System.Windows.Forms.DockStyle.Fill;
      this.trvProduto.Location = new System.Drawing.Point(0, 9);
      this.trvProduto.Name = "trvProduto";
      this.trvProduto.Size = new System.Drawing.Size(295, 220);
      this.trvProduto.TabIndex = 7;
      // 
      // tmr
      // 
      this.tmr.Interval = 1000;
      this.tmr.Tick += new System.EventHandler(this.Tmr_Tick);
      // 
      // lblWarning
      // 
      this.lblWarning.BackColor = System.Drawing.Color.Transparent;
      this.lblWarning.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblWarning.Dock = System.Windows.Forms.DockStyle.Top;
      this.lblWarning.FontSize = LmCorbieUI.Design.LmLabelSize.Small;
      this.lblWarning.FontWeight = LmCorbieUI.Design.LmLabelWeight.Bold;
      this.lblWarning.ForeColor = System.Drawing.Color.DarkGoldenrod;
      this.lblWarning.Location = new System.Drawing.Point(0, 319);
      this.lblWarning.Margin = new System.Windows.Forms.Padding(3);
      this.lblWarning.Name = "lblWarning";
      this.lblWarning.Size = new System.Drawing.Size(305, 33);
      this.lblWarning.TabIndex = 100;
      this.lblWarning.Text = "Alerta\r\nInformação";
      this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.lblWarning.UseCustomColor = true;
      this.lblWarning.Visible = false;
      // 
      // FrmProdutoImport
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(305, 592);
      this.Controls.Add(this.tbcOperacoes);
      this.Controls.Add(this.lblWarning);
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
      ((System.ComponentModel.ISupportInitialize)(this.ptbMaterialError)).EndInit();
      this.pnlDados.ResumeLayout(false);
      this.pnlDados.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.lmPanel3.ResumeLayout(false);
      this.lmPanel2.ResumeLayout(false);
      this.lmPanel1.ResumeLayout(false);
      this.tbcOperacoes.ResumeLayout(false);
      this.tbpLista.ResumeLayout(false);
      this.tbpEngenharia.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
        private LmCorbieUI.Controls.LmLabel lblCodMat;
        private LmCorbieUI.Controls.LmLabel cmxLabel2;
        private System.Windows.Forms.ToolTip cmxToolTip1;
    private LmCorbieUI.Controls.LmPanel pnlDados;
    private LmCorbieUI.Controls.LmLabel lmLabel1;
    private LmCorbieUI.Controls.LmLabel lmLabel2;
    private LmCorbieUI.Controls.LmTextBox txtDescricao;
    private LmCorbieUI.Controls.LmLabel lblDescMat;
    private LmCorbieUI.Controls.LmLabel lblEspess;
    private LmCorbieUI.Controls.LmLabel lmLabel8;
    private LmCorbieUI.Controls.LmTabControl tbcOperacoes;
    private LmCorbieUI.Controls.LmTabPage tbpLista;
    private LmCorbieUI.Controls.LmDataGridView dgv;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private LmCorbieUI.Controls.LmPanel lmPanel1;
    private LmCorbieUI.Controls.LmTabPage tbpEngenharia;
    private System.Windows.Forms.TreeView trvProduto;
    private System.Windows.Forms.Timer tmr;
    private System.Windows.Forms.PictureBox ptbMaterialError;
    private LmCorbieUI.Controls.LmLabel lblCodigoProduto;
    private LmCorbieUI.Controls.LmLabel lmLabel10;
    private LmCorbieUI.Controls.LmButton btnSalvar;
    private LmCorbieUI.Controls.LmButton btnCarrProcess;
    private LmCorbieUI.Controls.LmLabel lblPesoNbr;
    private LmCorbieUI.Controls.LmLabel lmLabel9;
    private LmCorbieUI.Controls.LmLabel lblPesoBrut;
    private LmCorbieUI.Controls.LmLabel lmLabel6;
    private LmCorbieUI.Controls.LmPanel lmPanel3;
    private LmCorbieUI.Controls.LmTextBox txtSmCompr;
    private LmCorbieUI.Controls.LmLabel lmLabel5;
    private LmCorbieUI.Controls.LmPanel lmPanel2;
    private LmCorbieUI.Controls.LmTextBox txtSmLarg;
    private LmCorbieUI.Controls.LmLabel lmLabel4;
    private LmCorbieUI.Controls.LmLabel lblWarning;
  }
}