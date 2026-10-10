using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    }

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
        LmException.ShowException(ex, "Erro ao excluir perfil");
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
        LmException.ShowException(ex, "Erro ao consultar perfis");
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
        LmException.ShowException(ex, "Erro ao carregar perfil");
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

    private void LmView_AjudaClick(object sender, EventArgs e) {
      Process.Start("https://youtu.be/vOrGxa57-jc");
    }
  }
}
