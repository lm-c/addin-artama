using LmCorbieUI;
using LmCorbieUI.Metodos;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Redefinir a senha do usuário logado (substitui o FrmAlterarSenha).</summary>
  public partial class ViewAlterarSenha : Wpf.LmView {
    public ViewAlterarSenha() {
      InitializeComponent();

      Loaded += (s, e) => txtSenhaAtual.Focus();
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      try {
        if (!LmValidacao.Validar(this))
          return;

        if (usuario_alocados.model.usuario.senha != txtSenhaAtual.Senha.CriptografarAES()) {
          Wpf.Toast.Warning("Senha Atual incorreta!");
          return;
        }

        if (txtSenhaNova.Senha != txtSenhaRepetir.Senha) {
          Wpf.Toast.Warning("Senhas não conferem!");
          return;
        }

        usuario_alocados.model.usuario.senha = txtSenhaNova.Senha.CriptografarAES();
        usuarios.Salvar(usuario_alocados.model.usuario);

        Wpf.Toast.Success("Senha atualizada com sucesso!");
      } catch (Exception ex) {
        Wpf.Toast.Error("Erro ao salvar senha nova.\r\n" + ex.Message);
      }
    }

    private void LmView_AjudaClick(object sender, EventArgs e) {
      Process.Start("https://youtu.be/OKXSHIKqfvc");
    }
  }
}
