using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Cadastro de perfil do usuário e suas permissões (substitui o FrmPerfil).</summary>
  public partial class ViewPerfil : Wpf.LmView {
    perfis model = new perfis();

    public ViewPerfil() : this(null) { }

    public ViewPerfil(int? idPerfil) {
      InitializeComponent();

      Loaded += (s, e) => {
        if (idPerfil != null) {
          txtID.Text = idPerfil.ToString();
          CarregarPerfil();
        } else {
          Limpar();
        }
      };

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Cadastro de Perfil do Usuário",
        "Um perfil é um conjunto de permissões (menus, telas e configurações) com um nome, como \"Projetista\" ou \"Administrador\". " +
        "Você monta o perfil uma vez e depois o atribui aos usuários no Cadastro de Usuário.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Info("1. Código",
        "Para um perfil NOVO, deixe o código em branco: ele é gerado ao salvar.\n\n" +
        "Para ALTERAR um perfil, digite o código e tecle Enter, ou use F7 para pesquisar.\n\n" +
        "Regra: depois de carregado ou salvo, o código fica bloqueado. Use Limpar para trocar de perfil.",
        txtID);

      yield return Wpf.LmPassoAssistente.Info("2. Descrição",
        "Nome do perfil, com até 80 caracteres. É obrigatório.",
        txtDescricao);

      yield return Wpf.LmPassoAssistente.Info("3. Observação",
        "Campo livre, opcional, para explicar para que serve o perfil (até 250 caracteres).",
        txtObservacao);

      yield return Wpf.LmPassoAssistente.Info("4. Salvar",
        "Grava a descrição e a observação do perfil.\n\n" +
        "Regra: o perfil precisa ser salvo ANTES de definir as permissões — é o Salvar que libera o botão Definir Permissões.",
        btnSalvar);

      yield return Wpf.LmPassoAssistente.Info("5. Definir permissões",
        "Abre a árvore de permissões, onde você marca os menus, as telas e as configurações que o perfil pode acessar.\n\n" +
        "Regras: só fica disponível com o perfil já salvo. Ao confirmar na árvore, as permissões são gravadas na hora — não é preciso clicar em Salvar de novo.",
        btnPermissao);

      yield return Wpf.LmPassoAssistente.Info("6. Usuários do perfil",
        "Lista os usuários ATIVOS que têm este perfil. Aparece ao carregar um perfil existente.\n\n" +
        "Serve para saber quem será afetado antes de mudar as permissões.",
        grpUsuarios);

      yield return Wpf.LmPassoAssistente.Info("7. Limpar e Excluir",
        "Limpar esvazia a tela para um novo cadastro (não apaga nada do banco).\n\n" +
        "Excluir remove o perfil carregado, depois de pedir confirmação.\n\n" +
        "Regra: um perfil em uso por algum usuário ativo NÃO pode ser excluído — a tela mostra quem está usando. Tire o perfil desses usuários antes.",
        btnLimpar, btnExcluir);

      yield return Wpf.LmPassoAssistente.Info("Pronto!",
        "O ciclo é: descrição → Salvar → Definir Permissões. Depois, atribua o perfil aos usuários no Cadastro de Usuário.\n\n" +
        "Para rever este passo a passo, clique no \"?\" no topo da tela.");
    }

    #endregion

    #region Cadastro

    private void Limpar() {
      txtID.IsReadOnly = false;
      txtID.Text = txtDescricao.Text = txtObservacao.Text = string.Empty;
      lstUsuarios.ItemsSource = null;

      model = new perfis();
      btnExcluir.IsEnabled = btnPermissao.IsEnabled = false;

      txtID.Focus();
    }

    private void BtnLimpar_Click(object sender, RoutedEventArgs e) {
      Limpar();
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      Salvar();
    }

    private void Salvar() {
      if (!LmValidacao.Validar(pnlCadastro)) return;

      model.descricao = txtDescricao.Text;
      model.observacao = txtObservacao.Text;

      if (perfis.Salvar(model)) {
        if (model.id > 0)
          btnExcluir.IsEnabled = btnPermissao.IsEnabled = true;

        txtID.Text = model.id.ToString();
        txtID.IsReadOnly = true;
      } else
        txtID.Text = "";
    }

    private void BtnExcluir_Click(object sender, RoutedEventArgs e) {
      try {
        var usuariosComPerfil = UsuariosDoPerfil();
        if (usuariosComPerfil.Count > 0) {
          Wpf.MsgBox.Show($"Este Perfil não pode ser Excluido, pois está sendo usado pelo(s) usuário(s) abaixo!\r\n\r\n" +
            string.Join("\r\n", usuariosComPerfil), "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
        }

        if (Wpf.MsgBox.Show("Deseja Realmente excluir este registro?",
            "Excluir Registro", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
          return;

        if (model.id == -1) {
          Wpf.MsgBox.Show("Este Perfil não pode ser Excluido!", "Cancelado", MessageBoxButton.OK, MessageBoxImage.Information);
          return;
        }

        if (perfis.Excluir(model.id))
          Limpar();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao excluir perfil");
      }
    }

    /// <summary>Usuários ativos que têm este perfil ("001 - Nome").</summary>
    private List<string> UsuariosDoPerfil() {
      using (ContextoDados db = new ContextoDados()) {
        var id = model.id.ToString();

        return db.usuarios
          .Where(x => x.ativo)
          .OrderBy(x => x.nome)
          .Select(x => new { x.id, x.nome, x.perfil })
          .ToList()
          .Where(x => x.perfil.Split('^').Contains(id))
          .Select(x => $"{x.id:000} - {x.nome}")
          .ToList();
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
        CarregarPerfil();
      }
    }

    private void TxtID_LostFocus(object sender, RoutedEventArgs e) {
      CarregarPerfil();
    }

    private void Consultar() {
      try {
        using (ContextoDados db = new ContextoDados()) {
          // Consulta da LmCorbieUI (WinForms): o controle vazio só serve para ela não formatar colunas pela tela.
          var frm = new FrmConsultaGeral(new System.Windows.Forms.Control(),
            db.perfis.OrderBy(x => x.descricao).ToList(), "Consulta de Perfil do Usuário");
          if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK && int.TryParse(frm.valor[0], out _)) {
            txtID.Text = frm.valor[0];
            CarregarPerfil();
          }
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao consultar perfis");
      }
    }

    private void CarregarPerfil() {
      if (string.IsNullOrEmpty(txtID.Text))
        return;

      try {
        int id = int.Parse(txtID.Text);

        if (model.id == id && id > 0) return;

        lstUsuarios.ItemsSource = null;

        using (ContextoDados db = new ContextoDados()) {
          var perfil = db.perfis.FirstOrDefault(x => x.id == id);

          if (perfil == null) {
            model = new perfis();
            return;
          }

          model = perfil;
        }

        txtDescricao.Text = model.descricao;
        txtObservacao.Text = model.observacao;

        txtID.IsReadOnly = true;
        btnExcluir.IsEnabled = btnPermissao.IsEnabled = true;

        lstUsuarios.ItemsSource = UsuariosDoPerfil();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao carregar perfil");
      }
    }

    #endregion

    /// <summary>Abre a árvore de permissões; ao salvar nela, grava o perfil.</summary>
    private void BtnPermissao_Click(object sender, RoutedEventArgs e) {
      if (model.id <= 0) {
        Wpf.MsgBox.Show("Você deve primeiro Salvar o Perfil.", "Perfil não Salvo", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      var janela = new WindowPermissao(model.permissoes);
      if (janela.ShowDialog() != true)
        return;

      model.permissoes = janela.Permissoes;
      Salvar();
    }
  }
}
