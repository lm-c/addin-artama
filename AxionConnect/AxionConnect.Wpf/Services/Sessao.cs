using AxionConnect.Wpf.Controls;
using AxionConnect.Wpf.Views;
using LmCorbieUI;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace AxionConnect.Wpf.Services {
  /// <summary>
  /// Troca entre a janela de login e a principal (antigos Program.Main, FrmLogin.Logar e FrmPrincipal_FormClosing).
  /// </summary>
  internal static class Sessao {
    public const string NomeSistema = "Axion Connect Artama";

    /// <summary>A mesma janela de login volta no logout (como o FrmLogin.Instancia), sem novo login automático.</summary>
    static LoginWindow login;

    /// <summary>Abertura do app: TLS, pastas padrão da LmCorbieUI e a cor do tema (o login já abre com ela).</summary>
    public static void Iniciar() {
      // Forçar o uso de TLS 1.2 (ou versões anteriores se necessário)
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      // Ignorar erros de certificado SSL
      ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

      DefinirVariaveisAmbiente();
      Aparencia.AplicarSalva();
    }

    /// <summary>Variáveis de ambiente da LmCorbieUI (mesmos valores do FrmLogin).</summary>
    public static void DefinirVariaveisAmbiente() =>
      ValPadrao.DefinirPadrao("LM Projetos Data", NomeSistema, "Artama", "michalakleo@gmail.com");

    public static void MostrarLogin() {
      login ??= new LoginWindow();
      Application.Current.MainWindow = login;
      login.Show();
    }

    /// <summary>
    /// Carrega o ambiente após o usuário ser validado (FrmLogin.Logar).
    /// Retorna false se a configuração da API não existir.
    /// </summary>
    public static bool CarregarAmbiente() {
      DefinirVariaveisAmbiente();

      // Carregar Globais
      var configAPI = configuracao_api.Selecionar();
      if (configAPI == null) {
        Notificacao.Erro("Configuração da API não encontrada.\nLogin cancelado.");
        return false;
      }

      Api.token = configAPI.token;
      Api.chave_edrawings = configAPI.chave_edrawings;
      Api.url = configAPI.endereco;
      Api.codigoEmpresa = configAPI.codigoEmpresa;
      EDrawingsViewer.ClsidEDrawings = configAPI.chave_edrawings;

      templates.Carregar();
      InfoSetting.Carregar();
      configuracao_sistema.Carregar();
      return true;
    }

    public static void AbrirPrincipal() {
      Log.Info("Abrindo a janela principal");
      var principal = new MainWindow();
      Application.Current.MainWindow = principal;
      principal.Show();
      login?.Hide();
    }

    /// <summary>Logout: fecha a principal e volta ao login (o usuário continua alocado nesta máquina, como antes).</summary>
    public static void Logout(Window principal) {
      MostrarLogin();
      principal.Close();
    }

    public static void Encerrar() {
      Log.Info("Encerrando o sistema. Origem:\n" + Environment.StackTrace);
      Application.Current.Shutdown();
    }
  }
}
