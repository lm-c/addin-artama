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
  public partial class FrmItemDuplicacaoCad : LmSingleForm {
    item_generico_duplicacao model = new item_generico_duplicacao();

    public FrmItemDuplicacaoCad(int id_item_generico_duplicacao = 0) {
      InitializeComponent();

      txtID.Text = id_item_generico_duplicacao.ToString("#");
    }

    private void FrmItemDuplicacaoCad_Load(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(txtID.Text))
        TxtID_Leave(txtID, new EventArgs());
      else
        BtnLimpar_Click(null, new EventArgs());
    }

    private void BtnLimpar_Click(object sender, EventArgs e) {
      txtID.ReadOnly = false;
      txtDescricao.Focus();
      Controles.Clear(this);

      btnExcluir.Enabled = false;

      txtID.ReadOnly = false;
      txtID.Refresh();

      model = new item_generico_duplicacao();
    }

    private void BtnExcluir_Click(object sender, EventArgs e) {
      try {
        if (MsgBox.Show("Deseja realmente excluir este registro?",
          "Excluir Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
          item_generico_duplicacao.Excluir(model.id);
          Toast.Info("Excluido com Sucesso!");
          BtnLimpar_Click(sender, new EventArgs());
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Matéria Prima");
      }
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      if (Controles.PossuiCamposInvalidos(this)) return;

      try {
        model.codigo = txtCodigoErp.Text;
        model.descricao = txtDescricao.Text;
        model.observacao = txtObservacao.Text;

        Salvar();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Item Para Duplicação");
      }
    }

    private async void Salvar() {
      if (await item_generico_duplicacao.SalvarAsync(model)) {
        Invoke(new MethodInvoker(delegate () {
          Toast.Success("Item Duplicação Salvo com Sucesso!");
          BtnLimpar_Click(null, null);
        }));
      }
    }

    private void TxtID_ButtonClickF7(object sender, EventArgs e) {
      FrmConsultaGeral frm = new FrmConsultaGeral(this,
        item_generico_duplicacao.Selecionar(), "Consulta de itens duplicação");
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
        model = item_generico_duplicacao.Selecionar(id);

        if (model != null) {
          txtID.Text = model.id.ToString();
          txtCodigoErp.Text = model.codigo;
          txtDescricao.Text = model.descricao;
          txtObservacao.Text = model.observacao;

          txtID.ReadOnly = true;
          btnExcluir.Enabled = true;
        } else model = new item_generico_duplicacao();
      }
    }

    private void FrmItemDuplicacaoCad_ClickHelp(object sender, EventArgs e) {
      //Process.Start("https://youtu.be/B_oJWzABF_A");
    }
  }
}
