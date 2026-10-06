namespace AddinArtama {
  partial class FrmSelecionarPerfil {
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelecionarPerfil));
      this.dgv = new LmCorbieUI.Controls.LmDataGridMini();
      this.lmPanel1 = new LmCorbieUI.Controls.LmPanel();
      this.btnConfirmar = new LmCorbieUI.Controls.LmButton();
      this.Sel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
      this.lmPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // dgv
      // 
      this.dgv.AllowUserToAddRows = false;
      this.dgv.AllowUserToDeleteRows = false;
      this.dgv.AllowUserToOrderColumns = true;
      this.dgv.AllowUserToResizeRows = false;
      this.dgv.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
      this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Custom;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(112)))), ((int)(((byte)(125)))));
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
      dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(112)))), ((int)(((byte)(125)))));
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sel,
            this.ID,
            this.Descricao});
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(199)))), ((int)(((byte)(206)))));
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
      dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(41)))), ((int)(((byte)(38)))));
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(151)))), ((int)(((byte)(161)))));
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(21)))), ((int)(((byte)(18)))));
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv.DefaultCellStyle = dataGridViewCellStyle2;
      this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgv.EnableHeadersVisualStyles = false;
      this.dgv.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
      this.dgv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.dgv.Location = new System.Drawing.Point(2, 30);
      this.dgv.MultiSelect = false;
      this.dgv.Name = "dgv";
      this.dgv.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(112)))), ((int)(((byte)(125)))));
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
      dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(112)))), ((int)(((byte)(125)))));
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.dgv.RowHeadersVisible = false;
      this.dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.dgv.RowTemplate.Height = 21;
      this.dgv.Size = new System.Drawing.Size(396, 268);
      this.dgv.TabIndex = 0;
      this.dgv.UseSelectable = true;
      // 
      // lmPanel1
      // 
      this.lmPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.lmPanel1.Controls.Add(this.btnConfirmar);
      this.lmPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lmPanel1.IsPanelMenu = false;
      this.lmPanel1.Location = new System.Drawing.Point(2, 263);
      this.lmPanel1.Name = "lmPanel1";
      this.lmPanel1.Size = new System.Drawing.Size(396, 35);
      this.lmPanel1.TabIndex = 1;
      // 
      // btnConfirmar
      // 
      this.btnConfirmar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnConfirmar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btnConfirmar.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnConfirmar.BorderRadius = 13;
      this.btnConfirmar.BorderSize = 0;
      this.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnConfirmar.Image = ((System.Drawing.Image)(resources.GetObject("btnConfirmar.Image")));
      this.btnConfirmar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnConfirmar.Location = new System.Drawing.Point(127, 4);
      this.btnConfirmar.Name = "btnConfirmar";
      this.btnConfirmar.Size = new System.Drawing.Size(143, 26);
      this.btnConfirmar.TabIndex = 6;
      this.btnConfirmar.Text = " Confirmar";
      this.btnConfirmar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnConfirmar.UseVisualStyleBackColor = false;
      this.btnConfirmar.Click += new System.EventHandler(this.BtnConfirmar_Click);
      // 
      // Sel
      // 
      this.Sel.HeaderText = "Sel";
      this.Sel.Name = "Sel";
      this.Sel.Width = 40;
      // 
      // ID
      // 
      this.ID.HeaderText = "ID";
      this.ID.Name = "ID";
      this.ID.ReadOnly = true;
      this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.ID.Width = 60;
      // 
      // Descricao
      // 
      this.Descricao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Descricao.HeaderText = "Descrição";
      this.Descricao.Name = "Descricao";
      this.Descricao.ReadOnly = true;
      this.Descricao.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.Descricao.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // FrmSelecionarPerfil
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(400, 300);
      this.Controls.Add(this.lmPanel1);
      this.Controls.Add(this.dgv);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(0, 0);
      this.Name = "FrmSelecionarPerfil";
      this.Padding = new System.Windows.Forms.Padding(2, 30, 2, 2);
      this.Text = "Selecionar Perfil";
      ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
      this.lmPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private LmCorbieUI.Controls.LmDataGridMini dgv;
    private LmCorbieUI.Controls.LmPanel lmPanel1;
    private LmCorbieUI.Controls.LmButton btnConfirmar;
    private System.Windows.Forms.DataGridViewCheckBoxColumn Sel;
    private System.Windows.Forms.DataGridViewTextBoxColumn ID;
    private System.Windows.Forms.DataGridViewTextBoxColumn Descricao;
  }
}