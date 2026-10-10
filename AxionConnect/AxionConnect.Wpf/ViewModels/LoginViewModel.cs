using AxionConnect.Wpf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using LmCorbieUI;
using LmCorbieUI.Metodos;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lm = LmCorbieUI.Wpf;

namespace AxionConnect.Wpf.ViewModels {
  /// <summary>Login (antigo FrmLogin): mesmas regras de validação e de alocação do usuário na máquina.</summary>
  internal partial class LoginViewModel : ObservableObject {
    readonly string userPC = Environment.UserName;
    readonly string hostname = Dns.GetHostName();

    [ObservableProperty]
    private string _usuario = string.Empty;

    [ObservableProperty]
    private bool _ocupado;

    [ObservableProperty]
    private string _mensagemOcupado = "Carregando...";

    /// <summary>Usuário e senha do login automático (a senha vai para o campo da janela).</summary>
    public event Action<string> SenhaAutomatica;

    /// <summary>Disparado quando o login foi concluído e o ambiente carregado.</summary>
    public event Action LoginConcluido;

    /// <summary>Se esta máquina já tem um usuário alocado, entra direto (FrmLogin_Loaded).</summary>
    public async Task TentarLoginAutomaticoAsync() {
      try {
        Ocupado = true;
        MensagemOcupado = "Verificando sessão...";

        var conectado = await Task.Run(Web.IsConnected);
        Log.Info($"Login automático: internet = {conectado}");
        if (!conectado) {
          Notificacao.Aviso("Sem Internet");
          return;
        }

        var alocado = await Task.Run(() => {
          using (ContextoDados db = new ContextoDados()) {
            return Queryable.FirstOrDefault(
              from x in db.usuario_alocados.Where(x => x.hostname == hostname && x.usuario_pc == userPC)
              select new { x, x.usuario });
          }
        });

        Log.Info($"Login automático: usuário alocado nesta máquina = {alocado?.usuario?.login ?? "nenhum"}");
        if (alocado == null)
          return;

        usuario_alocados.model = alocado.x;
        usuario_alocados.model.usuario = alocado.x.usuario;
        Usuario = usuario_alocados.model.usuario.login;

        var senha = usuario_alocados.model.usuario.senha.DescriptografarAES();
        SenhaAutomatica?.Invoke(senha);

        await EntrarAsync(senha);
      } catch (Exception ex) {
        Log.Erro("Erro ao inicializar login", ex);
        LmException.ShowException(ex, "Erro ao inicializar login");
      } finally {
        Ocupado = false;
      }
    }

    /// <summary>FrmLogin.Logar: valida usuário/senha, aloca o usuário nesta máquina e carrega o ambiente.</summary>
    public async Task EntrarAsync(string senhaDigitada) {
      try {
        Ocupado = true;
        MensagemOcupado = "Entrando...";

        var login = Usuario;
        Log.Info($"Login: validando usuário '{login}'");
        var usu = await Task.Run(() => {
          using (ContextoDados db = new ContextoDados()) {
            var senha = db.usuarios.FirstOrDefault(x => x.login == login)?.senha;
            if (senha != senhaDigitada.CriptografarAES())
              return (usuarios)null;

            return db.usuarios.Where(x => x.login == login).FirstOrDefault();
          }
        });

        if (usu == null) {
          Log.Info("Login: usuário ou senha inválido");
          Lm.MsgBox.Show("Usuário ou Senha Inválido.", "Mensagem", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
          return;
        }

        if (!usu.ativo) {
          Log.Info("Login: usuário inativo");
          Notificacao.Aviso("Usuário Inativo!");
          return;
        }

        if (usuario_alocados.model == null) {
          await Task.Run(() => {
            using (ContextoDados db = new ContextoDados()) {
              usuario_alocados.model = new usuario_alocados {
                usuario_pc = Environment.UserName,
                hostname = Dns.GetHostName(),
                usuario_id = usu.id,
              };

              db.usuario_alocados.Add(usuario_alocados.model);
              db.SaveChanges();
            }
          });

          // No FrmLogin o mesmo contexto do EF ligava o usuário à alocação; aqui são contextos separados.
          usuario_alocados.model.usuario = usu;
        }

        MensagemOcupado = "Carregando ambiente...";
        var ambiente = await Task.Run(Sessao.CarregarAmbiente);
        Log.Info($"Login: ambiente carregado = {ambiente}");
        if (ambiente)
          LoginConcluido?.Invoke();
      } catch (Exception ex) {
        Log.Erro("Erro ao logar no sistema", ex);
        LmException.ShowException(ex, "Erro ao Logar no Sistema");
      } finally {
        Ocupado = false;
      }
    }
  }
}
