using LmCorbieUI;
using LmCorbieUI.Wpf.Design;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace AxionConnect.Wpf.Services {
  /// <summary>
  /// Cor do tema (LmTema.CorPrimaria): a paleta inteira da LmCorbieUI.Wpf sai dela e as janelas abertas
  /// se atualizam na hora. Também vale para as janelas WinForms da LmCorbieUI (LmCor).
  /// A escolha fica salva nas predefinições locais do usuário (aparencia.lm, chave=valor), como no Addin.
  /// </summary>
  internal static class Aparencia {
    public record Opcao(string Nome, Color Cor);

    public static readonly Opcao[] Opcoes = {
      new Opcao("Padrão (azul petróleo)", LmPaleta.CorPrimariaPadrao),
      new Opcao("Azul", Color.FromRgb(21, 67, 120)),
      new Opcao("Verde", Color.FromRgb(0, 94, 58)),
      new Opcao("Roxo", Color.FromRgb(60, 50, 120)),
      new Opcao("Vinho", Color.FromRgb(120, 30, 40)),
      new Opcao("Terracota", Color.FromRgb(150, 70, 30)),
      new Opcao("Grafite", Color.FromRgb(55, 60, 66)),
    };

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
          .Select(l => (l ?? string.Empty).Split('=', 2, StringSplitOptions.TrimEntries))
          .Where(p => p.Length == 2 && p[0].Equals("cor_primaria", StringComparison.OrdinalIgnoreCase))
          .Select(p => p[1])
          .FirstOrDefault();

        return string.IsNullOrWhiteSpace(valor) ? null : (Color)ColorConverter.ConvertFromString(valor);
      } catch (Exception) {
        return null;
      }
    }

    static void Salvar(Color cor) {
      try {
        Directory.CreateDirectory(Path.GetDirectoryName(LocalConfig));
        File.WriteAllLines(LocalConfig, new[] { $"cor_primaria=#{cor.R:X2}{cor.G:X2}{cor.B:X2}" }, Encoding.UTF8);
      } catch (Exception ex) {
        Notificacao.Aviso("Não foi possível salvar a cor do tema.\n" + ex.Message);
      }
    }
  }
}
