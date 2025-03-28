using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using LmCorbieUI;
using LmCorbieUI.LmForms;

namespace AddinArtama {
  public partial class FrmFileProperties : LmSingleForm {
   string pastaProjeto = "";
    string nameShort = "";
    List<string> lista = new List<string>();

    public FrmFileProperties() {
      InitializeComponent();
    }

    private void FrmFileProperties_Loaded(object sender, EventArgs e) {
      Invoke(new MethodInvoker(delegate () {
        var dados = usuarios.Selecionar(ativo: true);

        cmbProjetista.CarregarComboBox(dados);
        cmbDesenhista.CarregarComboBox(dados);
        cmbProjetista.SelectedValue = usuario_alocados.model.usuario.id;
        cmbDesenhista.SelectedValue = usuario_alocados.model.usuario.id;
      }));
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      MsgBox.ShowWaitMessage("Inserindo Propriedades Personalizadas...");
      try {
        if (Sw.App.ActiveDoc == null) {
          MsgBox.Show($"Sem Documentos Abertos", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        string pathName = swModel.GetPathName();

        pastaProjeto = Path.GetDirectoryName(pathName);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          AddPropriedades(swModel);

          AtualizarPropriedades();
          lista.Clear();

          if (rdbPasta.Checked)
            MsgBox.Show($"Os componentes que estão na pasta\n\n{pastaProjeto}\n\nForam Atualizados com Sucesso.", "Sucesso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
          else if (rdbNome.Checked)
            MsgBox.Show($"Os componentes que o nome começa com\n\n{txtNome.Text}\n\nForam Atualizados com Sucesso.", "Sucesso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        } else if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
          AddPropriedades(swModel);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar componentes\n\n{ex.Message}", "Addin LM Projetos",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void BtnClose_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void AtualizarPropriedades() {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        ConfigurationManager swConfMgr;
        Configuration swConf;
        Component2 swRootComp;

        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        swRootComp = swConf.GetRootComponent3(true);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          TraverseComponent(swRootComp, 1);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar propriedades\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void TraverseComponent(Component2 swComp, long nLevel) {
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

          bool readOnly = swModel.IsOpenedReadOnly();

          if (supress == false && exclude == false /*&& !pathName.Contains(@"PROJETOS\COMPRADOS\ELEMENTOS DE FIXACAO")*/) {
            bool canAdd = true;

            foreach (string s in lista) {
              if (s == nameShort) {
                canAdd = false;
                break;
              }
            }

            if (canAdd == true) {
              //pathName = DadosArtama.ChangePathName(pathName);

              if (rdbPasta.Checked && pathName.Contains(pastaProjeto)) {
                AddPropriedades(swModel);
                lista.Add(nameShort);
              } else if (rdbNome.Checked && nameShort.StartsWith(txtNome.Text.Trim())) {
                AddPropriedades(swModel);
                lista.Add(nameShort);
              }
            }

            TraverseComponent(swChildComp, nLevel + 1);
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar componente [{nameShort}]\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void AddPropriedades(ModelDoc2 swModel) {
      try {
        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        swCustPropMgr.Add3("Projetista", (int)swCustomInfoType_e.swCustomInfoText, cmbProjetista.Text, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        swCustPropMgr.Add3("Data do Projeto", (int)swCustomInfoType_e.swCustomInfoDate, dtpDataProjeto.Text, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        swCustPropMgr.Add3("Desenhista", (int)swCustomInfoType_e.swCustomInfoText, cmbDesenhista.Text, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        swCustPropMgr.Add3("Data do Desenho", (int)swCustomInfoType_e.swCustomInfoDate, dtpDataDesenho.Text, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        swModel.Save();
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao adicionar propriedade\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }
}
