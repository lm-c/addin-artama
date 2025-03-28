namespace AddinArtama
{
    partial class FrmFileProperties
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFileProperties));
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.dtpDataDesenho = new LmCorbieUI.Controls.LmTextBox();
      this.dtpDataProjeto = new LmCorbieUI.Controls.LmTextBox();
      this.cmbDesenhista = new LmCorbieUI.Controls.LmTextBox();
      this.cmbProjetista = new LmCorbieUI.Controls.LmTextBox();
      this.cmxGroupBox1 = new LmCorbieUI.Controls.LmGroupBox();
      this.rdbPasta = new LmCorbieUI.Controls.LmRadioButton();
      this.txtNome = new LmCorbieUI.Controls.LmTextBox();
      this.rdbNome = new LmCorbieUI.Controls.LmRadioButton();
      this.cmxLabel4 = new LmCorbieUI.Controls.LmLabel();
      this.cmxLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.cmxLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.cmxLabel3 = new LmCorbieUI.Controls.LmLabel();
      this.cmxGroupBox1.SuspendLayout();
      this.SuspendLayout();
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
      this.btnSalvar.Location = new System.Drawing.Point(3, 54);
      this.btnSalvar.Margin = new System.Windows.Forms.Padding(1);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(31, 31);
      this.btnSalvar.TabIndex = 6;
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.btnSalvar, "Aplicar Propriedades");
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
      // 
      // dtpDataDesenho
      // 
      this.dtpDataDesenho.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.dtpDataDesenho.BorderRadius = 15;
      this.dtpDataDesenho.BorderSize = 2;
      this.dtpDataDesenho.F7ToolTipText = null;
      this.dtpDataDesenho.F8ToolTipText = null;
      this.dtpDataDesenho.F9ToolTipText = null;
      this.dtpDataDesenho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.dtpDataDesenho.IconF7 = ((System.Drawing.Image)(resources.GetObject("dtpDataDesenho.IconF7")));
      this.dtpDataDesenho.IconToolTipText = null;
      this.dtpDataDesenho.Lines = new string[0];
      this.dtpDataDesenho.Location = new System.Drawing.Point(3, 284);
      this.dtpDataDesenho.MaxLength = 32767;
      this.dtpDataDesenho.Name = "dtpDataDesenho";
      this.dtpDataDesenho.PasswordChar = '\0';
      this.dtpDataDesenho.Propriedade = null;
      this.dtpDataDesenho.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.dtpDataDesenho.SelectedText = "";
      this.dtpDataDesenho.SelectionLength = 0;
      this.dtpDataDesenho.SelectionStart = 0;
      this.dtpDataDesenho.ShortcutsEnabled = true;
      this.dtpDataDesenho.ShowButtonF7 = true;
      this.dtpDataDesenho.Size = new System.Drawing.Size(130, 31);
      this.dtpDataDesenho.TabIndex = 75;
      this.dtpDataDesenho.UnderlinedStyle = false;
      this.dtpDataDesenho.UseSelectable = true;
      this.dtpDataDesenho.Valor = LmCorbieUI.Design.LmValueType.Data;
      this.dtpDataDesenho.Valor_Decimais = ((short)(0));
      this.dtpDataDesenho.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.dtpDataDesenho.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // dtpDataProjeto
      // 
      this.dtpDataProjeto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.dtpDataProjeto.BorderRadius = 15;
      this.dtpDataProjeto.BorderSize = 2;
      this.dtpDataProjeto.F7ToolTipText = null;
      this.dtpDataProjeto.F8ToolTipText = null;
      this.dtpDataProjeto.F9ToolTipText = null;
      this.dtpDataProjeto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.dtpDataProjeto.IconF7 = ((System.Drawing.Image)(resources.GetObject("dtpDataProjeto.IconF7")));
      this.dtpDataProjeto.IconToolTipText = null;
      this.dtpDataProjeto.Lines = new string[0];
      this.dtpDataProjeto.Location = new System.Drawing.Point(3, 175);
      this.dtpDataProjeto.MaxLength = 32767;
      this.dtpDataProjeto.Name = "dtpDataProjeto";
      this.dtpDataProjeto.PasswordChar = '\0';
      this.dtpDataProjeto.Propriedade = null;
      this.dtpDataProjeto.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.dtpDataProjeto.SelectedText = "";
      this.dtpDataProjeto.SelectionLength = 0;
      this.dtpDataProjeto.SelectionStart = 0;
      this.dtpDataProjeto.ShortcutsEnabled = true;
      this.dtpDataProjeto.ShowButtonF7 = true;
      this.dtpDataProjeto.Size = new System.Drawing.Size(130, 31);
      this.dtpDataProjeto.TabIndex = 73;
      this.dtpDataProjeto.UnderlinedStyle = false;
      this.dtpDataProjeto.UseSelectable = true;
      this.dtpDataProjeto.Valor = LmCorbieUI.Design.LmValueType.Data;
      this.dtpDataProjeto.Valor_Decimais = ((short)(0));
      this.dtpDataProjeto.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.dtpDataProjeto.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // cmbDesenhista
      // 
      this.cmbDesenhista.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbDesenhista.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.cmbDesenhista.BorderRadius = 15;
      this.cmbDesenhista.BorderSize = 2;
      this.cmbDesenhista.F7ToolTipText = null;
      this.cmbDesenhista.F8ToolTipText = null;
      this.cmbDesenhista.F9ToolTipText = null;
      this.cmbDesenhista.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.cmbDesenhista.IconF7 = ((System.Drawing.Image)(resources.GetObject("cmbDesenhista.IconF7")));
      this.cmbDesenhista.IconToolTipText = null;
      this.cmbDesenhista.Lines = new string[0];
      this.cmbDesenhista.Location = new System.Drawing.Point(3, 229);
      this.cmbDesenhista.MaxLength = 32767;
      this.cmbDesenhista.Name = "cmbDesenhista";
      this.cmbDesenhista.PasswordChar = '\0';
      this.cmbDesenhista.Propriedade = null;
      this.cmbDesenhista.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.cmbDesenhista.SelectedText = "";
      this.cmbDesenhista.SelectionLength = 0;
      this.cmbDesenhista.SelectionStart = 0;
      this.cmbDesenhista.ShortcutsEnabled = true;
      this.cmbDesenhista.ShowButtonF7 = true;
      this.cmbDesenhista.Size = new System.Drawing.Size(338, 31);
      this.cmbDesenhista.TabIndex = 74;
      this.cmbDesenhista.UnderlinedStyle = false;
      this.cmbDesenhista.UseSelectable = true;
      this.cmbDesenhista.Valor = LmCorbieUI.Design.LmValueType.ComboBox;
      this.cmbDesenhista.Valor_Decimais = ((short)(0));
      this.cmbDesenhista.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.cmbDesenhista.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // cmbProjetista
      // 
      this.cmbProjetista.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbProjetista.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.cmbProjetista.BorderRadius = 15;
      this.cmbProjetista.BorderSize = 2;
      this.cmbProjetista.F7ToolTipText = null;
      this.cmbProjetista.F8ToolTipText = null;
      this.cmbProjetista.F9ToolTipText = null;
      this.cmbProjetista.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.cmbProjetista.IconF7 = ((System.Drawing.Image)(resources.GetObject("cmbProjetista.IconF7")));
      this.cmbProjetista.IconToolTipText = null;
      this.cmbProjetista.Lines = new string[0];
      this.cmbProjetista.Location = new System.Drawing.Point(3, 120);
      this.cmbProjetista.MaxLength = 32767;
      this.cmbProjetista.Name = "cmbProjetista";
      this.cmbProjetista.PasswordChar = '\0';
      this.cmbProjetista.Propriedade = null;
      this.cmbProjetista.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.cmbProjetista.SelectedText = "";
      this.cmbProjetista.SelectionLength = 0;
      this.cmbProjetista.SelectionStart = 0;
      this.cmbProjetista.ShortcutsEnabled = true;
      this.cmbProjetista.ShowButtonF7 = true;
      this.cmbProjetista.Size = new System.Drawing.Size(338, 31);
      this.cmbProjetista.TabIndex = 72;
      this.cmbProjetista.UnderlinedStyle = false;
      this.cmbProjetista.UseSelectable = true;
      this.cmbProjetista.Valor = LmCorbieUI.Design.LmValueType.ComboBox;
      this.cmbProjetista.Valor_Decimais = ((short)(0));
      this.cmbProjetista.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.cmbProjetista.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // cmxGroupBox1
      // 
      this.cmxGroupBox1.Controls.Add(this.rdbPasta);
      this.cmxGroupBox1.Controls.Add(this.txtNome);
      this.cmxGroupBox1.Controls.Add(this.rdbNome);
      this.cmxGroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(41)))), ((int)(((byte)(38)))));
      this.cmxGroupBox1.Location = new System.Drawing.Point(3, 319);
      this.cmxGroupBox1.Name = "cmxGroupBox1";
      this.cmxGroupBox1.Size = new System.Drawing.Size(344, 90);
      this.cmxGroupBox1.TabIndex = 76;
      this.cmxGroupBox1.TabStop = false;
      this.cmxGroupBox1.Text = "Atualizar Somente";
      // 
      // rdbPasta
      // 
      this.rdbPasta.AutoSize = true;
      this.rdbPasta.Checked = true;
      this.rdbPasta.Location = new System.Drawing.Point(6, 25);
      this.rdbPasta.Name = "rdbPasta";
      this.rdbPasta.Size = new System.Drawing.Size(189, 15);
      this.rdbPasta.TabIndex = 0;
      this.rdbPasta.TabStop = true;
      this.rdbPasta.Text = "Na Mesma Pasta da Montagem";
      this.rdbPasta.UseSelectable = true;
      // 
      // txtNome
      // 
      this.txtNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtNome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtNome.BorderRadius = 15;
      this.txtNome.BorderSize = 2;
      this.txtNome.F7ToolTipText = null;
      this.txtNome.F8ToolTipText = null;
      this.txtNome.F9ToolTipText = null;
      this.txtNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtNome.IconF7 = null;
      this.txtNome.IconToolTipText = null;
      this.txtNome.Lines = new string[0];
      this.txtNome.Location = new System.Drawing.Point(144, 49);
      this.txtNome.MaxLength = 32767;
      this.txtNome.Name = "txtNome";
      this.txtNome.PasswordChar = '\0';
      this.txtNome.Propriedade = null;
      this.txtNome.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtNome.SelectedText = "";
      this.txtNome.SelectionLength = 0;
      this.txtNome.SelectionStart = 0;
      this.txtNome.ShortcutsEnabled = true;
      this.txtNome.Size = new System.Drawing.Size(194, 31);
      this.txtNome.TabIndex = 2;
      this.txtNome.UnderlinedStyle = false;
      this.txtNome.UseSelectable = true;
      this.txtNome.Valor_Decimais = ((short)(0));
      this.txtNome.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtNome.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // rdbNome
      // 
      this.rdbNome.AutoSize = true;
      this.rdbNome.Location = new System.Drawing.Point(6, 57);
      this.rdbNome.Name = "rdbNome";
      this.rdbNome.Size = new System.Drawing.Size(132, 15);
      this.rdbNome.TabIndex = 1;
      this.rdbNome.Text = "Nome Começa Com";
      this.rdbNome.UseSelectable = true;
      // 
      // cmxLabel4
      // 
      this.cmxLabel4.AutoSize = true;
      this.cmxLabel4.Location = new System.Drawing.Point(3, 101);
      this.cmxLabel4.Margin = new System.Windows.Forms.Padding(3);
      this.cmxLabel4.Name = "cmxLabel4";
      this.cmxLabel4.Size = new System.Drawing.Size(66, 19);
      this.cmxLabel4.TabIndex = 77;
      this.cmxLabel4.Text = "Projetista";
      this.cmxLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // cmxLabel1
      // 
      this.cmxLabel1.AutoSize = true;
      this.cmxLabel1.Location = new System.Drawing.Point(3, 210);
      this.cmxLabel1.Margin = new System.Windows.Forms.Padding(3);
      this.cmxLabel1.Name = "cmxLabel1";
      this.cmxLabel1.Size = new System.Drawing.Size(76, 19);
      this.cmxLabel1.TabIndex = 78;
      this.cmxLabel1.Text = "Desenhista";
      this.cmxLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // cmxLabel2
      // 
      this.cmxLabel2.AutoSize = true;
      this.cmxLabel2.Location = new System.Drawing.Point(3, 265);
      this.cmxLabel2.Margin = new System.Windows.Forms.Padding(3);
      this.cmxLabel2.Name = "cmxLabel2";
      this.cmxLabel2.Size = new System.Drawing.Size(96, 19);
      this.cmxLabel2.TabIndex = 80;
      this.cmxLabel2.Text = "Data Desenho";
      this.cmxLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // cmxLabel3
      // 
      this.cmxLabel3.AutoSize = true;
      this.cmxLabel3.Location = new System.Drawing.Point(3, 156);
      this.cmxLabel3.Margin = new System.Windows.Forms.Padding(3);
      this.cmxLabel3.Name = "cmxLabel3";
      this.cmxLabel3.Size = new System.Drawing.Size(86, 19);
      this.cmxLabel3.TabIndex = 79;
      this.cmxLabel3.Text = "Data Projeto";
      this.cmxLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // FrmFileProperties
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(350, 518);
      this.Controls.Add(this.dtpDataDesenho);
      this.Controls.Add(this.dtpDataProjeto);
      this.Controls.Add(this.cmbDesenhista);
      this.Controls.Add(this.cmbProjetista);
      this.Controls.Add(this.cmxGroupBox1);
      this.Controls.Add(this.cmxLabel4);
      this.Controls.Add(this.cmxLabel1);
      this.Controls.Add(this.cmxLabel2);
      this.Controls.Add(this.cmxLabel3);
      this.Controls.Add(this.btnSalvar);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Movimentar = false;
      this.Name = "FrmFileProperties";
      this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
      this.Resizable = false;
      this.Text = "Propriedades Personalizadas";
      this.Loaded += new LmCorbieUI.LmForms.LmSingleForm.FormLoad(this.FrmFileProperties_Loaded);
      this.cmxGroupBox1.ResumeLayout(false);
      this.cmxGroupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion
        private LmCorbieUI.Controls.LmButton btnSalvar;
        private System.Windows.Forms.ToolTip cmxToolTip1;
        private LmCorbieUI.Controls.LmTextBox dtpDataDesenho;
        private LmCorbieUI.Controls.LmTextBox dtpDataProjeto;
        private LmCorbieUI.Controls.LmTextBox cmbDesenhista;
        private LmCorbieUI.Controls.LmTextBox cmbProjetista;
        private LmCorbieUI.Controls.LmGroupBox cmxGroupBox1;
        private LmCorbieUI.Controls.LmRadioButton rdbPasta;
        private LmCorbieUI.Controls.LmTextBox txtNome;
        private LmCorbieUI.Controls.LmRadioButton rdbNome;
        private LmCorbieUI.Controls.LmLabel cmxLabel4;
        private LmCorbieUI.Controls.LmLabel cmxLabel1;
        private LmCorbieUI.Controls.LmLabel cmxLabel2;
        private LmCorbieUI.Controls.LmLabel cmxLabel3;
  }
}