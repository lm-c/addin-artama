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

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Exportar PDF/DWG",
        "Esta tela exporta os desenhos de uma montagem para PDF e/ou DWG, um arquivo por desenho, e ainda monta um PDF geral com todos.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Aguardar("1. Abra a montagem",
        "No SolidWorks, abra (e deixe ativa) a montagem do projeto.\n\n" +
        "Regra: esta tela só funciona com uma MONTAGEM ativa.",
        ViewFormatosAtualizar.TemMontagemAtiva);

      yield return Wpf.LmPassoAssistente.Info("2. Formatos",
        "Marque o que deseja gerar: PDF, DWG ou os dois.\n\n" +
        "Regra: os arquivos vão para pastas criadas ao lado da montagem, com o código dela: \"CÓDIGO_PDF\" e \"CÓDIGO_DWG\". " +
        "O código vem da propriedade \"Componente\" da montagem.",
        ckbPdf, ckbDwg);

      yield return Wpf.LmPassoAssistente.Info("3. Ordem da lista",
        "Escolha como a lista é ordenada: pelo código do componente ou na ordem da árvore da montagem.",
        rdbOrdemCodigo, rdbOrdemArvore);

      yield return Wpf.LmPassoAssistente.Aguardar("4. Carregar os desenhos",
        "Clique em Carregar e aguarde a leitura da montagem.\n\n" +
        "Regra: só entram na lista os componentes que TÊM desenho (.SLDDRW com o mesmo nome, na mesma pasta). " +
        "O desenho da própria montagem aparece como \"001 - código\".",
        () => dgv.Items.Count > 0, btnCarregar);

      yield return Wpf.LmPassoAssistente.Info("5. Escolha o que exportar",
        "A caixa \"Sel\" define se o desenho será exportado. Com o botão direito você marca ou desmarca todos, e abre o 3D ou o 2D da linha.",
        dgv);

      yield return Wpf.LmPassoAssistente.Info("6. Pesquisar",
        "Digite parte do código ou da denominação e tecle Enter (ou F7) para ir ao próximo resultado.",
        txtPesquisar);

      yield return Wpf.LmPassoAssistente.Info("7. Exportar",
        "Para cada desenho marcado, o sistema abre o desenho, reconstrói, salva o PDF e/ou DWG com o código do componente e fecha.\n\n" +
        "Regras:\n" +
        "• A exportação começa na LINHA SELECIONADA e vai até o fim da lista. Para exportar tudo, selecione a primeira linha.\n" +
        "• Antes de começar, os arquivos da pasta que não correspondem a nenhum componente da lista são EXCLUÍDOS (desenhos obsoletos).\n" +
        "• Arquivo aberto em outro programa não é atualizado, e você é avisado.\n" +
        "• Ao chegar ao fim, é gerado e aberto o PDF geral (\"000 - código_GERAL.PDF\"), que junta todos os PDFs da pasta.\n\n" +
        "Durante a exportação aparece o botão Cancelar.",
        btnExportar);

      yield return Wpf.LmPassoAssistente.Info("8. Progresso",
        "Mostra em qual desenho a exportação está e o percentual concluído.",
        lblPercDesenho);

      yield return Wpf.LmPassoAssistente.Info("Pronto!",
        "Os PDFs gerados aqui são usados depois nos relatórios (Processo de Fabricação, Pack List e Plano de Pintura), que exigem a pasta de PDF pronta.\n\n" +
        "Para rever este passo a passo, clique no \"?\" no topo da tela.");
    }

    #endregion

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
        Msg.Erro(ex, "Erro ao carregar desenhos");
      }
    }

    private void BtnExportar_Click(object sender, RoutedEventArgs e) {
      if (dadosDraw.Count == 0) {
        Wpf.MsgBox.Show($"Você deve carregar os Componentes primeiro.", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      if (ckbPdf.IsChecked != true && ckbDwg.IsChecked != true) {
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
          string[] files = Directory.GetFiles(pastaDwg);
          ExcluirObsoleto(files);
        }

        imprimindo = true;
        HabilitarControles(false);

        tmrExportar.Start();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao exportar arquivos");
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
        Msg.Erro(ex, "Erro ao Exportar");
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
        Msg.Erro(ex, "Erro ao excluir");
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
        Msg.Erro(ex, "Erro ao exportar");
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
          Msg.Erro(ex, "Erro ao mesclar PDF");
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
        Msg.Erro(ex, "Erro ao abrir arquivo 3D");
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
        Msg.Erro(ex, "Erro ao abrir arquivo 2D");
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
        Msg.Erro(ex, "Erro ao pesquisar");
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
