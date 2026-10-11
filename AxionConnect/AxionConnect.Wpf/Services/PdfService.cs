using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AxionConnect.Wpf.Services {
  /// <summary>
  /// Localiza o PDF de um componente. Os PDFs são exportados pelo Addin para uma pasta "..._PDF"
  /// (DrawExport.GetFolder) que fica no mesmo diretório do desenho.
  /// </summary>
  public static class PdfService {
    public const string SufixoPasta = "_PDF";

    /// <summary>Pastas terminadas em "_PDF" no diretório do arquivo (desenho ou modelo).</summary>
    public static List<string> Pastas(string caminhoArquivo) {
      var diretorio = string.IsNullOrEmpty(caminhoArquivo) ? null : Path.GetDirectoryName(caminhoArquivo);
      if (string.IsNullOrEmpty(diretorio) || !Directory.Exists(diretorio))
        return new List<string>();

      return Directory.EnumerateDirectories(diretorio, "*" + SufixoPasta)
        .Where(x => x.EndsWith(SufixoPasta, StringComparison.OrdinalIgnoreCase))
        .ToList();
    }

    /// <summary>
    /// Procura, nas pastas "_PDF", o PDF com um dos nomes informados (nome do arquivo do desenho, nome do
    /// componente...). Primeiro o nome exato; depois o nome com prefixo, como o "001 - NOME.PDF" do conjunto
    /// principal. Retorna null se não achar.
    /// </summary>
    public static string Localizar(string caminhoArquivo, params string[] nomes) {
      var procurados = nomes.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
      if (procurados.Count == 0)
        return null;

      var pdfs = Pastas(caminhoArquivo)
        .SelectMany(x => Directory.EnumerateFiles(x, "*.pdf"))
        .Select(x => new { Caminho = x, Nome = Path.GetFileNameWithoutExtension(x) })
        .ToList();

      foreach (var nome in procurados) {
        var exato = pdfs.FirstOrDefault(x => x.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        if (exato != null)
          return exato.Caminho;
      }

      foreach (var nome in procurados) {
        var comPrefixo = pdfs.FirstOrDefault(x => x.Nome.EndsWith(" - " + nome, StringComparison.OrdinalIgnoreCase));
        if (comPrefixo != null)
          return comPrefixo.Caminho;
      }

      return null;
    }
  }
}
