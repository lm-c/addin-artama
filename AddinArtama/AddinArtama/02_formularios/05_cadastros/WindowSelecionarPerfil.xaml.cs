using LmCorbieUI.Metodos;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Marca os perfis do usuário (substitui o FrmSelecionarPerfil).</summary>
  public partial class WindowSelecionarPerfil : Wpf.LmWindow {
    public class PerfilLinha {
      public bool Selecionado { get; set; }
      public int Codigo { get; set; }
      public string Descricao { get; set; }
    }

    readonly List<PerfilLinha> linhas;

    /// <summary>Perfis marcados ao confirmar.</summary>
    internal SortableBindingList<W_UsuarioPerfil> PerfisSelecionados { get; } = new SortableBindingList<W_UsuarioPerfil>();

    internal WindowSelecionarPerfil(IEnumerable<W_UsuarioPerfil> perfisAtuais) {
      InitializeComponent();

      var atuais = new HashSet<int>(perfisAtuais.Select(x => x.Codigo));

      linhas = perfis.SelecionarPerfis(-1)
        .Select(p => new PerfilLinha { Selecionado = atuais.Contains(p.Codigo), Codigo = p.Codigo, Descricao = p.Descricao })
        .ToList();

      dgv.ItemsSource = linhas;
    }

    private void BtnConfirmar_Click(object sender, RoutedEventArgs e) {
      foreach (var linha in linhas.Where(x => x.Selecionado))
        PerfisSelecionados.Add(new W_UsuarioPerfil { Codigo = linha.Codigo, Descricao = linha.Descricao });

      DialogResult = true;
    }
  }
}
