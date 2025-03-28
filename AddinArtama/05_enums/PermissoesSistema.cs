using System.ComponentModel;

namespace AddinArtama {
  public enum TipoPermissao {
    Menu = 0,
    Formulario = 1,
    Configuracao = 2,
    Indefinido = 99
  }

  public enum PermissoesSistema {
    [Description("Solução Completa")]
    Solucao = 1,

    [Description("Aplicação de Processos"), PermissaoSistema(TipoPermissao.Configuracao)]
    AplicacaoProcesso = 101,

    [Description("Propriedades Personalizadas"), PermissaoSistema(TipoPermissao.Formulario)]
    PropsPersonalizadas = 102,

    [Description("Desenhos"), PermissaoSistema(TipoPermissao.Menu)]
    Desenho = 103,

    [Description("Criar/Alterar Desenhos"), PermissaoSistema(TipoPermissao.Formulario)]
    CriarAlterarDesenhos = 10301,

    [Description("Atualizar Templates dos Desenhos"), PermissaoSistema(TipoPermissao.Formulario)]
    AtualizarTemplatesDesenhos = 10302,

    [Description("Exportar Desenhos"), PermissaoSistema(TipoPermissao.Formulario)]
    Exportar = 104,

    [Description("Exportar PDF"), PermissaoSistema(TipoPermissao.Formulario)]
    ExportarPDF = 10401,

    [Description("Exportar DXF"), PermissaoSistema(TipoPermissao.Formulario)]
    ExportarDXF = 10402,

    [Description("Cadastros"), PermissaoSistema(TipoPermissao.Menu)]
    Cadastros = 105,

    [Description("Cadastro de Usuário"), PermissaoSistema(TipoPermissao.Formulario)]
    UsuarioCad = 10501,

    [Description("Cadastro de Perfil do Usuário"), PermissaoSistema(TipoPermissao.Formulario)]
    PerfilUsuarioCad = 10502,

    [Description("Redefinir Senha"), PermissaoSistema(TipoPermissao.Formulario)]
    SenhaRedefinir = 10503,

    [Description("Relatórios"), PermissaoSistema(TipoPermissao.Menu)]
    Relatorios = 106,

    [Description("Processo de Fabricação"), PermissaoSistema(TipoPermissao.Formulario)]
    ProcessoFabricacao = 10601,

    [Description("Pack List"), PermissaoSistema(TipoPermissao.Formulario)]
    PackList = 10602,

    [Description("Plano de Pintura"), PermissaoSistema(TipoPermissao.Formulario)]
    PlanoPintura = 10603,
    
    [Description("Manutenção Packlist e Plano de Pintura"), PermissaoSistema(TipoPermissao.Formulario)]
    ManutencaoPinturaPackList = 10604,

    [Description("Report Works"), PermissaoSistema(TipoPermissao.Formulario)]
    ReportWorks = 10605,

    [Description("Configurações"), PermissaoSistema(TipoPermissao.Formulario)]
    Configuracao = 107,


  }
}
