using AddinArtama;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AddinArtama {
  public partial class FrmUsuarioCad : LmSingleForm {
    usuarios model = new usuarios();

    SortableBindingList<W_Permissao> _permissoes = new SortableBindingList<W_Permissao>();

    public FrmUsuarioCad(int idUsuario = 0) {
      InitializeComponent();

      tbcUsuario.SelectedIndex = 0;

      dgvPerfil.MontarGrid<W_UsuarioPerfil>();
      dgvPermis.MontarGrid<W_Permissao>();

      txtID.Text = idUsuario.ToString("#");
    }

    private void FrmUsuarioCad_Load(object sender, EventArgs e) {
      if (!Corbie_Admin.TemPermissao(PermissoesSistema.PerfilUsuarioCad))
        btnSelPerfil.Visible = btnAltPerfil.Visible = false;

      if (!string.IsNullOrEmpty(txtID.Text))
        TxtID_Leave(txtID, new EventArgs());
      else
        BtnLimpar_Click(null, new EventArgs());
    }

    private void BtnLimpar_Click(object sender, EventArgs e) {
      dgvPerfil.MontarGrid<W_UsuarioPerfil>();
      dgvPermis.MontarGrid<W_Permissao>();

      _permissoes = new SortableBindingList<W_Permissao>();

      btnAltPerfil.Enabled = false;
      txtID.ReadOnly = false;
      btnExcluir.Enabled = false;
      txtNome.Focus();
      txtID.Text = txtNome.Text = txtLogin.Text = txtSenha.Text = string.Empty;
     
      txtID.ReadOnly = false;
      txtID.Refresh();

      ckbSituacao.Checked = true;
      model = new usuarios();
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      if (Controles.PossuiCamposInvalidos(this)) return;

      try {

        model.nome = txtNome.Text;
        model.login = txtLogin.Text;
        model.senha = txtSenha.Text.CriptografarAES();
        model.ativo = ckbSituacao.Checked;

        if(dgvPerfil.Grid.RowCount == 0) {
          Toast.Warning("Selecione um perfil!");
          return;
        }

        model.perfil = string.Empty;
        foreach (W_UsuarioPerfil perfil in (SortableBindingList<W_UsuarioPerfil>)dgvPerfil.Grid.DataSource)
          model.perfil += perfil.Codigo + "^";

        model.perfil = model.perfil.Substring(0, model.perfil.Length - 1);

        if (usuarios.Salvar(model)) {
          CarregarPerfis();

          txtID.Text = model.id.ToString();
          txtID.ReadOnly = true;
          txtID.Refresh();
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Matéria Prima");
      }
    }

    private void BtnExcluir_Click(object sender, EventArgs e) {
      try {
        if (MsgBox.Show("Deseja realmente excluir este registro?",
          "Excluir Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
          usuarios.Excluir(model.id);
          Toast.Info("Excluido com Sucesso!");
          BtnLimpar_Click(sender, new EventArgs());
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Matéria Prima");
      }
    }

    private void TxtID_ButtonClickF7(object sender, EventArgs e) {
      FrmConsultaGeral frm = new FrmConsultaGeral(this,
        usuarios.Selecionar(), "Consulta de Matéria Prima");
      if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK /*&& Modo == Modo.Novo*/)
        if (int.TryParse(frm.valor[0], out int ID)) {
          txtID.Text = frm.valor[0];
          TxtID_Leave(null, new EventArgs());
        }
    }

    private void TxtID_Leave(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(txtID.Text)) {
        int id = int.Parse(txtID.Text);

        if (model.id == id) return;
        model = usuarios.Selecionar(id);

        txtID.Text = model.id.ToString();
        txtNome.Text = model.nome;
        txtLogin.Text = model.login;
        txtSenha.Text = model.senha.DescriptografarAES();

        ckbSituacao.Checked = model.ativo;

        CarregarPerfis();

        txtID.ReadOnly = true;
        btnExcluir.Enabled = true;
      }
    }

    private void BtnSelPerfil_Click(object sender, EventArgs e) {
      FrmSelecionarPerfil frm = new FrmSelecionarPerfil((SortableBindingList<W_UsuarioPerfil>)dgvPerfil.Grid.DataSource);
      if (frm.ShowDialog() == DialogResult.OK) {
        dgvPerfil.CarregarGrid(frm._listaPerfis);
        //dgvPerfil.Grid.DataSource = frm._listaPerfis;
      }
    }

    private void BtnAltPerfil_Click(object sender, EventArgs e) {
      if (dgvPerfil.Grid.CurrentRow == null) return;

      int id = ((W_UsuarioPerfil)dgvPerfil.Grid.CurrentRow.DataBoundItem).Codigo;

      UcPainelTarefas.Instancia.AbrirFormFilho(new FrmPerfil(id));
    }

    private void BtnAddPerfil_Click(object sender, EventArgs e) {
      UcPainelTarefas.Instancia.AbrirFormFilho(new FrmPerfil());
    }

    private void FrmUsuarioCad_AtualizarDados(object sender, EventArgs e) {
      CarregarPerfis();
    }

    private void txtLogin_Leave(object sender, EventArgs e) {
      using (ContextoDados db = new ContextoDados()) {
        bool loginExiste = false;
        if (model.id != 0)
          loginExiste = db.usuarios.Any(x => x.id != model.id && x.login == txtLogin.Text);
        else
          loginExiste = db.usuarios.Any(x => x.login == txtLogin.Text);

        if (loginExiste) {
          MsgBox.ShowToolTip(txtLogin, $"Usuário \"{txtLogin.Text}\" já cadastrado");

          txtLogin.Focus();
        }
      }
    }

    private void CarregarPerfis() {
      try {
        _permissoes = new SortableBindingList<W_Permissao>();

        var usuPerfis = perfis.SelecionarPerfis(model.id);
        dgvPerfil.CarregarGrid(usuPerfis);

        btnAltPerfil.Enabled = usuPerfis.Count > 0 && Corbie_Admin.TemPermissao(PermissoesSistema.PerfilUsuarioCad);

        foreach (var perfil in usuPerfis) {
          var perms = perfil.Permissoes.Split('^');
          foreach (var perm in perms) {
            int idPerm = Convert.ToInt32(perm);
            string permissao = string.Empty;
            TipoPermissao tipo = TipoPermissao.Indefinido;

            try {
              permissao = ((PermissoesSistema)idPerm).ObterDescricaoEnum();
              if (!string.IsNullOrEmpty(permissao) && !_permissoes.Any(x => x.Codigo == idPerm)) {
                tipo = (TipoPermissao)((PermissoesSistema)idPerm).ObterPermissao();

                var bmp = new Bitmap(20, 20);

                switch (tipo) {
                  case TipoPermissao.Menu:
                  bmp = Properties.Resources.trv_menu;
                  break;
                  case TipoPermissao.Formulario:
                  bmp = Properties.Resources.trv_form;
                  break;
                  case TipoPermissao.Configuracao:
                  bmp = Properties.Resources.trv_permissao;
                  break;
                  default:
                  break;
                }

                _permissoes.Add(new W_Permissao { ImgAnexo = bmp, Codigo = idPerm, Descricao = permissao });
              }

            } catch (Exception ex) {
              MsgBox.Show($"Verificar a permissão nº ({idPerm}) do perfil ({perfil.Descricao}), " +
                  $"não encontrada nas permissões padrão do sistema", "Ação não permitida",
                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
          }
        }

        CarregarGridPermissao();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Carregar Perfis");
      }
    }

    private void CarregarGridPermissao() {
      dgvPermis.CarregarGrid(_permissoes);
    }

    private void BtnFechar_Click(object sender, EventArgs e) {
        this.Close();
    }

    private void FrmUsuarioCad_ClickHelp(object sender, EventArgs e) {
      Process.Start("https://youtu.be/B_oJWzABF_A");
    }
  }
}
