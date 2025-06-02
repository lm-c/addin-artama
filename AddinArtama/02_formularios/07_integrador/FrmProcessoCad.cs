using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddinArtama {
  public partial class FrmProcessoCad : LmSingleForm {
    processos model = new processos();

    public FrmProcessoCad(int id_processo = 0) {
      InitializeComponent();

      txtID.Text = id_processo.ToString("#");

      CarregarComboBox();
    }

    private void FrmProcessoCad_Load(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(txtID.Text))
        TxtID_Leave(txtID, new EventArgs());
      else
        BtnLimpar_Click(null, new EventArgs());
    }

    private void BtnLimpar_Click(object sender, EventArgs e) {
      txtID.ReadOnly = false;
      txtOperacao.Focus();
      txtID.Text = string.Empty;
      txtMaquina.SelectedValue = null;
      txtOperacao.SelectedValue = null;
      btnExcluir.Enabled = false;
      txtID.ReadOnly = false;
      txtID.Refresh();

      ckbSituacao.Checked = true;
      model = new processos();
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      if (Controles.PossuiCamposInvalidos(this)) return;

      SalvarAsync();
    }

    private async Task SalvarAsync() {
      try {
        model.ativo = ckbSituacao.Checked;
        model.codigo_maquina = (int)txtMaquina.SelectedValue;
        model.codigo_operacao = (int)txtOperacao.SelectedValue;

        if (processos.Salvar(model)) {
          //txtID.Text = model.id.ToString();
          //txtID.ReadOnly = true;
          //txtID.Refresh();
          BtnLimpar_Click(null, null);
        }

        await Processo.Carregar();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Operação");
      }
    }

    private void BtnExcluir_Click(object sender, EventArgs e) {
      try {
        if (MsgBox.Show("Deseja realmente excluir este registro?",
          "Excluir Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
          processos.Excluir(model.id);
          Toast.Info("Excluido com Sucesso!");
          BtnLimpar_Click(sender, new EventArgs());
          Processo.Carregar();
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Cadastrar Matéria Prima");
      }
    }

    private void TxtID_ButtonClickF7(object sender, EventArgs e) {
      FrmConsultaGeral frm = new FrmConsultaGeral(this,
       Processo.ListaProcessos, "Consulta de Processos");
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
        model = processos.SelecionarProcesso(id);

        if (model != null) {
          txtID.Text = model.id.ToString();
          ckbSituacao.Checked = model.ativo;
          txtMaquina.SelectedValue = model.codigo_maquina;
          txtOperacao.SelectedValue = model.codigo_operacao;

          ckbSituacao.Checked = model.ativo;

          btnExcluir.Enabled = true;

          txtID.ReadOnly = true;
        } else model = new processos();
      }
    }

    private void CarregarComboBox() {
      Invoke(new MethodInvoker(async delegate () {
        try {
          var maqs = await Api.GetMaquinasAsync();
          var ops = await Api.GetOpsAsync();

          maqs.ForEach(x => { x.descricao = $"{x.codMaquina} - {x.descricao} [{x.mascara}]"; });
          ops.ForEach(x => { x.descricao = $"{x.codOperacao} - {x.descricao} [{x.abreviatura}]"; });
          txtMaquina.CarregarComboBox(maqs);
          txtOperacao.CarregarComboBox(ops);
        } catch (Exception ex) {
          //Toast.Warning(ex.ToString());
          // LmException.ShowException(ex, "Erro ao carregar lista Maquina");
        }
      }));
    }

    private void FrmProcessoCad_ClickHelp(object sender, EventArgs e) {
      //Process.Start("https://youtu.be/B_oJWzABF_A");
    }
  }
}
