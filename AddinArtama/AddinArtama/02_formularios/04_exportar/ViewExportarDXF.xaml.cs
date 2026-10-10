using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Gera o DXF da planificação das chapas, por pasta de material (substitui o FrmExportarDXF).</summary>
  public partial class ViewExportarDXF : Wpf.LmView {
    List<DxfExport> dadosDraw = new List<DxfExport>();

    volatile bool exportando = false;

    public ViewExportarDXF() {
      InitializeComponent();
    }

    DxfExport Atual => dgv.SelectedItem as DxfExport;

    private async void BtnCarregar_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          Wpf.MsgBox.Show($"Comando Apenas para Montagem", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          List<DxfExport> dados = null;

          using (var worker = new LmBackgroundWorker()) {
            await worker.RunAsync("Lendo componentes da montagem...", (progress, token) => {
              progress.Report(("Lendo componentes da montagem...", 0, 0));
              dados = DxfExport.GetDrawing();
              return Task.CompletedTask;
            });
          }

          dadosDraw = dados ?? new List<DxfExport>();
          dgv.ItemsSource = dadosDraw;

          if (dadosDraw.Count > 0)
            dgv.SelectedIndex = 0;
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao carregar desenhos\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void BtnExportar_Click(object sender, RoutedEventArgs e) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (dadosDraw.Count == 0) {
          Wpf.Toast.Warning($"Favor carrregar componentes primeiro");
          return;
        }

        string pastaDXF = DrawExport.GetFolder("_DXF", swModel);
        if (!Directory.Exists(pastaDXF))
          Directory.CreateDirectory(pastaDXF);

        btnCancelar.Visibility = Visibility.Visible;
        btnCarregar.IsEnabled = false;
        btnExportar.IsEnabled = false;

        exportando = true;

        // Começa na linha selecionada, como no WinForms.
        int inicio = Math.Max(dgv.SelectedIndex, 0);
        string formatoNovo = $"{System.Windows.Forms.Application.StartupPath}\\01 - Addin LM\\FORMATO EM BRANCO.drwdot";

        Thread t = new Thread(() => { IniciarExportacao(pastaDXF, inicio, formatoNovo); }) { IsBackground = true };
        t.Start();
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao exportar arquivos\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void IniciarExportacao(string pasta, int inicio, string formatoNovo) {
      try {
        for (int i = inicio; i < dadosDraw.Count; i++) {
          if (!exportando)
            break;

          var indice = i;
          Dispatcher.Invoke(() => {
            dgv.SelectedIndex = indice;
            dgv.ScrollIntoView(dgv.SelectedItem);
          });

          var item = dadosDraw[i];

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

            GerarDXF(swModel, item, pastaNova, formatoNovo);
            Sw.App.CloseDoc(arquivo); //fechar arquivo SolidWorks
          }
        }

        if (exportando) {
          Wpf.MsgBox.Show($"Arquivos DXF gerados com sucesso!\n\n{pasta}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

          Dispatcher.Invoke(Cancelar);
        } else {
          Wpf.MsgBox.Show($"Rotina cancelada pelo usuário antes do término!", "Cancelado", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao exportar arquivos\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    // gerar arquivos em dxf
    private void GerarDXF(ModelDoc2 swModel, DxfExport item, string pasta, string formatoNovo) {
      try {
        int numViews;
        object[] viewNames;
        string viewPaletteName;
        int i;
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
            if (swFeat.GetTypeName() == "FlatPattern") {
              //planificar
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
          Wpf.MsgBox.Show($"Erro ao gerar DXF da peça '{item.PathName}'\n" + ex.Message, "Gerar DXF", MessageBoxButton.OK, MessageBoxImage.Error);
        } finally {
          if (swFeat != null) Marshal.ReleaseComObject(swFeat);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show("Erro ao gerar DXF\n" + ex.Message, "Gerar DXF", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
      Cancelar();
    }

    private void Cancelar() {
      btnCancelar.Visibility = Visibility.Collapsed;
      btnCarregar.IsEnabled = true;
      btnExportar.IsEnabled = true;

      exportando = false;
    }

    #region Menu da grade

    private void TsmSelectAll_Click(object sender, RoutedEventArgs e) {
      dadosDraw.ForEach(x => x.Exportar = true);
      dgv.Items.Refresh();
    }

    private void TsmUnselectAll_Click(object sender, RoutedEventArgs e) {
      dadosDraw.ForEach(x => x.Exportar = false);
      dgv.Items.Refresh();
    }

    private void TsmOpen3D_Click(object sender, RoutedEventArgs e) {
      try {
        if (Atual == null) return;

        string fileName = "", openFileNamePart = "", openFileNameAssembly = "";
        int status = 0;
        int warnings = 0;
        fileName = Atual.PathName;

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
        Wpf.MsgBox.Show($"Erro ao abrir arquivo 3D\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void TsmOpen2D_Click(object sender, RoutedEventArgs e) {
      try {
        if (Atual == null) return;

        string fileName = "";
        int status = 0;
        int warnings = 0;
        fileName = Atual.PathName;

        if (File.Exists(fileName)) {
          Sw.App.OpenDoc6(fileName, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(fileName, false, (int)errors);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao abrir arquivo 2D\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    #endregion

    #region Pesquisa

    private void TxtPesquisar_BotaoF7Click(object sender, RoutedEventArgs e) {
      ProcurarProximo();
    }

    /// <summary>Enter também vai para o próximo resultado.</summary>
    private void TxtPesquisar_PreviewKeyDown(object sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
        e.Handled = true;
        ProcurarProximo();
      }
    }

    /// <summary>Procura a partir da linha seguinte à selecionada, voltando ao início.</summary>
    private void ProcurarProximo() {
      try {
        int total = dadosDraw.Count;
        int index = dgv.SelectedIndex;

        if (index == total - 1)
          index = -1;

        for (int i = index + 1; i < total; i++) {
          if (SelectRow(i)) break;

          if (i == total - 1)
            for (int h = 0; h <= index; h++)
              if (SelectRow(h)) break;
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao pesquisar\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private bool SelectRow(int indice) {
      string txt = txtPesquisar.Text.ToUpper();
      var item = dadosDraw[indice];

      string componente = Convert.ToString(item.CodComponente).ToUpper();
      string denominacao = Convert.ToString(item.Denominacao).ToUpper();

      if (componente.Contains(txt) || denominacao.Contains(txt)) {
        dgv.SelectedIndex = indice;
        dgv.ScrollIntoView(item);
        return true;
      }

      return false;
    }

    private void Dgv_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (dadosDraw.Count > 0 && dgv.SelectedIndex >= 0) {
        var pos = dgv.SelectedIndex + 1;
        lblPercDesenho.Content = $"{pos} de {dadosDraw.Count} - {pos * 100 / dadosDraw.Count}%";
      }
    }

    #endregion
  }
}
