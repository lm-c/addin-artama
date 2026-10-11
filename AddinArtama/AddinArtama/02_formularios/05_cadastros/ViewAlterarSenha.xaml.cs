using LmCorbieUI;
using LmCorbieUI.Metodos;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Redefinir a senha do usuário logado (substitui o FrmAlterarSenha).</summary>
  public partial class ViewAlterarSenha : Wpf.LmView {
    public ViewAlterarSenha() {
      InitializeComponent();

      Loaded += (s, e) => txtSenhaAtual.Focus();

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private System.Collections.Generic.IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Redefinir Senha",
        "Aqui você troca a SUA senha de acesso (a do usuário que está logado).\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Info("1. Senha atual",
        "Digite a senha que você usa hoje.\n\n" +
        "Regra: se ela não conferir, a troca é recusada.",
        txtSenhaAtual);

      yield return Wpf.LmPassoAssistente.Info("2. Nova senha",
        "Digite a nova senha e repita no campo de baixo.\n\n" +
        "Regra: os dois campos precisam ser iguais.",
        txtSenhaNova, txtSenhaRepetir);

      yield return Wpf.LmPassoAssistente.Info("3. Salvar",
        "Grava a nova senha. Os três campos são obrigatórios.\n\n" +
        "Regra: a senha é gravada criptografada e já vale no próximo login.",
        btnSalvar);
    }

    #endregion

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
  }
}
