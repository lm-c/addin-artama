using LmCorbieUI;
using LmCorbieUI.Controls;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AddinArtama {
  public partial class FrmPerfil : LmSingleForm {
    internal perfis model = new perfis();

    public FrmPerfil(int? idPerfil = null) {
      InitializeComponent();

      if (idPerfil != null) {
        txtID.Text = idPerfil?.ToString();
      }
    }

    private void FrmPerfil_Load(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(txtID.Text))
        TxtID_Leave(txtID, new EventArgs());
      else
        BtnLimpar_Click(null, new EventArgs());
    }

    private void BtnLimpar_Click(object sender, EventArgs e) {
      txtID.ReadOnly = false;
      txtID.Refresh();

      txtID.Focus();

      flpUsuarios.Controls.Clear();
      Controles.Clear(this);

      model = new perfis();
      btnExcluir.Enabled = btnPermissao.Enabled = false;
    }

    internal void BtnSalvar_Click(object sender, EventArgs e) {
      if (Controles.PossuiCamposInvalidos(this)) return;

      model.descricao = txtDescricao.Text;
      model.observacao = txtObservacao.Text;

      if (perfis.Salvar(model) && !IsClosing) {

        if (model.id > 0)
          btnExcluir.Enabled = btnPermissao.Enabled = true;

        txtID.Text = model.id.ToString();
        txtID.ReadOnly = true;
        txtID.Refresh();

        //UcPainelTarefas.Instancia.ConfigurarPermissoes();

      } else
        txtID.Text = "";
    }

    private void BtnExcluir_Click(object sender, EventArgs e) {
      try {
        using (ContextoDados db = new ContextoDados()) {
          var usus = Enumerable.ToList(
              from x in db.usuarios.Where(x => x.ativo)
              .OrderBy(x => x.nome)
              select new { x.id, x.nome, x.perfil});
          string ususComPerfil = "";
          foreach (var usu in usus) {
            var perfs = usu.perfil.Split('^');
            if (perfs.Contains(model.id.ToString()))
              ususComPerfil += $"{usu.id:000} - {usu.nome}\r\n";
          }

          if (!string.IsNullOrEmpty(ususComPerfil)) {
            MsgBox.Show($"Este Perfil não pode ser Excluido, pois está sendo usado pelo(s) usuário(s) abaixo!\r\n\r\n" +
              $"{ususComPerfil}", "Alerta",
              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
          }

          if (MsgBox.Show("Deseja Realmente excluir este registro?",
              "Excluir Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
              if (model.id == -1) {
                MsgBox.Show("Este Perfil não pode ser Excluido!", "Cancelado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
              }
              if (perfis.Excluir(model.id))
                BtnLimpar_Click(btnLimpar, new EventArgs());
            }
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao excluir perfil");
      }
    }

    private void TxtID_ButtonClickF7(object sender, EventArgs e) {
      using (ContextoDados db = new ContextoDados()) {
        FrmConsultaGeral frm = new FrmConsultaGeral(this,
          (db.perfis.OrderBy(x => x.descricao)).ToList(), "Consulta de Perfil do Usuário");
        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK /*&& Modo == Modo.Novo*/)
          if (int.TryParse(frm.valor[0], out int ID)) {
            txtID.Text = frm.valor[0];
            TxtID_Leave(null, new EventArgs());
          }
      }
    }

    private void TxtID_Leave(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(txtID.Text)) {
        int id = int.Parse(txtID.Text);

        if (model.id == id && id > 0) return;

        flpUsuarios.Controls.Clear();

        using (ContextoDados db = new ContextoDados()) {
          model = Queryable.FirstOrDefault(db.perfis.Where(x => x.id == id));

          if (model != null) {

            txtDescricao.Text = model.descricao;
            txtObservacao.Text = model.observacao;

            txtID.ReadOnly = true;
            txtID.Refresh();

            Modo = Modo.Alteracao;
            btnExcluir.Enabled = btnPermissao.Enabled = true;

            var usus = Enumerable.ToList(
                from x in db.usuarios.Where(x => x.ativo)
                .OrderBy(x => x.nome)
                select new { x.id, x.nome, x.perfil});
            foreach (var usu in usus) {
              var perfs = usu.perfil.Split('^');
              if (perfs.Contains(model.id.ToString()))
                InserirLabel($"{usu.id:000} - {usu.nome}");
            }
          } else {
            model = new perfis();
          }
        }
      }
    }

    private void BtnPermissao_Click(object sender, EventArgs e) {
      if (model.id > 0) {
        UcPainelTarefas.Instancia.AbrirFormFilho(new FrmPermissao(model.permissoes));
      } else MsgBox.Show("Você deve primeiro Salvar o Perfil.", "Perfil não Salvo",
            MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
    }

    private void FrmPerfil_AtualizarDados(object sender, EventArgs e) {
      BtnSalvar_Click(btnSalvar, new EventArgs());
    }

    private void InserirLabel(string descricao) {
      LmLabel lbl = new LmLabel {
        AutoSize = false,
        Size = new Size(flpUsuarios.Width - 30, 21),
        FontWeight = LmCorbieUI.Design.LmLabelWeight.Regular,
        FontSize = LmCorbieUI.Design.LmLabelSize.Medium,
        Margin = new Padding(2),
        Text = descricao,
      };

      flpUsuarios.Controls.Add(lbl);
    }

    private void btnFechar_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void FrmPerfil_ClickHelp(object sender, EventArgs e) {
      Process.Start("https://youtu.be/vOrGxa57-jc");
    }
  }
}
