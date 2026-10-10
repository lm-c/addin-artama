using eDrawings.Interop.EModelViewControl;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms.Integration;

namespace AxionConnect.Wpf.Controls {
  /// <summary>
  /// Hospeda o ActiveX do eDrawings dentro do WPF.
  /// Por ser uma janela Win32, ele sempre fica por cima do conteúdo WPF na mesma área
  /// (não é possível sobrepor botões ou avisos WPF em cima do desenho).
  /// Se o eDrawings não estiver instalado/registrado, mostra um aviso no lugar em vez de derrubar a tela.
  /// </summary>
  public class EDrawingsViewer : WindowsFormsHost {
    // CLSID do controle eDrawings independente de versão (EModelView.EModelNonVersionSpecificViewControl)
    public const string ClsidPadrao = "22945A69-1191-4DCF-9E6F-409BDE94D101";
    public const string ProgId = "EModelView.EModelNonVersionSpecificViewControl";

    private static string _clsid = ClsidPadrao;

    /// <summary>
    /// CLSID usado ao criar o controle (configuracao_api.chave_edrawings, carregado no login).
    /// Aceita com ou sem chaves; vazio ou inválido volta ao padrão.
    /// </summary>
    public static string ClsidEDrawings {
      get => _clsid;
      set => _clsid = Guid.TryParse(value?.Trim(), out var guid) ? guid.ToString("D").ToUpperInvariant() : ClsidPadrao;
    }

    public EModelViewControl Controle { get; private set; }

    /// <summary>O eDrawings não pôde ser criado: no lugar fica um aviso (WinForms, também por cima do WPF).</summary>
    public bool Indisponivel { get; private set; }

    public event Action Carregado;

    /// <summary>Disparado quando o visualizador passa a mostrar o aviso de indisponível.</summary>
    public event Action IndisponivelAlterado;

    public EDrawingsViewer() {
      if (!VerificarRegistro(ClsidEDrawings, out var problema, out _)) {
        MostrarIndisponivel(problema);
        return;
      }

      var host = new EDrawingsHost(ClsidEDrawings);
      host.ControleCarregado += ctrl => {
        Controle = ctrl;
        Carregado?.Invoke();
      };
      // Adiado: a falha acontece enquanto o WPF cria a janela nativa do host.
      host.Falhou += problema => Dispatcher.BeginInvoke(new Action(() => MostrarIndisponivel(problema)));
      Child = host;
    }

    /// <summary>
    /// O registro do COM guarda o caminho completo da EModelView.dll: se a pasta do eDrawings for
    /// renomeada/movida ou a instalação estiver incompleta, a criação do controle falha com "Classe não registrada".
    /// </summary>
    public static bool VerificarRegistro(string clsid, out string problema, out string caminho) {
      problema = null;
      caminho = null;
      try {
        using (var chave = Registry.ClassesRoot.OpenSubKey($@"CLSID\{{{clsid}}}\InprocServer32")) {
          caminho = chave?.GetValue(string.Empty) as string;

          if (string.IsNullOrWhiteSpace(caminho)) {
            problema = "O eDrawings não está instalado (ou não está registrado) neste computador.";
            return false;
          }

          caminho = Environment.ExpandEnvironmentVariables(caminho.Trim('"'));
          if (!File.Exists(caminho)) {
            problema = $"O eDrawings está registrado em uma pasta que não existe mais:\n{caminho}\n\n" +
                       "Faça o Reparar da instalação do eDrawings (Painel de Controle > Programas).";
            return false;
          }

          return true;
        }
      } catch (Exception ex) {
        // Sem acesso ao registro: tenta criar o controle mesmo assim.
        System.Diagnostics.Debug.WriteLine(ex.Message);
        return true;
      }
    }

    /// <summary>CLSID que o instalador do eDrawings registrou para o <see cref="ProgId"/>; null se não houver.</summary>
    public static string ClsidDoRegistro() {
      try {
        using (var chave = Registry.ClassesRoot.OpenSubKey($@"{ProgId}\CLSID")) {
          var valor = chave?.GetValue(string.Empty) as string;
          return Guid.TryParse(valor?.Trim(), out var guid) ? guid.ToString("D").ToUpperInvariant() : null;
        }
      } catch {
        return null;
      }
    }

    /// <summary>Versão da EModelView.dll registrada (ex.: 32.5.0.0030 = eDrawings 2024 SP5); null se não achar.</summary>
    public static string VersaoInstalada(string clsid = null) {
      try {
        if (!VerificarRegistro(clsid ?? ClsidEDrawings, out _, out var caminho) || caminho == null)
          return null;
        return FileVersionInfo.GetVersionInfo(caminho).FileVersion;
      } catch {
        return null;
      }
    }

    private void MostrarIndisponivel(string problema) {
      var versao = VersaoInstalada();
      problema += $"\n\nCLSID: {ClsidEDrawings}\neDrawings instalado: {versao ?? "não identificado"} · processo {(Environment.Is64BitProcess ? "64" : "32")} bits";
      Services.Log.Erro("eDrawings indisponível: " + problema);

      Controle = null;
      Indisponivel = true;
      Child = new System.Windows.Forms.Label {
        Dock = System.Windows.Forms.DockStyle.Fill,
        TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
        BackColor = System.Drawing.Color.FromArgb(0xdf, 0xe4, 0xe9),
        Text = "Visualizador eDrawings indisponível.\n\n" + problema,
      };

      // Adiado: pode estar dentro de um layout do WPF, onde não é permitido abrir janelas (toast).
      Dispatcher.BeginInvoke(new Action(() => {
        IndisponivelAlterado?.Invoke();
        Services.Notificacao.Erro("Erro ao carregar o controle eDrawings:\n" + problema);
      }));
    }

    private class EDrawingsHost : System.Windows.Forms.AxHost {
      private bool _carregado;
      private bool _falhou;
      public event Action<EModelViewControl> ControleCarregado;
      public event Action<string> Falhou;

      public EDrawingsHost(string clsid) : base(clsid) { }

      /// <summary>A criação do ActiveX acontece aqui; uma falha não pode escapar para o laço de mensagens.</summary>
      protected override void CreateHandle() {
        if (_falhou)
          return;

        try {
          base.CreateHandle();
        } catch (Exception ex) {
          _falhou = true;
          Falhou?.Invoke(Detalhe("criar o controle", ex));
        }
      }

      protected override void OnCreateControl() {
        base.OnCreateControl();
        if (_carregado || _falhou)
          return;

        _carregado = true;
        try {
          var ocx = GetOcx();
          if (ocx is EModelViewControl ctrl) {
            ControleCarregado?.Invoke(ctrl);
          } else if (ocx == null) {
            Falhou?.Invoke("o controle não foi criado (GetOcx vazio).");
          } else {
            // Criou o ActiveX, mas ele não responde à interface usada na compilação: versão do eDrawings diferente.
            Falhou?.Invoke("o controle foi criado, mas não é compatível com esta versão do Axion Connect " +
                           "(versão do eDrawings diferente da usada na compilação).");
          }
        } catch (Exception ex) {
          Falhou?.Invoke(Detalhe("inicializar o controle", ex));
        }
      }

      private static string Detalhe(string etapa, Exception ex) {
        Services.Log.Erro($"eDrawings: falha ao {etapa}", ex);
        var interna = ex.InnerException != null ? $"\n{ex.InnerException.Message}" : string.Empty;
        return $"falha ao {etapa}: {ex.Message} (0x{ex.HResult:X8}){interna}";
      }
    }
  }
}
