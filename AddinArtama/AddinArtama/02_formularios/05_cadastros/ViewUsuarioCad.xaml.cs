using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Cadastro de usuário, com os perfis e as permissões que eles dão (substitui o FrmUsuarioCad).</summary>
  public partial class ViewUsuarioCad : Wpf.LmView {
    /// <summary>Linha do grid de permissões (ícone conforme o tipo: menu, formulário ou configuração).</summary>
    public class PermissaoLinha {
      public ImageSource Imagem { get; set; }
      public int Codigo { get; set; }
      public string Descricao { get; set; }
    }

    static readonly ImageSource imgMenu = ImagemWpf.Converter(Properties.Resources.menu);
    static readonly ImageSource imgFormulario = ImagemWpf.Converter(Properties.Resources.windows);
    static readonly ImageSource imgConfiguracao = ImagemWpf.Converter(Properties.Resources.tools);

    usuarios model = new usuarios();
    SortableBindingList<W_UsuarioPerfil> perfisUsuario = new SortableBindingList<W_UsuarioPerfil>();

    public ViewUsuarioCad() : this(0) { }

    public ViewUsuarioCad(int idUsuario) {
      InitializeComponent();

      if (!Corbie_Admin.TemPermissao(PermissoesSistema.PerfilUsuarioCad))
        btnSelPerfil.Visibility = btnAltPerfil.Visibility = Visibility.Collapsed;

      Loaded += (s, e) => {
        if (idUsuario > 0) {
          txtID.Text = idUsuario.ToString();
          CarregarUsuario();
        } else {
          Limpar();
        }
      };
    }

    #region Cadastro

    private void Limpar() {
      perfisUsuario = new SortableBindingList<W_UsuarioPerfil>();
      AtualizarGridPerfis();
      dgvPermis.ItemsSource = null;

      btnAltPerfil.IsEnabled = false;
      btnExcluir.IsEnabled = false;

      txtID.IsReadOnly = false;
      txtID.Text = txtNome.Text = txtLogin.Text = string.Empty;
      txtSenha.Clear();

      ckbSituacao.IsChecked = true;
      model = new usuarios();

      tbcUsuario.SelectedIndex = 0;
      txtNome.Focus();
    }

    private void BtnLimpar_Click(object sender, RoutedEventArgs e) {
      Limpar();
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      if (!LmValidacao.Validar(pnlCadastro)) {
        tbcUsuario.SelectedIndex = 0;
        return;
      }

      try {
        model.nome = txtNome.Text;
        model.login = txtLogin.Text;
        model.senha = txtSenha.Senha.CriptografarAES();
        model.ativo = ckbSituacao.IsChecked == true;

        if (perfisUsuario.Count == 0) {
          Wpf.Toast.Warning("Selecione um perfil!");
          tbcUsuario.SelectedIndex = 1;
          return;
        }

        model.perfil = string.Join("^", perfisUsuario.Select(x => x.Codigo));

        if (usuarios.Salvar(model)) {
          CarregarPerfis();

          txtID.Text = model.id.ToString();
          txtID.IsReadOnly = true;
          btnExcluir.IsEnabled = true;
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Usuário");
      }
    }

    private void BtnExcluir_Click(object sender, RoutedEventArgs e) {
      try {
        if (Wpf.MsgBox.Show("Deseja realmente excluir este registro?",
            "Excluir Registro", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
          usuarios.Excluir(model.id);
          Wpf.Toast.Info("Excluido com Sucesso!");
          Limpar();
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Excluir Usuário");
      }
    }

    private void TxtLogin_LostFocus(object sender, RoutedEventArgs e) {
      try {
        if (string.IsNullOrWhiteSpace(txtLogin.Text))
          return;

        using (ContextoDados db = new ContextoDados()) {
          var login = txtLogin.Text;
          var id = model.id;

          bool loginExiste = id != 0
            ? db.usuarios.Any(x => x.id != id && x.login == login)
            : db.usuarios.Any(x => x.login == login);

          if (loginExiste)
            Wpf.Toast.Warning($"Usuário \"{login}\" já cadastrado");
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao verificar Login");
      }
    }

    #endregion

    #region Código

    private void TxtID_BotaoF7Click(object sender, RoutedEventArgs e) {
      Consultar();
    }

    /// <summary>Enter carrega o código digitado (sem acionar o Salvar). F7 é tratado pelo próprio campo.</summary>
    private void TxtID_PreviewKeyDown(object sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
        e.Handled = true;
        CarregarUsuario();
      }
    }

    private void TxtID_LostFocus(object sender, RoutedEventArgs e) {
      CarregarUsuario();
    }

    private void Consultar() {
      // Consulta da LmCorbieUI (WinForms): o controle vazio só serve para ela não formatar colunas pela tela.
      var frm = new FrmConsultaGeral(new System.Windows.Forms.Control(), usuarios.Selecionar(), "Consulta de Usuário");
      if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK && int.TryParse(frm.valor[0], out _)) {
        txtID.Text = frm.valor[0];
        CarregarUsuario();
      }
    }

    private void CarregarUsuario() {
      if (string.IsNullOrEmpty(txtID.Text))
        return;

      try {
        int id = int.Parse(txtID.Text);

        if (model.id == id) return;

        var usuario = usuarios.Selecionar(id);
        if (usuario == null) {
          model = new usuarios();
          return;
        }

        model = usuario;

        txtID.Text = model.id.ToString();
        txtNome.Text = model.nome;
        txtLogin.Text = model.login;
        txtSenha.Senha = model.senha.DescriptografarAES();

        ckbSituacao.IsChecked = model.ativo;

        CarregarPerfis();

        txtID.IsReadOnly = true;
        btnExcluir.IsEnabled = true;
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Carregar Usuário");
      }
    }

    #endregion

    #region Perfis

    private void BtnSelPerfil_Click(object sender, RoutedEventArgs e) {
      var janela = new WindowSelecionarPerfil(perfisUsuario);
      if (janela.ShowDialog() == true) {
        perfisUsuario = janela.PerfisSelecionados;
        AtualizarGridPerfis();
      }
    }

    private void BtnAltPerfil_Click(object sender, RoutedEventArgs e) {
      if (!(dgvPerfil.SelectedItem is W_UsuarioPerfil perfil))
        return;

      AbrirPerfil(new ViewPerfil(perfil.Codigo));
    }

    private void BtnAddPerfil_Click(object sender, RoutedEventArgs e) {
      AbrirPerfil(new ViewPerfil());
    }

    /// <summary>Abre o cadastro de perfil como janela e, ao voltar, recarrega os perfis/permissões gravados do usuário.</summary>
    private void AbrirPerfil(ViewPerfil view) {
      JanelaView.Mostrar(view, altura: 640);

      if (model.id > 0)
        CarregarPerfis();
    }

    private void AtualizarGridPerfis() {
      dgvPerfil.ItemsSource = null;
      dgvPerfil.ItemsSource = perfisUsuario;

      if (perfisUsuario.Count > 0)
        dgvPerfil.SelectedIndex = 0;

      btnAltPerfil.IsEnabled = perfisUsuario.Count > 0 && Corbie_Admin.TemPermissao(PermissoesSistema.PerfilUsuarioCad);
    }

    /// <summary>Recarrega os perfis gravados do usuário e monta a lista de permissões (sem repetir).</summary>
    private void CarregarPerfis() {
      try {
        var permissoes = new List<PermissaoLinha>();

        perfisUsuario = perfis.SelecionarPerfis(model.id);
        AtualizarGridPerfis();

        foreach (var perfil in perfisUsuario) {
          var perms = perfil.Permissoes.Split('^');
          foreach (var perm in perms) {
            int idPerm = Convert.ToInt32(perm);

            try {
              var permissao = ((PermissoesSistema)idPerm).ObterDescricaoEnum();
              if (!string.IsNullOrEmpty(permissao) && !permissoes.Any(x => x.Codigo == idPerm)) {
                var tipo = (TipoPermissao)((PermissoesSistema)idPerm).ObterPermissao();

                permissoes.Add(new PermissaoLinha { Imagem = ImagemTipo(tipo), Codigo = idPerm, Descricao = permissao });
              }
            } catch (Exception) {
              Wpf.MsgBox.Show($"Verificar a permissão nº ({idPerm}) do perfil ({perfil.Descricao}), " +
                  $"não encontrada nas permissões padrão do sistema", "Ação não permitida",
                  MessageBoxButton.OK, MessageBoxImage.Warning);
            }
          }
        }

        dgvPermis.ItemsSource = permissoes;
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Carregar Perfis");
      }
    }

    private static ImageSource ImagemTipo(TipoPermissao tipo) {
      switch (tipo) {
        case TipoPermissao.Menu: return imgMenu;
        case TipoPermissao.Formulario: return imgFormulario;
        case TipoPermissao.Configuracao: return imgConfiguracao;
        default: return null;
      }
    }

    #endregion

    private void LmView_AjudaClick(object sender, EventArgs e) {
      Process.Start("https://youtu.be/B_oJWzABF_A");
    }
  }
}
