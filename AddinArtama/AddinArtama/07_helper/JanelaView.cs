using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>
  /// Abre uma tela do painel (<see cref="Wpf.LmView"/>) como janela modal, para quando outra tela
  /// precisa dela (ex.: cadastro de usuário abre o cadastro de perfil). Fechar a tela fecha a janela.
  /// </summary>
  internal static class JanelaView {
    public static void Mostrar(Wpf.LmView view, double largura = 420, double altura = 560) {
      // A janela já tem título e botão fechar: o cabeçalho da tela ficaria duplicado.
      view.MostrarCabecalho = false;

      var janela = new Wpf.LmWindow {
        Title = view.Titulo,
        Width = largura,
        Height = altura,
        ShowInTaskbar = false,
        MostrarMinimizar = false,
        Content = view,
      };

      view.Fechado += (s, e) => janela.Close();

      janela.ShowDialog();
    }
  }
}
