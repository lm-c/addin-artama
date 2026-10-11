using LmCorbieUI;
using LmCorbieUI.Wpf.Design;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace AddinArtama {
  /// <summary>
  /// Cor do tema. Aplica em LmTema (telas WPF) e LmCor (janelas WinForms que restam, ex.: consulta e relatórios),
  /// e fica salva nas predefinições locais do usuário (aparencia.lm, chave=valor).
  /// </summary>
  internal static class Aparencia {
    internal sealed class Opcao {
      public Opcao(string nome, Color cor) {
        Nome = nome;
        Cor = cor;
      }

      public string Nome { get; }
      public Color Cor { get; }
    }

    public static readonly Opcao[] Opcoes = {
      new Opcao("Padrão (azul petróleo)", LmPaleta.CorPrimariaPadrao),
      new Opcao("Azul", Color.FromRgb(21, 67, 120)),
      new Opcao("Verde", Color.FromRgb(0, 94, 58)),
      new Opcao("Roxo", Color.FromRgb(60, 50, 120)),
      new Opcao("Vinho", Color.FromRgb(120, 30, 40)),
      new Opcao("Terracota", Color.FromRgb(150, 70, 30)),
      new Opcao("Grafite", Color.FromRgb(55, 60, 66)),
    };

    /// <summary>Banco de teste (homologação): o rodapé do painel avisa.</summary>
    public static bool AmbienteTeste => ConexaoMySql.Database.StartsWith("teste");

    static string LocalConfig => Path.Combine(ValPadrao.PastaConfig, "aparencia.lm");

    /// <summary>Aplica a cor salva (chamar depois de ValPadrao.DefinirPadrao).</summary>
    public static void AplicarSalva() {
      Definir(Carregar() ?? LmPaleta.CorPrimariaPadrao);
    }

    public static void Alterar(Color cor) {
      Definir(cor);
      Salvar(cor);
    }

    static void Definir(Color cor) {
      LmTema.CorPrimaria = cor;
      LmCorbieUI.Design.LmCor.CorPrimaria = System.Drawing.Color.FromArgb(cor.R, cor.G, cor.B);
    }

    static Color? Carregar() {
      try {
        if (!File.Exists(LocalConfig))
          return null;

        var valor = File.ReadAllLines(LocalConfig, Encoding.UTF8)
          .Select(l => (l ?? string.Empty).Split(new[] { '=' }, 2))
          .Where(p => p.Length == 2 && p[0].Trim().Equals("cor_primaria", StringComparison.OrdinalIgnoreCase))
          .Select(p => p[1].Trim())
          .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(valor))
          return null;

        return (Color)ColorConverter.ConvertFromString(valor);
      } catch (Exception) {
        return null;
      }
    }

    static void Salvar(Color cor) {
      try {
        Directory.CreateDirectory(Path.GetDirectoryName(LocalConfig));
        File.WriteAllLines(LocalConfig, new[] { $"cor_primaria=#{cor.R:X2}{cor.G:X2}{cor.B:X2}" }, Encoding.UTF8);
      } catch (Exception ex) {
        LmCorbieUI.Wpf.Toast.Warning("Não foi possível salvar a cor do tema.\r\n" + ex.Message);
      }
    }
  }
}
