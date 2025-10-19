using Google.Protobuf.WellKnownTypes;
using iTextSharp.text;
using LmCorbieUI;
using LmCorbieUI.Controls;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using LmCorbieUI.Metodos.AtributosCustomizados;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AddinArtama.Api;

namespace AddinArtama {
  internal class ProdutoErp {
    // Cache thread-safe para documentos SolidWorks já abertos
    private static readonly ConcurrentDictionary<string, ModelDoc2> _documentCache = new ConcurrentDictionary<string, ModelDoc2>();
    private static readonly object _cacheLock = new object();

    // Cache thread-safe para consultas ao banco de dados
    private static readonly ConcurrentDictionary<string, ItemGenerico> _materialCache = new ConcurrentDictionary<string, ItemGenerico>();
    private static readonly ConcurrentDictionary<string, List<ItemGenerico>> _productCache = new ConcurrentDictionary<string, List<ItemGenerico>>();

    // Controle de tempo para limpeza automática do cache
    private static DateTime _lastCacheCleanup = DateTime.Now;

    [LarguraColunaGrid(25)]
    [DisplayName(" "), ToolTipGrid("Abrir 3D")]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public Bitmap Img3D { get; set; } = new Bitmap(20, 20);

    [LarguraColunaGrid(25)]
    [DisplayName(" "), ToolTipGrid("Abrir 2D")]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public Bitmap Img2D { get; set; } = new Bitmap(20, 20);

    [LarguraColunaGrid(25)]
    [DisplayName(" "), ToolTipGrid("Item Fantasma")]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public Bitmap ImgFantasma { get; set; } = new Bitmap(20, 20);

    [LarguraColunaGrid(25)]
    [DisplayName(" "), ToolTipGrid("Pendências")]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public Bitmap ImgPendencia { get; set; } = new Bitmap(20, 20);

    //[Browsable(false)]
    [DisplayName("Nível")]
    [LarguraColunaGrid(60)]
    public string Nivel { get; set; }

    [DisplayName("Nome Componente")]
    [LarguraColunaGrid(120)]
    public string Name { get; set; }

    [Browsable(false)]
    [DisplayName("Cód Componente")]
    [LarguraColunaGrid(120)]
    public string CodComponente { get; set; }

    [DisplayName("Cód Produto")]
    [LarguraColunaGrid(120)]
    public string CodProduto { get; set; }

    [DisplayName("Descrição Produto")]
    [LarguraColunaGrid(350)]
    public string Denominacao { get; set; }

    //[Browsable(false)]
    [DisplayName("Referência")]
    [LarguraColunaGrid(150)]
    public string Referencia { get; set; }

    [Browsable(false)]
    [DisplayName("Configuração")]
    [LarguraColunaGrid(150)]
    public string Configuracao { get; set; }

    [Browsable(false)]
    [DisplayName("QTD")]
    [LarguraColunaGrid(50)]
    public int Quantidade { get; set; }

    [Browsable(false)]
    [DisplayName("UM")]
    [LarguraColunaGrid(50)]
    public string UnidadeMedida { get; set; }

    [Browsable(false)]
    public double PesoBruto { get; set; }

    [Browsable(false)]
    public double PesoLiquido { get; set; }

    [Browsable(false)]
    public double PesoPadraoNBR { get; set; }

    [Browsable(false)]
    public string PathName { get; set; }

    [Browsable(false)]
    public TipoComponente TipoComponente { get; set; }

    [Browsable(false)]
    public double SobremetalLarg { get; set; } = 0;

    [Browsable(false)]
    public double SobremetalCompr { get; set; } = 0;

    //[Browsable(false)]
    //public bool CadastrarProdutoErp { get; set; }

    //[Browsable(false)]
    //public bool CadastrarAddin { get; set; }

    [Browsable(false)]
    public bool Fantasma { get; set; }

    [Browsable(false)]
    public bool NaoAlterarNomeERP { get; set; } = false;

    // dados lista de corte
    /// <summary>
    /// Usado para itens que não tem lista de corte, exemplo: Tinta e Cabo de Aço
    /// </summary>
    [Browsable(false)]
    public double Comprimento { get; set; } = 0;

    /// <summary>
    /// 0 - inativo, 1 - ativo
    /// </summary>
    [Browsable(false)]
    public int Situacao { get; set; } = 1;

    [Browsable(false)]
    public ListaCorte ItemCorte = null;

    [Browsable(false)]
    public ItemGenerico ItemGenerico = null;

    [Browsable(false)]
    public Engenharia Engenharia = null;

    // public List<ListaCorte> ItensCorte = new List<ListaCorte>();
    public List<PendenciasEngenharia> Pendencias = new List<PendenciasEngenharia>();
    public List<produto_erp_operacao> Operacoes = new List<produto_erp_operacao>();

    static string mensagemAtencao = string.Empty;

    /// <summary>
    /// Obtém um documento do cache ou abre um novo se não estiver em cache
    /// </summary>
    /// <param name="pathName">Caminho completo do arquivo</param>
    /// <param name="docType">Tipo do documento SolidWorks</param>
    /// <returns>ModelDoc2 do documento ou null se falhar</returns>
    private static ModelDoc2 GetOrOpenDocument(string pathName, int docType) {
      if (string.IsNullOrEmpty(pathName) || !File.Exists(pathName)) {
        return null;
      }

      // Verificar se o documento já está no cache
      if (_documentCache.TryGetValue(pathName, out ModelDoc2 cachedDoc)) {
        // Verificar se o documento ainda é válido
        try {
          var testPath = cachedDoc.GetPathName();
          if (!string.IsNullOrEmpty(testPath)) {
            return cachedDoc;
          }
        } catch {
          // Documento inválido, remover do cache
          _documentCache.TryRemove(pathName, out _);
        }
      }

      // Abrir novo documento
      lock (_cacheLock) {
        // Verificar novamente após obter o lock (double-check pattern)
        if (_documentCache.TryGetValue(pathName, out cachedDoc)) {
          try {
            var testPath = cachedDoc.GetPathName();
            if (!string.IsNullOrEmpty(testPath)) {
              return cachedDoc;
            }
          } catch {
            _documentCache.TryRemove(pathName, out _);
          }
        }

        try {
          int status = 0;
          int warnings = 0;
          var swModel = Sw.App.OpenDoc6(pathName, docType, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

          if (swModel != null) {
            // Adicionar ao cache
            _documentCache.TryAdd(pathName, swModel);
            return swModel;
          }
        } catch (Exception ex) {
          // Log do erro se necessário
          System.Diagnostics.Debug.WriteLine($"Erro ao abrir documento {pathName}: {ex.Message}");
        }
      }

      return null;
    }

    /// <summary>
    /// Limpa o cache de documentos e fecha todos os documentos em cache
    /// </summary>
    public static void ClearDocumentCache() {
      lock (_cacheLock) {
        foreach (var doc in _documentCache.Values) {
          try {
            if (doc != null) {
              Sw.App.CloseDoc(doc.GetTitle());
            }
          } catch {
            // Ignorar erros ao fechar documentos
          }
        }
        _documentCache.Clear();
      }
    }

    // Métodos de cache para consultas ao banco de dados
    private static async Task<ItemGenerico> GetOrFetchMaterialAsync(string codigo) {
      if (string.IsNullOrEmpty(codigo)) return null;

      // Verificar se precisa limpar o cache (a cada 30 minutos)
      CheckAndCleanCache();

      // Verificar se já existe no cache
      if (_materialCache.TryGetValue(codigo, out var cachedMaterial)) {
        return cachedMaterial;
      }

      // Buscar no banco e adicionar ao cache
      try {
        var material = await Api.GetItemGenericoAsync(codigo);
        if (material != null) {
          _materialCache.TryAdd(codigo, material);
        }
        return material;
      } catch {
        return null;
      }
    }

    private static async Task<List<ItemGenerico>> GetOrFetchProductByNameAsync(string name) {
      if (string.IsNullOrEmpty(name)) return new List<ItemGenerico>();

      // Verificar se precisa limpar o cache
      CheckAndCleanCache();

      // Verificar se já existe no cache
      if (_productCache.TryGetValue(name, out var cachedProducts)) {
        return cachedProducts;
      }

      // Buscar no banco e adicionar ao cache
      try {
        var products = await Api.GetItemGenericoByNameAsync(name);
        if (products != null) {
          _productCache.TryAdd(name, products);
        }
        return products ?? new List<ItemGenerico>();
      } catch {
        return new List<ItemGenerico>();
      }
    }

    private static void CheckAndCleanCache() {
      // Limpar cache a cada 30 minutos para evitar dados obsoletos
      if (DateTime.Now.Subtract(_lastCacheCleanup).TotalMinutes > 30) {
        ClearDatabaseCache();
        _lastCacheCleanup = DateTime.Now;
      }
    }

    public static void ClearDatabaseCache() {
      _materialCache.Clear();
      _productCache.Clear();
    }

    /// <summary>
    /// Limpa todos os caches do sistema (documentos e banco de dados)
    /// </summary>
    public static void ClearAllCaches() {
      try {
        var documentCacheCount = _documentCache.Count;
        var materialCacheCount = _materialCache.Count;
        var productCacheCount = _productCache.Count;

        // Limpar cache de documentos
        ClearDocumentCache();

        // Limpar cache de banco de dados
        ClearDatabaseCache();

        // Log para debug
        System.Diagnostics.Debug.WriteLine($"[ProdutoErp] Cache limpo - Documentos: {documentCacheCount}, Materiais: {materialCacheCount}, Produtos: {productCacheCount}");

        // Atualizar timestamp da última limpeza
        _lastCacheCleanup = DateTime.Now;
      } catch (Exception ex) {
        System.Diagnostics.Debug.WriteLine($"[ProdutoErp] Erro ao limpar cache: {ex.Message}");
      }
    }

    public static async Task<SortableBindingList<ProdutoErp>> GetComponentsAsync(TreeView treeView) {
      var _listaProduto = new List<ProdutoErp>();

      Dictionary<string, TreeNode> nodes = new Dictionary<string, TreeNode>();

      try {
        mensagemAtencao = string.Empty;

        using (ContextoDados db = new ContextoDados()) {
          treeView.Nodes.Clear();

          // Inicializar o rootNode
          TreeNode rootNode = null;

          var swModel = (ModelDoc2)Sw.App.ActiveDoc;
          var tipo = swModel.GetType();

          await PegarDadosComponenteAsync(db, swModel, qtd: 1, nivel: "1", _listaProduto, rootNode, nodes, treeView);

          // Pegar o rootNode do dictionary nodes
          if (nodes.ContainsKey("1")) {
            rootNode = nodes["1"];
          }

          if (tipo == (int)swDocumentTypes_e.swDocASSEMBLY) {
            var swModelDocExt = swModel.Extension;
            var swConfMgr = swModel.ConfigurationManager;
            var swConf = swConfMgr.ActiveConfiguration;
            CustomPropertyManager swCustPropMngr = swModelDocExt.get_CustomPropertyManager(swConf.Name);

            // Inserir lista de material e pegar dados
            string templateGeral = $"{Application.StartupPath}\\01 - Addin LM\\ListaCompleta.sldbomtbt";
            int BomTypeGeral = (int)swBomType_e.swBomType_Indented;
            int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
            bool DetailedCutList = false;
            var swBOMAnnotationGeral = swModelDocExt.InsertBomTable3(templateGeral, 0, 1, BomTypeGeral, swConf.Name, false, NumberingType, DetailedCutList);
            await PegaDadosListaGeralAsync(db, swBOMAnnotationGeral, rootNode, _listaProduto, nodes, treeView);
            ListaCorte.ExcluirLista(swModel);
          }

          rootNode.ExpandAll();
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar componentes.\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        ClearDocumentCache();
        MsgBox.CloseWaitMessage();
      }

      if (!string.IsNullOrEmpty(mensagemAtencao)) {
        MsgBox.CloseWaitMessage();
        mensagemAtencao = "Analisar Itens Abaixo!\r\n\r\n" + mensagemAtencao;
        MsgBox.InputBox(titulo: "Atenção", textoImputPadrao: mensagemAtencao, textoLongo: true, CentralizarForm: true, somenteLeitura: true);
      }

      return new SortableBindingList<ProdutoErp>(_listaProduto);
    }

    private static async Task PegaDadosListaGeralAsync(ContextoDados db, BomTableAnnotation swBOMAnnotation,
      TreeNode rootNode, List<ProdutoErp> _listaProduto, Dictionary<string, TreeNode> nodes, TreeView treeView) {
      string nameShort = "";
      try {
        string[] vModelPathNames = null;
        string strItemNumber = null;
        string strPartNumber = null;
        var swTableAnnotation = (TableAnnotation)swBOMAnnotation;

        int lStartRow = 1;

        if (!(swTableAnnotation.TitleVisible == false)) {
          lStartRow = 2;
        }

        var swBOMFeature = swBOMAnnotation.BomFeature;

        string nivelPecaLido = string.Empty;
        await Loader.ShowDuringOperation(
            "Iniciando leitura da tabela...",
            async (progress2) => {
              var total = swTableAnnotation.TotalRowCount;
              for (int i = lStartRow; i < swTableAnnotation.TotalRowCount; i++) {
                if (!Loader._isWorking) {
                  rootNode.Nodes.Clear();
                  mensagemAtencao = string.Empty;
                  MsgBox.Show("Operação cancelada pelo usuário.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  return Task.FromResult("Cancelado");
                }

                vModelPathNames = (string[])swBOMAnnotation.GetModelPathNames(i, out strItemNumber, out strPartNumber);

                if (vModelPathNames != null) {
                  string ptNm = vModelPathNames[0];
                  nameShort = Path.GetFileNameWithoutExtension(ptNm);

                  progress2.Report(($"Lendo componentes... \n\n{nameShort}", i, total));

                  var nivel = swTableAnnotation.get_Text(i, 0).Trim();
                  var qtd = Convert.ToInt32(swTableAnnotation.get_Text(i, 1));
                  var codMaterial = swTableAnnotation.get_Text(i, 2).Trim();
                  var configName = swTableAnnotation.get_Text(i, 11).Trim();

                  if (!string.IsNullOrEmpty(codMaterial) || nameShort.Contains("~"))
                    continue;

                  nivel = "1." + nivel;

                  int swTipo = ptNm.ToUpper().EndsWith("SLDPRT") ? (int)swDocumentTypes_e.swDocPART : (int)swDocumentTypes_e.swDocASSEMBLY;

                  var swModel = GetOrOpenDocument(ptNm, swTipo);

                  if (swModel != null) {
                    // verificar se pai é item de biblioteca
                    var parentLevel = nivel.Substring(0, nivel.LastIndexOf('.'));
                    if (nodes.ContainsKey(parentLevel) &&
                        ((ProdutoErp)nodes[parentLevel].Tag).TipoComponente != TipoComponente.ItemBiblioteca) {
                      swModel.ShowConfiguration2(configName);

                      await PegarDadosComponenteAsync(db, swModel, qtd, nivel, _listaProduto, rootNode, nodes, treeView);
                    }
                  }
                  //}
                }
              }
              return Task.FromResult("concluído");
            },
            100
        );
      } catch (Exception ex) {
        var msg = "Erro ao ler dados da Lista de Montagem";
        msg = ex.Message.Contains(msg) ? ex.Message : $"{msg}\n\nItem: {nameShort}\n\n{ex.Message}";
        MsgBox.Show(msg, "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #region Analizar Componentes e montar Lista de produtos

    private static async Task PegarDadosComponenteAsync(ContextoDados db, ModelDoc2 swModel, int qtd, string nivel, List<ProdutoErp> _listaProduto,
      TreeNode rootNode, Dictionary<string, TreeNode> nodes, TreeView treeView) {
      if (swModel == null) {
        return;
      }

      var pathName = swModel.GetPathName();
      var name = Path.GetFileNameWithoutExtension(pathName);
      string resolvedValOut = string.Empty;

      try {
        ModelDocExtension swModelDocExt = swModel.Extension;
        ConfigurationManager swConfMgr = swModel.ConfigurationManager;
        Configuration swConf = swConfMgr.ActiveConfiguration;

        var tipo = swModel.GetType();

        ProdutoErp produtoErp = new ProdutoErp {
          Nivel = nivel,
          PathName = pathName,
          Name = name,
          Referencia = name,
          Quantidade = qtd,
          Configuracao = swConf.Name,
          TipoComponente = tipo == (int)swDocumentTypes_e.swDocASSEMBLY ? TipoComponente.Montagem : TipoComponente.Peca,
        };

        if (swModel.IsOpenedReadOnly()) {
          mensagemAtencao += $"{produtoErp.Name} - {produtoErp.Denominacao}\r\n- Aberto como Somente Leitura.\r\n\r\n";
        }

        var desenhoExiste = File.Exists(pathName.ToUpper().Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW");
        produtoErp.Img3D = produtoErp.TipoComponente == TipoComponente.Peca ? Properties.Resources.part : Properties.Resources.assembly;
        produtoErp.Img2D = desenhoExiste ? Properties.Resources.draw : Properties.Resources.not_draw;

        CustomPropertyManager swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");

        swCustPropMngr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        swCustPropMngr = swModelDocExt.get_CustomPropertyManager(swConf.Name);
        swCustPropMngr.Get2("Componente", out _, out resolvedValOut);
        produtoErp.CodComponente = resolvedValOut;
        swCustPropMngr.Get2("Denominação", out _, out resolvedValOut);
        produtoErp.Denominacao = resolvedValOut;

        swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");
        if (string.IsNullOrEmpty(produtoErp.CodComponente)) {
          swCustPropMngr.Get2("Componente", out _, out resolvedValOut);
          produtoErp.CodComponente = resolvedValOut;
        }

        if (string.IsNullOrEmpty(produtoErp.Denominacao)) {
          swCustPropMngr.Get2("Denominação", out _, out resolvedValOut);
          produtoErp.Denominacao = resolvedValOut;
        }

        //if (swConf.UseAlternateNameInBOM == true) {
        //  if (!string.IsNullOrEmpty(swConf.AlternateName))
        //    produtoErp.CodComponente = swConf.AlternateName;
        //  else
        //    produtoErp.CodComponente = swConf.Name;
        //}

        if (pathName.ToUpper().Contains("BIBLIOTECA") ||
          produtoErp.CodComponente.StartsWith("10") ||
          produtoErp.CodComponente.StartsWith("20") ||
          produtoErp.CodComponente.StartsWith("30"))
          produtoErp.TipoComponente = TipoComponente.ItemBiblioteca;

        if ((produtoErp.CodComponente.StartsWith("30") || produtoErp.CodComponente.StartsWith("40"))) {
          var eng = await Api.GetEngenhariaAsync(produtoErp.CodComponente);
          if (eng != null && eng.statusEngenharia != StatusEngenharia.EmDesenvolvimento) {
            if (nivel == "1") {
              throw new Exception($"A Engenharia {produtoErp.CodComponente} está com status '{eng.statusEngenharia.ObterDescricaoEnum()}', só pode ser importada se estiver 'Em Desenvolvimento'.");
            } else {
              produtoErp.TipoComponente = tipo == (int)swDocumentTypes_e.swDocASSEMBLY ? TipoComponente.Montagem : TipoComponente.Peca;
              produtoErp.CodProduto = produtoErp.CodComponente;
            }
          } else {
            produtoErp.CodProduto = produtoErp.CodComponente;
            produtoErp.NaoAlterarNomeERP = true;
            produtoErp.TipoComponente = tipo == (int)swDocumentTypes_e.swDocASSEMBLY ? TipoComponente.Montagem : TipoComponente.Peca;
          }
          if (eng != null)
            produtoErp.Engenharia = eng;
        }

        // Prioriza massa calculada diretamente do componente
        var massaDireta = GetMassFromComponent(swModel);
        if (massaDireta > 0) {
          produtoErp.PesoLiquido = massaDireta;
          produtoErp.PesoBruto = massaDireta;
        } else {
          // Fallback: usa cálculo direto mesmo quando massaDireta <= 0
          var massaDiretaFallback = GetMassFromComponent(swModel);
          produtoErp.PesoLiquido = Math.Round(massaDiretaFallback, 4);
          produtoErp.PesoBruto = produtoErp.PesoLiquido;
        }
        swCustPropMngr.Get2("Fantasma", out _, out resolvedValOut);
        produtoErp.Fantasma = !string.IsNullOrEmpty(resolvedValOut) && resolvedValOut.ToLower() == "sim";

        swCustPropMngr = swModelDocExt.get_CustomPropertyManager(swConf.Name);
        swCustPropMngr.Get2("COMPRIMENTO", out _, out string comprim);
        double.TryParse(comprim.Replace(".", ","), out double comprimento);
        produtoErp.Comprimento = comprimento;

        if (produtoErp.Fantasma)
          produtoErp.ImgFantasma = Properties.Resources.fantasma;

        if (produtoErp.CodComponente.Replace(" ", "") == "-" && !mensagemAtencao.Contains(produtoErp.Name)) {
          mensagemAtencao += $"{produtoErp.Name} - {produtoErp.Denominacao}\r\n- Código componente = '-'.\r\n\r\n";
        } else if ((produtoErp.TipoComponente == TipoComponente.Peca || produtoErp.TipoComponente == TipoComponente.Montagem) && !produtoErp.Name.StartsWith(produtoErp.CodComponente)) {
          mensagemAtencao += $"{produtoErp.Name} - {produtoErp.Denominacao}\r\n- Código componente '{produtoErp.CodComponente}' parece estar incorreto.\r\n\r\n";
        }

        // pegar dados da lista de corte
        var itensCorte = new List<ListaCorte>();

        if (produtoErp.TipoComponente == TipoComponente.Peca) {

          itensCorte = ListaCorte.GetCutList(swModel, produtoErp.PathName);

          if (itensCorte.Count == 1) {
            produtoErp.ItemCorte = itensCorte[0];
          }

          if (itensCorte.Count > 1 || (itensCorte.Count == 1 && itensCorte[0].Quantidade > 1))
            produtoErp.UnidadeMedida = "CJ";
        }

        // analisarProduto no ERP
        produtoErp = await AnalisarProdutoERP(db, swModel, swModelDocExt, produtoErp, _listaProduto);

        if (
          (produtoErp.TipoComponente == TipoComponente.ItemBiblioteca || produtoErp.CodComponente.StartsWith("40")) &&
          (!long.TryParse(produtoErp.CodProduto, out _) || produtoErp.CodComponente != produtoErp.CodProduto)) {
          produtoErp.CodProduto = produtoErp.CodComponente;
        }

        if (!_listaProduto.Any(x => x.Name == produtoErp.Name && x.Referencia == produtoErp.Referencia && x.Configuracao == produtoErp.Configuracao) &&
          produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
          _listaProduto.Add(produtoErp);
        }

        // engenharia de produto
        bool verificaMaterial = produtoErp.TipoComponente == TipoComponente.Peca && itensCorte.Count > 0 && itensCorte[0].Quantidade == 1;
        CreateTreeCompNode(rootNode, produtoErp, nivel, verificaMaterial, nodes, _listaProduto, treeView);

        if (produtoErp.TipoComponente == TipoComponente.Peca && itensCorte.Count > 1 ||
          (produtoErp.TipoComponente == TipoComponente.Peca && itensCorte.Count == 1 && itensCorte[0].Quantidade > 1)) {
          produtoErp.TipoComponente = TipoComponente.Montagem;
          produtoErp.Img3D = Properties.Resources.assembly;

          for (int indiceLista = 0; indiceLista < itensCorte.Count; indiceLista++) {
            ListaCorte itemCorte = itensCorte[indiceLista];
            var indiceNome = indiceLista + 1;
            var sufixo = $" - {indiceNome}";

            var item = new ProdutoErp {
              PathName = produtoErp.PathName,
              Name = produtoErp.Name + sufixo,
              Denominacao = produtoErp.Denominacao,
              Referencia = itemCorte.NomeLista,
              CodComponente = produtoErp.CodComponente,
              Img3D = Properties.Resources.part,
              Img2D = produtoErp.Img2D,
              TipoComponente = TipoComponente.Peca,
              Nivel = produtoErp.Nivel + "." + indiceNome,
              Configuracao = produtoErp.Configuracao,
              Quantidade = itemCorte.Quantidade,
              PesoLiquido = itemCorte.Massa,
              PesoBruto = itemCorte.Massa,
              ItemCorte = itemCorte
            };

            // analisarProduto no ERP
            item = await AnalisarProdutoERP(db, swModel, swModelDocExt, item, _listaProduto);

            if (item.Name.Length + sufixo.Length > 50)
              item.Name = item.Name.Substring(0, 50 - sufixo.Length) + sufixo;

            if (!_listaProduto.Any(x => x.Name == item.Name && x.Referencia == item.Referencia && x.Configuracao == item.Configuracao) &&
                item.TipoComponente != TipoComponente.ItemBiblioteca) {
              _listaProduto.Add(item);
            }

            CreateTreeCompNode(rootNode, item, nivel + "." + indiceNome, verificaMaterial: true, nodes, _listaProduto, treeView);
          }
        }
      } catch (Exception ex) {
        throw new Exception($"Erro ao pegar dados do componente\n\nItem: {name}\n\n{ex.Message}");
      }
    }

    private static async Task<ProdutoErp> AnalisarProdutoERP(ContextoDados db, ModelDoc2 swModel, ModelDocExtension swModelDocExt, ProdutoErp produtoErp, List<ProdutoErp> _listaProduto) {
      // =================== verificar Materia prima
      var swCustPropMgr = swModelDocExt.get_CustomPropertyManager(produtoErp.Configuracao);

      if (produtoErp.TipoComponente == TipoComponente.Peca && produtoErp.ItemCorte != null) {


        // pegar unidade de medida
        produtoErp.UnidadeMedida = "PC";
        var material = await GetOrFetchMaterialAsync(produtoErp.ItemCorte?.Codigo.ToString());
        if (material != null) {
          produtoErp.ItemCorte.unidadeMedida = material.unidadeMedida;
          produtoErp.PesoLiquido = produtoErp.ItemCorte.Massa;
          produtoErp.PesoPadraoNBR = material.pesoPadraoNBR;
          // analisar material
          if (produtoErp.ItemCorte.Tipo == TipoListaMaterial.Chapa && produtoErp.ItemCorte.Espessura > 0 && produtoErp.ItemCorte.unidadeMedida != "PC") {
            var chapas = materia_primas.Selecionar(ativo: true, produtoErp.ItemCorte.Espessura);
            if (!chapas.Any(x => x.CodigoChapa == produtoErp.ItemCorte.Codigo))
              AdicionarPendencia(produtoErp, PendenciasEngenharia.MateriaErrado);
          }

          // calcular peso
          CalcularPeso(ref produtoErp);

          // verificar situação do material
          if (material.situacao == 0) {
            mensagemAtencao += $"{material.codigo} - {material.nome}\r\n- Material Inativo.\r\n\r\n";
          }
        } else {
          AdicionarPendencia(produtoErp, PendenciasEngenharia.MateriaPrimaInexistente);
        }
      } else if (produtoErp.TipoComponente == TipoComponente.Montagem) {
        produtoErp.UnidadeMedida = "CJ";
      } else if (produtoErp.Comprimento > 0 && produtoErp.PathName.ToUpper().EndsWith("SLDPRT")) {
        var material = await GetOrFetchMaterialAsync(produtoErp.CodComponente);
        produtoErp.UnidadeMedida = material.unidadeMedida;
      }

      // =================== verificar se item existe no ERP
      if (produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
        var itens = !produtoErp.CodComponente.StartsWith("40")
          ? await GetOrFetchProductByNameAsync(produtoErp.Name)
          : new List<ItemGenerico> { await GetOrFetchMaterialAsync(produtoErp.CodComponente) };

        if (!produtoErp.CodComponente.StartsWith("40"))
          itens = itens.Where(x => x.nome.EndsWith(produtoErp.Name) && x.situacao == 1 && x.codigo.StartsWith("3")).ToList();
        else { // verificar componente de revenda
          if (itens[0] == null)
            throw new Exception($"O componente {produtoErp.Name} com código de revenda {produtoErp.CodComponente} não foi encontrado no ERP.\r\n" +
              $"Favor cadastrar o item no ERP ou Corrigir o código no projeto.");

          var nomeNaRef = produtoErp.Name.ToLower().Contains(itens[0].refTecnica.ToLower());
          var nomeNaDes = itens[0].nome.ToLower().Contains(produtoErp.Name.ToLower());

          if (!nomeNaRef && !nomeNaDes) {
            throw new Exception($"O componente {produtoErp.Name} não corresponde ao código de revenda {produtoErp.CodComponente}.\r\n" +
               $"Favor corrigir o nome do componente ou o código de revenda.");
          }
        }

        var itensValidos = itens.Where(x => !string.IsNullOrEmpty(x.mascaraSaida)).ToList();
        if (itensValidos.Count == 1) { // produto encontrado no erp
          if (itens.Count > 1) {
            var itensCancelar = itens.Where(x => string.IsNullOrEmpty(x.mascaraSaida));
            foreach (var itemCancelar in itensCancelar) {
              var prodCancelar = new ProdutoErp {
                CodProduto = itemCancelar.codigo,
                CodComponente = itemCancelar.codigo,
                Denominacao = produtoErp.Denominacao,
                Name = produtoErp.Name,
                TipoComponente = produtoErp.TipoComponente,
                PesoBruto = itemCancelar.pesoBruto,
                PesoLiquido = itemCancelar.pesoLiquido,
                UnidadeMedida = itemCancelar.unidadeMedida,
                Situacao = 0,
              };

              await Api.UpdateItemGenericoAsync(prodCancelar);
            }
          }

          var itemValido = itensValidos.First();

          produtoErp.CodProduto = itemValido.codigo.ToString();
          produtoErp.ItemGenerico = itemValido;

          swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
          swModel.Save();

          // verificar engenharia
          var engenharia = await Api.GetEngenhariaAsync(produtoErp.CodProduto);
          if (engenharia != null) {
            produtoErp.Engenharia = engenharia;

            PegarOperacaoERP(produtoErp, engenharia.operacoes);

            if (!produtoErp.Referencia.StartsWith("Item da lista de corte")) {
              var pesoItem = Math.Round(itens[0].pesoBruto, 2);
              var pesoProd = Math.Round(produtoErp.PesoBruto, 2);
              var dif = Math.Round(Math.Abs(pesoItem - pesoProd), 2);
              if (dif > 0.01 && !produtoErp.Pendencias.Contains(PendenciasEngenharia.MateriaErrado)) {
                AdicionarPendencia(produtoErp, PendenciasEngenharia.ItemAlterado);
              }
            }
          }
        } else if (itens.Count > 1) { // multiplos produtos encontrados no erp
          produtoErp.CodProduto = string.Empty;

          var t = new Thread(async () => {
            using (var frm = new FrmEscolherCodigoErp(itens, produtoErp.Name)) {
              frm.ShowDialog();

              if (string.IsNullOrEmpty(frm._codigoErp)) {
                throw new Exception($"Foram encontrados mais de um código para este item {produtoErp.Name}.\r\n" +
                  $"E não foi selecionado uma ação para tratar a mesma");
              }

              var itemValido = itens.First(x => x.codigo == frm._codigoErp);

              produtoErp.CodProduto = itemValido.codigo.ToString();
              produtoErp.ItemGenerico = itemValido;

              swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
              swModel.Save();

              var itensCancelar = itens.Where(x => x.codigo != frm._codigoErp);
              foreach (var itemCancelar in itensCancelar) {
                var prodCancelar = new ProdutoErp {
                  CodProduto = itemCancelar.codigo,
                  CodComponente = itemCancelar.codigo,
                  Denominacao = produtoErp.Denominacao,
                  Name = produtoErp.Name,
                  TipoComponente = produtoErp.TipoComponente,
                  PesoBruto = itemCancelar.pesoBruto,
                  PesoLiquido = itemCancelar.pesoLiquido,
                  UnidadeMedida = itemCancelar.unidadeMedida,
                  Situacao = 0,
                };

                await Api.UpdateItemGenericoAsync(prodCancelar);
              }
            }
          });

          // O segredo está aqui:
          t.SetApartmentState(ApartmentState.STA);

          t.Start();

          // Aguarda o form ser fechado
          t.Join();
        }
      }
      return produtoErp;
    }

    private static void CreateTreeCompNode(TreeNode rootNode, ProdutoErp produtoErp,
      string nivel, bool verificaMaterial, Dictionary<string, TreeNode> nodes, List<ProdutoErp> _listaProduto, TreeView treeView) {
      string nodeText = $"{produtoErp.Name} - {produtoErp.Denominacao}";
      var node = new TreeNode(nodeText);

      node.Tag = produtoErp;

      var iconIndex = produtoErp.TipoComponente == TipoComponente.ItemBiblioteca
        ? 5
        : produtoErp.TipoComponente == TipoComponente.Montagem
        ? 0
        : 1;

      node.ImageIndex = iconIndex;
      node.SelectedImageIndex = iconIndex;

      if (verificaMaterial && produtoErp.TipoComponente == TipoComponente.Peca && produtoErp.ItemCorte != null) {
        var produtoFilho = new ProdutoErp {
          PathName = produtoErp.PathName,
          Name = produtoErp.Name,
          Denominacao = produtoErp.ItemCorte.Denominacao,
          Referencia = produtoErp.Referencia,
          CodComponente = produtoErp.ItemCorte.Codigo.ToString(),
          CodProduto = produtoErp.ItemCorte.Codigo.ToString(),
          TipoComponente = TipoComponente.ListaMaterial,
          Nivel = produtoErp.Nivel + ".1",
          Configuracao = produtoErp.Configuracao,
          Quantidade = produtoErp.ItemCorte.Quantidade,
          ItemCorte = produtoErp.ItemCorte,
        };

        int imgIndex = produtoErp.ItemCorte.Tipo == TipoListaMaterial.Chapa ? 3 : produtoErp.ItemCorte.Tipo == TipoListaMaterial.NaoPossui ? 6 : 4;

        string nodeTextFilho = $"{produtoFilho.CodComponente} - {produtoFilho.Denominacao}";
        var nodeFilho = new TreeNode(nodeTextFilho);
        nodeFilho.Tag = produtoFilho;
        nodeFilho.ImageIndex = imgIndex;
        nodeFilho.SelectedImageIndex = imgIndex;

        node.Nodes.Add(nodeFilho);

        if (!_listaProduto.Any(x => x.Name == produtoFilho.Name && x.Referencia == produtoFilho.Referencia && x.Configuracao == produtoFilho.Configuracao) &&
          produtoFilho.TipoComponente != TipoComponente.ItemBiblioteca) {
          _listaProduto.Add(produtoFilho);
        }
      }

      // Adicionar o nó à estrutura hierárquica
      if (nivel.Contains('.')) {
        var parentLevel = nivel.Substring(0, nivel.LastIndexOf('.'));
        if (nodes.ContainsKey(parentLevel) &&
            ((ProdutoErp)nodes[parentLevel].Tag).TipoComponente != TipoComponente.ItemBiblioteca) {
          nodes[nivel] = node;
          nodes[parentLevel].Nodes.Add(node);
        }
      } else {
        nodes[nivel] = node;
        if (treeView != null) {
          treeView.Nodes.Add(node);
        }
      }
    }

    private static void PegarOperacaoERP(ProdutoErp produtoErp, List<OperacaoEng> engenhariaOperacoes) {
      if (engenhariaOperacoes != null && engenhariaOperacoes.Count > 0) {
        // verificar alteração de operações no erp ou solid
        foreach (var operacao in engenhariaOperacoes) {
          var op = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == operacao.codOperacao && x.mascaraMaquina == operacao.codMascaraMaquina);

          if (op != null) {
            AtualizarOperacao(produtoErp, operacao, op.codAxion);
          }
        }
      } else {
        //AdicionarPendencia(produtoErp, PendenciasEngenharia.OperacaoNaoPossui);
      }
    }

    private static void AtualizarOperacao(ProdutoErp produtoErp, OperacaoEng operacao, int codAxion) {
      produtoErp.Operacoes.Add(new produto_erp_operacao {
        sequencia = operacao.seqOperacao,
        processo_id = codAxion,
        name = produtoErp.Name,
        referencia = produtoErp.Referencia,
        tempo = operacao.tempoPadraoOperacao.FormatarHora(),
        qtd_operador = Convert.ToInt32(operacao.numOperadores),
      });

      //RemoverPendencia(produtoErp, PendenciasEngenharia.OperacaoNaoPossui);
    }

    internal static void CalcularPeso(ref ProdutoErp produtoErp) {
      if (produtoErp.PesoPadraoNBR == 0)
        return;

      if (produtoErp.ItemCorte.Tipo == TipoListaMaterial.Chapa) {
        produtoErp.PesoBruto = ((produtoErp.ItemCorte.Largura + produtoErp.SobremetalLarg) / 1000) *
          ((produtoErp.ItemCorte.Comprimento + produtoErp.SobremetalCompr) / 1000) * produtoErp.PesoPadraoNBR;
      } else {
        produtoErp.PesoBruto = ((produtoErp.ItemCorte.Comprimento + produtoErp.SobremetalCompr) / 1000) * produtoErp.PesoPadraoNBR;
      }
      produtoErp.PesoBruto = Math.Round(produtoErp.PesoBruto, 4);
      if (produtoErp.PesoLiquido > produtoErp.PesoBruto)
        produtoErp.PesoLiquido = produtoErp.PesoBruto;
    }


    /// <summary>
    /// Obtém a massa diretamente do componente SolidWorks usando GetMassProperties.
    /// Esta abordagem é mais confiável que ler a propriedade customizada "Massa",
    /// pois evita erros de unidade/resolução de propriedade.
    /// </summary>
    /// <param name="swModel">Documento do SolidWorks (peça ou montagem)</param>
    /// <returns>Massa em kg quando disponível; caso contrário, 0.</returns>
    public static double GetMassFromComponent(ModelDoc2 swModel) {
      try {
        if (swModel == null) return 0;
        var swModelDocExt = swModel.Extension;
        // O primeiro parâmetro (1) usa o sistema de unidades do documento;
        // massProp[5] é a massa total conforme API do SW.
        var massPropObj = swModelDocExt.GetMassProperties(1, 0);
        if (massPropObj is double[] massProp && massProp.Length >= 6) {
          return Math.Round(massProp[5], 4);
        }
      } catch {
        // Ignora exceções e retorna 0 como fallback
      }
      return 0;
    }
    internal static void AdicionarPendencia(ProdutoErp produtoErp, PendenciasEngenharia pendencia) {
      if (!produtoErp.Pendencias.Contains(pendencia)) {
        produtoErp.Pendencias.Add(pendencia);
        produtoErp.ImgPendencia = pendencia.EhPendenciaCritica() || produtoErp.Pendencias.Any(x => x.EhPendenciaCritica()) ? Properties.Resources.error : Properties.Resources.warning;
      }
    }

    internal static void RemoverPendencia(ProdutoErp produtoErp, PendenciasEngenharia pendencia) {
      produtoErp.Pendencias.RemoveAll(x => x == pendencia);

      if (produtoErp.Pendencias.Count == 0)
        produtoErp.ImgPendencia = new Bitmap(20, 20);
      else if (produtoErp.Pendencias.Any(x => x.EhPendenciaCritica()))
        produtoErp.ImgPendencia = Properties.Resources.error;
      else
        produtoErp.ImgPendencia = Properties.Resources.warning;
    }

    /// <summary>
    /// Busca recursivamente um nó que corresponda ao produto
    /// </summary>
    private static TreeNode SearchNodeRecursive(TreeNode node, ProdutoErp product) {
      if (node.Tag is ProdutoErp nodeProduct &&
          nodeProduct.PathName == product.PathName &&
          nodeProduct.Configuracao == product.Configuracao) {
        return node;
      }

      foreach (TreeNode childNode in node.Nodes) {
        var foundNode = SearchNodeRecursive(childNode, product);
        if (foundNode != null) return foundNode;
      }

      return null;
    }

    /// <summary>
    /// Clona um produto com novos dados de quantidade e nível
    /// </summary>
    private static ProdutoErp CloneProduct(ProdutoErp original, int qtd, string nivel) {
      return new ProdutoErp {
        PathName = original.PathName,
        Name = original.Name,
        Referencia = original.Referencia,
        Denominacao = original.Denominacao,
        CodComponente = original.CodComponente,
        CodProduto = original.CodProduto,
        TipoComponente = original.TipoComponente,
        Configuracao = original.Configuracao,
        Quantidade = qtd,
        Nivel = nivel,
        PesoBruto = original.PesoBruto,
        PesoLiquido = original.PesoLiquido,
        UnidadeMedida = original.UnidadeMedida,
        SobremetalCompr = original.SobremetalCompr,
        SobremetalLarg = original.SobremetalLarg,
        ItemCorte = original.ItemCorte,
        ItemGenerico = original.ItemGenerico,
        Img3D = original.Img3D,
        Img2D = original.Img2D,
        ImgPendencia = original.ImgPendencia,
        Operacoes = new List<produto_erp_operacao>(original.Operacoes),
        Pendencias = new List<PendenciasEngenharia>(original.Pendencias)
      };
    }

    /// <summary>
    /// Clona um nó da TreeView recursivamente
    /// </summary>
    private static TreeNode CloneTreeNode(TreeNode originalNode, ProdutoErp clonedProduct, string nivel, List<ProdutoErp> _listaProduto) {
      string nodeText = $"{clonedProduct.Name} - {clonedProduct.Denominacao}";
      var clonedNode = new TreeNode(nodeText) {
        Tag = clonedProduct,
        ImageIndex = originalNode.ImageIndex,
        SelectedImageIndex = originalNode.SelectedImageIndex
      };

      // Clonar nós filhos recursivamente
      foreach (TreeNode childNode in originalNode.Nodes) {
        if (childNode.Tag is ProdutoErp childProduct) {
          // Calcular novo nível para o filho
          string childLevel = nivel + "." + childProduct.Nivel.Split('.').Last();

          // Clonar o produto filho
          var clonedChildProduct = CloneProduct(childProduct, childProduct.Quantidade, childLevel);

          // Adicionar à lista se não existir
          if (!_listaProduto.Any(x => x.Name == clonedChildProduct.Name && x.Referencia == clonedChildProduct.Referencia &&
                                      x.Configuracao == clonedChildProduct.Configuracao && x.Nivel == clonedChildProduct.Nivel)) {
            _listaProduto.Add(clonedChildProduct);
          }

          // Clonar o nó filho recursivamente
          TreeNode clonedChildNode = CloneTreeNode(childNode, clonedChildProduct, childLevel, _listaProduto);
          clonedNode.Nodes.Add(clonedChildNode);
        }
      }

      return clonedNode;
    }

    #endregion

  }
}
