using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using SolidWorks.Interop.sldworks;
using LmCorbieUI;
using LmCorbieUI.LmForms;

namespace AddinArtama {
  public partial class FrmConfigTemplate : LmSingleForm {
    templates model = new templates();

    public FrmConfigTemplate() {
      InitializeComponent();

      this.WindowState = FormWindowState.Maximized;
    }

    private void FrmConfigTemplate_Loaded(object sender, EventArgs e) {
      try {
        Invoke(new MethodInvoker(delegate () {
            model = templates.model;

            if (model != null) {
              this.Modo = Modo.Alteracao;

              txtA4R.Text = model.formato_a4r;
              txtA4P.Text = model.formato_a4p;
              txtA3.Text =  model.formato_a3;
              txtA2.Text =  model.formato_a2;
              txtA1.Text =  model.formato_a1;
              txtA0.Text =  model.formato_a0;
              lblA4R.Text = model.template_a4r;
              lblA4P.Text = model.template_a4p;
              lblA3.Text =  model.template_a3;
              lblA2.Text =  model.template_a2;
              lblA1.Text =  model.template_a1;
              lblA0.Text =  model.template_a0;
              txtListaMotagem.Text = model.lista_montagem;
              txtListaSoldagem.Text = model.lista_soldagem;
            } else {
              Modo = Modo.Novo;
              model = new templates();
            }
        }));
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Carregar Dados\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      if (!ValidarDados()) return;

      if (Modo == Modo.Novo && model.id == 0 && templates.Salvar(model)) {
        Modo = Modo.Alteracao;
        MsgBox.Show("Salvo com Sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Atalizar();
      } else if (Modo == Modo.Alteracao && templates.Alterar(model)) {
        MsgBox.Show("Alterado com Sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Atalizar();
      }
    }

    private void TxtA4R_ButtonClickF7(object sender, EventArgs e) {
      txtA4R.Text = SelecionarFormato("A4 Retrato");
      if (!string.IsNullOrEmpty(txtA4R.Text))
        lblA4R.Text = Path.GetFileNameWithoutExtension(txtA4R.Text) + ".slddrt";
    }

    private void TxtA4P_ButtonClickF7(object sender, EventArgs e) {
      txtA4P.Text = SelecionarFormato("A4 Paisagem");
      if (!string.IsNullOrEmpty(txtA4P.Text))
        lblA4P.Text = Path.GetFileNameWithoutExtension(txtA4P.Text) + ".slddrt";
    }

    private void TxtA3_ButtonClickF7(object sender, EventArgs e) {
      txtA3.Text = SelecionarFormato("A3");
      if (!string.IsNullOrEmpty(txtA3.Text))
        lblA3.Text = Path.GetFileNameWithoutExtension(txtA3.Text) + ".slddrt";
    }

    private void TxtA2_ButtonClickF7(object sender, EventArgs e) {
      txtA2.Text = SelecionarFormato("A2");
      if (!string.IsNullOrEmpty(txtA2.Text))
        lblA2.Text = Path.GetFileNameWithoutExtension(txtA2.Text) + ".slddrt";
    }

    private void TxtA1_ButtonClickF7(object sender, EventArgs e) {
      txtA1.Text = SelecionarFormato("A1");
      if (!string.IsNullOrEmpty(txtA1.Text))
        lblA1.Text = Path.GetFileNameWithoutExtension(txtA1.Text) + ".slddrt";
    }

    private void TxtA0_ButtonClickF7(object sender, EventArgs e) {
      txtA0.Text = SelecionarFormato("A0");
      if (!string.IsNullOrEmpty(txtA0.Text))
        lblA0.Text = Path.GetFileNameWithoutExtension(txtA0.Text) + ".slddrt";
    }

    private void TxtListaMotagem_ButtonClickF7(object sender, EventArgs e) {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.RestoreDirectory = true;
      ofd.Title = "Selecionar Lista de Montagem";
      ofd.Filter = "Lista Montagem|*.sldbomtbt|All files|*.*";
      ofd.DefaultExt = "sldbomtbt";

      if (ofd.ShowDialog() == DialogResult.OK)
        txtListaMotagem.Text = ofd.FileName;
    }

    private void TxtListaSoldagem_ButtonClickF7(object sender, EventArgs e) {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.RestoreDirectory = true;
      ofd.Title = "Selecionar Lista de Corte/Soldagem";
      ofd.Filter = "Lista Corte/Soldagem|*.sldwldtbt|All files|*.*";
      ofd.DefaultExt = "sldwldtbt";

      if (ofd.ShowDialog() == DialogResult.OK)
        txtListaSoldagem.Text = ofd.FileName;
    }

    private string SelecionarFormato(string formato) {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.RestoreDirectory = true;
      ofd.Title = $"Selecionar Formato de Folha {formato}";
      ofd.Filter = "Templates Desenho|*.drwdot|All files|*.*";
      ofd.DefaultExt = "drwdot";

      if (ofd.ShowDialog() == DialogResult.OK)
        return ofd.FileName;
      else
        return string.Empty;
    }

    private bool ValidarDados() {
      //string msg = string.Empty;

      //if (string.IsNullOrEmpty(txtNome.Text))
      //    msg += "Campo \"Nome\" é Obrigatório\n";
      //if (string.IsNullOrEmpty(txtLogin.Text))
      //    msg += "Campo \"Login\" é Obrigatório\n";
      //if (string.IsNullOrEmpty(cmbSetor.Text))
      //    msg += "Campo \"Setor\" é Obrigatório\n";
      //if (string.IsNullOrEmpty(cmbCargo.Text))
      //    msg += "Campo \"Cargo\" é Obrigatório\n";

      //if (!string.IsNullOrEmpty(msg))
      //{
      //    MsgBox.Show($"Campos Obrigatórios não Informados\n\n" +
      //        $"{msg}", "Ação não Permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      //    return false;
      //}

      model.formato_a4r = txtA4R.Text;
      model.formato_a4p = txtA4P.Text;
      model.formato_a3 = txtA3.Text;
      model.formato_a2 = txtA2.Text;
      model.formato_a1 = txtA1.Text;
      model.formato_a0 = txtA0.Text;
      model.template_a4r = lblA4R.Text;
      model.template_a4p = lblA4P.Text;
      model.template_a3 = lblA3.Text;
      model.template_a2 = lblA2.Text;
      model.template_a1 = lblA1.Text;
      model.template_a0 = lblA0.Text;
      model.lista_montagem = txtListaMotagem.Text;
      model.lista_soldagem = txtListaSoldagem.Text;

      return true;
    }

    private void Atalizar() {
      try {
        //bool boolstatus;
        //boolstatus = Sw.App.SetUserPreferenceStringValue(
        //    (int)swUserPreferenceStringValue_e.swFileLocationsDocumentTemplates,
        //    Path.GetDirectoryName(txtA3.Text));
        //boolstatus = Sw.App.SetUserPreferenceStringValue(
        //    (int)swUserPreferenceStringValue_e.swFileLocationsDraftingStandard,
        //    Path.GetDirectoryName(txtA3.Text));
        //boolstatus = Sw.App.SetUserPreferenceStringValue(
        //    (int)swUserPreferenceStringValue_e.swFileLocationsSheetFormat,
        //    Path.GetDirectoryName(txtA3.Text));
        //boolstatus = Sw.App.SetUserPreferenceStringValue(
        //   (int)swUserPreferenceStringValue_e.swFileLocationsBOMTemplates,
        //   Path.GetDirectoryName(txtListaMotagem.Text));
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Atualizar Configurações do Solid\n" +
            $"\n" +
            $"{ex.Message}", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }
}
