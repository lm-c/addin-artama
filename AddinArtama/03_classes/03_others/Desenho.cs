using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

      return listaDesenhos.OrderBy(x=>x.Tipo3D).ThenBy(x=>x.CompCodigo).ToList();
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
