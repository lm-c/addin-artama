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
using Microsoft.Reporting.WinForms;
using LmCorbieUI;
using LmCorbieUI.Metodos;
using LmCorbieUI.LmForms;

namespace AddinArtama {
  public partial class FrmPackList : LmSingleForm {
    List<Z_Padrao> descVolumes = new List<Z_Padrao>();

    BindingSource dadosPackList = new BindingSource();
    PackList packList = new PackList();
    List<PackList> pdfPackList = new List<PackList>();

    string _pastaPDF = string.Empty;

    public FrmPackList() {
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
            MsgBox.Show($"Você Deve gerar os PDF's antes de montar o Packlist", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
          }

          MsgBox.ShowWaitMessage("Lendo componentes da montagem...");
          descVolumes = new List<Z_Padrao>();

          // _packListEstrutura = PackListDATA.GetPackLit(out descVolumes);
          dadosPackList.DataSource = PackList.GetPackLit(out descVolumes);
          dgv.Grid.DataSource = dadosPackList;

          //descVolumes = PackListDATA.GetDescricaoVolume();

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
      } else if (dadosPackList.Count == 0) {
        MsgBox.Show($"Lista não Contem Elementos, verificar se as propridades 'CHECK' foram preenchidas corretamente",
            "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      if (!Directory.Exists(_pastaPDF)) {
        MsgBox.Show($"Você Deve gerar os PDF's antes de montar o Packlist", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      var swModel = (ModelDoc2)Sw.App.ActiveDoc;

      try {
        MsgBox.ShowWaitMessage("Gerando PackList...");

        string pastaPackList = Path.GetDirectoryName(swModel.GetPathName()) + "\\PACKING LIST\\";

        if (!Directory.Exists(pastaPackList))
          Directory.CreateDirectory(pastaPackList);

        string packListPDFCompleto = $"{pastaPackList}{txtPedido.Text} - PACKLIST.PDF";
        //string packListPDFCapa = $"{pastaPackList}{txtPedido.Text} - PACKLIST CAPA.PDF";

        List<PackList> PackListCapa = new List<PackList>();

        foreach (var item in (List<PackList>)dadosPackList.DataSource) {
          if (!PackListCapa.Any(x => x.DescricaoVolume == item.DescricaoVolume)) {
            PackListCapa.Add(new PackList {
              CodigoItem = item.CodigoItem,
              NomeConfiguracao = item.NomeConfiguracao,
              DescricaoItem = item.DescricaoItem,
              DescricaoVolume = item.DescricaoVolume,
              IdVolume = item.IdVolume,
              QtdItem = item.QtdItem * qtda,
            });
          }
        }
        List<BindingSource> listaCapa = new List<BindingSource>()
        {
                    new BindingSource{DataSource = PackListCapa },
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

        if (!Directory.Exists(pastaPackList))
          Directory.CreateDirectory(pastaPackList);

        List<string> pdfMesclar = new List<string>();

        for (int i = 0; i < descVolumes.Count(); i++) {
          int idVol = i + 1;

          string volume = descVolumes[i].Descricao;
          if (string.IsNullOrEmpty(volume))
            break;

          List<PackList> listasNova = new List<PackList>();

          string packListPDF = $"{pastaPackList}{i} PACKLIST.PDF";
          string desenhosPDF = $"{pastaPackList}{i} DESENHOS.PDF";

          foreach (PackList item in ((List<PackList>)dadosPackList.DataSource).Where(x => x.IdVolume == idVol)) {
            listasNova.Add(new PackList {
              CodigoItem = item.CodigoItem,
              DescricaoItem = item.DescricaoItem,
              NomeConfiguracao = item.NomeConfiguracao,
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
            "PackListDS"
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

          string report = "AddinArtama.Relatorios.rptPackList.rdlc";
          if (!Directory.Exists(pastaPackList))
            Directory.CreateDirectory(pastaPackList);

          FrmRelatorio frm = new FrmRelatorio(listas, nomeDS, report, parameters, true);

          if (!Controles.ArquivoEstaAberto(packListPDF)) {
            File.WriteAllBytes(packListPDF, frm.bytesPDF);

            //Process.Start(fileNamePdfCompleto);
          } else {
            MsgBox.Show($"Arquivo PDF já está aberto.\n\n\"{packListPDF}\"",
                "Em Uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          }

          //GERAR PDF VOLUMES
          GerarPdfVolumesDesenhos(desenhosPDF, qtda, idVol);

          pdfMesclar.Add(packListPDF);
          pdfMesclar.Add(desenhosPDF);
        }

        Controles.MesclarPDFs(pdfMesclar, packListPDFCompleto);

        foreach (var file in pdfMesclar) {
          File.Delete(file);
        }

        Process.Start(pastaPackList);

        //Process.Start(fileNamePdfCompleto);
        //// Process.Start(fileNamePdfVolume);
        //Process.Start(targetPDF);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Salvar PackList\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
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

        txtDescricao.Text = PackList.GetDenominacao(swModel);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar descrição\n\n{ex.Message}", "Addin LM Projetos",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void GerarPdfVolumesDesenhos(string targetPDF, int qtda, int idVol) {
      List<string> arquivosExclusao = new List<string>();

      using (FileStream stream = new FileStream(targetPDF, FileMode.Create)) {
        try {
          pdfPackList.Clear();

          Document pdfDoc = new Document(PageSize.A4);
          PdfCopy pdf = new PdfCopy(pdfDoc, stream);
          pdfDoc.Open();

          var files = Directory.GetFiles(_pastaPDF);
          bool pdfHasPages = false;

          foreach (PackList packList in dadosPackList) {
            if (packList.IdVolume != idVol)
              continue;

            string codPk = packList.CodigoItem.ToString();
            foreach (string file in files) {
              PdfReader reader = null;

              string nmPdf = Path.GetFileNameWithoutExtension(file);

              if (codPk.ToUpper() == nmPdf.ToUpper()) {
                reader = new PdfReader(file);

                pdf.AddDocument(reader, new List<int> { 1 });
                pdfHasPages = true;
                pdfPackList.Add(packList);

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
                x = 300f;
                y = 830f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 10, f, BaseColor.BLUE);
              } else if ((largPDF > 590 && largPDF < 600) && (altPDF > 835 && altPDF < 845))//formato A4
                {
                x = 120f;
                y = 7f;
                ang = 0;
                font = FontFactory.GetFont("Arial", 10, f, BaseColor.BLUE);
              } else if ((largPDF > 835 && largPDF < 845) && (altPDF > 1185 && altPDF < 1195))//formato A3
                {
                x = 450f;
                y = 7f;
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

              volume = pdfPackList[i - 1].IdVolume;
              qtd = pdfPackList[i - 1].QtdItem * qtda;
              descricaoVolume = pdfPackList[i - 1].DescricaoVolume;

              ColumnText.ShowTextAligned(stamper.GetOverContent(i),
                  Element.ALIGN_LEFT, new Phrase($"Ped. {txtPedido.Text} - Vol.{volume} - {descricaoVolume}({qtd}X)", font), x, y, ang);
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
