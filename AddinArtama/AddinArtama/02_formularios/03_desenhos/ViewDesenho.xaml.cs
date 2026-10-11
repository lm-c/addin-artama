using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Navegação peça a peça para criar/detalhar desenhos e trocar formato de folha (substitui o FrmDesenho).</summary>
  public partial class ViewDesenho : Wpf.LmView {
    List<Desenho> dadosDesenho = new List<Desenho>();
    string montagemGeral = "";

    // Monitora o disco: quando o desenho de um componente passa a existir, a linha deixa de ficar vermelha.
    readonly DispatcherTimer tmrStatus2D = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };

    public ViewDesenho() {
      InitializeComponent();

      tmrStatus2D.Tick += (s, e) => AtualizarStatus2D();
      Loaded += (s, e) => tmrStatus2D.IsEnabled = dadosDesenho.Count > 0;
      Unloaded += (s, e) => tmrStatus2D.Stop();

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Criar/Detalhar Desenhos",
        "Esta tela percorre, um a um, os componentes de uma peça ou montagem para você criar ou revisar o desenho 2D de cada um.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Aguardar("1. Abra a peça ou a montagem",
        "No SolidWorks, abra (e deixe ativa) a peça ou a montagem principal do projeto.\n\n" +
        "Regra: o carregamento só funciona com peça ou montagem ativa; com um desenho ativo ele é recusado.",
        TemModeloAtivo);

      yield return Wpf.LmPassoAssistente.Clique("2. Carregar os componentes",
        "Clique em Carregar para listar a montagem ativa e todos os componentes dela.\n\n" +
        "Regra: a montagem carregada vira a \"montagem geral\" da sessão. Ela continua aberta enquanto você avança pelas peças.",
        btnCarregar);

      yield return Wpf.LmPassoAssistente.Info("3. A lista de componentes",
        "Cada linha é um componente. O primeiro ícone mostra o tipo (peça ou montagem) e o segundo se já existe desenho.\n\n" +
        "Regra: o nome em vermelho indica componente SEM desenho — o desenho é o arquivo .SLDDRW com o mesmo nome, na mesma pasta do modelo. " +
        "Quando o desenho passa a existir, o destaque em vermelho é removido e o ícone de desenho é atualizado.\n\n" +
        "Com o botão direito você abre o 3D ou o 2D da linha.",
        dgv);

      yield return Wpf.LmPassoAssistente.Clique("4. Avançar para o componente",
        "Clique em Avançar para abrir o componente da linha selecionada. Se ele já tiver desenho, o desenho abre junto; se não, o modelo fica em vista isométrica.\n\n" +
        "Regra: antes de abrir, os documentos abertos (menos a montagem geral) são SALVOS e fechados. " +
        "Depois de abrir, a seleção já passa para a próxima linha.",
        btnAvancar);

      yield return Wpf.LmPassoAssistente.Info("5. Formato da folha",
        "Com a peça ou montagem ativa (sem desenho), clique em um formato para CRIAR o desenho nele: " +
        "peça recebe as vistas \"Padrão plano\" e isométrica; montagem recebe a isométrica. A lista de materiais é inserida junto.\n\n" +
        "Regras: se o desenho já existir, ele apenas é aberto. Com um desenho ativo, o botão TROCA o formato da folha atual e reinsere a lista.\n\n" +
        "Os templates de cada formato vêm de Configuração > Configurar Templates.",
        btnA4R, btnA1);

      yield return Wpf.LmPassoAssistente.Info("6. Folhas do desenho",
        "Com um desenho ativo, estes botões vão para a folha anterior e para a próxima.",
        btnFolhaAnterior, btnFolhaPosterior);

      yield return Wpf.LmPassoAssistente.Info("7. Nome da folha",
        "Mostra o nome da folha ativa do desenho. Para renomear, digite o novo nome e use o botão do campo ou a tecla F7.\n\n" +
        "Regra: folhas chamadas P1, P2, P3... recebem a lista de materiais daquela posição; a primeira folha recebe a lista geral.",
        txtName);

      yield return Wpf.LmPassoAssistente.Info("8. Inserir lista",
        "Refaz as listas de materiais do desenho ativo.\n\n" +
        "Regra: as tabelas existentes são apagadas em todas as folhas e inseridas de novo — a geral na primeira folha e a da posição nas folhas P1, P2...",
        btnInserirLista);

      yield return Wpf.LmPassoAssistente.Info("9. Progresso",
        "Aqui você acompanha em qual componente está e o percentual já percorrido.",
        lblPercDesenho);

      yield return Wpf.LmPassoAssistente.Info("Pronto!",
        "O ciclo é: Avançar → criar ou revisar o desenho → Avançar de novo, até o fim da lista.\n\n" +
        "Para rever este passo a passo, clique no \"?\" no topo da tela.");
    }

    /// <summary>Há uma peça ou montagem ativa no SolidWorks (também usado pelos assistentes das outras telas).</summary>
    internal static bool TemModeloAtivo() {
      var swModel = Sw.App.ActiveDoc as ModelDoc2;
      return swModel != null && swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING;
    }

    #endregion

    private void AtualizarStatus2D() {
      try {
        foreach (var desenho in dadosDesenho)
          desenho.AtualizarTemDesenho();
      } catch (Exception ex) {
        tmrStatus2D.Stop();
        Msg.Erro(ex, "Erro ao atualizar status dos desenhos");
      }
    }

    Desenho Atual => dgv.SelectedItem as Desenho;

    private void Dgv_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (dadosDesenho.Count > 0 && dgv.SelectedIndex >= 0) {
        var pos = dgv.SelectedIndex + 1;
        lblPercDesenho.Content = $"{pos} de {dadosDesenho.Count} - {pos * 100 / dadosDesenho.Count}%";
      }
    }

    private void BtnLoadDesenho_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          Wpf.Toast.Warning("Comando Apenas para Peça e Montagem");
          return;
        }

        dadosDesenho = Desenho.GetDesenhos(swModel) ?? new List<Desenho>();
        dgv.ItemsSource = dadosDesenho;
        if (dadosDesenho.Count > 0)
          dgv.SelectedIndex = 0;

        tmrStatus2D.IsEnabled = dadosDesenho.Count > 0;

        montagemGeral = swModel.GetPathName();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao carregar componentes");
      }
    }

    /// <summary>Fecha os documentos abertos, abre a peça atual (e o desenho, se existir) e avança a linha.</summary>
    private void BtnFFdesenho_Click(object sender, RoutedEventArgs e) {
      string fileNameDesenho = "";
      try {
        if (dadosDesenho.Count == 0 || Atual == null) {
          Wpf.Toast.Warning("Favor Carregar Componentes primeiro.");
          return;
        }

        var atual = Atual;
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        CloseDocs(swModel);

        if (atual.Tipo3D == "P")
          fileNameDesenho = atual.PathName.ToUpper().Replace("SLDPRT", "SLDDRW");
        else if (atual.Tipo3D == "M")
          fileNameDesenho = atual.PathName.ToUpper().Replace("SLDASM", "SLDDRW");

        int status = 0;
        int warnings = 0;
        Sw.App.OpenDoc6(atual.PathName,
            (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
        int errors = 0;
        Sw.App.ActivateDoc2(atual.PathName, false, (int)errors);

        if (File.Exists(fileNameDesenho)) {
          Sw.App.OpenDoc6(fileNameDesenho, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          errors = 0;
          Sw.App.ActivateDoc2(fileNameDesenho, false, (int)errors);
          swModel = (ModelDoc2)Sw.App.ActiveDoc;
          AttachEventHandlers();
        } else {
          swModel = (ModelDoc2)Sw.App.ActiveDoc;

          swModel.ShowNamedView("*Isométrica");
          swModel.ViewZoomtofit();
        }

        // MoveNext: na última linha fica onde está.
        if (dgv.SelectedIndex < dadosDesenho.Count - 1) {
          dgv.SelectedIndex++;
          dgv.ScrollIntoView(dgv.SelectedItem);
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao avançar peça para desenho");
      }
    }

    #region Formatos

    private void BtnA4R_Click(object sender, RoutedEventArgs e) {
      if (Sw.App.ActiveDoc == null) {
        Wpf.Toast.Warning("Sem documentos abertos");
        return;
      }

      var swModel = (ModelDoc2)Sw.App.ActiveDoc;

      if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
        string sheet = templates.model.template_a4p;
        double largura = 0.115;
        double altura = 0.14;
        AltFormato(swModel, sheet, largura, altura);

        sheet = templates.model.template_a4r;
        largura = 0.21;
        altura = 0.297;
        AltFormato(swModel, sheet, largura, altura);
      } else {
        string formato = templates.model.formato_a4r;
        double X = 0.115;
        double Y = 0.14;
        NewDrawing(swModel, formato, X, Y);
      }
    }

    private void BtnA4_Click(object sender, RoutedEventArgs e) {
      if (Sw.App.ActiveDoc == null) {
        Wpf.Toast.Warning("Sem documentos abertos");
        return;
      }

      var swModel = (ModelDoc2)Sw.App.ActiveDoc;

      if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
        string sheet = templates.model.template_a3;
        double largura = 0.42;
        double altura = 0.297;
        AltFormato(swModel, sheet, largura, altura);

        sheet = templates.model.template_a4p;
        largura = 0.115;
        altura = 0.14;
        AltFormato(swModel, sheet, largura, altura);
      } else {
        string formato = templates.model.formato_a4p;
        double X = 0.115;
        double Y = 0.14;
        NewDrawing(swModel, formato, X, Y);
      }
    }

    private void BtnA3_Click(object sender, RoutedEventArgs e) {
      if (Sw.App.ActiveDoc == null) {
        Wpf.Toast.Warning("Sem documentos abertos");
        return;
      }

      var swModel = (ModelDoc2)Sw.App.ActiveDoc;

      if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
        string sheet = templates.model.template_a2;
        double largura = 0.594;
        double altura = 0.42;
        AltFormato(swModel, sheet, largura, altura);

        sheet = templates.model.template_a3;
        largura = 0.42;
        altura = 0.297;
        AltFormato(swModel, sheet, largura, altura);
      } else {
        string formato = templates.model.formato_a3;
        double X = 0.16;
        double Y = 0.15;
        NewDrawing(swModel, formato, X, Y);
      }
    }

    private void BtnA2_Click(object sender, RoutedEventArgs e) {
      if (Sw.App.ActiveDoc == null) {
        Wpf.Toast.Warning("Sem documentos abertos");
        return;
      }

      var swModel = (ModelDoc2)Sw.App.ActiveDoc;

      if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
        string sheet = templates.model.template_a1;
        double largura = 0.841;
        double altura = 0.594;
        AltFormato(swModel, sheet, largura, altura);

        sheet = templates.model.template_a2;
        largura = 0.594;
        altura = 0.42;
        AltFormato(swModel, sheet, largura, altura);
      } else {
        string formato = templates.model.formato_a2;
        double X = 0.24;
        double Y = 0.21;
        NewDrawing(swModel, formato, X, Y);
      }
    }

    private void BtnA1_Click(object sender, RoutedEventArgs e) {
      if (Sw.App.ActiveDoc == null) {
        Wpf.Toast.Warning("Sem documentos abertos");
        return;
      }

      var swModel = (ModelDoc2)Sw.App.ActiveDoc;

      if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
        string sheet = templates.model.template_a2;
        double largura = 0.594;
        double altura = 0.42;
        AltFormato(swModel, sheet, largura, altura);

        sheet = templates.model.template_a1;
        largura = 0.841;
        altura = 0.594;
        AltFormato(swModel, sheet, largura, altura);
      } else {
        string formato = templates.model.formato_a1;
        double X = 0.35;
        double Y = 0.3;
        NewDrawing(swModel, formato, X, Y);
      }
    }

    #endregion

    private void BtnInserirListaSoldagem_Click(object sender, RoutedEventArgs e) {
      InserirListaSoldagem();
    }

    private void InserirListaSoldagem() {
      if (Sw.App.ActiveDoc == null) {
        Wpf.Toast.Warning("Sem documentos abertos");
        return;
      }
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        swModel.ClearSelection2(true);

        DrawingDoc swDraw = (DrawingDoc)swModel;
        string[] sheetNames = swDraw.GetSheetNames();

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
        if (!string.IsNullOrEmpty(activeSheetName))
          swDraw.ActivateSheet(activeSheetName);
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao inserir lista de soldagem");
      }
    }

    private void CloseDocs(ModelDoc2 swModel) {
      try {
        int cont = 0;
        while (swModel != null && swModel.GetPathName() != montagemGeral && cont <= 10) {
          if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING)
            swModel.ViewZoomtofit2();

          if (!swModel.IsOpenedReadOnly())
            swModel.Save();

          Sw.App.CloseDoc(swModel.GetPathName());
          swModel = (ModelDoc2)Sw.App.ActiveDoc;
          cont++;
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao fechar documentos");
      }
    }

    private void AltFormato(ModelDoc2 swModel, string formato, double largura, double altura) {
      bool boolstatus;

      DrawingDoc swDraw;
      swDraw = (DrawingDoc)swModel;
      swDraw = (DrawingDoc)Sw.App.ActiveDoc;
      Sheet swSheet = swDraw.GetCurrentSheet();
      double[] vSheetProps = swSheet.GetProperties();

      boolstatus = swDraw.SetupSheet5(swSheet.GetName(), 12, 12, vSheetProps[2], vSheetProps[3], true, formato, largura, altura, "'", true);

      InserirListaSoldagem();
      swDraw.ForceRebuild();
    }

    private void NewDrawing(ModelDoc2 swModel, string DrawingName, double X, double Y) {
      try {
        int numViews;
        object[] viewNames;
        object[] AtiveConfiguration = null;
        string viewPaletteName;
        swDocumentTypes_e tipoArquivo = swDocumentTypes_e.swDocNONE;
        int i;
        bool boolstatus;

        DrawingDoc swDrawing;
        SolidWorks.Interop.sldworks.View swView;

        tipoArquivo = (swDocumentTypes_e)swModel.GetType();

        string fileName = swModel.GetPathName();
        AtiveConfiguration = (object[])swModel.GetConfigurationNames();
        string activeConfig = (string)AtiveConfiguration[0];

        // se desenho existir abra
        int comp = fileName.Length;
        string nomeDesenho = swModel.GetPathName().Substring(0, comp - 6) + "SLDDRW";
        if (File.Exists(nomeDesenho)) {
          int status = 0;
          int warnings = 0;
          Sw.App.OpenDoc6(nomeDesenho, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          return;
        }

        swDrawing = (DrawingDoc)Sw.App.NewDocument(DrawingName, 5, 0, 0);
        swModel = (ModelDoc2)Sw.App.ActiveDoc;
        swDrawing = (DrawingDoc)swModel;
        var swModelDocExt = swModel.Extension;

        swDrawing.GenerateViewPaletteViews(fileName);

        numViews = 0;
        viewNames = (object[])swDrawing.GetDrawingPaletteViewNames();

        if (!(viewNames == null)) {
          numViews = (viewNames.GetUpperBound(0) - viewNames.GetLowerBound(0));
          for (i = 0; i <= numViews; i++) {
            viewPaletteName = (string)viewNames[i];

            if (tipoArquivo == swDocumentTypes_e.swDocASSEMBLY) {
              if ((viewPaletteName == "*Isométrica")) {
                swModel = (ModelDoc2)Sw.App.ActiveDoc;
                swDrawing = (DrawingDoc)swModel;
                swDrawing = (DrawingDoc)Sw.App.ActiveDoc;

                swView = swDrawing.DropDrawingViewFromPalette2(viewPaletteName, X, Y, 0.0);

                boolstatus = swDrawing.ActivateSheet("Folha1");
                boolstatus = swDrawing.ActivateView("Vista de desenho1");

                Desenho.InserirListasMateriais(tipoArquivo);
                break;
              }
            }

            if (tipoArquivo == swDocumentTypes_e.swDocPART) {
              if ((viewPaletteName == "Padrão plano")) {
                // cria vista 1
                swView = swDrawing.DropDrawingViewFromPalette2(viewPaletteName, X, Y, 0.0);
              }
              if ((viewPaletteName == "*Isométrica")) {
                // cria vista 4
                swView = swDrawing.DropDrawingViewFromPalette2(viewPaletteName, X + 0.06, Y - 0.05, 0.0);
                boolstatus = swModel.Extension.SelectByID2("Vista de desenho1", "DRAWINGVIEW", X + 0.06, Y - 0.05, 0.0, false, 0, null, 0);

                Desenho.InserirListasMateriais(tipoArquivo);
                break;
              }
            }
          }
        }

        // A linha já avançou (Avançar): marca como desenhada a peça anterior.
        if (dadosDesenho.Count > 0) {
          dadosDesenho[dgv.SelectedIndex - 1].TemDesenho = "Sim";
          dgv.Items.Refresh();
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao criar novo desenho");
      }
    }

    #region Menu da grade

    private void Open3D() {
      try {
        if (Atual == null) return;

        string openFileName = "";
        int status = 0;
        int warnings = 0;
        openFileName = Atual.PathName;

        if (Atual.Tipo3D == "P") {
          Sw.App.OpenDoc6(openFileName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(openFileName, false, (int)errors);
        } else {
          Sw.App.OpenDoc6(openFileName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(openFileName, false, (int)errors);
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao abrir arquivo");
      }
    }

    private void TsmOpen3D_Click(object sender, RoutedEventArgs e) {
      Open3D();
    }

    private void TsmOpen2D_Click(object sender, RoutedEventArgs e) {
      try {
        if (Atual == null) return;

        string openFileName = "";
        int status = 0;
        int warnings = 0;

        if (Atual.Tipo3D == "P") {
          openFileName = Atual.PathName.Replace("SLDPRT", "SLDDRW");
          Sw.App.OpenDoc6(openFileName, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(openFileName, false, (int)errors);
        } else {
          openFileName = Atual.PathName.Replace("SLDASM", "SLDDRW");
          Sw.App.OpenDoc6(openFileName, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
          int errors = 0;
          Sw.App.ActivateDoc2(openFileName, false, (int)errors);
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao abrir desenho");
      }
    }

    private void TsmAttCodigo_Click(object sender, RoutedEventArgs e) {
      var atual = Atual;
      Open3D();

      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING || atual == null)
          return;

        var swModelDocExt = swModel.Extension;
        var swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");
        swCustPropMngr.Add3("CÓDIGO PROJETO", (int)swCustomInfoType_e.swCustomInfoText, atual.ShortName,
            (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        swModel.Save();
        Sw.App.CloseDoc(swModel.GetPathName());
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao atualizar código");
      }
    }

    #endregion

    #region Folhas

    private void BtnDownDes_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          var swDraw = (DrawingDoc)swModel;
          swDraw = (DrawingDoc)Sw.App.ActiveDoc;
          swDraw.SheetPrevious();
          AttachEventHandlers();
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao mudar Folha para anterior");
      }
    }

    private void BtnUpDes_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          var swDraw = (DrawingDoc)swModel;
          swDraw = (DrawingDoc)Sw.App.ActiveDoc;
          swDraw.SheetNext();
          AttachEventHandlers();
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao mudar folha para posterior");
      }
    }

    private void BtnRename_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        DrawingDoc swDraw;
        swDraw = (DrawingDoc)swModel;
        swDraw = (DrawingDoc)Sw.App.ActiveDoc;
        Sheet swSheet = swDraw.GetCurrentSheet();
        if (!string.IsNullOrEmpty(txtName.Text))
          swSheet.SetName(txtName.Text);

        swModel.Rebuild(0);
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Renomear Folha");
      }
    }

    public void AttachEventHandlers() {
      var swModel = (ModelDoc2)Sw.App.ActiveDoc;
      var swDraw = (DrawingDoc)swModel;
      Sheet swSheet = swDraw.GetCurrentSheet();
      AttachSWEvents(swDraw);
      bool status = swDraw.ActivateSheet(swSheet.GetName());
      txtName.Text = swSheet.GetName();
    }

    public void AttachSWEvents(DrawingDoc swDraw) {
      swDraw.ActivateSheetPostNotify += this.SwDraw_ActivateSheetPostNotify;
    }

    private int SwDraw_ActivateSheetPostNotify(string SheetName) {
      Dispatcher.Invoke(GetSheetName);
      return 1;
    }

    private void GetSheetName() {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        var swDraw = (DrawingDoc)swModel;
        swDraw = (DrawingDoc)Sw.App.ActiveDoc;
        Sheet swSheet = swDraw.GetCurrentSheet();
        txtName.Text = string.Empty;
        txtName.Text = swSheet.GetName();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Retornar Nome Folha");
      }
    }

    #endregion
  }
}
