using LmCorbieUI;
using Microsoft.Reporting.Map.WebForms.BingMaps;
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
  internal class PlanoPintura {
    [DisplayName("ID")]
    public int IdVolume { get; set; }

    [Browsable(false)]
    public string DescricaoTemp { get; set; }

    [DisplayName("VOLUME")]
    public string DescricaoVolume { get; set; }

    [DisplayName("CÓDIGO")]
    public string CodigoItem { get; set; }

    [DisplayName("DENOMINAÇÃO")]
    public string DescricaoItem { get; set; }

    [DisplayName("QTD")]
    public int QtdItem { get; set; }

    public static List<PlanoPintura> GetPlanoPintura(out List<Z_Padrao> descVolumes) {

      var listaPlanoPintura = new List<PlanoPintura>();

      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        ConfigurationManager swConfMgr;
        Configuration swConf;
        Component2 swRootComp;

        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        swRootComp = swConf.GetRootComponent3(true);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          TraverseComponent(swRootComp, listaPlanoPintura, 1);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar Plano de Pintura\n\n{ex.Message}", "Addin Artama",
          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }

      BubbleSort(listaPlanoPintura);
      descVolumes = PegarDescricaoVolumes(listaPlanoPintura);

      return listaPlanoPintura;
    }

    private static void TraverseComponent(Component2 swComp, List<PlanoPintura> listaPlanoPintura, long nLevel) {
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
          string nameLong = "";

          if (supress == false && exclude == false) {
            bool canAdd = true;

            var planoPintura = new PlanoPintura();

            if (IsPlanoPintura(swModel, refConfig, planoPintura) == true) {

              nameLong = nameShort + "?" + refConfig;

              foreach (PlanoPintura ls in listaPlanoPintura) {
                if (ls.DescricaoTemp == nameLong) {
                  ls.QtdItem++;
                  canAdd = false;
                  break;
                }
              }

              planoPintura.DescricaoTemp = nameLong;

              if (canAdd == true)
                listaPlanoPintura.Add(planoPintura);

              continue;
            }

            TraverseComponent(swChildComp, listaPlanoPintura, nLevel + 1);
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar componente. + [{nameShort}]\n\n{ex.Message}", "Addin Artama",
             System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }
    }

    private static bool IsPlanoPintura(ModelDoc2 swModel, string activeConfig, PlanoPintura planoPintura) {
      bool _return = false;

      try {
        ModelDocExtension swModelDocExt = default(ModelDocExtension);
        Configuration swConfig = default(Configuration);
        ConfigurationManager swConfMgr = default(ConfigurationManager);
        CustomPropertyManager swCustPropMgr = default(CustomPropertyManager);

        object[] configNameArr = null;
        object[] vPropNames;
        bool status = false;
        int nNbrProps;
        string valOut;
        string resolvedValOut;
        string check = "";
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

        swCustPropMgr.Get2("PINTURA", out valOut, out resolvedValOut);
        check = resolvedValOut;
        swCustPropMgr.Get2("Componente", out valOut, out resolvedValOut);
        comp = resolvedValOut;
        swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
        denom = resolvedValOut;

        if (check == "") {
          swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
          nNbrProps = swCustPropMgr.Count;
          vPropNames = (object[])swCustPropMgr.GetNames();

          swCustPropMgr.Get2("PINTURA", out valOut, out resolvedValOut);
          check = resolvedValOut;
        }

        if (comp == "") {
          swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
          nNbrProps = swCustPropMgr.Count;
          vPropNames = (object[])swCustPropMgr.GetNames();

          swCustPropMgr.Get2("Componente", out valOut, out resolvedValOut);
          comp = resolvedValOut;
        }

        if (denom == "") {
          swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
          nNbrProps = swCustPropMgr.Count;
          vPropNames = (object[])swCustPropMgr.GetNames();

          swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
          denom = resolvedValOut;
        }


        if (swConfig.UseAlternateNameInBOM == true) {
          swConfig.UseAlternateNameInBOM = true;

          if (!string.IsNullOrEmpty(swConfig.AlternateName))
            comp = swConfig.AlternateName;
        }

        if (check != "") {
          _return = true;
          planoPintura.CodigoItem = comp;
          planoPintura.DescricaoVolume = check;
          planoPintura.DescricaoItem = denom.Trim();
          planoPintura.QtdItem = 1;
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao verificar Plano de Pintura\n\n{ex.Message}", "Addin Artama",
           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }
      return _return;
    }

    private static void BubbleSort(List<PlanoPintura> listaPlanoPintura) {
      try {
        PlanoPintura packChange = new PlanoPintura();

        for (int i = 0; i < listaPlanoPintura.Count; i++) {
          for (int h = i + 1; h < listaPlanoPintura.Count; h++) {
            int j = string.Compare(listaPlanoPintura[i].DescricaoVolume, listaPlanoPintura[h].DescricaoVolume);
            if (j == 1) {
              packChange = listaPlanoPintura[i];
              listaPlanoPintura[i] = listaPlanoPintura[h];
              listaPlanoPintura[h] = packChange;
            }
          }
        }

        for (int i = 0; i < listaPlanoPintura.Count; i++) {
          for (int h = i + 1; h < listaPlanoPintura.Count; h++) {
            int j = string.Compare(listaPlanoPintura[i].CodigoItem, listaPlanoPintura[h].CodigoItem);
            if (j == 1 & listaPlanoPintura[i].DescricaoVolume == listaPlanoPintura[h].DescricaoVolume) {
              packChange = listaPlanoPintura[i];
              listaPlanoPintura[i] = listaPlanoPintura[h];
              listaPlanoPintura[h] = packChange;
            }
          }
        }

        int index = 1;
        for (int i = 0; i < listaPlanoPintura.Count; i++) {
          if (i == 0) {
            listaPlanoPintura[i].IdVolume = index;
          } else if (i == listaPlanoPintura.Count - 1) {
            if (listaPlanoPintura[i].DescricaoVolume != listaPlanoPintura[i - 1].DescricaoVolume) {
              index++;
              listaPlanoPintura[i].IdVolume = index;
            } else listaPlanoPintura[i].IdVolume = index;
          } else {
            if (listaPlanoPintura[i].DescricaoVolume != listaPlanoPintura[i - 1].DescricaoVolume) {
              index++;
              listaPlanoPintura[i].IdVolume = index;
            } else listaPlanoPintura[i].IdVolume = index;
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao ordenar\n\n{ex.Message}", "Addin Artama",
           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }
    }

    public static string GetDenominacao(ModelDoc2 swModel) {
      string _return = "";

      try {
        string valOut;
        string resolvedValOut;

        ModelDocExtension swModelDocExt = default(ModelDocExtension);
        CustomPropertyManager swCustPropMgr = default(CustomPropertyManager);

        swModelDocExt = swModel.Extension;

        swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
        _return = resolvedValOut;
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar denominção\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return _return;
    }

    //public static List<W_Formato> SelecionarFormatos() {
    //  List<W_Formato> _return = new List<W_Formato>();

    //  const double xA4R_yA4P = 0.21;
    //  const double yA4R_xA4P_yA3 = 0.297;
    //  const double xA3_yA2 = 0.420;
    //  const double xA2_yA1 = 0.840;
    //  const double xA1_yA0 = 1.188;
    //  const double xA0 = 1.680;

    //  const short tamanhoFonte = 12;
    //  const float def = 2.835f;
    //  double eixoX = def;
    //  double eixoY = def;
    //  float ang = 0;

    //  using (ContextoDados db = new ContextoDados()) {
    //    var model = db.Template.FirstOrDefault();

    //    if (!string.IsNullOrEmpty(model.TemplateA4R)) {
    //      var posNota = db.NotaPDF.Where(x => x.ID == 1).FirstOrDefault();

    //      try {
    //        if (posNota != null)
    //          posNota.Rotacao = posNota.Rotacao;
    //      } catch (Exception) {
    //        posNota.Rotacao = ang;
    //      }

    //      _return.Add(new W_Formato {
    //        Codigo = 1,
    //        Formato = model.FormatoA4R,
    //        Template = model.TemplateA4R,
    //        LarguraFormato = xA4R_yA4P,
    //        AlturaFormato = yA4R_xA4P_yA3,
    //        TamanhoNota = posNota != null ? posNota.TamanhoFonte : tamanhoFonte,
    //        EixoX = posNota != null ? posNota.EixoX : eixoX,
    //        EixoY = posNota != null ? posNota.EixoY : eixoY,
    //        Rotacao = posNota != null ? posNota.Rotacao : ang,
    //      });
    //    }

    //    if (!string.IsNullOrEmpty(model.TemplateA4P)) {
    //      var posNota = db.NotaPDF.Where(x => x.ID == 2).FirstOrDefault();

    //      try {
    //        if (posNota != null)
    //          posNota.Rotacao = posNota.Rotacao;
    //      } catch (Exception) {
    //        posNota.Rotacao = ang;
    //      }

    //      _return.Add(new W_Formato {
    //        Codigo = 2,
    //        Formato = model.FormatoA4P,
    //        Template = model.TemplateA4P,
    //        LarguraFormato = yA4R_xA4P_yA3,
    //        AlturaFormato = xA4R_yA4P,
    //        TamanhoNota = posNota != null ? posNota.TamanhoFonte : tamanhoFonte,
    //        EixoX = posNota != null ? posNota.EixoX : eixoX,
    //        EixoY = posNota != null ? posNota.EixoY : eixoY,
    //        Rotacao = posNota != null ? posNota.Rotacao : ang,
    //      });
    //    }

    //    if (!string.IsNullOrEmpty(model.TemplateA3)) {
    //      var posNota = db.NotaPDF.Where(x => x.ID == 3).FirstOrDefault();

    //      try {
    //        if (posNota != null)
    //          posNota.Rotacao = posNota.Rotacao;
    //      } catch (Exception) {
    //        posNota.Rotacao = ang;
    //      }

    //      _return.Add(new W_Formato {
    //        Codigo = 3,
    //        Formato = model.FormatoA3,
    //        Template = model.TemplateA3,
    //        LarguraFormato = xA3_yA2,
    //        AlturaFormato = yA4R_xA4P_yA3,
    //        TamanhoNota = posNota != null ? posNota.TamanhoFonte : tamanhoFonte,
    //        EixoX = posNota != null ? posNota.EixoX : eixoX,
    //        EixoY = posNota != null ? posNota.EixoY : eixoY,
    //        Rotacao = posNota != null ? posNota.Rotacao : ang,
    //      });
    //    }

    //    if (!string.IsNullOrEmpty(model.TemplateA2)) {
    //      var posNota = db.NotaPDF.Where(x => x.ID == 4).FirstOrDefault();

    //      try {
    //        if (posNota != null)
    //          posNota.Rotacao = posNota.Rotacao;
    //      } catch (Exception) {
    //        posNota.Rotacao = ang;
    //      }

    //      _return.Add(new W_Formato {
    //        Codigo = 4,
    //        Formato = model.FormatoA2,
    //        Template = model.TemplateA2,
    //        LarguraFormato = xA2_yA1,
    //        AlturaFormato = xA3_yA2,
    //        TamanhoNota = posNota != null ? posNota.TamanhoFonte : tamanhoFonte,
    //        EixoX = posNota != null ? posNota.EixoX : eixoX,
    //        EixoY = posNota != null ? posNota.EixoY : eixoY,
    //        Rotacao = posNota != null ? posNota.Rotacao : ang,
    //      });
    //    }

    //    if (!string.IsNullOrEmpty(model.TemplateA1)) {
    //      var posNota = db.NotaPDF.Where(x => x.ID == 5).FirstOrDefault();

    //      try {
    //        if (posNota != null)
    //          posNota.Rotacao = posNota.Rotacao;
    //      } catch (Exception) {
    //        posNota.Rotacao = ang;
    //      }

    //      _return.Add(new W_Formato {
    //        Codigo = 5,
    //        Formato = model.FormatoA1,
    //        Template = model.TemplateA1,
    //        LarguraFormato = xA1_yA0,
    //        AlturaFormato = xA2_yA1,
    //        TamanhoNota = posNota != null ? posNota.TamanhoFonte : tamanhoFonte,
    //        EixoX = posNota != null ? posNota.EixoX : eixoX,
    //        EixoY = posNota != null ? posNota.EixoY : eixoY,
    //        Rotacao = posNota != null ? posNota.Rotacao : ang,
    //      });
    //    }

    //    if (!string.IsNullOrEmpty(model.TemplateA0)) {
    //      var posNota = db.NotaPDF.Where(x => x.ID == 6).FirstOrDefault();

    //      try {
    //        if (posNota != null)
    //          posNota.Rotacao = posNota.Rotacao;
    //      } catch (Exception) {
    //        posNota.Rotacao = ang;
    //      }

    //      _return.Add(new W_Formato {
    //        Codigo = 6,
    //        Formato = model.FormatoA0,
    //        Template = model.TemplateA0,
    //        LarguraFormato = xA0,
    //        AlturaFormato = xA1_yA0,
    //        TamanhoNota = posNota != null ? posNota.TamanhoFonte : tamanhoFonte,
    //        EixoX = posNota != null ? posNota.EixoX : eixoX,
    //        EixoY = posNota != null ? posNota.EixoY : eixoY,
    //        Rotacao = posNota != null ? posNota.Rotacao : ang,
    //      });
    //    }
    //  }

    //  return _return;
    //}

    private static List<Z_Padrao> PegarDescricaoVolumes(List<PlanoPintura> listaPlanoPintura) {
      var _return = new List<Z_Padrao>();

      foreach (var item in listaPlanoPintura) {
        if (!_return.Any(x => x.Descricao == item.DescricaoVolume))
          _return.Add(new Z_Padrao {
            Codigo = item.IdVolume,
            Descricao = item.DescricaoVolume
          });
      }

      return _return;
    }

  }
}
