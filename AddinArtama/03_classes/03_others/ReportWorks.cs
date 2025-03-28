using LmCorbieUI.Metodos;
using LmCorbieUI;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddinArtama {
  internal class ReportWorks {
    [DisplayName("Nivel")]
    public string Nivel { get; set; }

    [DisplayName("Qtd")]
    public string Qtd { get; set; }

    [DisplayName("Código")]
    public string Codigo { get; set; }

    [DisplayName("Componente")]
    public string Componente { get; set; }

    [DisplayName("Material")]
    public string Material { get; set; }

    [DisplayName("Denominação")]
    public string Denominacao { get; set; }

    [DisplayName("Comprimento")]
    public string Comprimento { get; set; }

    [DisplayName("Seriado")]
    public string Seriado { get; set; }

    [DisplayName("Operação")]
    public string Operacao { get; set; }

    [DisplayName("Máquina")]
    public string Maquina { get; set; }

    [Browsable(false)]
    public string Interno { get; set; }

    [Browsable(false)]
    public string PathName { get; set; }

    public static SortableBindingList<ReportWorks> GetReport() {
      var listaReportWorks = new List<ReportWorks>();

      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        object[] AtiveConfiguration = null;
        string valOut;
        string resolvedValOut;
        var swModelDocExt = swModel.Extension;

        var reportWorks = new ReportWorks();

        reportWorks.Nivel = "0";
        reportWorks.Qtd = "1";
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
        swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
        reportWorks.Denominacao = resolvedValOut;
        swCustPropMgr.Get2("Componente", out valOut, out resolvedValOut);
        reportWorks.Componente = resolvedValOut;
        reportWorks.Seriado = "Não";
        reportWorks.PathName = swModel.GetPathName();
        listaReportWorks.Add(reportWorks);

        string fileName = swModel.GetPathName().ToUpper();
        AtiveConfiguration = (object[])swModel.GetConfigurationNames();
        string activeConfig = (string)AtiveConfiguration[0];

        swModelDocExt = swModel.Extension;

        // Inserir lista de material e pegar dados
        string lista = $"{Application.StartupPath}\\01 - Addin LM\\ListaReportWorks.sldbomtbt";

        int BomType = (int)swBomType_e.swBomType_Indented;
        int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;

        var swBOMAnnotation = swModelDocExt.InsertBomTable3(lista, 0, 1, BomType, activeConfig, false, NumberingType, DetailedCutList: true);

        PegaDadosLista(swBOMAnnotation, listaReportWorks);
        ListaCorte.ExcluirLista(swModel);

        foreach (var item in listaReportWorks.Where(x => x.PathName.ToUpper().EndsWith("SLDPRT"))) {
          int status = 0;
          int warnings = 0;
          swModel = Sw.App.OpenDoc6(item.PathName,
              (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

          string PathName = swModel.GetPathName();

          if (swModel == null) continue;

          swModelDocExt = swModel.Extension;

          swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

          FeatureManager swFeatMgr = default(FeatureManager);
          Feature swFeat = default(Feature);
          string FeatType = null;
          string FeatTypeName = null;
          int bodyCount = 0;
          bool boolstatus;

          BodyFolder swBodyFolder = default(BodyFolder);

          Feature[] featureArr = new Feature[3];

          swFeatMgr = swModel.FeatureManager;

          var configAtiva = ((Configuration)swModel.GetActiveConfiguration()).Name;

          var configs = (object[])swModel.GetConfigurationNames();
          TipoListaMaterial? tipo = null;

          List<Tuple<string, double>> compPorConfigs = new List<Tuple<string, double>>();

          foreach (var conf in configs) {
            if (tipo == TipoListaMaterial.Chapa)
              break;

            swFeat = (Feature)swModel.FirstFeature();

            swModel.ShowConfiguration((string)conf);
            //swModel.ForceRebuild3(true);

            bool possuiListaCorte = false;

            while ((swFeat != null)) {
              FeatType = swFeat.Name;
              FeatTypeName = swFeat.GetTypeName2();

              if (FeatTypeName == "CutListFolder") {
                possuiListaCorte = true;

                swBodyFolder = (BodyFolder)swFeat.GetSpecificFeature2();
                bodyCount = swBodyFolder.GetBodyCount();

                if (bodyCount > 0) {
                  boolstatus = swModel.Extension.SelectByID2(FeatType, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

                  SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
                  swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);

                  CustomPropertyManager oCustPropMngr = swFeat.CustomPropertyManager;

                  object[] custPropNames = (object[])oCustPropMngr.GetNames();

                  if (custPropNames != null) {
                    object[] vBodies = (object[])swBodyFolder.GetBodies();

                    if ((vBodies != null)) {
                      for (int h = vBodies.GetLowerBound(0); h <= vBodies.GetUpperBound(0); h++) {
                        Body2 Body = default(Body2);
                        Body = (Body2)vBodies[h];

                        if (Body.IsSheetMetal())
                          tipo = TipoListaMaterial.Chapa;
                        else
                          tipo = TipoListaMaterial.Soldagem;
                      }
                    }

                    if (tipo == TipoListaMaterial.Soldagem) {
                      oCustPropMngr.Get2("COMPRIMENTO", out string sValue, out string sResolvedvalue);

                      if (double.TryParse(sResolvedvalue.Replace(".", ","), out double vlr) && double.TryParse(item.Comprimento, out double compr)) {
                        if (vlr > compr && vlr - compr < 10) {
                          item.Comprimento = vlr.ToString();

                        }
                      }
                    } else
                      break;
                  }
                }
              }
              swFeat = (Feature)swFeat.GetNextFeature();
            }
            if (!possuiListaCorte) break;
          }

          Sw.App.CloseDoc(item.PathName);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar Report\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      EliminarObsoletos(listaReportWorks);

      return new SortableBindingList<ReportWorks>(listaReportWorks);
    }

    private static void PegaDadosLista(BomTableAnnotation swBOMAnnotation, List<ReportWorks> listaReportWorks) {
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

          var comp = swTableAnnotation.get_Text(i, 6).Replace(".", ",");

          var reportWorks = new ReportWorks {
            Nivel = swTableAnnotation.get_Text(i, 0),
            Qtd = swTableAnnotation.get_Text(i, 1),
            Codigo = swTableAnnotation.get_Text(i, 2),
            Componente = swTableAnnotation.get_Text(i, 3),
            Material = swTableAnnotation.get_Text(i, 4),
            Denominacao = swTableAnnotation.get_Text(i, 5),
            Comprimento = comp,
            Seriado = swTableAnnotation.get_Text(i, 7),
            Interno = swTableAnnotation.get_Text(i, 8),
            Operacao = swTableAnnotation.get_Text(i, 9),
            Maquina = swTableAnnotation.get_Text(i, 10),
          };

          if (vModelPathNames != null)
            reportWorks.PathName = vModelPathNames[0];

          listaReportWorks.Add(reportWorks);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista Reportworks\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void EliminarObsoletos(List<ReportWorks> listaReportWorks) {
      try {
        for (int i = 1; i < listaReportWorks.Count; i++) {
          //string[] NivelPaiStart = listaReportWorks[i - 1].Nivel.Split('.');
          //string[] NivelFilhoStart = listaReportWorks[i].Nivel.Split('.');
          string NivelPai = listaReportWorks[i - 1].Nivel;
          string NivelFilho = listaReportWorks[i].Nivel;
          bool paiEhSeriado = false;

          if (NivelFilho.Length > NivelPai.Length) {
            NivelFilho = NivelFilho.Substring(0, NivelPai.Length);
          }


          if (listaReportWorks[i - 1].Seriado == "Sim")
            paiEhSeriado = true;
          else if (listaReportWorks[i - 1].Seriado == "Não")
            paiEhSeriado = false;

          if (NivelFilho == NivelPai && paiEhSeriado) {
            listaReportWorks.Remove(listaReportWorks[i]);
            i--;
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao eliminar obsoletos\n\n{ex.Message}", "Addin LM Projetos",
              MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public static bool UpdateCutList(int indexLista, ReportWorks reportWorks) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        FeatureManager swFeatMgr = default(FeatureManager);
        Feature swFeat = default(Feature);
        string FeatType = null;
        string FeatTypeName = null;
        int bodyCount = 0;
        int cutListIndex = 0;
        bool boolstatus;

        BodyFolder swBodyFolder = default(BodyFolder);

        Feature[] featureArr = new Feature[3];

        swFeatMgr = swModel.FeatureManager;

        swFeat = (Feature)swModel.FirstFeature();

        while ((swFeat != null)) {

          FeatType = swFeat.Name;
          FeatTypeName = swFeat.GetTypeName2();

          if (FeatTypeName == "CutListFolder") {
            cutListIndex++;
            if (cutListIndex == indexLista) {
              swBodyFolder = (BodyFolder)swFeat.GetSpecificFeature2();
              bodyCount = swBodyFolder.GetBodyCount();
              int BodyFolderTypeE = swBodyFolder.Type;

              if (bodyCount > 0) {
                boolstatus = swModel.Extension.SelectByID2(FeatType, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

                SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
                swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);

                CustomPropertyManager oCustPropMngr = swFeat.CustomPropertyManager;

                object[] custPropNames = (object[])oCustPropMngr.GetNames();

                if (custPropNames != null) {
                  oCustPropMngr.Add3("Código", (int)swCustomInfoType_e.swCustomInfoText, reportWorks.Codigo,
                      (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                  oCustPropMngr.Add3("Material", (int)swCustomInfoType_e.swCustomInfoText, reportWorks.Material,
                     (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                  oCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, reportWorks.Denominacao,
                     (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                  oCustPropMngr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText, reportWorks.Operacao,
                     (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                  return true;
                }
              }
              break;
            }
          }
          swFeat = (Feature)swFeat.GetNextFeature();
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar Lista de corte\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return false;
    }

  }
}
