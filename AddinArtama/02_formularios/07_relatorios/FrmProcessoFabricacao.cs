using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using LmCorbieUI;
using LmCorbieUI.Metodos;
using LmCorbieUI.LmForms;
using Microsoft.Reporting.WinForms;
using System.Linq;

namespace AddinArtama {
  public partial class FrmProcessoFabricacao : LmSingleForm {

    List<ProcessoFabricacao> _processos = new List<ProcessoFabricacao>();
    List<Z_Padrao> _descProcessos = new List<Z_Padrao>();

    //ProcessoFabricacao processoFabricacao = new ProcessoFabricacao();
    //List<ProcessoFabricacao> pdfProcessoFabricacao = new List<ProcessoFabricacao>();
    //string[] descVolumes;

    public FrmProcessoFabricacao() {
      InitializeComponent();
    }

    private void BtnCarrProcess_Click(object sender, EventArgs e) {
      MsgBox.ShowWaitMessage("Lendo componentes da montagem...");
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (Sw.App.ActiveDoc == null) {
          MsgBox.Show($"Sem documentos abertos", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          _processos = ProcessoFabricacao.GetProcessoFabricacao();
          dgv.Grid.DataSource = _processos;

          _descProcessos = ProcessoFabricacao.GetDescricaoProcesso(_processos);

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
      //dgv.Grid.Columns["IdProcesso"].Width = 35;
      dgv.Grid.Columns["CodigoItem"].Width = 80;
      dgv.Grid.Columns["DescricaoItem"].Width = 200;
      dgv.Grid.Columns["QtdItem"].Width = 40;
      dgv.Grid.Columns["DescricaoProcesso"].Width = 120;
      dgv.Grid.Columns["EspessuraMaterial"].Width = 80;

      // dgv.Grid.Columns["DescricaoProcesso"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

      //dgv.Grid.Columns["IdProcesso"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      //dgv.Grid.Columns["IdProcesso"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["CodigoItem"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["CodigoItem"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["QtdItem"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["QtdItem"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["EspessuraMaterial"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      dgv.Grid.Columns["EspessuraMaterial"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {

      if (_processos.Count == 0) {
        MsgBox.Show($"Favor Carregar Componentes primeiro!", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      try {
        if (Controles.PossuiCamposInvalidos(pnlControls))
          return;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (Sw.App.ActiveDoc == null) {
          MsgBox.Show($"Sem documentos abertos", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }
        string sourceDir = DrawExport.GetFolder("_PDF", swModel);

        if (!Directory.Exists(sourceDir)) {
          MsgBox.Show("Os PDF's devem ser gerados antes de criar os processos de fabricação!", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

        int qtda = Convert.ToInt32(txtQtd.Text);

        string pastaProcessoFabricacao = DrawExport.GetFolder("_PROCESSO", swModel);
        if (!Directory.Exists(pastaProcessoFabricacao))
          Directory.CreateDirectory(pastaProcessoFabricacao);

        string pastaProcessoFabricacaoPDF = $"{pastaProcessoFabricacao}Ped.{txtPedido.Text}_PDF\\";
        if (!Directory.Exists(pastaProcessoFabricacaoPDF))
          Directory.CreateDirectory(pastaProcessoFabricacaoPDF);

        string pastaProcessoFabricacaoCSV = $"{pastaProcessoFabricacao}Ped.{txtPedido.Text}_CSV\\";
        if (!Directory.Exists(pastaProcessoFabricacaoCSV))
          Directory.CreateDirectory(pastaProcessoFabricacaoCSV);

        List<string> arquivosTemp = new List<string>();

        foreach (var item in _descProcessos) {
          if (string.IsNullOrEmpty(item.Descricao))
            continue;

          string packListCSV = $"{pastaProcessoFabricacaoCSV}{item.Descricao}.CSV";
          string desenhoPDF  = $"{pastaProcessoFabricacaoPDF}{item.Descricao}.PDF";

          MsgBox.ShowWaitMessage($"[{item.Descricao}] Gerando PDF");

          List<ProcessoFabricacao> listasNova = _processos
            .Where(x => Convert.ToInt32(x.IdProcesso) == item.Codigo)
            .OrderBy(x => x.EspessuraMaterial).ToList();


          using (FileStream fs = new FileStream(packListCSV, FileMode.Create)) {
            using (StreamWriter file = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"))) {
              file.WriteLine($"PEDIDO: {txtPedido.Text};{item.Descricao} - PROCESSO");
              file.WriteLine(" ");
              file.WriteLine("Código Item;Descrição Item,Espessura;Quantidade");

              foreach (ProcessoFabricacao proc in listasNova) {
                file.WriteLine($"{proc.CodigoItem};{proc.DescricaoItem};{proc.EspessuraMaterial};{proc.QtdItem * qtda}");
              }
            }
          }

          //string report = "AddinArtama.Relatorios.rptProcessoFabricacao.rdlc";

          //List<BindingSource> listas = new List<BindingSource> {
          //  new BindingSource{DataSource = listasNova },
          //};

          //List<string> nomeDS = new List<string> {
          //  "ProcessoFabricacaoDS"
          //};

          //List<ReportParameter> parameters = new List<ReportParameter>();

          //parameters.Add(new ReportParameter("rppEquipamento", txtDescricao.Text, true));
          //parameters.Add(new ReportParameter("rppPedido", txtPedido.Text, true));
          //parameters.Add(new ReportParameter("rppProcesso", item.Codigo.ToString(), true));
          //parameters.Add(new ReportParameter("rppObs", txtObs.Text, true));
          //parameters.Add(new ReportParameter("rppRevisao", txtRevisao.Text, true));
          //parameters.Add(new ReportParameter("rppData", txtData.Text, true));

          //FrmRelatorio frm = new FrmRelatorio(listas, nomeDS, report, parameters, true);

          //if (!Controles.ArquivoEstaAberto(processoPDF)) {
          //  File.WriteAllBytes(processoPDF, frm.bytesPDF);
          //} else {
          //  MsgBox.Show($"Arquivo PDF já está aberto.\n\n\"{processoPDF}\"",
          //      "Em Uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          //}

          //GERAR PDF VOLUMES
          GerarPdfVolumesDesenhos(desenhoPDF, listasNova, sourceDir, arquivosTemp, qtda);
        }

        foreach (var arquivoTemp in arquivosTemp)
          File.Delete(arquivoTemp);

        Process.Start("explorer.exe", pastaProcessoFabricacao);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao imprimir Processo de Fabricacao\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void GerarPdfVolumesDesenhos(string targetPDF, List<ProcessoFabricacao> processos, string pastaPDF, List<string> arquivosTemp, int qtda) {
      try {
        var files = Directory.GetFiles(pastaPDF);
        var destFolder = Path.GetDirectoryName(targetPDF);

        var listaPecas = new List<DrawExport>();

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
        MsgBox.Show($"Erro ao mesclar PDF\n\n{ex.Message}", "Addin LM Projetos",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void InserirNotaVolume(string targetPDF, ProcessoFabricacao processo, int qtda) {
      try {
        int largPDF = 0;
        int altPDF = 0;

        int f = Convert.ToInt32(Font.Italic);
        byte[] bytes = File.ReadAllBytes(targetPDF);
        iTextSharp.text.Font font;
        using (MemoryStream stream = new MemoryStream()) {
          PdfReader reader = new PdfReader(bytes);
          using (PdfStamper stamper = new PdfStamper(reader, stream)) {
            string codigoProc = "0";
            int qtd = 0;
            string descricaoProc = "", codigoItem = "", descricaoTemp = "";

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

              codigoProc = processo.IdProcesso.ToString();
              qtd = processo.QtdItem * qtda;
              descricaoProc = processo.DescricaoProcesso;
              codigoItem = processo.CodigoItem;
              descricaoTemp = processo.DescricaoItem;

              ColumnText.ShowTextAligned(stamper.GetOverContent(i),
                  Element.ALIGN_LEFT, new Phrase($"{descricaoProc} ({qtd}X)", font), x, y, ang);
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
