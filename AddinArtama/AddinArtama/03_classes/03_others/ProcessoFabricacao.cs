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
  internal class ProcessoFabricacao {
    [Browsable(false)]
    [DisplayName("ID")]
    public int IdProcesso { get; set; }

    [DisplayName("PROCESSO")]
    public string DescricaoProcesso { get; set; } = string.Empty;

    [DisplayName("CÓDIGO")]
    public string CodigoItem { get; set; } = string.Empty;

    [DisplayName("DENOMINAÇÃO")]
    public string DescricaoItem { get; set; } = string.Empty;

    [DisplayName("ESPES.")]
    public double EspessuraMaterial { get; set; } = 0;

    [DisplayName("QTD")]
    public int QtdItem { get; set; } = 0;

    [Browsable(false)]
    public string NomeComponente { get; set; }

    [Browsable(false)]
    public string PathName { get; set; }

    public static List<ProcessoFabricacao> GetProcessoFabricacao() {
      var listaProcessoFabricacao = new List<ProcessoFabricacao>();

      try {
        using (ContextoDados db = new ContextoDados()) {
          var swModel = (ModelDoc2)Sw.App.ActiveDoc;

          ConfigurationManager swConfMgr;
          Configuration swConf;
          Component2 swRootComp;

          swConfMgr = swModel.ConfigurationManager;
          swConf = swConfMgr.ActiveConfiguration;

          var processoFabricacao = new ProcessoFabricacao();
          if (PossuiProcesso(swModel, swConf.Name, processoFabricacao) == true) {

            var spl = processoFabricacao.DescricaoProcesso.Split('/');
            foreach (var p in spl) {
              if (!string.IsNullOrEmpty(p.Trim()) && int.TryParse(p, out int idProc)) {
                var processo = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == idProc);

                if (processo != null) {
                  listaProcessoFabricacao.Add(new ProcessoFabricacao {
                    IdProcesso = idProc,
                    DescricaoProcesso = idProc + " - " + processo.descrOperacao,
                    CodigoItem = processoFabricacao.CodigoItem,
                    DescricaoItem = processoFabricacao.DescricaoItem,
                    QtdItem = processoFabricacao.QtdItem,
                    PathName = processoFabricacao.PathName,
                    NomeComponente = processoFabricacao.NomeComponente,
                  });

                }
              }
            }
          }
          swRootComp = swConf.GetRootComponent3(true);

          if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
            TraverseComponent(db, listaProcessoFabricacao, swRootComp, 1);
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar Processo de Fabricacao\n\n{ex.Message}", "Addin LM Projetos",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return listaProcessoFabricacao.OrderBy(x => x.DescricaoProcesso).ThenBy(x => x.CodigoItem).ToList();
    }

    private static void TraverseComponent(ContextoDados db, List<ProcessoFabricacao> listaProcessoFabricacao, Component2 swComp, long nLevel) {
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

          var swModel = (ModelDoc2)swChildComp.GetModelDoc2();
          if (swModel == null) continue;

          string pathName = swModel.GetPathName();

          nameShort = Path.GetFileNameWithoutExtension(pathName);

          if (supress == false && exclude == false) {
            bool canAdd = true;

            var processoFabricacao = new ProcessoFabricacao();

            if (PossuiProcesso(swModel, refConfig, processoFabricacao) == true) {

              if (listaProcessoFabricacao.Any(x => x.NomeComponente == processoFabricacao.NomeComponente)) {
                listaProcessoFabricacao.FirstOrDefault(x => x.NomeComponente == processoFabricacao.NomeComponente).QtdItem += 1;
                canAdd = false;
              }

              if (canAdd == true) {
                var spl = processoFabricacao.DescricaoProcesso.Split('/');
                foreach (var p in spl) {
                  if (!string.IsNullOrEmpty(p.Trim()) && int.TryParse(p, out int idProc)) {
                    var processo = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == idProc);

                    if (processo != null) {
                      listaProcessoFabricacao.Add(new ProcessoFabricacao {
                        IdProcesso = idProc,
                        DescricaoProcesso = idProc + " - " + processo.descrOperacao,
                        CodigoItem = processoFabricacao.CodigoItem,
                        DescricaoItem = processoFabricacao.DescricaoItem,
                        QtdItem = processoFabricacao.QtdItem,
                        EspessuraMaterial = processoFabricacao.EspessuraMaterial,
                        PathName = processoFabricacao.PathName,
                        NomeComponente = processoFabricacao.NomeComponente,
                      });
                    }
                  }
                }
              }

              //continue;
            }

            TraverseComponent(db, listaProcessoFabricacao, swChildComp, nLevel + 1);
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar componente. + [{nameShort}]\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static bool PossuiProcesso(ModelDoc2 swModel, string activeConfig, ProcessoFabricacao processoFabricacao) {
      bool _return = false;

      try {
        ModelDocExtension swModelDocExt = default(ModelDocExtension);
        Configuration swConfig = default(Configuration);
        ConfigurationManager swConfMgr = default(ConfigurationManager);
        CustomPropertyManager swCustPropMgr = default(CustomPropertyManager);

        object[] configNameArr = null;
        object[] vPropNames;
        bool status = false;
        bool ehTerceiro = false;
        int nNbrProps;
        string valOut;
        string resolvedValOut;
        string operacao = "";
        string comp = "";
        string denom = "";

        swConfMgr = swModel.ConfigurationManager;
        swModelDocExt = swModel.Extension;
        configNameArr = (object[])swModel.GetConfigurationNames();

        swConfig = (Configuration)swModel.GetConfigurationByName(activeConfig);

        status = swModel.ShowConfiguration2(activeConfig);

        swCustPropMgr = swConfig.CustomPropertyManager;
        nNbrProps = swCustPropMgr.Count;
        vPropNames = (object[])swCustPropMgr.GetNames();

        swCustPropMgr.Get2("Operação", out valOut, out resolvedValOut); // substyituir por processo;
        operacao = resolvedValOut;
        swCustPropMgr.Get2("Componente", out valOut, out resolvedValOut);
        comp = resolvedValOut;
        swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
        denom = resolvedValOut;

        swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
        nNbrProps = swCustPropMgr.Count;
        vPropNames = (object[])swCustPropMgr.GetNames();

        swCustPropMgr.Get2("Terceiro", out valOut, out resolvedValOut);
        ehTerceiro = resolvedValOut == "Sim";

        if (operacao == "") {
          swCustPropMgr.Get2("Operação", out valOut, out resolvedValOut);
          operacao = resolvedValOut;
        }

        if (comp == "") {
          swCustPropMgr.Get2("Componente", out valOut, out resolvedValOut);
          comp = resolvedValOut;
        }

        if (denom == "") {
          swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
          denom = resolvedValOut;
        }

        if (swConfig.UseAlternateNameInBOM == true) {
          swConfig.UseAlternateNameInBOM = true;

          if (!string.IsNullOrEmpty(swConfig.AlternateName))
            comp = swConfig.AlternateName;
        }

        if ((swModel.GetType() == (int)swDocumentTypes_e.swDocPART && !ehTerceiro) || (operacao == "" && ehTerceiro)) {
          var opTmp = operacao;
          operacao = GetOpFromCutList(swModel, processoFabricacao);
          if (operacao == "")
            operacao = opTmp;
        }

        if (operacao != "") {
          _return = true;
          processoFabricacao.CodigoItem = comp;
          processoFabricacao.DescricaoProcesso = operacao;
          processoFabricacao.DescricaoItem = denom.Trim();
          processoFabricacao.PathName = swModel.GetPathName();
          processoFabricacao.NomeComponente = Path.GetFileNameWithoutExtension(processoFabricacao.PathName);
          processoFabricacao.QtdItem = 1;
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao verificar CheckList\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return _return;
    }

    private static string GetOpFromCutList(ModelDoc2 swModel, ProcessoFabricacao processoFabricacao) {
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
                if (!string.IsNullOrEmpty(sResolvedvalue))
                  processoFabricacao.EspessuraMaterial = Convert.ToDouble(sResolvedvalue.Replace(".", ","));

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

    public static List<Z_Padrao> GetDescricaoProcesso(List<ProcessoFabricacao> listaProcessoFabricacao) {
      var _return = new List<Z_Padrao>();

      try {
        foreach (var processo in listaProcessoFabricacao) {
          if (!_return.Any(x => x.Descricao == processo.DescricaoProcesso))
            _return.Add(new Z_Padrao { Codigo = Convert.ToInt32(processo.IdProcesso), Descricao = processo.DescricaoProcesso });
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar descrição processos\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return _return;
    }

    public static string GetDenominacao(ModelDoc2 swModel) {
      string _return = "";

      try {
        ModelDocExtension swModelDocExt = default(ModelDocExtension);
        CustomPropertyManager swCustPropMgr = default(CustomPropertyManager);

        swModelDocExt = swModel.Extension;

        swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        swCustPropMgr.Get2("Denominação", out string valOut, out string resolvedValOut);
        _return = resolvedValOut;
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar denominção\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return _return;
    }
  }
}
