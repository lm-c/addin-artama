using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Cadastro de matéria prima (chapas por espessura e material) — substitui o FrmMateriaPrimaCad.</summary>
  public partial class ViewMateriaPrimaCad : Wpf.LmView {
    materia_primas model = new materia_primas();

    public ViewMateriaPrimaCad() : this(0) { }

    public ViewMateriaPrimaCad(int idMateriaPrima) {
      InitializeComponent();

      CarregarComboMaterial();

      Loaded += (s, e) => {
        if (idMateriaPrima > 0) {
          txtID.Text = idMateriaPrima.ToString();
          CarregarMateriaPrima();
        } else {
          Limpar();
        }
      };
    }

    private void Limpar() {
      txtID.IsReadOnly = false;
      txtID.Text = txtDescricao.Text = txtEspessura.Text = txtCodigoChapa.Text = string.Empty;
      cmbMaterial.SelectedValue = null;
      cmbMaterial.Text = string.Empty;
      ckbSituacao.IsChecked = true;
      model = new materia_primas();
      txtCodigoChapa.Focus();
    }

    private void BtnLimpar_Click(object sender, RoutedEventArgs e) {
      Limpar();
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      if (!LmValidacao.Validar(this)) return;

      if (!(cmbMaterial.SelectedItem is materiais materialSelecionado)) {
        Wpf.Toast.Warning("Selecione um material da lista!");
        cmbMaterial.Focus();
        return;
      }

      try {
        model.codigo = Convert.ToInt32(txtCodigoChapa.Text);
        model.descricao = txtDescricao.Text;
        model.espessura = Convert.ToDouble(txtEspessura.Text);
        model.material_id = materialSelecionado.id;
        model.tipo_materia_prima = TipoMateriaPrima.Chapa;
        model.ativo = ckbSituacao.IsChecked == true;

        if (materia_primas.Salvar(model)) {
          txtID.Text = model.id.ToString();
          txtID.IsReadOnly = true;
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Matéria Prima");
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
        CarregarMateriaPrima();
      }
    }

    private void TxtID_LostFocus(object sender, RoutedEventArgs e) {
      CarregarMateriaPrima();
    }

    private void Consultar() {
      // Consulta da LmCorbieUI (WinForms): o controle vazio só serve para ela não formatar colunas pela tela.
      var frm = new FrmConsultaGeral(new System.Windows.Forms.Control(), materia_primas.Selecionar(), "Consulta de Matéria Prima");
      if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK && int.TryParse(frm.valor[0], out _)) {
        txtID.Text = frm.valor[0];
        CarregarMateriaPrima();
      }
    }

    private void CarregarMateriaPrima() {
      if (string.IsNullOrEmpty(txtID.Text))
        return;

      try {
        int id = int.Parse(txtID.Text);

        if (model.id == id) return;
        model = materia_primas.SelecionarMateriaPrima(id);

        if (model != null) {
          txtID.Text = model.id.ToString();
          txtCodigoChapa.Text = model.codigo.ToString();
          txtDescricao.Text = model.descricao;
          txtEspessura.Text = model.espessura?.ToString("0.0000");
          cmbMaterial.SelectedValue = model.material_id;
          ckbSituacao.IsChecked = model.ativo;

          txtID.IsReadOnly = true;
        } else model = new materia_primas();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Carregar Matéria Prima");
      }
    }

    #endregion

    private void CarregarComboMaterial() {
      try {
        cmbMaterial.ItemsSource = materiais.Selecionar(ativo: true).OrderBy(x => x.descricao).ToList();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar lista de Material");
      }
    }
  }
}
