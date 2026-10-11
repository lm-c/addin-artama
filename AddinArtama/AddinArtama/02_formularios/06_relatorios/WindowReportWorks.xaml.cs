using LmCorbieUI;
using LmCorbieUI.Metodos;
using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Mouse = System.Windows.Input.Mouse;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>
  /// Conferência da estrutura da montagem (ReportWorks): edição, CSV e gravação das propriedades
  /// no SolidWorks — substitui o FrmReportWorks. Abre como janela separada (não modal).
  /// </summary>
  public partial class WindowReportWorks : Wpf.LmWindow {
    /// <summary>Opção do filtro de seriado (descrição do enum).</summary>
    public class OpcaoSeriado {
      public SeriadoFiltro Valor { get; set; }
      public string Descricao { get; set; }
    }

    /// <summary>Pinta as células com erro (coluna 0 + as colunas do erro), como no WinForms.</summary>
    sealed class CorErroConverter : IValueConverter {
      readonly WindowReportWorks tela;

      public CorErroConverter(WindowReportWorks tela) => this.tela = tela;

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is ReportWorks rpt && tela.erros.TryGetValue(rpt, out var colunas) && colunas.Contains((int)parameter))
          return Brushes.OrangeRed;

        return DependencyProperty.UnsetValue;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    protected string FileNameCSV;

    SortableBindingList<ReportWorks> _reports = new SortableBindingList<ReportWorks>();
    ICollectionView visao;

    /// <summary>Linha → colunas pintadas de erro.</summary>
    readonly Dictionary<ReportWorks, HashSet<int>> erros = new Dictionary<ReportWorks, HashSet<int>>();

    public WindowReportWorks() {
      InitializeComponent();

      cmbSeriado.ItemsSource = Enum.GetValues(typeof(SeriadoFiltro)).Cast<SeriadoFiltro>()
        .Select(x => new OpcaoSeriado { Valor = x, Descricao = x.ObterDescricaoEnum() })
        .ToList();

      Loaded += WindowReportWorks_Loaded;
    }

    private void WindowReportWorks_Loaded(object sender, RoutedEventArgs e) {
      Loaded -= WindowReportWorks_Loaded;

      // Fundo de erro por coluna, mantendo o estilo de célula do tema.
      var conversor = new CorErroConverter(this);
      for (int i = 0; i < dgv.Columns.Count; i++) {
        var estilo = new Style(typeof(DataGridCell), dgv.CellStyle);
        estilo.Setters.Add(new Setter(Control.BackgroundProperty, new Binding { Converter = conversor, ConverterParameter = i }));
        dgv.Columns[i].CellStyle = estilo;
      }

      if (InfoSetting.UnicodeUtf8)
        rdbUtf8.IsChecked = true;
      else if (InfoSetting.UnicodeIso)
        rdbIso.IsChecked = true;

      rdbUtf8.Checked += RdbUtf8_Checked;
      rdbIso.Checked += RdbIso_Checked;

      cmbSeriado.SelectedValue = SeriadoFiltro.Todos;

      VerificarErros();
    }

    void CarregarGrid(SortableBindingList<ReportWorks> reports) {
      _reports = reports;

      // ListCollectionView: a view padrão de um BindingList não aceita Filter.
      visao = _reports != null ? new ListCollectionView(_reports) { Filter = Filtrar } : null;

      dgv.ItemsSource = visao;
    }

    ReportWorks LinhaAtual => dgv.SelectedItem as ReportWorks;

    private async void BtnLoad_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          swModel.ShowNamedView("*Isométrica");
          swModel.ViewZoomtofit();
          SortableBindingList<ReportWorks> reports = null;

          using (var worker = new LmBackgroundWorker()) {
            await worker.RunAsync("Lendo componentes da montagem...", (progress, token) => {
              progress.Report(("Lendo componentes da montagem...", 0, 0));
              reports = ReportWorks.GetReport();
              return Task.CompletedTask;
            });
          }

          CarregarGrid(reports);

          if (_reports != null)
            VerificarErros();
        } else {
          Wpf.MsgBox.Show($"Comando Apenas para Montagem", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao carregar componentes");
      }
    }

    private void BtnAbrir_Click(object sender, RoutedEventArgs e) {
      if (Sw.App.ActiveDoc != null) {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        var ofd = new OpenFileDialog {
          InitialDirectory = Path.GetDirectoryName(swModel.GetPathName()),
          FileName = Path.GetFileNameWithoutExtension(swModel.GetPathName()),
          Title = "Selecionar arquivo CSV",
          Filter = "CSV|*.csv|All files|*.*",
          DefaultExt = "csv",
        };

        if (ofd.ShowDialog(this) == true) {
          CarregarGrid(ReadFile(ofd.FileName));

          FileNameCSV = ofd.FileName;
          VerificarErros();
        }
      }
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      if (_reports == null || _reports.Count == 0) return;

      dgv.CommitEdit(DataGridEditingUnit.Row, true);

      if (Sw.App.ActiveDoc != null && !string.IsNullOrEmpty(_reports[0].PathName)) {
        FileNameCSV = Path.GetDirectoryName(_reports[0].PathName) + "\\" + _reports[0].Componente + ".csv";

        WriteFile(FileNameCSV);

        if (File.Exists(FileNameCSV))
          AbrirCSV(FileNameCSV);
      } else {
        var sfd = new SaveFileDialog {
          Title = "Salvar arquivo CSV do ReportWorks",
          Filter = "CSV|*.csv|All files|*.*",
          DefaultExt = "csv",
        };
        if (!string.IsNullOrEmpty(FileNameCSV))
          sfd.InitialDirectory = Path.GetDirectoryName(FileNameCSV);

        if (sfd.ShowDialog(this) == true) {
          FileNameCSV = sfd.FileName;
          WriteFile(FileNameCSV);

          if (File.Exists(FileNameCSV))
            AbrirCSV(FileNameCSV);
        }
      }
    }

    private bool Open3D() {
      bool _return = true;

      try {
        string openFileName = "";
        int status = 0;
        int warnings = 0;
        openFileName = LinhaAtual.PathName.ToUpper();

        if (openFileName.Contains("SLDPRT")) {
          Sw.App.OpenDoc6(openFileName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(openFileName, false, (int)errors);
        } else if (openFileName.Contains("SLDASM")) {
          Sw.App.OpenDoc6(openFileName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(openFileName, false, (int)errors);
        }
        _return = true;
      } catch (Exception) {
        _return = false;
      }

      return _return;
    }

    #region Erros

    private void VerificarErros() {
      try {
        erros.Clear();

        if (_reports == null)
          return;

        int rowIndex = 0;

        foreach (ReportWorks rpt in _reports) {
          rowIndex = _reports.IndexOf(rpt);

          if (rowIndex == 0) continue;

          if (((rpt.Componente.StartsWith("1") || rpt.Componente.StartsWith("2") || rpt.Componente.StartsWith("3")) && rpt.Seriado == "Não" && rpt.Interno != "Sim"))
            AlterarCorCelulas(rpt, 3, 7);

          if ((!rpt.Componente.StartsWith("1") && !rpt.Componente.StartsWith("2") && !rpt.Componente.StartsWith("3") && rpt.Seriado == "Sim"))
            AlterarCorCelulas(rpt, 3, 7);

          if ((rpt.Componente.StartsWith("1") || rpt.Componente.StartsWith("2") || rpt.Componente.StartsWith("3")) && rpt.Interno != "Sim" && string.IsNullOrEmpty(rpt.Operacao))
            AlterarCorCelulas(rpt, 3, 8);

          if ((!rpt.Componente.StartsWith("1") && !rpt.Componente.StartsWith("2") && !rpt.Componente.StartsWith("3") && rpt.Interno != "Sim") &&
              (!string.IsNullOrEmpty(rpt.Componente) && (!string.IsNullOrEmpty(rpt.Operacao) || !string.IsNullOrEmpty(rpt.Comprimento))))
            AlterarCorCelulas(rpt, 3, 8);

          if (!string.IsNullOrEmpty(rpt.Codigo) && (string.IsNullOrEmpty(rpt.Comprimento)))
            AlterarCorCelulas(rpt, 2, 6);

          if (!string.IsNullOrEmpty(rpt.Codigo) && (string.IsNullOrEmpty(rpt.Operacao)))
            AlterarCorCelulas(rpt, 2, 8);

          if (string.IsNullOrEmpty(rpt.Codigo) && (string.IsNullOrEmpty(rpt.Componente)))
            AlterarCorCelulas(rpt, 2, 3);

          if (!string.IsNullOrEmpty(rpt.Componente) && (string.IsNullOrEmpty(rpt.Seriado)))
            AlterarCorCelulas(rpt, 3, 7);

          if (string.IsNullOrEmpty(rpt.Denominacao))
            AlterarCorCelulas(rpt, 0, 5);
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Não foi possível verificar erros");
      } finally {
        dgv.Items.Refresh();
      }
    }

    private void AlterarCorCelulas(ReportWorks rpt, int celIndex1, int celIndex2) {
      if (!erros.TryGetValue(rpt, out var colunas))
        erros[rpt] = colunas = new HashSet<int>();

      colunas.Add(0);
      colunas.Add(celIndex1);
      colunas.Add(celIndex2);
    }

    #endregion

    #region Linhas

    private void Dgv_PreviewKeyDown(object sender, KeyEventArgs e) {
      // Delete exclui a linha (fora da edição de uma célula).
      if (e.Key != Key.Delete || e.OriginalSource is TextBox)
        return;

      e.Handled = true;
      try {
        DeletearLinha();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao deletar linha");
      }
    }

    private void TsmOpen3D_Click(object sender, RoutedEventArgs e) {
      try {
        if (!Open3D()) return;
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao abrir arquivo");
      }
    }

    private void TsmDelete_Click(object sender, RoutedEventArgs e) {
      try {
        DeletearLinha();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao deletar linha");
      }
    }

    private void DeletearLinha() {
      var linha = LinhaAtual;
      if (linha == null)
        return;

      _reports.Remove(linha);
      dgv.Items.Refresh();
    }

    private void TsmAdd_Click(object sender, RoutedEventArgs e) {
      try {
        if (_reports == null)
          CarregarGrid(new SortableBindingList<ReportWorks>());

        var novo = new ReportWorks();
        _reports.Add(novo);
        dgv.Items.Refresh();
        dgv.SelectedItem = novo;
        dgv.ScrollIntoView(novo);
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao adicionar linha");
      }
    }

    private void TsmAltDados_Click(object sender, RoutedEventArgs e) {
      string fileName = null;
      var swModel = (ModelDoc2)Sw.App.ActiveDoc;
      var atual = LinhaAtual;

      if (atual == null)
        return;

      dgv.CommitEdit(DataGridEditingUnit.Row, true);

      try {
        if (Open3D()) {
          fileName = swModel.GetPathName().ToUpper();

          var swModelDocExt = swModel.Extension;
          var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

          if (!string.IsNullOrEmpty(atual.Codigo)) {
            int nCutList = 1;
            while (true) {
              int indexCurrent = _reports.IndexOf(atual);
              if (!string.IsNullOrEmpty(_reports[indexCurrent - nCutList].Codigo))
                nCutList++;
              else
                break;
            }

            if (!ReportWorks.UpdateCutList(nCutList, atual))
              Wpf.MsgBox.Show($"Lista de Corte não foi atualizada", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Warning);
          } else {
            if (atual.Seriado == "Não") {
              swCustPropMgr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText,
                  atual.Denominacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

              swCustPropMgr.Add3("Componente", (int)swCustomInfoType_e.swCustomInfoText,
                  atual.Componente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
            }

            swCustPropMgr.Add3("Seriado", (int)swCustomInfoType_e.swCustomInfoText,
                atual.Seriado, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

            if (!string.IsNullOrEmpty(atual.Operacao))
              swCustPropMgr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText,
               atual.Operacao.Replace("/", "^"), (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
          }
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao salvar dados");
      } finally {
        if (!string.IsNullOrEmpty(fileName)) {
          swModel.Save();
          Sw.App.CloseDoc(fileName);
        }
      }
    }

    #endregion

    #region CSV

    private SortableBindingList<ReportWorks> ReadFile(string filename) {
      Mouse.OverrideCursor = Cursors.Wait;

      List<ReportWorks> listReport = new List<ReportWorks>();

      try {
        string encoding = rdbUtf8.IsChecked == true ? "UTF-8" : "ISO-8859-1";
        using (StreamReader leitor = new StreamReader(filename, Encoding.GetEncoding(encoding))) {
          string linha = leitor.ReadLine();
          linha = leitor.ReadLine();

          while (linha != null) {
            string[] columns = linha.Split(';');

            listReport.Add(new ReportWorks() {
              Nivel = columns[0].Trim().Replace(",", "."),
              Qtd = Convert.ToDouble(columns[1].Trim()).ToString("0"),
              Codigo = columns[2].Trim(),
              Componente = columns[3].Trim(),
              Material = columns[4].Trim(),
              Denominacao = columns[5].Trim(),
              Comprimento = columns[6].Trim().Replace(",", "."),
              Seriado = columns[7].Trim(),
              Operacao = columns[8].Trim(),
              Maquina = columns[9].Trim(),
            });

            linha = leitor.ReadLine();
          }
          leitor.Close();
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao ler arquivo");
      } finally {
        Mouse.OverrideCursor = null;
      }

      return new SortableBindingList<ReportWorks>(listReport);
    }

    private bool WriteFile(string filename) {
      Mouse.OverrideCursor = Cursors.Wait;
      try {
        using (FileStream fs = new FileStream(filename, FileMode.Create)) {
          string encoding = rdbUtf8.IsChecked == true ? "UTF-8" : "ISO-8859-1";
          using (StreamWriter file = new StreamWriter(fs, Encoding.GetEncoding(encoding))) {
            string linha = "Nivel;Quantidade;Codigo;Componente;Material;Denominacao;Comprimento;Seriado;Operacao;Maquina";
            file.WriteLine(linha);

            foreach (var rpt in _reports) {
              linha = string.Join(";",
                rpt.Nivel,
                Convert.ToDouble(rpt.Qtd).ToString("0.000000"),
                rpt.Codigo,
                rpt.Componente,
                rpt.Material,
                rpt.Denominacao,
                rpt.Comprimento,
                rpt.Seriado,
                rpt.Operacao,
                rpt.Maquina);
              file.WriteLine(linha);
            }
            file.Close();
          }
          fs.Close();
          System.Threading.Thread.Sleep(500);
        }

        return true;
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao escrever arquivo");
      } finally {
        Mouse.OverrideCursor = null;
      }
      return false;
    }

    private void AbrirCSV(string FileNameCSV) {
      if (Wpf.MsgBox.Show("ReportWorks Salvo Com Sucesso.\nDeseja Abrir o Arquivo", "Addin LM Projetos",
          MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
        Process.Start(FileNameCSV);
      }
    }

    private void RdbUtf8_Checked(object sender, RoutedEventArgs e) {
      InfoSetting.UnicodeUtf8 = true;
      InfoSetting.UnicodeIso = false;
      InfoSetting.Salvar();
    }

    private void RdbIso_Checked(object sender, RoutedEventArgs e) {
      InfoSetting.UnicodeUtf8 = false;
      InfoSetting.UnicodeIso = true;
      InfoSetting.Salvar();
    }

    #endregion

    #region Filtros

    /// <summary>Filtro de seriado + texto procurado em qualquer coluna.</summary>
    private bool Filtrar(object item) {
      if (!(item is ReportWorks rpt))
        return true;

      var seriado = cmbSeriado.SelectedValue is SeriadoFiltro s ? s : SeriadoFiltro.Todos;
      switch (seriado) {
        case SeriadoFiltro.Sim:
          if (rpt.Seriado != "Sim") return false;
          break;
        case SeriadoFiltro.Nao:
          if (rpt.Seriado != "Não") return false;
          break;
        case SeriadoFiltro.Vazia:
          if (!string.IsNullOrEmpty(rpt.Seriado)) return false;
          break;
      }

      var procurar = txtProcurar.Text;
      if (string.IsNullOrWhiteSpace(procurar))
        return true;

      return new[] { rpt.Nivel, rpt.Qtd, rpt.Codigo, rpt.Componente, rpt.Material, rpt.Denominacao, rpt.Comprimento, rpt.Seriado, rpt.Operacao, rpt.Maquina }
        .Any(x => x != null && x.IndexOf(procurar, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    private void CmbSeriado_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      try {
        if (cmbSeriado.SelectedValue == null) {
          cmbSeriado.SelectedValue = SeriadoFiltro.Todos;
          return;
        }

        dgv.CommitEdit(DataGridEditingUnit.Row, true);
        visao?.Refresh();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao filtrar Seriado");
      }
    }

    private void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e) {
      dgv.CommitEdit(DataGridEditingUnit.Row, true);
      visao?.Refresh();
    }

    #endregion

    private void BtnSubstituir_Click(object sender, RoutedEventArgs e) {
      try {
        if (string.IsNullOrEmpty(txtDe.Text) || string.IsNullOrEmpty(txtPara.Text) || _reports == null) return;

        string de = txtDe.Text;
        string para = txtPara.Text;

        foreach (var report in _reports.Where(x => !string.IsNullOrEmpty(x.Operacao)).ToList()) {
          if (report.Operacao.Contains(de))
            report.Operacao = report.Operacao.Replace(de, para);
        }

        dgv.Items.Refresh();

        Wpf.MsgBox.Show("Substituido com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao substituir componentes");
      }
    }
  }
}
