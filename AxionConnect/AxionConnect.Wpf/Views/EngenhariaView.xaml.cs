using AxionConnect.Wpf.Models;
using AxionConnect.Wpf.Services;
using AxionConnect.Wpf.ViewModels;
using eDrawings.Interop.EModelViewControl;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LmCorbieUI.Wpf.Design;
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

    /// <summary>O PDF está na tela (o documento do eDrawings continua aberto por baixo).</summary>
    private bool _visualizandoPdf;

    /// <summary>Ferramenta do mouse ativa no eDrawings (zoom, girar, mover...).</summary>
    private EMVOperators _ferramenta = EMVOperators.eMVOperatorSelect;

    /// <summary>O produto exibido tem desenho / modelo no disco.</summary>
    private bool _tem2D, _tem3D;

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

      // O eDrawings e o PDF são janelas nativas e cobririam o assistente: ficam escondidos enquanto ele está na tela.
      DefinirAssistente(RoteiroAssistente);
      AssistenteIniciado += (s, e) => {
        _assistenteAtivo = true;
        AtualizarVisibilidadeViewer();
      };
      AssistenteEncerrado += (s, e) => {
        _assistenteAtivo = false;
        AtualizarVisibilidadeViewer();
      };
    }

    #region Assistente (botão "?")

    private bool _assistenteAtivo;

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private System.Collections.Generic.IEnumerable<Lm.LmPassoAssistente> RoteiroAssistente() {
      // Cada passo seleciona a aba onde está o controle destacado.
      Lm.LmPassoAssistente Passo(int aba, string titulo, string texto, FrameworkElement alvo) {
        var passo = Lm.LmPassoAssistente.Info(titulo, texto, alvo);
        passo.AoEntrar = () => Abas.SelectedIndex = aba;
        return passo;
      }

      yield return Lm.LmPassoAssistente.Info("Engenharia de Produto",
        "Esta tela busca no ERP a engenharia de um produto. Nela você confere o desenho de cada componente, define as operações de fabricação " +
        "e em qual operação cada item é consumido — e depois envia a engenharia de volta ao ERP.\n\n" +
        "Enquanto o assistente está aberto, o desenho fica escondido; ele volta ao sair.\n\n" +
        "Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Lm.LmPassoAssistente.Aguardar("1. Código da engenharia",
        "Digite o código da engenharia no ERP e tecle Enter (ou saia do campo). A leitura busca o produto e todos os componentes dele.\n\n" +
        "Regras: depois de carregar, o código fica bloqueado — use Nova Engenharia para trocar. " +
        "Se nenhum componente for encontrado, o campo é limpo para você digitar de novo.",
        () => _vm != null && _vm.Produtos.Count > 0, TxtCodigo);

      yield return Passo(0, "2. Componentes",
        "Cada linha é um componente da estrutura, com o nível, o nome, o código do produto, a descrição e a referência. " +
        "Os ícones indicam o 3D, o 2D, item fantasma e pendências. O campo acima da lista filtra por qualquer uma dessas informações.\n\n" +
        "Regras: ao selecionar uma linha, o desenho dela é aberto ao lado. " +
        "Depois de salvar, as linhas enviadas ficam verdes.",
        Grade);

      yield return Lm.LmPassoAssistente.Info("3. Dados do componente",
        "Mostra o código do produto (clique nele para copiar), o nome, a denominação e o peso. Em peças aparecem também a dimensão e o material.",
        CartaoInfo);

      var desenho = Lm.LmPassoAssistente.Info("4. Desenho",
        "Aqui aparece o desenho do componente selecionado.\n\n" +
        "Regras: abre o desenho 2D; se não existir, o modelo 3D; se nenhum dos dois existir, tenta o PDF. " +
        "Componente com nome terminado em \" - P3\" já abre na folha P3.\n\n" +
        "Os botões acima do desenho alternam entre 2D e 3D, abrem o PDF, trocam de folha, voltam à posição inicial e ativam zoom, girar e mover.\n\n" +
        "Regra do PDF: ele é procurado em uma pasta terminada em \"_PDF\", no mesmo diretório do desenho.",
        AreaDesenho, BarraVisualizador);
      yield return desenho;

      yield return Lm.LmPassoAssistente.Info("5. Pendências",
        "Aqui aparecem as pendências do componente selecionado: em vermelho as críticas, em amarelo as demais.\n\n" +
        "Regra: enquanto o componente a salvar, ou qualquer filho dele, tiver pendência, o Salvar é bloqueado. " +
        "A mais comum é \"sem operação\" — resolvida na aba Operações.",
        FaixaPendencias);

      yield return Passo(1, "6. Inserir operações",
        "Na aba Operações você monta o roteiro de fabricação do componente: escolha a operação, a máquina, o tempo (m:ss) e a quantidade de operadores, e clique em Inserir Processo.\n\n" +
        "Regras:\n" +
        "• Operação INTERNA lista só as máquinas cadastradas para ela e já seleciona a primeira. Operação EXTERNA não usa máquina.\n" +
        "• O par operação + máquina precisa existir no Cadastro de Processo.\n" +
        "• A mesma operação com a mesma máquina não pode ser inserida duas vezes.\n\n" +
        "Pelo teclado: digite o código (ou parte da descrição) e tecle Enter — cada Enter confirma o campo e passa ao próximo " +
        "(Operação → Máquina → Tempo → QTD), e o Enter em QTD insere o processo. " +
        "As setas ← e → no campo Operação passam para o componente anterior e o próximo.",
        FormOperacao);

      yield return Passo(1, "7. Produto fantasma",
        "Marque quando o componente não é fabricado como item próprio.\n\n" +
        "Regras: produto fantasma não exige operações — a pendência \"sem operação\" some. " +
        "Ao salvar, a engenharia dele vai marcada como fantasma e o item é inativado no ERP.",
        ChkFantasma);

      yield return Passo(1, "8. Operações do componente",
        "As operações inseridas aparecem aqui como cartões, na ordem em que serão executadas.\n\n" +
        "Regra: arraste um cartão para mudar a ordem — a sequência é renumerada sozinha. O botão vermelho remove a operação.",
        ListaOperacoes);

      yield return Passo(2, "9. Sequência operacional",
        "A aba Sequência Oper. define em qual operação cada item filho é CONSUMIDO. Há um grupo por operação do componente.\n\n" +
        "Regras: item sem sequência definida fica na primeira operação. Para mudar, arraste o item para outro grupo ou use o botão do item. " +
        "A aba só tem conteúdo depois que o componente tiver operações.",
        Abas);

      yield return Passo(3, "10. Engenharia",
        "A aba Engenharia mostra a estrutura do componente selecionado: ele e tudo o que está abaixo dele. É exatamente isso que será enviado ao salvar.",
        Abas);

      yield return Passo(0, "11. Salvar engenharia",
        "Envia ao ERP a engenharia do componente SELECIONADO e de todos os filhos dele. Para enviar a estrutura inteira, selecione a primeira linha.\n\n" +
        "Regras:\n" +
        "• Com filtro na lista de componentes o Salvar é bloqueado — limpe o filtro antes.\n" +
        "• Itens de biblioteca não podem ser o ponto de partida.\n" +
        "• Qualquer pendência no componente ou nos filhos bloqueia o envio.\n\n" +
        "O andamento aparece na tela e pode ser cancelado.",
        BtnSalvar);

      yield return Lm.LmPassoAssistente.Info("12. Nova engenharia",
        "Limpa a tela e libera o campo do código para carregar outra engenharia.\n\n" +
        "Regra: se já houver componentes carregados, o sistema pede confirmação — o que não foi salvo é perdido.",
        BtnNova);

      yield return Lm.LmPassoAssistente.Info("Pronto!",
        "O ciclo é: código → conferir o desenho → operações → sequência operacional → Salvar.\n\n" +
        "Para rever este passo a passo, clique no \"?\" no topo da tela.");
    }

    #endregion

    private void EngenhariaView_Loaded(object sender, RoutedEventArgs e) {
      if (_vm != null || DataContext is not EngenhariaViewModel vm)
        return;

      _vm = vm;
      _vm.PropertyChanged += Vm_PropertyChanged;
      _vm.FecharDesenho += FecharDocumento;
      _vm.FocarCodigo += () => TxtCodigo.Focus();

      TxtCodigo.Focus();
      _vm.Inicializar();
      VisualizadorPdf.PreCarregar();
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

    #region Digitação na aba Operações (mesmo fluxo do Axion Connect da TGM)

    /// <summary>
    /// Enter/Tab confirmam o texto digitado e seguem: Operação → Máquina → Tempo → QTD.
    /// Enter em QTD insere o processo e volta para a Operação.
    /// </summary>
    private void CampoEditavel_PreviewKeyDown(object sender, KeyEventArgs e) {
      if (_vm == null)
        return;

      // ← / → no campo Operação navegam entre os componentes (TxtOperacao_KeyDown).
      if (sender == CmbOperacao && (e.Key == Key.Left || e.Key == Key.Right)
          && Keyboard.Modifiers == ModifierKeys.None && !CmbOperacao.IsDropDownOpen) {
        e.Handled = true;
        var comando = e.Key == Key.Left ? _vm.AnteriorCommand : _vm.ProximoCommand;
        comando.Execute(null);

        // A troca de componente recarrega os campos: volta o foco para seguir navegando ou digitar.
        Dispatcher.BeginInvoke(() => {
          CmbOperacao.Focus();
          SelecionarTexto(CmbOperacao);
        }, System.Windows.Threading.DispatcherPriority.Input);
        return;
      }

      var enter = e.Key == Key.Enter;
      var tab = e.Key == Key.Tab && Keyboard.Modifiers == ModifierKeys.None;
      if (!enter && !tab)
        return;

      // Lista aberta: o Enter escolhe o item destacado (comportamento padrão do ComboBox)
      if (sender is ComboBox { IsDropDownOpen: true })
        return;

      if (sender == CmbOperacao) {
        e.Handled = true;
        if (!_vm.ConfirmarOperacao()) {
          SelecionarTexto(CmbOperacao);
          return;
        }
        if (CmbMaquina.IsEnabled)
          CmbMaquina.Focus();
        else
          TxtTempo.Focus();
      } else if (sender == CmbMaquina) {
        e.Handled = true;
        if (!_vm.ConfirmarMaquina()) {
          SelecionarTexto(CmbMaquina);
          return;
        }
        TxtTempo.Focus();
      } else if (sender == TxtTempo && enter) {
        e.Handled = true;
        TxtOperadores.Focus();
      } else if (sender == TxtOperadores && enter) {
        e.Handled = true;
        _vm.InserirOperacaoCommand.Execute(null);
        CmbOperacao.Focus();
      }
    }

    /// <summary>Saiu do campo com o mouse: confirma o que foi digitado.</summary>
    private void CampoEditavel_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
      var combo = (ComboBox)sender;
      if (_vm == null || combo.IsDropDownOpen || (e.NewFocus is DependencyObject novo && EhDescendente(combo, novo)))
        return;

      if (combo == CmbOperacao)
        _vm.ConfirmarOperacao();
      else if (combo == CmbMaquina)
        _vm.ConfirmarMaquina();
    }

    /// <summary>Ao entrar no campo, seleciona o texto para digitar por cima.</summary>
    private void CampoEditavel_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
      if (e.OriginalSource is TextBox caixa)
        Dispatcher.BeginInvoke(caixa.SelectAll, System.Windows.Threading.DispatcherPriority.Input);
    }

    private static void SelecionarTexto(ComboBox combo) =>
      (combo.Template.FindName("PART_EditableTextBox", combo) as TextBox)?.SelectAll();

    private static bool EhDescendente(DependencyObject pai, DependencyObject elemento) {
      while (elemento != null) {
        if (elemento == pai)
          return true;
        elemento = elemento is Visual ? VisualTreeHelper.GetParent(elemento) : LogicalTreeHelper.GetParent(elemento);
      }
      return false;
    }

    #endregion

    #region eDrawings

    private EModelViewControl Ctrl => Viewer.Controle;

    /// <summary>
    /// Mesmas regras do FrmEngenharia.AtualizarComponente: abre o desenho (e a folha "Pn" quando o nome
    /// termina em " - Pn"); sem desenho abre o modelo 3D; sem nenhum dos dois avisa.
    /// </summary>
    private async void ExibirProduto(ProdutoErp produto) {
      if (Ctrl == null)
        return;

      if (produto == null) {
        FecharDocumento();
        return;
      }

      try {
        var nomeDesenho = CaminhoDesenho(produto);
        _tem2D = File.Exists(nomeDesenho);
        _tem3D = File.Exists(produto.PathName);

        if (_tem2D) {
          AbrirDesenho2D(produto, nomeDesenho);
        } else if (_tem3D) {
          AbrirModelo3D(produto);
        } else {
          // Sem desenho nem modelo: mostra o PDF, se houver.
          FecharDocumento();
          if (!await AbrirPdf(produto, avisar: false) && _vm?.ProdutoSelecionado?.Produto == produto)
            Notificacao.Info($"Produto não encontrado:\r\n{produto.PathName}");
          return;
        }

        Ctrl.BackgroundColorOverride = true;
        Ctrl.BackgroundColor = 0xdfe4e9;
        Ctrl.ActivateInkMarkup(0);
        BarraVisualizador.Visibility = Visibility.Visible;

        // Navegando com o PDF na tela: segue no PDF do novo componente (o eDrawings já carregou por baixo).
        if (_visualizandoPdf)
          await AbrirPdf(produto, avisar: false);
      } catch (Exception ex) {
        Notificacao.Erro($"Erro ao Atualizar Dados\n\n{ex.Message}");
      }
    }

    private static string CaminhoDesenho(ProdutoErp produto) =>
      produto.PathName != null && produto.PathName.Length > 6
        ? produto.PathName.Substring(0, produto.PathName.Length - 6) + "SLDDRW"
        : string.Empty;

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

      BtnProximaFolha.Visibility = Ctrl.SheetCount > 1 ? Visibility.Visible : Visibility.Collapsed;
      BtnGirar.Visibility = Visibility.Collapsed;
      AtualizarBarra();
    }

    private void AbrirModelo3D(ProdutoErp produto) {
      AbrirDocumento(produto.PathName);
      _visualizando3D = true;

      BtnProximaFolha.Visibility = Visibility.Collapsed;
      BtnGirar.Visibility = Visibility.Visible;
      AtualizarBarra();
    }

    /// <summary>
    /// Busca e exibe o PDF do produto: fica em uma pasta terminada em "_PDF" no mesmo diretório do desenho.
    /// O PDF carregado fica no visualizador: voltar ao PDF do mesmo componente é imediato.
    /// Sem PDF, volta ao eDrawings.
    /// </summary>
    private async Task<bool> AbrirPdf(ProdutoErp produto, bool avisar) {
      // O desenho e o modelo ficam no mesmo diretório; o nome do PDF é o do arquivo ou o do componente.
      var arquivo = _tem2D ? CaminhoDesenho(produto) : produto.PathName;
      var nome = produto.Name;
      try {
        var caminho = await Task.Run(() => PdfService.Localizar(arquivo, Path.GetFileNameWithoutExtension(arquivo), nome));
        if (_vm?.ProdutoSelecionado?.Produto != produto)
          return false;

        if (caminho == null) {
          if (avisar) {
            var pastas = PdfService.Pastas(arquivo);
            Notificacao.Info(pastas.Count == 0
              ? $"Pasta \"{PdfService.SufixoPasta}\" não encontrada em:\n{Path.GetDirectoryName(arquivo)}"
              : $"PDF de \"{nome}\" não encontrado em:\n{string.Join("\n", pastas)}");
          }
          SairPdf();
          return false;
        }

        await VisualizadorPdf.Abrir(caminho);
        if (_vm?.ProdutoSelecionado?.Produto != produto)
          return false;
      } catch (Exception ex) {
        Log.Erro("Erro ao abrir o PDF", ex);
        Notificacao.Erro($"Erro ao abrir o PDF\n{ex.Message}");
        SairPdf();
        return false;
      }

      _visualizandoPdf = true;
      BarraVisualizador.Visibility = Visibility.Visible;
      AtualizarBarra();
      AtualizarVisibilidadeViewer();
      return true;
    }

    /// <summary>Volta ao eDrawings (o documento já está aberto: troca imediata).</summary>
    private void SairPdf() {
      if (!_visualizandoPdf)
        return;

      _visualizandoPdf = false;
      if (_documentoAberto == null)
        BarraVisualizador.Visibility = Visibility.Collapsed;
      AtualizarBarra();
      AtualizarVisibilidadeViewer();
    }

    /// <summary>Botões e texto da barra conforme o que está na tela (2D, 3D ou PDF).</summary>
    private void AtualizarBarra() {
      var temDocumento = _documentoAberto != null && Ctrl != null;

      BotoesEDrawings.Visibility = _visualizandoPdf ? Visibility.Collapsed : Visibility.Visible;
      // No eDrawings o botão leva à outra vista; com o PDF na tela, volta à vista que estava aberta.
      var destino3D = _visualizandoPdf ? _visualizando3D : !_visualizando3D;
      var montagem = _vm?.ProdutoSelecionado?.Produto?.TipoComponente == TipoComponente.Montagem;
      ImgAlternar.Source = !destino3D ? Icones.Desenho : montagem ? Icones.Montagem : Icones.Peca;
      BtnAlternar.ToolTip = (_visualizandoPdf ? "Voltar ao " : "Abrir ")
        + (!destino3D ? "desenho (2D)" : montagem ? "conjunto (3D)" : "peça (3D)");
      BtnAlternar.Visibility = temDocumento && (_visualizandoPdf || (_visualizando3D ? _tem2D : _tem3D))
        ? Visibility.Visible : Visibility.Collapsed;

      // Com o PDF na tela, o botão volta ao eDrawings; sem documento no eDrawings não há para onde voltar.
      BtnPdf.Visibility = _visualizandoPdf && !temDocumento ? Visibility.Collapsed : Visibility.Visible;
      BtnPdf.ToolTip = !_visualizandoPdf ? "Abrir PDF"
        : _visualizando3D ? "Voltar ao modelo (3D)" : "Voltar ao desenho (2D)";

      if (_visualizandoPdf)
        TxtFolha.Text = $"PDF · {Path.GetFileName(VisualizadorPdf.CaminhoAberto)}";
      else if (!temDocumento)
        TxtFolha.Text = string.Empty;
      else if (_visualizando3D)
        TxtFolha.Text = "Modelo 3D";
      else
        AtualizarTextoFolha();
    }

    private void AbrirDocumento(string caminho) {
      Ctrl.OpenDoc(caminho, false, false, false, "");
      _documentoAberto = caminho;

      // Cada documento começa no "Selecionar" (o padrão do eDrawings varia por documento)
      AtivarFerramenta(EMVOperators.eMVOperatorSelect);
      AtualizarVisibilidadeViewer();
    }

    private void FecharDocumento() {
      BarraVisualizador.Visibility = Visibility.Collapsed;
      _visualizandoPdf = false;
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
    /// O PDF ocupa a mesma área: com ele na tela, o eDrawings fica escondido (mas com o documento aberto).
    /// </summary>
    private void AtualizarVisibilidadeViewer() {
      // Com o assistente na tela vale o mesmo que no "processando": as janelas nativas saem da frente.
      var ocupado = _vm?.Ocupado == true || _assistenteAtivo;
      var visivel = Viewer.Indisponivel
        ? !ocupado && !_visualizandoPdf
        : Ctrl == null || (_documentoAberto != null && !ocupado && !_visualizandoPdf);
      Viewer.Visibility = visivel ? Visibility.Visible : Visibility.Hidden;
      VisualizadorPdf.Visibility = _visualizandoPdf && !ocupado ? Visibility.Visible : Visibility.Hidden;
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

      // Com o PDF na tela, volta à vista do eDrawings que estava aberta (o documento continua carregado).
      if (_visualizandoPdf) {
        SairPdf();
        return;
      }

      try {
        if (_visualizando3D)
          AbrirDesenho2D(produto, CaminhoDesenho(produto));
        else
          AbrirModelo3D(produto);
      } catch (Exception ex) {
        Notificacao.Erro($"Erro ao abrir no eDrawings\n{ex.Message}");
      }
    }

    private async void BtnPdf_Click(object sender, RoutedEventArgs e) {
      if (_visualizandoPdf) {
        SairPdf();
        return;
      }

      var produto = _vm.ProdutoSelecionado?.Produto;
      if (produto != null)
        await AbrirPdf(produto, avisar: true);
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

    private void BtnPosicaoInicial_Click(object sender, RoutedEventArgs e) {
      if (Ctrl != null)
        Ctrl.ViewOrientation = EMVViewOrientation.eMVOrientationHome;
    }

    private void BtnFerramenta_Click(object sender, RoutedEventArgs e) {
      if (Ctrl == null)
        return;

      var ferramenta = Enum.Parse<EMVOperators>((string)((FrameworkElement)sender).Tag);
      AtivarFerramenta(ferramenta == _ferramenta ? EMVOperators.eMVOperatorSelect : ferramenta);
    }

    /// <summary>Ferramenta do mouse no eDrawings; o botão da ferramenta ativa fica destacado.</summary>
    private void AtivarFerramenta(EMVOperators ferramenta) {
      try {
        Ctrl.ViewOperator = ferramenta;
        _ferramenta = ferramenta;
      } catch (Exception ex) {
        Log.Erro($"eDrawings: não foi possível ativar a ferramenta {ferramenta}", ex);
      }

      foreach (var botao in new[] { BtnZoomArea, BtnZoomContinuo, BtnGirar, BtnMover })
        botao.Estilo = (string)botao.Tag == _ferramenta.ToString() ? ButtonStyle.Information : ButtonStyle.Gray;
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
