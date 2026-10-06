using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using System;

namespace AddinArtama {
  public partial class FrmMateriaPrimaCad : LmSingleForm {
    materia_primas model = new materia_primas();

    public FrmMateriaPrimaCad(int id_materia_prima = 0) {
      InitializeComponent();

      txtID.Text = id_materia_prima.ToString("#");

      CarregarComboBox();
    }

    private void FrmMateriaPrimaCad_Load(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(txtID.Text))
        TxtID_Leave(txtID, new EventArgs());
      else
        BtnLimpar_Click(null, new EventArgs());
    }

    private void BtnLimpar_Click(object sender, EventArgs e) {
      txtID.ReadOnly = false;
      txtDescricao.Focus();
      txtID.Text = txtDescricao.Text = txtEspessura.Text = txtCodigoChapa.Text = string.Empty;
      txtMaterial.SelectedValue = null;

      txtID.ReadOnly = false;
      txtID.Refresh();

      ckbSituacao.Checked = true;
      model = new materia_primas();
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      if (Controles.PossuiCamposInvalidos(this)) return;

      try {

        model.codigo = Convert.ToInt32(txtCodigoChapa.Text);
        model.descricao = txtDescricao.Text;
        model.espessura = Convert.ToDouble(txtEspessura.Text);
        model.material_id = (int)txtMaterial.SelectedValue;
        model.tipo_materia_prima = TipoMateriaPrima.Chapa;
        model.ativo = ckbSituacao.Checked;

        if (materia_primas.Salvar(model)) {
          txtID.Text = model.id.ToString();
          txtID.ReadOnly = true;
          txtID.Refresh();
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Operação");
      }
    }

    private void TxtID_ButtonClickF7(object sender, EventArgs e) {
      FrmConsultaGeral frm = new FrmConsultaGeral(this,
        materia_primas.Selecionar(), "Consulta de Matéria Prima");
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
        model = materia_primas.SelecionarMateriaPrima(id);

        if (model != null) {
          txtID.Text = model.id.ToString();
          txtCodigoChapa.Text = model.codigo.ToString();
          txtDescricao.Text = model.descricao;
          ckbSituacao.Checked = model.ativo;
          txtEspessura.Text = model.espessura?.ToString("0.0000");
          txtMaterial.SelectedValue = model.material_id;

          ckbSituacao.Checked = model.ativo;

          txtID.ReadOnly = true;
        } else model = new materia_primas();
      }
    }

    private void CarregarComboBox() {
      try {
        txtMaterial.CarregarComboBox(materiais.Selecionar(ativo: true));
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar lista Maquina");
      }
    }

    private void FrmMateriaPrimaCad_ClickHelp(object sender, EventArgs e) {
      //Process.Start("https://youtu.be/B_oJWzABF_A");
    }
  }
}
