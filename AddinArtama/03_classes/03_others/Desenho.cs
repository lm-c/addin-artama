using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AddinArtama {
  public class Desenho {
    [DisplayName("TP")]
    public string Tipo3D { get; set; }

    [DisplayName("DS")]
    public string TemDesenho { get; set; }

    [DisplayName("COMPONENTE")]
    public string ShortName { get; set; }

    [DisplayName("DENOMINAÇÃO")]
    public string Denominacao { get; set; }

    [Browsable(false)]
    public string CompCodigo { get; set; }

    [Browsable(false)]
    public string PathName { get; set; }

    static List<Desenho> listaDesenhos;
    static Desenho desenho;

    public static List<Desenho> GetDesenhos(ModelDoc2 swModel) {
      listaDesenhos = new List<Desenho>();

      try {

        ConfigurationManager swConfMgr;
        Configuration swConf;

        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        var swModelDocExt = swModel.Extension;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          // Inserir lista de material e pegar dados
          string templateGeral = templates.model.lista_montagem;
          int BomTypeGeral = (int)swBomType_e.swBomType_Indented;
          int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
          var swBOMAnnotationGeral = swModelDocExt.InsertBomTable3(templateGeral, 0, 1, BomTypeGeral, swConf.Name, Hidden: false, NumberingType, DetailedCutList: false);
          PegaDadosListaGeral(swBOMAnnotationGeral, listaDesenhos);
          ListaCorte.ExcluirLista(swModel);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar desenhos\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return listaDesenhos.OrderBy(x => x.Tipo3D).ThenBy(x => x.CompCodigo).ToList();
    }

    private static void PegaDadosListaGeral(BomTableAnnotation swBOMAnnotation, List<Desenho> listaDesenhos) {
      string nameShort = "";
      try {
        string[] vModelPathNames = null;
        string strItemNumber = null;
        string strPartNumber = null;
        var swTableAnnotation = (TableAnnotation)swBOMAnnotation;

        int lStartRow = swTableAnnotation.TotalRowCount - 1;

        var swBOMFeature = swBOMAnnotation.BomFeature;

        for (int i = 0; i < swTableAnnotation.TotalRowCount; i++) {
          vModelPathNames = (string[])swBOMAnnotation.GetModelPathNames(i, out strItemNumber, out strPartNumber);

          if (vModelPathNames != null) {
            var desenho = new Desenho();
            desenho.PathName = vModelPathNames[0];

            if (string.IsNullOrEmpty(swTableAnnotation.get_Text(i, 2).Trim()))
              continue;

            if (desenho.PathName.ToUpper().Contains("BIBLIOTECA") || desenho.PathName.ToUpper().Contains("ESQ"))
              continue;

            if (int.TryParse(swTableAnnotation.get_Text(i, 1).Trim(), out int qtd)) {
              string PathNameDesenho = desenho.PathName.Substring(0, desenho.PathName.Length - 6) + "SLDDRW";

              if (desenho.PathName.ToUpper().EndsWith("SLDASM"))
                desenho.Tipo3D = "M";
              else if (desenho.PathName.ToUpper().EndsWith("SLDPRT"))
                desenho.Tipo3D = "P";

              if (File.Exists(PathNameDesenho))
                desenho.TemDesenho = "Sim";
              else
                desenho.TemDesenho = "Não";

              desenho.ShortName = Path.GetFileNameWithoutExtension(PathNameDesenho);
              desenho.Denominacao = swTableAnnotation.get_Text(i, 3).Trim();

              if (!listaDesenhos.Any(x => x.ShortName == desenho.ShortName))
                listaDesenhos.Add(desenho);
            }
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista Pack List\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public static void InsertMaterialsList(ModelDoc2 swModel, int? posicaoListaDesejada = null) {
      string modelPath = swModel.GetPathName().ToLower();

      if (File.Exists(modelPath.Replace("slddrw", "sldprt"))) {
        InserirListasMateriais(swDocumentTypes_e.swDocPART, posicaoListaDesejada);
      } else {
        InserirListasMateriais(swDocumentTypes_e.swDocASSEMBLY);
      }
    }

    public static void InserirListasMateriais(swDocumentTypes_e swDocumentTypes_E, int? posicaoListaDesejada = null) {
      try {
        DrawingDoc swDraw = default(DrawingDoc);
        swDraw = (DrawingDoc)Sw.App.ActiveDoc;

        BomTableAnnotation swBOMAnnotation = default(BomTableAnnotation);
        WeldmentCutListAnnotation WMTable = default(WeldmentCutListAnnotation);

        SolidWorks.Interop.sldworks.View swView = (SolidWorks.Interop.sldworks.View)swDraw.GetFirstView();
        swView = (SolidWorks.Interop.sldworks.View)swView.GetNextView();
        swDraw.ActivateView(swView.GetName2());

        // Obtém a configuração ativa
        ModelDoc2 swModel2 = (ModelDoc2)swView.ReferencedDocument;
        string activeConfiguration = swModel2.GetActiveConfiguration().Name;

        int AnchorType = (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomRight;
        int BomType = (int)swBomType_e.swBomType_TopLevelOnly;

        if (swDocumentTypes_E == swDocumentTypes_e.swDocPART) {
          WMTable = swView.InsertWeldmentTable(true, 0, 0, AnchorType, "", templates.model.lista_soldagem);
          if (posicaoListaDesejada.HasValue && WMTable != null) {
            var table = (TableAnnotation)WMTable;
            int rowCount = table.RowCount;

            List<int> linhasParaExcluir = new List<int>();

            for (int row = 0; row < rowCount; row++) {
              string cellValue = table.get_Text(row, 0); // Coluna 0 = posição

              if (int.TryParse(cellValue, out int posicaoAtual) && posicaoAtual != posicaoListaDesejada.Value) {
                linhasParaExcluir.Add(row);
              }
            }

            // 🔥 Ordem decrescente evita o shift dos índices!
            foreach (var row in linhasParaExcluir.OrderByDescending(r => r)) {
              table.DeleteRow(row);
            }

            // Após exclusões, atualiza a célula [0,0] com a posição correta
            if (table.RowCount > 0) {
              table.set_Text(0, 0, posicaoListaDesejada.Value.ToString());
            }
          }
        } else {
          swBOMAnnotation = swView.InsertBomTable3(true, 0, 0, AnchorType, BomType, activeConfiguration, templates.model.lista_montagem, false);

          if(posicaoListaDesejada.HasValue && WMTable != null) {
            var table = (TableAnnotation)WMTable;
            int rowCount = table.RowCount;

            List<int> linhasParaExcluir = new List<int>();

            for (int row = 0; row < rowCount; row++) {
              string cellValue = table.get_Text(row, 0); // Coluna 0 = posição

              if (int.TryParse(cellValue, out int posicaoAtual) && posicaoAtual != posicaoListaDesejada.Value) {
                linhasParaExcluir.Add(row);
              }
            }

            // 🔥 Ordem decrescente evita o shift dos índices!
            foreach (var row in linhasParaExcluir.OrderByDescending(r => r)) {
              table.DeleteRow(row);
            }

            // Após exclusões, atualiza a célula [0,0] com a posição correta
            if (table.RowCount > 0) {
              table.set_Text(0, 0, posicaoListaDesejada.Value.ToString());
            }
          }
        }
      } catch (Exception ex) {
        //MsgBox.Show($"Erro ao inserir Lista\n\n{ex.Message}", "Addin LM Projetos",
        //     MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public static void ClearExistingTables(ModelDoc2 swModel, out bool hasExistingTable) {
      swModel.ClearSelection2(true);
      hasExistingTable = false;

      if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING)
        return;

      List<Feature> featuresToDelete = new List<Feature>();

      Feature swFeature = (Feature)swModel.FirstFeature();

      while (swFeature != null) {
        string featureType = swFeature.GetTypeName2();

        if (featureType == "BomFeat" || featureType == "WeldmentTableFeat") {
          featuresToDelete.Add(swFeature);
        }

        if (featureType == "DrSheet") {
          featuresToDelete.AddRange(GetSheetTables(swFeature));
        }

        swFeature = swFeature.GetNextFeature();
      }

      foreach (var feat in featuresToDelete) {
        hasExistingTable = true;
        feat.Select2(false, -1);
        swModel.EditDelete();
      }
    }


    private static List<Feature> GetSheetTables(Feature sheetFeature) {
      var list = new List<Feature>();
      Feature subFeature = sheetFeature.GetFirstSubFeature();

      while (subFeature != null) {
        string subType = subFeature.GetTypeName2();
        if (subType == "GeneralTableFeature") {
          list.Add(subFeature);
        }

        subFeature = subFeature.GetNextSubFeature();
      }

      return list;
    }

    private static bool ProcessSheetSubFeatures(ModelDoc2 swModel, Feature sheetFeature) {
      Feature subFeature = sheetFeature.GetFirstSubFeature();
      bool foundTable = false;

      while (subFeature != null) {
        string subFeatureType = subFeature.GetTypeName2();

        if (subFeatureType == "GeneralTableFeature") {
          foundTable = true;
          subFeature.Select(true);
          swModel.EditDelete();
        }

        subFeature = (Feature)subFeature.GetNextSubFeature();
      }

      return foundTable;
    }

    public static SwDwgPaperSizes_e GetFormat(double largura, double altura) {
      if (largura > 200 && largura < 215 && altura > 290 && altura < 305)
        return SwDwgPaperSizes_e.A4R;
      else if (largura > 290 && largura < 305 && altura > 200 && altura < 215)
        return SwDwgPaperSizes_e.A4P;
      else if (largura > 415 && largura < 425 && altura > 290 && altura < 305)
        return SwDwgPaperSizes_e.A3;
      else if (largura > 590 && largura < 600 && altura > 415 && altura < 425)
        return SwDwgPaperSizes_e.A2;
      else if (largura > 835 && largura < 845 && altura > 590 && altura < 600)
        return SwDwgPaperSizes_e.A1;
      else if (largura > 1180 && largura < 1195 && altura > 835 && altura < 845)
        return SwDwgPaperSizes_e.A0;

      return SwDwgPaperSizes_e.A4R;
    }

  }
}
