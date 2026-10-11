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

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private System.Collections.Generic.IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Cadastro de Matéria Prima",
        "Aqui você cadastra as chapas: cada uma é a combinação de um material com uma espessura. " +
        "É desta lista que sai a matéria prima oferecida na tela Aplicação de Processos/Matéria Prima.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Info("1. Código",
        "Para uma matéria prima NOVA, deixe o código em branco: ele é gerado ao salvar.\n\n" +
        "Para ALTERAR, digite o código e tecle Enter, ou use F7 para pesquisar.\n\n" +
        "Regra: depois de carregado ou salvo, o código fica bloqueado. Use Limpar para trocar de registro.",
        txtID);

      yield return Wpf.LmPassoAssistente.Info("2. Código da chapa",
        "Código numérico da chapa. É obrigatório.\n\n" +
        "Regra: é este código que é gravado na peça quando a matéria prima é aplicada, e que é comparado para avisar \"matéria prima incorreta\".",
        txtCodigoChapa);

      yield return Wpf.LmPassoAssistente.Info("3. Espessura",
        "Espessura da chapa, com até 4 casas decimais. É obrigatória.\n\n" +
        "Regra: na aplicação de matéria prima, só são oferecidas as chapas com a MESMA espessura da peça.",
        txtEspessura);

      yield return Wpf.LmPassoAssistente.Info("4. Material",
        "Escolha o material da chapa. É obrigatório e precisa ser um item da lista.\n\n" +
        "Regra: a lista traz só os materiais ativos do Cadastro de Material.",
        cmbMaterial);

      yield return Wpf.LmPassoAssistente.Info("5. Descrição da chapa",
        "Descrição completa da chapa. É obrigatória.\n\n" +
        "Regra: na aplicação de matéria prima, o sistema escolhe a chapa sozinho quando esta descrição é igual à do material da peça.",
        txtDescricao);

      yield return Wpf.LmPassoAssistente.Info("6. Situação",
        "Desmarque \"Ativo\" para desativar a chapa sem apagar o cadastro.\n\n" +
        "Regra: chapas inativas não são oferecidas na aplicação de matéria prima.",
        pnlSituacao);

      yield return Wpf.LmPassoAssistente.Info("7. Salvar e Limpar",
        "Salvar grava a matéria prima. Limpar esvazia a tela para um novo cadastro (não apaga nada do banco).",
        btnSalvar, btnLimpar);
    }

    #endregion

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
        Msg.Erro(ex, "Erro ao Cadastrar Matéria Prima");
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
        Msg.Erro(ex, "Erro ao Carregar Matéria Prima");
      }
    }

    #endregion

    private void CarregarComboMaterial() {
      try {
        cmbMaterial.ItemsSource = materiais.Selecionar(ativo: true).OrderBy(x => x.descricao).ToList();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao carregar lista de Material");
      }
    }
  }
}
