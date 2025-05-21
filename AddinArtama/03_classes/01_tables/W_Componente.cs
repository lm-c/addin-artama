using LmCorbieUI.Metodos.AtributosCustomizados;
using LmCorbieUI.Metodos;
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
using System.Data.Entity.Infrastructure;

namespace AddinArtama {
  internal class W_Componente {
    [DisplayName("NOME")]
    [LarguraColunaGrid(120)]
    public string NomeComponente { get; set; }

    [LarguraColunaGrid(80)]
    [DisplayName("QTD")]
    [AlinhamentoColunaGrid(DataGridViewContentAlignment.MiddleCenter)]
    public int Quantidade { get; set; }

    [LarguraColunaGrid(350)]
    [DisplayName("DESCRIÇÃO")]
    public string Denominacao { get; set; }

    [Browsable(false)]
    public string PathName { get; set; }

    #region Metodos

    public static SortableBindingList<W_Componente> GetComponentes(ModelDoc2 swModel) {
      var listaComponentes = new SortableBindingList<W_Componente>();

      try {
        swModel = (ModelDoc2)Sw.App.ActiveDoc;
        object[] AtiveConfiguration = null;
        string valOut;
        string resolvedValOut;
        double[] massProp;
        var swModelDocExt = swModel.Extension;

        swModel.Rebuild((int)swRebuildOptions_e.swRebuildAll);
        //swModel.Rebuild((int)swRebuildOptions_e.swForceRebuildAll);

        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        ConfigurationManager swConfMgr;
        Configuration swConf;

        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        AtiveConfiguration = (object[])swModel.GetConfigurationNames();

        var componente = new W_Componente();
        swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
        componente.Denominacao = resolvedValOut;
        componente.PathName = swModel.GetPathName();
        componente.NomeComponente = Path.GetFileNameWithoutExtension(componente.PathName);
        componente.Quantidade = 1;

        var directoryPath = Path.GetDirectoryName(componente.PathName);
        var backupPrefix = "~$";

        string[] files = Directory.GetFiles(directoryPath);

        foreach (string file in files.Where(x => Path.GetFileNameWithoutExtension(x).StartsWith(backupPrefix))) {
          //if (Path.GetFileName(file).StartsWith(backupPrefix)) {
          try {
            File.Delete(file);
            Console.WriteLine($"Arquivo excluído: {file}");
          } catch (Exception ex) {
            Console.WriteLine($"Erro ao excluir o arquivo {file}: {ex.Message}");
          }
          //}
        }

        listaComponentes.Add(componente);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          string templateGeral = $"{Application.StartupPath}\\01 - Addin LM\\ListaCompleta.sldbomtbt";
          int BomTypeGeral = (int)swBomType_e.swBomType_Indented;
          int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
          bool DetailedCutList = false;
          var swBOMAnnotationGeral = swModelDocExt.InsertBomTable3(templateGeral, 0, 1, BomTypeGeral, swConf.Name, false, NumberingType, DetailedCutList);

          PegaDadosLista(swBOMAnnotationGeral, listaComponentes);
          ListaCorte.ExcluirLista(swModel);
        } 
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar componentes\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return listaComponentes;
    }

    private static void PegaDadosLista(BomTableAnnotation swBOMAnnotation, SortableBindingList<W_Componente> listaComponentes) {
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
            var componente = new W_Componente();
            string ptNm = vModelPathNames[0];

            componente.PathName = ptNm;
            componente.NomeComponente = Path.GetFileNameWithoutExtension(ptNm).ToUpper();

            if (string.IsNullOrEmpty(componente.NomeComponente))
              continue;

            if (int.TryParse(swTableAnnotation.get_Text(i, 1).Trim(), out int qtd)) {
              componente.Quantidade = qtd;
              componente.Denominacao = swTableAnnotation.get_Text(i, 6).Trim();

              //bool isSheetMetal = IsSheetMetal(Sw.App, componente.PathName, fecharPeca: true);
              if (!listaComponentes.Any(x => x.NomeComponente == componente.NomeComponente))
                listaComponentes.Add(componente);
              else
                listaComponentes.FirstOrDefault(x => x.NomeComponente == componente.NomeComponente).Quantidade += componente.Quantidade;
            }
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion
  }
}
