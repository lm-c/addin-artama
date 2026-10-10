using iTextSharp.text;
using iTextSharp.text.pdf;
using LmCorbieUI;
using LmCorbieUI.Metodos;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Exporta os desenhos da montagem para PDF/DWG e gera o PDF geral (substitui o FrmExportarPDF).</summary>
  public partial class ViewExportarPDF : Wpf.LmView {
    List<DrawExport> dadosDraw = new List<DrawExport>();

    string nomeMontagem = "";
    string pastaPdf = "";
    string pastaDwg = "";
    bool imprimindo = false;

    /// <summary>Exporta um desenho por tique, avançando a linha selecionada (como o Timer do WinForms).</summary>
    readonly DispatcherTimer tmrExportar = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };

    public ViewExportarPDF() {
      InitializeComponent();

      tmrExportar.Tick += TmrExportar_Tick;
      Unloaded += (s, e) => tmrExportar.Stop();
    }

    DrawExport Atual => dgv.SelectedItem as DrawExport;

    private void CarregarGrid(List<DrawExport> lista) {
      dadosDraw = lista ?? new List<DrawExport>();
      dgv.ItemsSource = dadosDraw;

      if (dadosDraw.Count > 0)
        dgv.SelectedIndex = 0;
    }

    private async void BtnCarregar_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        string pathName = swModel.GetPathName();
        nomeMontagem = pathName;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          Wpf.MsgBox.Show($"Comando Apenas para Montagem", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          List<DrawExport> lista = null;

          using (var worker = new LmBackgroundWorker()) {
            await worker.RunAsync("Lendo componentes da montagem...", (progress, token) => {
              progress.Report(("Lendo componentes da montagem...", 0, 0));
              lista = DrawExport.GetDrawing();
              return Task.CompletedTask;
            });
          }

          if (rdbOrdemCodigo.IsChecked == true)
            lista = lista.OrderBy(x => x.CodComponente).ToList();
          else
            lista = lista.OrderBy(x => x.IndexTree).ToList();

          CarregarGrid(lista);

          pastaPdf = DrawExport.GetFolder("_PDF", swModel);
          pastaDwg = DrawExport.GetFolder("_DWG", swModel);

          if (!Directory.Exists(pastaPdf) && (ckbPdf.IsChecked == true))
            Directory.CreateDirectory(pastaPdf);
          if (!Directory.Exists(pastaDwg) && (ckbDwg.IsChecked == true))
            Directory.CreateDirectory(pastaDwg);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao carregar desenhos\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void BtnExportar_Click(object sender, RoutedEventArgs e) {
      if (dadosDraw.Count == 0) {
        Wpf.MsgBox.Show($"Você deve carregar os Componentes primeiro.", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      if (ckbPdf.IsChecked != true && ckbPdf.IsChecked != true) {
        Wpf.MsgBox.Show($"Você Marcar 'PDF' ou 'DWG', para Salvar.", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      try {
        if (!Directory.Exists(pastaPdf) && ckbPdf.IsChecked == true) {
          Directory.CreateDirectory(pastaPdf);
        } else if (ckbPdf.IsChecked == true) {
          string[] files = Directory.GetFiles(pastaPdf);
          ExcluirObsoleto(files);
        }

        if (!Directory.Exists(pastaDwg) && ckbDwg.IsChecked == true) {
          Directory.CreateDirectory(pastaDwg);
        } else if (ckbDwg.IsChecked == true) {
          string[] files = Directory.GetFiles(pastaPdf);
          ExcluirObsoleto(files);
        }

        imprimindo = true;
        HabilitarControles(false);

        tmrExportar.Start();
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao exportar arquivos\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    /// <summary>Durante a exportação só o Cancelar fica disponível.</summary>
    private void HabilitarControles(bool habilitar) {
      btnCancelar.Visibility = habilitar ? Visibility.Collapsed : Visibility.Visible;
      btnCarregar.IsEnabled =
      btnExportar.IsEnabled =
      ckbPdf.IsEnabled =
      ckbDwg.IsEnabled =
      rdbOrdemArvore.IsEnabled =
      rdbOrdemCodigo.IsEnabled = habilitar;
    }

    private void TmrExportar_Tick(object sender, EventArgs e) {
      tmrExportar.Stop();

      try {
        int status = 0;
        int warnings = 0;
        var item = Atual;
        if (item == null)
          return;

        if (item.Exportar == true) {
          Sw.App.OpenDoc6(item.PathName, (int)swDocumentTypes_e.swDocDRAWING,
          (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

          var swModel = (ModelDoc2)Sw.App.ActiveDoc;
          swModel.ForceRebuild3(true);

          Exportar(swModel, item);

          Sw.App.CloseDoc(item.PathName);
          GC.Collect();
        }

        var index = dgv.SelectedIndex;

        if (imprimindo == false) {
          Wpf.MsgBox.Show($"Impressão Cancelada!", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
          return;
        } else if (index == dadosDraw.Count - 1) {
          imprimindo = false;
          HabilitarControles(true);

          if (ckbPdf.IsChecked == true) {
            string targetPDF = pastaPdf + "000 - " +
                dadosDraw[0].CodComponente.Substring(6, dadosDraw[0].CodComponente.Length - 6) + "_GERAL.PDF";
            CreateMergedPDF(targetPDF, pastaPdf);

            Process.Start(targetPDF);
          }
        } else {
          dgv.SelectedIndex = index + 1;
          dgv.ScrollIntoView(dgv.SelectedItem);
          tmrExportar.Start();
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao Exportar\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
      HabilitarControles(true);
      imprimindo = false;
    }

    private void ExcluirObsoleto(string[] files) {
      try {
        foreach (string file in files) {
          bool Excluir = true;
          string nm = Path.GetFileNameWithoutExtension(file);

          foreach (DrawExport drw in dadosDraw) {
            string dr = drw.CodComponente;

            if (dr == nm) {
              Excluir = false;
              break;
            }
          }
          if (Excluir == true)
            File.Delete(file);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao excluir\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void Exportar(ModelDoc2 swModel, DrawExport item) {
      try {
        string fileName = "";

        if (ckbPdf.IsChecked == true) {
          fileName = $"{pastaPdf}{item.CodComponente}.PDF";
          SalvarComo(fileName, swModel);
        }

        if (ckbDwg.IsChecked == true) {
          fileName = $"{pastaDwg}{item.CodComponente}.DWG";
          SalvarComo(fileName, swModel);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao exportar\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void SalvarComo(string fileName, ModelDoc2 swModel) {
      bool bRet;
      int version = 0, errors = 0, options = 0, warnings = 0;
      if (!Controles.ArquivoEstaAberto(fileName))
        bRet = swModel.SaveAs4(fileName, version, options, ref errors, ref warnings);
      else
        Wpf.MsgBox.Show($"Arquivo: \"{fileName}\"\nEstá em Uso.\nNão Será Atualizado.", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private void CreateMergedPDF(string targetPDF, string sourceDir) {
      using (FileStream stream = new FileStream(targetPDF, FileMode.Create)) {
        try {
          Document pdfDoc = new Document(PageSize.A4);
          PdfCopy pdf = new PdfCopy(pdfDoc, stream);
          pdfDoc.Open();
          var files = Directory.GetFiles(sourceDir);

          foreach (string file in files)
            if (file != targetPDF)
              pdf.AddDocument(new PdfReader(file));

          pdf.Close();
          pdf.Dispose();
          pdfDoc.Close();
          pdfDoc.Dispose();
        } catch (Exception ex) {
          Wpf.MsgBox.Show($"Erro ao mesclar PDF\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
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

    #region Pesquisa / ordem

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

    private void RdbOrdem_Checked(object sender, RoutedEventArgs e) {
      if (dgv == null || dadosDraw.Count == 0)
        return;

      if (rdbOrdemCodigo.IsChecked == true)
        CarregarGrid(dadosDraw.OrderBy(x => x.CodComponente).ToList());
      else if (rdbOrdemArvore.IsChecked == true)
        CarregarGrid(dadosDraw.OrderBy(x => x.IndexTree).ToList());
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
