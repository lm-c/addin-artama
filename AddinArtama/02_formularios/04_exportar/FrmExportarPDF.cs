using System;
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
using System.Collections.Generic;
using System.Linq;

namespace AddinArtama {
  public partial class FrmExportarPDF : LmSingleForm {

    BindingSource dadosDraw = new BindingSource();
    DrawExport drawExport = new DrawExport();

    string nomeMontagem = "";
    string pastaPdf = "";
    string pastaPdfMontagem = "";
    string pastaDwg = "";
    bool imprimindo = false;

    public FrmExportarPDF() {
      InitializeComponent();

      dadosDraw.CurrentChanged += DadosDraw_CurrentChanged;
    }

    private void DadosDraw_CurrentChanged(object sender, EventArgs e) {

    }

    private void FormatarGrid() {
      dgv.Grid.Columns["Exportar"].Width = 30;
      dgv.Grid.Columns["CodComponente"].Width = 80;
      dgv.Grid.Columns["Denominacao"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

      dgv.Grid.Columns["Exportar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["Exportar"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["CodComponente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.Grid.Columns["CodComponente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

      dgv.Grid.ReadOnly = false;
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
        string pathName = swModel.GetPathName();
        nomeMontagem = pathName;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          MsgBox.Show($"Comando Apenas para Montagem", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          var lista = DrawExport.GetDrawing();

          if (rdbOrdemCodigo.Checked)
            lista = lista.OrderBy(x => x.CodComponente).ToList();
          else
            lista = lista.OrderBy(x => x.IndexTree).ToList();

          dadosDraw.DataSource = lista;
          dgv.Grid.DataSource = dadosDraw;
          dadosDraw.MoveFirst();

          FormatarGrid();

          string shortName = Path.GetFileNameWithoutExtension(pathName);

          pastaPdf = DrawExport.GetFolder("_PDF", swModel);
          pastaDwg = DrawExport.GetFolder("_DWG", swModel);

          if (!Directory.Exists(pastaPdf) && (ckbPdf.Checked))
            Directory.CreateDirectory(pastaPdf);
          if (!Directory.Exists(pastaDwg) && (ckbDwg.Checked))
            Directory.CreateDirectory(pastaDwg);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar desenhos\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void BtnExportar_Click(object sender, EventArgs e) {
      if (dadosDraw.Count == 0) {
        MsgBox.Show($"Você deve carregar os Componentes primeiro.", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      if (!ckbPdf.Checked && !ckbPdf.Checked) {
        MsgBox.Show($"Você Marcar 'PDF' ou 'DWG', para Salvar.", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      try {
        if (!Directory.Exists(pastaPdf) && ckbPdf.Checked) {
          Directory.CreateDirectory(pastaPdf);
        } else if (ckbPdf.Checked) {
          string[] files = Directory.GetFiles(pastaPdf);
          ExcluirObsoleto(files);
        }

        if (!Directory.Exists(pastaDwg) && ckbDwg.Checked) {
          Directory.CreateDirectory(pastaDwg);
        } else if (ckbDwg.Checked) {
          string[] files = Directory.GetFiles(pastaPdf);
          ExcluirObsoleto(files);
        }

        imprimindo = true;
        btnCancelar.Visible = true;
        btnCarregar.Enabled =
        btnExportar.Enabled =
        ckbPdf.Enabled =
        ckbDwg.Enabled =
        rdbOrdemArvore.Enabled =
        rdbOrdemCodigo.Enabled = !btnCancelar.Visible;

        //Sw.App.CloseDoc(nomeMontagem);

        tmrExportar.Enabled = true;

        //System.Threading.Thread t = new System.Threading.Thread(() => { GerarPDFs(); }) { IsBackground = true };
        //t.Start();

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao exportar arquivos\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void TmrExportar_Tick(object sender, EventArgs e) {
      tmrExportar.Enabled = false;

      try {
        int status = 0;
        int warnings = 0;
        if (dgv.Grid.CurrentRow == null)
          return;

        var item = (DrawExport)dgv.Grid.CurrentRow.DataBoundItem;
        if (item.Exportar == true) {
          Sw.App.OpenDoc6(item.PathName, (int)swDocumentTypes_e.swDocDRAWING,
          (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

          var swModel = (ModelDoc2)Sw.App.ActiveDoc;
          swModel.ForceRebuild3(true);

          Exportar(swModel);

          Sw.App.CloseDoc(item.PathName);
          GC.Collect();
        }

        var index = dgv.Grid.CurrentRow.Index;

        if (imprimindo == false) {
          MsgBox.Show($"Impressão Cancelada!", "Addin LM Projetos",
               MessageBoxButtons.OK, MessageBoxIcon.Information);

          //Sw.App.OpenDoc6(nomeMontagem, (int)swDocumentTypes_e.swDocASSEMBLY,
          //  (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

          //swModel = (ModelDoc2)Sw.App.ActiveDoc;

          return;
        } else if (index == dadosDraw.Count - 1) {
          imprimindo = false;
          btnCancelar.Visible = false;

          btnCarregar.Enabled =
          btnExportar.Enabled =
          ckbPdf.Enabled =
          ckbDwg.Enabled =
          rdbOrdemArvore.Enabled =
          rdbOrdemCodigo.Enabled = !btnCancelar.Visible;

          if (ckbPdf.Checked) {
            string targetPDF = pastaPdf + "000 - " +
                ((DrawExport)dadosDraw[0]).CodComponente.Substring(6, ((DrawExport)dadosDraw[0]).CodComponente.Length - 6) + "_GERAL.PDF";
            CreateMergedPDF(targetPDF, pastaPdf);

            Process.Start(targetPDF);

            //Sw.App.OpenDoc6(nomeMontagem, (int)swDocumentTypes_e.swDocASSEMBLY,
            //  (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

            //swModel = (ModelDoc2)Sw.App.ActiveDoc;
          }
        } else {
          dgv.Grid.Rows[dgv.Grid.CurrentRow.Index + 1].Cells[1].Selected = true;
          tmrExportar.Enabled = true;
        }
      } catch (Exception ex) {
        MessageBox.Show($"Erro ao Exportar\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

    }

    //private void GerarPDFs() {

    //  try {
    //    int status = 0;
    //    int warnings = 0;

    //    System.Collections.IList list = dgv.Grid.Rows;
    //    for (int i = dgv.Grid.CurrentRow.Index; i < list.Count; i++) {
    //      DataGridViewRow row = (DataGridViewRow)list[i];
    //      var item = row.DataBoundItem as DrawExport;

    //      Invoke(new MethodInvoker(delegate () {
    //        dgv.Grid.Rows[i].Cells[1].Selected = true;
    //      }));

    //      if (imprimindo == false) {
    //        MsgBox.Show($"Impressão Cancelada!", "Addin LM Projetos",
    //             MessageBoxButtons.OK, MessageBoxIcon.Information);

    //        Sw.App.OpenDoc6(nomeMontagem, (int)swDocumentTypes_e.swDocASSEMBLY,
    //          (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

    //        swModel = (ModelDoc2)Sw.App.ActiveDoc;

    //        return;
    //      } 

    //      if (item.Exportar) {
    //        Sw.App.OpenDoc6(item.PathName, (int)swDocumentTypes_e.swDocDRAWING,
    //          (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

    //        swModel = (ModelDoc2)Sw.App.ActiveDoc;
    //        swModel.ForceRebuild3(true);

    //        Exportar();

    //        Sw.App.CloseDoc(item.PathName);
    //        GC.Collect();

    //        swModel = default(ModelDoc2);
    //      }
    //    }
    //    Invoke(new MethodInvoker(delegate () {
    //      imprimindo = false;
    //      btnCancelar.Visible = false;
    //      btnCarregar.Enabled =
    //      btnExportar.Enabled =
    //      ckbPdf.Enabled =
    //      ckbDwg.Enabled =
    //      rdbOrdemArvore.Enabled =
    //      rdbOrdemCodigo.Enabled =
    //      btnClose.Enabled = !btnCancelar.Visible;
    //    }));

    //    if (ckbPdf.Checked) {
    //      string targetPDF = pastaPdf + "000 - " +
    //          ((DrawExport)dadosDraw[0]).CodComponente.Substring(6, ((DrawExport)dadosDraw[0]).CodComponente.Length - 6) + "_GERAL.PDF";
    //      CreateMergedPDF(targetPDF, pastaPdf);

    //      Process.Start(targetPDF);

    //      Sw.App.OpenDoc6(nomeMontagem, (int)swDocumentTypes_e.swDocASSEMBLY,
    //        (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

    //      swModel = (ModelDoc2)Sw.App.ActiveDoc;          
    //    }

    //  } catch (Exception ex) {
    //    MsgBox.Show($"Erro ao Exportar\n\n{ex.Message}", "Addin LM Projetos",
    //         MessageBoxButtons.OK, MessageBoxIcon.Error);
    //  }
    //}

    private void BtnCancelar_Click(object sender, EventArgs e) {
      btnCancelar.Visible = false;
      btnCarregar.Enabled =
      btnExportar.Enabled =
      ckbPdf.Enabled =
      ckbDwg.Enabled =
      rdbOrdemArvore.Enabled =
      rdbOrdemCodigo.Enabled = !btnCancelar.Visible;

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
        MsgBox.Show($"Erro ao excluir\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void BtnClose_Click(object sender, EventArgs e) {
      this.Close();
    }

    public void Exportar(ModelDoc2 swModel) {
      try {
        string fileName = "";

        if (ckbPdf.Checked) {
          fileName = $"{pastaPdf}{((DrawExport)dadosDraw.Current).CodComponente}.PDF";
          SalvarComo(fileName, swModel);
        }

        if (ckbDwg.Checked) {
          fileName = $"{pastaDwg}{((DrawExport)dadosDraw.Current).CodComponente}.DWG";
          SalvarComo(fileName, swModel);
        }

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao exportar\n\n{ex.Message}", "Addin LM Projetos",
           MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void SalvarComo(string fileName, ModelDoc2 swModel) {
      try {
        bool bRet;
        int version = 0, errors = 0, options = 0, warnings = 0;
        if (!Controles.ArquivoEstaAberto(fileName))
          bRet = swModel.SaveAs4(fileName, version, options, ref errors, ref warnings);
        else
          MsgBox.Show($"Arquivo: \"{fileName}\"\nEstá em Uso.\nNão Será Atualizado.", "Addin LM Projetos",
              MessageBoxButtons.OK, MessageBoxIcon.Warning);
      } catch (Exception) {
        throw;
      }
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
          MsgBox.Show($"Erro ao mesclar PDF\n\n{ex.Message}", "Addin LM Projetos",
         MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
        fileName = ((DrawExport)dadosDraw.Current).PathName;

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
        fileName = ((DrawExport)dadosDraw.Current).PathName;


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

    private void RdbOrdemCodigo_CheckedChanged(object sender, EventArgs e) {
      if (rdbOrdemCodigo.Checked) {
        var lista = dadosDraw.DataSource as List<DrawExport>;

        lista = lista.OrderBy(x => x.CodComponente).ToList();

        dadosDraw.DataSource = lista;
        dgv.Grid.DataSource = dadosDraw;
        dadosDraw.MoveFirst();

        FormatarGrid();
      }
    }

    private void RdbOrdemArvore_CheckedChanged(object sender, EventArgs e) {
      if (rdbOrdemArvore.Checked) {
        var lista = dadosDraw.DataSource as List<DrawExport>;

        lista = lista.OrderBy(x => x.IndexTree).ToList();

        dadosDraw.DataSource = lista;
        dgv.Grid.DataSource = dadosDraw;
        dadosDraw.MoveFirst();

        FormatarGrid();
      }
    }

    private void Dgv_RowIndexChanged(object sender, EventArgs e) {
      try {
        if (dadosDraw.Count > 0) {
          lblPercDesenho.Text = (dgv.Grid.CurrentRow.Index + 1) + " de " + dgv.Grid.RowCount +
              " - " + (((dgv.Grid.CurrentRow.Index + 1) * 100) / dgv.Grid.RowCount) + "%";
        }
      } catch (Exception) {

      }
    }
  }
}
