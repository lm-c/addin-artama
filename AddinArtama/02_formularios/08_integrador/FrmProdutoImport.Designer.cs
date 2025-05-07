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
      this.btnImportar = new LmCorbieUI.Controls.LmButton();
      this.btnCarrProcess = new LmCorbieUI.Controls.LmButton();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.pnlControl.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlControl
      // 
      this.pnlControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.pnlControl.Controls.Add(this.btnImportar);
      this.pnlControl.Controls.Add(this.btnCarrProcess);
      this.pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlControl.IsPanelMenu = false;
      this.pnlControl.Location = new System.Drawing.Point(0, 30);
      this.pnlControl.Name = "pnlControl";
      this.pnlControl.Size = new System.Drawing.Size(288, 80);
      this.pnlControl.TabIndex = 2;
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
      this.btnImportar.Text = " Cadastrar Itens Genéricos";
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
      this.dgv.EnabledFind = true;
      this.dgv.EnabledHideColumnsButton = false;
      this.dgv.EnabledPdfButton = false;
      this.dgv.EnabledRefreshButton = false;
      this.dgv.LimparSelecaoAposCarregar = false;
      this.dgv.Location = new System.Drawing.Point(0, 110);
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
      this.dgv.Size = new System.Drawing.Size(288, 394);
      this.dgv.TabIndex = 3;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      // 
      // FrmProdutoImport
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(288, 504);
      this.Controls.Add(this.dgv);
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
      this.Text = "Cadastro de Item Genérico";
      this.Loaded += new LmCorbieUI.LmForms.LmSingleForm.FormLoad(this.FrmProdutoImport_Loaded);
      this.pnlControl.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
        private LmCorbieUI.Controls.LmPanel pnlControl;
        private LmCorbieUI.Controls.LmButton btnCarrProcess;
        private System.Windows.Forms.ToolTip cmxToolTip1;
    private LmCorbieUI.Controls.LmButton btnImportar;
    private LmCorbieUI.Controls.LmDataGridView dgv;
  }
}