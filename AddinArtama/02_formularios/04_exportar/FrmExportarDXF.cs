using System;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;

using SolidWorks.Interop.swconst;

using System.IO;
using System.Threading;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using System.Runtime.InteropServices;

namespace AddinArtama {
  public partial class FrmExportarDXF : LmSingleForm {
    BindingSource dadosDraw = new BindingSource();
    DxfExport dxfExport = new DxfExport();

    string pastaDxf = "";
    bool exportando = false;

    public FrmExportarDXF() {
      InitializeComponent();

      dadosDraw.CurrentChanged += DadosDraw_CurrentChanged;
    }

    private void DadosDraw_CurrentChanged(object sender, EventArgs e) {
      try {
        if (dadosDraw.Count > 0) {
          Invoke(new MethodInvoker(() => {
            lblPercDesenho.Text = (dadosDraw.IndexOf(dadosDraw.Current) + 1) + " de " + dadosDraw.Count +
            " - " + (((dadosDraw.IndexOf(dadosDraw.Current) + 1) * 100) / dadosDraw.Count) + "%";
          }));
        }
      } catch (Exception) {

      }
    }

    private void FormatarGrid() {
      dgv.Grid.Columns["Exportar"].Width = 30;
      dgv.Grid.Columns["CodComponente"].Width = 80;
      dgv.Grid.Columns["Denominacao"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

      dgv.Grid.Columns["Exportar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["Exportar"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["CodComponente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["CodComponente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

      dgv.Grid.Columns["CodComponente"].ReadOnly = true;
      dgv.Grid.Columns["Denominacao"].ReadOnly = true;
    }

    private void BtnCarregar_Click(object sender, EventArgs e) {
      MsgBox.ShowWaitMessage("Lendo componentes da montagem...");
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Info($"Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          MsgBox.Show($"Comando Apenas para Montagem", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          dadosDraw.DataSource = DxfExport.GetDrawing();
          dgv.Grid.DataSource = dadosDraw;
          dadosDraw.MoveFirst();

          FormatarGrid();

          string pathName = swModel.GetPathName();
          string shortName = Path.GetFileNameWithoutExtension(pathName);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar desenhos\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void BtnExportar_Click(object sender, EventArgs e) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (dadosDraw.Count == 0) {
          Toast.Warning($"Favor carrregar componentes primeiro");
          return;
        }

        if (dadosDraw.Count == 0) return;

        string pastaDXF = DrawExport.GetFolder("_DXF", swModel);
        if (!Directory.Exists(pastaDXF))
          Directory.CreateDirectory(pastaDXF);

        btnCancelar.Visible = true;
        btnCarregar.Enabled = false;
        btnExportar.Enabled = false;

        exportando = true;

        Thread t = new Thread(() => { IniciarExportacao(pastaDXF); }) { IsBackground = true };
        t.Start();
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao exportar arquivos\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void IniciarExportacao(string pasta) {
      try {
        for (int i = dadosDraw.Position; i <= dadosDraw.Count; i++) {
          if (!exportando)
            break;

          dadosDraw.Position = i;

          var item = (DxfExport)dadosDraw.Current;

          if (!item.Exportar)
            continue;

          var arquivo = item.PathName;

          if (File.Exists(arquivo) && Path.GetExtension(arquivo).ToUpper() == ".SLDPRT") {
            //Abrir Documento
            int status = 0;
            int warnings = 0;
            Sw.App.OpenDoc6(arquivo, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
            int errors = 0;
            Sw.App.ActivateDoc2(arquivo, false, (int)errors);

            var swModel = (ModelDoc2)Sw.App.ActiveDoc;
            swModel.ViewZoomtofit2();
            swModel.ForceRebuild3(true);

            var pastaNova = $"{pasta}{item.CodigoMaterial} - {item.DescricaoMaterial.Replace("\"", "").Replace("/", "-")}\\";
            if (!Directory.Exists(pastaNova))
              Directory.CreateDirectory(pastaNova);

            GerarDXF(swModel, item, pastaNova);
            Sw.App.CloseDoc(arquivo); //fechar arquivo SolidWorks
          }
        }

        if (exportando) {
          MsgBox.Show($"Arquivos DXF gerados com sucesso!\n\n{pasta}",
              "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

          Invoke(new MethodInvoker(() => {
            BtnCancelar_Click(btnCancelar, new EventArgs());
          }));
        } else {
          MsgBox.Show($"Rotina cancelada pelo usuário antes do término!",
              "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao exportar arquivos\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    // gerar arquivos em dxf
    private void GerarDXF(ModelDoc2 swModel, DxfExport item, string pasta) {
      try {
        int numViews;
        object[] viewNames;
        string viewPaletteName;
        int i;
        string formatoNovo = $"{Application.StartupPath}\\01 - Addin LM\\FORMATO EM BRANCO.drwdot";
        string nomeSaida = "";
        string arquivoAbert = "";

        string fileName = swModel.GetPathName();

        DrawingDoc swDrawing;
        SolidWorks.Interop.sldworks.View swView;


        var swModelDocExt = swModel.Extension;
        Feature swFeat = default;

        swFeat = (Feature)swModel.FirstFeature();

        while (swFeat != null) {
          swFeat = (Feature)swFeat.GetNextFeature();
        }

        swFeat = (Feature)swModel.FirstFeature();

        try {
          while (swFeat != null) {
            //    MsgBox.Show(swFeat.GetTypeName());
            if (swFeat.GetTypeName() == "FlatPattern") {
              //  planificar
              var nameFeat = swFeat.Name;
              var typeFeat = swFeat.GetTypeName2();

              bool boolstatus = swModel.Extension.SelectByID2(nameFeat, typeFeat, 0, 0, 0, false, 0, null, 0);
              bool status = swModel.EditUnsuppress2();

              swDrawing = (DrawingDoc)Sw.App.NewDocument(formatoNovo, 5, 0.1, 0.1);

              swModel = (ModelDoc2)Sw.App.ActiveDoc;
              swDrawing = (DrawingDoc)swModel;
              swModelDocExt = swModel.Extension;
              swDrawing = (DrawingDoc)Sw.App.ActiveDoc;

              swDrawing.GenerateViewPaletteViews(fileName);
              numViews = 0;
              viewNames = (object[])swDrawing.GetDrawingPaletteViewNames();

              if (!((viewNames == null))) {
                numViews = (viewNames.GetUpperBound(0) - viewNames.GetLowerBound(0));
                for (i = 0; i <= numViews; i++) {
                  viewPaletteName = (string)viewNames[i];

                  if ((viewPaletteName == "Padrão plano")) {
                    swView = (SolidWorks.Interop.sldworks.View)swDrawing.DropDrawingViewFromPalette2(viewPaletteName, 3, 3, 0.0);
                    boolstatus = swDrawing.SetupSheet5("SHEET", 12, 12, 1, 1, true, formatoNovo, 0.1, 0.1, "Valor predeterminado", false);
                    boolstatus = swModel.Extension.SelectByID2(swView.Name, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                    swModel.ViewZoomToSelection();
                    swModel.ClearSelection();
                  }
                }
              }

              nomeSaida = pasta + "\\" + item.CodComponente + ".DXF";

              int longStatus = swModel.SaveAs3(nomeSaida, 0, 0);

              arquivoAbert = swModel.GetPathName();
              Sw.App.CloseDoc(arquivoAbert);

              swModel = (ModelDoc2)Sw.App.ActiveDoc;
              //retornar chapa
              boolstatus = swModel.Extension.SelectByID2(nameFeat, typeFeat, 0, 0, 0, false, 0, null, 0);
              status = swModel.EditSuppress2();

            }
            swFeat = (Feature)swFeat.GetNextFeature();
          }

          arquivoAbert = swModel.GetPathName();
          Sw.App.CloseDoc(arquivoAbert);
        } catch (Exception ex) {
          Invoke(new MethodInvoker(() => {
            MsgBox.Show($"Erro ao gerar DXF da peça '{item.PathName}'\n" + ex.Message, "Gerar DXF", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }));
        } finally {
          if (swFeat != null) Marshal.ReleaseComObject(swFeat);
        }

        UcPainelTarefas.Instancia.BringToFront();
        Invoke(new MethodInvoker(() => {
          this.Refresh();
        }));
      } catch (Exception ex) {
        MsgBox.Show("Erro ao gerar DXF\n" + ex.Message, "Gerar DXF", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }


    private void BtnCancelar_Click(object sender, EventArgs e) {
      btnCancelar.Visible = false;
      btnCarregar.Enabled = true;
      btnExportar.Enabled = true;

      exportando = false;
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
        fileName = ((DxfExport)dadosDraw.Current).PathName;

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
        fileName = ((DxfExport)dadosDraw.Current).PathName;


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


    private void TxtPesquisar_ButtonClickF7(object sender, EventArgs e) {
      try {
        int index = 0;
        if (dgv.Grid.CurrentRow != null)
          index = dgv.Grid.CurrentRow.Index;

        if (index == dgv.Grid.RowCount - 1)
          index = -1;

        for (int i = index + 1; i < dgv.Grid.RowCount; i++) {
          if (SelectRow(i)) break;

          if (i == dgv.Grid.RowCount - 1)
            for (int h = 0; h <= index; h++)
              if (SelectRow(h)) break;
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pesquisar\n\n{ex.Message}", "Addin LM Projetos",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private bool SelectRow(int indice) {
      try {
        string txt = txtPesquisar.Text.ToUpper();

        string componente = Convert.ToString(dgv.Grid.Rows[indice].Cells["CodComponente"].Value).ToUpper();
        string denominacao = Convert.ToString(dgv.Grid.Rows[indice].Cells["Denominacao"].Value).ToUpper();

        if (componente.Contains(txt)) {
          if (dgv.Grid.Rows[indice].Visible == true) {
            dgv.Grid.Rows[indice].Cells["CodComponente"].Selected = true;
            return true;
          }
        } else if (denominacao.Contains(txt)) {
          if (dgv.Grid.Rows[indice].Visible == true) {
            dgv.Grid.Rows[indice].Cells["Denominacao"].Selected = true;
            return true;
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao selecionar linha do grid\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return false;
    }
  }
}
