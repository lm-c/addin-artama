using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddinArtama {
  public partial class FrmAlterarSenha : LmSingleForm {
    public FrmAlterarSenha() {
      InitializeComponent();
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      try {
        if (Controles.PossuiCamposInvalidos(this))
          return;

        using (ContextoDados db = new ContextoDados()) {
          if (usuario_alocados.model.usuario.senha != txtSenhaAtual.Text.CriptografarAES()) {
            Toast.Warning("Senha Atual incorreta!");
            return;
          }

          if (txtSenhaNova.Text != txtSenhaRepetir.Text) {
            Toast.Warning("Senhas não conferem!");
            return;
          }

          usuario_alocados.model.usuario.senha = txtSenhaNova.Text.CriptografarAES();
          usuarios.Salvar(usuario_alocados.model.usuario);

          Toast.Success("Senha atualizada com sucesso!");
        }
      } catch (Exception ex) {
        Toast.Error("Erro ao salvar senha nova.\r\n" + ex.Message);
      }
    }

    private void FrmAlterarSenha_ClickHelp(object sender, EventArgs e) {
      Process.Start("https://youtu.be/OKXSHIKqfvc");
    }
  }
}
