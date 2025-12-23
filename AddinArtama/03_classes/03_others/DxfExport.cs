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
  internal class DxfExport {
    [DisplayName("Sel")]
    public bool Exportar { get; set; }

    [DisplayName("COMPONENTE")]
    public string CodComponente { get; set; }

    [DisplayName("DENOMINAÇÃO")]
    public string Denominacao { get; set; }

    [DisplayName("ESPESSURA")]
    public double EspessuraMaterial { get; set; }

    [DisplayName("CÓD. MATERIAL")]
    public string CodigoMaterial { get; set; }

    [DisplayName("DESCRIÇÃO MATERIAL")]
    [Browsable(false)]
    public string DescricaoMaterial { get; set; }

    [Browsable(false)]
    public string PathName { get; set; }

    public static List<DxfExport> GetDrawing() {
      var listaDxf = new List<DxfExport>();

      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        ConfigurationManager swConfMgr;
        Configuration swConf;
        Component2 swRootComp;

        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        swRootComp = swConf.GetRootComponent3(true);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          using (ContextoDados db = new ContextoDados())
            TraverseComponent(db, listaDxf, swRootComp, 0);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar desenho\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      BubbleSort(listaDxf);

      return listaDxf;
    }

    private static void TraverseComponent(ContextoDados db, List<DxfExport> listaDxf, Component2 swComp, long nLevel) {
      string nameShort = "";
      try {
        object[] vChildComp;

        Component2 swChildComp;

        vChildComp = (object[])swComp.GetChildren();

        for (int i = 0; i < vChildComp.Length; i++) {
          swChildComp = (Component2)vChildComp[i];
          bool supress = swChildComp.IsSuppressed();
          bool exclude = swChildComp.ExcludeFromBOM;
          string refConfig = swChildComp.ReferencedConfiguration;

          string PathName = "";

          var swModel = (ModelDoc2)swChildComp.GetModelDoc2();
          if (swModel == null) continue;
          bool readOnly = swModel.IsOpenedReadOnly();

          string valOut;
          string resolvedValOut;

          if (supress == false && exclude == false && swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
            PathName = swModel.GetPathName().ToUpper();
            nameShort = Path.GetFileNameWithoutExtension(PathName);

            var dxfExport = new DxfExport();
            //var procs = GetProcesso(refConfig, swModel, dxfExport);
            var codMat = 0;

            if (/*!string.IsNullOrEmpty(procs) && */int.TryParse(dxfExport.CodigoMaterial, out codMat)) {
              dxfExport.Exportar = true;
              dxfExport.PathName = PathName;
              dxfExport.CodComponente = nameShort;

              var swModelDocExt = swModel.Extension;
              var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
              swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
              dxfExport.Denominacao = resolvedValOut;

              if (Queryable.FirstOrDefault(
                      db.materia_primas.Where(x => x.codigo == codMat)) != null) {

                if(File.Exists(PathName) && !listaDxf.Any(x=>x.CodComponente == nameShort)) {
                  listaDxf.Add(dxfExport);
                }
              }
            }
          }
          TraverseComponent(db, listaDxf, swChildComp, nLevel + 1);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Carregar Desenhos Para Exportar. + [{nameShort}]\n" +
            $"\n" +
            $"{ex.Message}", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void BubbleSort(List<DxfExport> listaDxf) {
      try {
        for (int i = 0; i < listaDxf.Count; i++) {
          for (int h = i + 1; h < listaDxf.Count; h++) {
            int j = string.Compare(listaDxf[i].CodComponente, listaDxf[h].CodComponente);
            if (j == 1) {
              DxfExport CompChange = listaDxf[i];
              listaDxf[i] = listaDxf[h];
              listaDxf[h] = CompChange;
            }
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao ordenar\n\n{ex.Message}", "Addin LM Projetos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    //private static string GetProcesso(string activeConfig, ModelDoc2 swModel, DxfExport dxfExport) {
    //  try {
    //    ModelDocExtension swModelDocExt = default(ModelDocExtension);
    //    Configuration swConfig = default(Configuration);
    //    ConfigurationManager swConfMgr = default(ConfigurationManager);
    //    CustomPropertyManager swCustPropMgr = default(CustomPropertyManager);

    //    object[] configNameArr = null;
    //    object[] vPropNames;
    //    bool status = false;
    //    bool ehTerceiro = false;
    //    int nNbrProps;
    //    string valOut;
    //    string resolvedValOut;
    //    string operacao = "";

    //    swConfMgr = swModel.ConfigurationManager;
    //    swModelDocExt = swModel.Extension;
    //    configNameArr = (object[])swModel.GetConfigurationNames();

    //    swConfig = (Configuration)swModel.GetConfigurationByName(activeConfig);

    //    status = swModel.ShowConfiguration2(activeConfig);

    //    swCustPropMgr = swConfig.CustomPropertyManager;
    //    nNbrProps = swCustPropMgr.Count;
    //    vPropNames = (object[])swCustPropMgr.GetNames();

    //    swCustPropMgr.Get2("Operação", out valOut, out resolvedValOut);
    //    operacao = resolvedValOut;

    //    swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
    //    nNbrProps = swCustPropMgr.Count;
    //    vPropNames = (object[])swCustPropMgr.GetNames();

    //    swCustPropMgr.Get2("Terceiro", out valOut, out resolvedValOut);
    //    ehTerceiro = resolvedValOut == "Sim";

    //    if (operacao == "") {
    //      swCustPropMgr.Get2("Operação", out valOut, out resolvedValOut);
    //      operacao = resolvedValOut;
    //    }

    //    if ((swModel.GetType() == (int)swDocumentTypes_e.swDocPART && !ehTerceiro) || (operacao == "" && ehTerceiro)) {
    //      var opTmp = operacao;
    //      operacao = GetInfoFromCutList(swModel, dxfExport);
    //      if (operacao == "")
    //        operacao = opTmp;
    //    }

    //    return operacao;
    //  } catch (Exception ex) {
    //    MsgBox.Show($"Erro ao verificar CheckList\n\n{ex.Message}", "Addin LM Projetos",
    //       MessageBoxButtons.OK, MessageBoxIcon.Error);
    //    return "";
    //  }
    //}

    private static string GetInfoFromCutList(ModelDoc2 swModel, DxfExport dxfExport) {
      string _return = string.Empty;
      bool boolstatus;
      var processos = new List<string>();

      try {
        FeatureManager swFeatMgr = default(FeatureManager);
        Feature swFeat = default(Feature);
        string FeatType = null;
        string FeatTypeName = null;
        int bodyCount = 0;

        BodyFolder swBodyFolder = default(BodyFolder);

        Feature[] featureArr = new Feature[3];

        swFeatMgr = swModel.FeatureManager;

        swFeat = (Feature)swModel.FirstFeature();


        while ((swFeat != null)) {
          FeatType = swFeat.Name;
          FeatTypeName = swFeat.GetTypeName2();


          if (FeatTypeName == "CutListFolder") {
            swBodyFolder = (BodyFolder)swFeat.GetSpecificFeature2();
            bodyCount = swBodyFolder.GetBodyCount();

            if (bodyCount > 0) {
              boolstatus = swModel.Extension.SelectByID2(FeatType, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

              SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
              swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);

              CustomPropertyManager swCustPropMngr = swFeat.CustomPropertyManager;

              object[] custPropNames = (object[])swCustPropMngr.GetNames();

              if (custPropNames != null) {
                string sValue, sResolvedvalue;

                swBodyFolder = (BodyFolder)swFeat.GetSpecificFeature2();
                boolstatus = swBodyFolder.SetAutomaticCutList(true);
                boolstatus = swBodyFolder.UpdateCutList();

                swCustPropMngr.Get2("Espessura da Chapa metálica", out sValue, out sResolvedvalue);
                if (!string.IsNullOrEmpty(sResolvedvalue)) {
                  var esp = Math.Round(Convert.ToDouble(sResolvedvalue.Replace(".", ",")), 2);

                  dxfExport.EspessuraMaterial = esp;
                }

                swCustPropMngr.Get2("Código", out sValue, out sResolvedvalue);
                if (!string.IsNullOrEmpty(sResolvedvalue)) {
                  dxfExport.CodigoMaterial = sResolvedvalue;
                }

                swCustPropMngr.Get2("Denominação", out sValue, out sResolvedvalue);
                if (!string.IsNullOrEmpty(sResolvedvalue)) {
                  dxfExport.DescricaoMaterial = sResolvedvalue;
                }

                swCustPropMngr.Get2("Operação", out sValue, out sResolvedvalue);
                if (!string.IsNullOrEmpty(sResolvedvalue)) {
                  var spl = sResolvedvalue.Split('/');
                  foreach (var p in spl) {
                    if (string.IsNullOrEmpty(p))
                      continue;

                    if (!processos.Contains(p))
                      processos.Add(p);
                  }
                }
              }
            }
          }
          swFeat = (Feature)swFeat.GetNextFeature();
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar lista corte\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      foreach (var processo in processos)
        _return += processo + "/";

      if (_return.EndsWith("/"))
        _return = _return.Substring(0, _return.Length - 1);

      return _return;
    }

  }
}
