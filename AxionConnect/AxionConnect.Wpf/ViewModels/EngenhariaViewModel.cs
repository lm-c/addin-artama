using AxionConnect.Wpf.Models;
using AxionConnect.Wpf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LmCorbieUI;
using LmCorbieUI.Metodos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static AxionConnect.Api;
using Lm = LmCorbieUI.Wpf;
using TreeNode = System.Windows.Forms.TreeNode;
using TreeView = System.Windows.Forms.TreeView;

namespace AxionConnect.Wpf.ViewModels {
  /// <summary>
  /// Engenharia de Produto (antigo FrmEngenharia). As regras são as mesmas do form; a leitura do ERP continua
  /// no CORE (ProdutoErp.GetComponentsFromERPAsync, que monta um TreeView WinForms) e a árvore é copiada para o WPF.
  /// O visualizador eDrawings fica na View, que acompanha <see cref="ProdutoSelecionado"/>.
  /// </summary>
  internal partial class EngenhariaViewModel : ObservableObject {
    const string TempoPadrao = "00:01";
    const string OperadoresPadrao = "1";

    readonly TreeView _arvoreCompleta = new TreeView();
    SortableBindingList<ProdutoErp> _produtos = new SortableBindingList<ProdutoErp>();

    /// <summary>Cópia do nó do componente selecionado (antigo trvProduto.Nodes[0]): base para salvar a engenharia.</summary>
    TreeNode _noSelecionado;

    Task _tarefaCadastros;
    bool _preenchendoCampos;

    public ObservableCollection<LinhaProduto> Produtos { get; } = new ObservableCollection<LinhaProduto>();
    public ICollectionView ProdutosView { get; }
    public ObservableCollection<CartaoOperacao> Cartoes { get; } = new ObservableCollection<CartaoOperacao>();
    public ObservableCollection<EtapaConsumo> Etapas { get; } = new ObservableCollection<EtapaConsumo>();

    /// <summary>Pedido para a View fechar o documento aberto no eDrawings.</summary>
    public event Action FecharDesenho;

    /// <summary>Pedido para a View focar o código da engenharia.</summary>
    public event Action FocarCodigo;

    public EngenhariaViewModel() {
      ProdutosView = CollectionViewSource.GetDefaultView(Produtos);
      ProdutosView.Filter = FiltrarProduto;

      // O CORE mostra o progresso pelo Loader: aqui, no overlay da tela (com Cancelar).
      Loader.Exibir = ExecutarComProgressoAsync;
    }

    #region Propriedades

    [ObservableProperty]
    private string _codigoEngenharia = string.Empty;

    [ObservableProperty]
    private bool _codigoBloqueado;

    [ObservableProperty]
    private bool _botoesHabilitados = true;

    [ObservableProperty]
    private string _filtro = string.Empty;

    [ObservableProperty]
    private LinhaProduto _produtoSelecionado;

    [ObservableProperty]
    private IEnumerable<NoProduto> _arvoreSelecionada;

    [ObservableProperty]
    private string _alerta = string.Empty;

    [ObservableProperty]
    private bool _alertaCritico;

    [ObservableProperty]
    private string _dimensao = string.Empty;

    [ObservableProperty]
    private string _material = string.Empty;

    [ObservableProperty]
    private string _smLargura = string.Empty;

    [ObservableProperty]
    private string _smComprimento = string.Empty;

    [ObservableProperty]
    private string _peso = string.Empty;

    [ObservableProperty]
    private List<OpcaoCombo> _operacoes = new List<OpcaoCombo>();

    [ObservableProperty]
    private List<OpcaoCombo> _maquinas = new List<OpcaoCombo>();

    [ObservableProperty]
    private int? _operacaoSelecionada;

    [ObservableProperty]
    private int? _maquinaSelecionada;

    [ObservableProperty]
    private bool _maquinaHabilitada = true;

    [ObservableProperty]
    private string _tempoOperacao = TempoPadrao;

    [ObservableProperty]
    private string _numeroOperadores = OperadoresPadrao;

    [ObservableProperty]
    private bool _fantasma;

    [ObservableProperty]
    private bool _possuiVariasEtapas;

    [ObservableProperty]
    private bool _ocupado;

    [ObservableProperty]
    private string _mensagemOcupado = string.Empty;

    [ObservableProperty]
    private bool _carregandoCadastros;

    #endregion

    #region Progresso (Loader do CORE)

    private async Task ExecutarComProgressoAsync(Func<IProgress<string>, Task> operacao) {
      Ocupado = true;
      MensagemOcupado = "Aguarde...";
      try {
        await operacao(new Progress<string>(m => MensagemOcupado = m));
      } finally {
        Ocupado = false;
      }
    }

    [RelayCommand]
    private void Cancelar() => Loader.Cancelar();

    #endregion

    #region Carregar operações (FrmEngenharia_Load)

    public void Inicializar() {
      _tarefaCadastros ??= CarregarCadastrosAsync();
    }

    /// <summary>O cadastro de processos mudou: recarrega as listas mantendo a operação/máquina que estavam selecionadas.</summary>
    public async Task RecarregarCadastrosAsync() {
      if (_tarefaCadastros == null)
        return;

      await _tarefaCadastros;

      var operacao = OperacaoSelecionada;
      var maquina = MaquinaSelecionada;

      _tarefaCadastros = CarregarCadastrosAsync();
      await _tarefaCadastros;

      OperacaoSelecionada = Operacoes.Any(x => x.Codigo == operacao) ? operacao : null;
      if (Maquinas.Any(x => x.Codigo == maquina))
        MaquinaSelecionada = maquina;
    }

    private async Task CarregarCadastrosAsync() {
      CarregandoCadastros = true;
      try {
        await Processo.Carregar();
        Operacoes = Processo.ListaProcessos
          .GroupBy(x => x.codOperacao)
          .Select(g => new OpcaoCombo(g.Key, g.Key + " - " + g.First().descrOperacao))
          .OrderBy(x => x.Codigo)
          .ToList();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar tela");
      } finally {
        CarregandoCadastros = false;
      }
    }

    #endregion

    #region Carregar / Nova engenharia

    /// <summary>Saiu do código (ou Enter) com um valor diferente do que estava ao entrar: carrega (TxtCodEngenharia_Leave).</summary>
    public async Task CarregarSeAlteradoAsync(string codigoAoEntrar) {
      if (CodigoBloqueado || codigoAoEntrar == CodigoEngenharia || !long.TryParse(CodigoEngenharia, out long codEngenharia))
        return;

      await CarregarEngenhariaAsync(codEngenharia);
    }

    private async Task CarregarEngenhariaAsync(long codEngenharia) {
      try {
        CodigoBloqueado = true;
        BotoesHabilitados = false;
        LimparTela();

        // As operações do ERP precisam estar carregadas para reconhecer as operações da engenharia.
        Inicializar();
        await _tarefaCadastros;

        _produtos = await ProdutoErp.GetComponentsFromERPAsync(_arvoreCompleta, codEngenharia);

        CarregarGrid();
        if (_produtos.Count == 0) {
          CodigoBloqueado = false;
          CodigoEngenharia = string.Empty;
          FocarCodigo?.Invoke();
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao processar o código de engenharia.");
      } finally {
        BotoesHabilitados = true;
      }
    }

    private void CarregarGrid() {
      try {
        foreach (var produtoErp in _produtos) {
          if (produtoErp.Operacoes != null && produtoErp.Operacoes.Count == 0 && produtoErp.TipoComponente != TipoComponente.ItemBiblioteca && !produtoErp.Fantasma) {
            ProdutoErp.AdicionarPendencia(produtoErp, PendenciasEngenharia.OperacaoNaoPossui);
          }
        }
      } catch (Exception ex) {
        Notificacao.Erro("Erro ao verificar pendencias de processos. \r\n" + ex.Message);
      }

      foreach (var produto in _produtos)
        Produtos.Add(new LinhaProduto(produto));

      if (Produtos.Count > 0)
        ProdutoSelecionado = Produtos[0];
    }

    [RelayCommand]
    private void NovaEngenharia() {
      if (Produtos.Count > 0 && !Notificacao.Confirmar("Já existem componentes carregados.\r\n" +
          "Deseja Limpar os dados atuais para iniciar nova engenharia?", "Axion LM Projetos"))
        return;

      FecharDesenho?.Invoke();
      LimparTela();
      CodigoBloqueado = false;
      CodigoEngenharia = string.Empty;
      FocarCodigo?.Invoke();
    }

    private void LimparTela() {
      ProdutoSelecionado = null;
      Produtos.Clear();
      _arvoreCompleta.Nodes.Clear();
      _produtos = new SortableBindingList<ProdutoErp>();
      Filtro = string.Empty;
    }

    private bool FiltrarProduto(object item) {
      if (string.IsNullOrWhiteSpace(Filtro) || item is not LinhaProduto p)
        return true;

      var f = Filtro.Trim();
      return new[] { p.Nivel, p.Name, p.CodProduto, p.Denominacao, p.Referencia }
        .Any(x => x?.Contains(f, StringComparison.OrdinalIgnoreCase) == true);
    }

    partial void OnFiltroChanged(string value) => ProdutosView.Refresh();

    #endregion

    #region Componente selecionado (AtualizarComponente)

    partial void OnProdutoSelecionadoChanged(LinhaProduto value) {
      _preenchendoCampos = true;
      try {
        LimparCamposOperacao();
        Cartoes.Clear();
        Etapas.Clear();
        PossuiVariasEtapas = false;

        if (value == null) {
          _noSelecionado = null;
          ArvoreSelecionada = null;
          Fantasma = false;
          SmLargura = SmComprimento = Dimensao = Material = Alerta = Peso = string.Empty;
          return;
        }

        var produtoErp = value.Produto;
        Fantasma = produtoErp.Fantasma;

        var node = GetNodeByLevelPath(_arvoreCompleta, produtoErp.Nivel);
        _noSelecionado = (TreeNode)node?.Clone();
        ArvoreSelecionada = _noSelecionado != null ? new[] { NoProduto.Criar(_noSelecionado) } : null;

        AtualizarInfoAlerta(produtoErp);
        AtualizarInformacoes(produtoErp);
        GetProcess(produtoErp);
        CarregarSequenciaOp(produtoErp);
      } catch (Exception ex) {
        Notificacao.Erro($"Erro ao Atualizar Dados\n\n{ex.Message}");
      } finally {
        _preenchendoCampos = false;
      }
    }

    private void AtualizarInfoAlerta(ProdutoErp produtoErp) {
      var criticas = produtoErp.Pendencias.Where(y => y.EhPendenciaCritica()).ToList();
      var naoCriticas = produtoErp.Pendencias.Where(y => !y.EhPendenciaCritica()).ToList();

      if (criticas.Count > 0) {
        AlertaCritico = true;
        Alerta = string.Join("\r\n", criticas.Select(y => $"- {y.ObterDescricaoEnum()}"));
      } else if (naoCriticas.Count > 0) {
        AlertaCritico = false;
        Alerta = string.Join("\r\n", naoCriticas.Select(y => $"- {y.ObterDescricaoEnum()}"));
      } else {
        Alerta = string.Empty;
      }
    }

    private void AtualizarInformacoes(ProdutoErp produtoErp) {
      SmLargura = produtoErp.SobremetalLarg.ToString("#");
      SmComprimento = produtoErp.SobremetalCompr.ToString("#");
      Peso = produtoErp.PesoBruto + " kg";
      Dimensao = Material = string.Empty;

      if (produtoErp.TipoComponente == TipoComponente.Peca) {
        var espess = produtoErp.Espessura;
        var largur = produtoErp.Largura;
        var compri = produtoErp.Comprimento;
        var tipo = produtoErp.TipoListaMaterial;

        Dimensao = tipo == TipoListaMaterial.Chapa ? $"{espess}x{largur}x{compri}" : $"{compri}";

        var nodeFilho = _noSelecionado != null && _noSelecionado.Nodes.Count > 0 ? _noSelecionado.Nodes[0] : null;
        if (nodeFilho?.Tag is ProdutoErp produtoErpFilho)
          Material = $"{produtoErpFilho.CodProduto} - {produtoErpFilho.Denominacao}";
      }
    }

    private void GetProcess(ProdutoErp produtoErp) {
      try {
        Cartoes.Clear();

        if (produtoErp.Operacoes != null && produtoErp.Operacoes.Count > 0) {
          foreach (var prc in produtoErp.Operacoes)
            Cartoes.Add(new CartaoOperacao(prc));

          LimparCamposOperacao();
        }
      } catch (Exception ex) {
        Notificacao.Erro($"Erro ao retornar Processos\n\n{ex.Message}");
      }
    }

    /// <summary>Produto fantasma: não exige operações (CkbFantasma_CheckedChanged).</summary>
    partial void OnFantasmaChanged(bool value) {
      if (_preenchendoCampos)
        return;

      var linha = ProdutoSelecionado;
      if (linha == null) {
        Notificacao.Info($"Nenhum produto selecionado");
        return;
      }

      try {
        var produtoErp = linha.Produto;

        produtoErp.Fantasma = value;
        Cartoes.Clear();

        if (produtoErp.Fantasma) {
          ProdutoErp.RemoverPendencia(produtoErp, PendenciasEngenharia.OperacaoRevisar);
          ProdutoErp.RemoverPendencia(produtoErp, PendenciasEngenharia.OperacaoNaoPossui);
        } else {
          ProdutoErp.AdicionarPendencia(produtoErp, PendenciasEngenharia.OperacaoNaoPossui);
        }

        linha.Atualizar();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao inserir Operação ao Componente");
      }
    }

    [RelayCommand]
    private void Anterior() {
      var lista = ProdutosView.Cast<LinhaProduto>().ToList();
      if (_produtos.Count == 0) {
        Notificacao.Aviso("Favor Carregar Componentes primeiro.");
        return;
      }

      var atual = lista.IndexOf(ProdutoSelecionado);
      if (atual > 0)
        ProdutoSelecionado = lista[atual - 1];
      else
        Notificacao.Info($"Você está no primeiro componente");
    }

    [RelayCommand]
    private void Proximo() {
      var lista = ProdutosView.Cast<LinhaProduto>().ToList();
      if (_produtos.Count == 0) {
        Notificacao.Aviso("Favor Carregar Componentes primeiro.");
        return;
      }

      var atual = lista.IndexOf(ProdutoSelecionado);
      if (atual + 1 < lista.Count)
        ProdutoSelecionado = lista[atual + 1];
      else
        Notificacao.Info($"Você já chegou no último componente");
    }

    [RelayCommand]
    private void CopiarCodigo() {
      var codigo = ProdutoSelecionado?.CodProduto;
      if (string.IsNullOrEmpty(codigo))
        return;

      Clipboard.SetText(codigo);
      Notificacao.Info("Código copiado para área de transferência!");
    }

    static TreeNode GetNodeByLevelPath(TreeView treeView, string path) {
      string[] levels = path.Split('.');
      TreeNode currentNode = null;

      for (int i = 0; i < levels.Length; i++) {
        int index = int.Parse(levels[i]) - 1; // Assume que "1.3" significa índice 0,2

        if (i == 0) {
          // Nível raiz
          if (treeView.Nodes.Count > index)
            currentNode = treeView.Nodes[index];
          else
            return null;
        } else {
          // Nível filho
          if (currentNode != null && currentNode.Nodes.Count > index)
            currentNode = currentNode.Nodes[index];
          else
            return null;
        }
      }

      return currentNode;
    }

    #endregion

    #region Operações

    // Texto digitado nos ComboBox editáveis (código ou parte da descrição)
    [ObservableProperty]
    private string _operacaoTexto = string.Empty;

    [ObservableProperty]
    private string _maquinaTexto = string.Empty;

    /// <summary>Localiza a operação pelo texto digitado (Enter/Tab). Retorna false se não encontrou.</summary>
    public bool ConfirmarOperacao() {
      if (string.IsNullOrWhiteSpace(OperacaoTexto)) {
        OperacaoSelecionada = null;
        return true;
      }

      var item = Resolver(Operacoes, OperacaoTexto, "Operação");
      if (item == null)
        return false;

      if (OperacaoSelecionada != item.Codigo)
        OperacaoSelecionada = item.Codigo;
      OperacaoTexto = item.Descricao;
      return true;
    }

    /// <summary>Localiza a máquina (entre as da operação selecionada) pelo texto digitado.</summary>
    public bool ConfirmarMaquina() {
      if (!MaquinaHabilitada)
        return true;

      if (string.IsNullOrWhiteSpace(MaquinaTexto)) {
        MaquinaSelecionada = null;
        return true;
      }

      var item = Resolver(Maquinas, MaquinaTexto, "Máquina");
      if (item == null)
        return false;

      MaquinaSelecionada = item.Codigo;
      MaquinaTexto = item.Descricao;
      return true;
    }

    /// <summary>Código exato primeiro ("12" ou "12 - DESCRIÇÃO"); senão, parte da descrição (se for única).</summary>
    private static OpcaoCombo Resolver(IEnumerable<OpcaoCombo> lista, string texto, string nome) {
      texto = texto.Trim();

      var codigo = texto.Split(" - ", 2)[0].Trim();
      if (int.TryParse(codigo, out var cod)) {
        var porCodigo = lista.FirstOrDefault(x => x.Codigo == cod);
        if (porCodigo != null)
          return porCodigo;
      }

      var encontrados = lista.Where(x => x.Descricao.Contains(texto, StringComparison.OrdinalIgnoreCase)).Take(2).ToList();
      if (encontrados.Count == 1)
        return encontrados[0];

      Notificacao.Aviso(encontrados.Count == 0
        ? $"{nome} '{texto}' não encontrada."
        : $"Mais de uma {nome.ToLower()} contém '{texto}'.\nDigite o código ou abra a lista para escolher.");
      return null;
    }

    partial void OnMaquinaSelecionadaChanged(int? value) {
      MaquinaTexto = Maquinas.FirstOrDefault(x => x.Codigo == value)?.Descricao ?? string.Empty;
    }

    /// <summary>
    /// TxtOperacao_SelectedValueChanched: operação interna lista as máquinas do processo e já seleciona a primeira;
    /// operação externa não usa máquina.
    /// </summary>
    partial void OnOperacaoSelecionadaChanged(int? value) {
      OperacaoTexto = Operacoes.FirstOrDefault(x => x.Codigo == value)?.Descricao ?? string.Empty;
      MaquinaSelecionada = null;
      try {
        if (value != null) {
          var op = Processo.ListaOperacoesERP.Where(x => x.codOperacao == value).FirstOrDefault();

          if (op?.tipo == "Interno") {
            MaquinaHabilitada = true;
            Maquinas = Processo.ListaProcessos
              .Where(x => x.codOperacao == value && x.tipoOperacao == TipoOperacao.Interna)
              .Select(x => new OpcaoCombo((int)x.codMaquina, $"{x.codMaquina} - {x.descrMaquina}"))
              .ToList();

            MaquinaSelecionada = Maquinas.FirstOrDefault()?.Codigo;
          } else {
            MaquinaHabilitada = false;
            Maquinas = new List<OpcaoCombo>();
          }
        } else {
          MaquinaHabilitada = true;
          Maquinas = new List<OpcaoCombo>();
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao selecionar Máquinas");
      }
    }

    private void LimparCamposOperacao() {
      OperacaoSelecionada = null;
      OperacaoTexto = MaquinaTexto = string.Empty;
      TempoOperacao = TempoPadrao;
      NumeroOperadores = OperadoresPadrao;
    }

    [RelayCommand]
    private void InserirOperacao() {
      try {
        var linha = ProdutoSelecionado;
        if (linha == null) {
          Notificacao.Info($"Nenhum produto selecionado");
          return;
        }

        // Texto digitado e ainda não confirmado com Enter/Tab
        if (!ConfirmarOperacao() || !ConfirmarMaquina())
          return;

        if (OperacaoSelecionada == null) {
          Notificacao.Aviso("Selecione a Operação.");
          return;
        }

        var idOp = OperacaoSelecionada.Value;
        var op = Processo.ListaOperacoesERP.Where(x => x.codOperacao == idOp).FirstOrDefault();

        if (op?.tipo == "Interno" && MaquinaSelecionada == null) {
          Notificacao.Aviso("Selecione a Máquina.");
          return;
        }

        var idMq = MaquinaSelecionada;

        var proc = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == idOp && x.codMaquina == idMq);
        if (proc == null) {
          Notificacao.Aviso("Processo (operação + máquina) não cadastrado no Axion.");
          return;
        }

        if (Cartoes.Any(x => x.Operacao.processo_id == proc.codAxion)) {
          Notificacao.Aviso("Esta Operação com esta Máquina já foi inserida");
          return;
        }

        var produtoERP = linha.Produto;

        var processo = new produto_erp_operacao {
          processo_id = proc.codAxion,
          name = produtoERP.Name,
          referencia = produtoERP.Referencia,
          sequencia = Cartoes.Count + 1,
          qtd_operador = !string.IsNullOrEmpty(NumeroOperadores) ? Convert.ToInt32(NumeroOperadores) : 1,
          tempo = !string.IsNullOrEmpty(TempoOperacao) ? TempoOperacao.FormatarHora() : "00:01",
        };

        Cartoes.Add(new CartaoOperacao(processo));

        AtualizarProcessos();

        LimparCamposOperacao();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao inserir Operação ao Componente");
      }
    }

    [RelayCommand]
    private void RemoverOperacao(CartaoOperacao cartao) {
      Cartoes.Remove(cartao);
      AtualizarProcessos();
    }

    /// <summary>Reordena por arrastar e soltar.</summary>
    public void MoverOperacao(CartaoOperacao cartao, int novoIndice) {
      var atual = Cartoes.IndexOf(cartao);
      if (atual < 0 || novoIndice < 0 || novoIndice >= Cartoes.Count || atual == novoIndice)
        return;

      Cartoes.Move(atual, novoIndice);
      AtualizarProcessos();
    }

    private void AtualizarProcessos() {
      var linha = ProdutoSelecionado;
      if (linha == null)
        return;

      var produtoErp = linha.Produto;

      var operacoes = Cartoes.Select(a => a.Operacao).ToList();

      if (operacoes != null && operacoes.Count() > 0) {
        for (int i = 1; i <= operacoes.Count; i++) {
          produto_erp_operacao operacao = operacoes[i - 1];
          operacao.sequencia = i;
        }
        produtoErp.Operacoes = operacoes;

        ProdutoErp.RemoverPendencia(produtoErp, PendenciasEngenharia.OperacaoRevisar);
        ProdutoErp.RemoverPendencia(produtoErp, PendenciasEngenharia.OperacaoNaoPossui);

        AtualizarInfoAlerta(produtoErp);
        linha.Atualizar();
      }

      CarregarSequenciaOp(produtoErp);
      Notificacao.Sucesso("Processo atualizado com sucesso!");
    }

    #endregion

    #region Sequência operacional

    private void CarregarSequenciaOp(ProdutoErp produtoErp) {
      try {
        Etapas.Clear();
        CriarControlesGrupoEtapas(produtoErp);
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar sequência operacional");
      }
    }

    /// <summary>Um grupo por operação; cada filho do componente fica no grupo da sua sequência (ou no primeiro).</summary>
    private void CriarControlesGrupoEtapas(ProdutoErp produtoErp) {
      PossuiVariasEtapas = produtoErp.Operacoes.Count > 1;

      if (produtoErp.Operacoes.Count == 0 || _noSelecionado == null)
        return;

      foreach (var proc in produtoErp.Operacoes) {
        var operacao = Processo.ListaProcessos.FirstOrDefault(x => x.codAxion == proc.processo_id);
        if (operacao != null)
          operacao.sequencia = proc.sequencia;

        Etapas.Add(new EtapaConsumo {
          Sequencia = proc.sequencia,
          Titulo = operacao != null ? $"{operacao.codOperacao} {operacao.descrOperacao}" : $"Processo {proc.processo_id}",
        });
      }

      foreach (TreeNode nodeFilho in _noSelecionado.Nodes) {
        if (!(nodeFilho.Tag is ProdutoErp prod))
          continue;

        if (prod.SeqOperacional == 0)
          prod.SeqOperacional = 1;

        var etapa = Etapas.FirstOrDefault(x => x.Sequencia == prod.SeqOperacional) ?? Etapas[0];
        etapa.Produtos.Add(prod);
      }
    }

    /// <summary>Move o item para outra operação (arrastar ou botão "mover"): muda a sequência operacional do filho.</summary>
    public void MoverProdutoParaEtapa(ProdutoErp produto, EtapaConsumo destino, bool noInicio) {
      var origem = Etapas.FirstOrDefault(x => x.Produtos.Contains(produto));
      if (origem == null || destino == null || origem == destino)
        return;

      origem.Produtos.Remove(produto);
      if (noInicio)
        destino.Produtos.Insert(0, produto);
      else
        destino.Produtos.Add(produto);

      produto.SeqOperacional = destino.Sequencia;
    }

    #endregion

    #region Salvar engenharia

    [RelayCommand]
    private async Task SalvarAsync() {
      try {
        var linha = ProdutoSelecionado;
        if (linha == null) {
          Notificacao.Info($"Nenhum produto selecionado");
          return;
        }

        if (_produtos.Count == 0) {
          Notificacao.Aviso("Carregar componentes primeiro");
          return;
        }

        if (_produtos.Count != ProdutosView.Cast<object>().Count()) {
          Notificacao.Aviso("Remova o filtro da Tabela de Componentes para prosseguir.");
          return;
        }

        var produtoERP = linha.Produto;

        if (produtoERP.TipoComponente == TipoComponente.ItemBiblioteca) {
          Notificacao.Aviso("Recurso indisponivel para o componentes de biblioteca.");
          return;
        }

        var possuiErro = false;
        _produtos.Where(x => x.Nivel == produtoERP.Nivel || x.Nivel.StartsWith($"{produtoERP.Nivel}.")).ToList().ForEach(x => {
          if (x.Pendencias.Count > 0) {
            var msgPend = string.Empty;
            x.Pendencias.ForEach(y => {
              msgPend += $"- {y.ObterDescricaoEnum()}\r\n";
            });

            Notificacao.Aviso($"{x.Name} [{x.Nivel}]\r\n{msgPend}");
            possuiErro = true;
          }
        });

        if (possuiErro) {
          return;
        }

        await CadastrarNovoAsync(produtoERP);
      } catch (Exception ex) {
        Lm.MsgBox.Show($"Erro ao salvar..\n\n{ex.Message}", "Axion LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private async Task CadastrarNovoAsync(ProdutoErp produtoErp) {
      try {
        BotoesHabilitados = false;
        _avisouCancelamento = false;

        using (ContextoDados db = new ContextoDados()) {
          var noEngenharia = _noSelecionado;

          // salvar engenharia
          await Loader.ShowDuringOperation(async (progress) => {
            progress.Report("Criando Engenharia de Produto...");
            var configApi = configuracao_api.Selecionar();

            await PercorrerTreeViewSalvarEngAsync(db, noEngenharia, configApi, progress);
          });

          AtualizarInfoAlerta(produtoErp);

          Notificacao.Sucesso("Cadastro de engenharia finalizado com sucesso");
        }
      } catch (Exception ex) {
        Lm.MsgBox.Show($"Erro ao atualizar tempalte\n\n{ex.Message}", "Axion LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      } finally {
        BotoesHabilitados = true;
      }
    }

    /// <summary>O aviso de cancelamento aparece uma vez (no form saía uma caixa para cada nó restante).</summary>
    bool _avisouCancelamento;

    private async Task PercorrerTreeViewSalvarEngAsync(ContextoDados db, TreeNode node, configuracao_api configApi, IProgress<string> progress) {
      try {
        if (!Loader._isWorking) {
          if (!_avisouCancelamento) {
            _avisouCancelamento = true;
            Lm.MsgBox.Show($"Cadastro de engenharia cancelado pelo usuário.", "Ação não Permitida", MessageBoxButton.OK, MessageBoxImage.Warning);
          }
          return;
        }

        var produtoErp = node.Tag as ProdutoErp;
        if (produtoErp != null) {
          if (produtoErp.TipoComponente != TipoComponente.ListaMaterial && produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
            foreach (TreeNode nodeFilho in node.Nodes) {
              await PercorrerTreeViewSalvarEngAsync(db, nodeFilho, configApi, progress);
            }

            // inativar item aqui se for fantasma
            if (produtoErp.Fantasma) {
              produtoErp.Situacao = 0;
              await Api.UpdateItemGenericoAsync(produtoErp);
            }

            var produto = _produtos.ToList().FirstOrDefault(x => produtoErp.Name == x.Name && produtoErp.Referencia == x.Referencia && produtoErp.Configuracao == x.Configuracao);

            if (produto != null) {
              produtoErp.CodProduto = produto.CodProduto;
              produtoErp.Denominacao = produto.Denominacao;
              produtoErp.SobremetalCompr = produto.SobremetalCompr;
              produtoErp.SobremetalLarg = produto.SobremetalLarg;
              produtoErp.Operacoes = produto.Operacoes;
            }

            var engenharia = new Engenharia {
              codEmpresa = configApi.codigoEmpresa,
              codProduto = produtoErp.CodProduto,
              tipoModulo = "S",
              codClassificacao = produtoErp.TipoComponente == TipoComponente.Montagem ? 3 : 4,
              nomeArquivoDesenhoEng = produtoErp.Name,
              descricaoProduto = $"{produtoErp.Denominacao} - {produtoErp.Name}",
              engenhariaFantasma = produtoErp.Fantasma,
              descEngenhariaFantasma = produtoErp.Fantasma ? "Engenharia Fantasma" : "",
            };

            if (node.Nodes.Count > 0) {
              System.Collections.IList list = node.Nodes;
              for (int i = 0; i < list.Count; i++) {
                int index = i + 1;
                TreeNode nodeFilho = (TreeNode)list[i];
                var itemFilho = nodeFilho.Tag as ProdutoErp;
                if (itemFilho != null) {
                  var classificacao = itemFilho.CodComponente.StartsWith("10") || itemFilho.CodComponente.StartsWith("20") ? 5 : itemFilho.TipoComponente == TipoComponente.Montagem ? 3 : 4;

                  double qtd = itemFilho.Quantidade;
                  double espessura = itemFilho.Espessura;
                  double largura = itemFilho.Largura + itemFilho.SobremetalLarg;
                  double compr = itemFilho.Comprimento + itemFilho.SobremetalCompr;

                  var componenteEng = new ComponenteEng {
                    seqComponente = index,
                    seqOperacional = itemFilho.SeqOperacional <= produtoErp.Operacoes.Count ? itemFilho.SeqOperacional : 0,
                    codInsumo = itemFilho.CodProduto,
                    quantidade = qtd,
                    itemKanban = 0,
                    comprimento = compr,
                    largura = largura,
                    espessura = espessura,
                    percQuebra = 0,
                    codClassificacaoInsumo = classificacao, // 1 = produto, 3 = subconjunto, 4 = peças e 5 = insumo comprado
                  };
                  engenharia.componentes.Add(componenteEng);
                }
              }
            }

            if (produtoErp.Operacoes != null && produtoErp.Operacoes.Count > 0) {
              for (int seqOperacao = 1; seqOperacao <= produtoErp.Operacoes.Count; seqOperacao++) {
                var proc = produtoErp.Operacoes[seqOperacao - 1];

                var processo = Processo.ListaProcessos.FirstOrDefault(x => x.codAxion == proc.processo_id);
                if (processo == null) {
                  Notificacao.Aviso($"Processo '{proc.processo_id}' não encontrado no Axion.");
                  continue;
                }

                var operacaoEng = new OperacaoEng {
                  seqOperacao = seqOperacao,
                  codOperacao = processo.codOperacao,
                  abreviaturaOperacao = processo.abreviatura,
                  numOperadores = proc.qtd_operador,
                  codFaseOperacao = processo.faseProducao,
                  codMascaraMaquina = processo.mascaraMaquina?.Replace(".", ""),
                  centroCusto = processo.centroCusto,
                  tempoPadraoOperacao = proc.tempo.FormatarHoraDouble(),
                  tempoPreparacaoOperacao = 0,
                  tipoOperacao = processo.tipoOperacao,
                };
                engenharia.operacoes.Add(operacaoEng);
              }
            }

            progress.Report($"Cadastrando Engenharia:\r\n{produtoErp.CodProduto} - {produtoErp.Denominacao}");
            await Api.CadastrarEngenhariaAsync(db, engenharia);

            // Linha verde no grid (corSucesso do form)
            var linha = Produtos.FirstOrDefault(x => x.CodProduto == produto?.CodProduto);
            if (linha != null)
              linha.Salvo = true;
          }
        }
      } catch (Exception ex) {
        Notificacao.Erro($"Erro ao gerar Engenharia.\n\nItem: {((ProdutoErp)node.Tag).Name}\n\n{ex.Message}");
      }
    }

    #endregion
  }
}
