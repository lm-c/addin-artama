using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>
  /// Ponto único para caixas de mensagem do add-in, sempre pelo MsgBox da LmCorbieUI.Wpf
  /// (substitui o MsgBox/LmException WinForms). Pode ser chamada de qualquer thread e de classes
  /// que ainda importam System.Windows.Forms, sem conflito de enums.
  /// </summary>
  public static class Msg {
    public const string Titulo = "Addin LM Projetos";

    public static void Info(string texto, string titulo = Titulo) =>
      Wpf.MsgBox.Show(texto, titulo, MessageBoxButton.OK, MessageBoxImage.Information);

    public static void Aviso(string texto, string titulo = Titulo) =>
      Wpf.MsgBox.Show(texto, titulo, MessageBoxButton.OK, MessageBoxImage.Warning);

    public static void Erro(string texto, string titulo = Titulo) =>
      Wpf.MsgBox.Show(texto, titulo, MessageBoxButton.OK, MessageBoxImage.Error);

    public static bool Confirmar(string texto, string titulo = Titulo) =>
      Wpf.MsgBox.Show(texto, titulo, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

    /// <summary>
    /// Mostra a exceção com o que estava sendo feito (contexto), a causa real (exceção mais interna,
    /// traduzida quando conhecida), o tipo do erro e onde ele aconteceu (classe.método, arquivo e linha).
    /// </summary>
    public static void Erro(Exception ex, string contexto = null, string titulo = Titulo) {
      Debug.WriteLine(ex);
      Erro(Descrever(ex, contexto), titulo);
    }

    /// <summary>Texto completo do erro, no mesmo formato da caixa de mensagem.</summary>
    public static string Descrever(Exception ex, string contexto = null) {
      var sb = new StringBuilder();

      if (!string.IsNullOrWhiteSpace(contexto))
        sb.AppendLine(contexto.Trim()).AppendLine();

      if (ex == null)
        return sb.Append("Erro desconhecido.").ToString();

      var origem = Origem(ex);

      sb.AppendLine(Causa(ex, origem));
      sb.AppendLine();
      sb.Append("Tipo: ").Append(origem.GetType().Name);

      var onde = Onde(ex);
      if (onde != null)
        sb.AppendLine().Append("Onde: ").Append(onde);

      return sb.ToString();
    }

    /// <summary>A exceção de origem (a mais interna da cadeia).</summary>
    static Exception Origem(Exception ex) {
      while (true) {
        if (ex is AggregateException agregada && agregada.InnerExceptions.Count > 0)
          ex = agregada.InnerExceptions[0];
        else if (ex.InnerException != null)
          ex = ex.InnerException;
        else
          return ex;
      }
    }

    static string Causa(Exception ex, Exception origem) {
      var validacao = ErrosValidacao(ex);
      if (validacao != null)
        return validacao;

      if (origem is NullReferenceException)
        return "Um objeto necessário não foi encontrado (referência nula). " +
               "Normalmente o SolidWorks não retornou o que foi pedido ou falta uma configuração.";

      if (origem is COMException com)
        return $"O SolidWorks retornou um erro (0x{com.ErrorCode:X8}):\n{com.Message}";

      if (origem is InvalidCastException)
        return $"O documento/objeto não é do tipo esperado para este comando.\n{origem.Message}";

      if (origem is FileNotFoundException arquivo && !string.IsNullOrEmpty(arquivo.FileName))
        return $"Arquivo não encontrado:\n{arquivo.FileName}";

      return Traduzir(origem.Message);
    }

    static string Traduzir(string erro) {
      var minusculo = erro.ToLower();

      if (erro.StartsWith("The model backing the"))
        return "Banco de dados alterado.\nAtualize o Addin para a última versão.";

      if (minusculo.Contains("unable to connect to any of the specified mysql hosts") ||
          minusculo.Contains("the underlying provider failed on open"))
        return "Não foi possível conectar ao servidor do banco de dados.\nVerifique a rede e a configuração de conexão.";

      if (minusculo.Contains("duplicate entry") && minusculo.Contains("for key"))
        return $"Já existe um registro com esta chave na base de dados.\n{erro}";

      if (minusculo.Contains("a foreign key constraint fails"))
        return $"O registro está vinculado a outros dados (chave estrangeira).\n{erro}";

      return erro;
    }

    /// <summary>
    /// Primeiro método do add-in no stack trace da exceção mais interna: é onde o erro aconteceu
    /// (o topo pode ser código do .NET/SolidWorks). Arquivo e linha só aparecem se o .pdb estiver junto da DLL.
    /// </summary>
    static string Onde(Exception ex) {
      try {
        Exception comStack = null;
        for (var atual = ex; atual != null; atual = atual.InnerException) {
          if (!string.IsNullOrWhiteSpace(atual.StackTrace))
            comStack = atual;
        }

        if (comStack == null)
          return null;

        var frames = new StackTrace(comStack, true).GetFrames();
        if (frames == null || frames.Length == 0)
          return null;

        var assembly = typeof(Msg).Assembly;
        var frame = frames.FirstOrDefault(x => x.GetMethod()?.DeclaringType?.Assembly == assembly) ?? frames[0];
        var metodo = frame.GetMethod();
        if (metodo == null)
          return null;

        var onde = NomeMetodo(metodo);

        var arquivo = frame.GetFileName();
        if (!string.IsNullOrEmpty(arquivo))
          onde += $" ({Path.GetFileName(arquivo)}, linha {frame.GetFileLineNumber()})";

        return onde;
      } catch (Exception) {
        return null;
      }
    }

    static string NomeMetodo(MethodBase metodo) {
      var tipo = metodo.DeclaringType;
      var nome = metodo.Name;

      // Métodos async/lambdas: "Classe.<Carregar>d__9.MoveNext" -> "Classe.Carregar"
      var gerado = Regex.Match(tipo?.Name ?? "", @"^<(\w+)>");
      if (gerado.Success) {
        nome = gerado.Groups[1].Value;
        tipo = tipo.DeclaringType;
      } else {
        var lambda = Regex.Match(nome, @"^<(\w+)>");
        if (lambda.Success)
          nome = lambda.Groups[1].Value;
      }

      while (tipo != null && tipo.Name.StartsWith("<"))
        tipo = tipo.DeclaringType;

      return tipo == null ? nome : $"{tipo.Name}.{nome}";
    }

    /// <summary>Erros de validação do Entity Framework (lidos por reflexão para não depender do tipo).</summary>
    static string ErrosValidacao(Exception ex) {
      try {
        for (var atual = ex; atual != null; atual = atual.InnerException) {
          var entidades = atual.GetType().GetProperty("EntityValidationErrors")?.GetValue(atual, null) as IEnumerable;
          if (entidades == null)
            continue;

          var sb = new StringBuilder();

          foreach (var entidade in entidades) {
            var entry = entidade?.GetType().GetProperty("Entry")?.GetValue(entidade, null);
            var nomeEntidade = entry?.GetType().GetProperty("Entity")?.GetValue(entry, null)?.GetType().Name;
            var erros = entidade?.GetType().GetProperty("ValidationErrors")?.GetValue(entidade, null) as IEnumerable;
            if (erros == null)
              continue;

            foreach (var erro in erros) {
              var propriedade = Convert.ToString(erro?.GetType().GetProperty("PropertyName")?.GetValue(erro, null));
              var mensagem = Convert.ToString(erro?.GetType().GetProperty("ErrorMessage")?.GetValue(erro, null));
              sb.AppendLine($"{nomeEntidade}.{propriedade}: {mensagem}");
            }
          }

          if (sb.Length > 0)
            return "Falha de validação:\n" + sb.ToString().TrimEnd();
        }
      } catch (Exception) {
      }

      return null;
    }
  }
}
