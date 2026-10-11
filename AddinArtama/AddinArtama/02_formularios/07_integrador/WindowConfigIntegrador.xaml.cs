using LmCorbieUI.Wpf.Controls;
using System;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Configuração da API do ERP e dos dados padrão do produto (substitui o FrmConfigIntegrador).</summary>
  public partial class WindowConfigIntegrador : Wpf.LmWindow {
    configuracao_api model = new configuracao_api();

    public WindowConfigIntegrador() {
      InitializeComponent();

      Loaded += (s, e) => Carregar();
    }

    private void Carregar() {
      try {
        model = configuracao_api.Selecionar();

        if (model == null) {
          model = new configuracao_api();
          return;
        }

        txtCodEmpresa.Text = model.codigoEmpresa.ToString();
        txtEndereco.Text = model.endereco;
        txtToken.Text = model.token;
        txtEDrawings.Text = model.chave_edrawings;

        txtMascaraPeca.Text = model.mascara_peca;
        txtMascaraConjunto.Text = model.mascara_conjunto;

        txtClassificacaoFiscal.Text = model.classificacaoFiscal.ToString();
        txtFinalidade.Text = model.classificacaoFinalidade.ToString();
        txtOrigem.Text = model.classificacaoOrigem.ToString();
        txtIpiPerc.Text = model.perIPI.ToString();
        txtControleSaida.Text = model.tipoControleSaida.ToString();
        txtContaContabil.Text = model.codContaContabil;
        txtLocalizEntrada.Text = model.localizacaoEntrada.ToString();
        txtLocalizSaida.Text = model.localizacaoSaida.ToString();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Carregar Dados");
      }
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      if (!LmValidacao.Validar(this)) return;

      try {
        model.codigoEmpresa = Inteiro(txtCodEmpresa.Text);
        model.endereco = txtEndereco.Text;
        model.token = txtToken.Text;
        model.chave_edrawings = txtEDrawings.Text;

        model.mascara_peca = txtMascaraPeca.Text;
        model.mascara_conjunto = txtMascaraConjunto.Text;

        model.classificacaoFiscal = Inteiro(txtClassificacaoFiscal.Text);
        model.classificacaoFinalidade = Inteiro(txtFinalidade.Text);
        model.classificacaoOrigem = Inteiro(txtOrigem.Text);
        model.perIPI = string.IsNullOrWhiteSpace(txtIpiPerc.Text) ? 0 : Convert.ToDouble(txtIpiPerc.Text);
        model.tipoControleSaida = Inteiro(txtControleSaida.Text);
        model.codContaContabil = txtContaContabil.Text;
        model.localizacaoEntrada = Inteiro(txtLocalizEntrada.Text);
        model.localizacaoSaida = Inteiro(txtLocalizSaida.Text);

        configuracao_api.Salvar(model);

        Api.token = model.token;
        Api.chave_edrawings = model.chave_edrawings;
        Api.url = model.endereco;
        Api.codigoEmpresa = model.codigoEmpresa;

        Wpf.MsgBox.Show("Alterado com Sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
      } catch (Exception ex) {
        LmCorbieUI.LmException.ShowException(ex, "Erro ao Salvar Configuração");
      }
    }

    static int Inteiro(string texto) => string.IsNullOrWhiteSpace(texto) ? 0 : Convert.ToInt32(texto);
  }
}
