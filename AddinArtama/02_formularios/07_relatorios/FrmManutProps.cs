using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;

using SolidWorks.Interop.swconst;
using System.IO;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using System.Threading;

namespace AddinArtama {
  public partial class FrmManutProps : LmSingleForm {
    public FrmManutProps() {
      InitializeComponent();

    }

    private void BtnIniciar_Click(object sender, EventArgs e) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (Sw.App.ActiveDoc == null) {
          Toast.Info($"Sem documentos abertos");
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          MsgBox.ShowWaitMessage("Efetuando Manutenção. Aguarde......");

          ConfigurationManager swConfMgr;
          Configuration swConf;
          Component2 swRootComp;

          swConfMgr = swModel.ConfigurationManager;
          swConf = swConfMgr.ActiveConfiguration;
          swRootComp = swConf.GetRootComponent3(true);

          if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
            var nomesVisitados = new List<string>();
            TraverseComponent(swRootComp, nomesVisitados, 1);

            MsgBox.Show("Concluido com Sucesso!",
              "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        } else {
          MsgBox.Show($"Comando apenas para Montagens", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar componentes\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void TraverseComponent(Component2 swComp, List<string> nomesVisitados, long nLevel) {
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

          nameShort = Path.GetFileNameWithoutExtension(pathName).ToUpper();
          if (!nomesVisitados.Contains(nameShort)) {
            if (supress == false && exclude == false) {
              ModelDocExtension swModelDocExt = default(ModelDocExtension);
              swModelDocExt = swModel.Extension;
              var swCustPropMgr = swModelDocExt.get_CustomPropertyManager(refConfig);
              swCustPropMgr.Delete2("PINTURA");
              swCustPropMgr.Delete2("CHECK");
              swModel.Save();

              nomesVisitados.Add(nameShort);

              TraverseComponent(swChildComp, nomesVisitados, nLevel + 1);
            } else {
              nomesVisitados.Add(nameShort);
            }
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar componente. + [{nameShort}]\n\n{ex.Message}", "Addin LM Projetos",
             System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }
    }

    private void PegaDadosLista(BomTableAnnotation swBOMAnnotation) {
      try {
        string[] vModelPathNames = null;
        string strItemNumber = null;
        string strPartNumber = null;
        var swTableAnnotation = (TableAnnotation)swBOMAnnotation;
        var comps = new List<string>();

        int lStartRow = 1;

        if (!(swTableAnnotation.TitleVisible == false)) {
          lStartRow = 2;
        }

        var swBOMFeature = swBOMAnnotation.BomFeature;
        var totalRowsCount = swTableAnnotation.TotalRowCount;
        ModelDoc2 swModel = default(ModelDoc2);

        for (int i = lStartRow; i < totalRowsCount; i++) {
          double perc = (i / (double)totalRowsCount) * 100;

          Invoke(new MethodInvoker(delegate () {
            lblInfo.Text = $"{Math.Round(perc, 1):0.0}%";
            lblInfo.Refresh();
          }));

          vModelPathNames = (string[])swBOMAnnotation.GetModelPathNames(i, out strItemNumber, out strPartNumber);
          string ptNm = vModelPathNames[0];

          var codMat = swTableAnnotation.get_Text(i, 2);

          if (string.IsNullOrEmpty(codMat)) {
            var configName = swTableAnnotation.get_Text(i, 14);

            var descricao = Path.GetFileNameWithoutExtension(ptNm).ToUpper();

            if (!comps.Any(x => x == Path.GetFileNameWithoutExtension(ptNm).ToUpper())) {
              comps.Add(descricao);

              int status = 0;
              int warnings = 0;
              swModel = Sw.App.OpenDoc6(ptNm,
                  Path.GetExtension(ptNm).ToUpper() == "SLDPRT" ? (int)swDocumentTypes_e.swDocPART : (int)swDocumentTypes_e.swDocASSEMBLY,
                   (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

              if (swModel != null) {
                var swModelDocExt = swModel.Extension;

                var swCustPropMgr = swModelDocExt.get_CustomPropertyManager(configName);
                swCustPropMgr.Delete2("PINTURA");
                swCustPropMgr.Delete2("CHECK");
                swModel.Save();
                Sw.App.CloseDoc(ptNm);
              }

            }
          }
        }
        Invoke(new MethodInvoker(delegate () {
          lblInfo.Text = "Concluido";
        }));

        swModel = (ModelDoc2)Sw.App.ActiveDoc;
        swModel.Save();

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista Plano Pintura\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

  }
}
