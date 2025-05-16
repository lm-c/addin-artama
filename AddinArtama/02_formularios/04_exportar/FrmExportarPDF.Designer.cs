namespace AddinArtama
{
    partial class FrmExportarPDF
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmExportarPDF));
      this.cmsOpenFile = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.lblPercDesenho = new LmCorbieUI.Controls.LmLabel();
      this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
      this.rdbOrdemArvore = new LmCorbieUI.Controls.LmRadioButton();
      this.rdbOrdemCodigo = new LmCorbieUI.Controls.LmRadioButton();
      this.ckbDwg = new LmCorbieUI.Controls.LmCheckBox();
      this.ckbPdf = new LmCorbieUI.Controls.LmCheckBox();
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.tmrExportar = new System.Windows.Forms.Timer(this.components);
      this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmUnselectAll = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmOpen3D = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmOpen2D = new System.Windows.Forms.ToolStripMenuItem();
      this.txtPesquisar = new LmCorbieUI.Controls.LmTextBox();
      this.btnExportar = new LmCorbieUI.Controls.LmButton();
      this.btnCancelar = new LmCorbieUI.Controls.LmButton();
      this.btnCarregar = new LmCorbieUI.Controls.LmButton();
      this.cmsOpenFile.SuspendLayout();
      this.lmPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // cmsOpenFile
      // 
      this.cmsOpenFile.Font = new System.Drawing.Font("Segoe UI", 12F);
      this.cmsOpenFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmSelectAll,
            this.tsmUnselectAll,
            this.toolStripMenuItem2,
            this.tsmOpen3D,
            this.tsmOpen2D});
      this.cmsOpenFile.Name = "cmsOpenFile";
      this.cmsOpenFile.Size = new System.Drawing.Size(245, 114);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(241, 6);
      // 
      // lblPercDesenho
      // 
      this.lblPercDesenho.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lblPercDesenho.Location = new System.Drawing.Point(0, 428);
      this.lblPercDesenho.Margin = new System.Windows.Forms.Padding(3);
      this.lblPercDesenho.Name = "lblPercDesenho";
      this.lblPercDesenho.Size = new System.Drawing.Size(350, 22);
      this.lblPercDesenho.TabIndex = 78;
      this.lblPercDesenho.Text = "Peça 0 de 0";
      this.lblPercDesenho.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmPanel1
      // 
      this.lmPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel1.Controls.Add(this.txtPesquisar);
      this.lmPanel1.Controls.Add(this.rdbOrdemArvore);
      this.lmPanel1.Controls.Add(this.rdbOrdemCodigo);
      this.lmPanel1.Controls.Add(this.ckbDwg);
      this.lmPanel1.Controls.Add(this.ckbPdf);
      this.lmPanel1.Controls.Add(this.btnExportar);
      this.lmPanel1.Controls.Add(this.btnCancelar);
      this.lmPanel1.Controls.Add(this.btnCarregar);
      this.lmPanel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.lmPanel1.IsPanelMenu = false;
      this.lmPanel1.Location = new System.Drawing.Point(0, 30);
      this.lmPanel1.Name = "lmPanel1";
      this.lmPanel1.Size = new System.Drawing.Size(350, 125);
      this.lmPanel1.TabIndex = 0;
      // 
      // rdbOrdemArvore
      // 
      this.rdbOrdemArvore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.rdbOrdemArvore.AutoSize = true;
      this.rdbOrdemArvore.Location = new System.Drawing.Point(76, 65);
      this.rdbOrdemArvore.Name = "rdbOrdemArvore";
      this.rdbOrdemArvore.Size = new System.Drawing.Size(111, 15);
      this.rdbOrdemArvore.TabIndex = 9;
      this.rdbOrdemArvore.Text = "ORDEM ÁRVORE";
      this.rdbOrdemArvore.UseSelectable = true;
      this.rdbOrdemArvore.CheckedChanged += new System.EventHandler(this.RdbOrdemArvore_CheckedChanged);
      // 
      // rdbOrdemCodigo
      // 
      this.rdbOrdemCodigo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.rdbOrdemCodigo.AutoSize = true;
      this.rdbOrdemCodigo.Checked = true;
      this.rdbOrdemCodigo.Location = new System.Drawing.Point(76, 44);
      this.rdbOrdemCodigo.Name = "rdbOrdemCodigo";
      this.rdbOrdemCodigo.Size = new System.Drawing.Size(112, 15);
      this.rdbOrdemCodigo.TabIndex = 8;
      this.rdbOrdemCodigo.TabStop = true;
      this.rdbOrdemCodigo.Text = "ORDEM CÓDIGO";
      this.rdbOrdemCodigo.UseSelectable = true;
      this.rdbOrdemCodigo.CheckedChanged += new System.EventHandler(this.RdbOrdemCodigo_CheckedChanged);
      // 
      // ckbDwg
      // 
      this.ckbDwg.AutoSize = true;
      this.ckbDwg.Location = new System.Drawing.Point(4, 63);
      this.ckbDwg.Name = "ckbDwg";
      this.ckbDwg.Propriedade = null;
      this.ckbDwg.Size = new System.Drawing.Size(58, 19);
      this.ckbDwg.TabIndex = 6;
      this.ckbDwg.Text = "DWG";
      this.ckbDwg.UseSelectable = true;
      // 
      // ckbPdf
      // 
      this.ckbPdf.AutoSize = true;
      this.ckbPdf.Checked = true;
      this.ckbPdf.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckbPdf.Location = new System.Drawing.Point(4, 42);
      this.ckbPdf.Name = "ckbPdf";
      this.ckbPdf.Propriedade = null;
      this.ckbPdf.Size = new System.Drawing.Size(50, 19);
      this.ckbPdf.TabIndex = 5;
      this.ckbPdf.Text = "PDF";
      this.ckbPdf.UseSelectable = true;
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
      this.dgv.ContextMenuStrip = this.cmsOpenFile;
      this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgv.EnabledCsvButton = true;
      this.dgv.EnabledFind = true;
      this.dgv.EnabledHideColumnsButton = true;
      this.dgv.EnabledPdfButton = true;
      this.dgv.EnabledRefreshButton = true;
      this.dgv.LimparSelecaoAposCarregar = false;
      this.dgv.Location = new System.Drawing.Point(0, 155);
      this.dgv.Margin = new System.Windows.Forms.Padding(0);
      this.dgv.MostrarRodapeBotoes = false;
      this.dgv.MostrarTotalizador = false;
      this.dgv.Name = "dgv";
      this.dgv.PermiteAutoDimensionarLinha = false;
      this.dgv.PermiteDimensionarColuna = true;
      this.dgv.PermiteOrdenarColunas = true;
      this.dgv.PermiteOrdenarLinhas = true;
      this.dgv.PermiteQuebrarLinhaCabecalho = false;
      this.dgv.PermiteSelecaoMultipla = false;
      this.dgv.PosColunasGrid = "";
      this.dgv.Size = new System.Drawing.Size(350, 273);
      this.dgv.TabIndex = 81;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      this.dgv.RowIndexChanged += new LmCorbieUI.Controls.LmDataGridView.RowEvent(this.Dgv_RowIndexChanged);
      // 
      // tmrExportar
      // 
      this.tmrExportar.Tick += new System.EventHandler(this.TmrExportar_Tick);
      // 
      // tsmSelectAll
      // 
      this.tsmSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("tsmSelectAll.Image")));
      this.tsmSelectAll.Name = "tsmSelectAll";
      this.tsmSelectAll.Size = new System.Drawing.Size(244, 26);
      this.tsmSelectAll.Text = "Selecionar Todos";
      this.tsmSelectAll.Click += new System.EventHandler(this.TsmSelectAll_Click);
      // 
      // tsmUnselectAll
      // 
      this.tsmUnselectAll.Image = ((System.Drawing.Image)(resources.GetObject("tsmUnselectAll.Image")));
      this.tsmUnselectAll.Name = "tsmUnselectAll";
      this.tsmUnselectAll.Size = new System.Drawing.Size(244, 26);
      this.tsmUnselectAll.Text = "Remover Seleção Todos";
      this.tsmUnselectAll.Click += new System.EventHandler(this.TsmUnselectAll_Click);
      // 
      // tsmOpen3D
      // 
      this.tsmOpen3D.Image = ((System.Drawing.Image)(resources.GetObject("tsmOpen3D.Image")));
      this.tsmOpen3D.Name = "tsmOpen3D";
      this.tsmOpen3D.Size = new System.Drawing.Size(244, 26);
      this.tsmOpen3D.Text = "Abrir 3D";
      this.tsmOpen3D.Click += new System.EventHandler(this.TsmOpen3D_Click);
      // 
      // tsmOpen2D
      // 
      this.tsmOpen2D.Image = ((System.Drawing.Image)(resources.GetObject("tsmOpen2D.Image")));
      this.tsmOpen2D.Name = "tsmOpen2D";
      this.tsmOpen2D.Size = new System.Drawing.Size(244, 26);
      this.tsmOpen2D.Text = "Abrir 2D";
      this.tsmOpen2D.Click += new System.EventHandler(this.TsmOpen2D_Click);
      // 
      // txtPesquisar
      // 
      this.txtPesquisar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtPesquisar.BorderRadius = 15;
      this.txtPesquisar.BorderSize = 2;
      this.txtPesquisar.F7ToolTipText = null;
      this.txtPesquisar.F8ToolTipText = null;
      this.txtPesquisar.F9ToolTipText = null;
      this.txtPesquisar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtPesquisar.Icon = ((System.Drawing.Image)(resources.GetObject("txtPesquisar.Icon")));
      this.txtPesquisar.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtPesquisar.IconF7")));
      this.txtPesquisar.IconToolTipText = null;
      this.txtPesquisar.Lines = new string[0];
      this.txtPesquisar.Location = new System.Drawing.Point(4, 86);
      this.txtPesquisar.MaxLength = 32767;
      this.txtPesquisar.Name = "txtPesquisar";
      this.txtPesquisar.PasswordChar = '\0';
      this.txtPesquisar.Propriedade = null;
      this.txtPesquisar.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtPesquisar.SelectedText = "";
      this.txtPesquisar.SelectionLength = 0;
      this.txtPesquisar.SelectionStart = 0;
      this.txtPesquisar.ShortcutsEnabled = true;
      this.txtPesquisar.ShowButtonF7 = true;
      this.txtPesquisar.ShowIcon = true;
      this.txtPesquisar.Size = new System.Drawing.Size(184, 30);
      this.txtPesquisar.TabIndex = 10;
      this.txtPesquisar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.txtPesquisar.UnderlinedStyle = false;
      this.txtPesquisar.UseSelectable = true;
      this.txtPesquisar.Valor_Decimais = ((short)(0));
      this.txtPesquisar.WaterMark = "Pesquisar";
      this.txtPesquisar.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtPesquisar.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
      this.txtPesquisar.ButtonClickF7 += new LmCorbieUI.Controls.LmTextBox.ButClick(this.TxtPesquisar_ButtonClickF7);
      // 
      // btnExportar
      // 
      this.btnExportar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnExportar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnExportar.BorderRadius = 15;
      this.btnExportar.BorderSize = 0;
      this.btnExportar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnExportar.Image = ((System.Drawing.Image)(resources.GetObject("btnExportar.Image")));
      this.btnExportar.Location = new System.Drawing.Point(36, 7);
      this.btnExportar.Margin = new System.Windows.Forms.Padding(1);
      this.btnExportar.Name = "btnExportar";
      this.btnExportar.Size = new System.Drawing.Size(31, 31);
      this.btnExportar.TabIndex = 1;
      this.btnExportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnExportar.UseVisualStyleBackColor = false;
      this.btnExportar.Click += new System.EventHandler(this.BtnExportar_Click);
      // 
      // btnCancelar
      // 
      this.btnCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnCancelar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnCancelar.BorderRadius = 15;
      this.btnCancelar.BorderSize = 0;
      this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCancelar.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelar.Image")));
      this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnCancelar.Location = new System.Drawing.Point(69, 7);
      this.btnCancelar.Margin = new System.Windows.Forms.Padding(1);
      this.btnCancelar.Name = "btnCancelar";
      this.btnCancelar.Size = new System.Drawing.Size(119, 31);
      this.btnCancelar.TabIndex = 3;
      this.btnCancelar.Tag = "Avançar";
      this.btnCancelar.Text = "Cancelar";
      this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnCancelar.UseVisualStyleBackColor = false;
      this.btnCancelar.Visible = false;
      this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
      // 
      // btnCarregar
      // 
      this.btnCarregar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnCarregar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnCarregar.BorderRadius = 15;
      this.btnCarregar.BorderSize = 0;
      this.btnCarregar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCarregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCarregar.Image = ((System.Drawing.Image)(resources.GetObject("btnCarregar.Image")));
      this.btnCarregar.Location = new System.Drawing.Point(3, 6);
      this.btnCarregar.Margin = new System.Windows.Forms.Padding(1);
      this.btnCarregar.Name = "btnCarregar";
      this.btnCarregar.Size = new System.Drawing.Size(31, 31);
      this.btnCarregar.TabIndex = 0;
      this.btnCarregar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnCarregar.UseVisualStyleBackColor = false;
      this.btnCarregar.Click += new System.EventHandler(this.BtnCarregar_Click);
      // 
      // FrmExportarPDF
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(350, 450);
      this.Controls.Add(this.dgv);
      this.Controls.Add(this.lmPanel1);
      this.Controls.Add(this.lblPercDesenho);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Movimentar = false;
      this.Name = "FrmExportarPDF";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.Resizable = false;
      this.Text = "Exportar PDF";
      this.cmsOpenFile.ResumeLayout(false);
      this.lmPanel1.ResumeLayout(false);
      this.lmPanel1.PerformLayout();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsOpenFile;
        private System.Windows.Forms.ToolStripMenuItem tsmSelectAll;
        private System.Windows.Forms.ToolStripMenuItem tsmUnselectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen3D;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen2D;
        private System.Windows.Forms.ToolTip cmxToolTip1;
        private LmCorbieUI.Controls.LmLabel lblPercDesenho;
        private LmCorbieUI.Controls.LmPanel lmPanel1;
        private LmCorbieUI.Controls.LmButton btnExportar;
        private LmCorbieUI.Controls.LmButton btnCancelar;
        private LmCorbieUI.Controls.LmButton btnCarregar;
        private LmCorbieUI.Controls.LmCheckBox ckbDwg;
        private LmCorbieUI.Controls.LmCheckBox ckbPdf;
        private LmCorbieUI.Controls.LmRadioButton rdbOrdemArvore;
        private LmCorbieUI.Controls.LmRadioButton rdbOrdemCodigo;
        private LmCorbieUI.Controls.LmTextBox txtPesquisar;
        private LmCorbieUI.Controls.LmDataGridView dgv;
    private System.Windows.Forms.Timer tmrExportar;
  }
}