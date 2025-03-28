namespace AddinArtama {
  partial class FrmAlterarSenha {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAlterarSenha));
      this.txtSenhaAtual = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel2 = new LmCorbieUI.Controls.LmLabel();
      this.txtSenhaNova = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel1 = new LmCorbieUI.Controls.LmLabel();
      this.txtSenhaRepetir = new LmCorbieUI.Controls.LmTextBox();
      this.lmLabel3 = new LmCorbieUI.Controls.LmLabel();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.SuspendLayout();
      // 
      // txtSenhaAtual
      // 
      this.txtSenhaAtual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSenhaAtual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtSenhaAtual.BorderRadius = 15;
      this.txtSenhaAtual.BorderSize = 2;
      this.txtSenhaAtual.CampoObrigatorio = true;
      this.txtSenhaAtual.F7ToolTipText = null;
      this.txtSenhaAtual.F8ToolTipText = null;
      this.txtSenhaAtual.F9ToolTipText = null;
      this.txtSenhaAtual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtSenhaAtual.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtSenhaAtual.IconF7")));
      this.txtSenhaAtual.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtSenhaAtual.IconF8")));
      this.txtSenhaAtual.IconToolTipText = null;
      this.txtSenhaAtual.Lines = new string[0];
      this.txtSenhaAtual.Location = new System.Drawing.Point(15, 72);
      this.txtSenhaAtual.MaxLength = 100;
      this.txtSenhaAtual.Name = "txtSenhaAtual";
      this.txtSenhaAtual.PasswordChar = '●';
      this.txtSenhaAtual.Propriedade = null;
      this.txtSenhaAtual.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtSenhaAtual.SelectedText = "";
      this.txtSenhaAtual.SelectionLength = 0;
      this.txtSenhaAtual.SelectionStart = 0;
      this.txtSenhaAtual.ShortcutsEnabled = true;
      this.txtSenhaAtual.Size = new System.Drawing.Size(312, 30);
      this.txtSenhaAtual.TabIndex = 0;
      this.txtSenhaAtual.UnderlinedStyle = false;
      this.txtSenhaAtual.UseSelectable = true;
      this.txtSenhaAtual.UseSystemPasswordChar = true;
      this.txtSenhaAtual.Valor_Decimais = ((short)(0));
      this.txtSenhaAtual.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtSenhaAtual.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel2
      // 
      this.lmLabel2.AutoSize = true;
      this.lmLabel2.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel2.Location = new System.Drawing.Point(15, 49);
      this.lmLabel2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel2.Name = "lmLabel2";
      this.lmLabel2.Size = new System.Drawing.Size(92, 19);
      this.lmLabel2.TabIndex = 355;
      this.lmLabel2.Text = "Senha Atual *";
      this.lmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtSenhaNova
      // 
      this.txtSenhaNova.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSenhaNova.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtSenhaNova.BorderRadius = 15;
      this.txtSenhaNova.BorderSize = 2;
      this.txtSenhaNova.CampoObrigatorio = true;
      this.txtSenhaNova.F7ToolTipText = null;
      this.txtSenhaNova.F8ToolTipText = null;
      this.txtSenhaNova.F9ToolTipText = null;
      this.txtSenhaNova.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtSenhaNova.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtSenhaNova.IconF7")));
      this.txtSenhaNova.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtSenhaNova.IconF8")));
      this.txtSenhaNova.IconToolTipText = null;
      this.txtSenhaNova.Lines = new string[0];
      this.txtSenhaNova.Location = new System.Drawing.Point(15, 131);
      this.txtSenhaNova.MaxLength = 100;
      this.txtSenhaNova.Name = "txtSenhaNova";
      this.txtSenhaNova.PasswordChar = '●';
      this.txtSenhaNova.Propriedade = null;
      this.txtSenhaNova.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtSenhaNova.SelectedText = "";
      this.txtSenhaNova.SelectionLength = 0;
      this.txtSenhaNova.SelectionStart = 0;
      this.txtSenhaNova.ShortcutsEnabled = true;
      this.txtSenhaNova.Size = new System.Drawing.Size(312, 30);
      this.txtSenhaNova.TabIndex = 1;
      this.txtSenhaNova.UnderlinedStyle = false;
      this.txtSenhaNova.UseSelectable = true;
      this.txtSenhaNova.UseSystemPasswordChar = true;
      this.txtSenhaNova.Valor_Decimais = ((short)(0));
      this.txtSenhaNova.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtSenhaNova.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel1
      // 
      this.lmLabel1.AutoSize = true;
      this.lmLabel1.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel1.Location = new System.Drawing.Point(15, 108);
      this.lmLabel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel1.Name = "lmLabel1";
      this.lmLabel1.Size = new System.Drawing.Size(92, 19);
      this.lmLabel1.TabIndex = 357;
      this.lmLabel1.Text = "Nova Senha *";
      this.lmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtSenhaRepetir
      // 
      this.txtSenhaRepetir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSenhaRepetir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
      this.txtSenhaRepetir.BorderRadius = 15;
      this.txtSenhaRepetir.BorderSize = 2;
      this.txtSenhaRepetir.CampoObrigatorio = true;
      this.txtSenhaRepetir.F7ToolTipText = null;
      this.txtSenhaRepetir.F8ToolTipText = null;
      this.txtSenhaRepetir.F9ToolTipText = null;
      this.txtSenhaRepetir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtSenhaRepetir.IconF7 = ((System.Drawing.Image)(resources.GetObject("txtSenhaRepetir.IconF7")));
      this.txtSenhaRepetir.IconF8 = ((System.Drawing.Image)(resources.GetObject("txtSenhaRepetir.IconF8")));
      this.txtSenhaRepetir.IconToolTipText = null;
      this.txtSenhaRepetir.Lines = new string[0];
      this.txtSenhaRepetir.Location = new System.Drawing.Point(15, 190);
      this.txtSenhaRepetir.MaxLength = 100;
      this.txtSenhaRepetir.Name = "txtSenhaRepetir";
      this.txtSenhaRepetir.PasswordChar = '●';
      this.txtSenhaRepetir.Propriedade = null;
      this.txtSenhaRepetir.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.txtSenhaRepetir.SelectedText = "";
      this.txtSenhaRepetir.SelectionLength = 0;
      this.txtSenhaRepetir.SelectionStart = 0;
      this.txtSenhaRepetir.ShortcutsEnabled = true;
      this.txtSenhaRepetir.Size = new System.Drawing.Size(312, 30);
      this.txtSenhaRepetir.TabIndex = 2;
      this.txtSenhaRepetir.UnderlinedStyle = false;
      this.txtSenhaRepetir.UseSelectable = true;
      this.txtSenhaRepetir.UseSystemPasswordChar = true;
      this.txtSenhaRepetir.Valor_Decimais = ((short)(0));
      this.txtSenhaRepetir.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
      this.txtSenhaRepetir.WaterMarkFont = new System.Drawing.Font("Segoe UI", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Pixel);
      // 
      // lmLabel3
      // 
      this.lmLabel3.AutoSize = true;
      this.lmLabel3.BackColor = System.Drawing.Color.Transparent;
      this.lmLabel3.Location = new System.Drawing.Point(15, 167);
      this.lmLabel3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
      this.lmLabel3.Name = "lmLabel3";
      this.lmLabel3.Size = new System.Drawing.Size(103, 19);
      this.lmLabel3.TabIndex = 359;
      this.lmLabel3.Text = "Repetir Senha *";
      this.lmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // btnSalvar
      // 
      this.btnSalvar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnSalvar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnSalvar.BorderRadius = 13;
      this.btnSalvar.BorderSize = 0;
      this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSalvar.Image = ((System.Drawing.Image)(resources.GetObject("btnSalvar.Image")));
      this.btnSalvar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnSalvar.Location = new System.Drawing.Point(18, 226);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(100, 26);
      this.btnSalvar.TabIndex = 3;
      this.btnSalvar.Text = " &Salvar";
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
      // 
      // FrmAlterarSenha
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(342, 431);
      this.Controls.Add(this.btnSalvar);
      this.Controls.Add(this.txtSenhaRepetir);
      this.Controls.Add(this.lmLabel3);
      this.Controls.Add(this.txtSenhaNova);
      this.Controls.Add(this.lmLabel1);
      this.Controls.Add(this.txtSenhaAtual);
      this.Controls.Add(this.lmLabel2);
      this.HelpButton = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmAlterarSenha";
      this.ShowInTaskbar = false;
      this.Text = "Redefinir Senha";
      this.ClickHelp += new LmCorbieUI.LmForms.LmSingleForm.ButClick(this.FrmAlterarSenha_ClickHelp);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private LmCorbieUI.Controls.LmTextBox txtSenhaAtual;
    private LmCorbieUI.Controls.LmLabel lmLabel2;
    private LmCorbieUI.Controls.LmTextBox txtSenhaNova;
    private LmCorbieUI.Controls.LmLabel lmLabel1;
    private LmCorbieUI.Controls.LmTextBox txtSenhaRepetir;
    private LmCorbieUI.Controls.LmLabel lmLabel3;
    private LmCorbieUI.Controls.LmButton btnSalvar;
  }
}