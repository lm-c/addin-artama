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
      this.pnlControl = new LmCorbieUI.Controls.LmPanel();
      this.btnCancel = new LmCorbieUI.Controls.LmButton();
      this.btnImportar = new LmCorbieUI.Controls.LmButton();
      this.btnCarrProcess = new LmCorbieUI.Controls.LmButton();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.lmButton1 = new LmCorbieUI.Controls.LmButton();
      this.tbcProduto = new LmCorbieUI.Controls.LmTabControl();
      this.tbpCadastoItem = new LmCorbieUI.Controls.LmTabPage();
      this.tbpEngenharia = new LmCorbieUI.Controls.LmTabPage();
      this.trvProduto = new System.Windows.Forms.TreeView();
      this.pnlControl.SuspendLayout();
      this.tbcProduto.SuspendLayout();
      this.tbpCadastoItem.SuspendLayout();
      this.tbpEngenharia.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlControl
      // 
      this.pnlControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.pnlControl.Controls.Add(this.btnCancel);
      this.pnlControl.Controls.Add(this.btnImportar);
      this.pnlControl.Controls.Add(this.btnCarrProcess);
      this.pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlControl.IsPanelMenu = false;
      this.pnlControl.Location = new System.Drawing.Point(0, 30);
      this.pnlControl.Name = "pnlControl";
      this.pnlControl.Size = new System.Drawing.Size(288, 116);
      this.pnlControl.TabIndex = 2;
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnCancel.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnCancel.BorderRadius = 15;
      this.btnCancel.BorderSize = 0;
      this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCancel.Enabled = false;
      this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
      this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnCancel.Location = new System.Drawing.Point(6, 76);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(277, 31);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = " Cancelar Importação";
      this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnCancel.UseVisualStyleBackColor = false;
      this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
      // 
      // btnImportar
      // 
      this.btnImportar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImportar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnImportar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnImportar.BorderRadius = 15;
      this.btnImportar.BorderSize = 0;
      this.btnImportar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnImportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnImportar.Image = ((System.Drawing.Image)(resources.GetObject("btnImportar.Image")));
      this.btnImportar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnImportar.Location = new System.Drawing.Point(6, 40);
      this.btnImportar.Name = "btnImportar";
      this.btnImportar.Size = new System.Drawing.Size(277, 31);
      this.btnImportar.TabIndex = 2;
      this.btnImportar.Text = " Cadastrar Produto";
      this.btnImportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnImportar.UseVisualStyleBackColor = false;
      this.btnImportar.Click += new System.EventHandler(this.BtnImportar_Click);
      // 
      // btnCarrProcess
      // 
      this.btnCarrProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCarrProcess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnCarrProcess.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnCarrProcess.BorderRadius = 15;
      this.btnCarrProcess.BorderSize = 0;
      this.btnCarrProcess.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCarrProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCarrProcess.Image = ((System.Drawing.Image)(resources.GetObject("btnCarrProcess.Image")));
      this.btnCarrProcess.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnCarrProcess.Location = new System.Drawing.Point(6, 3);
      this.btnCarrProcess.Name = "btnCarrProcess";
      this.btnCarrProcess.Size = new System.Drawing.Size(277, 31);
      this.btnCarrProcess.TabIndex = 0;
      this.btnCarrProcess.Text = " Carregar componentes";
      this.btnCarrProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnCarrProcess.UseVisualStyleBackColor = false;
      this.btnCarrProcess.Click += new System.EventHandler(this.BtnCarrProcess_Click);
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
      this.dgv.Size = new System.Drawing.Size(278, 305);
      this.dgv.TabIndex = 3;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      this.dgv.ProcurarTextChanged += new LmCorbieUI.Controls.LmDataGridView.TxtChange(this.Dgv_ProcurarTextChanged);
      this.dgv.CellClick += new LmCorbieUI.Controls.LmDataGridView.CellEvent(this.dgv_CellClick);
      // 
      // lmButton1
      // 
      this.lmButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lmButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.lmButton1.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.lmButton1.BorderRadius = 15;
      this.lmButton1.BorderSize = 0;
      this.lmButton1.Cursor = System.Windows.Forms.Cursors.Hand;
      this.lmButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.lmButton1.Image = ((System.Drawing.Image)(resources.GetObject("lmButton1.Image")));
      this.lmButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.lmButton1.Location = new System.Drawing.Point(163, 0);
      this.lmButton1.Name = "lmButton1";
      this.lmButton1.Size = new System.Drawing.Size(87, 31);
      this.lmButton1.TabIndex = 4;
      this.lmButton1.Text = " Excluir tudo";
      this.lmButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.lmButton1.UseVisualStyleBackColor = false;
      this.lmButton1.Visible = false;
      this.lmButton1.Click += new System.EventHandler(this.lmButton1_Click);
      // 
      // tbcProduto
      // 
      this.tbcProduto.Controls.Add(this.tbpCadastoItem);
      this.tbcProduto.Controls.Add(this.tbpEngenharia);
      this.tbcProduto.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbcProduto.Location = new System.Drawing.Point(0, 146);
      this.tbcProduto.Name = "tbcProduto";
      this.tbcProduto.SelectedIndex = 0;
      this.tbcProduto.Size = new System.Drawing.Size(288, 358);
      this.tbcProduto.TabIndex = 5;
      this.tbcProduto.UseSelectable = true;
      // 
      // tbpCadastoItem
      // 
      this.tbpCadastoItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpCadastoItem.Controls.Add(this.dgv);
      this.tbpCadastoItem.Location = new System.Drawing.Point(4, 38);
      this.tbpCadastoItem.Name = "tbpCadastoItem";
      this.tbpCadastoItem.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
      this.tbpCadastoItem.Size = new System.Drawing.Size(280, 316);
      this.tbpCadastoItem.TabIndex = 0;
      this.tbpCadastoItem.Text = "Itens";
      // 
      // tbpEngenharia
      // 
      this.tbpEngenharia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tbpEngenharia.Controls.Add(this.trvProduto);
      this.tbpEngenharia.Location = new System.Drawing.Point(4, 38);
      this.tbpEngenharia.Name = "tbpEngenharia";
      this.tbpEngenharia.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
      this.tbpEngenharia.Size = new System.Drawing.Size(280, 316);
      this.tbpEngenharia.TabIndex = 1;
      this.tbpEngenharia.Text = "Engenharia";
      // 
      // trvProduto
      // 
      this.trvProduto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.trvProduto.Dock = System.Windows.Forms.DockStyle.Fill;
      this.trvProduto.Location = new System.Drawing.Point(0, 9);
      this.trvProduto.Name = "trvProduto";
      this.trvProduto.Size = new System.Drawing.Size(278, 305);
      this.trvProduto.TabIndex = 6;
      // 
      // FrmProdutoImport
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(288, 504);
      this.Controls.Add(this.tbcProduto);
      this.Controls.Add(this.lmButton1);
      this.Controls.Add(this.pnlControl);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(288, 300);
      this.Movimentar = false;
      this.Name = "FrmProdutoImport";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.ShowInTaskbar = false;
      this.Text = "Cadastro de Produto";
      this.Loaded += new LmCorbieUI.LmForms.LmSingleForm.FormLoad(this.FrmProdutoImport_Loaded);
      this.pnlControl.ResumeLayout(false);
      this.tbcProduto.ResumeLayout(false);
      this.tbpCadastoItem.ResumeLayout(false);
      this.tbpEngenharia.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
        private LmCorbieUI.Controls.LmPanel pnlControl;
        private LmCorbieUI.Controls.LmButton btnCarrProcess;
        private System.Windows.Forms.ToolTip cmxToolTip1;
    private LmCorbieUI.Controls.LmButton btnImportar;
    private LmCorbieUI.Controls.LmDataGridView dgv;
    private LmCorbieUI.Controls.LmButton btnCancel;
    private LmCorbieUI.Controls.LmButton lmButton1;
    private LmCorbieUI.Controls.LmTabControl tbcProduto;
    private LmCorbieUI.Controls.LmTabPage tbpCadastoItem;
    private LmCorbieUI.Controls.LmTabPage tbpEngenharia;
    private System.Windows.Forms.TreeView trvProduto;
  }
}