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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AddinArtama.Api;

namespace AddinArtama {
  internal class ProdutoErp {

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
    public double Quantidade { get; set; }

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

    [Browsable(false)]
    public bool CadastrarProdutoErp { get; set; }

    [Browsable(false)]
    public bool CadastrarAddin { get; set; }

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

    [Browsable(false)]
    public ListaCorte ItemCorte = null;

    // public List<ListaCorte> ItensCorte = new List<ListaCorte>();
    public List<PendenciasEngenharia> Pendencias = new List<PendenciasEngenharia>();
    public List<produto_erp_operacao> Operacoes = new List<produto_erp_operacao>();

    static string mensagemAtencao = string.Empty;

    public static async Task<SortableBindingList<ProdutoErp>> GetComponentsAsync(TreeView treeView, string montageGeralNome) {
      var _listaProduto = new List<ProdutoErp>();

      try {
        mensagemAtencao = string.Empty;

        using (ContextoDados db = new ContextoDados()) {
          treeView.Nodes.Clear();

          var swModel = (ModelDoc2)Sw.App.ActiveDoc;
          ModelDocExtension swModelDocExt;
          CustomPropertyManager swCustPropMngr = default(CustomPropertyManager);

          var pathName = swModel.GetPathName();
          var name = Path.GetFileNameWithoutExtension(pathName);

          ConfigurationManager swConfMgr;
          Configuration swConf;
          //Component2 swRootComp;

          swModelDocExt = swModel.Extension;
          swConfMgr = swModel.ConfigurationManager;
          swConf = swConfMgr.ActiveConfiguration;
          //swRootComp = swConf.GetRootComponent3(true);

          var tipo = swModel.GetType();

          ProdutoErp produtoErp = new ProdutoErp {
            Nivel = "1",
            PathName = pathName,
            Name = name,
            Referencia = name,
            TipoComponente = pathName.ToUpper().EndsWith("SLDASM") ? TipoComponente.Montagem : TipoComponente.Peca,
          };

          if (produtoErp.Name.Length > 50)
            produtoErp.Name = produtoErp.Name.Substring(0, 50);

          //if (produtoErp.Name.Length > 50 && !produtoErp.Name.StartsWith("4")) {
          //  Toast.Warning($"O nome do componente '{produtoErp.Name}' é muito longo, altere o nome para prosseguir.");
          //  return new SortableBindingList<ProdutoErp>();
          //}

          var desenhoExiste = File.Exists(pathName.ToUpper().Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW");
          produtoErp.Img3D = produtoErp.TipoComponente == TipoComponente.Peca ? Properties.Resources.part : Properties.Resources.assembly;
          produtoErp.Img2D = desenhoExiste ? Properties.Resources.draw : Properties.Resources.not_draw;

          string valOut;
          string resolvedValOut;

          swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");

          swCustPropMngr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

          swCustPropMngr.Get2("Código Produto", out valOut, out resolvedValOut);
          produtoErp.CodProduto = resolvedValOut;
          swCustPropMngr.Get2("Denominação", out valOut, out resolvedValOut);
          produtoErp.Denominacao = resolvedValOut;
          swCustPropMngr.Get2("Componente", out valOut, out resolvedValOut);
          produtoErp.CodComponente = resolvedValOut;
          swCustPropMngr.Get2("Massa", out valOut, out resolvedValOut);
          double.TryParse(resolvedValOut, NumberStyles.Any, CultureInfo.InvariantCulture, out double massa);
          produtoErp.PesoLiquido = Math.Round(massa, 4);
          produtoErp.PesoBruto = produtoErp.PesoLiquido;
          swCustPropMngr.Get2("Fantasma", out valOut, out resolvedValOut);
          produtoErp.Fantasma = !string.IsNullOrEmpty(resolvedValOut) && resolvedValOut.ToLower() == "sim";

          if (produtoErp.Fantasma)
            produtoErp.ImgFantasma = Properties.Resources.fantasma;

          if (pathName.ToUpper().Contains("BIBLIOTECA") ||
            produtoErp.CodComponente.StartsWith("10") ||
            produtoErp.CodComponente.StartsWith("20")) {
            MsgBox.Show($"Recurso indisponivel para o componentes de biblioteca.",
              "Ação não Permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //Toast.Warning("Recurso indisponivel para o componentes de biblioteca.");
            return new SortableBindingList<ProdutoErp>();
          }

          if (produtoErp.CodComponente.StartsWith("3") || produtoErp.CodComponente.StartsWith("4")) {
            var eng = await Api.GetEngenhariaAsync(produtoErp.CodComponente);
            if (eng != null && eng.statusEngenharia != StatusEngenharia.EmDesenvolvimento) {
              MsgBox.Show($"A Engenharia {produtoErp.CodComponente} está com status '{eng.statusEngenharia.ObterDescricaoEnum()}', só pode ser importada se estiver 'Em Desenvolvimento'.",
                "Ação não Permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              return new SortableBindingList<ProdutoErp>();
            } else {
              produtoErp.CodProduto = produtoErp.CodComponente;
              produtoErp.NaoAlterarNomeERP = true;
            }
          }

          swCustPropMngr = swModelDocExt.get_CustomPropertyManager(swConf.Name);

          if (string.IsNullOrEmpty(produtoErp.CodComponente)) {
            swCustPropMngr.Get2("Componente", out valOut, out resolvedValOut);
            produtoErp.CodComponente = resolvedValOut;
          }

          if (string.IsNullOrEmpty(produtoErp.Denominacao)) {
            swCustPropMngr.Get2("Denominação", out valOut, out resolvedValOut);
            produtoErp.Denominacao = resolvedValOut;
          }

          if (produtoErp.Name.StartsWith("40") && produtoErp.Name.Contains(produtoErp.CodComponente))
            produtoErp.CodComponente = produtoErp.Name.Split(' ')[0];

          if (string.IsNullOrEmpty(produtoErp.CodProduto) && produtoErp.CodComponente.StartsWith("40")) {
            swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");
            swCustPropMngr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodComponente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
            produtoErp.CodProduto = produtoErp.CodComponente;
          }

          produtoErp.Quantidade = 1;

          produtoErp.Configuracao = swConf.Name;

          //produto_erp_operacao.SelecionarProcessoProduto(produtoErp);

          TreeNode rootNode = treeView.Nodes.Add("Root", $"{produtoErp.Name} - {produtoErp.Denominacao}", 0);
          rootNode.Tag = produtoErp;

          if (tipo == (int)swDocumentTypes_e.swDocASSEMBLY) {
            // Inserir lista de material e pegar dados
            string templateGeral = $"{Application.StartupPath}\\01 - Addin LM\\ListaCompleta.sldbomtbt";
            int BomTypeGeral = (int)swBomType_e.swBomType_Indented;
            int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
            bool DetailedCutList = true;
            var swBOMAnnotationGeral = swModelDocExt.InsertBomTable3(templateGeral, 0, 1, BomTypeGeral, swConf.Name, false, NumberingType, DetailedCutList);
            await PegaDadosListaGeralAsync(swBOMAnnotationGeral, rootNode);
            ListaCorte.ExcluirLista(swModel);
          } else {  // =========================== INICIO PEÇA
            int status = 0;
            int warnings = 0;
            int errors = 0;

            var itensCorte = ListaCorte.GetCutList(swModel, produtoErp.PathName);

            //if (changeCutList && itensCorte.Count > 1) {
            //  var pathNameDesenho = produtoErp.PathName.Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW";

            //  Sw.App.OpenDoc6(pathNameDesenho, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
            //  Sw.App.ActivateDoc2(pathNameDesenho, false, errors);
            //  swModel = (ModelDoc2)Sw.App.ActiveDoc;

            //  Desenho.InserirAtualizarListaMaterias(swModel);
            //  swModel.Save();
            //  Sw.App.CloseDoc(pathNameDesenho);
            //}

            if (produtoErp.TipoComponente == TipoComponente.Peca && itensCorte.Count > 1 ||
              (produtoErp.TipoComponente == TipoComponente.Peca && itensCorte.Count == 1 &&
              itensCorte[0].Quantidade > 1)) {
              produtoErp.TipoComponente = TipoComponente.Montagem;
              produtoErp.Img3D = Properties.Resources.assembly;

              for (int indiceLista = 0; indiceLista < itensCorte.Count; indiceLista++) {
                ListaCorte itemCorte = itensCorte[indiceLista];
                var indiceNome = indiceLista + 1;
                var sufixo = $" - P{indiceNome}";

                // add soldagem como peça
                var item = new ProdutoErp {
                  PathName = produtoErp.PathName,
                  Name = produtoErp.Name + sufixo,
                  Denominacao = produtoErp.Denominacao,
                  Referencia = itemCorte.NomeLista,
                  CodComponente = produtoErp.CodComponente,
                  //CodProduto = itemCorte.CodProduto.ToString(),
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

                //produto_erp_operacao.SelecionarProcessoProduto(item);

                if (produtoErp.Name.Length + sufixo.Length > 50)
                  produtoErp.Name = produtoErp.Name.Substring(0, 50 - sufixo.Length) + sufixo;

                string nodeTextFilho = $"{item.Name} - {item.Denominacao}";
                var nodeFilho = new TreeNode(nodeTextFilho);
                nodeFilho.Tag = item;
                nodeFilho.ImageIndex = 1;
                nodeFilho.SelectedImageIndex = 1;
                rootNode.Nodes.Add(nodeFilho);

                // add material da peça de soldagem
                var itemLista = new ProdutoErp {
                  PathName = produtoErp.PathName,
                  Name = produtoErp.Name + sufixo,
                  Denominacao = itemCorte.Denominacao,
                  Referencia = itemCorte.NomeLista,
                  CodComponente = itemCorte.Codigo.ToString(),
                  CodProduto = itemCorte.Codigo.ToString(),
                  TipoComponente = TipoComponente.ListaMaterial,
                  Nivel = produtoErp.Nivel + ".1",
                  Configuracao = produtoErp.Configuracao,
                  Quantidade = itemCorte.Quantidade,
                  ItemCorte = itemCorte,
                };

                int imgIndex = itemCorte.Tipo == TipoListaMaterial.Chapa ? 3 : 4;

                string nodeTextLista = $"{itemLista.CodComponente} - {itemLista.Denominacao}";
                var nodeLista = new TreeNode(nodeTextLista);
                nodeLista.Tag = itemLista;
                nodeLista.ImageIndex = imgIndex;
                nodeLista.SelectedImageIndex = imgIndex;

                nodeFilho.Nodes.Add(nodeLista);
              }
            } else {
              if (itensCorte.Count == 1) {
                var itemCorte = itensCorte.FirstOrDefault();
                produtoErp.ItemCorte = itemCorte;

                var item = new ProdutoErp {
                  PathName = produtoErp.PathName,
                  Name = produtoErp.Name,
                  Denominacao = itemCorte.Denominacao,
                  Referencia = produtoErp.Referencia,
                  CodComponente = itemCorte.Codigo.ToString(),
                  CodProduto = itemCorte.Codigo.ToString(),
                  TipoComponente = TipoComponente.ListaMaterial,
                  Nivel = produtoErp.Nivel + ".1",
                  Configuracao = produtoErp.Configuracao,
                  Quantidade = itemCorte.Quantidade,
                  ItemCorte = itemCorte,
                };

                int imgIndex = itemCorte.Tipo == TipoListaMaterial.Chapa ? 3 : 4;

                string nodeTextFilho = $"{item.CodComponente} - {item.Denominacao}";
                var nodeFilho = new TreeNode(nodeTextFilho);
                nodeFilho.Tag = item;
                nodeFilho.ImageIndex = imgIndex;
                nodeFilho.SelectedImageIndex = imgIndex;
                rootNode.Nodes.Add(nodeFilho);
              }
            }
          }

          if (rootNode.Nodes.Count > 0) {
            rootNode.ExpandAll();

            await Loader.ShowDuringOperation(async (progress) => {
              progress.Report("Analisando componentes...");
              await PercorrerTreeViewAnalisarCompAsync(db, rootNode, _listaProduto, montageGeralNome, progress);
            });
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar lista produtos\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      if (!string.IsNullOrEmpty(mensagemAtencao)) {
        MsgBox.CloseWaitMessage();
        mensagemAtencao = "Analisar Itens Abaixo!\r\n\r\n" + mensagemAtencao;
        MsgBox.InputBox(titulo: "Atenção", textoImputPadrao: mensagemAtencao, textoLongo: true, CentralizarForm: true, somenteLeitura: true);
      }

      return new SortableBindingList<ProdutoErp>(_listaProduto);
    }

    private static async Task PegaDadosListaGeralAsync(BomTableAnnotation swBOMAnnotation, TreeNode rootNode) {
      string nameShort = "";
      try {
        int status = 0;
        int warnings = 0;
        string[] vModelPathNames = null;
        string strItemNumber = null;
        string strPartNumber = null;
        var swTableAnnotation = (TableAnnotation)swBOMAnnotation;

        int lStartRow = 1;

        if (!(swTableAnnotation.TitleVisible == false)) {
          lStartRow = 2;
        }

        var swBOMFeature = swBOMAnnotation.BomFeature;

        Dictionary<string, TreeNode> nodes = new Dictionary<string, TreeNode>();
        string nivelPecaLido = string.Empty;
        await Loader.ShowDuringOperation(
            "Iniciando leitura da tabela...",
            (progress2) => {
              var total = swTableAnnotation.TotalRowCount;
              for (int i = lStartRow; i < swTableAnnotation.TotalRowCount; i++) {
                if (!Loader._isWorking) {
                  rootNode.Nodes.Clear();
                  mensagemAtencao = string.Empty;
                  MsgBox.Show("Operação cancelada pelo usuário.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  return Task.FromResult("Cancelado");
                }

                // rotoina ler lista
                vModelPathNames = (string[])swBOMAnnotation.GetModelPathNames(i, out strItemNumber, out strPartNumber);
                if (vModelPathNames != null) {
                  var produtoErp = new ProdutoErp();
                  string ptNm = vModelPathNames[0];

                  produtoErp.PathName = ptNm;
                  produtoErp.Name = nameShort = Path.GetFileNameWithoutExtension(produtoErp.PathName);
                  if (produtoErp.Name.Contains("^"))
                    produtoErp.Name = produtoErp.Name.Substring(0, produtoErp.Name.IndexOf("^"));

                  if (produtoErp.Name.Length > 50)
                    produtoErp.Name = produtoErp.Name.Substring(0, 50);

                  progress2.Report(($"Lendo componentes... \n\n{nameShort}", i, total));

                  if (!File.Exists(produtoErp.PathName))
                    continue;

                  produtoErp.TipoComponente = ptNm.ToUpper().EndsWith("SLDPRT") ? TipoComponente.Peca : TipoComponente.Montagem;
                  var nivel = swTableAnnotation.get_Text(i, 0).Trim();

                  if (produtoErp.TipoComponente == TipoComponente.Peca) {
                    if (nivel.StartsWith(nivelPecaLido + "."))
                      continue;
                    else
                      nivelPecaLido = nivel;

                    nivelPecaLido = nivel;
                  }

                  var qtd = Convert.ToInt32(swTableAnnotation.get_Text(i, 1));
                  var codMaterial = swTableAnnotation.get_Text(i, 2).Trim();
                  var codComp = swTableAnnotation.get_Text(i, 3).Trim();

                  if (!string.IsNullOrEmpty(codMaterial) || produtoErp.Name.Contains("~"))
                    continue;

                  if (string.IsNullOrEmpty(codComp))
                    codComp = produtoErp.Name;

                  produtoErp.Nivel = "1." + nivel;
                  produtoErp.Quantidade = qtd;
                  produtoErp.CodComponente = codComp;
                  // produtoErp.CodProduto = swTableAnnotation.get_Text(i, 4).Trim();
                  produtoErp.Denominacao = swTableAnnotation.get_Text(i, 6);
                  var comprim = swTableAnnotation.get_Text(i, 10);
                  double.TryParse(comprim.Replace(".", ","), out double comprimento);
                  produtoErp.Comprimento = comprimento;
                  produtoErp.Configuracao = swTableAnnotation.get_Text(i, 11);
                  produtoErp.Referencia = produtoErp.Name;
                  var massa = swTableAnnotation.get_Text(i, 13);
                  produtoErp.PesoLiquido = !string.IsNullOrEmpty(massa) ? Convert.ToDouble(massa.Replace(".", ",")) : 0;
                  produtoErp.PesoBruto = produtoErp.PesoLiquido;
                  var fantasma = swTableAnnotation.get_Text(i, 14);
                  produtoErp.Fantasma = !string.IsNullOrEmpty(fantasma) && fantasma.ToLower() == "sim";

                  if (produtoErp.Fantasma)
                    produtoErp.ImgFantasma = Properties.Resources.fantasma;

                  produtoErp.TipoComponente = ptNm.ToUpper().Contains("BIBLIOTECA") ||
                    produtoErp.CodComponente.StartsWith("10") ||
                    produtoErp.CodComponente.StartsWith("20") ||
                    produtoErp.CodComponente.StartsWith("30")
                    ? TipoComponente.ItemBiblioteca
                    : ptNm.ToUpper().EndsWith("SLDPRT")
                    ? TipoComponente.Peca : TipoComponente.Montagem;

                  var swTipo = ptNm.ToUpper().EndsWith("SLDPRT") ? swDocumentTypes_e.swDocPART : swDocumentTypes_e.swDocASSEMBLY;

                  //produto_erp_operacao.SelecionarProcessoProduto(produtoErp);

                  if (produtoErp.CodComponente.Replace(" ", "") == "-" && !mensagemAtencao.Contains(produtoErp.Name)) {
                    mensagemAtencao += $"{produtoErp.Name} - {produtoErp.Denominacao}\r\n- Código componente = '-'.\r\n\r\n";
                  } else if ((produtoErp.TipoComponente == TipoComponente.Peca || produtoErp.TipoComponente == TipoComponente.Montagem) && !produtoErp.Name.StartsWith(produtoErp.CodComponente)) {
                    mensagemAtencao += $"{produtoErp.Name} - {produtoErp.Denominacao}\r\n- Código componente '{produtoErp.CodComponente}' parece estar incorreto.\r\n\r\n";
                  }

                  var swModel = Sw.App.OpenDoc6(produtoErp.PathName,
                  (int)swTipo, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, produtoErp.Configuracao, ref status, ref warnings);

                  if (produtoErp.PesoLiquido == 0) {
                    ModelDocExtension swModelDocExt = swModel.Extension;
                    CustomPropertyManager swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");
                    swCustPropMngr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                    swCustPropMngr.Get2("Massa", out _, out string resolvedValOut);
                    double.TryParse(resolvedValOut, NumberStyles.Any, CultureInfo.InvariantCulture, out double massaKG);
                    produtoErp.PesoLiquido = Math.Round(massaKG, 4);
                    produtoErp.PesoBruto = produtoErp.PesoLiquido;
                  }

                  var itensCorte = new List<ListaCorte>();

                  if (produtoErp.TipoComponente == TipoComponente.Peca) {
                    if (swModel == null) {
                      MsgBox.Show($"Erro ao abrir o arquivo {produtoErp.PathName}", "Addin LM Projetos",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                      continue;
                    }

                    itensCorte = ListaCorte.GetCutList(swModel, produtoErp.PathName);

                    if (itensCorte.Count == 1) {
                      produtoErp.ItemCorte = itensCorte[0];
                    }
                  }

                  var desenhoExiste = File.Exists(ptNm.ToUpper().Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW");
                  produtoErp.Img3D = produtoErp.TipoComponente == TipoComponente.ItemBiblioteca
                    ? Properties.Resources.toolbox_item
                    : produtoErp.TipoComponente == TipoComponente.Peca
                    ? Properties.Resources.part
                    : Properties.Resources.assembly;
                  produtoErp.Img2D = desenhoExiste ? Properties.Resources.draw : Properties.Resources.not_draw;

                  if (swModel != null && produtoErp.TipoComponente == TipoComponente.ItemBiblioteca || (produtoErp.CodComponente.StartsWith("40")) && string.IsNullOrEmpty(produtoErp.CodProduto)) {
                    produtoErp.CodProduto = produtoErp.CodComponente;
                    var swModelDocExt = swModel.Extension;
                    var swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");

                    if (produtoErp.CodComponente.StartsWith("10")) {
                      swCustPropMngr.Delete2("Código Produto");
                      swCustPropMngr = swModelDocExt.get_CustomPropertyManager(produtoErp.Configuracao);
                    }

                    swCustPropMngr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodComponente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                    swModel.Save();
                  } else if (swModel == null && produtoErp.TipoComponente == TipoComponente.ItemBiblioteca && produtoErp.CodComponente.StartsWith("10"))
                    produtoErp.CodProduto = produtoErp.CodComponente;

                  // engenharia de produto
                  bool verificaMaterial = produtoErp.TipoComponente == TipoComponente.Peca && itensCorte.Count > 0 && itensCorte[0].Quantidade == 1;
                  CreateTreeCompNode(rootNode, nodes, produtoErp, nivel, verificaMaterial);

                  if (produtoErp.TipoComponente == TipoComponente.Peca && itensCorte.Count > 1 ||
                    (produtoErp.TipoComponente == TipoComponente.Peca && itensCorte.Count == 1 && itensCorte[0].Quantidade > 1)) {
                    produtoErp.TipoComponente = TipoComponente.Montagem;
                    produtoErp.Img3D = Properties.Resources.assembly;

                    for (int indiceLista = 0; indiceLista < itensCorte.Count; indiceLista++) {
                      ListaCorte itemCorte = itensCorte[indiceLista];
                      var indiceNome = indiceLista + 1;
                      var sufixo = $" - P{indiceNome}";

                      var item = new ProdutoErp {
                        PathName = produtoErp.PathName,
                        Name = produtoErp.Name + sufixo,
                        Denominacao = produtoErp.Denominacao,
                        Referencia = itemCorte.NomeLista,
                        CodComponente = produtoErp.CodComponente,
                        //CodProduto = itemCorte.CodProduto,
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

                      //produto_erp_operacao.SelecionarProcessoProduto(item);

                      if (produtoErp.Name.Length + sufixo.Length > 50)
                        produtoErp.Name = produtoErp.Name.Substring(0, 50 - sufixo.Length) + sufixo;

                      CreateTreeCompNode(rootNode, nodes, item, nivel + "." + indiceNome, verificaMaterial: true);
                    }
                  }

                  Sw.App.CloseDoc(produtoErp.PathName);
                }
              }
              return Task.FromResult("concluído");
            },
            100
        );
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista\n\nItem: {nameShort}\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void CreateTreeCompNode(TreeNode rootNode, Dictionary<string, TreeNode> nodes, ProdutoErp produtoErp, string nivel, bool verificaMaterial) {
      string nodeText = $"{produtoErp.Name} - {produtoErp.Denominacao}";
      var node = new TreeNode(nodeText);

      node.Tag = produtoErp;

      var iconIndex = produtoErp.TipoComponente == TipoComponente.ItemBiblioteca ? 5 : produtoErp.TipoComponente == TipoComponente.Montagem ? 0 : 1;

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

        int imgIndex = produtoErp.ItemCorte.Tipo == TipoListaMaterial.Chapa ? 3 : 4;

        string nodeTextFilho = $"{produtoFilho.CodComponente} - {produtoFilho.Denominacao}";
        var nodeFilho = new TreeNode(nodeTextFilho);
        nodeFilho.Tag = produtoFilho;
        nodeFilho.ImageIndex = imgIndex;
        nodeFilho.SelectedImageIndex = imgIndex;

        node.Nodes.Add(nodeFilho);
      }

      nodes[nivel] = node;

      if (nivel.Contains('.')) {
        var parentLevel = nivel.Substring(0, nivel.LastIndexOf('.'));
        if (((ProdutoErp)nodes[parentLevel].Tag).TipoComponente != TipoComponente.ItemBiblioteca) {
          nodes[nivel] = node;
          nodes[parentLevel].Nodes.Add(node);
        }
      } else {
        nodes[nivel] = node;
        rootNode.Nodes.Add(node);
      }
    }

    #region Analizar Componentes e montar Lista de produtos

    private static async Task PercorrerTreeViewAnalisarCompAsync(ContextoDados db, TreeNode node, List<ProdutoErp> _listaProduto, string montageGeralNome, IProgress<string> progress) {
      var produtoErp = node.Tag as ProdutoErp;

      try {

        if (!Loader._isWorking) {
          // MsgBox.Show("Operação cancelada pelo usuário.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

        if (produtoErp != null) {
          if (!_listaProduto.Any(x => x.Name == produtoErp.Name && x.Referencia == produtoErp.Referencia && x.Configuracao == produtoErp.Configuracao)) {

            if (produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
              await AnalisarProdutoAsync(db, produtoErp, node, montageGeralNome);
            } else {
              // verificar tinta e cabo de aço
              if (produtoErp.Comprimento > 0 && produtoErp.PathName.EndsWith("SLDPRT")) {
                var material = await Api.GetItemGenericoAsync(produtoErp.CodProduto);
                produtoErp.UnidadeMedida = material.unidadeMedida;
              }

              var codprod = Convert.ToInt64(produtoErp.CodProduto);
              produtoErp.CadastrarAddin = !db.produto_erp.Any(x => x.codigo_produto == codprod);
            }

            if (produtoErp.TipoComponente != TipoComponente.ListaMaterial)
              _listaProduto.Add(produtoErp);

            if (node.Nodes.Count > 0) {
              foreach (TreeNode nodeFilho in node.Nodes) {
                progress.Report($"Analisando Produto\r\n{produtoErp.Name}");

                await PercorrerTreeViewAnalisarCompAsync(db, nodeFilho, _listaProduto, montageGeralNome, progress);
              }
            }
          }
        }
      } catch (Exception ex) {
        Loader._isWorking = false;
        MsgBox.Show($"Erro ao fazer análise.\r\nItem: {produtoErp?.Name}\n\n{ex.Message}.",
          "Ação não Permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //Toast.Error($"Erro ao fazer análise.\r\nItem: {produtoErp?.Name}\n\n{ex.Message}");
      }
    }

    private static async Task AnalisarProdutoAsync(ContextoDados db, ProdutoErp produtoErp, TreeNode node, string montageGeralNome) {
      try {
        int status = 0;
        int warnings = 0;
        int tipo = produtoErp.PathName.EndsWith("SLDASM")
        ? (int)swDocumentTypes_e.swDocASSEMBLY
        : (int)swDocumentTypes_e.swDocPART;
        var swModel = Sw.App.OpenDoc6(produtoErp.PathName, tipo,
          (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

        /*
         *  === Mudanças que acontecem no produto aqui, devem ser replicadas manualmente ao preparar engenharia para cadastro, pois este produto pode estar em outtro ponto da arvore
         * */
        // analizar materia prima
        if (produtoErp.TipoComponente == TipoComponente.Peca) {
          var material = await Api.GetItemGenericoAsync(produtoErp.ItemCorte?.Codigo.ToString());
          produtoErp.UnidadeMedida = "PC";
          if (material != null) {
            produtoErp.ItemCorte.unidadeMedida = material.unidadeMedida;
            produtoErp.PesoLiquido = produtoErp.ItemCorte.Massa;
            produtoErp.PesoPadraoNBR = material.pesoPadraoNBR;
            // analisar material
            if (produtoErp.ItemCorte.Tipo == TipoListaMaterial.Chapa && produtoErp.ItemCorte.Espessura > 0 &&
              produtoErp.ItemCorte.unidadeMedida != "PC") {
              var chapas = materia_primas.Selecionar(ativo: true, produtoErp.ItemCorte.Espessura);
              if (!chapas.Any(x => x.CodigoChapa == produtoErp.ItemCorte.Codigo))
                AdicionarPendencia(produtoErp, PendenciasEngenharia.MateriaErrado);
            } else {

            }

            // calcular peso
            CalcularPeso(ref produtoErp);

            if (produtoErp.PesoBruto == 0 && !mensagemAtencao.Contains(produtoErp.Name)) {
              mensagemAtencao += $"{produtoErp.Name} - {produtoErp.Denominacao}\r\n- Erro ao calcular peso.\r\n\r\n";
            }

          } else if (produtoErp.ItemCorte != null) {
            AdicionarPendencia(produtoErp, PendenciasEngenharia.MateriaPrimaInexistente);
          }
        } else {
          produtoErp.UnidadeMedida = "CJ";
        }

        // verificar se item já foi cadastrado no ERP
        var itens = await Api.GetItemGenericoByNameAsync(" - " + produtoErp.Name);
        itens = itens.Where(x => x.nome.EndsWith(" - " + produtoErp.Name) && x.situacao == 1).ToList();
        if (itens.Count == 1) {
          produtoErp.CadastrarProdutoErp = produtoErp.CadastrarAddin = false;
          produtoErp.CodProduto = itens[0].codigo.ToString();

          var swModelDocExt = swModel.Extension;
          var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

          if (produtoErp.TipoComponente == TipoComponente.Peca && produtoErp.Referencia.StartsWith("Item da lista de corte")) {
            bool boolstatus = swModel.Extension.SelectByID2(produtoErp.ItemCorte.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
            Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
            swCustPropMgr = swFeat.CustomPropertyManager;
          }

          swCustPropMgr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
          swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
          swModel.Save();
        } else if (itens.Count > 1) {
          string msg = string.Join("\r\n", itens.Select(item => $"- {item.codigo}"));

          MsgBox.Show($"Foram encontrados mais de um código para este item {produtoErp.Name}.\r\n" +
            $"{msg}", "Item em Duplicidade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        if (string.IsNullOrEmpty(produtoErp.CodProduto)) {
          var prod = db.produto_erp.FirstOrDefault(x => x.name == produtoErp.Name && x.referencia == produtoErp.Referencia);
          if (prod != null) {
            produtoErp.CadastrarProdutoErp = produtoErp.CadastrarAddin = false;
            produtoErp.CodProduto = prod.codigo_produto.ToString();
            // atualizar props
            var swModelDocExt = swModel.Extension;
            var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

            if (produtoErp.TipoComponente == TipoComponente.Peca && produtoErp.Referencia.StartsWith("Item da lista de corte")) {
              bool boolstatus = swModel.Extension.SelectByID2(produtoErp.ItemCorte.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

              SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
              Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
              swCustPropMgr = swFeat.CustomPropertyManager;
            }

            swCustPropMgr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
            swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
            swModel.Save();
          } else {
            // verificar cadastros no ERP
            produtoErp.CadastrarProdutoErp = !produtoErp.CodComponente.StartsWith("40");

            produtoErp.CadastrarAddin = true;
          }
        } else {
          var cod = Convert.ToInt64(produtoErp.CodProduto);
          var prod = db.produto_erp.FirstOrDefault(x => x.codigo_produto == cod && x.name == produtoErp.Name && x.referencia == produtoErp.Referencia);

          if (prod != null) {
            var engenharia = await Api.GetEngenhariaAsync(produtoErp.CodProduto);
            produtoErp.SobremetalLarg = prod.sobremetal_largura;
            produtoErp.SobremetalCompr = prod.sobremetal_comprimento;
            produtoErp.CadastrarProdutoErp = produtoErp.CadastrarAddin = false;

            if (produtoErp.SobremetalCompr > 0 || produtoErp.SobremetalLarg > 0) {
              CalcularPeso(ref produtoErp);

              if (produtoErp.PesoBruto == 0 && !mensagemAtencao.Contains(produtoErp.Name)) {
                mensagemAtencao += $"{produtoErp.Name} - {produtoErp.Denominacao}\r\n- Erro ao calcular peso.\r\n\r\n";
              }
            }

            if (engenharia != null) {
              if (engenharia.tipoEngenharia == "2")
                produtoErp.Fantasma = true;

              await VerificarOperacaoERPAsync(db, produtoErp, swModel, engenharia.operacoes);
            }
          } else {
            var engenharia = await Api.GetEngenhariaAsync(produtoErp.CodProduto);
            if (!produtoErp.Referencia.StartsWith("Item da lista de corte")) {
              var itemERP = await Api.GetItemGenericoAsync(produtoErp.CodProduto);

              if (!produtoErp.CodComponente.StartsWith("30") && !produtoErp.CodComponente.StartsWith("4"))
                produtoErp.CadastrarProdutoErp = itemERP == null || (produtoErp.Referencia.ToUpper() != itemERP.refTecnica.ToUpper() && !itemERP.nome.ToUpper().EndsWith(produtoErp.Referencia.ToUpper()));
              else
                produtoErp.CadastrarProdutoErp = false;

              if (produtoErp.CodProduto.StartsWith("40") && produtoErp.CadastrarProdutoErp) {
                produtoErp.CadastrarProdutoErp = false;
                //throw new Exception($"Código Produto {produtoErp.CodProduto} inválido para esta montagem:\r\n{produtoErp.Name} - {produtoErp.Denominacao}");
              }

              if (!produtoErp.CadastrarProdutoErp && engenharia != null) {
                await VerificarOperacaoERPAsync(db, produtoErp, swModel, engenharia.operacoes);
              }

            } else {
              await
                VerificarOperacaoERPAsync(db, produtoErp, swModel, engenharia?.operacoes);
            }

            produtoErp.CadastrarAddin = true;
          }
        }

        if (!produtoErp.Name.StartsWith(montageGeralNome))
          Sw.App.CloseDoc(produtoErp.PathName);

        if (produtoErp.CadastrarProdutoErp)
          produtoErp.CodProduto = string.Empty;
      } catch (Exception ex) {
        throw new Exception(ex.Message);
        //LmException.ShowException(ex, "Erro ao analisar produto");
      }
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

    private static async Task VerificarOperacaoERPAsync(ContextoDados db, ProdutoErp produtoErp, ModelDoc2 swModel, List<OperacaoEng> engenhariaOperacoes) {
      if (engenhariaOperacoes != null && engenhariaOperacoes.Count > 0) {
        // verificar alteração de operações no erp ou solid
        foreach (var operacao in engenhariaOperacoes) {
          var op = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == operacao.codOperacao && x.mascaraMaquina == operacao.codMascaraMaquina);
          var minhasOps = produtoErp.Operacoes.Select(x => x.processo_id).ToList();

          if (op != null && !minhasOps.Contains(op.codAxion)) {
            AtualizarOperacao(produtoErp, operacao, op.codAxion);
          }
          //else if (op == null) {
          //  var opCad = Processo.ListaOperacoesERP.FirstOrDefault(x => x.codOperacao == operacao.codOperacao);
          //  var maCad = Processo.ListaMaquinasERP.FirstOrDefault(x => x.mascara == operacao.codMascaraMaquina);
          //  if (opCad == null || maCad == null)
          //    continue;

          //  var processo = new processos {
          //    codigo_maquina = maCad.codMaquina,
          //    codigo_operacao = opCad.codOperacao,
          //    ativo = true,
          //  };

          //  db.processos.Add(processo);
          //  db.SaveChanges();

          //  await Processo.Carregar();

          //  AtualizarOperacao(produtoErp, operacao, processo.id);

          //  // atualizar props
          //  if (!minhasOps.Contains(processo.codigo_operacao)) {
          //    AtualizarOperacao(produtoErp, operacao, processo.id);
          //  }
          //}
        }
      }
    }

    internal static void AdicionarPendencia(ProdutoErp produtoErp, PendenciasEngenharia pendencia) {
      if (!produtoErp.Pendencias.Contains(pendencia)) {
        produtoErp.Pendencias.Add(pendencia);
        produtoErp.ImgPendencia = pendencia.EhPendenciaCritica() || produtoErp.Pendencias.Any(x => x.EhPendenciaCritica()) ? Properties.Resources.error : Properties.Resources.warning;
      }
    }

    internal static void RemoverPendencia(ProdutoErp produtoErp, PendenciasEngenharia pendencia) {
      var list = produtoErp.Pendencias.Where(x => x == pendencia);
      list.ToList().ForEach(x => { produtoErp.Pendencias.Remove(x); });

      if (!produtoErp.Pendencias.Any())
        produtoErp.ImgPendencia = new Bitmap(20, 20);
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
    }

    #endregion

  }
}
