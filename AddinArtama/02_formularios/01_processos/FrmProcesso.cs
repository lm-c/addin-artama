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
    int _posAtualItemCorte = 0;
    Componente _componente = new Componente();
    SortableBindingList<W_Componente> _componentes = new SortableBindingList<W_Componente>();

    public FrmProcesso() {
      InitializeComponent();

      CarregarControlesProcessos();
    }

    private void FrmProcesso_Loaded(object sender, EventArgs e) {
      Invoke(new MethodInvoker(delegate () {
        //dadosMateriais.DataSource = ChapaDATA.Selecionar();
        ckbAddDenom.Checked = InfoSetting.AddDenominacaoTodasConfig;
      }));
    }

    private void CarregarControlesProcessos() {
      try {
        foreach (var proc in Processo.ListaProcessos) {
          LmCheckBox ckb = new LmCheckBox {
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Color.Transparent,
            Size = new Size(flpOperacoes.Width - 30, 19),
            Margin = new Padding(6, 1, 6, 1),
            Name = $"ckb{proc.codOperacao}",
            Tag = $"{proc.codOperacao}",
            Text = $"{proc.codOperacao} - {proc.descricao}",
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
          _componentes = new SortableBindingList<W_Componente>();
          dgv.Grid.DataSource = _componentes;

          _montagemPrincipal = swModel.GetPathName().ToLower();

          _componentes = W_Componente.GetComponentes(swModel);
          dgv.CarregarGrid(_componentes);
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
          MsgBox.Show($"Sem documentos abertos", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        if (Controles.PossuiCamposInvalidos(pnlDados))
          return;

        MsgBox.ShowWaitMessage("Salvando. Aguarde...");

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        _componente.Denominacao = txtDescricao.Text;

        AdicionarDescricaoTodasConfiguracoes();

        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        swCustPropMgr.Add3("Componente", (int)swCustomInfoType_e.swCustomInfoText,
            _componente.CompCodInterno, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        swCustPropMgr.Add3("Massa", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
          swCustPropMgr.Add3("Material", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Material\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        } else
          swCustPropMgr.Delete("Material");

        if (ckbInterno.Checked) {
          _componente.Interno = true;
          swCustPropMgr.Add3("Interno", (int)swCustomInfoType_e.swCustomInfoText, "Sim", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        }

        if (_componente.ItensCorte.Count > 0) {
          ListaCorte.AtualizarListaCorte(swModel, _componente.ItensCorte[_posAtualItemCorte]);
        } else {
          swCustPropMgr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText, _componente.Operacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        }

        swModel.Save();

        lblMaterial.UseCustomColor =
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
          if (_componentes.Count == 0) {
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
        if (_componentes.Count == 0) {
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

        if (swModel != null && swModel.GetPathName().ToLower() != _montagemPrincipal) {
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
        lblListaCorte.Text = "Nome Lista Corte - 0 de 0";
        lblPeso.Text = "0,000Kg";
        lblEspess.Text = "";
        lblDescMat.Text = "";
        lblMaterial.Text = "";
        lblProcess.Text = "";

        txtMaterial.CarregarComboBox(new List<Z_Chapa>());
        txtMaterial.Text = string.Empty;
        txtMaterial.CampoObrigatorio = false;
        ClearCheckBox();
        txtMaterial.Text = string.Empty;
        lblDescMat.UseCustomColor =
        lblMaterial.UseCustomColor = false;
        var w_componente = dgv.Grid.CurrentRow.DataBoundItem as W_Componente;
        _componente = new Componente();
        _posAtualItemCorte = 0;

        if (w_componente.PathName.ToUpper().EndsWith(".SLDPRT"))
          Sw.App.OpenDoc6(w_componente.PathName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
        else
          Sw.App.OpenDoc6(w_componente.PathName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);

        var swModel = (ModelDoc2)Sw.App.ActivateDoc2(Name: w_componente.PathName, Silent: false, Errors: 0);
        if (swModel == null)
          return;

        swModel.ClearSelection2(true);

        _componente = Componente.GetComponente(swModel);

        AtualizarInformacoes();
        GetProcess();
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Atualizar Dados\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void BtnUpDown_Click(object sender, EventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocPART || _componente.ItensCorte.Count == 0)
          return;

        if (_componente.ItensCorte.Count > 0) {
          if (((Button)sender).Tag.ToString() == "Up")
            _posAtualItemCorte = _posAtualItemCorte < _componente.ItensCorte.Count - 1 ? _posAtualItemCorte + 1 : 0;
          if (((Button)sender).Tag.ToString() == "Down")
            _posAtualItemCorte = _posAtualItemCorte > 0 ? _posAtualItemCorte - 1 : _componente.ItensCorte.Count - 1;
        }

        AtualizarInformacoes();
      } catch (Exception ex) {
        MsgBox.Show($"Erro \n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void AtualizarInformacoes() {
      if (_componente.ItensCorte?.Count > 0) {
        var espess = _componente.ItensCorte[_posAtualItemCorte].CxdEspess;
        var descricMaterial = _componente.ItensCorte[_posAtualItemCorte].Denominacao;
        var tipo = _componente.ItensCorte[_posAtualItemCorte].Tipo;

        lblListaCorte.Text = $"{_componente.ItensCorte[_posAtualItemCorte].NomeLista} - {(_posAtualItemCorte + 1) + " de " + _componente.ItensCorte.Count}";
        lblPeso.Text = _componente.ItensCorte[_posAtualItemCorte].Massa + " kg";
        lblEspess.Text = espess + "x" +
            _componente.ItensCorte[_posAtualItemCorte].CxdLarg + "x" +
            _componente.ItensCorte[_posAtualItemCorte].Comprimento;
        lblDescMat.Text = _componente.ItensCorte[_posAtualItemCorte].Denominacao;
        lblMaterial.Text = _componente.ItensCorte[_posAtualItemCorte].Material;
        lblCodMat.Text = _componente.ItensCorte[_posAtualItemCorte].Codigo.ToString();

        txtDescricao.Text = _componente.Denominacao;

        if (tipo == TipoListaMaterial.Chapa) {
          txtMaterial.CampoObrigatorio = true;

          var list = materia_primas.Selecionar(ativo: true, espessura: espess);
          txtMaterial.CarregarComboBox(list);

          if (list.Count > 0) {
            var mat = list.FirstOrDefault(x => x.DescricaoChapa == _componente.ItensCorte[_posAtualItemCorte].Material);
            if (mat != null) {
              txtMaterial.SelectedValue = mat.Id;
              txtMaterial.Text = mat.DescricaoChapa;
            } else {
              mat = list.FirstOrDefault(x => x.DescricaoChapa == descricMaterial);
              if (mat != null) {
                txtMaterial.SelectedValue = mat.Id;
                txtMaterial.Text = mat.DescricaoChapa;
              }
            }
          } else if (list.Count == 1) {
            txtMaterial.SelectedValue = list[0].Id;
            txtMaterial.Text = list[0].DescricaoChapa;
          }

          var id = (int?)txtMaterial.SelectedValue;
          var mat2 = list.FirstOrDefault(x => x.DescricaoChapa == descricMaterial);
          if (mat2 != null && mat2.Id != (int?)txtMaterial.SelectedValue) {
            lblDescMat.UseCustomColor = true;
          }
        } else {
          txtMaterial.CampoObrigatorio = false;
        }
      } else {
        txtDescricao.Text = _componente.Denominacao;
        lblPeso.Text = _componente.Massa + " kg";
      }
    }

    private void GetProcess() {
      try {
        ClearCheckBox();
        string[] procs;

        if (!string.IsNullOrEmpty(_componente.Operacao)) {
          procs = _componente.Operacao?.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        } else if (_componente.ItensCorte.Count > 0)
          procs = _componente.ItensCorte[_posAtualItemCorte].Operacao?.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
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
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING || _componentes.Count == 0)
          return;

        _componente.CompCodInterno = _componente.ShortName;
        txtComponente.Text = _componente.ShortName;
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
          MsgBox.Show($"Sem documentos abertos", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          MsgBox.Show($"Comando apenas para Peças e Montagens", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        string PartAtual;
        string DrawingAtual;

        if (_componentes.Count > 0)
          PartAtual = _componente.LongName;
        else
          PartAtual = swModel.GetPathName().ToUpper();


        SaveFileDialog sfd = new SaveFileDialog {
          AddExtension = true,
          RestoreDirectory = true,
          Title = "Salvar Como",
          FileName = txtComponente.Text
        };

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
          sfd.Filter = "Peça (*.sldprt) | *.sldprt";
          DrawingAtual = PartAtual.Replace(".SLDPRT", ".SLDDRW");
        } else {
          sfd.Filter = "Montagem (*.sldasm) | *.sldasm";
          DrawingAtual = PartAtual.Replace(".SLDASM", ".SLDDRW");
        }

        if (sfd.ShowDialog() == DialogResult.OK) {
          MsgBox.ShowWaitMessage("Salvando Aguarde..");

          if (!string.IsNullOrEmpty(sfd.FileName)) {
            string drawingNew;
            string partNew = sfd.FileName.ToUpper();

            if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART)
              drawingNew = partNew.Replace(".SLDPRT", ".SLDDRW");
            else
              drawingNew = partNew.Replace(".SLDASM", ".SLDDRW");

            if (_componentes.Count > 0) {
              _componente.LongName = partNew;

              _componente.ShortName = Path.GetFileNameWithoutExtension(partNew);
              _componente.CompCodInterno = _componente.ShortName;

              txtComponente.Text = _componente.ShortName;
              txtComponente.BackColor = Color.White;
            }

            int status = 0;
            int warnings = 0;

            if (File.Exists(DrawingAtual)) {
              Sw.App.OpenDoc6(DrawingAtual, (int)swDocumentTypes_e.swDocDRAWING,
                  (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

              Sw.App.ActivateDoc2(DrawingAtual, false, 0);

              Sw.App.ActivateDoc2(PartAtual, false, 0);
              swModel = (ModelDoc2)Sw.App.ActiveDoc;
              swModel.SaveAs(partNew);

              Sw.App.ActivateDoc2(DrawingAtual, false, 0);
              swModel = (ModelDoc2)Sw.App.ActiveDoc;
              swModel.SaveAs(drawingNew);

              Sw.App.CloseDoc(drawingNew);
            } else swModel.SaveAs(partNew);

            if (MsgBox.Show("Deseja Eliminar Antigo e Manter Somente Novo???", "Excluir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
              File.Delete(PartAtual);
              if (File.Exists(DrawingAtual))
                File.Delete(DrawingAtual);
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

    private void ckbInterno_CheckedChanged(object sender, EventArgs e) {
      //if (ckbInterno.Checked == true && ckbSeriado.Checked == true)
      //  ckbSeriado.Checked = false;
    }

    private void TxtDenominacao_Leave(object sender, EventArgs e) {
      if (_componente != null)
        _componente.Denominacao = txtDescricao.Text;
    }

    private void ClearCheckBox() {
      flpOperacoes.Controls.OfType<LmCheckBox>().Where(x => x.Checked).ToList().ForEach(x => x.Checked = false);
    }

    private void Ckb_CheckedChanged(object sender, EventArgs e) {
      if (_componentes.Count == 0) return;

      string p = ((LmCheckBox)sender).Tag.ToString();

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

      lblProcess.UseCustomColor = false;

      if (!string.IsNullOrEmpty(_componente.Operacao)) {
        _componente.Operacao = lblProcess.Text;
        lblProcess.UseCustomColor = _componente.Operacao != _componente.OperacaoOrigem;
      } else if (_componente.ItensCorte.Count > 0) {
        _componente.ItensCorte[_posAtualItemCorte].Operacao = lblProcess.Text;
        foreach (var item in _componente.ItensCorte) {
          if (item.Operacao != item.OperacaoOrigem) {
            lblProcess.UseCustomColor = true;
            break;
          }
        }
      }
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

    private void TxtMaterial_SelectedValueChanched(object sender, EventArgs e) {
      try {
        if (txtMaterial.SelectedValue != null) {
          var id = (int)txtMaterial.SelectedValue;
          var mat = materia_primas.Selecionar(id);
          if (mat != null && _componente.ItensCorte[_posAtualItemCorte] != null) {
            if (mat.DescricaoChapa != _componente.ItensCorte[_posAtualItemCorte].Denominacao) {
              lblDescMat.UseCustomColor = true;
            } else {
              lblDescMat.UseCustomColor = false;
            }
            if (mat.DescricaoMaterial != _componente.ItensCorte[_posAtualItemCorte].Material) {
              lblMaterial.UseCustomColor = true;
            } else {
              lblMaterial.UseCustomColor = false;
            }
          }
        } else {
          lblDescMat.UseCustomColor = false;
          lblMaterial.UseCustomColor = false;
        }
        pnlDados.Refresh();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ");
      }
    }

    private void AdicionarDescricaoTodasConfiguracoes() {
      try {
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

            swCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, _componente.Denominacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
          }
        } else {
          swModelDocExt = swModel.Extension;
          var swCustPropMngr = swModelDocExt.get_CustomPropertyManager(defConfig);

          swCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, _componente.Denominacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        }
        status = swModel.ShowConfiguration2(defConfig);
      } catch (Exception ex) {
        MessageBox.Show("Falha ao Atualizar Denominação: \n" + ex.Message);
      }
    }

  }
}
