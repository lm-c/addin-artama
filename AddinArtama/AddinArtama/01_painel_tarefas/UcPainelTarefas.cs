using LmCorbieUI;
using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MediaColor = System.Windows.Media.Color;
using Wpf = LmCorbieUI.Wpf;
using WpfCtl = System.Windows.Controls;

namespace AddinArtama {
  /// <summary>
  /// Task Pane do add-in. O conteúdo é a casca WPF (LmPainel: menu, telas, rodapé) num único LmWpfHost.
  /// </summary>
  [ComVisible(true)]
  [ProgId(SWTASKPANE_PROGID)]
  public partial class UcPainelTarefas : UserControl {
    public const string SWTASKPANE_PROGID = "AddinArtama.SWTaskPane.Plugin";

    static UcPainelTarefas instancia;

    public static UcPainelTarefas Instancia {
      get {
        if (instancia == null)
          instancia = new UcPainelTarefas();

        return instancia;
      }
    }

    /// <summary>Logo da Artama (fundo do painel e tela de login).</summary>
    internal static readonly ImageSource Logo = CarregarLogo();

    readonly Wpf.LmPainel painel;
    ViewLogin login;

    Wpf.LmItemMenu miProcess, miProperties;
    Wpf.LmItemMenu miDesenho, miCriarDesenho, miAtualizarDesenho;
    Wpf.LmItemMenu miExportar, miExportarPDF, miExportarDXF;
    Wpf.LmItemMenu miCadastros, miUsuarioCad, miPerfilCad, miMaterialCad, miMateriaPrimaCad, miRedefinirSenha;
    Wpf.LmItemMenu miRelatorio, miProcessoFabricacao, miPackList, miPlanoPintura, miManutPacklistPlanoDePintura, miReportWorks;
    Wpf.LmItemMenu miCSW, miCadastroProduto, miConfiguracaoProcesso, miConfiguracaoIntegracao;
    Wpf.LmItemMenu miConfig, miConfigTemplate, miAparencia;
    Wpf.LmItemMenu miLogout;

    public UcPainelTarefas() {
      InitializeComponent();

      // Forçar o uso de TLS 1.2 (ou versões anteriores se necessário)
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      // Ignorar erros de certificado SSL
      ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

      // Variáveis de ambiente (também usadas pela cor do tema salva)
      ValPadrao.DefinirPadrao("LM Projetos Data", "Axion Artama", "Artama", "michalakleo@gmail.com");

      // Cor do tema salva pelo usuário (no banco de teste fica fixa em vermelho)
      Aparencia.AplicarSalva();

      painel = new Wpf.LmPainel {
        MostrarMenu = false,
        ImagemFundo = Logo,
      };

      MontarMenu();

      Controls.Add(new Wpf.LmWpfHost(painel));
    }

    static ImageSource CarregarLogo() {
      try {
        var logo = new BitmapImage();
        logo.BeginInit();
        logo.UriSource = new Uri("pack://application:,,,/AddinArtama;component/Resources/logo_fundo.png");
        logo.CacheOption = BitmapCacheOption.OnLoad;
        logo.EndInit();
        logo.Freeze();

        return logo;
      } catch (Exception) {
        return null;
      }
    }

    private void UcPainelTarefas_Load(object sender, EventArgs e) {
      instancia = this;

      login = new ViewLogin();
      AbrirTela(login);
    }

    #region Menu

    void MontarMenu() {
      miProcess = new Wpf.LmItemMenu("Aplicação de Processos/Matéria Prima", "", AbrirTela<ViewMateriaPrimaApl>);
      miProperties = new Wpf.LmItemMenu("Propriedades Personalizadas", "", AbrirTela<ViewFileProperties>);

      miDesenho = new Wpf.LmItemMenu("Desenhos", "");
      miCriarDesenho = new Wpf.LmItemMenu("Criar/Alterar Desenhos", "", AbrirTela<ViewDesenho>);
      miAtualizarDesenho = new Wpf.LmItemMenu("Atualizar Templates dos Desenhos", "", AbrirTela<ViewFormatosAtualizar>);
      miDesenho.SubItens.Add(miCriarDesenho);
      miDesenho.SubItens.Add(miAtualizarDesenho);

      miExportar = new Wpf.LmItemMenu("Exportar Arquivos", "");
      miExportarPDF = new Wpf.LmItemMenu("Exportar PDF/DWG", "", AbrirTela<ViewExportarPDF>);
      miExportarDXF = new Wpf.LmItemMenu("Exportar DXF", "", AbrirTela<ViewExportarDXF>);
      miExportar.SubItens.Add(miExportarPDF);
      miExportar.SubItens.Add(miExportarDXF);

      miCadastros = new Wpf.LmItemMenu("Cadastros Gerais", "");
      miUsuarioCad = new Wpf.LmItemMenu("Cadastro de Usuário", "", AbrirTela<ViewUsuarioCad>);
      miPerfilCad = new Wpf.LmItemMenu("Cadastro de Perfil do Usuário", "", AbrirTela<ViewPerfil>);
      miMaterialCad = new Wpf.LmItemMenu("Cadastro de Material", "", AbrirTela<ViewMaterialCad>);
      miMateriaPrimaCad = new Wpf.LmItemMenu("Cadastro de Matéria Prima", "", AbrirTela<ViewMateriaPrimaCad>);
      miRedefinirSenha = new Wpf.LmItemMenu("Redefinir Senha", "", AbrirTela<ViewAlterarSenha>);
      miCadastros.SubItens.Add(miUsuarioCad);
      miCadastros.SubItens.Add(miPerfilCad);
      miCadastros.SubItens.Add(miMaterialCad);
      miCadastros.SubItens.Add(miMateriaPrimaCad);
      miCadastros.SubItens.Add(miRedefinirSenha);

      miRelatorio = new Wpf.LmItemMenu("Relatórios", "");
      miProcessoFabricacao = new Wpf.LmItemMenu("Processo de Fabricação", "", AbrirTela<ViewProcessoFabricacao>);
      miPackList = new Wpf.LmItemMenu("Pack List", "", AbrirTela<ViewPackList>);
      miPlanoPintura = new Wpf.LmItemMenu("Plano de Pintura", "", AbrirTela<ViewPlanoPintura>);
      miManutPacklistPlanoDePintura = new Wpf.LmItemMenu("Manutenção Packlist e Plano de Pintura", "", AbrirTela<ViewManutProps>);
      miReportWorks = new Wpf.LmItemMenu("Report Works", "", () => new WindowReportWorks().Show());
      miRelatorio.SubItens.Add(miProcessoFabricacao);
      miRelatorio.SubItens.Add(miPackList);
      miRelatorio.SubItens.Add(miPlanoPintura);
      miRelatorio.SubItens.Add(miManutPacklistPlanoDePintura);
      miRelatorio.SubItens.Add(miReportWorks);

      miCSW = new Wpf.LmItemMenu("Integração ERP", "");
      miCadastroProduto = new Wpf.LmItemMenu("Cadastro de Produto/Engenharia", "", AbrirTela<ViewProdutoImport>);
      miConfiguracaoProcesso = new Wpf.LmItemMenu("Configuração de Processo", "", AbrirTela<ViewProcessoCad>);
      miConfiguracaoIntegracao = new Wpf.LmItemMenu("Configuração de Integração", "", () => new WindowConfigIntegrador().ShowDialog());
      miCSW.SubItens.Add(miCadastroProduto);
      miCSW.SubItens.Add(miConfiguracaoProcesso);
      miCSW.SubItens.Add(miConfiguracaoIntegracao);

      miConfig = new Wpf.LmItemMenu("Configuração", "") { Direita = true };
      miConfigTemplate = new Wpf.LmItemMenu("Configurar Templates", "", () => new WindowConfigTemplate().ShowDialog());
      miAparencia = new Wpf.LmItemMenu("Aparência", "", AbrirMenuAparencia);
      miConfig.SubItens.Add(miConfigTemplate);
      miConfig.SubItens.Add(miAparencia);

      miLogout = new Wpf.LmItemMenu("Logout", "", Logout) { Direita = true };

      painel.Itens.Add(miProcess);
      painel.Itens.Add(miProperties);
      painel.Itens.Add(miDesenho);
      painel.Itens.Add(miExportar);
      painel.Itens.Add(miCadastros);
      painel.Itens.Add(miRelatorio);
      painel.Itens.Add(miCSW);
      // À direita: o primeiro adicionado fica mais à direita.
      painel.Itens.Add(miLogout);
      painel.Itens.Add(miConfig);
    }

    internal void ConfigurarPermissoes() {
      if (InvokeRequired) {
        Invoke(new MethodInvoker(ConfigurarPermissoes));
        return;
      }
      try {
        if (usuario_alocados.model.usuario_id == -1)
          return;

        Corbie_Admin.PermissoesPerfil = usuarios.SelecionarPermissoes(usuario_alocados.model.usuario_id);

        //Menu Controle
        miProcess.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.AplicacaoProcesso);

        miProperties.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.PropsPersonalizadas);

        miDesenho.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.Desenho);
        miCriarDesenho.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.CriarAlterarDesenhos);
        miAtualizarDesenho.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.AtualizarTemplatesDesenhos);

        miExportar.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.Exportar);
        miExportarPDF.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.ExportarPDF);
        miExportarDXF.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.ExportarDXF);

        miCadastros.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.Cadastros);
        miUsuarioCad.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.UsuarioCad);
        miPerfilCad.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.PerfilUsuarioCad);
        miMaterialCad.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.MaterialCad);
        miMateriaPrimaCad.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.MateriaPrimaCad);
        miRedefinirSenha.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.SenhaRedefinir);

        miRelatorio.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.Relatorios);
        miProcessoFabricacao.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.ProcessoFabricacao);
        miPackList.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.PackList);
        miPlanoPintura.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.PlanoPintura);
        miManutPacklistPlanoDePintura.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.ManutencaoPinturaPackList);
        miReportWorks.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.ReportWorks);

        miCSW.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.menuIntegracao);
        miCadastroProduto.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.produtoCad);
        miConfiguracaoProcesso.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.processoConfig);
        miConfiguracaoIntegracao.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.IntegracaoConfig);

        miConfig.Visivel = Corbie_Admin.TemPermissao(PermissoesSistema.Configuracao);
      } catch (Exception ex) {
        Wpf.MsgBox.Show("Erro Crítico!!!\nSuas Permissões do sistema precisa ser redefinida.",
            "Erro Crítico", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        LmException.ShowException(ex, "Erro Permissão.", true);
      }
    }

    /// <summary>Menu de cores do tema: opções prontas, a atual marcada, e uma cor personalizada.</summary>
    void AbrirMenuAparencia() {
      var menu = new WpfCtl.ContextMenu {
        PlacementTarget = painel,
        Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint,
        Style = (System.Windows.Style)painel.FindResource(Wpf.LmPainel.EstiloSubmenuKey),
      };
      Wpf.Design.LmTema.Aplicar(menu);

      if (Aparencia.AmbienteTeste) {
        AdicionarItemAparencia(menu, "Ambiente de teste: cor fixa em vermelho", null).IsEnabled = false;
        menu.IsOpen = true;
        return;
      }

      var atual = Wpf.Design.LmTema.CorPrimaria;
      foreach (var opcao in Aparencia.Opcoes) {
        var cor = opcao.Cor;
        AdicionarItemAparencia(menu, CabecalhoCor(opcao.Nome, cor, cor == atual), () => Aparencia.Alterar(cor));
      }

      var personalizada = !Aparencia.Opcoes.Any(x => x.Cor == atual);
      AdicionarItemAparencia(menu, CabecalhoCor("Cor personalizada...", personalizada ? atual : (MediaColor?)null, personalizada), () => {
        using (var dialogo = new ColorDialog {
          FullOpen = true,
          Color = System.Drawing.Color.FromArgb(atual.R, atual.G, atual.B),
        }) {
          if (dialogo.ShowDialog() == DialogResult.OK)
            Aparencia.Alterar(MediaColor.FromRgb(dialogo.Color.R, dialogo.Color.G, dialogo.Color.B));
        }
      });

      menu.IsOpen = true;
    }

    /// <summary>Amostra da cor + nome + ✓ na cor atual.</summary>
    static object CabecalhoCor(string nome, MediaColor? cor, bool atual) {
      var linha = new WpfCtl.StackPanel { Orientation = WpfCtl.Orientation.Horizontal };
      linha.Children.Add(new WpfCtl.Border {
        Width = 16, Height = 16, CornerRadius = new System.Windows.CornerRadius(3), BorderThickness = new System.Windows.Thickness(1),
        BorderBrush = Brushes.Gray,
        Background = cor.HasValue ? new SolidColorBrush(cor.Value) : (Brush)Brushes.Transparent,
        Margin = new System.Windows.Thickness(0, 0, 10, 0), VerticalAlignment = System.Windows.VerticalAlignment.Center,
      });
      linha.Children.Add(new WpfCtl.TextBlock { Text = nome, VerticalAlignment = System.Windows.VerticalAlignment.Center, MinWidth = 150 });
      if (atual)
        linha.Children.Add(new WpfCtl.TextBlock {
          Text = "", FontFamily = new FontFamily("Segoe MDL2 Assets"), VerticalAlignment = System.Windows.VerticalAlignment.Center,
        });
      return linha;
    }

    WpfCtl.MenuItem AdicionarItemAparencia(WpfCtl.ContextMenu menu, object cabecalho, Action acao) {
      var item = new WpfCtl.MenuItem {
        Header = cabecalho,
        Style = (System.Windows.Style)painel.FindResource(Wpf.LmPainel.EstiloItemSubmenuKey),
      };
      if (acao != null)
        item.Click += (s, e) => acao();
      menu.Items.Add(item);
      return item;
    }

    #endregion

    #region Telas

    /// <summary>Abre uma tela WPF na casca (se já estiver aberta, traz para frente).</summary>
    internal void AbrirTela(Wpf.LmView view) {
      try {
        painel.AbrirTela(view);
      } catch (Exception ex) {
        Wpf.Toast.Error("Erro ao Abrir Tela.\r\n" + ex.ToString());
      }
    }

    /// <summary>Abre a tela WPF do tipo T, criando-a só se ainda não estiver aberta.</summary>
    internal void AbrirTela<T>() where T : Wpf.LmView, new() {
      try {
        painel.AbrirTela<T>();
      } catch (Exception ex) {
        Wpf.Toast.Error("Erro ao Abrir Tela.\r\n" + ex.ToString());
      }
    }

    #endregion

    #region Login

    /// <summary>Chamado pela ViewLogin após autenticar: esconde o login e libera o menu.</summary>
    internal void LoginConcluido() {
      painel.OcultarTela(login);
      painel.MostrarMenu = true;
    }

    internal void Carregarrodape(string version, string user) {
      painel.TextoRodape = $"Versão Atual: {version} | Usuário: {user}";
    }

    void Logout() {
      try {
        usuario_alocados.Deslogar();

        painel.FecharTodas(login);
        painel.MostrarMenu = false;
        painel.TextoRodape = null;

        login.LimparSenha();
        AbrirTela(login);
      } catch (Exception) {
        Wpf.Toast.Error("Erro ao Fazer Logout");
      }
    }

    #endregion
  }
}
