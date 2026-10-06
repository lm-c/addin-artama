namespace AddinCorbieArtama.VIEW
{
    partial class FrmReportWorks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReportWorks));
            this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
            this.btnSalvar = new LmCorbieUI.Controls.LmButton();
            this.btnAbrir = new LmCorbieUI.Controls.LmButton();
            this.btnLoad = new LmCorbieUI.Controls.LmButton();
            this.lmPanel3 = new LmCorbieUI.Controls.LmPanel();
            this.btnSubstituir = new LmCorbieUI.Controls.LmButton();
            this.txtPara = new LmCorbieUI.Controls.LmTextBox();
            this.txtDe = new LmCorbieUI.Controls.LmTextBox();
            this.txtSeriado = new LmCorbieUI.Controls.LmTextBox();
            this.lmLabel5 = new LmCorbieUI.Controls.LmLabel();
            this.lmLabel3 = new LmCorbieUI.Controls.LmLabel();
            this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
            this.dgv = new LmCorbieUI.Controls.LmDataGridView();
            this.cmsOpenFile = new LmCorbieUI.Controls.LmDropdownMenu(this.components);
            this.tsmAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAltDados = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOpen3D = new System.Windows.Forms.ToolStripMenuItem();
            this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
            this.rdbUtf8 = new LmCorbieUI.Controls.LmRadioButton();
            this.rdbIso = new LmCorbieUI.Controls.LmRadioButton();
            this.lmPanel1.SuspendLayout();
            this.lmPanel3.SuspendLayout();
            this.cmsOpenFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // lmPanel1
            // 
            this.lmPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(215)))), ((int)(((byte)(242)))));
            this.lmPanel1.Controls.Add(this.btnSalvar);
            this.lmPanel1.Controls.Add(this.btnAbrir);
            this.lmPanel1.Controls.Add(this.btnLoad);
            this.lmPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lmPanel1.IsPanelMenu = false;
            this.lmPanel1.Location = new System.Drawing.Point(2, 30);
            this.lmPanel1.Name = "lmPanel1";
            this.lmPanel1.Size = new System.Drawing.Size(996, 41);
            this.lmPanel1.TabIndex = 0;
            // 
            // btnSalvar
            // 
            this.btnSalvar.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnSalvar.BorderRadius = 13;
            this.btnSalvar.BorderSize = 0;
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.Image = ((System.Drawing.Image)(resources.GetObject("btnSalvar.Image")));
            this.btnSalvar.Location = new System.Drawing.Point(235, 6);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(110, 26);
            this.btnSalvar.TabIndex = 2;
            this.btnSalvar.Text = " Salvar CSV";
            this.btnSalvar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
            // 
            // btnAbrir
            // 
            this.btnAbrir.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnAbrir.BorderRadius = 13;
            this.btnAbrir.BorderSize = 0;
            this.btnAbrir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrir.Image = ((System.Drawing.Image)(resources.GetObject("btnAbrir.Image")));
            this.btnAbrir.Location = new System.Drawing.Point(119, 6);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(110, 26);
            this.btnAbrir.TabIndex = 1;
            this.btnAbrir.Text = " Abrir CSV";
            this.btnAbrir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAbrir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAbrir.UseVisualStyleBackColor = false;
            this.btnAbrir.Click += new System.EventHandler(this.BtnAbrir_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnLoad.BorderRadius = 13;
            this.btnLoad.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
            this.btnLoad.Location = new System.Drawing.Point(3, 6);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(110, 26);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = " Carregar";
            this.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // lmPanel3
            // 
            this.lmPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(215)))), ((int)(((byte)(242)))));
            this.lmPanel3.Controls.Add(this.btnSubstituir);
            this.lmPanel3.Controls.Add(this.txtPara);
            this.lmPanel3.Controls.Add(this.txtDe);
            this.lmPanel3.Controls.Add(this.txtSeriado);
            this.lmPanel3.Controls.Add(this.lmLabel5);
            this.lmPanel3.Controls.Add(this.lmLabel3);
            this.lmPanel3.Controls.Add(this.lmLabel2);
            this.lmPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lmPanel3.IsPanelMenu = false;
            this.lmPanel3.Location = new System.Drawing.Point(2, 455);
            this.lmPanel3.Name = "lmPanel3";
            this.lmPanel3.Size = new System.Drawing.Size(996, 43);
            this.lmPanel3.TabIndex = 2;
            // 
            // btnSubstituir
            // 
            this.btnSubstituir.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnSubstituir.BorderRadius = 13;
            this.btnSubstituir.BorderSize = 0;
            this.btnSubstituir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubstituir.Image = ((System.Drawing.Image)(resources.GetObject("btnSubstituir.Image")));
            this.btnSubstituir.Location = new System.Drawing.Point(613, 8);
            this.btnSubstituir.Name = "btnSubstituir";
            this.btnSubstituir.Size = new System.Drawing.Size(110, 26);
            this.btnSubstituir.TabIndex = 3;
            this.btnSubstituir.Text = " Substituir";
            this.btnSubstituir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSubstituir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSubstituir.UseVisualStyleBackColor = false;
            this.btnSubstituir.Click += new System.EventHandler(this.BtnSubstituir_Click);
            // 
            // txtPara
            // 
            this.txtPara.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(223)))), ((int)(((byte)(246)))));
            this.txtPara.BorderRadius = 15;
            this.txtPara.BorderSize = 2;
            this.txtPara.F7ToolTipText = null;
            this.txtPara.F8ToolTipText = null;
            this.txtPara.F9ToolTipText = null;
            this.txtPara.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtPara.IconF7 = null;
            this.txtPara.IconToolTipText = null;
            this.txtPara.Lines = new string[0];
            this.txtPara.Location = new System.Drawing.Point(533, 6);
            this.txtPara.MaxLength = 32767;
            this.txtPara.Name = "txtPara";
            this.txtPara.PasswordChar = '\0';
            this.txtPara.Propriedade = null;
            this.txtPara.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPara.SelectedText = "";
            this.txtPara.SelectionLength = 0;
            this.txtPara.SelectionStart = 0;
            this.txtPara.ShortcutsEnabled = true;
            this.txtPara.Size = new System.Drawing.Size(74, 30);
            this.txtPara.TabIndex = 2;
            this.txtPara.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPara.UnderlinedStyle = false;
            this.txtPara.UseSelectable = true;
            this.txtPara.Valor_Decimais = ((short)(0));
            this.txtPara.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.txtPara.WaterMarkFont = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            // 
            // txtDe
            // 
            this.txtDe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(223)))), ((int)(((byte)(246)))));
            this.txtDe.BorderRadius = 15;
            this.txtDe.BorderSize = 2;
            this.txtDe.F7ToolTipText = null;
            this.txtDe.F8ToolTipText = null;
            this.txtDe.F9ToolTipText = null;
            this.txtDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtDe.IconF7 = null;
            this.txtDe.IconToolTipText = null;
            this.txtDe.Lines = new string[0];
            this.txtDe.Location = new System.Drawing.Point(411, 6);
            this.txtDe.MaxLength = 32767;
            this.txtDe.Name = "txtDe";
            this.txtDe.PasswordChar = '\0';
            this.txtDe.Propriedade = null;
            this.txtDe.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtDe.SelectedText = "";
            this.txtDe.SelectionLength = 0;
            this.txtDe.SelectionStart = 0;
            this.txtDe.ShortcutsEnabled = true;
            this.txtDe.Size = new System.Drawing.Size(74, 30);
            this.txtDe.TabIndex = 1;
            this.txtDe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDe.UnderlinedStyle = false;
            this.txtDe.UseSelectable = true;
            this.txtDe.Valor_Decimais = ((short)(0));
            this.txtDe.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.txtDe.WaterMarkFont = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            // 
            // txtSeriado
            // 
            this.txtSeriado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(223)))), ((int)(((byte)(246)))));
            this.txtSeriado.BorderRadius = 15;
            this.txtSeriado.BorderSize = 2;
            this.txtSeriado.F7ToolTipText = null;
            this.txtSeriado.F8ToolTipText = null;
            this.txtSeriado.F9ToolTipText = null;
            this.txtSeriado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtSeriado.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtSeriado.IconF7")));
            this.txtSeriado.IconToolTipText = null;
            this.txtSeriado.Lines = new string[0];
            this.txtSeriado.Location = new System.Drawing.Point(68, 6);
            this.txtSeriado.MaxLength = 32767;
            this.txtSeriado.Name = "txtSeriado";
            this.txtSeriado.PasswordChar = '\0';
            this.txtSeriado.Propriedade = null;
            this.txtSeriado.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSeriado.SelectedText = "";
            this.txtSeriado.SelectionLength = 0;
            this.txtSeriado.SelectionStart = 0;
            this.txtSeriado.ShortcutsEnabled = true;
            this.txtSeriado.ShowButtonF7 = true;
            this.txtSeriado.Size = new System.Drawing.Size(161, 30);
            this.txtSeriado.TabIndex = 0;
            this.txtSeriado.UnderlinedStyle = false;
            this.txtSeriado.UseSelectable = true;
            this.txtSeriado.Valor = LmCorbieUI.Design.LmValueType.ComboBox_Enum;
            this.txtSeriado.Valor_Decimais = ((short)(0));
            this.txtSeriado.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.txtSeriado.WaterMarkFont = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.txtSeriado.SelectedValueChanched += new LmCorbieUI.Controls.LmTextBox.ValChange(this.TxtSeriado_SelectedValueChanched);
            // 
            // lmLabel5
            // 
            this.lmLabel5.AutoSize = true;
            this.lmLabel5.Location = new System.Drawing.Point(491, 12);
            this.lmLabel5.Margin = new System.Windows.Forms.Padding(3);
            this.lmLabel5.Name = "lmLabel5";
            this.lmLabel5.Size = new System.Drawing.Size(36, 19);
            this.lmLabel5.TabIndex = 7;
            this.lmLabel5.Text = "Para";
            // 
            // lmLabel3
            // 
            this.lmLabel3.AutoSize = true;
            this.lmLabel3.Location = new System.Drawing.Point(259, 12);
            this.lmLabel3.Margin = new System.Windows.Forms.Padding(3);
            this.lmLabel3.Name = "lmLabel3";
            this.lmLabel3.Size = new System.Drawing.Size(146, 19);
            this.lmLabel3.TabIndex = 5;
            this.lmLabel3.Text = "Substituir Processo De";
            // 
            // lmLabel2
            // 
            this.lmLabel2.AutoSize = true;
            this.lmLabel2.Location = new System.Drawing.Point(8, 12);
            this.lmLabel2.Margin = new System.Windows.Forms.Padding(3);
            this.lmLabel2.Name = "lmLabel2";
            this.lmLabel2.Size = new System.Drawing.Size(54, 19);
            this.lmLabel2.TabIndex = 4;
            this.lmLabel2.Text = "Seriado";
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
            this.dgv.Location = new System.Drawing.Point(2, 71);
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
            this.dgv.Size = new System.Drawing.Size(996, 384);
            this.dgv.TabIndex = 1;
            this.dgv.Texto = "";
            this.dgv.TituloRelatorio = "";
            this.dgv.UseSelectable = true;
            this.dgv.ProcurarTextChanged += new LmCorbieUI.Controls.LmDataGridView.TxtChange(this.Dgv_ProcurarTextChanged);
            this.dgv.KeyDowm += new LmCorbieUI.Controls.LmDataGridView.KeyEvent(this.Dgv_KeyDown);
            // 
            // cmsOpenFile
            // 
            this.cmsOpenFile.IsMainMenu = false;
            this.cmsOpenFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAdd,
            this.tsmDelete,
            this.tsmAltDados,
            this.tsmOpen3D});
            this.cmsOpenFile.MenuItemHeight = 25;
            this.cmsOpenFile.MenuItemTextColor = System.Drawing.Color.Empty;
            this.cmsOpenFile.Name = "cmsOpenFile";
            this.cmsOpenFile.NaoInverterCorImagem = false;
            this.cmsOpenFile.PrimaryColor = System.Drawing.Color.Empty;
            this.cmsOpenFile.Size = new System.Drawing.Size(223, 92);
            this.cmsOpenFile.Z_Teste = 0;
            // 
            // tsmAdd
            // 
            this.tsmAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsmAdd.Image")));
            this.tsmAdd.Name = "tsmAdd";
            this.tsmAdd.Size = new System.Drawing.Size(222, 22);
            this.tsmAdd.Text = "Inserir Registro";
            this.tsmAdd.Click += new System.EventHandler(this.TsmAdd_Click);
            // 
            // tsmDelete
            // 
            this.tsmDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsmDelete.Image")));
            this.tsmDelete.Name = "tsmDelete";
            this.tsmDelete.Size = new System.Drawing.Size(222, 22);
            this.tsmDelete.Text = "Excluir Registro";
            this.tsmDelete.Click += new System.EventHandler(this.TsmDelete_Click);
            // 
            // tsmAltDados
            // 
            this.tsmAltDados.Image = ((System.Drawing.Image)(resources.GetObject("tsmAltDados.Image")));
            this.tsmAltDados.Name = "tsmAltDados";
            this.tsmAltDados.Size = new System.Drawing.Size(222, 22);
            this.tsmAltDados.Text = "Salvar Dados No SolidWorks";
            this.tsmAltDados.Click += new System.EventHandler(this.TsmAltDados_Click);
            // 
            // tsmOpen3D
            // 
            this.tsmOpen3D.Image = ((System.Drawing.Image)(resources.GetObject("tsmOpen3D.Image")));
            this.tsmOpen3D.Name = "tsmOpen3D";
            this.tsmOpen3D.Size = new System.Drawing.Size(222, 22);
            this.tsmOpen3D.Text = "Abrir 3D";
            this.tsmOpen3D.Click += new System.EventHandler(this.TsmOpen3D_Click);
            // 
            // lmLabel1
            // 
            this.lmLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lmLabel1.AutoSize = true;
            this.lmLabel1.Location = new System.Drawing.Point(480, 425);
            this.lmLabel1.Margin = new System.Windows.Forms.Padding(3);
            this.lmLabel1.Name = "lmLabel1";
            this.lmLabel1.Size = new System.Drawing.Size(59, 19);
            this.lmLabel1.TabIndex = 3;
            this.lmLabel1.Text = "Unicode";
            // 
            // rdbUtf8
            // 
            this.rdbUtf8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbUtf8.AutoSize = true;
            this.rdbUtf8.Checked = true;
            this.rdbUtf8.Location = new System.Drawing.Point(546, 427);
            this.rdbUtf8.Name = "rdbUtf8";
            this.rdbUtf8.Size = new System.Drawing.Size(54, 15);
            this.rdbUtf8.TabIndex = 4;
            this.rdbUtf8.TabStop = true;
            this.rdbUtf8.Text = "UTF-8";
            this.rdbUtf8.UseSelectable = true;
            this.rdbUtf8.CheckedChanged += new System.EventHandler(this.RdbUtf8_CheckedChanged);
            // 
            // rdbIso
            // 
            this.rdbIso.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbIso.AutoSize = true;
            this.rdbIso.Location = new System.Drawing.Point(606, 427);
            this.rdbIso.Name = "rdbIso";
            this.rdbIso.Size = new System.Drawing.Size(81, 15);
            this.rdbIso.TabIndex = 5;
            this.rdbIso.Text = "ISO-8859-1";
            this.rdbIso.UseSelectable = true;
            this.rdbIso.CheckedChanged += new System.EventHandler(this.RdbIso_CheckedChanged);
            // 
            // FrmReportWorks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Controls.Add(this.rdbIso);
            this.Controls.Add(this.rdbUtf8);
            this.Controls.Add(this.lmLabel1);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.lmPanel3);
            this.Controls.Add(this.lmPanel1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FrmReportWorks";
            this.Padding = new System.Windows.Forms.Padding(2, 30, 2, 2);
            this.Text = "Report Works";
            this.Loaded += new LmCorbieUI.LmForms.LmSingleForm.FormLoad(this.FrmReportWorks_Loaded);
            this.Load += new System.EventHandler(this.FrmReportWorks_Load);
            this.lmPanel1.ResumeLayout(false);
            this.lmPanel3.ResumeLayout(false);
            this.lmPanel3.PerformLayout();
            this.cmsOpenFile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LmCorbieUI.Controls.LmPanel lmPanel1;
        private LmCorbieUI.Controls.LmPanel lmPanel3;
        private LmCorbieUI.Controls.LmDataGridView dgv;
        private LmCorbieUI.Controls.LmButton btnSalvar;
        private LmCorbieUI.Controls.LmButton btnAbrir;
        private LmCorbieUI.Controls.LmButton btnLoad;
        private LmCorbieUI.Controls.LmLabel lmLabel1;
        private LmCorbieUI.Controls.LmRadioButton rdbUtf8;
        private LmCorbieUI.Controls.LmRadioButton rdbIso;
        private LmCorbieUI.Controls.LmLabel lmLabel2;
        private LmCorbieUI.Controls.LmLabel lmLabel5;
        private LmCorbieUI.Controls.LmLabel lmLabel3;
        private LmCorbieUI.Controls.LmTextBox txtSeriado;
        private LmCorbieUI.Controls.LmTextBox txtPara;
        private LmCorbieUI.Controls.LmTextBox txtDe;
        private LmCorbieUI.Controls.LmButton btnSubstituir;
        private LmCorbieUI.Controls.LmDropdownMenu cmsOpenFile;
        private System.Windows.Forms.ToolStripMenuItem tsmAdd;
        private System.Windows.Forms.ToolStripMenuItem tsmDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmAltDados;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen3D;
    }
}