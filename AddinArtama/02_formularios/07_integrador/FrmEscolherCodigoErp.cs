using LmCorbieUI.Controls;
using LmCorbieUI.Design;
using LmCorbieUI.LmForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static AddinArtama.Api;

namespace AddinArtama {
  internal partial class FrmEscolherCodigoErp : LmSingleForm {
    internal string _codigoErp = string.Empty;

    public FrmEscolherCodigoErp(List<ItemGenerico> itens, string nomePeca) {
      InitializeComponent();

      this.Text = $"{nomePeca} - Escolher Código";

      flpChosen.Controls.Clear();
      int height = 0;
      foreach (var item in itens) {
        var lbl = new LmLabel {
          UseCustomColor = true,
          BackColor = LmCor.Bc_Btn_Normal,
          FontSize = LmCorbieUI.Design.LmLabelSize.Tall,
          Margin = new System.Windows.Forms.Padding(3),
          Name = "lbl" + item.codigo,
          Size = new System.Drawing.Size(151, 39),
          TabIndex = 76,
          Text = item.codigo,
          TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };

        lbl.MouseEnter += Lbl_MouseEnter;
        lbl.MouseLeave += Lbl_MouseLeave;
        lbl.Click += Lbl_Click;

        flpChosen.Controls.Add(lbl);

        height = lbl.Location.Y + 150;
      }

      this.Height = height;
    }

    private void Lbl_MouseEnter(object sender, EventArgs e) {
      ((LmLabel)sender).BackColor = LmCor.Bc_Btn_Selected;
    }

    private void Lbl_MouseLeave(object sender, EventArgs e) {
      ((LmLabel)sender).BackColor = LmCor.Bc_Btn_Normal;
    }

    private void Lbl_Click(object sender, EventArgs e) {
      _codigoErp = ((LmLabel)sender).Text;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }
}
