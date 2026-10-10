using LmCorbieUI;
using LmCorbieUI.Metodos;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Tela de login em WPF (substitui o FrmLogin).</summary>
  public partial class ViewLogin : Wpf.LmView {
    readonly string userPC;
    readonly string hostname;

    bool inicializado;

    public ViewLogin() {
      InitializeComponent();

      imgLogo.Source = UcPainelTarefas.Logo;

      userPC = Environment.UserName;
      hostname = Dns.GetHostName();

      Loaded += ViewLogin_Loaded;
    }

    private void ViewLogin_Loaded(object sender, RoutedEventArgs e) {
      if (inicializado)
        return;

      inicializado = true;
      Carregando = true;

      // Deixa o overlay "Carregando..." aparecer antes de acessar o banco.
      Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(LoginAutomatico));
    }

    /// <summary>Usuário já alocado neste computador entra direto.</summary>
    private void LoginAutomatico() {
      try {
        if (Web.IsConnected()) {
          using (ContextoDados db = new ContextoDados()) {
            var usuAlocado = Queryable.FirstOrDefault(
              from x in db.usuario_alocados.Where(x => x.hostname == hostname && x.usuario_pc == userPC)
              select new { x, x.usuario });

            if (usuAlocado != null) {
              usuario_alocados.model = usuAlocado.x;
              usuario_alocados.model.usuario = usuAlocado.x.usuario;
              txtUsuario.Text = usuario_alocados.model.usuario.login;
              txtSenha.Senha = usuario_alocados.model.usuario.senha.DescriptografarAES();

              Logar();
            } else {
              txtUsuario.Focus();
            }
          }
        } else {
          Wpf.Toast.Warning("Sem Internet");
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao inicializar login");
      } finally {
        Carregando = false;
      }
    }

    /// <summary>Enter no usuário vai para a senha; Enter na senha entra.</summary>
    protected override void OnPreviewKeyDown(KeyEventArgs e) {
      base.OnPreviewKeyDown(e);

      if (e.Key != Key.Enter)
        return;

      if (txtUsuario.IsKeyboardFocusWithin) {
        e.Handled = true;
        txtSenha.Focus();
      } else if (txtSenha.IsKeyboardFocusWithin) {
        e.Handled = true;
        Logar();
      }
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e) {
      Logar();
    }

    /// <summary>Limpa a senha para o próximo login (logout).</summary>
    internal void LimparSenha() {
      txtSenha.Clear();
    }

    private void Logar() {
      if (!LmValidacao.Validar(this))
        return;

      using (ContextoDados db = new ContextoDados()) {
        var login = txtUsuario.Text;

        try {
          Carregando = true;

          var senha = db.usuarios.FirstOrDefault(x => x.login == login)?.senha;

          if (senha != txtSenha.Senha.CriptografarAES()) {
            Wpf.MsgBox.Show("Usuário ou Senha Inválido.", "Mensagem", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
          }

          var usu = db.usuarios.Where(x => x.login == login).FirstOrDefault();

          if (usu != null) {
            if (!usu.ativo) {
              Wpf.Toast.Warning("Usuário Inativo!");
              return;
            }
          } else {
            Wpf.Toast.Error("Retornou Usuário Inválido.\nLogin Cancelado.");
            return;
          }

          if (usuario_alocados.model == null) {
            usuario_alocados.model = new usuario_alocados {
              usuario_pc = Environment.UserName,
              hostname = Dns.GetHostName(),
              usuario_id = usu.id,
            };

            db.usuario_alocados.Add(usuario_alocados.model);
            db.SaveChanges();
          }

          UcPainelTarefas.Instancia.ConfigurarPermissoes();

          // Carregar Variáveis de Ambiente
          ValPadrao.DefinirPadrao("LM Projetos Data", "Axion Artama", "Artama", "michalakleo@gmail.com");

          UcPainelTarefas.Instancia.Carregarrodape(InfoAssembly.Version, usu.login);

          // Carregar Globais
          var configAPI = configuracao_api.Selecionar();

          Api.token = configAPI.token;
          Api.chave_edrawings = configAPI.chave_edrawings;
          Api.url = configAPI.endereco;
          Api.codigoEmpresa = configAPI.codigoEmpresa;

          _ = Processo.Carregar();
          _ = ProcessoNaoSeriado.Carregar();
          templates.Carregar();
          InfoSetting.Carregar();

          UcPainelTarefas.Instancia.LoginConcluido();
        } catch (Exception ex) {
          LmException.ShowException(ex, "Erro ao Logar no Sistema");

          txtUsuario.Focus();
        } finally {
          Carregando = false;
        }
      }
    }
  }
}
