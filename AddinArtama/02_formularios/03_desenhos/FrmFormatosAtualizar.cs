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
using System.Data.Entity.Infrastructure;

namespace AddinArtama {
  public partial class FrmFormatosAtualizar : LmSingleForm {
    SortableBindingList<DesenhosAtualizar> _dadosDesenho = new SortableBindingList<DesenhosAtualizar>();
    bool imprimindo = false;

    public FrmFormatosAtualizar() {
      InitializeComponent();

      dgv.MontarGrid<DesenhosAtualizar>();

      dgv.Grid.ReadOnly = false;
      for (int i = 1; i < dgv.Grid.Columns.Count ; i++) {
        dgv.Grid.Columns[i].ReadOnly = true;
      }
    }

    private void FrmFormatosAtualizar_Loaded(object sender, EventArgs e) {
    }

    private void BtnCarrDesenhos_Click(object sender, EventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY) {
          Toast.Warning("Comando apenas para Montagens");
          return;
        }

        _dadosDesenho = DesenhosAtualizar.GetDesenhos();

        CarregarGrid();
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar desenhos\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {

      }
    }

    private void CarregarGrid() {
      dgv.CarregarGrid(_dadosDesenho);
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
        DrawingDoc swDraw = (DrawingDoc)swModel;
        string[] sheetNames = swDraw.GetSheetNames();

        if (sheetNames == null || sheetNames.Length == 0) return;

        // Processar todas as folhas
        for (int i = 0; i < sheetNames.Length; i++) {
          string sheetName = sheetNames[i];

          // Ativar a folha atual
          bool sheetActivated = swDraw.ActivateSheet(sheetName);
          if (!sheetActivated) continue;

          Sheet swSheet = swDraw.GetCurrentSheet();
          if (swSheet == null) continue;

          // Atualizar formato da folha
          UpdateSheetFormat(swDraw, swSheet);

          // Limpar tabelas existentes em todas as folhas
          ClearExistingTables(swModel, out bool hasExistingTable);

          // Inserir lista de materiais apenas na primeira folha
          if (i == 0 && hasExistingTable) {
            InsertMaterialsList(swModel);
          }
        }

        swModel.ViewZoomtofit2();

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar formato de folhas do desenho\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void UpdateSheetFormat(DrawingDoc swDraw, Sheet swSheet) {
      double[] vSheetProps = swSheet.GetProperties();
      if (vSheetProps == null || vSheetProps.Length < 7) return;

      var largura = Convert.ToDouble(vSheetProps[5]) * 1000;
      var altura = Convert.ToDouble(vSheetProps[6]) * 1000;

      var format = Desenho.GetFormat(largura, altura);
      ApplySheetFormat(swDraw, swSheet, format, vSheetProps);
    }

    private static void ApplySheetFormat(DrawingDoc swDraw, Sheet swSheet, SwDwgPaperSizes_e format, double[] vSheetProps) {
      string sheetName = swSheet.GetName();

      switch (format) {
        case SwDwgPaperSizes_e.A4R:
        // Transição A3 -> A4R
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.42, 0.297, templates.model.template_a3,
            0.21, 0.297, templates.model.template_a4r);
        break;

        case SwDwgPaperSizes_e.A4P:
        // Transição A3 -> A4P
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.42, 0.297, templates.model.template_a3,
            0.297, 0.21, templates.model.template_a4p);
        break;

        case SwDwgPaperSizes_e.A3:
        // Transição A4R -> A3
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.21, 0.297, templates.model.template_a4r,
            0.42, 0.297, templates.model.template_a3);
        break;

        case SwDwgPaperSizes_e.A2:
        // Transição A3 -> A2
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.42, 0.297, templates.model.template_a3,
            0.594, 0.42, templates.model.template_a2);
        break;

        case SwDwgPaperSizes_e.A1:
        // Transição A2 -> A1
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.594, 0.42, templates.model.template_a2,
            0.840, 0.594, templates.model.template_a1);
        break;
      }
    }

    private static void SetupSheetTransition(DrawingDoc swDraw, string sheetName, double[] vSheetProps,
    double tempWidth, double tempHeight, string tempTemplate,
    double finalWidth, double finalHeight, string finalTemplate) {

      // Configuração temporária
      bool boolstatus = swDraw.SetupSheet5(sheetName, 12, 12, vSheetProps[2], vSheetProps[3],
          true, tempTemplate, tempWidth, tempHeight, "'", true);

      // Configuração final
      boolstatus = swDraw.SetupSheet5(sheetName, 12, 12, vSheetProps[2], vSheetProps[3],
          true, finalTemplate, finalWidth, finalHeight, "'", true);
    }

    private static void ClearExistingTables(ModelDoc2 swModel, out bool hasExistingTable) {
      swModel.ClearSelection2(true);
      hasExistingTable = false;

      if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
        return;
      }

      var swFeature = (Feature)swModel.FirstFeature();

      while (swFeature != null) {
        string featureType = swFeature.GetTypeName2();

        // Verificar se existe tabela de soldagem ou BOM antes de remover
        if (featureType == "WeldmentTableFeat" || featureType == "BomFeat") {
          hasExistingTable = true;
          swFeature.Select(true);
          swModel.EditDelete();
        }

        // Processar sub-features de folhas
        if (featureType == "DrSheet") {
          if (ProcessSheetSubFeatures(swModel, swFeature)) {
            hasExistingTable = true;
          }
        }

        swFeature = (Feature)swFeature.GetNextFeature();
      }
    }

    private static bool ProcessSheetSubFeatures(ModelDoc2 swModel, Feature sheetFeature) {
      Feature subFeature = sheetFeature.GetFirstSubFeature();
      bool foundTable = false;

      while (subFeature != null) {
        string subFeatureType = subFeature.GetTypeName2();

        if (subFeatureType == "GeneralTableFeature") {
          foundTable = true;
          subFeature.Select(true);
          swModel.EditDelete();
        }

        subFeature = (Feature)subFeature.GetNextSubFeature();
      }

      return foundTable;
    }

    private static void InsertMaterialsList(ModelDoc2 swModel) {
      string modelPath = swModel.GetPathName().ToLower();

      if (File.Exists(modelPath.Replace("slddrw", "sldprt"))) {
        InserirListasMateriais(swDocumentTypes_e.swDocPART);
      } else {
        InserirListasMateriais(swDocumentTypes_e.swDocASSEMBLY);
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

    private void Dgv_ProcurarTextChanged(object sender, EventArgs e) {
      CarregarGrid();
    }

    private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e) {
      //if (dgv.Grid.CurrentRow == null)
      //  return;

      //if (e.RowIndex != -1) {
      //  var item = (DesenhosAtualizar)dgv.Grid.CurrentRow.DataBoundItem;

      //  if (e.ColumnIndex == dgv.Grid.Columns["Atualizar"].Index)
      //    item.Atualizar = !item.Atualizar;
      //}

      //dgv.Grid.Refresh();
    }
  }
}
