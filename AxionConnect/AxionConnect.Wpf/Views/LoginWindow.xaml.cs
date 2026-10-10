using AxionConnect.Wpf.Services;
using AxionConnect.Wpf.ViewModels;
using LmCorbieUI.Wpf.Controls;
using System.ComponentModel;
using System.Windows;
using Lm = LmCorbieUI.Wpf;

namespace AxionConnect.Wpf.Views {
  public partial class LoginWindow : Lm.LmWindow {
    private readonly LoginViewModel _vm = new LoginViewModel();

    /// <summary>O login automático só acontece na primeira abertura (no logout a janela volta sem entrar sozinha).</summary>
    private bool _inicializado;

    public LoginWindow() {
      InitializeComponent();
      DataContext = _vm;

      _vm.SenhaAutomatica += senha => TxtSenha.Senha = senha;
      _vm.LoginConcluido += Sessao.AbrirPrincipal;

      Loaded += async (s, e) => {
        if (_inicializado)
          return;

        _inicializado = true;
        await _vm.TentarLoginAutomaticoAsync();
        if (IsVisible)
          TxtUsuario.Focus();
      };
    }

    /// <summary>Fechar o login (sem a principal aberta) encerra o sistema.</summary>
    protected override void OnClosing(CancelEventArgs e) {
      base.OnClosing(e);
      // Escondida (principal aberta), o fechamento vem do próprio encerramento do app.
      if (!e.Cancel && IsVisible)
        Sessao.Encerrar();
    }

    // Botão com IsDefault: Enter em qualquer campo também entra
    private async void BtnEntrar_Click(object sender, RoutedEventArgs e) {
      if (!LmValidacao.Validar(View)) {
        Log.Info("Login: campos obrigatórios não preenchidos");
        return;
      }

      await _vm.EntrarAsync(TxtSenha.Senha);
    }
  }
}
