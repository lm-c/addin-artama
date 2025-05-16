using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using SolidWorks.Interop.sldworks;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;

namespace AddinArtama {
  public partial class FrmConfigIntegrador : LmSingleForm {
    configuracao_api model = new configuracao_api();

    public FrmConfigIntegrador() {
      InitializeComponent();
    }

    private void FrmConfigIntegrador_Loaded(object sender, EventArgs e) {
      try {
        Invoke(new MethodInvoker(delegate () {
          model = configuracao_api.Selecionar();

          if (model != null) {
            this.Modo = Modo.Alteracao;

            Controles.PreencherControles(this, model);
          } else {
            Modo = Modo.Novo;
            model = new configuracao_api();
          }
        }));
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Carregar Dados\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      if (!ValidarDados()) return;

      configuracao_api.SalvarAsync(model).Wait();

      Api.token = model.token;
      Api.url = model.endereco;

      MsgBox.Show("Alterado com Sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private bool ValidarDados() {
      if (Controles.PossuiCamposInvalidos(this)) {
        return false;
      }

      Controles.AtualizarObjeto(this, model);

      return true;
    }
  }
}
