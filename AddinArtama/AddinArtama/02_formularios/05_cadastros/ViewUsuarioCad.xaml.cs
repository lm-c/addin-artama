using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Collections.Generic;
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

      DefinirAssistente(RoteiroAssistente);

      Loaded += (s, e) => {
        if (idUsuario > 0) {
          txtID.Text = idUsuario.ToString();
          CarregarUsuario();
        } else {
          Limpar();
        }
      };
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      // Cada passo seleciona a aba onde está o controle destacado.
      Wpf.LmPassoAssistente Passo(int aba, string titulo, string texto, FrameworkElement alvo, FrameworkElement alvoAte = null) {
        var passo = Wpf.LmPassoAssistente.Info(titulo, texto, alvo, alvoAte);
        passo.AoEntrar = () => tbcUsuario.SelectedIndex = aba;
        return passo;
      }

      yield return Wpf.LmPassoAssistente.Info("Cadastro de Usuário",
        "Aqui você cadastra quem usa o sistema e define o que cada um pode acessar, por meio dos perfis.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Passo(0, "1. Código",
        "Para um usuário NOVO, deixe o código em branco: ele é gerado ao salvar.\n\n" +
        "Para ALTERAR um usuário, digite o código e tecle Enter, ou use F7 para pesquisar.\n\n" +
        "Regra: depois de carregado ou salvo, o código fica bloqueado. Use Limpar para trocar de usuário.",
        txtID);

      yield return Passo(0, "2. Nome",
        "Nome completo do usuário. É obrigatório.",
        txtNome);

      yield return Passo(0, "3. Login",
        "É o nome que o usuário digita para entrar no sistema. Obrigatório, com até 10 caracteres.\n\n" +
        "Regra: ao sair do campo, o sistema avisa se já existe outro usuário com o mesmo login.",
        txtLogin);

      yield return Passo(0, "4. E-mail",
        "E-mail do usuário. É opcional, mas recomendado.\n\n" +
        "Regra: é para este e-mail que vai o código do botão Redefinir Senha, na tela de login. Sem e-mail cadastrado, o usuário não consegue redefinir a senha sozinho.",
        txtEmail);

      yield return Passo(0, "5. Senha",
        "Senha de acesso do usuário. É obrigatória.\n\n" +
        "Regra: a senha é gravada criptografada no banco de dados.",
        txtSenha);

      yield return Passo(0, "6. Situação",
        "Desmarque \"Ativo\" para desativar o usuário sem excluir o cadastro.\n\n" +
        "Regra: usuários inativos deixam de aparecer nas listas de Projetista e Desenhista da tela Propriedades Personalizadas.",
        pnlSituacao);

      yield return Passo(1, "7. Perfis do usuário",
        "Na aba Perfil você define o que o usuário pode acessar.\n\n" +
        "• Selecionar: escolhe um ou mais perfis para o usuário.\n" +
        "• Alterar: abre o cadastro do perfil selecionado na lista.\n" +
        "• Perfil: cadastra um perfil novo.\n\n" +
        "Regras: o usuário precisa de pelo menos UM perfil para ser salvo. " +
        "Selecionar e Alterar só aparecem para quem tem permissão no Cadastro de Perfil.",
        pnlBotoesPerfil);

      yield return Passo(1, "8. Perfis selecionados",
        "Lista os perfis atribuídos ao usuário. A escolha só é gravada quando você clica em Salvar, na aba Cadastro.",
        dgvPerfil);

      yield return Passo(1, "9. Permissões",
        "Mostra tudo o que o usuário pode acessar: a soma das permissões de todos os perfis dele, sem repetir. " +
        "O ícone indica o tipo: menu, tela ou configuração.\n\n" +
        "Regra: esta lista é atualizada ao carregar ou salvar o usuário — depois de trocar os perfis, salve para ver as permissões novas.",
        dgvPermis);

      yield return Passo(0, "10. Salvar",
        "Grava o usuário e os perfis.\n\n" +
        "Regras: nome, login e senha são obrigatórios, e é preciso ter pelo menos um perfil — se faltar, a tela leva você até o que está pendente.",
        btnSalvar);

      yield return Passo(0, "11. Limpar e Excluir",
        "Limpar esvazia a tela para um novo cadastro (não apaga nada do banco).\n\n" +
        "Excluir remove o usuário carregado, depois de pedir confirmação. Só fica disponível com um usuário carregado ou salvo.",
        btnLimpar, btnExcluir);

      yield return Wpf.LmPassoAssistente.Info("Pronto!",
        "O ciclo é: preencher os dados → escolher os perfis → Salvar.\n\n" +
        "Para rever este passo a passo, clique no \"?\" no topo da tela.");
    }

    #endregion

    #region Cadastro

    private void Limpar() {
      perfisUsuario = new SortableBindingList<W_UsuarioPerfil>();
      AtualizarGridPerfis();
      dgvPermis.ItemsSource = null;

      btnAltPerfil.IsEnabled = false;
      btnExcluir.IsEnabled = false;

      txtID.IsReadOnly = false;
      txtID.Text = txtNome.Text = txtLogin.Text = txtEmail.Text = string.Empty;
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

      // O e-mail é opcional, mas sem ele o usuário não consegue redefinir a senha pelo login.
      var email = txtEmail.Text.Trim();
      if (email.Length > 0 && !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) {
        Wpf.Toast.Warning("E-mail inválido!");
        tbcUsuario.SelectedIndex = 0;
        txtEmail.Focus();
        return;
      }

      try {
        model.nome = txtNome.Text;
        model.login = txtLogin.Text;
        model.email = email;
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
        Msg.Erro(ex, "Erro ao Cadastrar Usuário");
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
        Msg.Erro(ex, "Erro ao Excluir Usuário");
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
        Msg.Erro(ex, "Erro ao verificar Login");
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
        txtEmail.Text = model.email;
        txtSenha.Senha = model.senha.DescriptografarAES();

        ckbSituacao.IsChecked = model.ativo;

        CarregarPerfis();

        txtID.IsReadOnly = true;
        btnExcluir.IsEnabled = true;
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Carregar Usuário");
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
        Msg.Erro(ex, "Erro ao Carregar Perfis");
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
  }
}
