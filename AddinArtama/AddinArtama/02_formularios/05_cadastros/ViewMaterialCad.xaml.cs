using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Windows;
using System.Windows.Input;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Cadastro de material (substitui o FrmMaterialCad).</summary>
  public partial class ViewMaterialCad : Wpf.LmView {
    materiais model = new materiais();

    public ViewMaterialCad() : this(0) { }

    public ViewMaterialCad(int idMaterial) {
      InitializeComponent();

      Loaded += (s, e) => {
        if (idMaterial > 0) {
          txtID.Text = idMaterial.ToString();
          CarregarMaterial();
        } else {
          Limpar();
        }
      };
    }

    private void Limpar() {
      txtID.IsReadOnly = false;
      txtID.Text = txtDescricao.Text = string.Empty;
      ckbSituacao.IsChecked = true;
      model = new materiais();
      txtDescricao.Focus();
    }

    private void BtnLimpar_Click(object sender, RoutedEventArgs e) {
      Limpar();
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      if (!LmValidacao.Validar(this)) return;

      try {
        model.descricao = txtDescricao.Text;
        model.ativo = ckbSituacao.IsChecked == true;

        if (materiais.Salvar(model)) {
          txtID.Text = model.id.ToString();
          txtID.IsReadOnly = true;
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Material");
      }
    }

    #region Código

    private void TxtID_BotaoF7Click(object sender, RoutedEventArgs e) {
      Consultar();
    }

    /// <summary>Enter carrega o código digitado (sem acionar o Salvar). F7 é tratado pelo próprio campo.</summary>
    private void TxtID_PreviewKeyDown(object sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
        e.Handled = true;
        CarregarMaterial();
      }
    }

    private void TxtID_LostFocus(object sender, RoutedEventArgs e) {
      CarregarMaterial();
    }

    private void Consultar() {
      // Consulta da LmCorbieUI (WinForms): o controle vazio só serve para ela não formatar colunas pela tela.
      var frm = new FrmConsultaGeral(new System.Windows.Forms.Control(), materiais.Selecionar(), "Consulta de materiais");
      if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK && int.TryParse(frm.valor[0], out _)) {
        txtID.Text = frm.valor[0];
        CarregarMaterial();
      }
    }

    private void CarregarMaterial() {
      if (string.IsNullOrEmpty(txtID.Text))
        return;

      try {
        int id = int.Parse(txtID.Text);

        if (model.id == id) return;
        model = materiais.Selecionar(id);

        if (model != null) {
          txtID.Text = model.id.ToString();
          txtDescricao.Text = model.descricao;
          ckbSituacao.IsChecked = model.ativo;

          txtID.IsReadOnly = true;
        } else model = new materiais();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Carregar Material");
      }
    }

    #endregion
  }
}
