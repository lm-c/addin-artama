using LmCorbieUI;
using LmCorbieUI.Metodos;
using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Aplica processos (operações) e matéria prima peça a peça (substitui o FrmMateriaPrimaApl).</summary>
  public partial class ViewMateriaPrimaApl : Wpf.LmView {
    /// <summary>Operação da lista (checkbox): marcar/desmarcar monta o texto de processos como no WinForms.</summary>
    public class OperacaoItem : INotifyPropertyChanged {
      bool marcado, visivel = true;

      public int Codigo { get; set; }
      public string Texto { get; set; }

      public bool Marcado {
        get => marcado;
        set {
          if (marcado == value) return;
          marcado = value;
          Notificar();
          MarcadoAlterado?.Invoke(this, EventArgs.Empty);
        }
      }

      public bool Visivel {
        get => visivel;
        set {
          if (visivel == value) return;
          visivel = value;
          Notificar();
        }
      }

      public event EventHandler MarcadoAlterado;
      public event PropertyChangedEventHandler PropertyChanged;

      void Notificar([CallerMemberName] string propriedade = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
    }

    static readonly Brush corDestaque = Brushes.Red;
    static readonly Brush corComponenteOk = new SolidColorBrush(Color.FromRgb(234, 238, 242));
    static readonly Brush corComponenteDiferente = new SolidColorBrush(Color.FromRgb(255, 118, 108));

    string _montageGeralNome = string.Empty;
    SortableBindingList<ProdutoErp> _produtos = new SortableBindingList<ProdutoErp>();

    /// <summary>Produtos sem operação (linha em vermelho na grade).</summary>
    readonly HashSet<ProdutoErp> pendentes = new HashSet<ProdutoErp>();

    readonly ObservableCollection<OperacaoItem> operacoes = new ObservableCollection<OperacaoItem>();

    /// <summary>Espera o usuário parar de digitar antes de filtrar as operações.</summary>
    readonly DispatcherTimer tmrInicioLocalizar = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(600) };

    public ViewMateriaPrimaApl() {
      InitializeComponent();

      lstOperacoes.ItemsSource = operacoes;
      tmrInicioLocalizar.Tick += (s, e) => IniciarLocalizar();

      CarregarControlesProcessos();
    }

    ProdutoErp Atual => dgv.SelectedItem as ProdutoErp;

    string Processos {
      get => lblProcess.Content as string ?? string.Empty;
      set => lblProcess.Content = value;
    }

    static void Destacar(Label label, bool destacar) {
      if (destacar)
        label.Foreground = corDestaque;
      else
        label.ClearValue(ForegroundProperty);
    }

    internal void CarregarControlesProcessos() {
      try {
        operacoes.Clear();
        Processos = string.Empty;

        foreach (var proc in ProcessoNaoSeriado.ListaProcessos) {
          var item = new OperacaoItem { Codigo = proc.Codigo, Texto = $"{proc.Codigo} - {proc.Descricao}" };
          item.MarcadoAlterado += Operacao_MarcadoAlterado;
          operacoes.Add(item);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao Carregar Controles Processos..\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private async void BtnCarrProcess_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
          _produtos = new SortableBindingList<ProdutoErp>();

          _montageGeralNome = Path.GetFileNameWithoutExtension(swModel.GetPathName());

          if (_montageGeralNome.Length > 50)
            _montageGeralNome = _montageGeralNome.Substring(0, 50);

          using (var worker = new LmBackgroundWorker()) {
            await worker.RunAsync("Aguarde, Iniciando leitura...", async (progress, token) => {
              _produtos = await ProcessoNaoSeriado.GetProdutos(progress, token);

              Wpf.Toast.Success("Concluído!");
            });
          }

          CarregarGrid();
        } else {
          Wpf.Toast.Warning("Comando apenas para Peças e Montagens");
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao Carregar Componentes..\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        if (!Wpf.Controls.LmValidacao.Validar(pnlDados))
          return;

        if (Atual == null) {
          Wpf.Toast.Info($"Nenhum produto selecionado");
          return;
        }

        var produtoERP = Atual;

        var ops = Processos;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        AdicionarDescricaoTodasConfiguracoes();

        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
          swCustPropMgr.Add3("Material", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Material\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        } else
          swCustPropMgr.Delete2("Material");

        if (produtoERP.TipoComponente == TipoComponente.Peca && produtoERP.ItemCorte != null) {
          produtoERP.ItemCorte.Operacao = ops;
          if (txtMaterial.SelectedValue != null) {
            var idMat = (int)txtMaterial.SelectedValue;
            var mat = materia_primas.Selecionar(idMat);
            if (mat != null) {
              produtoERP.ItemCorte.Codigo = mat.CodigoChapa;
              produtoERP.ItemCorte.Denominacao = mat.DescricaoChapa;
            }
          }
          ListaCorte.UpdateCutList(swModel, produtoERP.ItemCorte);
        } else {
          swCustPropMgr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText, ops, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        }

        var spl = ops.Split('/');
        produtoERP.Operacoes = new List<produto_erp_operacao>();
        foreach (var item in spl) {
          if (string.IsNullOrEmpty(item))
            continue;
          produtoERP.Operacoes.Add(new produto_erp_operacao() {
            processo_id = Convert.ToInt32(item),
          });
        }

        swModel.Save();

        if (produtoERP.Operacoes.Count() > 0) {
          produtoERP.Pendencias = new List<PendenciasEngenharia>();
          produtoERP.ImgPendencia = new System.Drawing.Bitmap(20, 20);
          pendentes.Remove(produtoERP);
        } else {
          ProdutoErp.AdicionarPendencia(produtoERP, PendenciasEngenharia.OperacaoNaoPossui);
          produtoERP.ImgPendencia = Properties.Resources.error;
          pendentes.Add(produtoERP);
        }
        AtualizarLinha(produtoERP);

        Destacar(lblMaterial, false);
        Destacar(lblCodMat, false);
        Destacar(lblDescMat, false);
        Destacar(lblProcess, false);
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao salvar..\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    #region Navegação

    private void BtnVoltar_Click(object sender, RoutedEventArgs e) {
      try {
        if (_produtos.Count == 0) {
          Wpf.Toast.Warning("Favor Carregar Componentes primeiro.");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (dgv.SelectedIndex > 0)
          SelecionarLinha(dgv.SelectedIndex - 1);
        else {
          Wpf.Toast.Info($"Você está no primeiro componente");
          SalvarFechar(swModel);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao voltar peça\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void BtnProximo_Click(object sender, RoutedEventArgs e) {
      try {
        if (_produtos.Count == 0) {
          Wpf.Toast.Warning("Favor Carregar Componentes primeiro.");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (dgv.SelectedIndex + 1 < dgv.Items.Count)
          SelecionarLinha(dgv.SelectedIndex + 1);
        else {
          Wpf.Toast.Info($"Você já chegou no último componente");
          SalvarFechar(swModel);
          SelecionarLinha(0);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao avançar peça\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void SelecionarLinha(int indice) {
      dgv.SelectedIndex = indice;
      dgv.ScrollIntoView(dgv.SelectedItem);
    }

    /// <summary>Trocar de linha salva/fecha o componente anterior e abre o novo.</summary>
    private void Dgv_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      try {
        if (Atual == null) return;

        var pos = dgv.SelectedIndex + 1;
        lblPecasProc.Content = $"Item {pos} de {dgv.Items.Count} - {pos * 100 / dgv.Items.Count}%";

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        SalvarFechar(swModel);

        AtualizarComponente();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao atualizar dados Componente");
      }
    }

    private void SalvarFechar(ModelDoc2 swModel) {
      if (swModel == null)
        return;

      var name = Path.GetFileNameWithoutExtension(swModel.GetPathName());
      if (name != _montageGeralNome) {
        swModel.ShowNamedView("*Isométrica");
        swModel.ViewZoomtofit();

        if (!swModel.IsOpenedReadOnly())
          swModel.Save();
        else Wpf.Toast.Warning($"Não salvo.\r\n{name} era somente leitura.");

        Sw.App.CloseDoc(swModel.GetPathName());
      }
    }

    #endregion

    #region Componente atual

    private void AtualizarComponente() {
      try {
        lblPeso.Content = "0,000Kg";
        lblEspess.Content = "";
        lblDescMat.Content = "";
        lblMaterial.Content = "";
        Processos = "";

        Destacar(lblMaterial, false);
        Destacar(lblDescMat, false);
        Destacar(lblProcess, false);

        txtMaterial.ItemsSource = new List<Z_Chapa>();
        txtMaterial.Text = string.Empty;
        txtMaterial.CampoObrigatorio = false;
        ClearCheckBox();

        var produtoERP = Atual;

        if (produtoERP.PathName.ToUpper().EndsWith(".SLDPRT"))
          Sw.App.OpenDoc6(produtoERP.PathName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
        else
          Sw.App.OpenDoc6(produtoERP.PathName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);

        var swModel = (ModelDoc2)Sw.App.ActivateDoc2(Name: produtoERP.PathName, Silent: false, Errors: 0);
        if (swModel == null)
          return;

        swModel.ClearSelection2(true);

        if (produtoERP.ItemCorte != null && produtoERP.ItemCorte.Tipo == TipoListaMaterial.Chapa)
          ListaCorte.RefreshCutList(swModel, "", produtoERP.ItemCorte);

        if (produtoERP.Referencia.StartsWith("Item da lista de corte")) {
          bool boolstatus = swModel.Extension.SelectByID2(produtoERP.ItemCorte.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);
        }

        AtualizarInformacoes(produtoERP);
        GetProcess(produtoERP);
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao Atualizar Dados\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void AtualizarInformacoes(ProdutoErp produtoErp) {
      txtComponente.Text = produtoErp.CodComponente;
      txtDescricao.Text = produtoErp.Denominacao;
      lblPeso.Content = produtoErp.PesoLiquido + " kg";
      if (produtoErp.ItemCorte != null || produtoErp.Referencia.StartsWith("Item da lista de corte")) {
        var espess = produtoErp.ItemCorte.Espessura;
        var largur = produtoErp.ItemCorte.Largura;
        var compri = produtoErp.ItemCorte.Comprimento;
        var descricMaterial = produtoErp.ItemCorte.Denominacao;
        var tipo = produtoErp.ItemCorte.Tipo;
        var codigo = produtoErp.ItemCorte.Codigo;

        lblPeso.Content = produtoErp.ItemCorte.Massa + " kg";

        lblEspess.Content = tipo == TipoListaMaterial.Chapa ? $"{espess}x{largur}x{compri}" : $"{compri}";
        lblDescMat.Content = produtoErp.ItemCorte.Denominacao;
        lblMaterial.Content = produtoErp.ItemCorte.Material;
        lblCodMat.Content = codigo.ToString();

        if (tipo == TipoListaMaterial.Chapa) {
          txtMaterial.CampoObrigatorio = true;
          Z_Chapa materialIdeal = null;

          var list = materia_primas.Selecionar(ativo: true, espessura: espess);
          txtMaterial.ItemsSource = list;

          if (list.Count > 0) {
            materialIdeal = list.FirstOrDefault(x => x.DescricaoChapa == produtoErp.ItemCorte.Material);
            if (materialIdeal != null) {
              txtMaterial.SelectedValue = materialIdeal.Id;
            } else {
              materialIdeal = list.FirstOrDefault(x => x.DescricaoChapa == descricMaterial);
              if (materialIdeal != null)
                txtMaterial.SelectedValue = materialIdeal.Id;
            }
          }

          var id = (int?)txtMaterial.SelectedValue;
          if (id == null || (materialIdeal != null && materialIdeal.CodigoChapa != (int?)codigo)) {
            Destacar(lblCodMat, true);
            Destacar(lblDescMat, true);
            Wpf.Toast.Warning("Atenção:\r\nMateria prima incorreta, selecione a matéria prima correta e clique em salvar.");
          }
        } else {
          txtMaterial.CampoObrigatorio = false;
        }

        if (produtoErp.Pendencias.Count > 0) {
          Wpf.Toast.Warning(produtoErp.Pendencias[0].ObterDescricaoEnum());
        }
      }
    }

    private void GetProcess(ProdutoErp produtoErp) {
      try {
        ClearCheckBox();
        foreach (var operacao in produtoErp.Operacoes) {
          operacoes.Where(x => x.Codigo == operacao.processo_id).ToList().ForEach(x => x.Marcado = true);
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao retornar Processos\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void TxtComponente_BotaoF7Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        if (Atual == null) {
          Wpf.Toast.Info($"Nenhum produto selecionado");
          return;
        }

        var produtoERP = Atual;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING)
          return;

        produtoERP.CodComponente = produtoERP.Name;
        txtComponente.Text = produtoERP.Name;
        txtComponente.Background = corComponenteOk;

        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
        swCustPropMgr.Add3("Componente", (int)swCustomInfoType_e.swCustomInfoText, txtComponente.Text, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao atualizar componente\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    /// <summary>Salvar Como (peça/montagem e o desenho junto).</summary>
    private async void TxtComponente_BotaoF8Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        if (Atual == null) {
          Wpf.Toast.Info($"Nenhum produto selecionado");
          return;
        }

        var produtoERP = Atual;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          Wpf.MsgBox.Show($"Comando apenas para Peças e Montagens", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
          return;
        }

        string partAtual;
        string drawingAtual;

        partAtual = swModel.GetPathName().ToUpper();

        var sfd = new SaveFileDialog {
          AddExtension = true,
          RestoreDirectory = true,
          Title = "Salvar Como",
          FileName = txtComponente.Text
        };

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
          sfd.Filter = "Peça (*.sldprt) | *.sldprt";
          drawingAtual = partAtual.Replace(".SLDPRT", ".SLDDRW");
        } else {
          sfd.Filter = "Montagem (*.sldasm) | *.sldasm";
          drawingAtual = partAtual.Replace(".SLDASM", ".SLDDRW");
        }

        if (sfd.ShowDialog() == true && !string.IsNullOrEmpty(sfd.FileName)) {
          string drawingNew;
          string partNew = sfd.FileName;

          drawingNew = partNew.Substring(0, partNew.Length - 6) + "SLDDRW";

          int status = 0;
          int errors = 0;
          int warnings = 0;

          swModel.Extension.RunCommand(2732, "");
          using (var worker = new LmBackgroundWorker()) {
            await worker.RunAsync("Salvando Aguarde....", async (progress, token) => {
              if (File.Exists(drawingAtual)) {
                Sw.App.OpenDoc6(drawingAtual, (int)swDocumentTypes_e.swDocDRAWING,
                    (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

                Sw.App.ActivateDoc2(drawingAtual, false, 0);

                Sw.App.ActivateDoc2(partAtual, false, 0);
                swModel = (ModelDoc2)Sw.App.ActiveDoc;

                swModel.SaveAs4(partNew, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref errors, ref warnings);

                Sw.App.ActivateDoc2(drawingAtual, false, 0);
                swModel = (ModelDoc2)Sw.App.ActiveDoc;
                swModel.SaveAs(drawingNew);

                Sw.App.CloseDoc(drawingNew);
              } else swModel.SaveAs4(partNew, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref errors, ref warnings);
              Wpf.Toast.Success("Concluído!");
            });
          }

          produtoERP.PathName = partNew;

          produtoERP.Name = Path.GetFileNameWithoutExtension(partNew);
          produtoERP.CodComponente = produtoERP.Name;

          txtComponente.Text = produtoERP.Name;
          txtComponente.Background = corComponenteOk;

          AtualizarLinha(produtoERP);

          if (Wpf.MsgBox.Show("Deseja Eliminar Antigo e Manter Somente Novo?", "Excluir",
              MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
            File.Delete(partAtual);
            if (File.Exists(drawingAtual))
              File.Delete(drawingAtual);
          }
        }
      } catch (Exception ex) {
        Wpf.MsgBox.Show($"Erro ao salvar cópia do componente\n\n{ex.Message}", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    /// <summary>Vermelho quando o código digitado não corresponde ao nome do arquivo.</summary>
    private void TxtComponente_TextChanged(object sender, TextChangedEventArgs e) {
      var produtoERP = Atual;
      if (produtoERP == null)
        return;

      txtComponente.Background = !produtoERP.Name.StartsWith(txtComponente.Text) ? corComponenteDiferente : corComponenteOk;
    }

    private void TxtDenominacao_LostFocus(object sender, RoutedEventArgs e) {
      var produtoERP = Atual;
      if (produtoERP != null)
        produtoERP.Denominacao = txtDescricao.Text;
    }

    private void AdicionarDescricaoTodasConfiguracoes() {
      try {
        if (Atual == null) {
          Wpf.Toast.Info($"Nenhum produto selecionado");
          return;
        }

        var produtoERP = Atual;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        var swModelDocExt = swModel.Extension;
        var swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");

        swCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, produtoERP.Denominacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
      } catch (Exception ex) {
        Wpf.MsgBox.Show("Falha ao Atualizar Denominação: \n" + ex.Message);
      }
    }

    #endregion

    #region Operações

    private void ClearCheckBox() {
      operacoes.Where(x => x.Marcado).ToList().ForEach(x => x.Marcado = false);
    }

    /// <summary>Mantém o texto de processos ("101/205") na ordem em que as operações foram marcadas.</summary>
    private void Operacao_MarcadoAlterado(object sender, EventArgs e) {
      if (Atual == null) {
        Wpf.Toast.Info($"Nenhum produto selecionado");
        return;
      }

      var item = (OperacaoItem)sender;
      string p = item.Codigo.ToString();

      if (item.Marcado) {
        Processos += !string.IsNullOrEmpty(Processos) ? $"/{p}" : p;
      } else {
        var texto = Processos.Replace(p, "").Replace("//", "/");

        if (texto.StartsWith("/"))
          texto = texto.Substring(1, texto.Length - 1);
        if (texto.EndsWith("/"))
          texto = texto.Substring(0, texto.Length - 1);

        Processos = texto;
      }
    }

    private void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e) {
      tmrInicioLocalizar.Stop();
      tmrInicioLocalizar.Start();
    }

    private void IniciarLocalizar() {
      tmrInicioLocalizar.Stop();

      try {
        var procurar = txtProcurar.Text.ToLower().RemoverCaracteresEspeciais();
        foreach (var x in operacoes)
          x.Visivel = x.Marcado || x.Texto.ToLower().RemoverCaracteresEspeciais().Contains(procurar);
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Filtrar Operações");
      }
    }

    #endregion

    #region Grade

    private void CarregarGrid() {
      pendentes.Clear();

      try {
        foreach (var produtoErp in _produtos) {
          if (produtoErp.Operacoes != null && produtoErp.Operacoes.Count == 0 && produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
            ProdutoErp.AdicionarPendencia(produtoErp, PendenciasEngenharia.OperacaoNaoPossui);
            pendentes.Add(produtoErp);
          }
        }
      } catch (Exception ex) {
        Wpf.Toast.Error("Erro ao formatar cores grid. \r\n" + ex.Message);
      }

      dgv.ItemsSource = null;
      dgv.ItemsSource = _produtos;

      if (_produtos.Count > 0)
        SelecionarLinha(0);
    }

    private void Dgv_LoadingRow(object sender, DataGridRowEventArgs e) {
      if (e.Row.Item is ProdutoErp produto && pendentes.Contains(produto))
        e.Row.Foreground = corDestaque;
      else
        e.Row.ClearValue(ForegroundProperty);
    }

    /// <summary>Redesenha a linha do produto (cor, ícone de pendência e nome) sem perder a seleção.</summary>
    private void AtualizarLinha(ProdutoErp produto) {
      if (dgv.ItemContainerGenerator.ContainerFromItem(produto) is DataGridRow linha) {
        if (pendentes.Contains(produto))
          linha.Foreground = corDestaque;
        else
          linha.ClearValue(ForegroundProperty);

        var contexto = linha.DataContext;
        linha.DataContext = null;
        linha.DataContext = contexto;
      }
    }

    #endregion
  }
}
