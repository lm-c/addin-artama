using AxionConnect.Wpf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Reflection;

namespace AxionConnect.Wpf.ViewModels {
  /// <summary>Janela principal (antigo FrmPrincipal): menu lateral, Engenharia e rodapé.</summary>
  internal partial class MainViewModel : ObservableObject {
    /// <summary>Seção: data/hora da abertura (como o tslSecao).</summary>
    public string Secao { get; } = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    public string Versao { get; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3);
    public string Usuario { get; } = usuario_alocados.model?.usuario?.login;
    /// <summary>Ambiente de homologação (InfoAssembly.EhAmbienteOficial = false no CORE): aviso no título e no rodapé.</summary>
    public bool AmbienteHomologacao { get; } = !InfoAssembly.EhAmbienteOficial;
    public string Banco { get; } = ConexaoMySql.Database;

    public string Titulo => $"Axion Connect — Versão {Versao}" + (AmbienteHomologacao ? "  [AMBIENTE DE HOMOLOGAÇÃO]" : "");

    public EngenhariaViewModel Engenharia { get; } = new EngenhariaViewModel();
    public ProcessoCadViewModel Processos { get; } = new ProcessoCadViewModel();

    /// <summary>Mesma permissão do Addin (Integração ERP > Configuração de Processo).</summary>
    public bool PermiteProcessos { get; } = TemPermissao(PermissoesSistema.processoConfig);

    public MainViewModel() {
      // Processo salvo/excluído: a Engenharia recarrega as operações e máquinas (Processo.Carregar).
      Processos.ProcessosAlterados += Engenharia.RecarregarCadastrosAsync;
    }

    private static bool TemPermissao(PermissoesSistema permissao) {
      try {
        var usuario = usuario_alocados.model?.usuario;
        return usuario != null && usuarios.SelecionarPermissoes(usuario.id).Contains(((int)permissao).ToString());
      } catch (Exception ex) {
        Log.Erro("Erro ao verificar permissões do usuário", ex);
        return false;
      }
    }

    /// <summary>Menu só com ícones: preferência do usuário gravada no banco (configuracao_sistema).</summary>
    [ObservableProperty]
    private bool _menuCompacto = configuracao_sistema.model?.menu_apenas_icone == true;

    partial void OnMenuCompactoChanged(bool value) {
      configuracao_sistema.model.menu_apenas_icone = value;
      configuracao_sistema.Salvar();
    }

    [RelayCommand]
    private void AlternarMenu() => MenuCompacto = !MenuCompacto;

    /// <summary>Tela em exibição (destaca o item do menu). As telas ficam abertas; só a visibilidade muda.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EngenhariaVisivel), nameof(ProcessosVisivel))]
    private string _nomeTelaAtual = "Engenharia";

    public bool EngenhariaVisivel => NomeTelaAtual == "Engenharia";
    public bool ProcessosVisivel => NomeTelaAtual == "Processos";

    [RelayCommand]
    private void AbrirTela(string nome) => NomeTelaAtual = nome;
  }
}
