namespace AddinArtama
{
    partial class FrmFormatosAtualizar
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFormatosAtualizar));
      this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
      this.cmsOpenFile = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmUnselectAll = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.tsmOpen3D = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmOpen2D = new System.Windows.Forms.ToolStripMenuItem();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.btnAtualizar = new LmCorbieUI.Controls.LmButton();
      this.btnCancelar = new LmCorbieUI.Controls.LmButton();
      this.btnCarregar = new LmCorbieUI.Controls.LmButton();
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.lmPanel1.SuspendLayout();
      this.cmsOpenFile.SuspendLayout();
      this.SuspendLayout();
      // 
      // lmPanel1
      // 
      this.lmPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel1.Controls.Add(this.btnAtualizar);
      this.lmPanel1.Controls.Add(this.btnCancelar);
      this.lmPanel1.Controls.Add(this.btnCarregar);
      this.lmPanel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.lmPanel1.IsPanelMenu = false;
      this.lmPanel1.Location = new System.Drawing.Point(0, 30);
      this.lmPanel1.Name = "lmPanel1";
      this.lmPanel1.Size = new System.Drawing.Size(269, 113);
      this.lmPanel1.TabIndex = 0;
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
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(241, 6);
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
      // btnAtualizar
      // 
      this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAtualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnAtualizar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnAtualizar.BorderRadius = 13;
      this.btnAtualizar.BorderSize = 0;
      this.btnAtualizar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnAtualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnAtualizar.Image = ((System.Drawing.Image)(resources.GetObject("btnAtualizar.Image")));
      this.btnAtualizar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnAtualizar.Location = new System.Drawing.Point(3, 41);
      this.btnAtualizar.Name = "btnAtualizar";
      this.btnAtualizar.Size = new System.Drawing.Size(259, 26);
      this.btnAtualizar.TabIndex = 1;
      this.btnAtualizar.Text = " Inicar Atualização";
      this.btnAtualizar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnAtualizar.UseVisualStyleBackColor = false;
      this.btnAtualizar.Click += new System.EventHandler(this.BtnAtualizar_Click);
      // 
      // btnCancelar
      // 
      this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnCancelar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnCancelar.BorderRadius = 13;
      this.btnCancelar.BorderSize = 0;
      this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCancelar.Enabled = false;
      this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCancelar.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelar.Image")));
      this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnCancelar.Location = new System.Drawing.Point(3, 73);
      this.btnCancelar.Name = "btnCancelar";
      this.btnCancelar.Size = new System.Drawing.Size(259, 26);
      this.btnCancelar.TabIndex = 2;
      this.btnCancelar.Tag = "Avançar";
      this.btnCancelar.Text = " Cancelar";
      this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnCancelar.UseVisualStyleBackColor = false;
      this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
      // 
      // btnCarregar
      // 
      this.btnCarregar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCarregar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnCarregar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnCarregar.BorderRadius = 13;
      this.btnCarregar.BorderSize = 0;
      this.btnCarregar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCarregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnCarregar.Image = ((System.Drawing.Image)(resources.GetObject("btnCarregar.Image")));
      this.btnCarregar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnCarregar.Location = new System.Drawing.Point(3, 9);
      this.btnCarregar.Name = "btnCarregar";
      this.btnCarregar.Size = new System.Drawing.Size(259, 26);
      this.btnCarregar.TabIndex = 0;
      this.btnCarregar.Text = " Selecionar Diretório";
      this.btnCarregar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnCarregar.UseVisualStyleBackColor = false;
      this.btnCarregar.Click += new System.EventHandler(this.BtnCarrDesenhos_Click);
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
      this.dgv.EnabledCsvButton = false;
      this.dgv.EnabledFind = true;
      this.dgv.EnabledHideColumnsButton = false;
      this.dgv.EnabledPdfButton = false;
      this.dgv.EnabledRefreshButton = false;
      this.dgv.LimparSelecaoAposCarregar = false;
      this.dgv.Location = new System.Drawing.Point(0, 143);
      this.dgv.Margin = new System.Windows.Forms.Padding(0);
      this.dgv.MostrarRodapeBotoes = true;
      this.dgv.MostrarTotalizador = false;
      this.dgv.Name = "dgv";
      this.dgv.PermiteAutoDimensionarLinha = false;
      this.dgv.PermiteDimensionarColuna = true;
      this.dgv.PermiteOrdenarColunas = true;
      this.dgv.PermiteOrdenarLinhas = true;
      this.dgv.PermiteQuebrarLinhaCabecalho = false;
      this.dgv.PermiteSelecaoMultipla = false;
      this.dgv.PosColunasGrid = "";
      this.dgv.Size = new System.Drawing.Size(269, 307);
      this.dgv.TabIndex = 1;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      this.dgv.ProcurarTextChanged += new LmCorbieUI.Controls.LmDataGridView.TxtChange(this.dgv_ProcurarTextChanged);
      // 
      // FrmFormatosAtualizar
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(269, 450);
      this.Controls.Add(this.dgv);
      this.Controls.Add(this.lmPanel1);
      this.HelpButton = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmFormatosAtualizar";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.Resizable = false;
      this.ShowInTaskbar = false;
      this.Text = "Atualizar Templates dos Desenhos";
      this.Loaded += new LmCorbieUI.LmForms.LmSingleForm.FormLoad(this.FrmFormatosAtualizar_Loaded);
      this.lmPanel1.ResumeLayout(false);
      this.cmsOpenFile.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
        private LmCorbieUI.Controls.LmPanel lmPanel1;
        private System.Windows.Forms.ToolTip cmxToolTip1;
        private System.Windows.Forms.ContextMenuStrip cmsOpenFile;
        private System.Windows.Forms.ToolStripMenuItem tsmSelectAll;
        private System.Windows.Forms.ToolStripMenuItem tsmUnselectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen3D;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen2D;
    private LmCorbieUI.Controls.LmButton btnAtualizar;
    private LmCorbieUI.Controls.LmButton btnCancelar;
    private LmCorbieUI.Controls.LmButton btnCarregar;
    private LmCorbieUI.Controls.LmDataGridView dgv;
  }
}