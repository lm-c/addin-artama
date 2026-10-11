using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Cadastro de processo (operação + máquina do ERP) — substitui o FrmProcessoCad.</summary>
  public partial class ViewProcessoCad : Wpf.LmView {
    processos model = new processos();

    public ViewProcessoCad() : this(0) { }

    public ViewProcessoCad(int idProcesso) {
      InitializeComponent();

      Loaded += async (s, e) => {
        await CarregarCombos();

        if (idProcesso > 0) {
          txtID.Text = idProcesso.ToString();
          CarregarProcesso();
        } else if (model.id == 0) {
          Limpar();
        }
      };

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private System.Collections.Generic.IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Cadastro de Processo",
        "Um processo é a ligação entre uma OPERAÇÃO e uma MÁQUINA do ERP. " +
        "São estes processos que aparecem para marcar na tela Aplicação de Processos/Matéria Prima e que vão para a engenharia do produto.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Info("1. Código",
        "Para um processo NOVO, deixe o código em branco: ele é gerado ao salvar.\n\n" +
        "Para ALTERAR um processo, digite o código e tecle Enter, ou use F7 para pesquisar.\n\n" +
        "Regra: depois de carregado, o código fica bloqueado. Use Limpar para trocar de processo.",
        txtID);

      yield return Wpf.LmPassoAssistente.Info("2. Operação",
        "Escolha a operação. É obrigatória e precisa ser um item da lista.\n\n" +
        "Regra: a lista vem do ERP no momento em que a tela abre (enquanto carrega, o campo mostra \"Carregando operações...\"). " +
        "O tipo da operação — interna ou externa — também vem do ERP.",
        cmbOperacao);

      yield return Wpf.LmPassoAssistente.Info("3. Máquina",
        "Escolha a máquina que executa a operação. A lista também vem do ERP.\n\n" +
        "Regra: operação INTERNA exige máquina. Operação EXTERNA não usa máquina — o campo é bloqueado e limpo sozinho.",
        cmbMaquina);

      yield return Wpf.LmPassoAssistente.Info("4. Situação",
        "Desmarque \"Ativo\" para desativar o processo sem apagar o cadastro.",
        pnlSituacao);

      yield return Wpf.LmPassoAssistente.Info("5. Salvar",
        "Grava o processo.\n\n" +
        "Regra: depois de salvar, a tela é limpa para o próximo cadastro e a lista de processos do sistema é recarregada — o processo novo já fica disponível nas outras telas.",
        btnSalvar);

      yield return Wpf.LmPassoAssistente.Info("6. Limpar e Excluir",
        "Limpar esvazia a tela para um novo cadastro (não apaga nada do banco).\n\n" +
        "Excluir remove o processo carregado, depois de pedir confirmação. Só fica disponível com um processo carregado.",
        btnLimpar, btnExcluir);
    }

    #endregion

    private void Limpar() {
      txtID.IsReadOnly = false;
      txtID.Text = string.Empty;
      cmbMaquina.SelectedValue = null;
      cmbOperacao.SelectedValue = null;
      btnExcluir.IsEnabled = false;

      ckbSituacao.IsChecked = true;
      model = new processos();

      cmbOperacao.Focus();
    }

    private void BtnLimpar_Click(object sender, RoutedEventArgs e) {
      Limpar();
    }

    private async void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      if (!LmValidacao.Validar(this)) return;

      if (!(cmbOperacao.SelectedItem is Api.Operacao operacao)) {
        Wpf.Toast.Warning("Selecione uma operação da lista!");
        cmbOperacao.Focus();
        return;
      }

      try {
        model.ativo = ckbSituacao.IsChecked == true;
        model.codigo_maquina = (cmbMaquina.SelectedItem as Api.Maquina)?.codMaquina;
        model.codigo_operacao = operacao.codOperacao;
        model.tipoOperacao = operacao.tipo == "Interno" ? TipoOperacao.Interna : TipoOperacao.Externa;

        if (processos.Salvar(model))
          Limpar();

        await Processo.Carregar();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Cadastrar Operação");
      }
    }

    private void BtnExcluir_Click(object sender, RoutedEventArgs e) {
      try {
        if (Wpf.MsgBox.Show("Deseja realmente excluir este registro?",
            "Excluir Registro", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
          processos.Excluir(model.id);
          Wpf.Toast.Info("Excluido com Sucesso!");
          Limpar();
          _ = Processo.Carregar();
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Excluir Processo");
      }
    }

    #region Código

    private void TxtID_BotaoF7Click(object sender, RoutedEventArgs e) {
      // Consulta da LmCorbieUI (WinForms): o controle vazio só serve para ela não formatar colunas pela tela.
      var frm = new FrmConsultaGeral(new System.Windows.Forms.Control(), Processo.ListaProcessos, "Consulta de Processos");
      if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK && int.TryParse(frm.valor[0], out _)) {
        txtID.Text = frm.valor[0];
        CarregarProcesso();
      }
    }

    /// <summary>Enter carrega o código digitado (sem acionar o Salvar). F7 é tratado pelo próprio campo.</summary>
    private void TxtID_PreviewKeyDown(object sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
        e.Handled = true;
        CarregarProcesso();
      }
    }

    private void TxtID_LostFocus(object sender, RoutedEventArgs e) {
      CarregarProcesso();
    }

    private void CarregarProcesso() {
      if (string.IsNullOrEmpty(txtID.Text))
        return;

      try {
        int id = int.Parse(txtID.Text);

        if (model.id == id) return;
        model = processos.SelecionarProcesso(id);

        if (model != null) {
          txtID.Text = model.id.ToString();
          ckbSituacao.IsChecked = model.ativo;
          cmbOperacao.SelectedValue = model.codigo_operacao;
          cmbMaquina.SelectedValue = model.codigo_maquina;

          btnExcluir.IsEnabled = true;

          txtID.IsReadOnly = true;
        } else model = new processos();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Carregar Processo");
      }
    }

    #endregion

    #region Operação / Máquina

    private async System.Threading.Tasks.Task CarregarCombos() {
      try {
        var maqs = await Api.GetMaquinasAsync();
        var ops = await Api.GetOpsAsync();

        maqs.ForEach(x => { x.descricao = $"{x.codMaquina} - {x.descricao} [{x.mascara}]"; });
        ops.ForEach(x => { x.descricao = $"{x.codOperacao} - {x.descricao} [{x.abreviatura}]"; });

        cmbMaquina.ItemsSource = maqs;
        cmbOperacao.ItemsSource = ops;
      } catch (Exception ex) {
        Wpf.Toast.Warning(ex.ToString());
      } finally {
        cmbMaquina.Watermark = cmbOperacao.Watermark = string.Empty;
      }
    }

    /// <summary>Operação externa não usa máquina; interna exige.</summary>
    private void CmbOperacao_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      try {
        var externa = (cmbOperacao.SelectedItem as Api.Operacao)?.tipo == "Externo";

        if (externa)
          cmbMaquina.SelectedValue = null;

        cmbMaquina.IsEnabled = !externa;
        cmbMaquina.CampoObrigatorio = !externa;
        lblMaquina.Content = externa ? "Máquina" : "Máquina *";
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao selecionar Máquinas");
      }
    }

    #endregion
  }
}
