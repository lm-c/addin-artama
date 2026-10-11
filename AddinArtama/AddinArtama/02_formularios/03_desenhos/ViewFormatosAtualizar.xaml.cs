using LmCorbieUI;
using LmCorbieUI.Metodos;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Troca o formato de folha dos desenhos da montagem pelos templates configurados (substitui o FrmFormatosAtualizar).</summary>
  public partial class ViewFormatosAtualizar : Wpf.LmView {
    SortableBindingList<DesenhosAtualizar> _dadosDesenho = new SortableBindingList<DesenhosAtualizar>();

    public ViewFormatosAtualizar() {
      InitializeComponent();

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private System.Collections.Generic.IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Atualizar Templates dos Desenhos",
        "Quando o template (formato de folha) da empresa muda, esta tela troca o formato de todos os desenhos de uma montagem de uma vez, sem abrir um por um.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Aguardar("1. Abra a montagem",
        "No SolidWorks, abra (e deixe ativa) a montagem do projeto.\n\n" +
        "Regra: esta tela só funciona com uma MONTAGEM ativa; com peça ou desenho o carregamento é recusado.",
        TemMontagemAtiva);

      yield return Wpf.LmPassoAssistente.Aguardar("2. Carregar os desenhos",
        "Clique em Carregar Desenhos e aguarde a leitura dos componentes da montagem (pode levar um tempo em montagens grandes).",
        () => dgv.Items.Count > 0, btnCarregar);

      yield return Wpf.LmPassoAssistente.Info("3. Escolha o que atualizar",
        "Cada linha é um desenho. A caixa da primeira coluna define se ele será atualizado.\n\n" +
        "Com o botão direito você marca ou desmarca todos de uma vez, e abre o 3D ou o 2D da linha.",
        dgv);

      yield return Wpf.LmPassoAssistente.Info("4. Pesquisar",
        "Digite parte do código do componente ou da denominação e tecle Enter (ou F7) para ir ao próximo resultado. No fim da lista a busca volta ao início.",
        txtPesquisar);

      yield return Wpf.LmPassoAssistente.Info("5. Iniciar a atualização",
        "Para cada desenho marcado, o sistema abre o desenho, troca o formato de TODAS as folhas pelo template configurado para o tamanho de cada uma, salva e fecha.\n\n" +
        "Regras:\n" +
        "• A atualização começa na LINHA SELECIONADA e vai até o fim da lista — as linhas acima dela não são processadas. Para atualizar tudo, selecione a primeira linha.\n" +
        "• Se o desenho tinha lista de materiais, ela é reinserida: a geral na primeira folha e a da posição nas folhas P1, P2...\n" +
        "• O andamento aparece na tela e pode ser cancelado.\n\n" +
        "Os templates usados são os de Configuração > Configurar Templates.",
        btnAtualizar);

      yield return Wpf.LmPassoAssistente.Info("Pronto!",
        "Ao terminar, aparece a mensagem de sucesso e os desenhos já estão salvos com o novo formato.\n\n" +
        "Para rever este passo a passo, clique no \"?\" no topo da tela.");
    }

    /// <summary>Há uma montagem ativa no SolidWorks (também usado pelos assistentes das outras telas).</summary>
    internal static bool TemMontagemAtiva() {
      var swModel = Sw.App.ActiveDoc as ModelDoc2;
      return swModel != null && swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY;
    }

    #endregion

    DesenhosAtualizar Atual => dgv.SelectedItem as DesenhosAtualizar;

    private async void BtnCarrDesenhos_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY) {
          Wpf.Toast.Warning("Comando apenas para Montagens");
          return;
        }

        btnCarregar.IsEnabled = btnAtualizar.IsEnabled = false;
        using (var worker = new LmBackgroundWorker()) {
          await worker.RunAsync("Iniciando leitura Componentes...", async (progress, token) => {
            _dadosDesenho = await DesenhosAtualizar.GetDesenhosAsync(progress, token);

            progress.Report(("Carregando Grid...", 100, 100));
          });
        }

        CarregarGrid();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao carregar desenhos");
      } finally {
        btnCarregar.IsEnabled = btnAtualizar.IsEnabled = true;
      }
    }

    private void CarregarGrid() {
      dgv.ItemsSource = null;
      dgv.ItemsSource = _dadosDesenho;

      if (_dadosDesenho.Count > 0)
        dgv.SelectedIndex = 0;
    }

    private async void BtnAtualizar_Click(object sender, RoutedEventArgs e) {
      if (_dadosDesenho.Count == 0) {
        Wpf.Toast.Warning("Carregar Desenhos primeiro");
        return;
      }

      try {
        btnCarregar.IsEnabled = btnAtualizar.IsEnabled = false;

        var swModel = default(ModelDoc2);

        templates.Carregar();

        int status = 0;
        int warnings = 0;

        // Começa na linha selecionada, como no WinForms.
        int inicio = Math.Max(dgv.SelectedIndex, 0);

        using (var worker = new LmBackgroundWorker()) {
          await worker.RunAsync("Iniciando leitura Componentes...", async (progress, token) => {
            var total = _dadosDesenho.Count;
            for (int i = inicio; i <= _dadosDesenho.Count - 1; i++) {
              if (token.IsCancellationRequested) {
                Wpf.Toast.Info("Operação cancelada pelo usuário.");
                return;
              }

              var file = _dadosDesenho[i];

              Dispatcher.Invoke(() => {
                dgv.SelectedItem = file;
                dgv.ScrollIntoView(file);
              });

              if (file.Atualizar) {
                progress.Report(($"Atualizando {file.ShortName}", i + 1, total));
                try {
                  Sw.App.OpenDoc6(file.PathName, (int)swDocumentTypes_e.swDocDRAWING,
                                         (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
                } catch (Exception ex) {
                  Wpf.Toast.Error($"Erro ao abrir arquivo \"{file.PathName}\"\n\n{ex.Message}");
                }

                Sw.App.ActivateDoc2(file.PathName, false, 0);
                swModel = (ModelDoc2)Sw.App.ActiveDoc;

                UpdateFormato(swModel);

                swModel.Save();
                Sw.App.CloseDoc(file.PathName);
              }
            }

            progress.Report(("Concluído!", 0, 0));
          });
        }

        Wpf.MsgBox.Show($"Formatos de folha atualizados com sucesso!\n", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao atualizar template");
      } finally {
        btnCarregar.IsEnabled = btnAtualizar.IsEnabled = true;
      }
    }

    #region Menu da grade

    private void TsmSelectAll_Click(object sender, RoutedEventArgs e) {
      foreach (var item in _dadosDesenho)
        item.Atualizar = true;
      dgv.Items.Refresh();
    }

    private void TsmUnselectAll_Click(object sender, RoutedEventArgs e) {
      foreach (var item in _dadosDesenho)
        item.Atualizar = false;
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

    #region Pesquisa

    private void TxtPesquisar_BotaoF7Click(object sender, RoutedEventArgs e) {
      ProcurarProximo();
    }

    private void TxtPesquisar_PreviewKeyDown(object sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
        e.Handled = true;
        ProcurarProximo();
      }
    }

    /// <summary>Seleciona o próximo desenho cujo componente ou denominação contém o texto (volta ao início).</summary>
    private void ProcurarProximo() {
      var txt = txtPesquisar.Text.ToUpper();
      int total = _dadosDesenho.Count;
      if (total == 0 || string.IsNullOrEmpty(txt))
        return;

      int index = dgv.SelectedIndex;
      for (int n = 1; n <= total; n++) {
        var item = _dadosDesenho[(index + n) % total];
        if (Convert.ToString(item.ShortName).ToUpper().Contains(txt) || Convert.ToString(item.Denominacao).ToUpper().Contains(txt)) {
          dgv.SelectedItem = item;
          dgv.ScrollIntoView(item);
          return;
        }
      }
    }

    #endregion

    #region Formato de folha

    public static void UpdateFormato(ModelDoc2 swModel) {
      try {
        DrawingDoc swDraw = (DrawingDoc)swModel;
        string[] sheetNames = swDraw.GetSheetNames();

        var name = Path.GetFileNameWithoutExtension(swModel.GetPathName());

        if (sheetNames == null || sheetNames.Length == 0) return;

        // Limpar tabelas existentes em todas as folhas
        Desenho.ClearExistingTables(swModel, out bool hasExistingTable);

        var activeSheetName = string.Empty;

        // Processar todas as folhas
        for (int i = 0; i < sheetNames.Length; i++) {
          string sheetName = sheetNames[i];

          if (string.IsNullOrEmpty(activeSheetName))
            activeSheetName = sheetName;

          // Ativar a folha atual
          bool sheetActivated = swDraw.ActivateSheet(sheetName);
          if (!sheetActivated) continue;

          Sheet swSheet = swDraw.GetCurrentSheet();
          if (swSheet == null) continue;

          // Atualizar formato da folha
          UpdateSheetFormat(swDraw, swSheet);

          if (hasExistingTable) {
            // Inserir lista de materiais apenas na primeira folha
            if (i == 0) {
              Desenho.InsertMaterialsList(swModel);
            } else if (i > 0) {
              string nomeFolha = sheetName.ToUpper();

              if (nomeFolha.StartsWith("P") && int.TryParse(nomeFolha.Substring(1), out int posicaoDesejada)) {
                Desenho.InsertMaterialsList(swModel, posicaoDesejada);
              }
            }
          }
        }
        if (!string.IsNullOrEmpty(activeSheetName))
          swDraw.ActivateSheet(activeSheetName);

        swModel.ViewZoomtofit2();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao atualizar formato de folhas do desenho");
      }
    }

    private static void UpdateSheetFormat(DrawingDoc swDraw, Sheet swSheet) {
      double[] vSheetProps = swSheet.GetProperties();
      if (vSheetProps == null || vSheetProps.Length < 7) return;

      var largura = Convert.ToDouble(vSheetProps[5]) * 1000;
      var altura = Convert.ToDouble(vSheetProps[6]) * 1000;

      var format = Desenho.GetFormat(largura, altura);
      ApplySheetFormat(swDraw, swSheet, format, vSheetProps);
    }

    private static void ApplySheetFormat(DrawingDoc swDraw, Sheet swSheet, SwDwgPaperSizes_e format, double[] vSheetProps) {
      string sheetName = swSheet.GetName();

      switch (format) {
        case SwDwgPaperSizes_e.A4R:
        // Transição A3 -> A4R
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.42, 0.297, templates.model.template_a3,
            0.21, 0.297, templates.model.template_a4r);
        break;

        case SwDwgPaperSizes_e.A4P:
        // Transição A3 -> A4P
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.42, 0.297, templates.model.template_a3,
            0.297, 0.21, templates.model.template_a4p);
        break;

        case SwDwgPaperSizes_e.A3:
        // Transição A4R -> A3
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.21, 0.297, templates.model.template_a4r,
            0.42, 0.297, templates.model.template_a3);
        break;

        case SwDwgPaperSizes_e.A2:
        // Transição A3 -> A2
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.42, 0.297, templates.model.template_a3,
            0.594, 0.42, templates.model.template_a2);
        break;

        case SwDwgPaperSizes_e.A1:
        // Transição A2 -> A1
        SetupSheetTransition(swDraw, sheetName, vSheetProps,
            0.594, 0.42, templates.model.template_a2,
            0.840, 0.594, templates.model.template_a1);
        break;
      }
    }

    private static void SetupSheetTransition(DrawingDoc swDraw, string sheetName, double[] vSheetProps,
    double tempWidth, double tempHeight, string tempTemplate,
    double finalWidth, double finalHeight, string finalTemplate) {
      // Configuração temporária
      bool boolstatus = swDraw.SetupSheet5(sheetName, 12, 12, vSheetProps[2], vSheetProps[3],
          true, tempTemplate, tempWidth, tempHeight, "'", true);

      // Configuração final
      boolstatus = swDraw.SetupSheet5(sheetName, 12, 12, vSheetProps[2], vSheetProps[3],
          true, finalTemplate, finalWidth, finalHeight, "'", true);
    }

    #endregion
  }
}
