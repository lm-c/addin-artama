using LmCorbieUI;
using LmCorbieUI.Metodos;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using static AddinArtama.Api;
using TreeNode = System.Windows.Forms.TreeNode;
using Wpf = LmCorbieUI.Wpf;
using WinTreeView = System.Windows.Forms.TreeView;

namespace AddinArtama {
  /// <summary>
  /// Cadastro de produtos e engenharia no ERP a partir da montagem (substitui o FrmProdutoImport).
  /// A leitura da montagem continua montando a árvore num TreeView WinForms (regra do ProdutoErp);
  /// a aba Engenharia mostra uma cópia dela em WPF.
  /// </summary>
  public partial class ViewProdutoImport : Wpf.LmView {
    /// <summary>Nó exibido na árvore WPF (espelho de um TreeNode).</summary>
    public class NoArvore {
      public string Texto { get; set; }
      public ImageSource Imagem { get; set; }
      public List<NoArvore> Filhos { get; } = new List<NoArvore>();
    }

    /// <summary>Mesmos ícones/índices do ImageList do WinForms.</summary>
    static readonly ImageSource[] imagensNo = {
      ImagemWpf.Converter(Properties.Resources.assembly),
      ImagemWpf.Converter(Properties.Resources.part),
      ImagemWpf.Converter(Properties.Resources.weldmentcutlist),
      ImagemWpf.Converter(Properties.Resources.sheetmetal),
      ImagemWpf.Converter(Properties.Resources.weldment),
      ImagemWpf.Converter(Properties.Resources.toolbox_item),
    };

    static readonly ImageSource imgErro = ImagemWpf.Converter(Properties.Resources.error);
    static readonly ImageSource imgAlerta = ImagemWpf.Converter(Properties.Resources.warning);

    static readonly Brush corErro = Brushes.Red;
    static readonly Brush corSucesso = CriarCorSucesso();

    static Brush CriarCorSucesso() {
      var cor = new SolidColorBrush(Color.FromRgb(0x22, 0xA0, 0x45));
      cor.Freeze();
      return cor;
    }

    string _montageGeralNome = string.Empty;
    SortableBindingList<ProdutoErp> _produtos = new SortableBindingList<ProdutoErp>();
    readonly WinTreeView _arvoreCompleta = new WinTreeView();
    bool _canceladoComErro = false;

    /// <summary>Nó (WinForms) do produto selecionado: a engenharia é gerada a partir dele.</summary>
    TreeNode _noEngenharia;

    ListCollectionView visao;

    /// <summary>Contador do andamento da geração da engenharia (percentual no aguarde).</summary>
    int _engAtual;
    int _engTotal;

    /// <summary>Pisca o ícone de material errado.</summary>
    readonly DispatcherTimer tmr = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
    bool useFirstImage = true;

    public ViewProdutoImport() {
      InitializeComponent();

      tmr.Tick += Tmr_Tick;
      Unloaded += (s, e) => tmr.Stop();

      AplicarCorCadastro();

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      // Cada passo seleciona a aba onde está o controle destacado.
      Wpf.LmPassoAssistente Passo(int aba, string titulo, string texto, FrameworkElement alvo, FrameworkElement alvoAte = null) {
        var passo = Wpf.LmPassoAssistente.Info(titulo, texto, alvo, alvoAte);
        passo.AoEntrar = () => tbcOperacoes.SelectedIndex = aba;
        return passo;
      }

      yield return Wpf.LmPassoAssistente.Info("Cadastro de Produto/Engenharia",
        "Esta tela envia o projeto para o ERP: cadastra (ou atualiza) cada componente como produto e depois monta a engenharia — a estrutura de componentes e as operações.\n\n" +
        "Antes de começar: aplique os processos e a matéria prima nos componentes (tela Aplicação de Processos/Matéria Prima).\n\n" +
        "Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Aguardar("1. Abra a peça ou a montagem",
        "No SolidWorks, abra (e deixe ativa) a peça ou a montagem principal do projeto.\n\n" +
        "Regra: o carregamento só funciona com peça ou montagem ativa; com um desenho ativo ele é recusado.",
        ViewDesenho.TemModeloAtivo);

      yield return Wpf.LmPassoAssistente.Aguardar("2. Carregar os componentes",
        "Clique em Carregar e aguarde a leitura da montagem e de todos os componentes (pode levar um tempo em montagens grandes).\n\n" +
        "Regra: se já houver componentes carregados, o sistema pergunta antes de substituir.",
        () => dgv.Items.Count > 0, btnCarrProcess);

      yield return Passo(0, "3. Lista de componentes",
        "Cada linha é um componente, com o nível na estrutura, o nome, o código do produto no ERP (quando já existe), a descrição e a referência. " +
        "Os ícones indicam o 3D, o 2D, item fantasma e pendências.\n\n" +
        "Regras: ao selecionar uma linha, o componente que estava aberto é salvo e fechado, e o da linha é aberto no SolidWorks. " +
        "Depois de um cadastro, a linha fica VERDE quando deu certo e VERMELHA quando deu erro.",
        dgv);

      yield return Passo(0, "4. Procurar",
        "Filtra a lista por nível, nome, código, descrição ou referência.\n\n" +
        "Regra: com filtro ativo o Salvar é bloqueado — limpe o campo antes de salvar.",
        txtProcurar);

      yield return Wpf.LmPassoAssistente.Info("5. Dados do componente",
        "Mostra o código do produto (clique nele para copiar), a dimensão, o material e os pesos do componente selecionado.\n\n" +
        "Regra: quando a matéria prima está errada, um ícone pisca ao lado do material. Clique nele para escolher a chapa correta — ela é gravada na peça na hora.",
        grdInfo);

      yield return Wpf.LmPassoAssistente.Info("6. Denominação",
        "Descrição do produto, com até 50 caracteres.\n\n" +
        "Regra: TODOS os componentes da lista precisam ter denominação; se faltar em algum, o Salvar é bloqueado.",
        txtDescricao);

      yield return Wpf.LmPassoAssistente.Info("7. Sobremetal",
        "Material a mais na largura e no comprimento da chapa, em milímetros.\n\n" +
        "Regras: o valor é somado às dimensões que vão para o ERP e o peso é recalculado. O máximo é 500.",
        txtSmLarg, txtSmCompr);

      yield return Wpf.LmPassoAssistente.Info("8. Pendências",
        "Aqui aparecem as pendências do componente selecionado: em vermelho as críticas, em amarelo as demais.\n\n" +
        "Regra: enquanto o componente a salvar, ou qualquer filho dele, tiver pendência, o Salvar é bloqueado.",
        lblWarning);

      yield return Passo(1, "9. Engenharia",
        "A aba Engenharia mostra a estrutura do componente selecionado: ele e tudo o que está abaixo dele. É exatamente isso que será enviado como engenharia.",
        tbcOperacoes);

      yield return Passo(0, "10. Salvar",
        "Envia para o ERP o componente SELECIONADO e todos os filhos dele: primeiro os produtos, depois a engenharia. Para enviar o projeto inteiro, selecione a primeira linha.\n\n" +
        "Regras:\n" +
        "• Itens de biblioteca não podem ser o ponto de partida.\n" +
        "• O código gerado pelo ERP é gravado na propriedade \"Código Produto\" de cada arquivo.\n" +
        "• A engenharia só é gravada se ainda não existir no ERP ou se estiver \"Em desenvolvimento\".\n" +
        "• Um item com quantidade zero interrompe a engenharia, com aviso.\n\n" +
        "O andamento aparece na tela e pode ser cancelado. Ao final, uma mensagem informa se terminou com sucesso ou com erro.",
        btnSalvar);

      yield return Wpf.LmPassoAssistente.Info("Pronto!",
        "O ciclo é: Carregar → conferir os dados e resolver as pendências → selecionar o item → Salvar.\n\n" +
        "Para rever este passo a passo, clique no \"?\" no topo da tela.");
    }

    #endregion

    ProdutoErp Atual => dgv.SelectedItem as ProdutoErp;

    #region Carregar

    private async void BtnCarrProcess_Click(object sender, RoutedEventArgs e) {
      try {
        MostrarMaterialErrado(false);

        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Warning("Sem documentos abertos");
          return;
        }

        if (dgv.Items.Count > 0) {
          var result = Wpf.MsgBox.Show("Já existem componentes carregados. Deseja substituir os dados atuais?",
            "Addin LM Projetos", MessageBoxButton.YesNo, MessageBoxImage.Question);
          if (result == MessageBoxResult.No)
            return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
          _arvoreCompleta.Nodes.Clear();
          trvProduto.ItemsSource = null;
          _noEngenharia = null;

          _produtos = new SortableBindingList<ProdutoErp>();
          CarregarGrid();

          _montageGeralNome = Path.GetFileNameWithoutExtension(swModel.GetPathName());

          if (_montageGeralNome.Length > 50)
            _montageGeralNome = _montageGeralNome.Substring(0, 50);

          btnCarrProcess.IsEnabled = btnSalvar.IsEnabled = false;
          try {
            using (var worker = new LmBackgroundWorker()) {
              await worker.RunAsync("Limpando dados antigos...", async (progress, token) => {
                progress.Report(($"Iniciando leitura Componentes", 100, 100));
                _produtos = await ProdutoErp.GetComponentsAsync(_arvoreCompleta, _montageGeralNome, progress, token);

                progress.Report(($"Carregando Grid", 100, 100));
              });
            }
          } finally {
            btnCarrProcess.IsEnabled = btnSalvar.IsEnabled = true;
          }

          CarregarGrid();

          Wpf.Toast.Success("Concluído!");
        } else {
          Wpf.Toast.Warning("Comando apenas para Peças e Montagens");
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Carregar Componentes..");
      }
    }

    private void CarregarGrid() {
      visao = new ListCollectionView(_produtos) { Filter = Filtrar };
      dgv.ItemsSource = visao;
    }

    /// <summary>Procura em nível, nome, código, denominação e referência.</summary>
    private bool Filtrar(object item) {
      var procurar = txtProcurar.Text;
      if (string.IsNullOrWhiteSpace(procurar) || !(item is ProdutoErp p))
        return true;

      return new[] { p.Nivel, p.Name, p.CodProduto, p.Denominacao, p.Referencia }
        .Any(x => x != null && x.IndexOf(procurar, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    private void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e) {
      visao?.Refresh();
    }

    #endregion

    #region Salvar (produto + engenharia)

    private async void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        if (Atual == null) {
          Wpf.Toast.Info($"Nenhum produto selecionado");
          return;
        }

        if (_produtos.Count == 0) {
          Wpf.Toast.Warning("Carregar Desenhos primeiro");
          return;
        }

        if (_produtos.Any(x => string.IsNullOrEmpty(x.Denominacao))) {
          Wpf.Toast.Warning("Favor preencher todas as Denominações");
          return;
        }

        var produtoERP = Atual;

        if (produtoERP.TipoComponente == TipoComponente.ItemBiblioteca) {
          Wpf.Toast.Warning("Recurso indisponivel para o componentes de biblioteca.");
          return;
        }

        if (!string.IsNullOrEmpty(txtProcurar.Text)) {
          Wpf.Toast.Warning("Remova o filtro do Grid para salvar Produtos.");
          return;
        }

        var possuiErro = false;
        _produtos.Where(x => x.Nivel == produtoERP.Nivel || x.Nivel.StartsWith($"{produtoERP.Nivel}.")).ToList().ForEach(x => {
          if (x.Pendencias.Count > 0) {
            var msgPend = string.Empty;
            x.Pendencias.ForEach(y => {
              msgPend += $"- {y.ObterDescricaoEnum()}\r\n";
            });

            Wpf.Toast.Warning($"{x.Name} [{x.Nivel}]\r\n{msgPend}");
            possuiErro = true;
          }
        });

        if (possuiErro) {
          return;
        }

        await CadastrarNovo(produtoERP);
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao salvar..");
      }
    }

    /// <summary>Cadastra/atualiza o produto selecionado e os filhos dele (pela ordem da grade) e depois a engenharia.</summary>
    private async Task CadastrarNovo(ProdutoErp produtoErp) {
      var nomeProduto = string.Empty;
      try {
        _canceladoComErro = false;

        btnSalvar.IsEnabled = btnCarrProcess.IsEnabled = false;

        var startIndex = _produtos.IndexOf(produtoErp);
        var noEngenharia = _noEngenharia;

        using (var worker = new LmBackgroundWorker()) {
          await worker.RunAsync("Iniciando Cadastro de Produtos...", async (progress, token) => {
            using (ContextoDados db = new ContextoDados()) {
              ModelDoc2 swModel = default(ModelDoc2);
              var nivelPai = produtoErp.Nivel;
              var nameItemPai = Path.GetFileNameWithoutExtension(produtoErp.PathName);

              int status = 0;
              int warnings = 0;

              var config = db.configuracao_api.FirstOrDefault();

              // Total de produtos a cadastrar: o selecionado e os filhos dele (linhas seguintes da grade).
              var indiceInicial = startIndex;
              var totalCadastro = 1;
              while (indiceInicial + totalCadastro < _produtos.Count && _produtos[indiceInicial + totalCadastro].Nivel.Contains(nivelPai + "."))
                totalCadastro++;

              while (true) {
                if (token.IsCancellationRequested) {
                  Wpf.MsgBox.Show($"Cadastro de produtos cancelado pelo usuário.", "Ação não Permitida", MessageBoxButton.OK, MessageBoxImage.Warning);
                  return;
                }

                nomeProduto = produtoErp.Name;

                progress.Report(($"Cadastrando produto...\r\n{nomeProduto}", startIndex - indiceInicial + 1, totalCadastro));

                int tipo = produtoErp.PathName.EndsWith("SLDASM")
                ? (int)swDocumentTypes_e.swDocASSEMBLY
                : (int)swDocumentTypes_e.swDocPART;

                produtoErp.PesoBruto = Math.Round(produtoErp.PesoBruto, 4);
                if (produtoErp.PesoLiquido > produtoErp.PesoBruto)
                  produtoErp.PesoLiquido = produtoErp.PesoBruto;

                if (produtoErp.CadastrarProdutoErp) {
                  var itemGenerico = new Api.ItemGenerico();
                  MontarItemGenerico(produtoErp, itemGenerico);

                  if (produtoErp.Fantasma) {
                    itemGenerico.situacao = 0;
                  }

                  var codigoNovo = await Api.DuplicarItemGenericoAsync(db, itemGenerico);

                  if (!string.IsNullOrEmpty(codigoNovo)) {
                    produtoErp.CodProduto = codigoNovo;
                    if (produtoErp.CadastrarAddin) {
                      CadastrarAddin(db, produtoErp);
                    } else {
                      AtualizarAddin(db, produtoErp);
                    }
                  }
                } else {
                  if (produtoErp.TipoComponente != TipoComponente.ItemBiblioteca && produtoErp.TipoComponente != TipoComponente.ListaMaterial && !produtoErp.Name.StartsWith("4")) {
                    await Api.UpdateItemGenericoAsync(db, produtoErp);
                  }

                  if (!string.IsNullOrEmpty(produtoErp.CodProduto)) {
                    if (produtoErp.CadastrarAddin) {
                      CadastrarAddin(db, produtoErp);
                    } else {
                      AtualizarAddin(db, produtoErp);
                    }
                  }
                }

                if (produtoErp.CadastrarAddin || produtoErp.CadastrarProdutoErp) {
                  swModel = Sw.App.OpenDoc6(produtoErp.PathName, tipo,
                    (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

                  if (swModel != null && !swModel.IsOpenedReadOnly()) {
                    var swModelDocExt = swModel.Extension;
                    var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

                    if (produtoErp.Referencia.StartsWith("Item da lista de corte")) {
                      bool boolstatus = swModel.Extension.SelectByID2(produtoErp.Referencia, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

                      SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
                      Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
                      swCustPropMgr = swFeat.CustomPropertyManager;
                    }

                    swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                    swModel.Save();

                    if (Path.GetFileNameWithoutExtension(produtoErp.PathName) != nameItemPai)
                      Sw.App.CloseDoc(produtoErp.PathName);
                  }
                }

                produtoErp.Pendencias = new List<PendenciasEngenharia>();

                var cadastrado = produtoErp;
                if (!string.IsNullOrEmpty(produtoErp.CodProduto)) {
                  produtoErp.ImgPendencia = new System.Drawing.Bitmap(20, 20);
                  Dispatcher.Invoke(() => MarcarCadastro(cadastrado, true));
                } else {
                  produtoErp.ImgPendencia = Properties.Resources.error;
                  produtoErp.Pendencias.Add(PendenciasEngenharia.ErroAoCadastrarCod);
                  Dispatcher.Invoke(() => MarcarCadastro(cadastrado, false));
                }

                Dispatcher.Invoke(() => AtualizarInfoAlerta(cadastrado));

                produtoErp.CadastrarProdutoErp = produtoErp.CadastrarAddin = false;

                startIndex++;
                if (startIndex >= _produtos.Count)
                  break;

                var prod = _produtos[startIndex];
                Dispatcher.Invoke(() => dgv.ScrollIntoView(prod));

                if (prod.Nivel.Contains(nivelPai + "."))
                  produtoErp = prod;
                else break;
              }

              // salvar engenharia
              progress.Report(("Criando Engenharia de Produto...", 0, 0));
              var configApi = configuracao_api.Selecionar();

              if (noEngenharia != null) {
                _engAtual = 0;
                _engTotal = Math.Max(1, ContarNosEngenharia(noEngenharia));
                await PercorrerTreeViewSalvarEngAsync(db, noEngenharia, configApi, progress, token);
              }

              swModel = (ModelDoc2)Sw.App.ActiveDoc;

              swModel.Save3(5, ref status, ref warnings);
            }

            Wpf.Toast.Success("Concluído!");
          });
        }

        lblCodigoProduto.Content = produtoErp.CodProduto;

        if (!_canceladoComErro)
          Wpf.MsgBox.Show("Cadastro de produtos e engenharia finalizado com sucesso", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        else
          Wpf.MsgBox.Show("Cadastro de produtos e engenharia finalizado com Erro", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      } catch (Exception ex) {
        Msg.Erro(ex, $"Erro ao cadastrar produto\n\nItem: {nomeProduto}");
      } finally {
        btnSalvar.IsEnabled = btnCarrProcess.IsEnabled = true;
      }
    }

    /// <summary>Quantos nós a geração da engenharia vai percorrer (base do percentual).</summary>
    static int ContarNosEngenharia(TreeNode node) {
      int total = node.Tag is ProdutoErp ? 1 : 0;

      // Lista de material e item de biblioteca não descem para os filhos (mesma regra do PercorrerTreeViewSalvarEngAsync).
      if (node.Tag is ProdutoErp p && (p.TipoComponente == TipoComponente.ListaMaterial || p.TipoComponente == TipoComponente.ItemBiblioteca))
        return total;

      foreach (TreeNode filho in node.Nodes)
        total += ContarNosEngenharia(filho);

      return total;
    }

    private async Task PercorrerTreeViewSalvarEngAsync(ContextoDados db, TreeNode node, configuracao_api configApi, IProgress<(string message, int current, int max)> progress, System.Threading.CancellationToken token) {
      try {
        if (_canceladoComErro) {
          return;
        }

        if (token.IsCancellationRequested) {
          _canceladoComErro = true;
          return;
        }

        var produtoErp = node.Tag as ProdutoErp;

        if (produtoErp != null) {
          progress.Report(($"Gerando Engenharia...\r\n{produtoErp.Name}", ++_engAtual, _engTotal));

          if (produtoErp.TipoComponente != TipoComponente.ListaMaterial && produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
            foreach (TreeNode nodeFilho in node.Nodes) {
              await PercorrerTreeViewSalvarEngAsync(db, nodeFilho, configApi, progress, token);
            }

            var produto = _produtos.ToList().FirstOrDefault(x => produtoErp.Name == x.Name && produtoErp.Referencia == x.Referencia && produtoErp.Configuracao == x.Configuracao);

            if (produto != null) {
              produtoErp.CodProduto = produto.CodProduto;
              produtoErp.Denominacao = produto.Denominacao;
              produtoErp.Operacoes = produto.Operacoes;
              produtoErp.UnidadeMedida = produto.UnidadeMedida;
              produtoErp.PesoLiquido = produto.PesoLiquido;
              produtoErp.PesoBruto = produto.PesoBruto;
              produtoErp.PesoPadraoNBR = produto.PesoPadraoNBR;
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
                  var produtoFilho = _produtos.ToList().FirstOrDefault(x => itemFilho.Name == x.Name && itemFilho.Referencia == x.Referencia && itemFilho.Configuracao == x.Configuracao);
                  if (produtoFilho != null) {
                    itemFilho.UnidadeMedida = produtoFilho.UnidadeMedida;
                    itemFilho.PesoBruto = produtoFilho.PesoBruto;
                    itemFilho.PesoLiquido = produtoFilho.PesoLiquido;
                    itemFilho.SobremetalCompr = produtoFilho.SobremetalCompr;
                    itemFilho.SobremetalLarg = produtoFilho.SobremetalLarg;
                    if (itemFilho.ItemCorte != null)
                      itemFilho.ItemCorte.unidadeMedida = produtoFilho.ItemCorte.unidadeMedida;
                  }

                  var classificacao = itemFilho.CodComponente.StartsWith("1") || itemFilho.CodComponente.StartsWith("2") ? 5 : itemFilho.TipoComponente == TipoComponente.Montagem ? 3 : 4;

                  double qtd = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? itemFilho.Quantidade : 1;
                  double espessura = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? 0 : itemFilho.ItemCorte.Espessura;
                  double larg = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? 0 : itemFilho.ItemCorte.Largura + itemFilho.SobremetalLarg;
                  double compr = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? itemFilho.Comprimento : itemFilho.ItemCorte.Comprimento + itemFilho.SobremetalCompr;
                  string um = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? itemFilho.UnidadeMedida : itemFilho.ItemCorte.unidadeMedida;

                  if (itemFilho.TipoComponente == TipoComponente.ListaMaterial) {
                    switch (um) {
                      case "M":
                      qtd = (compr / 1000); // para cabo de aço
                      break;
                      case "M2":
                      qtd = (larg / 1000) * (compr / 1000);
                      break;
                      case "KG":
                      qtd = produtoErp.PesoBruto;
                      break;
                      default:
                      break;
                    }
                  } else if (itemFilho.TipoComponente == TipoComponente.ItemBiblioteca && compr > 0) {
                    switch (itemFilho.UnidadeMedida) {
                      case "M":
                      qtd = (compr / 1000) * qtd; // para cabo de aço
                      break;
                      case "M2":
                      qtd = (larg / 1000) * (compr / 1000);
                      break;
                      case "KG":
                      qtd = produtoErp.PesoBruto;
                      break;
                      case "L":
                      case "LT":
                      qtd = compr * qtd;
                      break;
                      default:
                      break;
                    }
                  }

                  if (qtd == 0) {
                    _canceladoComErro = true;
                    Wpf.MsgBox.Show($"Não é possível cadastrar engenharia com quantidade '0' para o item: {itemFilho.Name}.", "Addin LM Projetos",
                      MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // não pode cadastrar engenharia com quantidade 0
                  }

                  var componenteEng = new ComponenteEng {
                    seqComponente = index,
                    codInsumo = itemFilho.CodProduto,
                    quantidade = qtd,
                    itemKanban = 0,
                    comprimento = compr,
                    largura = larg,
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
                  Wpf.Toast.Warning($"Processo '{proc.processo_id}' não encontrado no Axion.");
                  continue;
                }
                var operacaoEng = new OperacaoEng {
                  seqOperacao = seqOperacao,
                  codOperacao = processo.codOperacao,
                  abreviaturaOperacao = processo.abreviatura,
                  numOperadores = proc.qtd_operador,
                  codFaseOperacao = processo.faseProducao,
                  codMascaraMaquina = processo.mascaraMaquina.Replace(".", ""),
                  centroCusto = processo.centroCusto,
                  tempoPadraoOperacao = proc.tempo.FormatarHoraDouble(),
                  tempoPreparacaoOperacao = 0,
                  tipoOperacao = processo.tipoOperacao,
                };
                engenharia.operacoes.Add(operacaoEng);
              }
            }

            var eng = await Api.GetEngenhariaAsync(engenharia.codProduto, ignorarMsgError: true);

            if (eng == null || eng.statusEngenharia == StatusEngenharia.EmDesenvolvimento) {
              if (eng != null && engenharia.codProduto.StartsWith("4"))
                engenharia.descricaoProduto = eng.descricaoProduto;

              await Api.CadastrarEngenhariaAsync(db, engenharia);
            }
          }
        }
      } catch (Exception ex) {
        _canceladoComErro = true;
        Wpf.Toast.Error($"Erro ao gerar Engenharia Item: {((ProdutoErp)node.Tag).Name}\n\n{ex.Message}");
      }
    }

    private static void CadastrarAddin(ContextoDados db, ProdutoErp item) {
      try {
        var espessura = 0.0;
        var largura = 0.0;
        var comprimento = 0.0;

        if (item.TipoComponente == TipoComponente.Peca && item.ItemCorte != null) {
          var itemCorte = item.ItemCorte;
          espessura = itemCorte.Espessura;
          largura = itemCorte.Largura;
          comprimento = itemCorte.Comprimento;
        }

        var descricao = item.Denominacao.Length <= 70 ? item.Denominacao : item.Denominacao.Substring(0, 70);
        var referencia = item.Referencia.Length <= 50 ? item.Referencia : item.Referencia.Substring(0, 50);

        var produtoERP = new produto_erp {
          codigo_produto = Convert.ToInt64(item.CodProduto),
          codigo_componente = item.CodComponente,
          tipo_componente = item.TipoComponente,
          name = item.Name,
          descricao = descricao,
          pathname = item.PathName,
          referencia = referencia,
          configuracao = item.Configuracao,
          sobremetal_largura = item.SobremetalLarg,
          sobremetal_comprimento = item.SobremetalCompr,
          espessura = espessura,
          largura = largura,
          comprimento = comprimento,
          peso_liquido = item.PesoLiquido,
          peso_bruto = item.PesoBruto,
          fantasma = item.Fantasma,
          adicionado_por_id = usuario_alocados.model.usuario_id,
          adicionado_em = DateTime.Now,
          alterado_por_id = usuario_alocados.model.usuario_id,
          alterado_em = DateTime.Now,
        };

        if (produtoERP.descricao.Length > 70)
          produtoERP.descricao = produtoERP.descricao.Substring(0, 70);

        db.produto_erp.Add(produtoERP);
        db.SaveChanges();
      } catch (Exception ex) {
        Msg.Erro(ex, $"Item: {item.Name} | Erro ao cadastrar");
      }
    }

    private static void AtualizarAddin(ContextoDados db, ProdutoErp item) {
      try {
        var espessura = 0.0;
        var largura = 0.0;
        var comprimento = 0.0;

        if (item.TipoComponente == TipoComponente.Peca && item.ItemCorte != null) {
          var itemCorte = item.ItemCorte;
          espessura = itemCorte.Espessura;
          largura = itemCorte.Largura;
          comprimento = itemCorte.Comprimento;
        }

        var codProd = Convert.ToInt64(item.CodProduto);
        var produtoErp = db.produto_erp.FirstOrDefault(x => x.codigo_produto == codProd);
        if (produtoErp == null) {
          Msg.Erro("Produto não encontrado no Axion", "Erro ao atualizar produto no Axion");
          return;
        }

        var descricao = item.Denominacao.Length <= 70 ? item.Denominacao : item.Denominacao.Substring(0, 70);

        // Atualiza os campos necessários
        produtoErp.name = item.Name;
        produtoErp.descricao = descricao;
        produtoErp.pathname = item.PathName;
        produtoErp.referencia = item.Referencia;
        produtoErp.configuracao = item.Configuracao;
        produtoErp.sobremetal_largura = item.SobremetalLarg;
        produtoErp.sobremetal_comprimento = item.SobremetalCompr;
        produtoErp.espessura = espessura;
        produtoErp.largura = largura;
        produtoErp.comprimento = comprimento;
        produtoErp.peso_liquido = item.PesoLiquido;
        produtoErp.peso_bruto = item.PesoBruto;
        produtoErp.fantasma = item.Fantasma;
        produtoErp.alterado_por_id = usuario_alocados.model.usuario_id;
        produtoErp.alterado_em = DateTime.Now;
        db.SaveChanges();
      } catch (Exception ex) {
        Msg.Erro(ex, $"Item: {item.Name} | Erro ao atualizar");
      }
    }

    #endregion

    #region Componente atual

    /// <summary>Trocar de linha salva/fecha o componente anterior e abre o novo.</summary>
    private void Dgv_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      try {
        if (Atual == null) return;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        SalvarFechar(swModel);

        AtualizarComponente();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao atualizar dados Componente");
      }
    }

    private void SalvarFechar(ModelDoc2 swModel) {
      if (swModel == null)
        return;

      var name = Path.GetFileNameWithoutExtension(swModel.GetPathName());
      if (name != _montageGeralNome) {
        swModel.ShowNamedView("*Isométrica");
        swModel.ViewZoomtofit();

        if (!swModel.IsOpenedReadOnly())
          swModel.Save();
        else Wpf.Toast.Warning($"Não salvo.\r\n{name} era somente leitura.");

        Sw.App.CloseDoc(swModel.GetPathName());
      }
    }

    private void AtualizarComponente() {
      try {
        lblPesoBrut.Content = "0,000Kg";
        lblPesoNbr.Content = "0,000Kg";
        lblEspess.Content =
        lblCodMat.Content =
        lblCodigoProduto.Content =
        lblDescMat.Content = string.Empty;

        var produtoErp = Atual;

        if (produtoErp.PathName.ToUpper().EndsWith(".SLDPRT"))
          Sw.App.OpenDoc6(produtoErp.PathName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, produtoErp.Configuracao, 0, 0);
        else
          Sw.App.OpenDoc6(produtoErp.PathName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, produtoErp.Configuracao, 0, 0);

        var swModel = (ModelDoc2)Sw.App.ActivateDoc2(Name: produtoErp.PathName, Silent: false, Errors: 0);
        if (swModel == null)
          return;

        swModel.ClearSelection2(true);
        swModel.ShowConfiguration2(produtoErp.Configuracao);

        if (produtoErp.ItemCorte != null)
          ListaCorte.RefreshCutList(swModel, "", produtoErp.ItemCorte);

        if (produtoErp.Referencia.StartsWith("Item da lista de corte")) {
          bool boolstatus = swModel.Extension.SelectByID2(produtoErp.Referencia, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);
        }

        AtualizarInfoAlerta(produtoErp);

        AtualizarInformacoes(produtoErp);

        // Árvore da engenharia: cópia do ramo deste produto.
        TreeNode node = GetNodeByLevelPath(_arvoreCompleta, produtoErp.Nivel);
        _noEngenharia = (TreeNode)node.Clone();
        trvProduto.ItemsSource = new[] { CriarNo(_noEngenharia) };
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Atualizar Dados");
      }
    }

    static NoArvore CriarNo(TreeNode node) {
      var no = new NoArvore {
        Texto = node.Text,
        Imagem = node.ImageIndex >= 0 && node.ImageIndex < imagensNo.Length ? imagensNo[node.ImageIndex] : imagensNo[0],
      };

      foreach (TreeNode filho in node.Nodes)
        no.Filhos.Add(CriarNo(filho));

      return no;
    }

    private void AtualizarInfoAlerta(ProdutoErp produtoErp) {
      var criticas = produtoErp.Pendencias.Where(y => y.EhPendenciaCritica()).ToList();
      var naoCriticas = produtoErp.Pendencias.Where(y => !y.EhPendenciaCritica()).ToList();

      if (criticas.Count > 0) {
        lblWarning.Text = string.Join("\r\n", criticas.Select(y => $"- {y.ObterDescricaoEnum()}"));
        lblWarning.Foreground = Brushes.Red;
        lblWarning.Visibility = Visibility.Visible;
      } else if (naoCriticas.Count > 0) {
        lblWarning.Text = string.Join("\r\n", naoCriticas.Select(y => $"- {y.ObterDescricaoEnum()}"));
        lblWarning.Foreground = Brushes.DarkGoldenrod;
        lblWarning.Visibility = Visibility.Visible;
      } else {
        lblWarning.Visibility = Visibility.Collapsed;
      }
    }

    private void AtualizarInformacoes(ProdutoErp produtoErp) {
      txtDescricao.Text = produtoErp.Denominacao;
      txtSmLarg.Text = produtoErp.SobremetalLarg.ToString("#");
      txtSmCompr.Text = produtoErp.SobremetalCompr.ToString("#");
      lblPesoBrut.Content = produtoErp.PesoBruto + " kg";
      lblPesoNbr.Content = produtoErp.PesoPadraoNBR + " kg";
      lblCodigoProduto.Content = produtoErp.CodProduto;

      if (produtoErp.ItemCorte != null && produtoErp.TipoComponente == TipoComponente.Peca || produtoErp.Referencia.StartsWith("Item da lista de corte")) {
        var espess = produtoErp.ItemCorte.Espessura;
        var largur = produtoErp.ItemCorte.Largura + produtoErp.SobremetalLarg;
        var compri = produtoErp.ItemCorte.Comprimento + produtoErp.SobremetalCompr;
        var tipo = produtoErp.ItemCorte.Tipo;
        var codigo = produtoErp.ItemCorte.Codigo;

        lblEspess.Content = tipo == TipoListaMaterial.Chapa ? $"{espess}x{largur}x{compri}" : $"{compri}";
        lblDescMat.Content = produtoErp.ItemCorte.Denominacao;
        lblCodMat.Content = codigo.ToString();

        MostrarMaterialErrado(produtoErp.Pendencias.Contains(PendenciasEngenharia.MateriaErrado));
      }
    }

    private void TxtDenominacao_LostFocus(object sender, RoutedEventArgs e) {
      var produtoErp = Atual;
      if (produtoErp != null)
        produtoErp.Denominacao = txtDescricao.Text;
    }

    private void TxtSmLarg_LostFocus(object sender, RoutedEventArgs e) {
      var produtoErp = Atual;
      if (produtoErp == null)
        return;

      if (!string.IsNullOrEmpty(txtSmLarg.Text)) {
        var sm = Convert.ToDouble(txtSmLarg.Text);
        if (sm > 500) {
          Wpf.Toast.Warning("Sobremetal não pode ser maior que 500!");
          txtSmLarg.Text = string.Empty;
          produtoErp.SobremetalLarg = 0;
          txtSmLarg.Focus();
        } else
          produtoErp.SobremetalLarg = sm;
      } else produtoErp.SobremetalLarg = 0;

      ProdutoErp.CalcularPeso(ref produtoErp);

      AtualizarInformacoes(produtoErp);
    }

    private void TxtSmCompr_LostFocus(object sender, RoutedEventArgs e) {
      var produtoErp = Atual;
      if (produtoErp == null)
        return;

      if (!string.IsNullOrEmpty(txtSmCompr.Text)) {
        var sm = Convert.ToDouble(txtSmCompr.Text);
        if (sm > 500) {
          Wpf.Toast.Warning("Sobremetal não pode ser maior que 500!");
          txtSmCompr.Text = string.Empty;
          produtoErp.SobremetalCompr = 0;
          txtSmCompr.Focus();
        } else
          produtoErp.SobremetalCompr = sm;
      } else produtoErp.SobremetalCompr = 0;

      ProdutoErp.CalcularPeso(ref produtoErp);

      AtualizarInformacoes(produtoErp);
    }

    TreeNode GetNodeByLevelPath(WinTreeView treeView, string path) {
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

    #region Material errado

    private void MostrarMaterialErrado(bool mostrar) {
      ptbMaterialError.Source = imgErro;
      useFirstImage = false;
      ptbMaterialError.Visibility = mostrar ? Visibility.Visible : Visibility.Collapsed;

      if (mostrar)
        tmr.Start();
      else
        tmr.Stop();
    }

    private void Tmr_Tick(object sender, EventArgs e) {
      ptbMaterialError.Source = useFirstImage ? imgErro : imgAlerta;
      useFirstImage = !useFirstImage;
    }

    /// <summary>Escolher a chapa correta para a espessura e gravar na lista de corte.</summary>
    private void PtbMaterialError_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) {
      try {
        var produtoErp = Atual;
        if (produtoErp == null) return;

        var espMat = produtoErp.ItemCorte.Espessura;
        var list = materia_primas.Selecionar(ativo: true, espMat);
        var defatlList = list.ToList().Select(x => new DefaultObject { Codigo = x.CodigoChapa, Descricao = x.DescricaoChapa }).ToList();
        // InputBox com lista da LmCorbieUI (WinForms): o WPF ainda não tem caixa de seleção equivalente.
        var chapaDesc = MsgBox.InputBox("Selecione a chapa correta", lmValueType: LmCorbieUI.Design.LmValueType.ComboBox, itens: defatlList);
        var mat = list.FirstOrDefault(x => x.DescricaoChapa == chapaDesc);

        if (mat != null) {
          var swModel = (ModelDoc2)Sw.App.ActiveDoc;

          if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
            var cutList = produtoErp.ItemCorte;
            cutList.Codigo = mat.CodigoChapa;
            cutList.Denominacao = mat.DescricaoChapa;
            cutList.Material = mat.DescricaoMaterial;
            ListaCorte.UpdateCutList(swModel, cutList);

            AtualizarInfoAlerta(produtoErp);

            AtualizarInformacoes(produtoErp);

            ProdutoErp.CalcularPeso(ref produtoErp);

            lblCodMat.Content = mat.CodigoChapa.ToString();
            lblDescMat.Content = mat.DescricaoChapa;
          }

          MostrarMaterialErrado(false);

          swModel.Save();

          ProdutoErp.RemoverPendencia(produtoErp, PendenciasEngenharia.MateriaErrado);

          Wpf.Toast.Success("Atualizado com sucesso!");
        }
      } catch (Exception ex) {
        Wpf.Toast.Error(ex.Message);
      }
    }

    #endregion

    private void LblCodigoProduto_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) {
      var codigo = lblCodigoProduto.Content as string;
      if (!string.IsNullOrEmpty(codigo)) {
        Clipboard.SetText(codigo);
        Wpf.Toast.Info("Código copiado para área de transferência!");
      }
    }

    #region Cores da grade

    /// <summary>
    /// Linha cadastrada fica com o texto verde (erro = vermelho), normal e selecionada.
    /// Estende o estilo de célula do LmDataGrid; os gatilhos vêm depois do IsSelected, então vencem a cor de seleção.
    /// </summary>
    private void AplicarCorCadastro() {
      var estilo = new Style(typeof(DataGridCell), dgv.CellStyle);

      var sucesso = new DataTrigger { Binding = new Binding(nameof(ProdutoErp.Cadastrado)), Value = true };
      sucesso.Setters.Add(new Setter(Control.ForegroundProperty, corSucesso));
      estilo.Triggers.Add(sucesso);

      var erro = new DataTrigger { Binding = new Binding(nameof(ProdutoErp.Cadastrado)), Value = false };
      erro.Setters.Add(new Setter(Control.ForegroundProperty, corErro));
      estilo.Triggers.Add(erro);

      dgv.CellStyle = estilo;
    }

    /// <summary>Marca o resultado do cadastro (cor da linha) e redesenha a linha (ícone de pendência).</summary>
    private void MarcarCadastro(ProdutoErp produto, bool sucesso) {
      produto.Cadastrado = sucesso;

      if (dgv.ItemContainerGenerator.ContainerFromItem(produto) is DataGridRow linha) {
        var contexto = linha.DataContext;
        linha.DataContext = null;
        linha.DataContext = contexto;
      }
    }

    #endregion
  }
}
