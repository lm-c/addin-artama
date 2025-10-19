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
using System.Threading.Tasks;

namespace AddinArtama {
  public partial class FrmMateriaPrimaApl : LmSingleForm {
    string _montageGeralNome = string.Empty;
    SortableBindingList<ProdutoErp> _produtos = new SortableBindingList<ProdutoErp>();

    public FrmMateriaPrimaApl() {
      InitializeComponent();

      _produtos = new SortableBindingList<ProdutoErp>();
      dgv.MontarGrid<ProdutoErp>();

      dgv.ColunasBloqueadasGrid = "ImgFantasma^Nivel^CodProduto^Referencia^Configuracao^Quantidade^UnidadeMedida";

      CarregarControlesProcessos();
    }

    private void FrmProcesso_Loaded(object sender, EventArgs e) {
      Invoke(new MethodInvoker(delegate () {
        //dadosMateriais.DataSource = ChapaDATA.Selecionar();
        //ckbAddDenom.Checked = InfoSetting.AddDenominacaoTodasConfig;
      }));
    }

    internal void CarregarControlesProcessos() {
      try {
        flpOperacoes.Controls.Clear();
        lblProcess.Text = string.Empty;

        foreach (var proc in ProcessoNaoSeriado.ListaProcessos) {
          LmCheckBox ckb = new LmCheckBox {
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Color.Transparent,
            Size = new Size(flpOperacoes.Width - 30, 19),
            Margin = new Padding(6, 1, 6, 1),
            Name = $"ckb{proc.Codigo}",
            Tag = $"{proc.Codigo}",
            Text = $"{proc.Codigo} - {proc.Descricao}",
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
      CarregarProdutos();
    }

    private async Task CarregarProdutos() {
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
          _produtos = new SortableBindingList<ProdutoErp>();

          _montageGeralNome = Path.GetFileNameWithoutExtension(swModel.GetPathName());

          if (_montageGeralNome.Length > 50)
            _montageGeralNome = _montageGeralNome.Substring(0, 50);

          await Loader.ShowDuringOperation(async (progress) => {
            progress.Report("Iniciando leitura...");
            _produtos = await ProcessoNaoSeriado.GetProdutos();
          });

          CarregarGrid();
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

        var ops = lblProcess.Text;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        AdicionarDescricaoTodasConfiguracoes();

        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
          swCustPropMgr.Add3("Material", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Material\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        } else
          swCustPropMgr.Delete2("Material");

        if (produtoERP.TipoComponente == TipoComponente.Peca && produtoERP.ItemCorte != null) {
          produtoERP.ItemCorte.Operacao = ops;
          if (txtMaterial.SelectedValue != null) {
            var idMat = (int)txtMaterial.SelectedValue;
            var mat = materia_primas.Selecionar(idMat);
            if (mat != null) {
              produtoERP.ItemCorte.Codigo = mat.CodigoChapa;
              produtoERP.ItemCorte.Denominacao = mat.DescricaoChapa;
            }
          }
          ListaCorte.UpdateCutList(swModel, produtoERP.ItemCorte);
        } else {
          swCustPropMgr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText, ops, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        }

        var spl = ops.Split('/');
        produtoERP.Operacoes = new List<produto_erp_operacao>();
        foreach (var item in spl) {
          if (string.IsNullOrEmpty(item))
            continue;
          produtoERP.Operacoes.Add(new produto_erp_operacao() {
            processo_id = Convert.ToInt32(item),
          });

        }

        swModel.Save();

        if (produtoERP.Operacoes.Count() > 0) {
          produtoERP.Pendencias = new List<PendenciasEngenharia>();
          produtoERP.ImgPendencia = new Bitmap(20, 20);
          dgv.Grid.CurrentRow.DefaultCellStyle.ForeColor = Color.Black;
        } else {
          //ProdutoErp.AdicionarPendencia(produtoERP, PendenciasEngenharia.OperacaoNaoPossui);
          //produtoERP.ImgPendencia = Properties.Resources.error;
          //dgv.Grid.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
        }

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
          else {
            Toast.Info($"Você está no primeiro componente");
            SalvarFechar(swModel);
          }
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
        else {
          Toast.Info($"Você já chegou no último componente");
          SalvarFechar(swModel);
          dgv.Grid.Rows[0].Cells[1].Selected = true;
        }
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

        SalvarFechar(swModel);

        AtualizarComponente();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao atualizar dados Componente");
      }
    }

    private void SalvarFechar(ModelDoc2 swModel) {
      var name = Path.GetFileNameWithoutExtension(swModel.GetPathName());
      if (swModel != null && name != _montageGeralNome) {
        swModel.ShowNamedView("*Isométrica");
        swModel.ViewZoomtofit();

        if (!swModel.IsOpenedReadOnly())
          swModel.Save();
        else Toast.Warning($"Não salvo.\r\n{name} era somente leitura.");

        Sw.App.CloseDoc(swModel.GetPathName());
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

        if (produtoERP.ItemCorte != null && produtoERP.ItemCorte.Tipo == TipoListaMaterial.Chapa)
          ListaCorte.RefreshCutList(swModel, "", produtoERP.ItemCorte);

        if (produtoERP.Referencia.StartsWith("Item da lista de corte")) {
          var swModelDocExt = swModel.Extension;

          bool boolstatus = swModel.Extension.SelectByID2(produtoERP.ItemCorte.NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);
        }

        AtualizarInformacoes(produtoERP);
        GetProcess(produtoERP);
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Atualizar Dados\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void AtualizarInformacoes(ProdutoErp produtoErp) {
      txtComponente.Text = produtoErp.CodComponente;
      txtDescricao.Text = produtoErp.Denominacao;
      lblPeso.Text = produtoErp.PesoLiquido + " kg";
      if (produtoErp.ItemCorte != null || produtoErp.Referencia.StartsWith("Item da lista de corte")) {
        var espess = produtoErp.ItemCorte.Espessura;
        var largur = produtoErp.ItemCorte.Largura;
        var compri = produtoErp.ItemCorte.Comprimento;
        var descricMaterial = produtoErp.ItemCorte.Denominacao;
        var tipo = produtoErp.ItemCorte.Tipo;
        var codigo = produtoErp.ItemCorte.Codigo;

        lblPeso.Text = produtoErp.ItemCorte.Massa + " kg";

        lblEspess.Text = tipo == TipoListaMaterial.Chapa ? $"{espess}x{largur}x{compri}" : $"{compri}";
        lblDescMat.Text = produtoErp.ItemCorte.Denominacao;
        lblMaterial.Text = produtoErp.ItemCorte.Material;
        lblCodMat.Text = codigo.ToString();

        if (tipo == TipoListaMaterial.Chapa) {
          txtMaterial.CampoObrigatorio = true;
          Z_Chapa materialIdeal = null;

          var list = materia_primas.Selecionar(ativo: true, espessura: espess);
          txtMaterial.CarregarComboBox(list);

          if (list.Count > 0) {
            materialIdeal = list.FirstOrDefault(x => x.DescricaoChapa == produtoErp.ItemCorte.Material);
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

        if (produtoErp.Pendencias.Count > 0) {
          Toast.Warning(produtoErp.Pendencias[0].ObterDescricaoEnum());
        }
      }
    }

    private void GetProcess(ProdutoErp produtoErp) {
      try {
        ClearCheckBox();
        foreach (var operacao in produtoErp.Operacoes) {
          flpOperacoes.Controls.OfType<LmCheckBox>().Where(x => x.Tag.ToString() == operacao.processo_id.ToString()).ToList().ForEach(x => x.Checked = true);
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
        txtComponente.BackColor = Color.FromArgb(234, 238, 242);

        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");
        swCustPropMgr.Add3("Componente", (int)swCustomInfoType_e.swCustomInfoText, txtComponente.Text, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar componente\n\n{ex.Message}", "Addin LM Projetos",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void TxtComponente_ButtonClickF8(object sender, EventArgs e) {
      SalvarComponenteAsync();
    }

    private async Task SalvarComponenteAsync() {
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
          if (!string.IsNullOrEmpty(sfd.FileName)) {
            string drawingNew;
            string partNew = sfd.FileName;

            drawingNew = partNew.Substring(0, partNew.Length - 6) + "SLDDRW";

            int status = 0;
            int errors = 0;
            int warnings = 0;

            swModel.Extension.RunCommand(2732, "");
            await Loader.ShowDuringOperation(async (progress) => {
              progress.Report("Salvando Aguarde...");

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
            });

            produtoERP.PathName = partNew;

            produtoERP.Name = Path.GetFileNameWithoutExtension(partNew);
            produtoERP.CodComponente = produtoERP.Name;

            txtComponente.Text = produtoERP.Name;
            txtComponente.BackColor = Color.FromArgb(234, 238, 242);

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

    private void TxtComponente_TextChanged(object sender, EventArgs e) {
      if (dgv.Grid.CurrentRow == null) {
        return;
      }

      var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;
      if (produtoERP != null && !produtoERP.Name.StartsWith(txtComponente.Text))
        txtComponente.BackColor = Color.FromArgb(255, 118, 108);
      else
        txtComponente.BackColor = Color.FromArgb(234, 238, 242);
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

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        string filename = swModel.GetPathName();

        var swModelDocExt = swModel.Extension;
        var swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");

        swCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, produtoERP.Denominacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
      } catch (Exception ex) {
        MessageBox.Show("Falha ao Atualizar Denominação: \n" + ex.Message);
      }
    }

    private void CarregarGrid() {
      dgv.CarregarGrid(_produtos);

      try {
        using (ContextoDados db = new ContextoDados()) {
          System.Collections.IList list = dgv.Grid.Rows;
          for (int i = 0; i < list.Count; i++) {
            DataGridViewRow row = (DataGridViewRow)list[i];
            var produtoErp = row.DataBoundItem as ProdutoErp;

            //if (produtoErp.Operacoes != null && produtoErp.Operacoes.Count == 0 && produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
            //  ProdutoErp.AdicionarPendencia(produtoErp, PendenciasEngenharia.OperacaoNaoPossui);
            //  row.DefaultCellStyle.ForeColor = Color.Red;
            //}
          }
        }
      } catch (Exception ex) {
        Toast.Error("Erro ao formatar cores grid. \r\n" + ex.Message);
      } finally { MsgBox.CloseWaitMessage(); }
    }
  }
}
