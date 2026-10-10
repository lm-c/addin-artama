using AxionConnect.Wpf.Models;
using AxionConnect.Wpf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LmCorbieUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AxionConnect.Wpf.ViewModels {
  /// <summary>
  /// Cadastro de processo (operação + máquina do ERP): mesmas regras do ViewProcessoCad do Addin.
  /// A consulta [F7] do Addin aqui é a grade ao lado do formulário.
  /// </summary>
  internal partial class ProcessoCadViewModel : ObservableObject {
    processos model = new processos();

    List<Api.Operacao> _operacoesErp = new List<Api.Operacao>();
    List<Api.Maquina> _maquinasErp = new List<Api.Maquina>();
    Task _tarefaCombos;

    public ObservableCollection<LinhaProcesso> Processos { get; } = new ObservableCollection<LinhaProcesso>();
    public ICollectionView ProcessosView { get; }

    /// <summary>Pedido para a View focar a operação.</summary>
    public event Action FocarOperacao;

    /// <summary>Um processo foi salvo ou excluído: quem usa a lista de processos (Engenharia) recarrega.</summary>
    public event Func<Task> ProcessosAlterados;

    public ProcessoCadViewModel() {
      ProcessosView = CollectionViewSource.GetDefaultView(Processos);
      ProcessosView.Filter = FiltrarProcesso;
    }

    #region Propriedades

    [ObservableProperty]
    private string _codigo = string.Empty;

    [ObservableProperty]
    private bool _codigoBloqueado;

    [ObservableProperty]
    private List<OpcaoCombo> _operacoes = new List<OpcaoCombo>();

    [ObservableProperty]
    private List<OpcaoCombo> _maquinas = new List<OpcaoCombo>();

    [ObservableProperty]
    private int? _operacaoSelecionada;

    [ObservableProperty]
    private int? _maquinaSelecionada;

    /// <summary>Operação interna exige máquina; externa não usa (campo desabilitado).</summary>
    [ObservableProperty]
    private bool _maquinaObrigatoria = true;

    [ObservableProperty]
    private bool _ativo = true;

    [ObservableProperty]
    private bool _podeExcluir;

    [ObservableProperty]
    private bool _carregando;

    [ObservableProperty]
    private string _filtro = string.Empty;

    [ObservableProperty]
    private LinhaProcesso _processoSelecionado;

    #endregion

    #region Carregar operações / máquinas

    /// <summary>Primeira abertura da tela: carrega as operações e máquinas do ERP e os processos cadastrados.</summary>
    public Task InicializarAsync() => _tarefaCombos ??= CarregarCombosAsync();

    private async Task CarregarCombosAsync() {
      Carregando = true;
      try {
        _maquinasErp = await Api.GetMaquinasAsync();
        _operacoesErp = await Api.GetOpsAsync();

        Maquinas = _maquinasErp
          .Select(x => new OpcaoCombo(x.codMaquina, $"{x.codMaquina} - {x.descricao} [{x.mascara}]"))
          .ToList();
        Operacoes = _operacoesErp
          .Select(x => new OpcaoCombo(x.codOperacao, $"{x.codOperacao} - {x.descricao} [{x.abreviatura}]"))
          .ToList();

        CarregarGrade();
      } catch (Exception ex) {
        Log.Erro("Erro ao carregar operações e máquinas", ex);
        Notificacao.Aviso(ex.Message);
      } finally {
        Carregando = false;
      }

      FocarOperacao?.Invoke();
    }

    /// <summary>Processos cadastrados, com a descrição da operação e da máquina no ERP.</summary>
    private void CarregarGrade() {
      Processos.Clear();

      foreach (var processo in processos.SelecionarTodos().OrderBy(x => x.codigo_operacao).ThenBy(x => x.codigo_maquina)) {
        var operacao = _operacoesErp.FirstOrDefault(x => x.codOperacao == processo.codigo_operacao);
        var maquina = _maquinasErp.FirstOrDefault(x => x.codMaquina == processo.codigo_maquina);

        Processos.Add(new LinhaProcesso(
          processo.id,
          operacao == null ? $"{processo.codigo_operacao} - (não encontrada no ERP)" : $"{operacao.codOperacao} - {operacao.descricao}",
          processo.codigo_maquina == null ? string.Empty
            : maquina == null ? $"{processo.codigo_maquina} - (não encontrada no ERP)" : $"{maquina.codMaquina} - {maquina.descricao}",
          processo.tipoOperacao == TipoOperacao.Interna ? "Interna" : "Externa",
          processo.ativo ? "Ativo" : "Inativo"));
      }
    }

    partial void OnFiltroChanged(string value) => ProcessosView.Refresh();

    private bool FiltrarProcesso(object item) {
      if (string.IsNullOrWhiteSpace(Filtro) || item is not LinhaProcesso linha)
        return true;

      var texto = Filtro.Trim();
      return linha.Codigo.ToString() == texto
        || linha.Operacao.Contains(texto, StringComparison.CurrentCultureIgnoreCase)
        || linha.Maquina.Contains(texto, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>Operação externa não usa máquina; interna exige.</summary>
    partial void OnOperacaoSelecionadaChanged(int? value) {
      var externa = _operacoesErp.FirstOrDefault(x => x.codOperacao == value)?.tipo == "Externo";

      if (externa)
        MaquinaSelecionada = null;

      MaquinaObrigatoria = !externa;
    }

    #endregion

    #region Código

    partial void OnProcessoSelecionadoChanged(LinhaProcesso value) {
      if (value == null)
        return;

      Codigo = value.Codigo.ToString();
      CarregarProcesso();
    }

    /// <summary>Carrega o processo do código informado (Enter ou ao sair do campo).</summary>
    public void CarregarProcesso() {
      if (string.IsNullOrEmpty(Codigo))
        return;

      try {
        int id = int.Parse(Codigo);

        if (model.id == id) return;
        model = processos.SelecionarProcesso(id);

        if (model != null) {
          Codigo = model.id.ToString();
          Ativo = model.ativo;
          OperacaoSelecionada = model.codigo_operacao;
          MaquinaSelecionada = model.codigo_maquina;

          PodeExcluir = true;
          CodigoBloqueado = true;
        } else {
          model = new processos();
          Notificacao.Aviso("Processo não encontrado.");
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Carregar Processo");
      }
    }

    #endregion

    #region Salvar / Limpar / Excluir

    [RelayCommand]
    private void Limpar() {
      CodigoBloqueado = false;
      Codigo = string.Empty;
      MaquinaSelecionada = null;
      OperacaoSelecionada = null;
      PodeExcluir = false;

      Ativo = true;
      model = new processos();
      ProcessoSelecionado = null;

      FocarOperacao?.Invoke();
    }

    public async Task SalvarAsync() {
      var operacao = _operacoesErp.FirstOrDefault(x => x.codOperacao == OperacaoSelecionada);
      if (operacao == null) {
        Notificacao.Aviso("Selecione uma operação da lista!");
        FocarOperacao?.Invoke();
        return;
      }

      if (MaquinaObrigatoria && !_maquinasErp.Any(x => x.codMaquina == MaquinaSelecionada)) {
        Notificacao.Aviso("Selecione uma máquina da lista!");
        return;
      }

      try {
        var novo = model.id == 0;

        model.ativo = Ativo;
        model.codigo_maquina = MaquinaSelecionada;
        model.codigo_operacao = operacao.codOperacao;
        model.tipoOperacao = operacao.tipo == "Interno" ? TipoOperacao.Interna : TipoOperacao.Externa;

        if (processos.Salvar(model)) {
          Notificacao.Sucesso(novo ? "Processo Cadastrado com Sucesso!" : "Processo Alterado com Sucesso!");
          Limpar();
        } else {
          Notificacao.Aviso("Já existe um registro com este processo vinculado a uma máquina");
        }

        await AtualizarListasAsync();
      } catch (Exception ex) {
        Log.Erro("Erro ao salvar processo", ex);
        LmException.ShowException(ex, "Erro ao Cadastrar Operação");
      }
    }

    [RelayCommand]
    private async Task Excluir() {
      try {
        if (!Notificacao.Confirmar("Deseja realmente excluir este registro?", "Excluir Registro"))
          return;

        processos.Excluir(model.id);
        Notificacao.Info("Excluido com Sucesso!");
        Limpar();
        await AtualizarListasAsync();
      } catch (Exception ex) {
        Log.Erro("Erro ao excluir processo", ex);
        LmException.ShowException(ex, "Erro ao Excluir Processo");
      }
    }

    /// <summary>Como o Processo.Carregar() do Addin após salvar/excluir: atualiza a grade e a lista usada na Engenharia.</summary>
    private async Task AtualizarListasAsync() {
      CarregarGrade();

      if (ProcessosAlterados != null)
        await ProcessosAlterados.Invoke();
    }

    #endregion
  }
}
