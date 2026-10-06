namespace AddinArtama {
  partial class FrmPermissao {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPermissao));
      this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
      this.btnDesmarcar = new LmCorbieUI.Controls.LmButton();
      this.btnMarcar = new LmCorbieUI.Controls.LmButton();
      this.btnSalvar = new LmCorbieUI.Controls.LmButton();
      this.trvSolution = new System.Windows.Forms.TreeView();
      this.lmPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // lmPanel1
      // 
      this.lmPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel1.Controls.Add(this.btnDesmarcar);
      this.lmPanel1.Controls.Add(this.btnMarcar);
      this.lmPanel1.Controls.Add(this.btnSalvar);
      this.lmPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lmPanel1.IsPanelMenu = false;
      this.lmPanel1.Location = new System.Drawing.Point(2, 380);
      this.lmPanel1.Name = "lmPanel1";
      this.lmPanel1.Size = new System.Drawing.Size(239, 100);
      this.lmPanel1.TabIndex = 100;
      // 
      // btnDesmarcar
      // 
      this.btnDesmarcar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDesmarcar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnDesmarcar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnDesmarcar.BorderRadius = 13;
      this.btnDesmarcar.BorderSize = 0;
      this.btnDesmarcar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnDesmarcar.Image = ((System.Drawing.Image)(resources.GetObject("btnDesmarcar.Image")));
      this.btnDesmarcar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnDesmarcar.Location = new System.Drawing.Point(3, 65);
      this.btnDesmarcar.Name = "btnDesmarcar";
      this.btnDesmarcar.Size = new System.Drawing.Size(233, 26);
      this.btnDesmarcar.TabIndex = 7;
      this.btnDesmarcar.Text = " &Desmarcar Todos";
      this.btnDesmarcar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnDesmarcar.UseVisualStyleBackColor = false;
      this.btnDesmarcar.Click += new System.EventHandler(this.BtnDesmarcar_Click);
      // 
      // btnMarcar
      // 
      this.btnMarcar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnMarcar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnMarcar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnMarcar.BorderRadius = 13;
      this.btnMarcar.BorderSize = 0;
      this.btnMarcar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnMarcar.Image = ((System.Drawing.Image)(resources.GetObject("btnMarcar.Image")));
      this.btnMarcar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnMarcar.Location = new System.Drawing.Point(3, 33);
      this.btnMarcar.Name = "btnMarcar";
      this.btnMarcar.Size = new System.Drawing.Size(233, 26);
      this.btnMarcar.TabIndex = 6;
      this.btnMarcar.Text = " &Marcar Todos";
      this.btnMarcar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnMarcar.UseVisualStyleBackColor = false;
      this.btnMarcar.Click += new System.EventHandler(this.BtnMarcar_Click);
      // 
      // btnSalvar
      // 
      this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSalvar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnSalvar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnSalvar.BorderRadius = 13;
      this.btnSalvar.BorderSize = 0;
      this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSalvar.Image = ((System.Drawing.Image)(resources.GetObject("btnSalvar.Image")));
      this.btnSalvar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnSalvar.Location = new System.Drawing.Point(3, 1);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(233, 26);
      this.btnSalvar.TabIndex = 5;
      this.btnSalvar.Text = " &Salvar e Fechar";
      this.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnSalvar.UseVisualStyleBackColor = false;
      this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
      // 
      // trvSolution
      // 
      this.trvSolution.CheckBoxes = true;
      this.trvSolution.Dock = System.Windows.Forms.DockStyle.Fill;
      this.trvSolution.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.trvSolution.Location = new System.Drawing.Point(2, 30);
      this.trvSolution.Name = "trvSolution";
      this.trvSolution.Size = new System.Drawing.Size(239, 350);
      this.trvSolution.TabIndex = 101;
      this.trvSolution.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TrvSolution_AfterSelect);
      // 
      // FrmPermissao
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(243, 482);
      this.Controls.Add(this.trvSolution);
      this.Controls.Add(this.lmPanel1);
      this.Location = new System.Drawing.Point(0, 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmPermissao";
      this.Padding = new System.Windows.Forms.Padding(2, 30, 2, 2);
      this.Resizable = false;
      this.ShowInTaskbar = false;
      this.Text = "Definir Permissões do Perfil";
      this.lmPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion
    private LmCorbieUI.Controls.LmPanel lmPanel1;
    private LmCorbieUI.Controls.LmButton btnSalvar;
    private LmCorbieUI.Controls.LmButton btnDesmarcar;
    private LmCorbieUI.Controls.LmButton btnMarcar;
    private System.Windows.Forms.TreeView trvSolution;
  }
}