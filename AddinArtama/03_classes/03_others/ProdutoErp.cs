using LmCorbieUI;
using LmCorbieUI.Metodos;
using LmCorbieUI.Metodos.AtributosCustomizados;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
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

    [Browsable(false)]
    [DisplayName("Nível")]
    [LarguraColunaGrid(60)]
    public string Nivel { get; set; }

    [DisplayName("Nome Componente")]
    [LarguraColunaGrid(120)]
    public string Name { get; set; }

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
    public string PathName { get; set; }

    [Browsable(false)]
    public TipoComponente TipoComponente { get; set; }

    [Browsable(false)]
    public bool CadastrarErp { get; set; }

    [Browsable(false)]
    public bool CadastrarAddin { get; set; }

    public List<ListaCorte> ItensCorte = new List<ListaCorte>();

    public static SortableBindingList<ProdutoErp> GetComponents(TreeView treeView) {
      var _listaProduto = new List<ProdutoErp>();

      try {
        MsgBox.ShowWaitMessage("Lendo componentes...");

        treeView.Nodes.Clear();

        ImageList il = new ImageList();
        il.Images.Add(0.ToString(), Properties.Resources.assembly);
        il.Images.Add(1.ToString(), Properties.Resources.part);
        il.Images.Add(2.ToString(), Properties.Resources.cutlist);

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
          TipoComponente = tipo == (int)swDocumentTypes_e.swDocASSEMBLY ? TipoComponente.Montagem : TipoComponente.Peca,
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

        swCustPropMngr = swModelDocExt.get_CustomPropertyManager(swConf.Name);

        if (string.IsNullOrEmpty(produtoErp.CodComponente)) {
          swCustPropMngr.Get2("Componente", out valOut, out resolvedValOut);
          produtoErp.CodComponente = resolvedValOut;
        }

        if (string.IsNullOrEmpty(produtoErp.Denominacao)) {
          swCustPropMngr.Get2("Denominação", out valOut, out resolvedValOut);
          produtoErp.Denominacao = resolvedValOut;
        }

        var fantasma = string.Empty;
        if (string.IsNullOrEmpty(produtoErp.Denominacao)) {
          swCustPropMngr.Get2("Fantasma", out valOut, out resolvedValOut);
          fantasma = resolvedValOut;
        }

        swCustPropMngr.Get2("Código Produto", out valOut, out resolvedValOut);
        produtoErp.CodProduto = resolvedValOut;

        if (string.IsNullOrEmpty(produtoErp.CodProduto) &&
          (produtoErp.CodComponente.StartsWith("10") || produtoErp.CodComponente.StartsWith("20") || produtoErp.CodComponente.StartsWith("30") || produtoErp.CodComponente.StartsWith("40"))) {
          swCustPropMngr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodComponente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
          produtoErp.CodProduto = produtoErp.CodComponente;
        }

        produtoErp.Quantidade = 1;

        produtoErp.Configuracao = swConf.Name;

        _listaProduto.Add(produtoErp);

        // Engenharia de produto
        TreeNode rootNode = treeView.Nodes.Add("Root", $"{produtoErp.Name} - {produtoErp.Denominacao}");
        rootNode.Tag = produtoErp;

        if (tipo == (int)swDocumentTypes_e.swDocASSEMBLY) {
          // Inserir lista de material e pegar dados
          string templateGeral = $"{Application.StartupPath}\\01 - Addin LM\\ListaCompleta.sldbomtbt";
          int BomTypeGeral = (int)swBomType_e.swBomType_Indented;
          int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
          bool DetailedCutList = false;
          var swBOMAnnotationGeral = swModelDocExt.InsertBomTable3(templateGeral, 0, 1, BomTypeGeral, swConf.Name, false, NumberingType, DetailedCutList);
          PegaDadosListaGeral(swBOMAnnotationGeral, _listaProduto, rootNode);
          ListaCorte.ExcluirLista(swModel);

          rootNode.ExpandAll();
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

          if ((produtoErp.CodComponente.StartsWith("10") ||
            produtoErp.CodComponente.StartsWith("20") ||
            produtoErp.CodComponente.StartsWith("30") ||
            produtoErp.CodComponente.StartsWith("40")) &&
            string.IsNullOrEmpty(produtoErp.CodProduto)) {
            produtoErp.CodProduto = produtoErp.CodComponente;
            swModelDocExt = swModel.Extension;
            swConfMgr = swModel.ConfigurationManager;
            swConf = swConfMgr.ActiveConfiguration;
            swCustPropMngr = swModelDocExt.get_CustomPropertyManager(swConf.Name);
            swCustPropMngr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodComponente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

            swModel.Save();
          }

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
                ItensCorte = new List<ListaCorte> { itemCorte }
              };

              if (produtoErp.Name.Length + sufixo.Length > 50)
                produtoErp.Name = produtoErp.Name.Substring(0, 50 - sufixo.Length) + sufixo;

              _listaProduto.Add(item);
            }
          }
        }

        List<ProdutoErp> listaExclusao = new List<ProdutoErp>();
        foreach (var item in _listaProduto.Where(x => x.CodComponente.StartsWith("10") && x.TipoComponente == TipoComponente.Montagem)) {
          var nivel = item.Nivel + ".";

          _listaProduto.Where(x => x.Nivel.StartsWith(nivel)).ToList().ForEach(x => {
            listaExclusao.Add(x);
          });
        }
        if (listaExclusao.Count > 0)
          _listaProduto.RemoveAll(x => listaExclusao.Contains(x));

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar lista produtos\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return new SortableBindingList<ProdutoErp>(_listaProduto);
    }

    private static void PegaDadosListaGeral(BomTableAnnotation swBOMAnnotation, List<ProdutoErp> _listaProduto, TreeNode rootNode) {
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
            produtoErp.Referencia = produtoErp.Name;

            int swTipo = produtoErp.TipoComponente == TipoComponente.Peca ? (int)swDocumentTypes_e.swDocPART : (int)swDocumentTypes_e.swDocASSEMBLY;

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
            produtoErp.Img3D = produtoErp.TipoComponente == TipoComponente.Peca ? Properties.Resources.part : Properties.Resources.assembly;
            produtoErp.Img2D = desenhoExiste ? Properties.Resources.draw : Properties.Resources.not_draw;

            if ((produtoErp.CodComponente.StartsWith("10") || produtoErp.CodComponente.StartsWith("20") || produtoErp.CodComponente.StartsWith("30") || produtoErp.CodComponente.StartsWith("40")) && string.IsNullOrEmpty(produtoErp.CodProduto)) {
              produtoErp.CodProduto = produtoErp.CodComponente;
              var swModelDocExt = swModel.Extension;
              var swConfMgr = swModel.ConfigurationManager;
              var swConf = swConfMgr.ActiveConfiguration;
              var swCustPropMngr = swModelDocExt.get_CustomPropertyManager(swConf.Name);
              swCustPropMngr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodComponente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

              swModel.Save();
            }

            Sw.App.CloseDoc(produtoErp.PathName);

            // engenharia de produto
            CreateTreeCompNode(rootNode, nodes, produtoErp, nivel);

            if (!_listaProduto.Any(x => x.Name == produtoErp.Name && x.Configuracao == produtoErp.Configuracao))
              _listaProduto.Add(produtoErp);

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
                  ItensCorte = new List<ListaCorte> { itemCorte }
                };

                if (produtoErp.Name.Length + sufixo.Length > 50)
                  produtoErp.Name = produtoErp.Name.Substring(0, 50 - sufixo.Length) + sufixo;

                CreateTreeCompNode(rootNode, nodes, item, nivel + "." + indiceNome);

                if (!_listaProduto.Any(x => x.Name == item.Name && x.Configuracao == item.Configuracao && x.Referencia == item.Referencia))
                  _listaProduto.Add(item);
              }
            }
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

      var iconIndex = produtoErp.TipoComponente == TipoComponente.Montagem || produtoErp.ItensCorte.Count > 1 ? 0 : 1;

      node.ImageIndex = iconIndex;
      node.SelectedImageIndex = iconIndex;

      if(produtoErp.TipoComponente == TipoComponente.Peca && produtoErp.ItensCorte.Count == 1 && 
        !produtoErp.CodComponente.StartsWith("10") && 
        !produtoErp.CodComponente.StartsWith("20") && 
        !produtoErp.CodComponente.StartsWith("40")) { 
        var itemCorte = produtoErp.ItensCorte.FirstOrDefault();

        var produtoFilho = new ProdutoErp {
         PathName = produtoErp.PathName,
         Name = produtoErp.Name,
         Denominacao = itemCorte.Denominacao,
         Referencia = produtoErp.Referencia,
         CodComponente = itemCorte.Codigo.ToString(),
         CodProduto = itemCorte.CodProduto,
         TipoComponente = TipoComponente.ListaMaterial,
         Nivel = produtoErp.Nivel + ".1",
         Configuracao = produtoErp.Configuracao,
         Quantidade = itemCorte.Quantidade
        };
        string nodeTextFilho = $"{produtoFilho.CodComponente} - {produtoFilho.Denominacao}";
        var nodeFilho = new TreeNode(nodeTextFilho);
        nodeFilho.Tag = produtoFilho;
        nodeFilho.ImageIndex = 2;
        nodeFilho.SelectedImageIndex = 2;

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
  }
}
