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
using System.Threading.Tasks;
using System.Globalization;

namespace AddinArtama {
  public partial class FrmFormatosAtualizar : LmSingleForm {
    SortableBindingList<DesenhosAtualizar> _dadosDesenho = new SortableBindingList<DesenhosAtualizar>();

    public FrmFormatosAtualizar() {
      InitializeComponent();

      dgv.MontarGrid<DesenhosAtualizar>();

      dgv.Grid.ReadOnly = false;
      for (int i = 1; i < dgv.Grid.Columns.Count; i++) {
        dgv.Grid.Columns[i].ReadOnly = true;
      }
    }

    private void FrmFormatosAtualizar_Loaded(object sender, EventArgs e) {
    }

    private void BtnCarrDesenhos_Click(object sender, EventArgs e) {
      CarregarDesenhosAsync();
    }

    private async Task CarregarDesenhosAsync() {
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

        btnCarregar.Enabled = btnAtualizar.Enabled = false;

        await Loader.ShowDuringOperation(async (progress) => {
          progress.Report("Iniciando leitura Componentes");
          _dadosDesenho = await DesenhosAtualizar.GetDesenhosAsync();
        });

        await Loader.ShowDuringOperation((progress) => {
          progress.Report("Carregando Grid...");
          CarregarGrid();
          return Task.CompletedTask;
        });
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar desenhos\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        btnCarregar.Enabled = btnAtualizar.Enabled = true;
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

      AtualizarAsync();
    }

    private async Task AtualizarAsync() {
      try {
        btnCarregar.Enabled =
        btnAtualizar.Enabled = false;

        var swModel = default(ModelDoc2);
        //var swModelTemplate = default(ModelDoc2);

        templates.Carregar();

        int status = 0;
        int warnings = 0;

        //Sw.App.OpenDoc6(templates.model.formato_a4r, (int)swDocumentTypes_e.swDocDRAWING,
        //    (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

        //Sw.App.ActivateDoc2(templates.model.formato_a4r, false, 0);
        //swModelTemplate = (ModelDoc2)Sw.App.ActiveDoc;

        //FormatoPadrao.GetDefaultFileProps(swModelTemplate);
        //Sw.App.CloseDoc(templates.model.formato_a4r);

        await Loader.ShowDuringOperation(
            "Iniciando leitura da tabela...",
            (progress2) => {
              var total = _dadosDesenho.Count;
              for (int i = dgv.Grid.CurrentRow.Index; i <= _dadosDesenho.Count - 1; i++) {
                if (!Loader._isWorking) {
                  MsgBox.Show("Operação cancelada pelo usuário.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  return Task.FromResult("Cancelado");
                }

                UIThreadHelper.Invoke(dgv.Grid, () => {
                  dgv.Grid.Rows[i].Cells[1].Selected = true;
                });

                var file = (DesenhosAtualizar)dgv.Grid.CurrentRow.DataBoundItem;

                if (file.Atualizar) {
                  progress2.Report(($"Atualizando {file.ShortName}", i + 1, total));
                  try {
                    Sw.App.OpenDoc6(file.PathName, (int)swDocumentTypes_e.swDocDRAWING,
                                           (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
                  } catch (Exception ex) {
                    Toast.Error($"Erro ao abrir arquivo \"{file.PathName}\"\n\n{ex.Message}");
                  }

                  Sw.App.ActivateDoc2(file.PathName, false, 0);
                  swModel = (ModelDoc2)Sw.App.ActiveDoc;

                  // FormatoPadrao.ChangeFileProps(swModel);
                  UpdateFormato(swModel);

                  swModel.Save();
                  Sw.App.CloseDoc(file.PathName);
                }
              }
              return Task.FromResult("concluído");
            },
            100
        );

        MsgBox.Show($"Formatos de folha atualizados com sucesso!\n",
        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar template\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        btnCarregar.Enabled =
        btnAtualizar.Enabled = true;
      }
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

        // Limpar tabelas existentes em todas as folhas
        Desenho.ClearExistingTables(swModel, out bool hasExistingTable);

        if (!hasExistingTable) return;

        var activeSheetName = string.Empty;

        // Processar todas as folhas
        for (int i = 0; i < sheetNames.Length; i++) {
          string sheetName = sheetNames[i];

          if (string.IsNullOrEmpty(activeSheetName))
            activeSheetName = sheetName;

          // Ativar a folha atual
          bool sheetActivated = swDraw.ActivateSheet(sheetName);
          if (!sheetActivated) continue;

          Sheet swSheet = swDraw.GetCurrentSheet();
          if (swSheet == null) continue;

          // Atualizar formato da folha
          UpdateSheetFormat(swDraw, swSheet);

          // Inserir lista de materiais apenas na primeira folha
          if (i == 0) {
            Desenho.InsertMaterialsList(swModel);
          } else if (i > 0) {
            string nomeFolha = sheetName.ToUpper();

            if (nomeFolha.StartsWith("P") && int.TryParse(nomeFolha.Substring(1), out int posicaoDesejada)) {
              Desenho.InsertMaterialsList(swModel, posicaoDesejada);
            }
          }
        }
        if (!string.IsNullOrEmpty(activeSheetName))
          swDraw.ActivateSheet(activeSheetName);

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

    private void Dgv_ProcurarTextChanged(object sender, EventArgs e) {
      CarregarGrid();
    }
  }
}
