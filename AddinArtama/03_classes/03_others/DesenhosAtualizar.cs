using LmCorbieUI.Metodos;
using LmCorbieUI;
using LmCorbieUI.Metodos.AtributosCustomizados;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Google.Protobuf.WellKnownTypes;
using System.Data.Entity.Infrastructure;

namespace AddinArtama {
  public class DesenhosAtualizar {
    [DisplayName("-")]
    [LarguraColunaGrid(40)]
    [AlinhamentoColunaGrid(DataGridViewContentAlignment.MiddleCenter)]
    public bool Atualizar { get; set; }

    [DisplayName("COMPONENTE")]
    [LarguraColunaGrid(110)]
    [AlinhamentoColunaGrid(DataGridViewContentAlignment.MiddleCenter)]
    public string ShortName { get; set; }

    [DisplayName("DENOMINAÇÃO")]
    [LarguraColunaGrid(0)]
    public string Denominacao { get; set; }

    [Browsable(false)]
    public string PathName { get; set; }

    public static SortableBindingList<DesenhosAtualizar> GetDesenhos() {
      List<DesenhosAtualizar> listaDesenhos = new List<DesenhosAtualizar>();

      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        string valOut;
        string resolvedValOut;

        ModelDocExtension swModelDocExt = swModel.Extension;
        CustomPropertyManager swCustPropMngr = default(CustomPropertyManager);
        swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");

        ConfigurationManager swConfMgr = swModel.ConfigurationManager;
        Configuration swConf = swConfMgr.ActiveConfiguration;

        var pathName = swModel.GetPathName();
        string pathNameDraw = pathName.Substring(0, pathName.Length - 6) + "SLDDRW";

        if (File.Exists(pathNameDraw)) {
          var desenho = new DesenhosAtualizar();
          desenho.Atualizar = true;
          swCustPropMngr.Get2("Denominação", out valOut, out resolvedValOut);
          desenho.Denominacao = resolvedValOut;
          desenho.PathName = pathNameDraw;
          desenho.ShortName = Path.GetFileNameWithoutExtension(pathName);

          listaDesenhos.Add(desenho);
        }

        // Inserir lista de material e pegar dados
        string templateGeral = $"{Application.StartupPath}\\01 - Addin LM\\ListaCompleta.sldbomtbt";
        int BomTypeGeral = (int)swBomType_e.swBomType_Indented;
        int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
        bool DetailedCutList = true;
        var swBOMAnnotationGeral = swModelDocExt.InsertBomTable3(templateGeral, 0, 1, BomTypeGeral, swConf.Name, false, NumberingType, DetailedCutList);
        PegaDadosListaGeral(swBOMAnnotationGeral, listaDesenhos);
        ListaCorte.ExcluirLista(swModel);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar desenhos\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return new SortableBindingList<DesenhosAtualizar>(listaDesenhos);
    }

    private static void PegaDadosListaGeral(BomTableAnnotation swBOMAnnotation, List<DesenhosAtualizar> listaDesenhos) {
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

            var pathName = vModelPathNames[0];
            string pathNameDraw = pathName.Substring(0, pathName.Length - 6) + "SLDDRW";
            nameShort = Path.GetFileNameWithoutExtension(pathName);

            if (File.Exists(pathNameDraw) && !listaDesenhos.Any(x => x.ShortName == nameShort)) {
              var desenho = new DesenhosAtualizar();
              desenho.Atualizar = true;
              desenho.Denominacao = swTableAnnotation.get_Text(i, 6).Trim();
              desenho.PathName = pathNameDraw;
              desenho.ShortName = nameShort;

              listaDesenhos.Add(desenho);
            }
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista Pack List\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }
}
