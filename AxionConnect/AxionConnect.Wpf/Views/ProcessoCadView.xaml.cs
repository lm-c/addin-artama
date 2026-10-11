using AxionConnect.Wpf.ViewModels;
using LmCorbieUI.Wpf.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Lm = LmCorbieUI.Wpf;

namespace AxionConnect.Wpf.Views {
  /// <summary>
  /// A lógica da tela está no ProcessoCadViewModel. Aqui fica só o que é da View:
  /// foco, Enter no código e a marcação dos campos obrigatórios.
  /// </summary>
  public partial class ProcessoCadView : Lm.LmView {
    private ProcessoCadViewModel _vm;

    public ProcessoCadView() {
      InitializeComponent();

      // A tela fica oculta até ser escolhida no menu: só então busca as operações e máquinas no ERP.
      IsVisibleChanged += ProcessoCadView_IsVisibleChanged;

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private System.Collections.Generic.IEnumerable<Lm.LmPassoAssistente> RoteiroAssistente() {
      yield return Lm.LmPassoAssistente.Info("Cadastro de Processo",
        "Um processo é a ligação entre uma OPERAÇÃO e uma MÁQUINA do ERP. " +
        "São estes processos que ficam disponíveis na aba Operações da tela Engenharia de Produto.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Lm.LmPassoAssistente.Info("1. Processos cadastrados",
        "A lista mostra todos os processos, com a operação, a máquina, o tipo (interna ou externa) e a situação.\n\n" +
        "Regra: clique em uma linha para carregar o processo no formulário ao lado e alterá-lo.",
        GradeProcessos);

      yield return Lm.LmPassoAssistente.Info("2. Procurar",
        "Filtra a lista pelo código do processo ou por parte do nome da operação ou da máquina.",
        TxtFiltro);

      yield return Lm.LmPassoAssistente.Info("3. Código",
        "Para um processo NOVO, deixe o código em branco: ele é gerado ao salvar.\n\n" +
        "Para ALTERAR, digite o código e tecle Enter, ou selecione o processo na lista.\n\n" +
        "Regra: depois de carregado, o código fica bloqueado. Use Limpar para trocar de processo.",
        TxtCodigo);

      yield return Lm.LmPassoAssistente.Info("4. Operação",
        "Escolha a operação. É obrigatória e precisa ser um item da lista.\n\n" +
        "Regra: a lista vem do ERP quando a tela é aberta pela primeira vez. O tipo da operação — interna ou externa — também vem do ERP.",
        CmbOperacao);

      yield return Lm.LmPassoAssistente.Info("5. Máquina",
        "Escolha a máquina que executa a operação. A lista também vem do ERP.\n\n" +
        "Regra: operação INTERNA exige máquina. Operação EXTERNA não usa máquina — o campo é bloqueado e limpo sozinho.",
        CmbMaquina);

      yield return Lm.LmPassoAssistente.Info("6. Situação",
        "Desmarque \"Ativo\" para desativar o processo sem apagar o cadastro.",
        PnlSituacao);

      yield return Lm.LmPassoAssistente.Info("7. Salvar",
        "Grava o processo.\n\n" +
        "Regras: não é possível cadastrar duas vezes a mesma operação com a mesma máquina. " +
        "Depois de salvar, o formulário é limpo para o próximo cadastro, e a lista ao lado e a tela de Engenharia já passam a enxergar o processo novo.",
        BtnSalvar);

      yield return Lm.LmPassoAssistente.Info("8. Limpar e Excluir",
        "Limpar esvazia o formulário para um novo cadastro (não apaga nada do banco).\n\n" +
        "Excluir remove o processo carregado, depois de pedir confirmação. Só fica disponível com um processo carregado.",
        BtnLimpar, BtnExcluir);
    }

    #endregion

    private async void ProcessoCadView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
      if (!IsVisible || DataContext is not ProcessoCadViewModel vm)
        return;

      if (_vm == null) {
        _vm = vm;
        _vm.PropertyChanged += Vm_PropertyChanged;
        _vm.FocarOperacao += () => CmbOperacao.Focus();
      }

      await _vm.InicializarAsync();
    }

    /// <summary>Operação externa não usa máquina: o campo deixa de ser obrigatório.</summary>
    private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e) {
      if (e.PropertyName != nameof(ProcessoCadViewModel.MaquinaObrigatoria))
        return;

      CmbMaquina.CampoObrigatorio = _vm.MaquinaObrigatoria;
      MaquinaAsterisco.Text = _vm.MaquinaObrigatoria ? "*" : string.Empty;
    }

    private async void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      if (_vm == null || !LmValidacao.Validar(Formulario))
        return;

      await _vm.SalvarAsync();
    }

    #region Código

    /// <summary>Enter carrega o código digitado.</summary>
    private void TxtCodigo_PreviewKeyDown(object sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
        e.Handled = true;
        _vm?.CarregarProcesso();
      }
    }

    private void TxtCodigo_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
      _vm?.CarregarProcesso();
    }

    #endregion
  }
}
