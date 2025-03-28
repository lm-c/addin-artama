using AddinArtama;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AddinArtama {
  public partial class FrmMaterialCad : LmSingleForm {
    materiais model = new materiais();

    public FrmMaterialCad(int idMaterial = 0) {
      InitializeComponent();

      txtID.Text = idMaterial.ToString("#");
    }

    private void FrmMaterialCad_Load(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(txtID.Text))
        TxtID_Leave(txtID, new EventArgs());
      else
        BtnLimpar_Click(null, new EventArgs());
    }

    private void BtnLimpar_Click(object sender, EventArgs e) {
      txtID.ReadOnly = false;
      txtDescricao.Focus();
      txtID.Text = txtDescricao.Text = string.Empty;

      txtID.ReadOnly = false;
      txtID.Refresh();

      ckbSituacao.Checked = true;
      model = new materiais();
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      if (Controles.PossuiCamposInvalidos(this)) return;

      try {

        model.descricao = txtDescricao.Text;
        model.ativo = ckbSituacao.Checked;

        if (materiais.Salvar(model)) {
          txtID.Text = model.id.ToString();
          txtID.ReadOnly = true;
          txtID.Refresh();
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Matéria Prima");
      }
    }

    private void TxtID_ButtonClickF7(object sender, EventArgs e) {
      FrmConsultaGeral frm = new FrmConsultaGeral(this,
        materiais.Selecionar(), "Consulta de materiais");
      if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK /*&& Modo == Modo.Novo*/)
        if (int.TryParse(frm.valor[0], out int ID)) {
          txtID.Text = frm.valor[0];
          TxtID_Leave(null, new EventArgs());
        }
    }

    private void TxtID_Leave(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(txtID.Text)) {
        int id = int.Parse(txtID.Text);

        if (model.id == id) return;
        model = materiais.Selecionar(id);

        if (model != null) {
          txtID.Text = model.id.ToString();
          txtDescricao.Text = model.descricao;
          ckbSituacao.Checked = model.ativo;

          ckbSituacao.Checked = model.ativo;

          txtID.ReadOnly = true;
        } else model = new materiais();
      }
    }

    private void FrmMaterialCad_ClickHelp(object sender, EventArgs e) {
      //Process.Start("https://youtu.be/B_oJWzABF_A");
    }
  }
}
