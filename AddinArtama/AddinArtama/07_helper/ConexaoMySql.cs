using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AddinArtama {
  class Prefixo {
    public static string GetPrefixo => ""; // para Oficial
    // public static string GetPrefixo => "teste_"; // para testes
  }

  public class ConexaoMySql : IDisposable {
    public static readonly string Database = $"{Prefixo.GetPrefixo}artama";
    public static readonly string Port = "3306";

    // Credenciais lidas do arquivo .env ao lado da DLL (nao versionado; ver .env.example)
    private static readonly Dictionary<string, string> Env = CarregarEnv();

    public static readonly string Server = Env.TryGetValue("DB_SERVER", out var server) ? server : "";
    public static readonly string User = Env.TryGetValue("DB_USER", out var user) ? user : "";
    public static readonly string Pass = Env.TryGetValue("DB_PASS", out var pass) ? pass : "";

    /// <summary>Valor de uma chave do .env (vazio se não existir).</summary>
    internal static string LerEnv(string chave) => Env.TryGetValue(chave, out var valor) ? valor : "";

    private static Dictionary<string, string> CarregarEnv() {
      var valores = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      var caminho = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".env");
      if (!File.Exists(caminho))
        return valores;

      foreach (var linha in File.ReadAllLines(caminho)) {
        var texto = linha.Trim();
        if (texto.Length == 0 || texto.StartsWith("#"))
          continue;
        var idx = texto.IndexOf('=');
        if (idx <= 0)
          continue;
        valores[texto.Substring(0, idx).Trim()] = texto.Substring(idx + 1).Trim();
      }
      return valores;
    }

    public static MySqlConnection GetConexao() {
      return new MySqlConnection($"Server={Server};Port={Port};Database={Database};Uid={User};Pwd={Pass};SSL Mode=None");
    }

    public static string ConnectionString() {
      return $"Server={Server};Port={Port};Database={Database};Uid={User};Pwd={Pass};SSL Mode=None";
    }

    public void Dispose() {
      GC.Collect();
    }
  }
}

