using AxionConnect.Wpf.Services;
using System;
using System.Windows;
using System.Windows.Threading;

namespace AxionConnect.Wpf {
  /// <summary>
  /// ⚠ Este projeto deve ser compilado como x64 para que o controle ActiveX do eDrawings funcione.
  /// </summary>
  public partial class App : Application {
    protected override void OnStartup(StartupEventArgs e) {
      base.OnStartup(e);

      DispatcherUnhandledException += App_DispatcherUnhandledException;
      AppDomain.CurrentDomain.UnhandledException += (s, ex) => Log.Erro("Erro fatal", ex.ExceptionObject as Exception);
      System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, ex) => Log.Erro("Erro em tarefa", ex.Exception);
      Exit += (s, ex) => Log.Info($"Sistema encerrado (código {ex.ApplicationExitCode})");

      // Erros dentro de controles WinForms hospedados (ex.: ActiveX do eDrawings) não passam pelo
      // DispatcherUnhandledException: sem isto aparece a caixa padrão do .NET.
      System.Windows.Forms.Application.SetUnhandledExceptionMode(System.Windows.Forms.UnhandledExceptionMode.CatchException);
      System.Windows.Forms.Application.ThreadException += (s, ex) => Notificacao.Erro($"Erro inesperado:\n{ex.Exception.Message}");

      // Janelas da LmCorbieUI WinForms usadas pelas regras de negócio (MsgBox, LmException)
      System.Windows.Forms.Application.EnableVisualStyles();

      Sessao.Iniciar();
      Sessao.MostrarLogin();
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
      Log.Erro("Erro inesperado", e.Exception);
      Notificacao.Erro($"Erro inesperado:\n{e.Exception.Message}");
      e.Handled = true;
    }
  }
}
