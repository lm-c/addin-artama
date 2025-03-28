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
  internal class PackList {
    [DisplayName("ID")]
    public int IdVolume { get; set; }

    [Browsable(false)]
    public string NomeConfiguracao { get; set; }

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

    public static List<PackList> GetPackLit(out List<Z_Padrao> descVolumes) {
      List<PackList> listaPackList = new List<PackList>();
      descVolumes = new List<Z_Padrao>();

      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        ConfigurationManager swConfMgr;
        Configuration swConf;
        Component2 swRootComp;

        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        swRootComp = swConf.GetRootComponent3(true);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          TraverseComponent(swRootComp, listaPackList, 1);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar PackList\n\n{ex.Message}", "Addin Artama",
          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }

      BubbleSort(listaPackList);
      descVolumes = PegarDescricaoVolumes(listaPackList);

      return listaPackList;
    }

    private static void TraverseComponent(Component2 swComp, List<PackList> listaPackList, long nLevel) {
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

            PackList packList = new PackList();

            if (IsCheckList(swModel, refConfig, packList) == true) {
              nameLong = nameShort + "?" + refConfig;

              foreach (PackList ls in listaPackList) {
                if (ls.DescricaoTemp == nameLong) {
                  ls.QtdItem++;
                  canAdd = false;
                  break;
                }
              }
              packList.DescricaoTemp = nameLong;

              if (canAdd == true)
                listaPackList.Add(packList);

              continue;
            }

            TraverseComponent(swChildComp, listaPackList, nLevel + 1);
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar componente. + [{nameShort}]\n\n{ex.Message}", "Addin Artama",
             System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }
    }

    private static bool IsCheckList(ModelDoc2 swModel, string activeConfig, PackList packList) {
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

        swCustPropMgr.Get2("CHECK", out valOut, out resolvedValOut);
        check = resolvedValOut;
        swCustPropMgr.Get2("Componente", out valOut, out resolvedValOut);
        comp = resolvedValOut;
        swCustPropMgr.Get2("Denominação", out valOut, out resolvedValOut);
        denom = resolvedValOut;

        if (check == "") {
          swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
          nNbrProps = swCustPropMgr.Count;
          vPropNames = (object[])swCustPropMgr.GetNames();

          swCustPropMgr.Get2("CHECK", out valOut, out resolvedValOut);
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
          packList.CodigoItem = comp;
          packList.DescricaoVolume = check;
          packList.DescricaoItem = denom.Trim();
          packList.QtdItem = 1;
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao verificar CheckList\n\n{ex.Message}", "Addin Artama",
           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }
      return _return;
    }

    private static void BubbleSort(List<PackList> listaPackList) {
      try {
        PackList packChange = new PackList();

        for (int i = 0; i < listaPackList.Count; i++) {
          for (int h = i + 1; h < listaPackList.Count; h++) {
            int j = string.Compare(listaPackList[i].DescricaoVolume, listaPackList[h].DescricaoVolume);
            if (j == 1) {
              packChange = listaPackList[i];
              listaPackList[i] = listaPackList[h];
              listaPackList[h] = packChange;
            }
          }
        }

        for (int i = 0; i < listaPackList.Count; i++) {
          for (int h = i + 1; h < listaPackList.Count; h++) {
            int j = string.Compare(listaPackList[i].CodigoItem, listaPackList[h].CodigoItem);
            if (j == 1 & listaPackList[i].DescricaoVolume == listaPackList[h].DescricaoVolume) {
              packChange = listaPackList[i];
              listaPackList[i] = listaPackList[h];
              listaPackList[h] = packChange;
            }
          }
        }

        int index = 1;
        for (int i = 0; i < listaPackList.Count; i++) {
          if (i == 0) {
            listaPackList[i].IdVolume = index;
          } else if (i == listaPackList.Count - 1) {
            if (listaPackList[i].DescricaoVolume != listaPackList[i - 1].DescricaoVolume) {
              index++;
              listaPackList[i].IdVolume = index;
            } else listaPackList[i].IdVolume = index;
          } else {
            if (listaPackList[i].DescricaoVolume != listaPackList[i - 1].DescricaoVolume) {
              index++;
              listaPackList[i].IdVolume = index;
            } else listaPackList[i].IdVolume = index;
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

    private static List<Z_Padrao> PegarDescricaoVolumes(List<PackList> listaPackList) {
      var _return = new List<Z_Padrao>();

      foreach (var item in listaPackList) {
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
