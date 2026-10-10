using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Navegação peça a peça para criar/detalhar desenhos e trocar formato de folha (substitui o FrmDesenho).</summary>
  public partial class ViewDesenho : Wpf.LmView {
    List<Desenho> dadosDesenho = new List<Desenho>();
    string montagemGeral = "";

    public ViewDesenho() {
      InitializeComponent();
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

        montagemGeral = swModel.GetPathName();
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao carregar componentes\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao avançar peça para desenho\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao inserir lista de soldagem\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao fechar documentos\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao criar novo desenho\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao abrir arquivo\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao abrir desenho\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao atualizar código\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao mudar Folha para anterior\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao mudar folha para posterior\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao Renomear Folha\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
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
        Wpf.MsgBox.Show($"Erro ao Retornar Nome Folha\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    #endregion

    private void LmView_AjudaClick(object sender, EventArgs e) {
      Process.Start("https://youtu.be/YTJtQPIa8wU");
    }
  }
}
