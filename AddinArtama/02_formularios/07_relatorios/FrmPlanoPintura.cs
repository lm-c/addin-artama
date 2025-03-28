using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LmCorbieUI;
using LmCorbieUI.Metodos;
using LmCorbieUI.LmForms;
using Microsoft.Reporting.WinForms;

namespace AddinArtama {
  public partial class FrmPlanoPintura : LmSingleForm {
    List<Z_Padrao> descVolumes = new List<Z_Padrao>();

    BindingSource dadosPlanoPintura = new BindingSource();
    PlanoPintura planoPintura = new PlanoPintura();
    List<PlanoPintura> pdfPlanoPintura = new List<PlanoPintura>();

    string _pastaPDF = string.Empty;

    public FrmPlanoPintura() {
      InitializeComponent();
    }

    private void BtnCarrProcess_Click(object sender, EventArgs e) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (Sw.App.ActiveDoc == null) {
          MsgBox.Show($"Sem documentos abertos", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          _pastaPDF = DrawExport.GetFolder("_PDF", swModel);

          if (!Directory.Exists(_pastaPDF)) {
            MsgBox.Show($"Você Deve gerar os PDF's antes de montar o Plano de Pintura", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
          }

          MsgBox.ShowWaitMessage("Lendo componentes da montagem...");
          descVolumes = new List<Z_Padrao>();
         
          dadosPlanoPintura.DataSource = PlanoPintura.GetPlanoPintura(out descVolumes);
          dgv.Grid.DataSource = dadosPlanoPintura;

          //descVolumes = PlanoPinturaDATA.GetDescricaoVolume();

          FormatarGrid();
        } else {
          MsgBox.Show($"Comando apenas para Montagens", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar componentes\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void FormatarGrid() {
      dgv.Grid.Columns["IdVolume"].Width = 40;
      dgv.Grid.Columns["CodigoItem"].Width = 80;
      dgv.Grid.Columns["DescricaoItem"].Width = 150;
      dgv.Grid.Columns["DescricaoVolume"].Width = 250;
      dgv.Grid.Columns["QtdItem"].Width = 50;


      dgv.Grid.Columns["IdVolume"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["IdVolume"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["CodigoItem"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["CodigoItem"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["QtdItem"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["QtdItem"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      int qtda = 1;

      if (string.IsNullOrEmpty(txtPedido.Text)) {
        MsgBox.Show($"Informar Pedido", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      } else if (string.IsNullOrEmpty(txtDescricao.Text)) {
        MsgBox.Show($"Informar Descrição", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      } else if (dadosPlanoPintura.Count == 0) {
        MsgBox.Show($"Lista não Contem Elementos, verificar se as propridades 'Pintura' foram preenchidas corretamente",
            "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      if (!Directory.Exists(_pastaPDF)) {
        MsgBox.Show($"Você Deve gerar os PDF's antes de montar o Plano de Pintura", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      var swModel = (ModelDoc2)Sw.App.ActiveDoc;

      try {
        MsgBox.ShowWaitMessage("Gerando Plano de Pintura...");

        string pastaPlanoPintura = Path.GetDirectoryName(swModel.GetPathName()) + "\\PLANO DE PINTURA\\";

        if (!Directory.Exists(pastaPlanoPintura))
          Directory.CreateDirectory(pastaPlanoPintura);

        string PlanoPinturaPDFCompleto = $"{pastaPlanoPintura}{txtPedido.Text} - PLANO DE PINTURA.PDF";
        //string PlanoPinturaPDFCapa = $"{pastaPlanoPintura}{txtPedido.Text} - PLANO DE PINTURA CAPA.PDF";

        List<PackList> planoPinturaCapa = new List<PackList>();

        foreach (var item in (List<PlanoPintura>)dadosPlanoPintura.DataSource) {
          if (!planoPinturaCapa.Any(x => x.DescricaoVolume == item.DescricaoVolume)) {
            planoPinturaCapa.Add(new PackList {
              CodigoItem = item.CodigoItem,
              DescricaoItem = item.DescricaoItem,
              DescricaoVolume = item.DescricaoVolume,
              IdVolume = item.IdVolume,
              QtdItem = item.QtdItem * qtda,
            });
          }
        }
        List<BindingSource> listaCapa = new List<BindingSource>()
        {
          new BindingSource{DataSource = planoPinturaCapa },
        };

        List<string> nomeDSCapa = new List<string>()
        {
          "PackListDS"
        };

        List<ReportParameter> parametersCapa = new List<ReportParameter>();

        ReportParameter prtEquipamento = new ReportParameter("rppEquipamento", txtDescricao.Text, true);
        ReportParameter prtPedido = new ReportParameter("rppPedido", txtPedido.Text, true);

        parametersCapa.Add(prtEquipamento);
        parametersCapa.Add(prtPedido);

        if (!Directory.Exists(pastaPlanoPintura))
          Directory.CreateDirectory(pastaPlanoPintura);

        List<string> pdfMesclar = new List<string>();

        for (int i = 0; i < descVolumes.Count(); i++) {
          int idVol = i + 1;

          string volume = descVolumes[i].Descricao;
          if (string.IsNullOrEmpty(volume))
            break;

          List<PlanoPintura> listasNova = new List<PlanoPintura>();

          string planoPinturaPDF = $"{pastaPlanoPintura}{i} PLANO PINTURA.PDF";
          string desenhosPDF = $"{pastaPlanoPintura}{i} DESENHOS.PDF";

          foreach (PlanoPintura item in ((List<PlanoPintura>)dadosPlanoPintura.DataSource).Where(x => x.IdVolume == idVol)) {
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

          string logoArtama = $"{Application.StartupPath}\\01 - Addin LM\\LogoArtama.png";

          List<ReportParameter> parameters = new List<ReportParameter>();

          ReportParameter prtLogo = new ReportParameter("rppLogo", "File://" + logoArtama, true);
          ReportParameter prtVolumeCod = new ReportParameter("rppVolumeCodigo", idVol.ToString("00"), true);
          ReportParameter prtVolumeDesc = new ReportParameter("rppVolumeDescricao", descVolumes[i].Descricao, true);
          ReportParameter prtObs = new ReportParameter("rppObs", txtObs.Text, true);
          ReportParameter prtData = new ReportParameter("rppData", txtData.Text, true);

          parameters.Add(prtLogo);
          parameters.Add(prtEquipamento);
          parameters.Add(prtPedido);
          parameters.Add(prtVolumeCod);
          parameters.Add(prtVolumeDesc);
          parameters.Add(prtObs);
          parameters.Add(prtData);

          string report = "AddinArtama.Relatorios.rptPlanoPintura.rdlc";

          if (!Directory.Exists(pastaPlanoPintura))
            Directory.CreateDirectory(pastaPlanoPintura);

          FrmRelatorio frm = new FrmRelatorio(listas, nomeDS, report, parameters, true);

          if (!Controles.ArquivoEstaAberto(planoPinturaPDF)) {
            File.WriteAllBytes(planoPinturaPDF, frm.bytesPDF);

            //Process.Start(fileNamePdfCompleto);
          } else {
            MsgBox.Show($"Arquivo PDF já está aberto.\n\n\"{planoPinturaPDF}\"",
                "Em Uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          }

          //GERAR PDF VOLUMES
          GerarPdfVolumesDesenhos(desenhosPDF, qtda, idVol);

          pdfMesclar.Add(planoPinturaPDF);
          pdfMesclar.Add(desenhosPDF);
        }

        Controles.MesclarPDFs(pdfMesclar, PlanoPinturaPDFCompleto);

        foreach (var file in pdfMesclar) {
          File.Delete(file);
        }

        Process.Start(pastaPlanoPintura);

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao imprimir Plano de Pintura\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void BtnConfig_Click(object sender, EventArgs e) {
      if (Sw.App.ActiveDoc != null) {
        MsgBox.Show($"Feche todos os documentos antes de proseguir!", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      //FrmExportConfig frm = new FrmExportConfig();
      //frm.Show();
    }

    private void BtnClose_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void TxtDescricao_ButtonClickF7(object sender, EventArgs e) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (Sw.App.ActiveDoc == null) {
          MsgBox.Show($"Sem documentos abertos", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        txtDescricao.Text = PlanoPintura.GetDenominacao(swModel);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar descrição\n\n{ex.Message}", "Addin LM Projetos",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void GerarPdfVolumesDesenhos(string targetPDF, int qtda, int idVol) {
      List<string> arquivosExclusao = new List<string>();

      using (FileStream stream = new FileStream(targetPDF, FileMode.Create)) {
        try {
          pdfPlanoPintura.Clear();

          Document pdfDoc = new Document(PageSize.A4);
          PdfCopy pdf = new PdfCopy(pdfDoc, stream);
          pdfDoc.Open();

          var files = Directory.GetFiles(_pastaPDF);
          bool pdfHasPages = false;

          foreach (PlanoPintura planoPintura in dadosPlanoPintura) {
            if (planoPintura.IdVolume != idVol)
              continue;

            string codPk = planoPintura.CodigoItem.ToString();
            foreach (string file in files) {
              PdfReader reader = null;

              string nmPdf = Path.GetFileNameWithoutExtension(file);

              if (codPk.ToUpper() == nmPdf.ToUpper()) {
                reader = new PdfReader(file);

                pdf.AddDocument(reader, new List<int> { 1 });
                pdfHasPages = true;
                pdfPlanoPintura.Add(planoPintura);

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
            InserirNotaVolume(targetPDF, qtda);
          } else {
            arquivosExclusao.Add(targetPDF);
          }
        } catch (Exception ex) {
          MsgBox.Show($"Erro ao mesclar PDF\n\n{ex.Message}", "Addin LM Projetos",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      if (arquivosExclusao.Count > 0)
        foreach (var file in arquivosExclusao)
          File.Delete(file);
    }

    private void InserirNotaVolume(string targetPDF, int qtda) {
      try {
        int largPDF = 0;
        int altPDF = 0;

        int f = Convert.ToInt32(Font.Italic);
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

              volume = pdfPlanoPintura[i - 1].IdVolume;
              qtd = pdfPlanoPintura[i - 1].QtdItem * qtda;
              descricaoVolume = pdfPlanoPintura[i - 1].DescricaoVolume;

              ColumnText.ShowTextAligned(stamper.GetOverContent(i),
                  Element.ALIGN_LEFT, new Phrase($"Ped. {txtPedido.Text} - {descricaoVolume}({qtd}X)", font), x, y, ang);
            }
          }
          bytes = stream.ToArray();
        }
        File.WriteAllBytes(targetPDF, bytes);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao inserir número de página\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }
}
