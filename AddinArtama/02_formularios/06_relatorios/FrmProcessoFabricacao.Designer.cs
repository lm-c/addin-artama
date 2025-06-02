namespace AddinArtama
{
    partial class FrmProcessoFabricacao
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProcessoFabricacao));
      this.pnlControls = new LmCorbieUI.Controls.LmPanel();
      this.txtQtd = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel3 = new LmCorbieUI.Controls.LmLabel();
      this.txtPedido = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.btnCarrProcess = new LmCorbieUI.Controls.LmButton();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.dgv = new LmCorbieUI.Controls.LmDataGridView();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.pnlControls.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlControls
      // 
      this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.pnlControls.Controls.Add(this.txtQtd);
      this.pnlControls.Controls.Add(this.lmLabel3);
      this.pnlControls.Controls.Add(this.txtPedido);
      this.pnlControls.Controls.Add(this.lmLabel2);
      this.pnlControls.Controls.Add(this.btnCarrProcess);
      this.pnlControls.Controls.Add(this.btnSalvar);
      this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlControls.IsPanelMenu = false;
      this.pnlControls.Location = new System.Drawing.Point(0, 30);
      this.pnlControls.Name = "pnlControls";
      this.pnlControls.Size = new System.Drawing.Size(350, 108);
      this.pnlControls.TabIndex = 0;
      // 
      // txtQtd
      // 
      this.txtQtd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtQtd.BorderRadius = 15;
      this.txtQtd.BorderSize = 2;
      this.txtQtd.CampoObrigatorio = true;
      this.txtQtd.F7ToolTipText = null;
      this.txtQtd.F8ToolTipText = null;
      this.txtQtd.F9ToolTipText = null;
      this.txtQtd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtQtd.IconF7 = null;
      this.txtQtd.IconToolTipText = null;
      this.txtQtd.Lines = new string[0];
      this.txtQtd.Location = new System.Drawing.Point(129, 60);
      this.txtQtd.MaxLength = 32767;
      this.txtQtd.Name = "txtQtd";
      this.txtQtd.PasswordChar = '\0';
      this.txtQtd.Propriedade = null;
      this.txtQtd.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtQtd.SelectedText = "";
      this.txtQtd.SelectionLength = 0;
      this.txtQtd.SelectionStart = 0;
      this.txtQtd.ShortcutsEnabled = true;
      this.txtQtd.Size = new System.Drawing.Size(100, 31);
      this.txtQtd.TabIndex = 4;
      this.txtQtd.UnderlinedStyle = false;
      this.txtQtd.UseSelectable = true;
      this.txtQtd.Valor = LmCorbieUI.Design.LmValueType.Num_Inteiro;
      this.txtQtd.Valor_Decimais = ((short)(0));
      this.txtQtd.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtQtd.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel3
      // 
      this.lmLabel3.AutoSize = true;
      this.lmLabel3.Location = new System.Drawing.Point(129, 41);
      this.lmLabel3.Margin = new System.Windows.Forms.Padding(3);
      this.lmLabel3.Name = "lmLabel3";
      this.lmLabel3.Size = new System.Drawing.Size(33, 19);
      this.lmLabel3.TabIndex = 89;
      this.lmLabel3.Text = "Qtd";
      this.lmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // txtPedido
      // 
      this.txtPedido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtPedido.BorderRadius = 15;
      this.txtPedido.BorderSize = 2;
      this.txtPedido.CampoObrigatorio = true;
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
      this.txtPedido.Size = new System.Drawing.Size(120, 31);
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
      this.dgv.Location = new System.Drawing.Point(0, 138);
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
      this.dgv.Size = new System.Drawing.Size(350, 312);
      this.dgv.TabIndex = 84;
      this.dgv.Texto = "";
      this.dgv.TituloRelatorio = "";
      this.dgv.UseSelectable = true;
      // 
      // FrmProcessoFabricacao
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(350, 450);
      this.Controls.Add(this.dgv);
      this.Controls.Add(this.pnlControls);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmProcessoFabricacao";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.Resizable = false;
      this.Text = "Processo de Fabricação";
      this.pnlControls.ResumeLayout(false);
      this.pnlControls.PerformLayout();
      this.ResumeLayout(false);

        }

        #endregion
        private LmCorbieUI.Controls.LmButton btnSalvar;
        private LmCorbieUI.Controls.LmButton btnCarrProcess;
        private LmCorbieUI.Controls.LmPanel pnlControls;
        private LmCorbieUI.Controls.LmTextBox txtQtd;
        private LmCorbieUI.Controls.LmLabel lmLabel3;
        private LmCorbieUI.Controls.LmTextBox txtPedido;
        private LmCorbieUI.Controls.LmLabel lmLabel2;
        private LmCorbieUI.Controls.LmDataGridView dgv;
        private System.Windows.Forms.ToolTip cmxToolTip1;
    }
}