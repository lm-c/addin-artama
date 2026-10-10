using iTextSharp.text;
using iTextSharp.text.pdf;
using LmCorbieUI;
using LmCorbieUI.Metodos;
using Microsoft.Reporting.WinForms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BindingSource = System.Windows.Forms.BindingSource;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Gera o Plano de Pintura por volume (relatório + desenhos com nota) — substitui o FrmPlanoPintura.</summary>
  public partial class ViewPlanoPintura : Wpf.LmView {
    /// <summary>Campos da tela lidos antes de gerar (a geração roda fora da thread da tela).</summary>
    sealed class Cabecalho {
      public string Pedido, Descricao, Obs, Data;
    }

    List<Z_Padrao> descVolumes = new List<Z_Padrao>();

    List<PlanoPintura> dadosPlano = new List<PlanoPintura>();
    List<PlanoPintura> pdfPlano = new List<PlanoPintura>();

    string _pastaPDF = string.Empty;

    public ViewPlanoPintura() {
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
          _pastaPDF = DrawExport.GetFolder("_PDF", swModel);

          if (!Directory.Exists(_pastaPDF)) {
            Wpf.MsgBox.Show($"Você Deve gerar os PDF's antes de montar o Plano de Pintura", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
          }

          List<Z_Padrao> volumes = null;
          List<PlanoPintura> planoData = null;

          using (var worker = new LmBackgroundWorker()) {
            await worker.RunAsync("Lendo componentes da montagem...", (progress, token) => {
              progress.Report(("Lendo componentes da montagem...", 0, 0));
              planoData = PlanoPintura.GetPlanoPintura(out volumes);
              return Task.CompletedTask;
            });
          }

          descVolumes = volumes ?? new List<Z_Padrao>();
          dadosPlano = planoData ?? new List<PlanoPintura>();
          dgv.ItemsSource = dadosPlano;
        } else {
          Wpf.MsgBox.Show($"Comando apenas para Montagens", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao carregar componentes\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private async void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      int qtda = 1;

      if (string.IsNullOrEmpty(txtPedido.Text)) {
        Wpf.MsgBox.Show($"Informar Pedido", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      } else if (string.IsNullOrEmpty(txtDescricao.Text)) {
        Wpf.MsgBox.Show($"Informar Descrição", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      } else if (dadosPlano.Count == 0) {
        Wpf.MsgBox.Show($"Lista não Contem Elementos, verificar se as propridades 'Pintura' foram preenchidas corretamente",
            "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      if (!Directory.Exists(_pastaPDF)) {
        Wpf.MsgBox.Show($"Você Deve gerar os PDF's antes de montar o Plano de Pintura", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      var swModel = (ModelDoc2)Sw.App.ActiveDoc;

      var cabecalho = new Cabecalho {
        Pedido = txtPedido.Text,
        Descricao = txtDescricao.Text,
        Obs = txtObs.Text,
        Data = txtData.Text,
      };

      try {
        bool cancelado = false;
        string pastaPlano = string.Empty;

        using (var worker = new LmBackgroundWorker()) {
          await worker.RunAsync("Gerando Plano de Pintura...", (progress, token) => {
            pastaPlano = GerarPlanoPintura(swModel, cabecalho, qtda, progress, token, out cancelado);
            return Task.CompletedTask;
          });
        }

        if (cancelado) {
          Wpf.Toast.Info("Operação cancelada pelo usuário.");
          return;
        }

        Process.Start(pastaPlano);
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao imprimir Plano de Pintura\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private string GerarPlanoPintura(ModelDoc2 swModel, Cabecalho cabecalho, int qtda, IProgress<(string message, int current, int max)> progress, CancellationToken token, out bool cancelado) {
      string pastaPlano = Path.GetDirectoryName(swModel.GetPathName()) + "\\PLANO DE PINTURA\\";
      cancelado = false;

      if (!Directory.Exists(pastaPlano))
        Directory.CreateDirectory(pastaPlano);

      string planoPDFCompleto = $"{pastaPlano}{cabecalho.Pedido} - PLANO DE PINTURA.PDF";

      ReportParameter prtEquipamento = new ReportParameter("rppEquipamento", cabecalho.Descricao, true);
      ReportParameter prtPedido = new ReportParameter("rppPedido", cabecalho.Pedido, true);

      if (!Directory.Exists(pastaPlano))
        Directory.CreateDirectory(pastaPlano);

      List<string> pdfMesclar = new List<string>();
      int totalVolumes = descVolumes.Count();

      for (int i = 0; i < totalVolumes; i++) {
        if (token.IsCancellationRequested) {
          cancelado = true;
          return pastaPlano;
        }

        int idVol = i + 1;
        string volume = descVolumes[i].Descricao;
        if (string.IsNullOrEmpty(volume))
          break;

        progress.Report(($"Gerando Plano de Pintura [{volume}]...", i + 1, totalVolumes));

        List<PlanoPintura> listasNova = new List<PlanoPintura>();

        string planoPDF = $"{pastaPlano}{i} PLANO PINTURA.PDF";
        string desenhosPDF = $"{pastaPlano}{i} DESENHOS.PDF";

        foreach (PlanoPintura item in dadosPlano.Where(x => x.IdVolume == idVol)) {
          listasNova.Add(new PlanoPintura {
            CodigoItem = item.CodigoItem,
            DescricaoItem = item.DescricaoItem,
            DescricaoVolume = item.DescricaoVolume,
            IdVolume = item.IdVolume,
            QtdItem = item.QtdItem * qtda
          });
        }

        List<BindingSource> listas = new List<BindingSource>()
        {
          new BindingSource{DataSource = listasNova },
        };

        List<string> nomeDS = new List<string>()
        {
          "PlanoPinturaDS"
        };

        string logoArtama = $"{System.Windows.Forms.Application.StartupPath}\\01 - Addin LM\\LogoArtama.png";

        List<ReportParameter> parameters = new List<ReportParameter>();

        ReportParameter prtLogo = new ReportParameter("rppLogo", "File://" + logoArtama, true);
        ReportParameter prtVolumeCod = new ReportParameter("rppVolumeCodigo", idVol.ToString("00"), true);
        ReportParameter prtVolumeDesc = new ReportParameter("rppVolumeDescricao", descVolumes[i].Descricao, true);
        ReportParameter prtObs = new ReportParameter("rppObs", cabecalho.Obs, true);
        ReportParameter prtData = new ReportParameter("rppData", cabecalho.Data, true);

        parameters.Add(prtLogo);
        parameters.Add(prtEquipamento);
        parameters.Add(prtPedido);
        parameters.Add(prtVolumeCod);
        parameters.Add(prtVolumeDesc);
        parameters.Add(prtObs);
        parameters.Add(prtData);

        string report = "AddinArtama.Relatorios.rptPlanoPintura.rdlc";
        if (!Directory.Exists(pastaPlano))
          Directory.CreateDirectory(pastaPlano);

        FrmRelatorio frm = new FrmRelatorio(listas, nomeDS, report, parameters, true);

        if (!Controles.ArquivoEstaAberto(planoPDF)) {
          File.WriteAllBytes(planoPDF, frm.bytesPDF);
        } else {
          Wpf.MsgBox.Show($"Arquivo PDF já está aberto.\n\n\"{planoPDF}\"", "Em Uso", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        GerarPdfVolumesDesenhos(desenhosPDF, cabecalho.Pedido, qtda, idVol);

        pdfMesclar.Add(planoPDF);
        pdfMesclar.Add(desenhosPDF);
      }

      Controles.MesclarPDFs(pdfMesclar, planoPDFCompleto);

      foreach (var file in pdfMesclar) {
        File.Delete(file);
      }

      progress.Report(("Concluído!", totalVolumes, totalVolumes));
      return pastaPlano;
    }

    private void TxtDescricao_BotaoF7Click(object sender, RoutedEventArgs e) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        txtDescricao.Text = PlanoPintura.GetDenominacao(swModel);
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao retornar descrição\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void GerarPdfVolumesDesenhos(string targetPDF, string pedido, int qtda, int idVol) {
      List<string> arquivosExclusao = new List<string>();

      using (FileStream stream = new FileStream(targetPDF, FileMode.Create)) {
        try {
          pdfPlano.Clear();

          Document pdfDoc = new Document(PageSize.A4);
          PdfCopy pdf = new PdfCopy(pdfDoc, stream);
          pdfDoc.Open();

          var files = Directory.GetFiles(_pastaPDF);
          bool pdfHasPages = false;

          foreach (PlanoPintura plano in dadosPlano) {
            if (plano.IdVolume != idVol)
              continue;

            string codPk = plano.CodigoItem.ToString();
            foreach (string file in files) {
              PdfReader reader = null;

              string nmPdf = Path.GetFileNameWithoutExtension(file);

              if (codPk.ToUpper() == nmPdf.ToUpper()) {
                reader = new PdfReader(file);

                pdf.AddDocument(reader, new List<int> { 1 });
                pdfHasPages = true;
                pdfPlano.Add(plano);

                reader.Dispose();
                reader.Close();
                break;
              }
            }
          }

          if (pdf.PageNumber > 0 && pdfHasPages) {
            pdfDoc.Close();
            pdfDoc.Dispose();
            pdf.Close();
            pdf.Dispose();
            InserirNotaVolume(targetPDF, pedido, qtda);
          } else {
            arquivosExclusao.Add(targetPDF);
          }
        } catch (Exception ex) {
          Wpf.MsgBox.Show($"Erro ao mesclar PDF\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }

      if (arquivosExclusao.Count > 0)
        foreach (var file in arquivosExclusao)
          File.Delete(file);
    }

    private void InserirNotaVolume(string targetPDF, string pedido, int qtda) {
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
            int volume = 0, qtd = 0;
            string descricaoVolume = "";
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
                x = 60f;
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
                x = 250f;
                y = 5f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 12, f, BaseColor.BLUE);
              } else if ((largPDF > 1185 && largPDF < 1195) && (altPDF > 1680 && altPDF < 1690))//formato A2
                {
                x = 900f;
                y = 5f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 14, f, BaseColor.BLUE);
              } else if ((largPDF > 1680 && largPDF < 1690) && (altPDF > 2380 && altPDF < 2390))//formato A1
                {
                x = 1520f;
                y = 5f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 20, f, BaseColor.BLUE);
              }

              volume = pdfPlano[i - 1].IdVolume;
              qtd = pdfPlano[i - 1].QtdItem * qtda;
              descricaoVolume = pdfPlano[i - 1].DescricaoVolume;

              ColumnText.ShowTextAligned(stamper.GetOverContent(i),
                  Element.ALIGN_LEFT, new Phrase($"Ped. {pedido} - {descricaoVolume}({qtd}X)", font), x, y, ang);
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
