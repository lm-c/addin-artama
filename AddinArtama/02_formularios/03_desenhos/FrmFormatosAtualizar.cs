using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using System.Diagnostics;
using LmCorbieUI.Metodos;

namespace AddinArtama {
  public partial class FrmFormatosAtualizar : LmSingleForm {
    SortableBindingList<DesenhosAtualizar> _dadosDesenho = new SortableBindingList<DesenhosAtualizar>();
    bool imprimindo = false;

    public FrmFormatosAtualizar() {
      InitializeComponent();

      dgv.MontarGrid<DesenhosAtualizar>();

      dgv.Grid.ReadOnly = false;
      for (int i = 1; i < dgv.Grid.Columns.Count - 1; i++) {
        dgv.Grid.Columns[i].ReadOnly = true;
      }
    }

    private void FrmFormatosAtualizar_Loaded(object sender, EventArgs e) {
    }

    private void BtnCarrDesenhos_Click(object sender, EventArgs e) {
      try {
        Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog {
          IsFolderPicker = true,
          // InitialDirectory = ValPadrao.PastaArquivo,
          RestoreDirectory = true,
          Title = "Selecionar Diretório para atualizar desenhos"
        };

        if (dialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok) {
          var listaDesenhos = new List<DesenhosAtualizar>();
          int pos = 1;
          var files = Directory.GetFiles(dialog.FileName);
          foreach (string file in files) {
            if (Path.GetExtension(file).ToUpper() == ".SLDDRW") {
              var shorName = Path.GetFileNameWithoutExtension(file);
              if (!shorName.StartsWith("~"))
                listaDesenhos.Add(new DesenhosAtualizar { Atualizar = true, Id = pos, PathName = file, ShorName = shorName });
              pos++;
            }
          }
          _dadosDesenho = new SortableBindingList<DesenhosAtualizar>(listaDesenhos);
          dgv.CarregarGrid(_dadosDesenho);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar desenhos\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {

      }
    }

    private void BtnAtualizar_Click(object sender, EventArgs e) {
      if (_dadosDesenho.Count == 0) {
        Toast.Warning("Carregar Desenhos primeiro");
        return;
      }

      System.Threading.Thread t = new System.Threading.Thread(() => { Atualizar(); }) { IsBackground = true };
      t.Start();
    }

    private void Atualizar() {
      try {
        Invoke(new MethodInvoker(() => {
          btnCancelar.Enabled = true;
          btnCarregar.Enabled = false;
          btnAtualizar.Enabled = false;

          imprimindo = true;

          var swModel = default(ModelDoc2);
          var swModelTemplate = default(ModelDoc2);

          templates.Carregar();

          int status = 0;
          int warnings = 0;

          Sw.App.OpenDoc6(templates.model.formato_a4r, (int)swDocumentTypes_e.swDocDRAWING,
              (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

          Sw.App.ActivateDoc2(templates.model.formato_a4r, false, 0);
          swModelTemplate = (ModelDoc2)Sw.App.ActiveDoc;

          FormatoPadrao.GetDefaultFileProps(swModelTemplate);
          Sw.App.CloseDoc(templates.model.formato_a4r);

          for (int i = dgv.Grid.CurrentRow.Index; i <= _dadosDesenho.Count - 1; i++) {
            if (!imprimindo)
              break;

            dgv.Grid.Rows[i].Cells[1].Selected = true;

            var file = (DesenhosAtualizar)dgv.Grid.CurrentRow.DataBoundItem;

            if (file.Atualizar) {
              try {
                Sw.App.OpenDoc6(file.PathName, (int)swDocumentTypes_e.swDocDRAWING,
                                       (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
              } catch (Exception ex) {
                Toast.Error($"Erro ao abrir arquivo \"{file.PathName}\"\n\n{ex.Message}");
              }

              Sw.App.ActivateDoc2(file.PathName, false, 0);
              swModel = (ModelDoc2)Sw.App.ActiveDoc;

              UpdateFormato(swModel);
              FormatoPadrao.ChangeFileProps(swModel);

              swModel.Save();
              Sw.App.CloseDoc(file.PathName);
            }
          }

          Sw.App.CloseDoc(templates.model.formato_a4r);

          if (imprimindo) {
            MsgBox.Show($"Formatos de folha atualizados com sucesso!\n",
            "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            BtnCancelar_Click(btnCancelar, new EventArgs());
          } else {
            Toast.Success("Rotina cancelada pelo usuário antes do término!");
          }
        }));
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar template\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void BtnCancelar_Click(object sender, EventArgs e) {
      btnCancelar.Enabled = false;
      btnCarregar.Enabled = true;
      btnAtualizar.Enabled = true;

      imprimindo = false;
    }

    private void TsmSelectAll_Click(object sender, EventArgs e) {
      foreach (DataGridViewRow row in dgv.Grid.Rows)
        row.Cells[0].Value = true;
    }

    private void TsmUnselectAll_Click(object sender, EventArgs e) {
      foreach (DataGridViewRow row in dgv.Grid.Rows)
        row.Cells[0].Value = false;
    }

    private void TsmOpen3D_Click(object sender, EventArgs e) {
      try {
        string fileName = "", openFileNamePart = "", openFileNameAssembly = "";
        int status = 0;
        int warnings = 0;
        fileName = ((DesenhosAtualizar)dgv.Grid.CurrentRow.DataBoundItem).PathName;

        openFileNamePart = fileName.Replace("SLDDRW", "SLDPRT");
        openFileNameAssembly = fileName.Replace("SLDDRW", "SLDASM");


        if (File.Exists(openFileNamePart)) {
          Sw.App.OpenDoc6(openFileNamePart, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(openFileNamePart, false, (int)errors);
        } else if (File.Exists(openFileNameAssembly)) {
          Sw.App.OpenDoc6(openFileNameAssembly, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(openFileNameAssembly, false, (int)errors);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao abrir arquivo 3D\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void TsmOpen2D_Click(object sender, EventArgs e) {
      try {
        string fileName = "";
        int status = 0;
        int warnings = 0;
        fileName = ((DesenhosAtualizar)dgv.Grid.CurrentRow.DataBoundItem).PathName;


        if (File.Exists(fileName)) {
          Sw.App.OpenDoc6(fileName, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(fileName, false, (int)errors);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao abrir arquivo 2D\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public static void UpdateFormato(ModelDoc2 swModel) {
      try {
        DrawingDoc swDraw;
        swDraw = (DrawingDoc)swModel;
        swDraw = (DrawingDoc)Sw.App.ActiveDoc;
        Sheet swSheet = swDraw.GetCurrentSheet();
        double[] vSheetProps = swSheet.GetProperties();

        var largura = Convert.ToDouble(vSheetProps[5]) * 1000;
        var altura = Convert.ToDouble(vSheetProps[6]) * 1000;

        var format = Desenho.GetFormat(largura, altura);
        bool boolstatus;
        switch (format) {
          case SwDwgPaperSizes_e.A4R: {
              largura = 0.42;
              altura = 0.297;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a3, largura, altura, "'", true);

              largura = 0.21;
              altura = 0.297;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a4r, largura, altura, "'", true);
            }
            break;
          case SwDwgPaperSizes_e.A4P: {
              largura = 0.42;
              altura = 0.297;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a3, largura, altura, "'", true);

              largura = 0.297;
              altura = 0.21;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a4p, largura, altura, "'", true);
            }
            break;
          case SwDwgPaperSizes_e.A3: {
              largura = 0.21;
              altura = 0.297;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a4r, largura, altura, "'", true);

              largura = 0.42;
              altura = 0.297;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a3, largura, altura, "'", true);
            }
            break;
          case SwDwgPaperSizes_e.A2: {
              largura = 0.42;
              altura = 0.297;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a3, largura, altura, "'", true);

              largura = 0.594;
              altura = 0.42;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a2, largura, altura, "'", true);
            }
            break;
          case SwDwgPaperSizes_e.A1: {
              largura = 0.594;
              altura = 0.42;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a2, largura, altura, "'", true);

              largura = 0.840;
              altura = 0.594;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a1, largura, altura, "'", true);
            }
            break;
          default:
          break;
        }

        swModel.ClearSelection2(true);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          var swFeature = (Feature)swModel.FirstFeature();

          while ((swFeature != null)) {
            string nm = swFeature.GetTypeName2();
            if (nm == "WeldmentTableFeat" || nm == "BomFeat") {
              swFeature.Select(true);

              swModel.EditDelete();
            }

            if (nm == "DrSheet") {
              Feature subFeature = swFeature.GetFirstSubFeature();
              while ((subFeature != null)) {
                string nm2 = subFeature.GetTypeName2();
                if (nm2 == "GeneralTableFeature") {
                  subFeature.Select(true);

                  swModel.EditDelete();
                }

                subFeature = (Feature)subFeature.GetNextSubFeature();
              }
            }

            swFeature = (Feature)swFeature.GetNextFeature();
          }

          if (File.Exists(swModel.GetPathName().ToLower().Replace("slddrw", "sldprt")))
            InserirListasMateriais(swDocumentTypes_e.swDocPART);
          else
            InserirListasMateriais(swDocumentTypes_e.swDocASSEMBLY);

          swModel.ViewZoomtofit2();

          //bool boolstatus = swModel.Extension.SelectByID2("Tabela de revisão1", "DRAWINGVIEW", 0, 0, 0.0, false, 0, null, 0);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao alterar atualizar formato de folha do desenho\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void InserirListasMateriais(swDocumentTypes_e swDocumentTypes_E) {
      try {
        DrawingDoc swDraw = default(DrawingDoc);
        swDraw = (DrawingDoc)Sw.App.ActiveDoc;

        BomTableAnnotation swBOMAnnotation = default(BomTableAnnotation);
        WeldmentCutListAnnotation WMTable = default(WeldmentCutListAnnotation);

        SolidWorks.Interop.sldworks.View swView = (SolidWorks.Interop.sldworks.View)swDraw.GetFirstView();
        swView = (SolidWorks.Interop.sldworks.View)swView.GetNextView();
        swDraw.ActivateView(swView.GetName2());

        // Obtém a configuração ativa
        ModelDoc2 swModel2 = (ModelDoc2)swView.ReferencedDocument;
        string activeConfiguration = swModel2.GetActiveConfiguration().Name;

        int AnchorType = (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomRight;
        int BomType = (int)swBomType_e.swBomType_TopLevelOnly;

        if (swDocumentTypes_E == swDocumentTypes_e.swDocPART) {
          WMTable = swView.InsertWeldmentTable(true, 0, 0, AnchorType, "", templates.model.lista_soldagem);
        } else {
          swBOMAnnotation = swView.InsertBomTable3(true, 0, 0, AnchorType, BomType, activeConfiguration, templates.model.lista_montagem, false);
        }
      } catch (Exception ex) {
        //MsgBox.Show($"Erro ao inserir Lista\n\n{ex.Message}", "Addin LM Projetos",
        //     MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void dgv_ProcurarTextChanged(object sender, EventArgs e) {
      dgv.CarregarGrid(_dadosDesenho);
    }
  }
}
