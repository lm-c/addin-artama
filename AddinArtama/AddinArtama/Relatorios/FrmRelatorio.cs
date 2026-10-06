
using LmCorbieUI;
using LmCorbieUI.LmForms;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AddinArtama {
  public partial class FrmRelatorio : LmSingleForm {
    public byte[] bytesPDF = null;
    bool _imprimirAoAbrir = false;

    public FrmRelatorio(List<BindingSource> listas, List<string> nomeDataSet, string reportEmbeddedResource, List<ReportParameter> parametros, bool autoHide = false, bool imprimirAoAbrir = false) {
      InitializeComponent();

      _imprimirAoAbrir = imprimirAoAbrir;

      WindowState = FormWindowState.Maximized;

      try {
        rpv1.LocalReport.DataSources.Clear();

        int cont = 0;
        foreach (BindingSource bs in listas) {
          rpv1.LocalReport.ReportEmbeddedResource = reportEmbeddedResource;

          if (nomeDataSet.Count > 0) {
            ReportDataSource rptDTSource = new ReportDataSource(nomeDataSet[cont], bs);
            rpv1.LocalReport.DataSources.Add(rptDTSource);
            cont++;
          }
        }
        rpv1.LocalReport.EnableHyperlinks = true;
        rpv1.LocalReport.EnableExternalImages = true;
        rpv1.LocalReport.SetParameters(parametros);

        rpv1.LocalReport.Refresh();
        rpv1.RefreshReport();

        if (autoHide) {
          bytesPDF = rpv1.LocalReport.Render("Pdf", null, out string mimeType, out string encoding,
              out string extension, out string[] streamids, out Warning[] warnings);

          var par = rpv1.LocalReport.GetParameters();

          DialogResult = DialogResult.OK;
          this.Close();
        }
      } catch (Exception ex) {
        MsgBox.Show(ex.Message, "Erro ao inicializar relatório",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void FrmRelatorio_Load(object sender, System.EventArgs e) {
      this.rpv1.RefreshReport();
      if (_imprimirAoAbrir)
        timer1.Enabled = true;
    }

    private void FrmRelatorio_KeyDown(object sender, KeyEventArgs e) {
      if (e.Control && e.KeyCode == Keys.P)
        this.rpv1.PrintDialog();
      else if (e.KeyCode == Keys.Escape)
        Close();
    }

    private void Timer1_Tick(object sender, System.EventArgs e) {
      if (Convert.ToInt16(timer1.Tag) >= 3) {
        timer1.Enabled = false;
        try {
          this.rpv1.PrintDialog();
        } catch (Exception) {
          timer1.Tag = 0;
          timer1.Enabled = true;
        }
      } else {
        timer1.Tag = Convert.ToInt16(timer1.Tag) + 1;
      }
    }
  }
}
