using AxionConnect.Wpf.Services;
using AxionConnect.Wpf.ViewModels;
using LmCorbieUI.Wpf.Design;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Lm = LmCorbieUI.Wpf;

namespace AxionConnect.Wpf.Views {
  public partial class MainWindow : Lm.LmWindow {
    private bool _logout;

    public MainWindow() {
      // A cor do tema já foi aplicada em Sessao.Iniciar
      InitializeComponent();
      DataContext = new MainViewModel();
    }

    /// <summary>FrmPrincipal_FormClosing: salva as predefinições; fechar sem logout encerra o sistema.</summary>
    protected override void OnClosing(CancelEventArgs e) {
      base.OnClosing(e);
      if (e.Cancel)
        return;

      configuracao_sistema.Salvar();

      if (!_logout)
        Sessao.Encerrar();
    }

    private void BtnSair_Click(object sender, RoutedEventArgs e) {
      _logout = true;
      Sessao.Logout(this);
    }

    /// <summary>Menu de cores do tema: opções prontas, a atual marcada, e uma cor personalizada.</summary>
    private void BtnAparencia_Click(object sender, RoutedEventArgs e) {
      var menu = NovoMenu(BtnAparencia);

      var atual = LmTema.CorPrimaria;
      foreach (var opcao in Aparencia.Opcoes)
        AdicionarItem(menu, CabecalhoCor(opcao.Nome, opcao.Cor, opcao.Cor == atual), () => Aparencia.Alterar(opcao.Cor));

      var personalizada = !Array.Exists(Aparencia.Opcoes, x => x.Cor == atual);
      AdicionarItem(menu, CabecalhoCor("Cor personalizada...", personalizada ? atual : (Color?)null, personalizada), () => {
        using (var dialogo = new System.Windows.Forms.ColorDialog {
          FullOpen = true,
          Color = System.Drawing.Color.FromArgb(atual.R, atual.G, atual.B),
        }) {
          if (dialogo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            Aparencia.Alterar(Color.FromRgb(dialogo.Color.R, dialogo.Color.G, dialogo.Color.B));
        }
      });

      menu.IsOpen = true;
    }

    /// <summary>Amostra da cor + nome + ✓ na cor atual.</summary>
    private static object CabecalhoCor(string nome, Color? cor, bool atual) {
      var linha = new StackPanel { Orientation = Orientation.Horizontal };
      linha.Children.Add(new Border {
        Width = 16, Height = 16, CornerRadius = new CornerRadius(3), BorderThickness = new Thickness(1),
        BorderBrush = Brushes.Gray, Background = cor.HasValue ? new SolidColorBrush(cor.Value) : Brushes.Transparent,
        Margin = new Thickness(0, 0, 10, 0), VerticalAlignment = VerticalAlignment.Center,
      });
      linha.Children.Add(new TextBlock { Text = nome, VerticalAlignment = VerticalAlignment.Center, MinWidth = 150 });
      if (atual)
        linha.Children.Add(new TextBlock { Text = "", FontFamily = new FontFamily("Segoe MDL2 Assets"), VerticalAlignment = VerticalAlignment.Center });
      return linha;
    }

    /// <summary>Menu flutuante à direita do botão, no estilo dos submenus do LmPainel.</summary>
    private static ContextMenu NovoMenu(UIElement botao) {
      var menu = new ContextMenu { PlacementTarget = botao, Placement = PlacementMode.Right };
      menu.SetResourceReference(StyleProperty, Lm.LmPainel.EstiloSubmenuKey);
      return menu;
    }

    private static MenuItem AdicionarItem(ContextMenu menu, object cabecalho, Action acao) {
      var item = new MenuItem { Header = cabecalho };
      item.SetResourceReference(StyleProperty, Lm.LmPainel.EstiloItemSubmenuKey);
      if (acao != null)
        item.Click += (s, a) => acao();
      menu.Items.Add(item);
      return item;
    }
  }
}
