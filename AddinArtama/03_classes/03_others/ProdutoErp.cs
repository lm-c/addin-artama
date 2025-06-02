using LmCorbieUI;
using LmCorbieUI.Metodos;
using LmCorbieUI.Metodos.AtributosCustomizados;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    //[Browsable(false)]
    [DisplayName("Processos")]
    [LarguraColunaGrid(100)]
    public string Operacao { get; set; } = string.Empty;

    [Browsable(false)]
    [DisplayName("QTD")]
    [LarguraColunaGrid(50)]
    public int Quantidade { get; set; }

    [Browsable(false)]
    public double PesoBruto { get; set; }

    [Browsable(false)]
    public double PesoLiquido { get; set; }

    [Browsable(false)]
    public string PathName { get; set; }

    [Browsable(false)]
    public TipoComponente TipoComponente { get; set; }

    [Browsable(false)]
    public double SobremetalLarg { get; set; } = 0;

    [Browsable(false)]
    public double SobremetalCompr { get; set; } = 0;

    [Browsable(false)]
    public bool CadastrarErp { get; set; }

    [Browsable(false)]
    public bool Fantasma { get; set; }

    [Browsable(false)]
    public bool CadastrarAddin { get; set; }

    public List<PendenciasEngenharia> Pendencias = new List<PendenciasEngenharia>();

    public List<ListaCorte> ItensCorte = new List<ListaCorte>();

    public static async Task<SortableBindingList<ProdutoErp>> GetComponentsAsync(TreeView treeView, string montageGeralNome) {
      var _listaProduto = new List<ProdutoErp>();

      try {
        MsgBox.ShowWaitMessage("Lendo componentes...");
        using (ContextoDados db = new ContextoDados()) {
          treeView.Nodes.Clear();

          ImageList il = new ImageList();
          il.Images.Add(0.ToString(), Properties.Resources.assembly);
          il.Images.Add(1.ToString(), Properties.Resources.part);
          il.Images.Add(2.ToString(), Properties.Resources.weldmentcutlist);
          il.Images.Add(3.ToString(), Properties.Resources.sheetmetal);
          il.Images.Add(4.ToString(), Properties.Resources.weldment);
          il.Images.Add(5.ToString(), Properties.Resources.toolbox_item);

          treeView.ImageList = il;
          treeView.ItemHeight = 21;

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
          };

          var desenhoExiste = File.Exists(pathName.ToUpper().Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW");
          produtoErp.Img3D = produtoErp.TipoComponente == TipoComponente.Peca ? Properties.Resources.part : Properties.Resources.assembly;
          produtoErp.Img2D = desenhoExiste ? Properties.Resources.draw : Properties.Resources.not_draw;

          string valOut;
          string resolvedValOut;

          swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");
          swCustPropMngr.Get2("Denominação", out valOut, out resolvedValOut);
          produtoErp.Denominacao = resolvedValOut;
          swCustPropMngr.Get2("Componente", out valOut, out resolvedValOut);
          produtoErp.CodComponente = resolvedValOut;
          swCustPropMngr.Get2("Operação", out valOut, out resolvedValOut);
          produtoErp.Operacao = resolvedValOut;
          swCustPropMngr.Get2("Massa", out valOut, out resolvedValOut);
          double.TryParse(resolvedValOut, NumberStyles.Any, CultureInfo.InvariantCulture, out double massa);
          produtoErp.PesoLiquido = Math.Round(massa, 3);
          produtoErp.PesoBruto = produtoErp.PesoLiquido;
          swCustPropMngr.Get2("Fantasma", out valOut, out resolvedValOut);
          produtoErp.Fantasma = !string.IsNullOrEmpty(resolvedValOut) && resolvedValOut.ToLower() == "sim";

          if (produtoErp.Fantasma)
            produtoErp.ImgFantasma = Properties.Resources.fantasma;

          if (produtoErp.Operacao.Contains("/"))
            produtoErp.Operacao = produtoErp.Operacao.Replace("/", "^");

          produtoErp.TipoComponente = pathName.ToUpper().Contains("BIBLIOTECA") ||
            produtoErp.CodComponente.StartsWith("10") ||
            produtoErp.CodComponente.StartsWith("20") ||
            produtoErp.CodComponente.StartsWith("30")
            ? TipoComponente.ItemBiblioteca
            : pathName.ToUpper().EndsWith("SLDPRT")
            ? TipoComponente.Peca : TipoComponente.Montagem;

          if (produtoErp.TipoComponente == TipoComponente.ItemBiblioteca) {
            Toast.Warning("Recurso indisponivel para o componentes de biblioteca.");
            return new SortableBindingList<ProdutoErp>();
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

          swCustPropMngr.Get2("Código Produto", out valOut, out resolvedValOut);
          produtoErp.CodProduto = resolvedValOut;

          if (produtoErp.Name.StartsWith("40") && produtoErp.Name.Contains(produtoErp.CodComponente))
            produtoErp.CodComponente = produtoErp.Name.Split(' ')[0];

          if (string.IsNullOrEmpty(produtoErp.CodProduto) && produtoErp.CodComponente.StartsWith("40")) {
            swCustPropMngr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodComponente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
            produtoErp.CodProduto = produtoErp.CodComponente;
          }

          produtoErp.Quantidade = 1;

          produtoErp.Configuracao = swConf.Name;

          TreeNode rootNode = treeView.Nodes.Add("Root", $"{produtoErp.Name} - {produtoErp.Denominacao}", 0);
          rootNode.Tag = produtoErp;

          if (tipo == (int)swDocumentTypes_e.swDocASSEMBLY) {
            // Inserir lista de material e pegar dados
            string templateGeral = $"{Application.StartupPath}\\01 - Addin LM\\ListaCompleta.sldbomtbt";
            int BomTypeGeral = (int)swBomType_e.swBomType_Indented;
            int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
            bool DetailedCutList = false;
            var swBOMAnnotationGeral = swModelDocExt.InsertBomTable3(templateGeral, 0, 1, BomTypeGeral, swConf.Name, false, NumberingType, DetailedCutList);
            PegaDadosListaGeral(swBOMAnnotationGeral, rootNode);
            ListaCorte.ExcluirLista(swModel);
          } else {
            int status = 0;
            int warnings = 0;
            int errors = 0;

            produtoErp.ItensCorte = ListaCorte.GetCutList(swModel, produtoErp.PathName, out bool changeCutList);

            if (changeCutList && produtoErp.ItensCorte.Count > 1) {
              var pathNameDesenho = produtoErp.PathName.Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW";

              Sw.App.OpenDoc6(pathNameDesenho, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
              Sw.App.ActivateDoc2(pathNameDesenho, false, errors);
              swModel = (ModelDoc2)Sw.App.ActiveDoc;

              Desenho.InserirAtualizarListaMaterias(swModel);
              swModel.Save();
              Sw.App.CloseDoc(pathNameDesenho);
            }

            if (produtoErp.TipoComponente == TipoComponente.Peca && produtoErp.ItensCorte.Count > 1) {
              produtoErp.TipoComponente = TipoComponente.Montagem;
              produtoErp.Img3D = Properties.Resources.assembly;

              for (int indiceLista = 0; indiceLista < produtoErp.ItensCorte.Count; indiceLista++) {
                ListaCorte itemCorte = produtoErp.ItensCorte[indiceLista];
                var indiceNome = indiceLista + 1;
                var sufixo = $" - P{indiceNome}";

                // add soldagem como peça
                var item = new ProdutoErp {
                  PathName = produtoErp.PathName,
                  Name = produtoErp.Name + sufixo,
                  Denominacao = produtoErp.Denominacao,
                  Referencia = itemCorte.NomeLista,
                  CodComponente = produtoErp.CodComponente,
                  CodProduto = itemCorte.CodProduto,
                  Img3D = Properties.Resources.part,
                  Img2D = produtoErp.Img2D,
                  TipoComponente = TipoComponente.Peca,
                  Nivel = produtoErp.Nivel + "." + indiceNome,
                  Configuracao = produtoErp.Configuracao,
                  Quantidade = itemCorte.Quantidade,
                  Operacao = itemCorte.Operacao,
                  PesoLiquido = itemCorte.Massa,
                  PesoBruto = itemCorte.Massa,
                  ItensCorte = new List<ListaCorte> { itemCorte }
                };

                if (produtoErp.Name.Length + sufixo.Length > 50)
                  produtoErp.Name = produtoErp.Name.Substring(0, 50 - sufixo.Length) + sufixo;

                string nodeTextFilho = $"{item.CodComponente} - {item.Denominacao}";
                var nodeFilho = new TreeNode(nodeTextFilho);
                nodeFilho.Tag = item;
                nodeFilho.ImageIndex = 1;
                nodeFilho.SelectedImageIndex = 1;
                rootNode.Nodes.Add(nodeFilho);

                // add material da peça de soldagem
                var itemLista = new ProdutoErp {
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
                  ItensCorte = produtoErp.ItensCorte,
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
              if (produtoErp.ItensCorte.Count == 1) {
                var itemCorte = produtoErp.ItensCorte.FirstOrDefault();
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
                  ItensCorte = produtoErp.ItensCorte,
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

          rootNode.ExpandAll();

          MsgBox.ShowWaitMessage("Analisando componentes...");
          await PercorrerTreeViewAnalisarCompAsync(db, rootNode, _listaProduto, montageGeralNome);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar lista produtos\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return new SortableBindingList<ProdutoErp>(_listaProduto);
    }

    private static void PegaDadosListaGeral(BomTableAnnotation swBOMAnnotation, TreeNode rootNode) {
      string nameShort = "";
      try {
        int status = 0;
        int warnings = 0;
        int errors = 0;
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

        for (int i = lStartRow; i < swTableAnnotation.TotalRowCount; i++) {
          vModelPathNames = (string[])swBOMAnnotation.GetModelPathNames(i, out strItemNumber, out strPartNumber);

          if (vModelPathNames != null) {
            var produtoErp = new ProdutoErp();
            string ptNm = vModelPathNames[0];

            produtoErp.PathName = ptNm;
            produtoErp.Name = Path.GetFileNameWithoutExtension(produtoErp.PathName);
            if (produtoErp.Name.Contains("^"))
              produtoErp.Name = produtoErp.Name.Substring(0, produtoErp.Name.IndexOf("^"));

            if (produtoErp.Name.Length > 50)
              produtoErp.Name = produtoErp.Name.Substring(0, 50);

            if (!File.Exists(produtoErp.PathName))
              continue;

            var qtd = Convert.ToInt32(swTableAnnotation.get_Text(i, 1));

            produtoErp.TipoComponente = ptNm.ToUpper().EndsWith("SLDPRT") ? TipoComponente.Peca : TipoComponente.Montagem;
            var nivel = swTableAnnotation.get_Text(i, 0).Trim();
            produtoErp.Nivel = "1." + nivel;
            produtoErp.Quantidade = qtd;
            produtoErp.CodComponente = swTableAnnotation.get_Text(i, 3).Trim();
            produtoErp.CodProduto = swTableAnnotation.get_Text(i, 4).Trim();
            produtoErp.Denominacao = swTableAnnotation.get_Text(i, 6);
            produtoErp.Configuracao = swTableAnnotation.get_Text(i, 11);
            produtoErp.Operacao = swTableAnnotation.get_Text(i, 12);
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

            int swTipo = ptNm.ToUpper().EndsWith("SLDPRT") ? (int)swDocumentTypes_e.swDocPART : (int)swDocumentTypes_e.swDocASSEMBLY;

            if (produtoErp.Operacao.Contains("/"))
              produtoErp.Operacao = produtoErp.Operacao.Replace("/", "^");

            var swModel = Sw.App.OpenDoc6(produtoErp.PathName,
            swTipo, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

            if (produtoErp.TipoComponente == TipoComponente.Peca) {
              produtoErp.ItensCorte = ListaCorte.GetCutList(swModel, produtoErp.PathName, out bool changeCutList);

              if (changeCutList && produtoErp.ItensCorte.Count > 1) {
                var pathNameDesenho = produtoErp.PathName.Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW";

                Sw.App.OpenDoc6(pathNameDesenho, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
                Sw.App.ActivateDoc2(pathNameDesenho, false, errors);
                swModel = (ModelDoc2)Sw.App.ActiveDoc;

                Desenho.InserirAtualizarListaMaterias(swModel);
                swModel.Save();
                Sw.App.CloseDoc(pathNameDesenho);
              }
            }

            var desenhoExiste = File.Exists(ptNm.ToUpper().Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW");
            produtoErp.Img3D = produtoErp.TipoComponente == TipoComponente.ItemBiblioteca
              ? Properties.Resources.toolbox_item
              : produtoErp.TipoComponente == TipoComponente.Peca
              ? Properties.Resources.part
              : Properties.Resources.assembly;
            produtoErp.Img2D = desenhoExiste ? Properties.Resources.draw : Properties.Resources.not_draw;

            if ((produtoErp.TipoComponente == TipoComponente.ItemBiblioteca || produtoErp.CodComponente.StartsWith("40")) && string.IsNullOrEmpty(produtoErp.CodProduto)) {
              produtoErp.CodProduto = produtoErp.CodComponente;
              var swModelDocExt = swModel.Extension;
              var swConfMgr = swModel.ConfigurationManager;
              var swConf = swConfMgr.ActiveConfiguration;
              var swCustPropMngr = swModelDocExt.get_CustomPropertyManager(swConf.Name);
              swCustPropMngr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodComponente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

              swModel.Save();
            }

            CreateTreeCompNode(rootNode, nodes, produtoErp, nivel);

            if (produtoErp.TipoComponente == TipoComponente.Peca && produtoErp.ItensCorte.Count > 1) {
              produtoErp.TipoComponente = TipoComponente.Montagem;
              produtoErp.Img3D = Properties.Resources.assembly;

              for (int indiceLista = 0; indiceLista < produtoErp.ItensCorte.Count; indiceLista++) {
                ListaCorte itemCorte = produtoErp.ItensCorte[indiceLista];
                var indiceNome = indiceLista + 1;
                var sufixo = $" - P{indiceNome}";

                var item = new ProdutoErp {
                  PathName = produtoErp.PathName,
                  Name = produtoErp.Name + sufixo,
                  Denominacao = produtoErp.Denominacao,
                  Referencia = itemCorte.NomeLista,
                  CodComponente = produtoErp.CodComponente,
                  CodProduto = itemCorte.CodProduto,
                  Img3D = Properties.Resources.part,
                  Img2D = produtoErp.Img2D,
                  TipoComponente = TipoComponente.Peca,
                  Nivel = produtoErp.Nivel + "." + indiceNome,
                  Configuracao = produtoErp.Configuracao,
                  Quantidade = itemCorte.Quantidade,
                  Operacao = itemCorte.Operacao,
                  PesoLiquido = itemCorte.Massa,
                  PesoBruto = itemCorte.Massa,
                  ItensCorte = new List<ListaCorte> { itemCorte }
                };

                if (produtoErp.Operacao.Contains("/"))
                  produtoErp.Operacao = produtoErp.Operacao.Replace("/", "^");

                if (produtoErp.Name.Length + sufixo.Length > 50)
                  produtoErp.Name = produtoErp.Name.Substring(0, 50 - sufixo.Length) + sufixo;

                CreateTreeCompNode(rootNode, nodes, item, nivel + "." + indiceNome);
              }
            }

            Sw.App.CloseDoc(produtoErp.PathName);
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void CreateTreeCompNode(TreeNode rootNode, Dictionary<string, TreeNode> nodes, ProdutoErp produtoErp, string nivel) {
      string nodeText = $"{produtoErp.Name} - {produtoErp.Denominacao}";
      var node = new TreeNode(nodeText);

      node.Tag = produtoErp;

      var iconIndex = produtoErp.TipoComponente == TipoComponente.ItemBiblioteca ? 5 : produtoErp.TipoComponente == TipoComponente.Montagem || produtoErp.ItensCorte.Count > 1 ? 0 : 1;

      node.ImageIndex = iconIndex;
      node.SelectedImageIndex = iconIndex;

      if (produtoErp.TipoComponente == TipoComponente.Peca && produtoErp.ItensCorte.Count == 1 &&
        !produtoErp.CodComponente.StartsWith("40")) {
        var itemCorte = produtoErp.ItensCorte.FirstOrDefault();

        var produtoFilho = new ProdutoErp {
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
          ItensCorte = produtoErp.ItensCorte,
        };

        int imgIndex = itemCorte.Tipo == TipoListaMaterial.Chapa ? 3 : 4;

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
        nodes[parentLevel].Nodes.Add(node);
      } else {
        rootNode.Nodes.Add(node);
      }
    }

    #region Analizar Componentes e montar Lista de produtos

    private static async Task PercorrerTreeViewAnalisarCompAsync(ContextoDados db, TreeNode node, List<ProdutoErp> _listaProduto, string montageGeralNome) {
      try {
        var produtoErp = node.Tag as ProdutoErp;
        if (produtoErp != null) {
          if (!_listaProduto.Any(x => x.Name == produtoErp.Name && x.Referencia == produtoErp.Referencia && x.Configuracao == produtoErp.Configuracao)) {

            if (produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
              await AnalisarProdutoAsync(db, produtoErp, node, montageGeralNome);
            } else {
              var codprod = Convert.ToInt64(produtoErp.CodProduto);
              produtoErp.CadastrarAddin = !db.produto_erp.Any(x => x.codigo_produto == codprod);
            }

            if (produtoErp.TipoComponente != TipoComponente.ListaMaterial)
              _listaProduto.Add(produtoErp);

            if (node.Nodes.Count > 0) {
              foreach (TreeNode nodeFilho in node.Nodes) {
                await PercorrerTreeViewAnalisarCompAsync(db, nodeFilho, _listaProduto, montageGeralNome);
              }
            }
          }
        }
      } catch (Exception ex) {
        Toast.Error("Erro ao fazer análise:\r\n" + ex.Message);
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

        // analizar materia prima
        if (produtoErp.TipoComponente == TipoComponente.Peca && produtoErp.ItensCorte.Count == 1) {
          var material = await Api.GetItemGenericoAsync(produtoErp.ItensCorte[0].Codigo.ToString());
          if (material != null) {
            produtoErp.PesoLiquido = produtoErp.ItensCorte[0].Massa;
            if (produtoErp.ItensCorte[0].Tipo == TipoListaMaterial.Chapa) {
              produtoErp.PesoBruto = (produtoErp.ItensCorte[0].CxdLarg / 1000) * (produtoErp.ItensCorte[0].CxdCompr / 1000) * material.pesoPadraoNBR;
            } else {
              produtoErp.PesoBruto = (produtoErp.ItensCorte[0].CxdCompr / 1000) * material.pesoPadraoNBR;
            }

            produtoErp.PesoBruto = Math.Round(produtoErp.PesoBruto, 3);
          } else {
            AdicionarPendencia(produtoErp, PendenciasEngenharia.MateriaPrimaIncorreta);
          }
        }

        if (string.IsNullOrEmpty(produtoErp.CodProduto)) {
          var prod = db.produto_erp.FirstOrDefault(x => x.name == produtoErp.Name && x.referencia == produtoErp.Referencia);
          if (prod != null) {
            produtoErp.CadastrarErp = produtoErp.CadastrarAddin = false;
            produtoErp.CodProduto = prod.codigo_produto.ToString();
            // atualizar props
            var swModelDocExt = swModel.Extension;
            var swCustPropMgr = swModelDocExt.get_CustomPropertyManager(produtoErp.Configuracao);

            produtoErp.ItensCorte[0].CodProduto = produtoErp.CodProduto;
            bool boolstatus = swModel.Extension.SelectByID2(produtoErp.ItensCorte[0].NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
            Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
            swCustPropMgr = swFeat.CustomPropertyManager;

            swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
            swModel.Save();

            //if (i > 0)
          } else {
            produtoErp.CadastrarErp = !produtoErp.CodComponente.StartsWith("40");

            produtoErp.CadastrarAddin = true;
          }
        } else {
          var cod = Convert.ToInt64(produtoErp.CodProduto);
          var prod = db.produto_erp.FirstOrDefault(x => x.codigo_produto == cod && x.name == produtoErp.Name && x.referencia == produtoErp.Referencia);

          if (prod != null) {
            var engenharia = await Api.GetEngenhariaAsync(produtoErp.CodProduto);
            produtoErp.SobremetalLarg = prod.sobremetal_largura;
            produtoErp.SobremetalCompr = prod.sobremetal_comprimento;
            produtoErp.CadastrarErp = produtoErp.CadastrarAddin = false;
            if (engenharia != null) {
              await VerificarOperacaoERPAsync(db, produtoErp, swModel, engenharia.operacoes);
              if (produtoErp.PathName.ToUpper().EndsWith("SLDPRT") && produtoErp.ItensCorte.Count > 1) {
                // adicionar códigos nos filhos caso não tenham
                foreach (var compEng in engenharia.componentes) {

                }
              }
            }
          } else {
            var engenharia = await Api.GetEngenhariaAsync(produtoErp.CodProduto);
            if (!produtoErp.Referencia.StartsWith("Item da lista de corte")) {
              var itemERP = await Api.GetItemGenericoAsync(produtoErp.CodProduto);

              if (!produtoErp.CodComponente.StartsWith("30"))
                produtoErp.CadastrarErp = itemERP == null || (produtoErp.Referencia != itemERP.refTecnica && !itemERP.nome.EndsWith(produtoErp.Referencia));
              else
                produtoErp.CadastrarErp = itemERP == null || (produtoErp.CodComponente != itemERP.refTecnica && !itemERP.nome.EndsWith(produtoErp.CodComponente));

              //if (produtoErp.CodProduto.StartsWith("40") && produtoErp.CadastrarErp)
              //  throw new Exception($"Código Produto {produtoErp.CodProduto} inválido para esta montagem:\r\n{produtoErp.Name} - {produtoErp.Denominacao}");

              if (!produtoErp.CadastrarErp && engenharia != null) {
                await VerificarOperacaoERPAsync(db, produtoErp, swModel, engenharia.operacoes);
                if (produtoErp.PathName.ToUpper().EndsWith("SLDPRT") && produtoErp.ItensCorte.Count > 1) {
                  // adicionar códigos nos filhos caso não tenham
                }
              }
              // se for estrutura adiocionar código nos filhos
              if (produtoErp.ItensCorte.Count > 1 && engenharia != null) {
                for (int indiceLista = 0; indiceLista < node.Nodes.Count; indiceLista++) {
                  var itemNo = node.Nodes[indiceLista].Tag as ProdutoErp;
                  if (engenharia != null && engenharia.componentes.Count > indiceLista) {
                    var compEngenharia = engenharia.componentes[indiceLista];
                    itemNo.CodProduto = compEngenharia.codInsumo;
                  } else {
                    itemNo.CodProduto = string.Empty;
                  }
                }
              }
            } else {
              await VerificarOperacaoERPAsync(db, produtoErp, swModel, engenharia.operacoes);
            }

            produtoErp.CadastrarAddin = true;
          }
        }

        if (!produtoErp.Name.StartsWith(montageGeralNome))
          Sw.App.CloseDoc(produtoErp.PathName);

        if (produtoErp.CadastrarErp)
          produtoErp.CodProduto = string.Empty;
      } catch (Exception ex) {
        throw new Exception(ex.Message);
        //LmException.ShowException(ex, "Erro ao analisar produto");
      }
    }

    private static async Task VerificarOperacaoERPAsync(ContextoDados db, ProdutoErp produtoErp, ModelDoc2 swModel, List<OperacaoEng> engenhariaOperacoes) {
      if (engenhariaOperacoes != null && engenhariaOperacoes.Count > 0) {
        // verificar alteração de operações no erp ou solid
        foreach (var operacao in engenhariaOperacoes) {
          var op = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == operacao.codOperacao && x.mascaraMaquina == operacao.codMascaraMaquina);
          var minhasOps = produtoErp.Operacao.Split('^');

          if (op != null && !minhasOps.Contains(op.codOperacao.ToString())) {
            produtoErp.Operacao += !string.IsNullOrEmpty(produtoErp.Operacao) ? $"^{op.codOperacao}" : $"{op.codOperacao}";
            AtualizarOperacao(produtoErp, swModel);
          } else if (op == null) {
            var opCad = Processo.ListaOperacoesERP.FirstOrDefault(x => x.codOperacao == operacao.codOperacao);
            var maCad = Processo.ListaMaquinasERP.FirstOrDefault(x => x.mascara == operacao.codMascaraMaquina);
            if (opCad == null || maCad == null)
              continue;

            if (MsgBox.Show(
                $"Operação: {opCad.codOperacao} - {opCad.descricao}\r\nr\n" +
                $"Operação informada no ERP não consta no Axion.\r\nDeseja Cadastrar a mesma?",
                "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

              var process = new processos {
                codigo_operacao = opCad.codOperacao,
                codigo_maquina = maCad.codMaquina,
              };

              db.processos.Add(process);
              db.SaveChanges();

              await Processo.Carregar();

              op = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == opCad.codOperacao && x.codMaquina == maCad.codMaquina);
              if (op != null) {
                // atualizar props
                if (!minhasOps.Contains(op.codOperacao.ToString())) {
                  produtoErp.Operacao += !string.IsNullOrEmpty(produtoErp.Operacao) ? $"^{op.codOperacao}" : $"{op.codOperacao}";
                  AtualizarOperacao(produtoErp, swModel);
                }
              }

            } else {
              AdicionarPendencia(produtoErp, PendenciasEngenharia.OperacaoRevisar);
            }
          }
          minhasOps = produtoErp.Operacao.Split('^');
          foreach (var opTeste in minhasOps) {
            if (int.TryParse(opTeste, out int codOperacao)) {
              var opExixte = Processo.ListaProcessos.Any(x => x.codOperacao == codOperacao);
              if (!opExixte) {
                AdicionarPendencia(produtoErp, PendenciasEngenharia.OperacaoRevisar);
              }
            }
          }

          if (minhasOps.Count() != engenhariaOperacoes.Count) {
            AdicionarPendencia(produtoErp, PendenciasEngenharia.OperacaoRevisar);
          }
        }
      } else {
        if (!produtoErp.Pendencias.Contains(PendenciasEngenharia.OperacaoRevisar)) {
          AdicionarPendencia(produtoErp, PendenciasEngenharia.OperacaoRevisar);
        }
      }
    }

    private static void AdicionarPendencia(ProdutoErp produtoErp, PendenciasEngenharia pendencia) {
      if (!produtoErp.Pendencias.Contains(pendencia)) {
        produtoErp.Pendencias.Add(pendencia);
        produtoErp.ImgPendencia = Properties.Resources.warning;
      }
    }

    private static void AtualizarOperacao(ProdutoErp produtoErp, ModelDoc2 swModel) {
      if (swModel.IsOpenedReadOnly()) {
        AdicionarPendencia(produtoErp, PendenciasEngenharia.SomenteLeitura);
        return;
      }
      var swModelDocExt = swModel.Extension;
      var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

      if (produtoErp.Referencia.StartsWith("Item da lista de corte")) {
        produtoErp.ItensCorte[0].CodProduto = produtoErp.CodProduto;
        bool boolstatus = swModel.Extension.SelectByID2(produtoErp.ItensCorte[0].NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

        SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
        Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
        swCustPropMgr = swFeat.CustomPropertyManager;
      }

      swCustPropMgr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.Operacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
      swModel.Save();
    }

    #endregion

    //private static async Task AnalisarProdutoAsync(ContextoDados db, ProdutoErp item) {
    //  try {
    //    if (string.IsNullOrEmpty(item.CodProduto)) {
    //      var prod = db.produto_erp.FirstOrDefault(x => x.name == item.Name && x.referencia == item.Referencia && x.configuracao == item.Configuracao);
    //      if (prod != null) {
    //        item.CadastrarErp = item.CadastrarAddin = false;
    //        item.CodProduto = prod.codigo_produto.ToString();
    //        atualizar props
    //        int status = 0;
    //        int warnings = 0;
    //        int tipo = item.PathName.EndsWith("SLDASM")
    //        ? (int)swDocumentTypes_e.swDocASSEMBLY
    //        : (int)swDocumentTypes_e.swDocPART;
    //        var swModel = Sw.App.OpenDoc6(item.PathName, tipo,
    //          (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

    //        var swModelDocExt = swModel.Extension;
    //        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager(item.Configuracao);

    //        if (item.Referencia.StartsWith("Item da lista de corte")) {
    //          item.ItensCorte[0].CodProduto = item.CodProduto;
    //          bool boolstatus = swModel.Extension.SelectByID2(item.ItensCorte[0].NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

    //          SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
    //          Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
    //          swCustPropMgr = swFeat.CustomPropertyManager;
    //        }

    //        swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, item.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
    //        swModel.Save();

    //        if (i > 0)
    //          Sw.App.CloseDoc(item.PathName);
    //      } else {
    //        item.CadastrarErp = !item.CodComponente.StartsWith("10") && !item.CodComponente.StartsWith("20") && !item.CodComponente.StartsWith("30") && !item.CodComponente.StartsWith("40");

    //        if (item.CodComponente.StartsWith("10") || item.CodComponente.StartsWith("20") || item.CodComponente.StartsWith("30") || item.CodComponente.StartsWith("40")) {
    //          item.CodProduto = item.CodComponente;
    //        }

    //        item.CadastrarAddin = true;
    //      }
    //    } else {
    //      var cod = Convert.ToInt64(item.CodProduto);
    //      if (db.produto_erp.Any(x => x.codigo_produto == cod && x.name == item.Name && x.referencia == item.Referencia && x.configuracao == item.Configuracao)) {
    //        item.CadastrarErp = item.CadastrarAddin = false;
    //      } else {
    //        if (!item.CodComponente.StartsWith("10") && !item.CodComponente.StartsWith("20") && !item.CodComponente.StartsWith("40")) {
    //          if (!item.Referencia.StartsWith("Item da lista de corte")) {
    //            var itemERP = await Api.GetItemGenericoAsync(item.CodProduto);

    //            item.CadastrarErp = itemERP == null || (item.Referencia != itemERP.refTecnica && !itemERP.nome.EndsWith(item.Referencia));

    //            if (item.CodProduto.StartsWith("40") && item.CadastrarErp)
    //              throw new Exception($"Código Produto {item.CodProduto} inválido para esta montagem:\r\n{item.Name} - {item.Denominacao}");
    //          }
    //        } else {
    //          item.CadastrarErp = false;
    //        }

    //        item.CadastrarAddin = true;
    //      }
    //    }

    //    if (item.CadastrarErp)
    //      item.CodProduto = string.Empty;

    //  } catch (Exception ex) {
    //    throw new Exception(ex.Message);
    //    LmException.ShowException(ex, "Erro ao analisar produto");
    //  }
    //}

  }
}
