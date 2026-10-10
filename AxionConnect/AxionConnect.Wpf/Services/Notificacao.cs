using System;
using System.Windows;
using Lm = LmCorbieUI.Wpf;

namespace AxionConnect.Wpf.Services {
  /// <summary>
  /// Ponto único para mensagens ao usuário, usando o Toast/MsgBox da LmCorbieUI.Wpf.
  /// O alias "Lm" evita conflito com o Toast/MsgBox WinForms (namespace LmCorbieUI), ainda referenciado pelo CORE.
  /// Os toasts são janelas próprias: aparecem por cima do eDrawings. Pode ser chamada de qualquer thread.
  /// </summary>
  public static class Notificacao {
    public static void Sucesso(string mensagem) => Mostrar(() => Lm.Toast.Success(mensagem?.Trim()));
    public static void Info(string mensagem) => Mostrar(() => Lm.Toast.Info(mensagem?.Trim()));
    public static void Aviso(string mensagem) => Mostrar(() => Lm.Toast.Warning(mensagem?.Trim()));
    public static void Erro(string mensagem) => Mostrar(() => Lm.Toast.Error(mensagem?.Trim()));

    public static bool Confirmar(string mensagem, string titulo = "Axion Connect") =>
      Lm.MsgBox.Show(mensagem, titulo, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

    /// <summary>
    /// O toast é uma janela: abri-la durante um layout/render do WPF (ex.: erro ao criar o eDrawings)
    /// dá "O processamento do dispatcher foi suspenso". Enfileirar no dispatcher evita isso.
    /// </summary>
    private static void Mostrar(Action exibir) {
      var dispatcher = Application.Current?.Dispatcher;
      if (dispatcher == null) {
        exibir();
        return;
      }

      dispatcher.BeginInvoke(exibir);
    }
  }
}
