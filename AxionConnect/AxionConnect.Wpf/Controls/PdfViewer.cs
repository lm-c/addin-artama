using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AxionConnect.Wpf.Controls {
  /// <summary>
  /// Visualizador de PDF (leitor de PDF do Edge via WebView2, já presente no Windows 10/11).
  /// Mantém o último PDF carregado: alternar entre o PDF e o eDrawings não recarrega nada.
  /// Assim como o eDrawings, é uma janela nativa e fica por cima do conteúdo WPF na mesma área.
  /// </summary>
  public class PdfViewer : ContentControl {
    private readonly WebView2 _web;
    private Task _inicializacao;

    /// <summary>Só a navegação pedida por <see cref="Abrir"/> é aceita (Alt+← e links do PDF ficam bloqueados).</summary>
    private bool _navegacaoPermitida;

    /// <summary>PDF carregado no momento (continua em memória enquanto o visualizador estiver escondido).</summary>
    public string CaminhoAberto { get; private set; }

    public PdfViewer() {
      _web = new WebView2 {
        // A pasta padrão fica ao lado do .exe (Arquivos de Programas, sem permissão de escrita)
        CreationProperties = new CoreWebView2CreationProperties {
          UserDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lm Projetos", "AxionConnect", "WebView2"),
        },
      };
      Content = _web;
    }

    /// <summary>Inicia o WebView2 uma única vez (o primeiro PDF não espera a criação do navegador).</summary>
    public Task Inicializar() => _inicializacao ??= InicializarAsync();

    /// <summary>Inicializa em segundo plano; uma falha só é avisada quando o usuário pedir um PDF.</summary>
    public async void PreCarregar() {
      try {
        await Inicializar();
      } catch (Exception ex) {
        Services.Log.Erro("WebView2 (visualizador de PDF) indisponível", ex);
      }
    }

    private async Task InicializarAsync() {
      await _web.EnsureCoreWebView2Async();

      var core = _web.CoreWebView2;
      core.Settings.AreDevToolsEnabled = false;
      core.Settings.IsStatusBarEnabled = false;
      core.Settings.IsSwipeNavigationEnabled = false;
      core.NavigationStarting += (s, e) => {
        if (!_navegacaoPermitida)
          e.Cancel = true;
        _navegacaoPermitida = false;
      };
      core.NewWindowRequested += (s, e) => e.Handled = true;
    }

    /// <summary>Exibe o PDF; se já for o carregado, não faz nada.</summary>
    public async Task Abrir(string caminho) {
      await Inicializar();
      if (string.Equals(caminho, CaminhoAberto, StringComparison.OrdinalIgnoreCase))
        return;

      CaminhoAberto = caminho;
      _navegacaoPermitida = true;
      _web.CoreWebView2.Navigate(UriArquivo(caminho));
    }

    /// <summary>file:/// com cada parte escapada ("#", "%" e espaços no nome não quebram a URL).</summary>
    private static string UriArquivo(string caminho) {
      caminho = Path.GetFullPath(caminho);

      // Caminho de rede \\servidor\pasta\... → file://servidor/pasta/...
      if (caminho.StartsWith(@"\\")) {
        var rede = caminho.Substring(2).Split('\\');
        return "file://" + rede[0] + "/" + string.Join("/", rede.Skip(1).Select(Uri.EscapeDataString));
      }

      var partes = caminho.Split('\\');
      return "file:///" + partes[0] + "/" + string.Join("/", partes.Skip(1).Select(Uri.EscapeDataString));
    }
  }
}
