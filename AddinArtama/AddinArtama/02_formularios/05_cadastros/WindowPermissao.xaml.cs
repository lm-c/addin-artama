using LmCorbieUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>
  /// Árvore de permissões do perfil, na mesma estrutura do menu (substitui o FrmPermissao).
  /// Retorna em <see cref="Permissoes"/> os códigos marcados separados por "^".
  /// </summary>
  public partial class WindowPermissao : Wpf.LmWindow {
    static readonly Dictionary<TipoPermissao, ImageSource> imagens = new Dictionary<TipoPermissao, ImageSource> {
      [TipoPermissao.Menu] = ImagemWpf.Converter(Properties.Resources.menu),
      [TipoPermissao.Formulario] = ImagemWpf.Converter(Properties.Resources.windows),
      [TipoPermissao.Configuracao] = ImagemWpf.Converter(Properties.Resources.tools),
    };

    /// <summary>Nó da árvore: uma permissão do sistema, com o menu pai e os itens filhos.</summary>
    public class PermissaoNo : INotifyPropertyChanged {
      bool marcado;

      internal PermissaoNo(PermissoesSistema permissao, PermissaoNo pai) {
        Permissao = permissao;
        Pai = pai;
        Descricao = permissao.ObterDescricaoEnum();

        if (pai != null) {
          imagens.TryGetValue((TipoPermissao)permissao.ObterPermissao(), out var imagem);
          Imagem = imagem;
          pai.Filhos.Add(this);
        }
      }

      internal PermissoesSistema Permissao { get; }
      public PermissaoNo Pai { get; }
      public string Descricao { get; }
      public ImageSource Imagem { get; }
      public ObservableCollection<PermissaoNo> Filhos { get; } = new ObservableCollection<PermissaoNo>();

      public bool Marcado {
        get => marcado;
        set {
          if (marcado == value)
            return;

          marcado = value;
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Marcado)));
        }
      }

      public IEnumerable<PermissaoNo> Descendentes => Filhos.SelectMany(f => new[] { f }.Concat(f.Descendentes));

      public event PropertyChangedEventHandler PropertyChanged;
    }

    readonly PermissaoNo raiz;

    /// <summary>Permissões marcadas ao salvar ("101^102^10301").</summary>
    public string Permissoes { get; private set; }

    public WindowPermissao(string permissoes) {
      InitializeComponent();

      var atuais = new HashSet<string>((permissoes ?? string.Empty).Split('^'));

      raiz = new PermissaoNo(PermissoesSistema.Solucao, null) { Marcado = true };
      MontarArvore(raiz);

      foreach (var no in raiz.Descendentes)
        no.Marcado = atuais.Contains(((int)no.Permissao).ToString());

      trvPermissoes.ItemsSource = new[] { raiz };
    }

    /// <summary>Mesma estrutura do menu do painel.</summary>
    private static void MontarArvore(PermissaoNo raiz) {
      Novo(raiz, PermissoesSistema.AplicacaoProcesso);

      Novo(raiz, PermissoesSistema.PropsPersonalizadas);

      var desenho = Novo(raiz, PermissoesSistema.Desenho);
      Novo(desenho, PermissoesSistema.CriarAlterarDesenhos);
      Novo(desenho, PermissoesSistema.AtualizarTemplatesDesenhos);

      var exportar = Novo(raiz, PermissoesSistema.Exportar);
      Novo(exportar, PermissoesSistema.ExportarPDF);
      Novo(exportar, PermissoesSistema.ExportarDXF);

      var cadastros = Novo(raiz, PermissoesSistema.Cadastros);
      Novo(cadastros, PermissoesSistema.UsuarioCad);
      Novo(cadastros, PermissoesSistema.PerfilUsuarioCad);
      Novo(cadastros, PermissoesSistema.MaterialCad);
      Novo(cadastros, PermissoesSistema.MateriaPrimaCad);
      Novo(cadastros, PermissoesSistema.SenhaRedefinir);

      var relatorios = Novo(raiz, PermissoesSistema.Relatorios);
      Novo(relatorios, PermissoesSistema.ProcessoFabricacao);
      Novo(relatorios, PermissoesSistema.PackList);
      Novo(relatorios, PermissoesSistema.PlanoPintura);
      Novo(relatorios, PermissoesSistema.ManutencaoPinturaPackList);
      Novo(relatorios, PermissoesSistema.ReportWorks);

      var integracao = Novo(raiz, PermissoesSistema.menuIntegracao);
      Novo(integracao, PermissoesSistema.produtoCad);
      Novo(integracao, PermissoesSistema.processoConfig);
      Novo(integracao, PermissoesSistema.IntegracaoConfig);

      Novo(raiz, PermissoesSistema.Configuracao);
    }

    private static PermissaoNo Novo(PermissaoNo pai, PermissoesSistema permissao) => new PermissaoNo(permissao, pai);

    /// <summary>
    /// Marcar um item marca os menus acima dele (senão o item não aparece no painel);
    /// desmarcar o último item de um menu desmarca o menu.
    /// </summary>
    private void CkbPermissao_Click(object sender, RoutedEventArgs e) {
      if (!(((FrameworkElement)sender).DataContext is PermissaoNo no) || no.Pai == null)
        return;

      AtualizarPai(no.Pai, no.Marcado);
    }

    private static void AtualizarPai(PermissaoNo pai, bool marcado) {
      if (pai.Pai == null)
        return; // raiz (Solução): sempre marcada, não é gravada

      if (!marcado && pai.Filhos.Any(x => x.Marcado))
        return;

      pai.Marcado = marcado;
      AtualizarPai(pai.Pai, marcado);
    }

    private void BtnMarcar_Click(object sender, RoutedEventArgs e) {
      foreach (var no in raiz.Descendentes)
        no.Marcado = true;
    }

    private void BtnDesmarcar_Click(object sender, RoutedEventArgs e) {
      foreach (var no in raiz.Descendentes)
        no.Marcado = false;
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      Permissoes = string.Join("^", raiz.Descendentes.Where(x => x.Marcado).Select(x => (int)x.Permissao));
      DialogResult = true;
    }
  }
}
