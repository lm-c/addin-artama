using LmCorbieUI.Metodos;

namespace AddinArtama {
  /// <summary>
  /// Conta de envio dos e-mails do sistema (código de redefinição de senha).
  /// Lida do .env ao lado da DLL, como as credenciais do banco (ver .env.example).
  /// </summary>
  internal static class EmailSistema {
    /// <summary>O .env tem a conta de envio completa.</summary>
    public static bool Configurado { get; private set; }

    public static void Configurar() {
      var servidor = ConexaoMySql.LerEnv("SMTP_SERVER");
      var usuario = ConexaoMySql.LerEnv("SMTP_USER");
      var senha = ConexaoMySql.LerEnv("SMTP_PASS");

      if (!int.TryParse(ConexaoMySql.LerEnv("SMTP_PORT"), out var porta))
        porta = 587;

      Configurado = !string.IsNullOrEmpty(servidor) && !string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha);
      if (Configurado)
        Email.SetarPadrao(servidor, porta, usuario, senha);
    }

    /// <summary>
    /// Corpo (HTML) do e-mail com o código de redefinição de senha. Só estilos inline e tabelas: é o que os
    /// leitores de e-mail (Outlook, Gmail) respeitam. E-mail não roda script, então não existe "botão copiar":
    /// o código fica num bloco que é selecionado inteiro com um clique (user-select: all), pronto para Ctrl+C.
    /// </summary>
    public static string CorpoCodigoSenha(string nome, string login, string codigo, int minutosValidade, string sistema) {
      nome = System.Net.WebUtility.HtmlEncode(nome);
      login = System.Net.WebUtility.HtmlEncode(login);
      sistema = System.Net.WebUtility.HtmlEncode(sistema);

      return $@"<!DOCTYPE html>
<html lang='pt-BR'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Código de redefinição de senha</title>
</head>
<body style='margin:0; padding:0; background-color:#eef1f4; font-family:Segoe UI, Arial, Helvetica, sans-serif; color:#2b3440;'>
  <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:#eef1f4;'>
    <tr>
      <td align='center' style='padding:28px 12px;'>
        <table role='presentation' width='560' cellpadding='0' cellspacing='0' border='0' style='width:560px; max-width:100%; background-color:#ffffff; border:1px solid #dde3e9; border-radius:8px;'>
          <tr>
            <td style='background-color:#17475c; border-radius:8px 8px 0 0; padding:22px 32px;'>
              <div style='font-size:12px; letter-spacing:2px; text-transform:uppercase; color:#b9d3de;'>{sistema}</div>
              <div style='font-size:22px; font-weight:600; color:#ffffff; margin-top:4px;'>Redefinição de senha</div>
            </td>
          </tr>
          <tr>
            <td style='padding:28px 32px 8px 32px; font-size:15px; line-height:1.6;'>
              <p style='margin:0 0 12px 0;'>Olá, <strong>{nome}</strong>.</p>
              <p style='margin:0 0 12px 0;'>Recebemos um pedido para redefinir a senha do usuário <strong style='color:#17475c;'>{login}</strong>.</p>
              <p style='margin:0;'>Use o código abaixo na tela de redefinição:</p>
            </td>
          </tr>
          <tr>
            <td align='center' style='padding:16px 32px 6px 32px;'>
              <table role='presentation' cellpadding='0' cellspacing='0' border='0' style='background-color:#f4f8fa; border:2px dashed #17475c; border-radius:8px;'>
                <tr>
                  <td align='center' style='padding:18px 36px;'>
                    <div style='font-size:11px; letter-spacing:2px; text-transform:uppercase; color:#6b7a88; margin-bottom:8px;'>Código de verificação</div>
                    <div title='Clique para selecionar e copie com Ctrl+C' style='font-family:Consolas, Courier New, monospace; font-size:36px; font-weight:bold; letter-spacing:10px; color:#17475c; cursor:pointer; -webkit-user-select:all; -moz-user-select:all; -ms-user-select:all; user-select:all;'>{codigo}</div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td align='center' style='padding:0 32px 20px 32px;'>
              <span style='display:inline-block; background-color:#e6eef2; color:#17475c; font-size:12px; padding:5px 12px; border-radius:12px;'>&#128203; Para copiar: clique no código e tecle Ctrl+C</span>
            </td>
          </tr>
          <tr>
            <td style='padding:0 32px 24px 32px; font-size:14px; line-height:1.6;'>
              <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:#fff8e6; border-left:4px solid #e0a800; border-radius:4px;'>
                <tr>
                  <td style='padding:10px 14px; color:#6b5200;'>&#9201; Este código expira em <strong>{minutosValidade} minutos</strong>.</td>
                </tr>
              </table>
              <p style='margin:16px 0 0 0; color:#6b7a88; font-size:13px;'>Se você não pediu a redefinição, ignore este e-mail: sua senha continua a mesma.</p>
            </td>
          </tr>
          <tr>
            <td style='border-top:1px solid #e6eaee; padding:14px 32px; font-size:12px; color:#96a2ae; text-align:center;'>
              Mensagem automática do {sistema}. Não responda este e-mail.
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
    }
  }
}
