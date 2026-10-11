using LmCorbieUI;
using LmCorbieUI.Metodos;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>
  /// Redefinição de senha em 3 passos: envia um código por e-mail, verifica o código e grava a nova senha.
  /// </summary>
  public partial class WindowResetSenha : Wpf.LmWindow {
    private static readonly TimeSpan ValidadeCodigo = TimeSpan.FromMinutes(15);
    private const int MaxTentativas = 5;

    // O código só existe durante a vida desta janela
    private int _usuarioId;
    private string _codigo;
    private DateTime _codigoExpiraEm;
    private int _tentativas;
    private bool _codigoVerificado;

    public string _usuarioLogin;
    public string _novaSenha;

    public WindowResetSenha() {
      InitializeComponent();

      Loaded += (s, e) => txtLoginEmail.Focus();
    }

    private async void BtnEnviarCodigo_Click(object sender, RoutedEventArgs e) {
      string loginEmail = txtLoginEmail.Text.Trim();

      if (string.IsNullOrEmpty(loginEmail)) {
        Wpf.Toast.Warning("Informe o Login ou Email!");
        return;
      }

      if (!EmailSistema.Configurado) {
        Wpf.MsgBox.Show("O envio de e-mail não está configurado neste computador (arquivo .env).\nContate o administrador.",
          "Redefinir Senha", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      try {
        btnEnviarCodigo.IsEnabled = false;

        usuarios usu;
        using (ContextoDados db = new ContextoDados()) {
          usu = db.usuarios.FirstOrDefault(x => x.ativo && (x.login == loginEmail || x.email == loginEmail));
        }

        _usuarioId = 0;
        _codigo = null;
        _codigoVerificado = false;
        _tentativas = 0;

        if (usu == null) {
          Wpf.MsgBox.Show($"Não encontramos um usuário ativo com o login ou e-mail \"{loginEmail}\".\n\n" +
            "Confira o que foi digitado. Se o problema continuar, procure um administrador do sistema.",
            "Redefinir Senha", MessageBoxButton.OK, MessageBoxImage.Warning);
          txtLoginEmail.Focus();
          return;
        }

        // Usuário sem e-mail nunca receberia o código: avisa em vez de deixar esperando.
        if (string.IsNullOrWhiteSpace(usu.email)) {
          Wpf.MsgBox.Show($"O usuário \"{usu.login}\" não possui e-mail cadastrado, por isso não é possível enviar o código.\n\n" +
            "Peça a um administrador do sistema para cadastrar o seu e-mail no Cadastro de Usuário e tente novamente.",
            "Redefinir Senha", MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
        }

        var codigo = GerarCodigo();

        var erro = await Task.Run(() => Email.SendMail(
          titulo: "Código de redefinição de senha",
          corpoEmail: EmailSistema.CorpoCodigoSenha(usu.nome, usu.login, codigo, (int)ValidadeCodigo.TotalMinutes, "Axion Addin Artama"),
          emailPara: usu.email));

        if (!string.IsNullOrEmpty(erro)) {
          Wpf.MsgBox.Show($"Erro ao enviar e-mail com o código.\n\n{erro}", "Redefinir Senha", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }

        _usuarioId = usu.id;
        _usuarioLogin = usu.login;
        _codigo = codigo;
        _codigoExpiraEm = DateTime.Now.Add(ValidadeCodigo);

        Wpf.Toast.Success($"Código enviado para o e-mail cadastrado:\n{usu.email}");

        MostrarPasso(2);
        txtCodigo.Focus();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao processar solicitação");
      } finally {
        btnEnviarCodigo.IsEnabled = true;
      }
    }

    private void BtnVerificarCodigo_Click(object sender, RoutedEventArgs e) {
      var codigo = txtCodigo.Text.Trim();

      if (string.IsNullOrWhiteSpace(codigo)) {
        Wpf.Toast.Warning("Informe o código.");
        return;
      }

      if (_codigo == null || DateTime.Now > _codigoExpiraEm) {
        Wpf.Toast.Error("Código inválido ou expirado. Solicite um novo código.");
        return;
      }

      if (_tentativas >= MaxTentativas) {
        _codigo = null;
        Wpf.Toast.Error("Número máximo de tentativas atingido. Solicite um novo código.");
        return;
      }

      _tentativas++;

      if (codigo != _codigo) {
        Wpf.Toast.Error("Código inválido.");
        return;
      }

      _codigoVerificado = true;

      Wpf.Toast.Success("Código verificado com sucesso!");
      MostrarPasso(3);
      txtNovaSenha.Focus();
    }

    private void BtnSalvarSenha_Click(object sender, RoutedEventArgs e) {
      if (string.IsNullOrEmpty(txtNovaSenha.Senha)) {
        Wpf.Toast.Warning("Informe a nova senha.");
        return;
      }

      if (txtNovaSenha.Senha != txtConfirmarSenha.Senha) {
        Wpf.Toast.Warning("As senhas não conferem.");
        return;
      }

      if (!_codigoVerificado || _usuarioId == 0 || DateTime.Now > _codigoExpiraEm) {
        Wpf.Toast.Warning("Informe e verifique o código antes de salvar a senha.");
        return;
      }

      try {
        using (ContextoDados db = new ContextoDados()) {
          var usu = db.usuarios.FirstOrDefault(x => x.id == _usuarioId);
          if (usu == null) {
            Wpf.Toast.Error("Usuário não encontrado.");
            return;
          }

          usu.senha = txtNovaSenha.Senha.CriptografarAES();
          db.SaveChanges();
        }

        _codigo = null;
        Wpf.Toast.Success("Senha atualizada com sucesso!");

        _novaSenha = txtNovaSenha.Senha;
        Close();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao salvar nova senha");
        _novaSenha = string.Empty;
      }
    }

    /// <summary>Mostra só o passo informado; o botão do passo vira o padrão (Enter).</summary>
    private void MostrarPasso(int passo) {
      pnlStep1.Visibility = passo == 1 ? Visibility.Visible : Visibility.Collapsed;
      pnlStep2.Visibility = passo == 2 ? Visibility.Visible : Visibility.Collapsed;
      pnlStep3.Visibility = passo == 3 ? Visibility.Visible : Visibility.Collapsed;

      btnEnviarCodigo.IsDefault = passo == 1;
      btnVerificarCodigo.IsDefault = passo == 2;
      btnSalvarSenha.IsDefault = passo == 3;
    }

    private static string GerarCodigo() {
      var bytes = new byte[4];
      using (var rng = RandomNumberGenerator.Create()) {
        rng.GetBytes(bytes);
      }
      return (BitConverter.ToUInt32(bytes, 0) % 1000000).ToString("000000");
    }
  }
}
