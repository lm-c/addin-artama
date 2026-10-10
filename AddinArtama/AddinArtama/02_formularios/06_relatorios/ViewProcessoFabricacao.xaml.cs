using iTextSharp.text;
using iTextSharp.text.pdf;
using LmCorbieUI;
using LmCorbieUI.Wpf.Controls;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Gera os processos de fabricação (PDF dos desenhos com nota + CSV) por processo (substitui o FrmProcessoFabricacao).</summary>
  public partial class ViewProcessoFabricacao : Wpf.LmView {
    List<ProcessoFabricacao> _processos = new List<ProcessoFabricacao>();
    List<Z_Padrao> _descProcessos = new List<Z_Padrao>();

    public ViewProcessoFabricacao() {
      InitializeComponent();
    }

    private async void BtnCarrProcess_Click(object sender, RoutedEventArgs e) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          List<ProcessoFabricacao> processos = null;

          using (var worker = new LmBackgroundWorker()) {
            await worker.RunAsync("Lendo componentes da montagem...", (progress, token) => {
              progress.Report(("Lendo componentes da montagem...", 0, 0));
              processos = ProcessoFabricacao.GetProcessoFabricacao();
              return Task.CompletedTask;
            });
          }

          _processos = processos ?? new List<ProcessoFabricacao>();
          dgv.ItemsSource = _processos;

          _descProcessos = ProcessoFabricacao.GetDescricaoProcesso(_processos);
        } else {
          Wpf.MsgBox.Show($"Comando apenas para Montagens", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao carregar componentes\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private async void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      if (_processos.Count == 0) {
        Wpf.MsgBox.Show($"Favor Carregar Componentes primeiro!", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      try {
        if (!LmValidacao.Validar(pnlControls))
          return;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }
        string sourceDir = DrawExport.GetFolder("_PDF", swModel);

        if (!Directory.Exists(sourceDir)) {
          Wpf.MsgBox.Show("Os PDF's devem ser gerados antes de criar os processos de fabricação!", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
        }

        int qtda = Convert.ToInt32(txtQtd.Text);
        string pedido = txtPedido.Text;

        bool cancelado = false;
        string pastaProcessoFabricacao = string.Empty;

        using (var worker = new LmBackgroundWorker()) {
          await worker.RunAsync("Gerando Processo de Fabricação...", (progress, token) => {
            pastaProcessoFabricacao = GerarProcessos(swModel, sourceDir, pedido, qtda, progress, token, out cancelado);
            return Task.CompletedTask;
          });
        }

        if (cancelado) {
          Wpf.Toast.Info("Operação cancelada pelo usuário.");
          return;
        }

        Process.Start("explorer.exe", pastaProcessoFabricacao);
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao imprimir Processo de Fabricacao\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private string GerarProcessos(ModelDoc2 swModel, string sourceDir, string pedido, int qtda, IProgress<(string message, int current, int max)> progress, CancellationToken token, out bool cancelado) {
      string pastaProcessoFabricacao = DrawExport.GetFolder("_PROCESSO", swModel);
      cancelado = false;

      if (!Directory.Exists(pastaProcessoFabricacao))
        Directory.CreateDirectory(pastaProcessoFabricacao);

      string pastaProcessoFabricacaoPDF = $"{pastaProcessoFabricacao}Ped.{pedido}_PDF\\";
      if (!Directory.Exists(pastaProcessoFabricacaoPDF))
        Directory.CreateDirectory(pastaProcessoFabricacaoPDF);

      string pastaProcessoFabricacaoCSV = $"{pastaProcessoFabricacao}Ped.{pedido}_CSV\\";
      if (!Directory.Exists(pastaProcessoFabricacaoCSV))
        Directory.CreateDirectory(pastaProcessoFabricacaoCSV);

      List<string> arquivosTemp = new List<string>();
      int totalProcessos = _descProcessos.Count;

      foreach (var item in _descProcessos.Select((value, index) => new { value, index })) {
        if (token.IsCancellationRequested) {
          cancelado = true;
          break;
        }

        if (string.IsNullOrEmpty(item.value.Descricao))
          continue;

        progress.Report(($"[{item.value.Descricao}] Gerando PDF", item.index + 1, totalProcessos));

        string packListCSV = $"{pastaProcessoFabricacaoCSV}{item.value.Descricao}.CSV";
        string desenhoPDF = $"{pastaProcessoFabricacaoPDF}{item.value.Descricao}.PDF";

        List<ProcessoFabricacao> listasNova = _processos
          .Where(x => Convert.ToInt32(x.IdProcesso) == item.value.Codigo)
          .OrderBy(x => x.EspessuraMaterial).ToList();

        using (FileStream fs = new FileStream(packListCSV, FileMode.Create)) {
          using (StreamWriter file = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"))) {
            file.WriteLine($"PEDIDO: {pedido};{item.value.Descricao} - PROCESSO");
            file.WriteLine(" ");
            file.WriteLine("Código Item;Descrição Item,Espessura;Quantidade");

            foreach (ProcessoFabricacao proc in listasNova) {
              file.WriteLine($"{proc.CodigoItem};{proc.DescricaoItem};{proc.EspessuraMaterial};{proc.QtdItem * qtda}");
            }
          }
        }

        GerarPdfVolumesDesenhos(desenhoPDF, listasNova, sourceDir, arquivosTemp, qtda);
      }

      if (!cancelado) {
        foreach (var arquivoTemp in arquivosTemp)
          File.Delete(arquivoTemp);

        progress.Report(("Concluído!", totalProcessos, totalProcessos));
      }

      return pastaProcessoFabricacao;
    }

    private void GerarPdfVolumesDesenhos(string targetPDF, List<ProcessoFabricacao> processos, string pastaPDF, List<string> arquivosTemp, int qtda) {
      try {
        var files = Directory.GetFiles(pastaPDF);
        var destFolder = Path.GetDirectoryName(targetPDF);

        List<PdfReader> readers = new List<PdfReader>();

        foreach (ProcessoFabricacao processo in processos) {
          var file = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x) == processo.NomeComponente);
          if (file == null)
            continue;

          var fileDest = destFolder + "\\" + processo.IdProcesso + "_" + Path.GetFileName(file);

          File.Copy(file, fileDest, true);

          InserirNotaVolume(fileDest, processo, qtda);

          if (fileDest != null) {
            var reader = new PdfReader(fileDest);
            readers.Add(reader);
          }

          arquivosTemp.Add(fileDest);
        }

        if (readers.Count > 0) {
          using (FileStream stream = new FileStream(targetPDF, FileMode.Create)) {
            Document pdfDoc = new Document(PageSize.A4);
            PdfCopy pdf = new PdfCopy(pdfDoc, stream);
            pdfDoc.Open();

            foreach (var read in readers) {
              pdf.AddDocument(read);
              read.Dispose();
              read.Close();
            }

            pdfDoc.Close();
            pdfDoc.Dispose();
            pdf.Close();
            pdf.Dispose();
          }
        }

        GC.SuppressFinalize(readers);
        GC.Collect();
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao mesclar PDF\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void InserirNotaVolume(string targetPDF, ProcessoFabricacao processo, int qtda) {
      try {
        int largPDF = 0;
        int altPDF = 0;

        // Fonte normal (no WinForms vinha de Convert.ToInt32(Font.Italic) da tela, que não era itálica).
        int f = iTextSharp.text.Font.NORMAL;
        byte[] bytes = File.ReadAllBytes(targetPDF);
        iTextSharp.text.Font font;
        using (MemoryStream stream = new MemoryStream()) {
          PdfReader reader = new PdfReader(bytes);
          using (PdfStamper stamper = new PdfStamper(reader, stream)) {
            int qtd = 0;
            string descricaoProc = "";

            int pages = reader.NumberOfPages;
            for (int i = 1; i <= pages; i++) {
              iTextSharp.text.Rectangle mediabox = reader.GetPageSize(i);
              largPDF = Convert.ToInt16(mediabox.Height);
              altPDF = Convert.ToInt16(mediabox.Width);

              // default
              float x = 10f;
              float y = 7f;
              float ang = 0;
              font = FontFactory.GetFont("Arial", 10, f, BaseColor.BLUE);

              if ((largPDF > 835 && largPDF < 845) && (altPDF > 590 && altPDF < 600))//formato A4 Retrato
              {
                x = 300f;
                y = 830f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 10, f, BaseColor.BLUE);
              } else if ((largPDF > 590 && largPDF < 600) && (altPDF > 835 && altPDF < 845))//formato A4
                {
                x = 120f;
                y = 5f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 10, f, BaseColor.BLUE);
              } else if ((largPDF > 835 && largPDF < 845) && (altPDF > 1185 && altPDF < 1195))//formato A3
                {
                x = 450f;
                y = 5f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 12, f, BaseColor.BLUE);
              } else if ((largPDF > 1185 && largPDF < 1195) && (altPDF > 1680 && altPDF < 1690))//formato A2
                {
                x = 900f;
                y = 7f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 14, f, BaseColor.BLUE);
              } else if ((largPDF > 1680 && largPDF < 1690) && (altPDF > 2380 && altPDF < 2390))//formato A1
                {
                x = 1520f;
                y = 7f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 20, f, BaseColor.BLUE);
              }

              qtd = processo.QtdItem * qtda;
              descricaoProc = processo.DescricaoProcesso;

              ColumnText.ShowTextAligned(stamper.GetOverContent(i),
                  Element.ALIGN_LEFT, new Phrase($"{descricaoProc} ({qtd}X)", font), x, y, ang);
            }
          }
          bytes = stream.ToArray();
        }
        File.WriteAllBytes(targetPDF, bytes);
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao inserir número de página\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }
}
