using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using System.Diagnostics;

namespace AddinArtama {
  public partial class FrmFormatosAtualizar : LmSingleForm {
    BindingSource dadosDesenho = new BindingSource();
    bool imprimindo = false;

    public FrmFormatosAtualizar() {
      InitializeComponent();

      dadosDesenho.CurrentChanged += DadosDraw_CurrentChanged;
    }

    private void DadosDraw_CurrentChanged(object sender, EventArgs e) {
      try {
        if (dadosDesenho.Count > 0) {
          lblPercDesenho.Text = (dadosDesenho.IndexOf(dadosDesenho.Current) + 1) + " de " + dadosDesenho.Count +
              " - " + (((dadosDesenho.IndexOf(dadosDesenho.Current) + 1) * 100) / dadosDesenho.Count) + "%";
        }
      } catch (Exception) {

      }
    }

    private void FrmFormatosAtualizar_Loaded(object sender, EventArgs e) {
    }
    private void FormatarGrid() {
      dgv.Grid.Columns["Atualizar"].Width = 30;
      dgv.Grid.Columns["Id"].Width = 60;
      dgv.Grid.Columns["ShorName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

      dgv.Grid.Columns["Atualizar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["Atualizar"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["Id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

      dgv.Grid.ReadOnly = false;
      dgv.Grid.Columns["Id"].ReadOnly = true;
      dgv.Grid.Columns["ShorName"].ReadOnly = true;
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
              listaDesenhos.Add(new DesenhosAtualizar { Atualizar = true, Id = pos, PathName = file, ShorName = Path.GetFileNameWithoutExtension(file) });
              pos++;
            }
          }
          dadosDesenho.DataSource = listaDesenhos;
          dgv.Grid.DataSource = dadosDesenho;
          dadosDesenho.MoveFirst();
          FormatarGrid();
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar desenhos\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {

      }
    }

    private void BtnAtualizar_Click(object sender, EventArgs e) {
      if (dadosDesenho.Count == 0) return;

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

          for (int i = dadosDesenho.Position; i <= dadosDesenho.Count; i++) {
            if (!imprimindo)
              break;

            dadosDesenho.Position = i;

            var file = (DesenhosAtualizar)dadosDesenho.Current;

            if (file.Atualizar) {
              try {
                Sw.App.OpenDoc6(file.PathName, (int)swDocumentTypes_e.swDocDRAWING,
                                       (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
              } catch (Exception ex) {
                MsgBox.Show($"Erro ao abrir arquivo \"{file.PathName}\"\n\n{ex.Message}", "Addin LM Projetos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
              }

              Sw.App.ActivateDoc2(file.PathName, false, 0);
              swModel = (ModelDoc2)Sw.App.ActiveDoc;

              ChangeFileProps(swModel, swModelTemplate);
              UpdateFormato(swModel);

              swModel.Save();
              Sw.App.CloseDoc(file.PathName);
            }
          }

          Sw.App.CloseDoc(templates.model.formato_a4r);

          if (imprimindo) {
            Toast.Success("Formatos de folha atualizados com sucesso!");
            BtnCancelar_Click(btnCancelar, new EventArgs());
          } else {
            Toast.Success("Rotina cancelada pelo usuário antes do término!");
          }
        }));
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar tempalte\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void BtnCancelar_Click(object sender, EventArgs e) {
      btnCancelar.Enabled = false;
      btnCarregar.Enabled = true;
      btnAtualizar.Enabled = true;

      imprimindo = false;
    }

    private void BtnClose_Click(object sender, EventArgs e) {
      this.Close();
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
        fileName = ((DesenhosAtualizar)dadosDesenho.Current).PathName;

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
        fileName = ((DesenhosAtualizar)dadosDesenho.Current).PathName;


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
              //largura = 0.115;
              //altura = 0.14;
              largura = 0.297;
              altura = 0.21;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a4p, largura, altura, "'", true);

              largura = 0.21;
              altura = 0.297;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a4r, largura, altura, "'", true);
            }
            break;
          case SwDwgPaperSizes_e.A4P: {
              largura = 0.21;
              altura = 0.297;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a4r, largura, altura, "'", true);

              //largura = 0.115;
              //altura = 0.14;
              largura = 0.297;
              altura = 0.21;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a4p, largura, altura, "'", true);
            }
            break;
          case SwDwgPaperSizes_e.A3: {
              largura = 0.115;
              altura = 0.14;
              boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, templates.model.template_a4p, largura, altura, "'", true);

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
            string nm = swFeature.Name;
            if (swFeature.GetTypeName() == "WeldmentTableFeat" || swFeature.GetTypeName() == "BomFeat") {
              swFeature.Select(true);

              swModel.EditDelete();
            }
            swFeature = (Feature)swFeature.GetNextFeature();
          }
          InserirListasMateriais(swDraw, swModel);

          swModel.ViewZoomtofit2();

          //bool boolstatus = swModel.Extension.SelectByID2("Tabela de revisão1", "DRAWINGVIEW", 0, 0, 0.0, false, 0, null, 0);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao alterar atualizar formato de folha do desenho\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void ChangeFileProps(ModelDoc2 swModel, ModelDoc2 swModelTemplate) {
      try {
        bool boolstatus;

        // exibir/ocultar
        var swDisplayPlanes = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayPlanes);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayPlanes, swDisplayPlanes);

        var swDisplayAxes = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAxes);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAxes, swDisplayAxes);

        var swDisplayTemporaryAxes = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayTemporaryAxes);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayTemporaryAxes, swDisplayTemporaryAxes);

        var swDisplayCoordSystems = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCoordSystems);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCoordSystems, swDisplayCoordSystems);

        var swDisplayReferencePoints2 = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayReferencePoints2);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayReferencePoints2, swDisplayReferencePoints2);

        var swDisplayOrigins = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayOrigins);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayOrigins, swDisplayOrigins);

        var swDisplayDatumCoordSystems = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayDatumCoordSystems);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayDatumCoordSystems, swDisplayDatumCoordSystems);

        var swDisplayCurves = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCurves);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCurves, swDisplayCurves);

        // -============= REMOVER COMENTÁRIO QUANDO ATUALIZAR O SOLID
        //var swDisplayPartingLines = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayPartingLines);
        //boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayPartingLines, swDisplayPartingLines);

        var swDisplayAllAnnotations = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAllAnnotations);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAllAnnotations, swDisplayAllAnnotations);

        var swDisplayCompAnnotations = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCompAnnotations);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCompAnnotations, swDisplayCompAnnotations);

        var swHideShowSketchDimensions = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swHideShowSketchDimensions);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swHideShowSketchDimensions, swHideShowSketchDimensions);

        var swDisplaySketches = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplaySketches);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplaySketches, swDisplaySketches);

        var swViewSketchRelations = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swViewSketchRelations);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swViewSketchRelations, swViewSketchRelations);

        //// -============= REMOVER COMENTÁRIO QUANDO ATUALIZAR O SOLID
        //var swDisplaySketchPlanes = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplaySketchPlanes);
        //boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplaySketchPlanes, swDisplaySketchPlanes);

        var swGridDisplay = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swGridDisplay);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swGridDisplay, swGridDisplay);

        var swDisplayLights = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayLights);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayLights, swDisplayLights);

        var swDisplayCameras = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCameras);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCameras, swDisplayCameras);

        // -============= REMOVER COMENTÁRIO QUANDO ATUALIZAR O SOLID
        //var swDisplayDecals = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayDecals);
        //boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayDecals, swDisplayDecals);

        var swDisplayReferencePoints = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayReferencePoints);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayReferencePoints, swDisplayReferencePoints);

        var swDisplayLiveSections = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayLiveSections);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayLiveSections, swDisplayLiveSections);

        var swShowDimensionNames = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swShowDimensionNames);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swShowDimensionNames, swShowDimensionNames);

        // -============= REMOVER COMENTÁRIO QUANDO ATUALIZAR O SOLID
        //var swDisplayWeldBead = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayWeldBead);
        //boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayWeldBead, swDisplayWeldBead);

        var swDisplayCenterOfMassSymbol = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCenterOfMassSymbol);
        boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCenterOfMassSymbol, swDisplayCenterOfMassSymbol);

        // -============= REMOVER COMENTÁRIO QUANDO ATUALIZAR O SOLID
        //var swViewDispGlobalBBox = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swViewDispGlobalBBox);
        //boolstatus = swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swViewDispGlobalBBox, swViewDispGlobalBBox);

        // text preferences
        var swDetailingDimensionTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingDimension);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingDimension, swDetailingDimensionTextFormat);

        var swSheetMetalBendNotesTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swSheetMetalBendNotesTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swSheetMetalBendNotesTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalBendNotesTextFormat);

        var swDetailingAnnotationTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingAnnotationTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingAnnotationTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingAnnotationTextFormat);

        var swDetailingTableTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingTableTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingTableTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingTableTextFormat);

        var swDetailingViewTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingViewTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingViewTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingViewTextFormat);

        var swDetailingSectionTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionTextFormat);

        var swDetailingSectionLabelNameTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelNameTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelNameTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionLabelNameTextFormat);

        var swDetailingSectionLabelLabelTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelLabelTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelLabelTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionLabelLabelTextFormat);

        var swDetailingSectionLabelScaleTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelScaleTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelScaleTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionLabelScaleTextFormat);

        var swDetailingSectionLabelDelimiterTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelDelimiterTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelDelimiterTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionLabelDelimiterTextFormat);

        var swDetailingSectionView_RotationTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionView_RotationTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionView_RotationTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionView_RotationTextFormat);

        // toogle preferences
        var swDimensionsExtensionLineStyleSameAsLeaderRadius = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDimensionsExtensionLineStyleSameAsLeader, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDimensionsExtensionLineStyleSameAsLeader, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDimensionsExtensionLineStyleSameAsLeaderRadius);

        var swDimensionsExtensionLineStyleSameAsLeaderHole = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDimensionsExtensionLineStyleSameAsLeader, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDimensionsExtensionLineStyleSameAsLeader, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDimensionsExtensionLineStyleSameAsLeaderHole);

        var swDetailingDimsCenterText = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingDimsCenterText, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingDimsCenterText, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingDimsCenterText);

        var swDetailingScaleWithDimHeight = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingScaleWithDimHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingScaleWithDimHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingScaleWithDimHeight);

        var swSheetMetalBendNotesLeaderJustificationSnapping = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swSheetMetalBendNotesLeaderJustificationSnapping, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swSheetMetalBendNotesLeaderJustificationSnapping, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalBendNotesLeaderJustificationSnapping);

        var swDetailingOrthoView_AddViewLabelOnViewCreation = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingOrthoView_AddViewLabelOnViewCreation, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingOrthoView_AddViewLabelOnViewCreation, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingOrthoView_AddViewLabelOnViewCreation);

        var swWeldmentEnableAutomaticCutList = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentEnableAutomaticCutList, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentEnableAutomaticCutList, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swWeldmentEnableAutomaticCutList);

        var swWeldmentEnableAutomaticUpdate = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentEnableAutomaticUpdate, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentEnableAutomaticUpdate, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swWeldmentEnableAutomaticUpdate);

        var swDetailingMiscView_AddViewLabelOnViewCreation = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingMiscView_AddViewLabelOnViewCreation, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingMiscView_AddViewLabelOnViewCreation, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingMiscView_AddViewLabelOnViewCreation);

        var swDisableDerivedConfigurations = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisableDerivedConfigurations, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisableDerivedConfigurations, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDisableDerivedConfigurations);

        var swWeldmentRenameCutlistDescriptionPropertyValue = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentRenameCutlistDescriptionPropertyValue, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentRenameCutlistDescriptionPropertyValue, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swWeldmentRenameCutlistDescriptionPropertyValue);

        var swDisableWeldmentConfigStrings = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisableWeldmentConfigStrings, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisableWeldmentConfigStrings, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDisableWeldmentConfigStrings);

        var swDetailingScaleWithSectionTextHeight = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingScaleWithSectionTextHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingScaleWithSectionTextHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingScaleWithSectionTextHeight);

        var swDetailingSectionViewLabels_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingSectionViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingSectionViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionViewLabels_PerStandard);

        var swDetailingDetailViewLabels_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingDetailViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingDetailViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingDetailViewLabels_PerStandard);

        var swDetailingAuxViewLabels_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingAuxViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingAuxViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingAuxViewLabels_PerStandard);

        var swDetailingOrthoViewLabels_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingOrthoViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingOrthoViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingOrthoViewLabels_PerStandard);

        var swDetailingMiscView_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingMiscView_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingMiscView_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingMiscView_PerStandard);

        // string preferences
        var swSheetMetalBendNotesLayer = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swSheetMetalBendNotesLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swSheetMetalBendNotesLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalBendNotesLayer);

        var swCenterLineLayer = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swCenterLineLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swCenterLineLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swCenterLineLayer);

        var swCenterMarkLayer = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swCenterMarkLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swCenterMarkLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swCenterMarkLayer);

        var swDetailingLayer = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingLayer);

        var swDetailingLayerNote = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingNote);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingNote, swDetailingLayerNote);

        var swDetailingLayerBalloon = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBalloon);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBalloon, swDetailingLayerBalloon);

        var swDetailingLayerAngularRunningDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAngularRunningDimension);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAngularRunningDimension, swDetailingLayerAngularRunningDimension);

        var swDetailingLayerBendTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBendTable);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBendTable, swDetailingLayerBendTable);

        var swDetailingLayerLinearDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingLinearDimension);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingLinearDimension, swDetailingLayerLinearDimension);

        var swDetailingLayerDiameterDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDiameterDimension);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDiameterDimension, swDetailingLayerDiameterDimension);

        var swDetailingLayerHoleDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDetailingLayerHoleDimension);

        var swDetailingLayerChamferDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDetailingLayerChamferDimension);

        var swDetailingLayerOrdinateDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension, swDetailingLayerOrdinateDimension);

        var swDetailingLayerAngleDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAngleDimension);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAngleDimension, swDetailingLayerAngleDimension);

        var swDetailingLayerDatum = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDatum);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDatum, swDetailingLayerDatum);

        var swDetailingLayerSurfaceFinishSymbol = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingSurfaceFinishSymbol);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingSurfaceFinishSymbol, swDetailingLayerSurfaceFinishSymbol);

        var swDetailingLayerGeometricTolerance = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingGeometricTolerance);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingGeometricTolerance, swDetailingLayerGeometricTolerance);

        var swDetailingLayerWeldSymbol = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingWeldSymbol);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingWeldSymbol, swDetailingLayerWeldSymbol);

        var swDetailingLayerBillOfMaterial = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBillOfMaterial);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBillOfMaterial, swDetailingLayerBillOfMaterial);

        var swDetailingLayerRevisionTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRevisionTable);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRevisionTable, swDetailingLayerRevisionTable);

        var swDetailingLayerGeneralTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingGeneralTable);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingGeneralTable, swDetailingLayerGeneralTable);

        var swDetailingLayerSectionView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingSectionView);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingSectionView, swDetailingLayerSectionView);

        var swDetailingLayerDetailView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDetailView);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDetailView, swDetailingLayerDetailView);

        var swDetailingLayerAuxiliaryView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAuxiliaryView);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAuxiliaryView, swDetailingLayerAuxiliaryView);

        var swDetailingLayerHoleTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingHoleTable);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingHoleTable, swDetailingLayerHoleTable);

        var swDetailingLayerArcLengthDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension, swDetailingLayerArcLengthDimension);

        var swDetailingLayerOrthoView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingOrthoView);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingOrthoView, swDetailingLayerOrthoView);

        var swDetailingLayerPunchTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingPunchTable);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingPunchTable, swDetailingLayerPunchTable);

        var swDetailingLayerLocationLabel = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingLocationLabel);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingLocationLabel, swDetailingLayerLocationLabel);

        var swDetailingLayerRevisionCloud = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRevisionCloud);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRevisionCloud, swDetailingLayerRevisionCloud);

        var swDetailingLayerMiscView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingMiscView);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingMiscView, swDetailingLayerMiscView);

        var swDetailingLayerWeldTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingWeldTable);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingWeldTable, swDetailingLayerWeldTable);

        var swDetailingLayerRadiusDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        boolstatus = swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDetailingLayerRadiusDimension);

        // double preferences
        var swDetailingBreakLineGap = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBreakLineGap, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBreakLineGap, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBreakLineGap);

        var swDetailingCenterOfMassSize = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingCenterOfMassSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingCenterOfMassSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingCenterOfMassSize);

        var swDetailingArrowWidth = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowWidth, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowWidth, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingArrowWidth);

        var swDetailingArrowHeight = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingArrowHeight);

        var swDetailingArrowLength = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowLength, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowLength, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingArrowLength);

        var swDetailingObjectToDimOffset = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingObjectToDimOffset, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingObjectToDimOffset, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingObjectToDimOffset);

        var swDetailingDimToDimOffset = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingDimToDimOffset, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingDimToDimOffset, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingDimToDimOffset);

        var swDetailingWitnessLineGap = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingWitnessLineGap, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingWitnessLineGap, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingWitnessLineGap);

        var swDetailingWitnessLineExtension = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingWitnessLineExtension, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingWitnessLineExtension, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingWitnessLineExtension);

        var swDetailingBOMBalloonCustomSize = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBOMBalloonCustomSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBOMBalloonCustomSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBOMBalloonCustomSize);

        var swDetailingBOMStackedBalloonCustomSize = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBOMStackedBalloonCustomSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBOMStackedBalloonCustomSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBOMStackedBalloonCustomSize);

        // integer preferences sem especificação
        var swDetailingNotesLeaderStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingNotesLeaderStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingNotesLeaderStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingNotesLeaderStyle);

        var swDetailingBalloonLeaderStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBalloonLeaderStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBalloonLeaderStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBalloonLeaderStyle);

        var swDetailingBalloonAutoBalloons = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBalloonAutoBalloons, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBalloonAutoBalloons, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBalloonAutoBalloons);

        var swUnitsLinearDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsLinearDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsLinearDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsLinearDecimalPlaces);

        var swUnitsDualLinearDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsDualLinearDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsDualLinearDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsDualLinearDecimalPlaces);

        var swUnitsDualLinear = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsDualLinear, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsDualLinear, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsDualLinear);

        var swUnitsAngular = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsAngular, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsAngular, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsAngular);

        var swUnitsAngularDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsAngularDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsAngularDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsAngularDecimalPlaces);

        var swUnitsMassPropLength = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropLength, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropLength, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsMassPropLength);

        var swUnitsMassPropDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsMassPropDecimalPlaces);

        var swUnitsMassPropMass = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropMass, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropMass, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsMassPropMass);

        var swUnitsMassPropVolume = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropVolume, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropVolume, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsMassPropVolume);

        var swLineFontTangentEdgesStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontTangentEdgesStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontTangentEdgesStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontTangentEdgesStyle);

        var swLineFontBendLineUpStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontBendLineUpStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontBendLineUpStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontBendLineUpStyle);

        var swDetailingAngularDimPrecision = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingAngularDimPrecision);

        var swSheetMetalColorBendLinesUp = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorBendLinesUp, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorBendLinesUp, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalColorBendLinesUp);

        var swSheetMetalColorBendLinesDown = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorBendLinesDown, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorBendLinesDown, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalColorBendLinesDown);

        var swSheetMetalColorFlatPatternSketch = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorFlatPatternSketch, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorFlatPatternSketch, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalColorFlatPatternSketch);

        var swRevisionTableSymbolShape = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swRevisionTableSymbolShape, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swRevisionTableSymbolShape, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swRevisionTableSymbolShape);

        var swRevisionTableTagStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swRevisionTableTagStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swRevisionTableTagStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swRevisionTableTagStyle);

        var swUnitSystem = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitSystem, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitSystem, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitSystem);

        var swUnitsLinear = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsLinear, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsLinear, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsLinear);

        var swUnitsTimeDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsTimeDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsTimeDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsTimeDecimalPlaces);

        var swLineFontSectionLineStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontSectionLineStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontSectionLineStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontSectionLineStyle);

        var swLineFontSectionLineThickness = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontSectionLineThickness, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontSectionLineThickness, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontSectionLineThickness);

        var swDetailingSectionViewLineStyleDisplay = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingSectionViewLineStyleDisplay, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingSectionViewLineStyleDisplay, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionViewLineStyleDisplay);

        var swDetailingVirtualSharpStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingVirtualSharpStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingVirtualSharpStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingVirtualSharpStyle);

        var swLineFontDetailCircleStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontDetailCircleStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontDetailCircleStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontDetailCircleStyle);

        // integer preferences com especificação
        var swDetailingLinearDimPrecisionDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDimension, swDetailingLinearDimPrecisionDimension);

        var swDetailingLinearDimPrecisionDiameterDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDiameterDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDiameterDimension, swDetailingLinearDimPrecisionDiameterDimension);

        var swDetailingLinearDimPrecisionRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDetailingLinearDimPrecisionRadiusDimension);

        var swDetailingLinearDimPrecisionHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDetailingLinearDimPrecisionHoleDimension);

        var swDetailingLinearDimPrecisionChamferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDetailingLinearDimPrecisionChamferDimension);

        var swDetailingLinearDimPrecisionOrdinateDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension, swDetailingLinearDimPrecisionOrdinateDimension);

        var swDetailingLinearDimPrecisionArcLengthDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension, swDetailingLinearDimPrecisionArcLengthDimension);

        var swDetailingLinearDimPrecisionLinearDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingLinearDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingLinearDimension, swDetailingLinearDimPrecisionLinearDimension);

        var swDetailingAltLinearDimPrecisionDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDimension, swDetailingAltLinearDimPrecisionDimension);

        var swDetailingAltLinearDimPrecisionLinearDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingLinearDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingLinearDimension, swDetailingAltLinearDimPrecisionLinearDimension);

        var swDetailingAltLinearDimPrecisionDiameterDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDiameterDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDiameterDimension, swDetailingAltLinearDimPrecisionDiameterDimension);

        var swDetailingAltLinearDimPrecisionRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDetailingAltLinearDimPrecisionRadiusDimension);

        var swDetailingAltLinearDimPrecisionHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDetailingAltLinearDimPrecisionHoleDimension);

        var swDetailingAltLinearDimPrecisionChamferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDetailingAltLinearDimPrecisionChamferDimension);

        var swDetailingAltLinearDimPrecisionOrdinateDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension, swDetailingAltLinearDimPrecisionOrdinateDimension);

        var swDetailingAltLinearDimPrecisionArcLengthDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension, swDetailingAltLinearDimPrecisionArcLengthDimension);

        var swDetailingDimensionTextAndLeaderStyleDiameterDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingDiameterDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingDiameterDimension, swDetailingDimensionTextAndLeaderStyleDiameterDimension);

        var swDetailingDimensionTextAndLeaderStyleRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDetailingDimensionTextAndLeaderStyleRadiusDimension);

        var swDetailingDimensionTextAndLeaderStyleHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDetailingDimensionTextAndLeaderStyleHoleDimension);

        var swDimensionsExtensionLineStyleRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDimensionsExtensionLineStyleRadiusDimension);

        var swDimensionsExtensionLineStyleHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDimensionsExtensionLineStyleHoleDimension);

        var swDimensionsExtensionLineStyleChanferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDimensionsExtensionLineStyleChanferDimension);

        var swDimensionsExtensionLineStyleThicknessRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDimensionsExtensionLineStyleThicknessRadiusDimension);

        var swDimensionsExtensionLineStyleThicknessHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDimensionsExtensionLineStyleThicknessHoleDimension);

        var swDimensionsExtensionLineStyleThicknessChamferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDimensionsExtensionLineStyleThicknessChamferDimension);

        var swDetailingAngularDimPrecisionAngleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingAngleDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingAngleDimension, swDetailingAngularDimPrecisionAngleDimension);

        var swDetailingAngularDimPrecisionChamferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDetailingAngularDimPrecisionChamferDimension);

        var swDetailingDimTrailingZeroDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimTrailingZero, (int)swUserPreferenceOption_e.swDetailingDimension);
        boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimTrailingZero, (int)swUserPreferenceOption_e.swDetailingDimension, swDetailingDimTrailingZeroDimension);

        // -============= REMOVER COMENTÁRIO QUANDO ATUALIZAR O SOLID

        //var swDetailingAngleTrailingZero = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngleTrailingZero, (int)swUserPreferenceOption_e.swDetailingAngleDimension);
        //boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngleTrailingZero, (int)swUserPreferenceOption_e.swDetailingAngleDimension, swDetailingAngleTrailingZero);

        //var swDetailingAngleTrailingZeroTolerance = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngleTrailingZeroTolerance, (int)swUserPreferenceOption_e.swDetailingAngleDimension);
        //boolstatus = swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngleTrailingZeroTolerance, (int)swUserPreferenceOption_e.swDetailingAngleDimension, swDetailingAngleTrailingZeroTolerance);

        swModel.Rebuild((int)swRebuildOptions_e.swForceRebuildAll);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao alterar propriedades do desenho\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void InserirListasMateriais(DrawingDoc swDraw, ModelDoc2 swModel) {
      try {
        BomTableAnnotation swBOMAnnotation = default(BomTableAnnotation);
        WeldmentCutListAnnotation WMTable = default(WeldmentCutListAnnotation);

        SolidWorks.Interop.sldworks.View swView = (SolidWorks.Interop.sldworks.View)swDraw.GetFirstView();
        swView = (SolidWorks.Interop.sldworks.View)swView.GetNextView();
        swDraw.ActivateView(swView.GetName2());

        // Obtém a configuração ativa
        ModelDoc2 swModel2 = (ModelDoc2)swView.ReferencedDocument;
        string activeConfiguration = swModel2.GetActiveConfiguration().Name;

        string tipo = "";
        string nomeArquivo = swModel.GetPathName().ToLower().Replace("slddrw", "sldprt");
        if (File.Exists(nomeArquivo))
          tipo = "peça";

        int AnchorType = (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomLeft;
        int BomType = (int)swBomType_e.swBomType_TopLevelOnly;

        if (tipo == "peça") {
          WMTable = swView.InsertWeldmentTable(true, 0, 0, AnchorType, activeConfiguration, templates.model.lista_soldagem);
        } else {
          swBOMAnnotation = swView.InsertBomTable3(true, 0, 0, AnchorType, BomType, activeConfiguration, templates.model.lista_montagem, false);
        }
      } catch (Exception ex) {
        //MsgBox.Show($"Erro ao inserir Lista\n\n{ex.Message}", "Addin LM Projetos",
        //     MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void FrmFormatosAtualizar_ClickHelp(object sender, EventArgs e) {
      Process.Start("https://youtu.be/BTDiKEoslk8");
    }
  }
}
