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
    }

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
