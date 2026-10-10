using System;
using System.IO;

namespace AxionConnect.Wpf.Services {
  /// <summary>
  /// Log simples em arquivo: %AppData%\Lm Projetos\AxionConnect\axion.log
  /// Guarda o tempo de cada etapa de carregamento para diagnosticar lentidão/travamentos.
  /// </summary>
  public static class Log {
    private static readonly object _trava = new object();

    public static string Arquivo { get; } = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lm Projetos", "AxionConnect", "axion.log");

    public static void Info(string mensagem) => Escrever("INFO", mensagem);
    public static void Erro(string mensagem, Exception ex = null) => Escrever("ERRO", ex == null ? mensagem : $"{mensagem}: {ex}");

    private static void Escrever(string nivel, string mensagem) {
      try {
        lock (_trava) {
          Directory.CreateDirectory(Path.GetDirectoryName(Arquivo));

          // Mantém o arquivo pequeno
          var info = new FileInfo(Arquivo);
          if (info.Exists && info.Length > 2_000_000)
            info.Delete();

          File.AppendAllText(Arquivo, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{nivel}] {mensagem}{Environment.NewLine}");
        }
      } catch (Exception) {
        // log nunca pode derrubar o sistema
      }
    }
  }
}
