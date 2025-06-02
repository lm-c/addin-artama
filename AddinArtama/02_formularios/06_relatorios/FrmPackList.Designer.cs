namespace AddinArtama
{
    partial class FrmPackList
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPackList));
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
      this.txtData = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel3 = new LmCorbieUI.Controls.LmLabel();
      this.txtPedido = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.txtObs = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.txtDescricao = new LmCorbieUI.Controls.LmTextBox();
      this.cmxLabel4 = new LmCorbieUI.Controls.LmLabel();
      this.btnCarrProcess = new LmCorbieUI.Controls.LmButton();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.lmPanel1.SuspendLayout();
      this.SuspendLayout();
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
      this.dgv.Location = new System.Drawing.Point(0, 266);
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
      this.dgv.Size = new System.Drawing.Size(350, 224);
      this.dgv.TabIndex = 1;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      // 
      // lmPanel1
      // 
      this.lmPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel1.Controls.Add(this.txtData);
      this.lmPanel1.Controls.Add(this.lmLabel3);
      this.lmPanel1.Controls.Add(this.txtPedido);
      this.lmPanel1.Controls.Add(this.lmLabel2);
      this.lmPanel1.Controls.Add(this.txtObs);
      this.lmPanel1.Controls.Add(this.lmLabel1);
      this.lmPanel1.Controls.Add(this.txtDescricao);
      this.lmPanel1.Controls.Add(this.cmxLabel4);
      this.lmPanel1.Controls.Add(this.btnCarrProcess);
      this.lmPanel1.Controls.Add(this.btnSalvar);
      this.lmPanel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.lmPanel1.IsPanelMenu = false;
      this.lmPanel1.Location = new System.Drawing.Point(0, 30);
      this.lmPanel1.Name = "lmPanel1";
      this.lmPanel1.Size = new System.Drawing.Size(350, 236);
      this.lmPanel1.TabIndex = 0;
      // 
      // txtData
      // 
      this.txtData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtData.BorderRadius = 15;
      this.txtData.BorderSize = 2;
      this.txtData.F7ToolTipText = null;
      this.txtData.F8ToolTipText = null;
      this.txtData.F9ToolTipText = null;
      this.txtData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtData.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtData.IconF7")));
      this.txtData.IconToolTipText = null;
      this.txtData.Lines = new string[0];
      this.txtData.Location = new System.Drawing.Point(93, 60);
      this.txtData.MaxLength = 32767;
      this.txtData.Name = "txtData";
      this.txtData.PasswordChar = '\0';
      this.txtData.Propriedade = null;
      this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtData.SelectedText = "";
      this.txtData.SelectionLength = 0;
      this.txtData.SelectionStart = 0;
      this.txtData.ShortcutsEnabled = true;
      this.txtData.ShowButtonF7 = true;
      this.txtData.Size = new System.Drawing.Size(122, 31);
      this.txtData.TabIndex = 4;
      this.txtData.UnderlinedStyle = false;
      this.txtData.UseSelectable = true;
      this.txtData.Valor = LmCorbieUI.Design.LmValueType.Data;
      this.txtData.Valor_Decimais = ((short)(0));
      this.txtData.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtData.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel3
      // 
      this.lmLabel3.AutoSize = true;
      this.lmLabel3.Location = new System.Drawing.Point(93, 41);
      this.lmLabel3.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel3.Name = "lmLabel3";
      this.lmLabel3.Size = new System.Drawing.Size(38, 19);
      this.lmLabel3.TabIndex = 89;
      this.lmLabel3.Text = "Data";
      this.lmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // txtPedido
      // 
      this.txtPedido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtPedido.BorderRadius = 15;
      this.txtPedido.BorderSize = 2;
      this.txtPedido.F7ToolTipText = null;
      this.txtPedido.F8ToolTipText = null;
      this.txtPedido.F9ToolTipText = null;
      this.txtPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtPedido.IconF7 = null;
      this.txtPedido.IconToolTipText = null;
      this.txtPedido.Lines = new string[0];
      this.txtPedido.Location = new System.Drawing.Point(3, 60);
      this.txtPedido.MaxLength = 32767;
      this.txtPedido.Name = "txtPedido";
      this.txtPedido.PasswordChar = '\0';
      this.txtPedido.Propriedade = null;
      this.txtPedido.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtPedido.SelectedText = "";
      this.txtPedido.SelectionLength = 0;
      this.txtPedido.SelectionStart = 0;
      this.txtPedido.ShortcutsEnabled = true;
      this.txtPedido.Size = new System.Drawing.Size(84, 31);
      this.txtPedido.TabIndex = 3;
      this.txtPedido.UnderlinedStyle = false;
      this.txtPedido.UseSelectable = true;
      this.txtPedido.Valor_Decimais = ((short)(0));
      this.txtPedido.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtPedido.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel2
      // 
      this.lmLabel2.AutoSize = true;
      this.lmLabel2.Location = new System.Drawing.Point(3, 41);
      this.lmLabel2.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel2.Name = "lmLabel2";
      this.lmLabel2.Size = new System.Drawing.Size(50, 19);
      this.lmLabel2.TabIndex = 87;
      this.lmLabel2.Text = "Pedido";
      this.lmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // txtObs
      // 
      this.txtObs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtObs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtObs.BorderRadius = 15;
      this.txtObs.BorderSize = 2;
      this.txtObs.F7ToolTipText = null;
      this.txtObs.F8ToolTipText = null;
      this.txtObs.F9ToolTipText = null;
      this.txtObs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtObs.IconF7 = null;
      this.txtObs.IconToolTipText = null;
      this.txtObs.Lines = new string[0];
      this.txtObs.Location = new System.Drawing.Point(3, 166);
      this.txtObs.MaxLength = 32767;
      this.txtObs.Multiline = true;
      this.txtObs.Name = "txtObs";
      this.txtObs.PasswordChar = '\0';
      this.txtObs.Propriedade = null;
      this.txtObs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtObs.SelectedText = "";
      this.txtObs.SelectionLength = 0;
      this.txtObs.SelectionStart = 0;
      this.txtObs.ShortcutsEnabled = true;
      this.txtObs.Size = new System.Drawing.Size(338, 58);
      this.txtObs.TabIndex = 6;
      this.txtObs.UnderlinedStyle = false;
      this.txtObs.UseSelectable = true;
      this.txtObs.Valor_Decimais = ((short)(0));
      this.txtObs.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtObs.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel1
      // 
      this.lmLabel1.AutoSize = true;
      this.lmLabel1.Location = new System.Drawing.Point(3, 147);
      this.lmLabel1.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel1.Name = "lmLabel1";
      this.lmLabel1.Size = new System.Drawing.Size(81, 19);
      this.lmLabel1.TabIndex = 85;
      this.lmLabel1.Text = "Observação";
      this.lmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
      this.txtDescricao.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtDescricao.IconF7")));
      this.txtDescricao.IconToolTipText = null;
      this.txtDescricao.Lines = new string[0];
      this.txtDescricao.Location = new System.Drawing.Point(3, 113);
      this.txtDescricao.MaxLength = 32767;
      this.txtDescricao.Name = "txtDescricao";
      this.txtDescricao.PasswordChar = '\0';
      this.txtDescricao.Propriedade = null;
      this.txtDescricao.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtDescricao.SelectedText = "";
      this.txtDescricao.SelectionLength = 0;
      this.txtDescricao.SelectionStart = 0;
      this.txtDescricao.ShortcutsEnabled = true;
      this.txtDescricao.ShowButtonF7 = true;
      this.txtDescricao.Size = new System.Drawing.Size(338, 31);
      this.txtDescricao.TabIndex = 5;
      this.txtDescricao.UnderlinedStyle = false;
      this.txtDescricao.UseSelectable = true;
      this.txtDescricao.Valor_Decimais = ((short)(0));
      this.txtDescricao.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtDescricao.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      this.txtDescricao.ButtonClickF7 += new LmCorbieUI.Controls.LmTextBox.ButClick(this.TxtDescricao_ButtonClickF7);
      // 
      // cmxLabel4
      // 
      this.cmxLabel4.AutoSize = true;
      this.cmxLabel4.Location = new System.Drawing.Point(3, 94);
      this.cmxLabel4.Margin = new System.Windows.Forms.Padding(3);
      this.cmxLabel4.Name = "cmxLabel4";
      this.cmxLabel4.Size = new System.Drawing.Size(67, 19);
      this.cmxLabel4.TabIndex = 82;
      this.cmxLabel4.Text = "Descrição";
      this.cmxLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
      this.btnCarrProcess.Location = new System.Drawing.Point(3, 6);
      this.btnCarrProcess.Margin = new System.Windows.Forms.Padding(1);
      this.btnCarrProcess.Name = "btnCarrProcess";
      this.btnCarrProcess.Size = new System.Drawing.Size(31, 31);
      this.btnCarrProcess.TabIndex = 0;
      this.btnCarrProcess.UseVisualStyleBackColor = false;
      this.btnCarrProcess.Click += new System.EventHandler(this.BtnCarrProcess_Click);
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
      this.btnSalvar.Location = new System.Drawing.Point(36, 6);
      this.btnSalvar.Margin = new System.Windows.Forms.Padding(1);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(31, 31);
      this.btnSalvar.TabIndex = 1;
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
      // 
      // FrmPackList
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(350, 490);
      this.Controls.Add(this.dgv);
      this.Controls.Add(this.lmPanel1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Movimentar = false;
      this.Name = "FrmPackList";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.Resizable = false;
      this.Text = "Pack List";
      this.lmPanel1.ResumeLayout(false);
      this.lmPanel1.PerformLayout();
      this.ResumeLayout(false);

        }

        #endregion

        private LmCorbieUI.Controls.LmDataGridView dgv;
        private LmCorbieUI.Controls.LmPanel lmPanel1;
        private LmCorbieUI.Controls.LmTextBox txtPedido;
        private LmCorbieUI.Controls.LmLabel lmLabel2;
        private LmCorbieUI.Controls.LmTextBox txtObs;
        private LmCorbieUI.Controls.LmLabel lmLabel1;
        private LmCorbieUI.Controls.LmTextBox txtDescricao;
        private LmCorbieUI.Controls.LmLabel cmxLabel4;
        private LmCorbieUI.Controls.LmButton btnCarrProcess;
        private LmCorbieUI.Controls.LmButton btnSalvar;
    private LmCorbieUI.Controls.LmTextBox txtData;
    private LmCorbieUI.Controls.LmLabel lmLabel3;
  }
}