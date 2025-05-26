using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using System.Collections.Generic;
using LmCorbieUI.Controls;
using System.IO;

namespace AddinArtama {
  public partial class FrmProcesso : LmSingleForm {
    string _montagemPrincipal = string.Empty;
    SortableBindingList<ProdutoErp> _produtos = new SortableBindingList<ProdutoErp>();

    public FrmProcesso() {
      InitializeComponent();

      _produtos = new SortableBindingList<ProdutoErp>();
      dgv.MontarGrid<ProdutoErp>();

      CarregarControlesProcessos();
    }

    private void FrmProcesso_Loaded(object sender, EventArgs e) {
      Invoke(new MethodInvoker(delegate () {
        //dadosMateriais.DataSource = ChapaDATA.Selecionar();
        ckbAddDenom.Checked = InfoSetting.AddDenominacaoTodasConfig;
      }));
    }

    internal void CarregarControlesProcessos() {
      try {
        flpOperacoes.Controls.Clear();
        lblProcess.Text = string.Empty;

        foreach (var proc in Processo.ListaProcessos) {
          LmCheckBox ckb = new LmCheckBox {
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Color.Transparent,
            Size = new Size(flpOperacoes.Width - 30, 19),
            Margin = new Padding(6, 1, 6, 1),
            Name = $"ckb{proc.codOperacao}",
            Tag = $"{proc.codOperacao}",
            Text = $"{proc.codOperacao} - {proc.descrOperacao}",
            FontSize = LmCorbieUI.Design.LmCheckBoxSize.Small,
            UseCustomBackColor = true,
          };

          ckb.CheckedChanged += Ckb_CheckedChanged;

          flpOperacoes.Controls.Add(ckb);
        }

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Carregar Controles Processos..\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void BtnCarrProcess_Click(object sender, EventArgs e) {
      MsgBox.ShowWaitMessage("Lendo componentes da montagem...");
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
          _produtos = new SortableBindingList<ProdutoErp>();

          _montagemPrincipal = Path.GetFileNameWithoutExtension(swModel.GetPathName()).ToLower();

          _produtos = ProdutoErp.GetComponents();
          dgv.CarregarGrid(_produtos);
        } else {
          Toast.Warning("Comando apenas para Peças e Montagens");
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Carregar Componentes..\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Info($"Sem documentos abertos");
          return;
        }

        if (Controles.PossuiCamposInvalidos(pnlDados))
          return;

        if (dgv.Grid.CurrentRow == null) {
          Toast.Info($"Nenhum produto selecionado");
          return;
        }

        var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        produtoERP.Operacao = lblProcess.Text;

        MsgBox.ShowWaitMessage("Salvando. Aguarde...");

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        AdicionarDescricaoTodasConfiguracoes();

        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        //swCustPropMgr.Add3("Componente", (int)swCustomInfoType_e.swCustomInfoText,
        //    produtoERP.CompCodInterno, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        swCustPropMgr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
          swCustPropMgr.Add3("Material", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Material\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        } else
          swCustPropMgr.Delete2("Material");

        if (produtoERP.Referencia.StartsWith("Item da lista de corte")) {
          produtoERP.ItensCorte[0].Operacao = produtoERP.Operacao;
          ListaCorte.UpdateCutList(swModel, produtoERP.ItensCorte[0]);
        } else {
          swCustPropMgr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText, produtoERP.Operacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        }

        swModel.Save();

        lblMaterial.UseCustomColor =
        lblCodMat.UseCustomColor =
        lblDescMat.UseCustomColor =
        lblProcess.UseCustomColor = false;
        pnlDados.Refresh();
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao salvar..\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void BtnVoltar_Click(object sender, EventArgs e) {
      try {
        try {
          if (_produtos.Count == 0) {
            Toast.Warning("Favor Carregar Componentes primeiro.");
            return;
          }

          var swModel = (ModelDoc2)Sw.App.ActiveDoc;

          if (dgv.Grid.CurrentRow.Index > 0)
            dgv.Grid.Rows[dgv.Grid.CurrentRow.Index - 1].Cells[1].Selected = true;
          else
            dgv.Grid.Rows[dgv.Grid.RowCount - 1].Cells[1].Selected = true;
        } catch (Exception ex) {
          MsgBox.Show($"Erro ao voltar peça\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao {((Button)sender).Tag} Peça..\n\n{ex.Message}", "Addin LM Projetos",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void BtnProximo_Click(object sender, EventArgs e) {
      try {
        if (_produtos.Count == 0) {
          Toast.Warning("Favor Carregar Componentes primeiro.");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (dgv.Grid.CurrentRow.Index + 1 < dgv.Grid.RowCount)
          dgv.Grid.Rows[dgv.Grid.CurrentRow.Index + 1].Cells[1].Selected = true;
        else
          dgv.Grid.Rows[0].Cells[1].Selected = true;
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao avançar peça\n\n{ex.Message}", "Addin LM Projetos",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void Dgv_RowIndexChanged(object sender, EventArgs e) {
      try {
        if (sender == null) return;

        lblPecasProc.Text = $"Item {dgv.Grid.CurrentRow.Index + 1} de {dgv.Grid.RowCount} - {(((dgv.Grid.CurrentRow.Index + 1) * 100) / dgv.Grid.RowCount)}%";

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel != null && Path.GetFileNameWithoutExtension(swModel.GetPathName()) != _montagemPrincipal) {
          swModel.ShowNamedView("*Isométrica");
          swModel.ViewZoomtofit();

          swModel.Save();
          Sw.App.CloseDoc(swModel.GetPathName());
        }

        AtualizarComponente();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao atualizar dados Componente");
      }
    }


    private void AtualizarComponente() {
      try {
        lblPeso.Text = "0,000Kg";
        lblEspess.Text = "";
        lblDescMat.Text = "";
        lblMaterial.Text = "";
        lblProcess.Text = "";

        lblMaterial.UseCustomColor =
        lblDescMat.UseCustomColor =
        lblProcess.UseCustomColor = false;

        txtMaterial.CarregarComboBox(new List<Z_Chapa>());
        txtMaterial.Text = string.Empty;
        txtMaterial.CampoObrigatorio = false;
        ClearCheckBox();
        txtMaterial.Text = string.Empty;
        lblMaterial.UseCustomColor =
        lblDescMat.UseCustomColor =
        lblProcess.UseCustomColor = false;
        var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        if (produtoERP.PathName.ToUpper().EndsWith(".SLDPRT"))
          Sw.App.OpenDoc6(produtoERP.PathName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
        else
          Sw.App.OpenDoc6(produtoERP.PathName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);

        var swModel = (ModelDoc2)Sw.App.ActivateDoc2(Name: produtoERP.PathName, Silent: false, Errors: 0);
        if (swModel == null)
          return;

        swModel.ClearSelection2(true);

        if (produtoERP.ItensCorte != null && produtoERP.ItensCorte.Count == 1 && produtoERP.ItensCorte[0].Tipo == TipoListaMaterial.Chapa)
          ListaCorte.RefreshCutList(swModel, "", produtoERP.ItensCorte[0]);

        if (produtoERP.Referencia.StartsWith("Item da lista de corte")) {
          var swModelDocExt = swModel.Extension;

          bool boolstatus = swModel.Extension.SelectByID2(produtoERP.ItensCorte[0].NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);
        }

        AtualizarInformacoes(produtoERP);
        GetProcess(produtoERP);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Atualizar Dados\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void AtualizarInformacoes(ProdutoErp produtoErp) {
      txtDescricao.Text = produtoErp.Denominacao;
      lblPeso.Text = produtoErp.Massa + " kg";
      if (produtoErp.ItensCorte?.Count == 1 || produtoErp.Referencia.StartsWith("Item da lista de corte")) {
        var espess = produtoErp.ItensCorte[0].CxdEspess;
        var largur = produtoErp.ItensCorte[0].CxdLarg;
        var compri = produtoErp.ItensCorte[0].CxdCompr;
        var descricMaterial = produtoErp.ItensCorte[0].Denominacao;
        var tipo = produtoErp.ItensCorte[0].Tipo;
        var codigo = produtoErp.ItensCorte[0].Codigo;

        lblPeso.Text = produtoErp.ItensCorte[0].Massa + " kg";

        lblEspess.Text = tipo == TipoListaMaterial.Chapa ? $"{espess}x{largur}x{compri}" : $"{compri}";
        lblDescMat.Text = produtoErp.ItensCorte[0].Denominacao;
        lblMaterial.Text = produtoErp.ItensCorte[0].Material;
        lblCodMat.Text = codigo.ToString();

        if (tipo == TipoListaMaterial.Chapa) {
          txtMaterial.CampoObrigatorio = true;
          Z_Chapa materialIdeal = null;

          var list = materia_primas.Selecionar(ativo: true, espessura: espess);
          txtMaterial.CarregarComboBox(list);

          if (list.Count > 0) {
            materialIdeal = list.FirstOrDefault(x => x.DescricaoChapa == produtoErp.ItensCorte[0].Material);
            if (materialIdeal != null) {
              txtMaterial.SelectedValue = materialIdeal.Id;
              txtMaterial.Text = materialIdeal.DescricaoChapa;
            } else {
              materialIdeal = list.FirstOrDefault(x => x.DescricaoChapa == descricMaterial);
              if (materialIdeal != null) {
                txtMaterial.SelectedValue = materialIdeal.Id;
                txtMaterial.Text = materialIdeal.DescricaoChapa;
              }
            }
          }

          var id = (int?)txtMaterial.SelectedValue;
          if (id == null || (materialIdeal != null && materialIdeal.CodigoChapa != (int?)codigo)) {
            lblCodMat.UseCustomColor =
            lblDescMat.UseCustomColor = true;
            Toast.Warning("Atenção:\r\nMateria prima incorreta, selecione a matéria prima correta e clique em salvar.");
          }
        } else {
          txtMaterial.CampoObrigatorio = false;
        }
      }
    }

    private void GetProcess(ProdutoErp produtoErp) {
      try {
        ClearCheckBox();
        string[] procs;

        if (!string.IsNullOrEmpty(produtoErp.Operacao)) {
          procs = produtoErp.Operacao?.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        }
        //else if (produtoErp.ItensCorte.Count > 0)
        //  procs = produtoErp.ItensCorte[0].Operacao?.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        else return;

        if (procs != null) {
          foreach (string prc in procs) {
            flpOperacoes.Controls.OfType<LmCheckBox>().Where(x => x.Tag.ToString() == prc).ToList().ForEach(x => x.Checked = true);
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar Processos\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void TxtComponente_ButtonClickF7(object sender, EventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Info($"Sem documentos abertos");
          return;
        }

        if (dgv.Grid.CurrentRow == null) {
          Toast.Info($"Nenhum produto selecionado");
          return;
        }

        var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING)
          return;

        produtoERP.CodComponente = produtoERP.Name;
        txtComponente.Text = produtoERP.Name;
        txtComponente.BackColor = Color.White;

        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
        swCustPropMgr.Add3("Componente", (int)swCustomInfoType_e.swCustomInfoText, txtComponente.Text, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar componente\n\n{ex.Message}", "Addin LM Projetos",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void TxtComponente_ButtonClickF8(object sender, EventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Info($"Sem documentos abertos");
          return;
        }

        if (dgv.Grid.CurrentRow == null) {
          Toast.Info($"Nenhum produto selecionado");
          return;
        }

        var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          MsgBox.Show($"Comando apenas para Peças e Montagens", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        string partAtual;
        string drawingAtual;

        partAtual = swModel.GetPathName().ToUpper();

        SaveFileDialog sfd = new SaveFileDialog {
          AddExtension = true,
          RestoreDirectory = true,
          Title = "Salvar Como",
          FileName = txtComponente.Text
        };

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
          sfd.Filter = "Peça (*.sldprt) | *.sldprt";
          drawingAtual = partAtual.Replace(".SLDPRT", ".SLDDRW");
        } else {
          sfd.Filter = "Montagem (*.sldasm) | *.sldasm";
          drawingAtual = partAtual.Replace(".SLDASM", ".SLDDRW");
        }
        if (sfd.ShowDialog() == DialogResult.OK) {
          MsgBox.ShowWaitMessage("Salvando Aguarde..");

          if (!string.IsNullOrEmpty(sfd.FileName)) {
            string drawingNew;
            string partNew = sfd.FileName;

            drawingNew = partNew.Substring(0, partNew.Length - 6) + "SLDDRW";

            int status = 0;
            int errors = 0;
            int warnings = 0;

            swModel.Extension.RunCommand(2732, "");

            if (File.Exists(drawingAtual)) {
              Sw.App.OpenDoc6(drawingAtual, (int)swDocumentTypes_e.swDocDRAWING,
                  (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

              Sw.App.ActivateDoc2(drawingAtual, false, 0);

              Sw.App.ActivateDoc2(partAtual, false, 0);
              swModel = (ModelDoc2)Sw.App.ActiveDoc;

              swModel.SaveAs4(partNew, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref errors, ref warnings);
              //if(swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
              //  swModel.Extension.RunCommand(2732, "");

              Sw.App.ActivateDoc2(drawingAtual, false, 0);
              swModel = (ModelDoc2)Sw.App.ActiveDoc;
              swModel.SaveAs(drawingNew);

              Sw.App.CloseDoc(drawingNew);
            } else swModel.SaveAs4(partNew, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref errors, ref warnings);

            produtoERP.PathName = partNew;

            produtoERP.Name = Path.GetFileNameWithoutExtension(partNew);
            produtoERP.CodComponente = produtoERP.Name;

            txtComponente.Text = produtoERP.Name;
            txtComponente.BackColor = Color.White;

            dgv.Refresh();

            if (MsgBox.Show("Deseja Eliminar Antigo e Manter Somente Novo?", "Excluir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
              File.Delete(partAtual);
              if (File.Exists(drawingAtual))
                File.Delete(drawingAtual);
            }
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao salvar cópia do componente\n\n{ex.Message}", "Addin LM Projetos",
              MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void TxtDenominacao_Leave(object sender, EventArgs e) {
      if (dgv.Grid.CurrentRow == null) {
        return;
      }

      var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;
      if (produtoERP != null)
        produtoERP.Denominacao = txtDescricao.Text;
    }

    private void ClearCheckBox() {
      flpOperacoes.Controls.OfType<LmCheckBox>().Where(x => x.Checked).ToList().ForEach(x => x.Checked = false);
    }

    private void Ckb_CheckedChanged(object sender, EventArgs e) {
      if (dgv.Grid.CurrentRow == null) {
        Toast.Info($"Nenhum produto selecionado");
        return;
      }

      var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

      string p = ((LmCheckBox)sender).Tag.ToString();
      // var p = ((LmCheckBox)sender).Tag as Processo;

      if (((LmCheckBox)sender).Checked == true) {
        lblProcess.Text += !string.IsNullOrEmpty(lblProcess.Text) ? $"/{p}" : p;

        ((LmCheckBox)sender).BackColor = Color.SpringGreen;
      } else {
        lblProcess.Text = lblProcess.Text.Replace(p, "").Replace("//", "/");

        if (lblProcess.Text.StartsWith("/"))
          lblProcess.Text = lblProcess.Text.Substring(1, lblProcess.Text.Length - 1);
        if (lblProcess.Text.EndsWith("/"))
          lblProcess.Text = lblProcess.Text.Substring(0, lblProcess.Text.Length - 1);

        ((LmCheckBox)sender).BackColor = Color.Transparent;
      }

      lblProcess.UseCustomColor = produtoERP.Operacao != lblProcess.Text;
    }

    private void CkbAddDenom_CheckedChanged(object sender, EventArgs e) {
      InfoSetting.AddDenominacaoTodasConfig = ckbAddDenom.Checked;
      InfoSetting.Salvar();
    }

    private void TmrInicioLocalizar_Tick(object sender, EventArgs e) {
      ((System.Windows.Forms.Timer)sender).Tag = Convert.ToInt32(((System.Windows.Forms.Timer)sender).Tag) + 1;
      if (Convert.ToInt32(((System.Windows.Forms.Timer)sender).Tag) > 5)
        IniciarLocalizar();
    }

    private void TxtProcurar_TextChanged(object sender, EventArgs e) {
      if (tmrInicioLocalizar.Enabled == false)
        tmrInicioLocalizar.Enabled = true;
      else
        tmrInicioLocalizar.Tag = 0;
    }

    private void IniciarLocalizar() {
      tmrInicioLocalizar.Tag = 0;
      tmrInicioLocalizar.Enabled = false;

      try {
        flpOperacoes.Controls.OfType<LmCheckBox>().ToList()
        .ForEach(x => {
          x.Visible = x.Checked || x.Text.ToLower().RemoverCaracteresEspeciais().Contains(txtProcurar.Text.ToLower().RemoverCaracteresEspeciais());
        });

      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Filtrar Operações");
      }
    }

    private void FlpProcess_SizeChanged(object sender, EventArgs e) {
      try {
        flpOperacoes.Controls.OfType<LmCheckBox>().ToList()
        .ForEach(x => {
          x.Size = new Size(flpOperacoes.Width - 30, 19);
        });
      } catch (Exception ex) {

      }
    }

    private void AdicionarDescricaoTodasConfiguracoes() {
      try {
        if (dgv.Grid.CurrentRow == null) {
          Toast.Info($"Nenhum produto selecionado");
          return;
        }

        var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        object[] configNameArr = null;
        string configName = null;
        bool status = false;
        int i = 0;
        int h = 0;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        ConfigurationManager swConfMgr = swModel.ConfigurationManager;
        var swModelDocExt = swModel.Extension;
        configNameArr = (object[])swModel.GetConfigurationNames();

        string filename = swModel.GetPathName();

        configName = swConfMgr.ActiveConfiguration.Name;
        string defConfig = configName;

        if (ckbAddDenom.Checked) {
          for (i = 0; i <= configNameArr.GetUpperBound(0); i++) {
            configName = (string)configNameArr[i];
            status = swModel.ShowConfiguration2(configName);

            swModelDocExt = swModel.Extension;
            var swCustPropMngr = swModelDocExt.get_CustomPropertyManager(configName);

            swCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, produtoERP.Denominacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
          }
        } else {
          swModelDocExt = swModel.Extension;
          var swCustPropMngr = swModelDocExt.get_CustomPropertyManager(defConfig);

          swCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, produtoERP.Denominacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        }
        status = swModel.ShowConfiguration2(defConfig);
      } catch (Exception ex) {
        MessageBox.Show("Falha ao Atualizar Denominação: \n" + ex.Message);
      }
    }

  }
}
