using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddinArtama {
  internal class DrawExport {
    [Browsable(false)]
    public int IndexTree { get; set; }

    [DisplayName("Sel")]
    public bool Exportar { get; set; }

    [DisplayName("COMPONENTE")]
    public string CodComponente { get; set; }

    [DisplayName("DENOMINAÇÃO")]
    public string Denominacao { get; set; }

    [Browsable(false)]
    public string PathName { get; set; }

    [Browsable(false)]
    public swDocumentTypes_e Tipo { get; set; }

    public static List<DrawExport> GetDrawing() {
      var listaDraw = new List<DrawExport>();
      string PathNameDraw = string.Empty;

      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        try {
          PathNameDraw = swModel.GetPathName().ToUpper().Replace("SLDASM", "SLDDRW");
        } catch (Exception ex) {
          LmException.ShowException(ex, $"E-DE-001:\r\n{ex.Message}");
        }

        ConfigurationManager swConfMgr;
        Configuration swConf;
        Component2 swRootComp;

        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        swRootComp = swConf.GetRootComponent3(true);

        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        var drawExport = new DrawExport();

        if (File.Exists(PathNameDraw)) {
          drawExport.IndexTree = 0;
          drawExport.Exportar = true;
          drawExport.PathName = PathNameDraw;
          //drawExport.CodComponente = "000 - " + DadosArtama.GetShortName(PathNameDraw); 

          string valOut;
          string resolvedValOut;

          try {
            swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
            drawExport.Denominacao = resolvedValOut;
            swCustPropMgr.Get2("Componente", out valOut, out resolvedValOut);
            drawExport.CodComponente = "001 - " + resolvedValOut;
            drawExport.Tipo = swDocumentTypes_e.swDocASSEMBLY;
          } catch (Exception ex) {
            LmException.ShowException(ex, $"E-DE-003:\r\n{ex.Message}");
          }


          try {
            listaDraw.Add(drawExport);
          } catch (Exception ex) {
            LmException.ShowException(ex, $"E-DE-004:\r\n{ex.Message}");
          }

        }
        string lista = string.Empty;

        // Inserir lista de material e pegar dados
        try {
          lista = $"{Application.StartupPath}\\01 - Addin LM\\ListaCompleta.sldbomtbt";
        } catch (Exception ex) {
          LmException.ShowException(ex, $"E-DE-005:\r\n{ex.Message}");
        }

        try {
          int BomType = (int)swBomType_e.swBomType_Indented;
          int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
          var swBOMAnnotation = swModelDocExt.InsertBomTable3(lista, 0, 1, BomType, swConf.Name, false, NumberingType, DetailedCutList: false);
          PegaDadosLista(swBOMAnnotation, listaDraw);
        } catch (Exception ex) {
          LmException.ShowException(ex, $"E-DE-006:\r\n{ex.Message}");
        }

        try {
          ListaCorte.ExcluirLista(swModel);
        } catch (Exception ex) {
          LmException.ShowException(ex, $"E-DE-007:\r\n{ex.Message}");
        }

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar desenho\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return listaDraw;
    }

    private static void PegaDadosLista(BomTableAnnotation swBOMAnnotation, List<DrawExport> listaDraw) {
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
          string ptNm = vModelPathNames[0];

          if (string.IsNullOrEmpty(ptNm)) continue;

          var drawExport = new DrawExport();

          drawExport.PathName = ptNm.Substring(0, ptNm.Length - 6) + "SLDDRW";

          if (!File.Exists(drawExport.PathName))
            continue;

          drawExport.Exportar = true;

          try {
            drawExport.CodComponente = swTableAnnotation.get_Text(i, 3);
          } catch (Exception ex) {
            LmException.ShowException(ex, $"E-DE-008:\r\n{ex.Message}");
          }

          //if (string.IsNullOrEmpty(drawExport.CodComponente))
          //  continue;

          try {
            drawExport.Denominacao = swTableAnnotation.get_Text(i, 5);
          } catch (Exception ex) {
            LmException.ShowException(ex, $"E-DE-009:\r\n{ex.Message}");
          }

          try {
            var name = Path.GetFileNameWithoutExtension(ptNm).ToUpper();
            if (!listaDraw.Any(x => Path.GetFileNameWithoutExtension(x.PathName).ToUpper() == name)) {
              drawExport.IndexTree = listaDraw.Count;
              listaDraw.Add(drawExport);
            }
          } catch (Exception ex) {
            LmException.ShowException(ex, $"E-DE-010:\r\n{ex.Message}");
          }

        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista Pack List\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public static string GetFolder(string sufixo, ModelDoc2 swModel = default(ModelDoc2)) {
      string _return = "";

      try {
        ModelDocExtension swModelDocExt;
        CustomPropertyManager swCustPropMgr = default(CustomPropertyManager);

        swModelDocExt = swModel.Extension;

        string valOut;
        string resolvedValOut;

        swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
        swCustPropMgr.Get2("Componente", out valOut, out resolvedValOut);
        string codComponente = resolvedValOut;

        _return = Path.GetDirectoryName(swModel.GetPathName()) + "\\" + codComponente + sufixo + "\\";

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar pasta PDF\n\n{ex.Message}", "Addin LM Projetos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return _return;
    }
  }
}
