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

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private System.Collections.Generic.IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Cadastro de Material",
        "Aqui você cadastra os materiais (o tipo de material em si). Eles são usados depois no Cadastro de Matéria Prima, onde cada chapa é ligada a um material.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Info("1. Código",
        "Para um material NOVO, deixe o código em branco: ele é gerado ao salvar.\n\n" +
        "Para ALTERAR um material, digite o código e tecle Enter, ou use F7 para pesquisar.\n\n" +
        "Regra: depois de carregado ou salvo, o código fica bloqueado. Use Limpar para trocar de material.",
        txtID);

      yield return Wpf.LmPassoAssistente.Info("2. Descrição",
        "Nome do material. É obrigatório.",
        txtDescricao);

      yield return Wpf.LmPassoAssistente.Info("3. Situação",
        "Desmarque \"Ativo\" para desativar o material sem apagar o cadastro.\n\n" +
        "Regra: só os materiais ativos aparecem na lista do Cadastro de Matéria Prima.",
        pnlSituacao);

      yield return Wpf.LmPassoAssistente.Info("4. Salvar e Limpar",
        "Salvar grava o material. Limpar esvazia a tela para um novo cadastro (não apaga nada do banco).",
        btnSalvar, btnLimpar);
    }

    #endregion

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
        Msg.Erro(ex, "Erro ao Cadastrar Material");
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
        Msg.Erro(ex, "Erro ao Carregar Material");
      }
    }

    #endregion
  }
}
