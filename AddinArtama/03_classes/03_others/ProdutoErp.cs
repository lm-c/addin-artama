using LmCorbieUI;
using LmCorbieUI.Metodos;
using LmCorbieUI.Metodos.AtributosCustomizados;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddinArtama {
  internal class ProdutoErp {

    [DisplayName("Nível")]
    [LarguraColunaGrid(60)]
    public string Nivel { get; set; }

    [DisplayName("Componente")]
    [LarguraColunaGrid(120)]
    public string CodComponente { get; set; }

    [DisplayName("Código produto")]
    [LarguraColunaGrid(120)]
    public string CodProduto { get; set; }

    [DisplayName("DENOMINAÇÃO")]
    [LarguraColunaGrid(350)]
    public string Denominacao { get; set; }

    [DisplayName("Referência")]
    [LarguraColunaGrid(150)]
    public string Referencia { get; set; }

    [DisplayName("QTD")]
    [LarguraColunaGrid(50)]
    public int Quantidade { get; set; }

    [Browsable(false)]
    public string Name { get; set; }

    [Browsable(false)]
    public string pathName { get; set; }

    [Browsable(false)]
    public TipoComponente TipoComponente { get; set; }

    public static SortableBindingList<ProdutoErp> GetComponents() {
      var _listaProduto = new List<ProdutoErp>();

      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        ModelDocExtension swModelDocExt;
        CustomPropertyManager swCustPropMngr = default(CustomPropertyManager);

        var PathName = swModel.GetPathName();

        ConfigurationManager swConfMgr;
        Configuration swConf;
        Component2 swRootComp;

        swModelDocExt = swModel.Extension;
        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        swRootComp = swConf.GetRootComponent3(true);

        ProdutoErp produtoErp = new ProdutoErp();
        produtoErp.Nivel = "0";
        produtoErp.pathName = PathName;
        produtoErp.Name = Path.GetFileNameWithoutExtension(PathName);
        produtoErp.Referencia = produtoErp.Name;
        produtoErp.TipoComponente = TipoComponente.Montagem;

        string valOut;
        string resolvedValOut;

        swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");
        swCustPropMngr.Get2("Denominação", out valOut, out resolvedValOut);
        produtoErp.Denominacao = resolvedValOut;
        swCustPropMngr.Get2("Componente", out valOut, out resolvedValOut);
        produtoErp.CodComponente = resolvedValOut;
        swCustPropMngr.Get2("Código Produto", out valOut, out resolvedValOut);
        produtoErp.CodProduto = resolvedValOut;
        produtoErp.Quantidade = 1;

        _listaProduto.Add(produtoErp);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          // Inserir lista de material e pegar dados
          string templateGeral = $"{Application.StartupPath}\\01 - Addin TGM 4.0\\ListaCompleta.sldbomtbt";
          int BomTypeGeral = (int)swBomType_e.swBomType_Indented;
          int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
          bool DetailedCutList = false;
          var swBOMAnnotationGeral = swModelDocExt.InsertBomTable3(templateGeral, 0, 1, BomTypeGeral, swConf.Name, false, NumberingType, DetailedCutList);
          PegaDadosListaGeral(swBOMAnnotationGeral, _listaProduto);
          ListaCorte.ExcluirLista(swModel);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar desenho\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return new SortableBindingList<ProdutoErp>(_listaProduto);
    }

    private static void PegaDadosListaGeral(BomTableAnnotation swBOMAnnotation, List<ProdutoErp> _listaProduto) {
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

        for (int i = lStartRow; i < swTableAnnotation.TotalRowCount; i++) {
          vModelPathNames = (string[])swBOMAnnotation.GetModelPathNames(i, out strItemNumber, out strPartNumber);

          if (vModelPathNames != null) {
            var produtoErp = new ProdutoErp();
            string ptNm = vModelPathNames[0];

            produtoErp.pathName = ptNm;
            produtoErp.Name = Path.GetFileNameWithoutExtension(produtoErp.pathName);

            if (!File.Exists(produtoErp.pathName))
              continue;

            //if ( _listaProduto.Any(x => x.NomeComponente == produtoErp.NomeComponente)) {
            //  var item = _listaProduto.Where(x => x.NomeComponente == produtoErp.NomeComponente).FirstOrDefault();
            //  item.Quantidade += qtd;
            //  continue;
            //}

            produtoErp.TipoComponente = ptNm.ToUpper().EndsWith("SLDPRT") ? TipoComponente.Peca : TipoComponente.Montagem;

            produtoErp.Nivel = swTableAnnotation.get_Text(i, 0).Trim();
            produtoErp.Quantidade = Convert.ToInt32(swTableAnnotation.get_Text(i, 1)); 
            produtoErp.CodComponente = swTableAnnotation.get_Text(i, 3).Trim();
            produtoErp.CodProduto = swTableAnnotation.get_Text(i, 4).Trim();
            produtoErp.Denominacao = swTableAnnotation.get_Text(i, 6);
            _listaProduto.Add(produtoErp);
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }
}
