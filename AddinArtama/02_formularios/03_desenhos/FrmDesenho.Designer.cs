namespace AddinArtama
{
    partial class FrmDesenho
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDesenho));
      this.lblPercDesenho = new LmCorbieUI.Controls.LmLabel();
      this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
      this.btnUpDes = new LmCorbieUI.Controls.LmButton();
      this.btnDownDes = new LmCorbieUI.Controls.LmButton();
      this.btnFFdesenho = new LmCorbieUI.Controls.LmButton();
      this.btnA1 = new LmCorbieUI.Controls.LmButton();
      this.btnA2 = new LmCorbieUI.Controls.LmButton();
      this.btnA3 = new LmCorbieUI.Controls.LmButton();
      this.btnA4 = new LmCorbieUI.Controls.LmButton();
      this.btnA4R = new LmCorbieUI.Controls.LmButton();
      this.btnInserirListaSoldagem = new LmCorbieUI.Controls.LmButton();
      this.ckbPromov = new LmCorbieUI.Controls.LmCheckBox();
      this.txtName = new LmCorbieUI.Controls.LmTextBox();
      this.btnLoadDesenho = new LmCorbieUI.Controls.LmButton();
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.cmsOpenFile = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.tsmOpen3D = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmOpen2D = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmAttCodigo = new System.Windows.Forms.ToolStripMenuItem();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.lmPanel1.SuspendLayout();
      this.cmsOpenFile.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblPercDesenho
      // 
      this.lblPercDesenho.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lblPercDesenho.Location = new System.Drawing.Point(0, 428);
      this.lblPercDesenho.Margin = new System.Windows.Forms.Padding(3);
      this.lblPercDesenho.Name = "lblPercDesenho";
      this.lblPercDesenho.Size = new System.Drawing.Size(350, 22);
      this.lblPercDesenho.TabIndex = 73;
      this.lblPercDesenho.Text = "Peça 0 de 0";
      this.lblPercDesenho.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lmPanel1
      // 
      this.lmPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel1.Controls.Add(this.btnUpDes);
      this.lmPanel1.Controls.Add(this.btnDownDes);
      this.lmPanel1.Controls.Add(this.btnFFdesenho);
      this.lmPanel1.Controls.Add(this.btnA1);
      this.lmPanel1.Controls.Add(this.btnA2);
      this.lmPanel1.Controls.Add(this.btnA3);
      this.lmPanel1.Controls.Add(this.btnA4);
      this.lmPanel1.Controls.Add(this.btnA4R);
      this.lmPanel1.Controls.Add(this.btnInserirListaSoldagem);
      this.lmPanel1.Controls.Add(this.ckbPromov);
      this.lmPanel1.Controls.Add(this.txtName);
      this.lmPanel1.Controls.Add(this.btnLoadDesenho);
      this.lmPanel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.lmPanel1.IsPanelMenu = false;
      this.lmPanel1.Location = new System.Drawing.Point(0, 30);
      this.lmPanel1.Name = "lmPanel1";
      this.lmPanel1.Size = new System.Drawing.Size(350, 134);
      this.lmPanel1.TabIndex = 0;
      // 
      // btnUpDes
      // 
      this.btnUpDes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.btnUpDes.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnUpDes.BorderRadius = 15;
      this.btnUpDes.BorderSize = 0;
      this.btnUpDes.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnUpDes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnUpDes.Image = ((System.Drawing.Image)(resources.GetObject("btnUpDes.Image")));
      this.btnUpDes.Location = new System.Drawing.Point(192, 85);
      this.btnUpDes.Margin = new System.Windows.Forms.Padding(1);
      this.btnUpDes.Name = "btnUpDes";
      this.btnUpDes.Size = new System.Drawing.Size(31, 31);
      this.btnUpDes.TabIndex = 11;
      this.btnUpDes.Tag = "Up";
      this.btnUpDes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnUpDes, "Folha Posterior");
      this.btnUpDes.UseVisualStyleBackColor = false;
      this.btnUpDes.Click += new System.EventHandler(this.BtnUpDes_Click);
      // 
      // btnDownDes
      // 
      this.btnDownDes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnDownDes.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnDownDes.BorderRadius = 15;
      this.btnDownDes.BorderSize = 0;
      this.btnDownDes.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnDownDes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnDownDes.Image = ((System.Drawing.Image)(resources.GetObject("btnDownDes.Image")));
      this.btnDownDes.Location = new System.Drawing.Point(159, 85);
      this.btnDownDes.Margin = new System.Windows.Forms.Padding(1);
      this.btnDownDes.Name = "btnDownDes";
      this.btnDownDes.Size = new System.Drawing.Size(31, 31);
      this.btnDownDes.TabIndex = 10;
      this.btnDownDes.Tag = "Down";
      this.btnDownDes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnDownDes, "Folha Anterior");
      this.btnDownDes.UseVisualStyleBackColor = false;
      this.btnDownDes.Click += new System.EventHandler(this.BtnDownDes_Click);
      // 
      // btnFFdesenho
      // 
      this.btnFFdesenho.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnFFdesenho.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnFFdesenho.BorderRadius = 15;
      this.btnFFdesenho.BorderSize = 0;
      this.btnFFdesenho.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnFFdesenho.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnFFdesenho.Image = ((System.Drawing.Image)(resources.GetObject("btnFFdesenho.Image")));
      this.btnFFdesenho.Location = new System.Drawing.Point(3, 46);
      this.btnFFdesenho.Margin = new System.Windows.Forms.Padding(1);
      this.btnFFdesenho.Name = "btnFFdesenho";
      this.btnFFdesenho.Size = new System.Drawing.Size(31, 31);
      this.btnFFdesenho.TabIndex = 2;
      this.btnFFdesenho.Tag = "Avançar";
      this.btnFFdesenho.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnFFdesenho, "Avançar");
      this.btnFFdesenho.UseVisualStyleBackColor = false;
      this.btnFFdesenho.Click += new System.EventHandler(this.BtnFFdesenho_Click);
      // 
      // btnA1
      // 
      this.btnA1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnA1.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnA1.BorderRadius = 15;
      this.btnA1.BorderSize = 0;
      this.btnA1.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnA1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnA1.Location = new System.Drawing.Point(192, 46);
      this.btnA1.Margin = new System.Windows.Forms.Padding(1);
      this.btnA1.Name = "btnA1";
      this.btnA1.Size = new System.Drawing.Size(31, 31);
      this.btnA1.TabIndex = 7;
      this.btnA1.Text = "A1";
      this.btnA1.UseVisualStyleBackColor = false;
      this.btnA1.Click += new System.EventHandler(this.BtnA1_Click);
      // 
      // btnA2
      // 
      this.btnA2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnA2.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnA2.BorderRadius = 15;
      this.btnA2.BorderSize = 0;
      this.btnA2.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnA2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnA2.Location = new System.Drawing.Point(159, 46);
      this.btnA2.Margin = new System.Windows.Forms.Padding(1);
      this.btnA2.Name = "btnA2";
      this.btnA2.Size = new System.Drawing.Size(31, 31);
      this.btnA2.TabIndex = 6;
      this.btnA2.Text = "A2";
      this.btnA2.UseVisualStyleBackColor = false;
      this.btnA2.Click += new System.EventHandler(this.BtnA2_Click);
      // 
      // btnA3
      // 
      this.btnA3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnA3.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnA3.BorderRadius = 15;
      this.btnA3.BorderSize = 0;
      this.btnA3.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnA3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnA3.Location = new System.Drawing.Point(126, 46);
      this.btnA3.Margin = new System.Windows.Forms.Padding(1);
      this.btnA3.Name = "btnA3";
      this.btnA3.Size = new System.Drawing.Size(31, 31);
      this.btnA3.TabIndex = 5;
      this.btnA3.Text = "A3";
      this.btnA3.UseVisualStyleBackColor = false;
      this.btnA3.Click += new System.EventHandler(this.BtnA3_Click);
      // 
      // btnA4
      // 
      this.btnA4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnA4.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnA4.BorderRadius = 15;
      this.btnA4.BorderSize = 0;
      this.btnA4.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnA4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnA4.Location = new System.Drawing.Point(93, 46);
      this.btnA4.Margin = new System.Windows.Forms.Padding(1);
      this.btnA4.Name = "btnA4";
      this.btnA4.Size = new System.Drawing.Size(31, 31);
      this.btnA4.TabIndex = 4;
      this.btnA4.Text = "A4";
      this.cmxToolTip1.SetToolTip(this.btnA4, "A4 Paisagem");
      this.btnA4.UseVisualStyleBackColor = false;
      this.btnA4.Click += new System.EventHandler(this.BtnA4_Click);
      // 
      // btnA4R
      // 
      this.btnA4R.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnA4R.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnA4R.BorderRadius = 15;
      this.btnA4R.BorderSize = 0;
      this.btnA4R.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnA4R.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnA4R.Location = new System.Drawing.Point(60, 46);
      this.btnA4R.Margin = new System.Windows.Forms.Padding(1);
      this.btnA4R.Name = "btnA4R";
      this.btnA4R.Size = new System.Drawing.Size(31, 31);
      this.btnA4R.TabIndex = 3;
      this.btnA4R.Text = "4R";
      this.cmxToolTip1.SetToolTip(this.btnA4R, "A4 Retrato");
      this.btnA4R.UseVisualStyleBackColor = false;
      this.btnA4R.Click += new System.EventHandler(this.BtnA4R_Click);
      // 
      // btnInserirListaSoldagem
      // 
      this.btnInserirListaSoldagem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnInserirListaSoldagem.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnInserirListaSoldagem.BorderRadius = 15;
      this.btnInserirListaSoldagem.BorderSize = 0;
      this.btnInserirListaSoldagem.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnInserirListaSoldagem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnInserirListaSoldagem.Image = ((System.Drawing.Image)(resources.GetObject("btnInserirListaSoldagem.Image")));
      this.btnInserirListaSoldagem.Location = new System.Drawing.Point(3, 85);
      this.btnInserirListaSoldagem.Margin = new System.Windows.Forms.Padding(1);
      this.btnInserirListaSoldagem.Name = "btnInserirListaSoldagem";
      this.btnInserirListaSoldagem.Size = new System.Drawing.Size(31, 31);
      this.btnInserirListaSoldagem.TabIndex = 8;
      this.btnInserirListaSoldagem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnInserirListaSoldagem, "Inserir Lista");
      this.btnInserirListaSoldagem.UseVisualStyleBackColor = false;
      this.btnInserirListaSoldagem.Click += new System.EventHandler(this.BtnInserirListaSoldagem_Click);
      // 
      // ckbPromov
      // 
      this.ckbPromov.AutoSize = true;
      this.ckbPromov.Location = new System.Drawing.Point(38, 97);
      this.ckbPromov.Name = "ckbPromov";
      this.ckbPromov.Propriedade = null;
      this.ckbPromov.Size = new System.Drawing.Size(117, 19);
      this.ckbPromov.TabIndex = 9;
      this.ckbPromov.Text = "Promover Lista";
      this.ckbPromov.UseSelectable = true;
      // 
      // txtName
      // 
      this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtName.BorderRadius = 15;
      this.txtName.BorderSize = 2;
      this.txtName.F7ToolTipText = null;
      this.txtName.F8ToolTipText = null;
      this.txtName.F9ToolTipText = null;
      this.txtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtName.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtName.IconF7")));
      this.txtName.IconToolTipText = null;
      this.txtName.Lines = new string[0];
      this.txtName.Location = new System.Drawing.Point(53, 6);
      this.txtName.MaxLength = 32767;
      this.txtName.Name = "txtName";
      this.txtName.PasswordChar = '\0';
      this.txtName.Propriedade = null;
      this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtName.SelectedText = "";
      this.txtName.SelectionLength = 0;
      this.txtName.SelectionStart = 0;
      this.txtName.ShortcutsEnabled = true;
      this.txtName.ShowButtonF7 = true;
      this.txtName.Size = new System.Drawing.Size(170, 30);
      this.txtName.TabIndex = 1;
      this.txtName.UnderlinedStyle = false;
      this.txtName.UseSelectable = true;
      this.txtName.Valor_Decimais = ((short)(0));
      this.txtName.WaterMark = "Nome da Folha";
      this.txtName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtName.ButtonClickF7 += new LmCorbieUI.Controls.LmTextBox.ButClick(this.BtnRename_Click);
      // 
      // btnLoadDesenho
      // 
      this.btnLoadDesenho.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnLoadDesenho.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnLoadDesenho.BorderRadius = 15;
      this.btnLoadDesenho.BorderSize = 0;
      this.btnLoadDesenho.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnLoadDesenho.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnLoadDesenho.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadDesenho.Image")));
      this.btnLoadDesenho.Location = new System.Drawing.Point(3, 5);
      this.btnLoadDesenho.Margin = new System.Windows.Forms.Padding(1);
      this.btnLoadDesenho.Name = "btnLoadDesenho";
      this.btnLoadDesenho.Size = new System.Drawing.Size(31, 31);
      this.btnLoadDesenho.TabIndex = 0;
      this.btnLoadDesenho.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnLoadDesenho, "Carrgear");
      this.btnLoadDesenho.UseVisualStyleBackColor = false;
      this.btnLoadDesenho.Click += new System.EventHandler(this.BtnLoadDesenho_Click);
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
      this.dgv.Location = new System.Drawing.Point(0, 164);
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
      this.dgv.Size = new System.Drawing.Size(350, 264);
      this.dgv.TabIndex = 1;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      // 
      // cmsOpenFile
      // 
      this.cmsOpenFile.Font = new System.Drawing.Font("Segoe UI", 12F);
      this.cmsOpenFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOpen3D,
            this.tsmOpen2D,
            this.tsmAttCodigo});
      this.cmsOpenFile.Name = "cmsOpenFile";
      this.cmsOpenFile.Size = new System.Drawing.Size(300, 82);
      // 
      // tsmOpen3D
      // 
      this.tsmOpen3D.Image = ((System.Drawing.Image)(resources.GetObject("tsmOpen3D.Image")));
      this.tsmOpen3D.Name = "tsmOpen3D";
      this.tsmOpen3D.Size = new System.Drawing.Size(299, 26);
      this.tsmOpen3D.Text = "Abrir 3D";
      this.tsmOpen3D.Click += new System.EventHandler(this.TsmOpen3D_Click);
      // 
      // tsmOpen2D
      // 
      this.tsmOpen2D.Image = ((System.Drawing.Image)(resources.GetObject("tsmOpen2D.Image")));
      this.tsmOpen2D.Name = "tsmOpen2D";
      this.tsmOpen2D.Size = new System.Drawing.Size(299, 26);
      this.tsmOpen2D.Text = "Abrir 2D";
      this.tsmOpen2D.Click += new System.EventHandler(this.TsmOpen2D_Click);
      // 
      // tsmAttCodigo
      // 
      this.tsmAttCodigo.Image = ((System.Drawing.Image)(resources.GetObject("tsmAttCodigo.Image")));
      this.tsmAttCodigo.Name = "tsmAttCodigo";
      this.tsmAttCodigo.Size = new System.Drawing.Size(299, 26);
      this.tsmAttCodigo.Text = "Atualizar Código (Componente)";
      this.tsmAttCodigo.Visible = false;
      this.tsmAttCodigo.Click += new System.EventHandler(this.TsmAttCodigo_Click);
      // 
      // FrmDesenho
      // 
      this.ClientSize = new System.Drawing.Size(350, 450);
      this.Controls.Add(this.dgv);
      this.Controls.Add(this.lmPanel1);
      this.Controls.Add(this.lblPercDesenho);
      this.HelpButton = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmDesenho";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.Resizable = false;
      this.ShowInTaskbar = false;
      this.Text = "Criar/Detalhar Desenhos";
      this.Loaded += new LmCorbieUI.LmForms.LmSingleForm.FormLoad(this.FrmDesenho_Loaded);
      this.ClickHelp += new LmCorbieUI.LmForms.LmSingleForm.ButClick(this.FrmDesenho_ClickHelp);
      this.lmPanel1.ResumeLayout(false);
      this.lmPanel1.PerformLayout();
      this.cmsOpenFile.ResumeLayout(false);
      this.ResumeLayout(false);

        }


        #endregion
        private LmCorbieUI.Controls.LmLabel lblPercDesenho;
        private LmCorbieUI.Controls.LmPanel lmPanel1;
        private LmCorbieUI.Controls.LmDataGridView dgv;
        private LmCorbieUI.Controls.LmButton btnLoadDesenho;
        private LmCorbieUI.Controls.LmCheckBox ckbPromov;
        private LmCorbieUI.Controls.LmTextBox txtName;
        private LmCorbieUI.Controls.LmButton btnA1;
        private LmCorbieUI.Controls.LmButton btnA2;
        private LmCorbieUI.Controls.LmButton btnA3;
        private LmCorbieUI.Controls.LmButton btnA4;
        private LmCorbieUI.Controls.LmButton btnA4R;
        private LmCorbieUI.Controls.LmButton btnInserirListaSoldagem;
        private LmCorbieUI.Controls.LmButton btnUpDes;
        private LmCorbieUI.Controls.LmButton btnDownDes;
        private LmCorbieUI.Controls.LmButton btnFFdesenho;
        private System.Windows.Forms.ContextMenuStrip cmsOpenFile;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen3D;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen2D;
        private System.Windows.Forms.ToolStripMenuItem tsmAttCodigo;
    private System.Windows.Forms.ToolTip cmxToolTip1;
  }
}