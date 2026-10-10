using AxionConnect.Wpf.Models;
using AxionConnect.Wpf.Services;
using AxionConnect.Wpf.ViewModels;
using eDrawings.Interop.EModelViewControl;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Lm = LmCorbieUI.Wpf;

namespace AxionConnect.Wpf.Views {
  /// <summary>
  /// A lógica da tela está no EngenhariaViewModel. Aqui fica só o que é da View:
  /// o controle ActiveX do eDrawings, o arrastar e soltar e os atalhos de teclado.
  /// </summary>
  public partial class EngenhariaView : Lm.LmView {
    private EngenhariaViewModel _vm;
    private bool _visualizando3D;
    private string _documentoAberto;
    private string _codigoAoEntrar;

    private Point _inicioArraste;
    private object _itemArraste;

    public EngenhariaView() {
      InitializeComponent();

      ImgProximaFolha.Source = Icones.Proximo;

      Viewer.Carregado += () => {
        AtualizarVisibilidadeViewer();
        ExibirProduto(_vm?.ProdutoSelecionado?.Produto);
      };
      Viewer.IndisponivelAlterado += AtualizarVisibilidadeViewer;
      Loaded += EngenhariaView_Loaded;
    }

    private void EngenhariaView_Loaded(object sender, RoutedEventArgs e) {
      if (_vm != null || DataContext is not EngenhariaViewModel vm)
        return;

      _vm = vm;
      _vm.PropertyChanged += Vm_PropertyChanged;
      _vm.FecharDesenho += FecharDocumento;
      _vm.FocarCodigo += () => TxtCodigo.Focus();

      TxtCodigo.Focus();
      _vm.Inicializar();
    }

    private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e) {
      if (e.PropertyName == nameof(EngenhariaViewModel.ProdutoSelecionado))
        ExibirProduto(_vm.ProdutoSelecionado?.Produto);
      else if (e.PropertyName == nameof(EngenhariaViewModel.Ocupado))
        AtualizarVisibilidadeViewer();
    }

    #region Código da engenharia

    private void TxtCodigo_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
      _codigoAoEntrar = _vm?.CodigoEngenharia;
    }

    /// <summary>Saiu do código com outro valor: carrega a engenharia (TxtCodEngenharia_Leave).</summary>
    private async void TxtCodigo_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
      if (_vm != null)
        await _vm.CarregarSeAlteradoAsync(_codigoAoEntrar);
    }

    private async void TxtCodigo_PreviewKeyDown(object sender, KeyEventArgs e) {
      if (e.Key != Key.Enter || _vm == null)
        return;

      e.Handled = true;
      var anterior = _codigoAoEntrar;
      _codigoAoEntrar = _vm.CodigoEngenharia;
      await _vm.CarregarSeAlteradoAsync(anterior);
    }

    #endregion

    private void Grade_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (Grade.SelectedItem != null)
        Grade.ScrollIntoView(Grade.SelectedItem);
    }

    /// <summary>← / → no campo Operação navegam entre os componentes (TxtOperacao_KeyDown).</summary>
    private void CmbOperacao_PreviewKeyDown(object sender, KeyEventArgs e) {
      if ((e.Key != Key.Left && e.Key != Key.Right) || CmbOperacao.IsDropDownOpen || Keyboard.Modifiers != ModifierKeys.None)
        return;

      e.Handled = true;
      var comando = e.Key == Key.Left ? _vm.AnteriorCommand : _vm.ProximoCommand;
      comando.Execute(null);

      // A troca de componente recarrega os campos: volta o foco para seguir navegando.
      Dispatcher.BeginInvoke(() => CmbOperacao.Focus(), System.Windows.Threading.DispatcherPriority.Input);
    }

    #region eDrawings

    private EModelViewControl Ctrl => Viewer.Controle;

    /// <summary>
    /// Mesmas regras do FrmEngenharia.AtualizarComponente: abre o desenho (e a folha "Pn" quando o nome
    /// termina em " - Pn"); sem desenho abre o modelo 3D; sem nenhum dos dois avisa.
    /// </summary>
    private void ExibirProduto(ProdutoErp produto) {
      if (Ctrl == null)
        return;

      if (produto == null) {
        FecharDocumento();
        return;
      }

      try {
        var nomeDesenho = produto.PathName.Length > 6
          ? produto.PathName.Substring(0, produto.PathName.Length - 6) + "SLDDRW"
          : string.Empty;

        if (File.Exists(nomeDesenho)) {
          AbrirDesenho2D(produto, nomeDesenho);
        } else if (File.Exists(produto.PathName)) {
          AbrirModelo3D(produto);
        } else {
          FecharDocumento();
          Notificacao.Info($"Produto não encontrado:\r\n{produto.PathName}");
          return;
        }

        Ctrl.BackgroundColorOverride = true;
        Ctrl.BackgroundColor = 0xdfe4e9;
        Ctrl.ActivateInkMarkup(0);
        BarraVisualizador.Visibility = Visibility.Visible;
      } catch (Exception ex) {
        Notificacao.Erro($"Erro ao Atualizar Dados\n\n{ex.Message}");
      }
    }

    private void AbrirDesenho2D(ProdutoErp produto, string nomeDesenho) {
      AbrirDocumento(nomeDesenho);
      _visualizando3D = false;

      var match = Regex.Match(produto.Name ?? string.Empty, @" - P(\d+)$");
      if (match.Success) {
        string paginaProcurada = $"P{match.Groups[1].Value}";

        try {
          for (int i = 0; i < Ctrl.SheetCount; i++) {
            if (Ctrl.SheetName[i].Equals(paginaProcurada, StringComparison.OrdinalIgnoreCase)) {
              Ctrl.ShowSheet(i);
              break;
            }
          }
        } catch (Exception ex) {
          Log.Erro($"eDrawings: erro ao navegar para a folha {paginaProcurada}", ex);
        }
      } else {
        Ctrl.ShowSheet(0);
      }

      AtualizarBarra(produto);
    }

    private void AbrirModelo3D(ProdutoErp produto) {
      AbrirDocumento(produto.PathName);
      _visualizando3D = true;
      AtualizarBarra(produto);
    }

    /// <summary>Botões da barra: alternar 2D/3D (ptbDesenho), próxima folha (ptbProximoDesenho) e zoom (ptbZoom).</summary>
    private void AtualizarBarra(ProdutoErp produto) {
      var montagem = produto.TipoComponente == TipoComponente.Montagem;

      if (_visualizando3D) {
        ImgAlternar.Source = Icones.Desenho;
        BtnAlternar.ToolTip = "Abrir Desenho";
        BtnProximaFolha.Visibility = Visibility.Collapsed;
        ImgZoom.Source = Icones.Isometrico;
        TxtFolha.Text = "Modelo 3D";
      } else {
        ImgAlternar.Source = montagem ? Icones.Montagem : Icones.Peca;
        BtnAlternar.ToolTip = montagem ? "Abrir Conjunto" : "Abrir Peça";
        BtnProximaFolha.Visibility = Ctrl.SheetCount > 1 ? Visibility.Visible : Visibility.Collapsed;
        ImgZoom.Source = Icones.ZoomAjustar;
        AtualizarTextoFolha();
      }
    }

    private void AbrirDocumento(string caminho) {
      Ctrl.OpenDoc(caminho, false, false, false, "");
      _documentoAberto = caminho;
      AtualizarVisibilidadeViewer();
    }

    private void FecharDocumento() {
      BarraVisualizador.Visibility = Visibility.Collapsed;
      if (Ctrl != null && _documentoAberto != null) {
        try {
          Ctrl.CloseActiveDoc(_documentoAberto);
        } catch (Exception) {
          // documento já fechado
        }
      }
      _documentoAberto = null;
      AtualizarVisibilidadeViewer();
    }

    /// <summary>
    /// O eDrawings é uma janela nativa: sem documento aberto ele não se redesenha. Por isso só fica visível com
    /// um desenho aberto (e fora do "processando", que ele cobriria); até o ActiveX terminar de carregar
    /// (Ctrl == null) ele precisa ficar visível para ser criado.
    /// </summary>
    private void AtualizarVisibilidadeViewer() {
      var ocupado = _vm?.Ocupado == true;
      var visivel = Viewer.Indisponivel
        ? !ocupado
        : Ctrl == null || (_documentoAberto != null && !ocupado);
      Viewer.Visibility = visivel ? Visibility.Visible : Visibility.Hidden;
    }

    private void AtualizarTextoFolha() {
      TxtFolha.Text = Ctrl.SheetCount > 1
        ? $"Folha {Ctrl.SheetName[Ctrl.CurrentSheetIndex]} ({Ctrl.CurrentSheetIndex + 1} de {Ctrl.SheetCount})"
        : "Desenho 2D";
    }

    /// <summary>PnlDesenho_Click: alterna entre o desenho e o modelo 3D.</summary>
    private void BtnAlternar_Click(object sender, RoutedEventArgs e) {
      var produto = _vm.ProdutoSelecionado?.Produto;
      if (produto == null || Ctrl == null)
        return;

      if (_visualizando3D) {
        var nomeDesenho = produto.PathName.Substring(0, produto.PathName.Length - 6) + "SLDDRW";
        if (File.Exists(nomeDesenho)) {
          AbrirDocumento(nomeDesenho);
        }
        _visualizando3D = false;
      } else {
        AbrirDocumento(produto.PathName);
        _visualizando3D = true;
      }

      AtualizarBarra(produto);
    }

    /// <summary>PnlProximoDesenho_Click: próxima folha (da última volta para a primeira).</summary>
    private void BtnProximaFolha_Click(object sender, RoutedEventArgs e) {
      if (Ctrl == null || Ctrl.SheetCount == 0)
        return;

      int proxima = Ctrl.CurrentSheetIndex + 1;
      if (proxima >= Ctrl.SheetCount)
        proxima = 0; // Volta para primeira

      Ctrl.ShowSheet(proxima);
      AtualizarTextoFolha();
    }

    /// <summary>PnlRefresh_Click: no 3D volta para a isométrica; sempre ajusta o zoom.</summary>
    private void BtnZoom_Click(object sender, RoutedEventArgs e) {
      if (Ctrl == null)
        return;

      if (_visualizando3D)
        Ctrl.ViewOrientation = EMVViewOrientation.eMVOrientationIsoMetric;
      Ctrl.ViewOrientation = EMVViewOrientation.eMVOrientationZoomToFit;
    }

    #endregion

    #region Arrastar e soltar

    private void Arrastavel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
      _inicioArraste = e.GetPosition(null);
      _itemArraste = (e.OriginalSource as FrameworkElement)?.DataContext;

      // Não arrasta quando o clique é num botão (remover / mover)
      if (EhDentroDeBotao(e.OriginalSource as DependencyObject))
        _itemArraste = null;
    }

    private bool IniciouArraste(MouseEventArgs e) {
      if (e.LeftButton != MouseButtonState.Pressed || _itemArraste == null)
        return false;

      var delta = _inicioArraste - e.GetPosition(null);
      return Math.Abs(delta.X) > SystemParameters.MinimumHorizontalDragDistance
          || Math.Abs(delta.Y) > SystemParameters.MinimumVerticalDragDistance;
    }

    private void ListaOperacoes_PreviewMouseMove(object sender, MouseEventArgs e) {
      if (IniciouArraste(e) && _itemArraste is CartaoOperacao cartao) {
        _itemArraste = null;
        DragDrop.DoDragDrop(ListaOperacoes, cartao, DragDropEffects.Move);
      }
    }

    private void ListaOperacoes_Drop(object sender, DragEventArgs e) {
      if (e.Data.GetData(typeof(CartaoOperacao)) is not CartaoOperacao origem)
        return;

      var destino = (e.OriginalSource as FrameworkElement)?.DataContext as CartaoOperacao;
      var indice = destino != null ? _vm.Cartoes.IndexOf(destino) : _vm.Cartoes.Count - 1;
      _vm.MoverOperacao(origem, indice);
    }

    private void ProdutoEtapa_PreviewMouseMove(object sender, MouseEventArgs e) {
      if (IniciouArraste(e) && _itemArraste is ProdutoErp produto) {
        _itemArraste = null;
        DragDrop.DoDragDrop((DependencyObject)sender, produto, DragDropEffects.Move);
      }
    }

    private void Etapa_Drop(object sender, DragEventArgs e) {
      if (e.Data.GetData(typeof(ProdutoErp)) is ProdutoErp produto && ((FrameworkElement)sender).DataContext is EtapaConsumo etapa)
        _vm.MoverProdutoParaEtapa(produto, etapa, noInicio: true);
      e.Handled = true;
    }

    /// <summary>Botão "mover" do item: menu com as outras operações (antigo FrmSeqSel).</summary>
    private void BtnMoverEtapa_Click(object sender, RoutedEventArgs e) {
      var botao = (Button)sender;
      if (botao.DataContext is not ProdutoErp produto)
        return;

      // Mesmo visual dos submenus do LmPainel
      var menu = new ContextMenu { PlacementTarget = botao };
      menu.SetResourceReference(StyleProperty, Lm.LmPainel.EstiloSubmenuKey);
      foreach (var etapa in _vm.Etapas.Where(x => !x.Produtos.Contains(produto))) {
        var item = new MenuItem { Header = etapa.Titulo };
        item.SetResourceReference(StyleProperty, Lm.LmPainel.EstiloItemSubmenuKey);
        item.Click += (s, a) => _vm.MoverProdutoParaEtapa(produto, etapa, noInicio: false);
        menu.Items.Add(item);
      }
      menu.IsOpen = true;
    }

    private static bool EhDentroDeBotao(DependencyObject elemento) {
      while (elemento != null) {
        if (elemento is Button)
          return true;
        elemento = elemento is Visual ? VisualTreeHelper.GetParent(elemento) : LogicalTreeHelper.GetParent(elemento);
      }
      return false;
    }

    #endregion
  }
}
