using System;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using System.Linq;
using System.Drawing;
using LmCorbieUI.Metodos;
using LmCorbieUI.Design;
using System.IO;
using System.Web.UI.WebControls.WebParts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using static AddinArtama.Api;

namespace AddinArtama {
  public partial class FrmProdutoImport : LmSingleForm {
    SortableBindingList<ProdutoErp> _dadosDraw = new SortableBindingList<ProdutoErp>();

    Color corErro = Color.Red;
    Color corAlerta = Color.LightGoldenrodYellow;
    Color corSucesso = Color.Green;

    public FrmProdutoImport() {
      InitializeComponent();

      _dadosDraw = new SortableBindingList<ProdutoErp>();

      tbcProduto.SelectedIndex = 0;

      dgv.MontarGrid<ProdutoErp>();

    }

    private void FrmProdutoImport_Loaded(object sender, EventArgs e) {
      Invoke(new MethodInvoker(() => {

      }));
    }

    private void BtnCarrProcess_Click(object sender, EventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {

          _dadosDraw = ProdutoErp.GetComponents(trvProduto);
          CarregarGrid();

          //TreeComponent.GetComponents(trvProduto);
          //trvProduto.TopNode = trvProduto.Nodes[0];
          //TrvProduto_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(trvProduto.Nodes[0], MouseButtons.Left, 1, 0, 0));
        } else {
          Toast.Warning("Comando apenas para Peças e Montagens.");
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Carregar Componentes..\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void BtnImportar_Click(object sender, EventArgs e) {
      if (_dadosDraw.Count == 0) {
        Toast.Warning("Carregar Desenhos primeiro");
        return;
      }

      MsgBox.ShowWaitMessage("Cadastrando Produtos...");
      btnCancel.Enabled = true;
      btnCarrProcess.Enabled = btnImportar.Enabled = !btnCancel.Enabled;

      System.Threading.Thread t = new System.Threading.Thread(() => { CadastrarNovo(); }) { IsBackground = true };
      t.Start();
    }

    private void CadastrarNovo() {
      try {
        Invoke(new MethodInvoker(async () => {

          using (ContextoDados db = new ContextoDados()) {
            ModelDoc2 swModel = default(ModelDoc2);

            int status = 0;
            int warnings = 0;

            for (int index = 0; index < _dadosDraw.Count; index++) {
              if (btnCancel.Enabled == false) {
                Toast.Info("Rotina Cancelada pelo usuário");
                MsgBox.CloseWaitMessage();
                return;
              }

              ProdutoErp item = _dadosDraw[index];
              var config = db.configuracao_api.FirstOrDefault();

              dgv.Grid.Rows[index].Cells[1].Selected = true;

              int tipo = item.PathName.EndsWith("SLDASM")
              ? (int)swDocumentTypes_e.swDocASSEMBLY
              : (int)swDocumentTypes_e.swDocPART;

              if (item.CadastrarErp) {
                var itemGenerico = new Api.ItemGenerico();
                var name = item.Name;

                if (item.CodComponente.StartsWith("10") || item.CodComponente.StartsWith("20") || item.CodComponente.StartsWith("40")) {
                  name = item.Denominacao.Length + item.CodComponente.Length + 3 > 60
                      ? $"{item.Denominacao.Replace("\"", "").Substring(0, item.Denominacao.Length - item.CodComponente.Length - 3)} - {item.CodComponente}"
                      : $"{item.Denominacao.Replace("\"", "")} - {item.CodComponente}";
                } else {
                  name = item.Denominacao.Length + item.Name.Length + 3 > 60
                      ? $"{item.Denominacao.Replace("\"", "").Substring(0, item.Denominacao.Length - item.Name.Length - 3)} - {item.Name}"
                      : $"{item.Denominacao.Replace("\"", "")} - {item.Name}";
                }

                itemGenerico.Nome = name;
                itemGenerico.Tipo = item.TipoComponente == TipoComponente.Montagem || item.ItensCorte.Count > 1 ? TipoDucumento.Montagem : TipoDucumento.Peca;
                itemGenerico.UnidadeMedida = item.TipoComponente == TipoComponente.Peca
                ? "PC"
                : "CJ";

                var codigoNovo = await Api.CadasterItemGenericoAsync(itemGenerico);

                if (!string.IsNullOrEmpty(codigoNovo)) {
                  item.CodProduto = codigoNovo;
                  if (item.CadastrarAddin) {
                    CadastrarAddin(db, item, codigoNovo);
                  }
                } else {
                  BtnCancel_Click(null, null);
                  return;
                }
              } else {
                if (item.CadastrarAddin && !string.IsNullOrEmpty(item.CodProduto)) {
                  CadastrarAddin(db, item, item.CodProduto);
                }
              }

              if (item.CadastrarAddin || item.CadastrarErp) {
                swModel = Sw.App.OpenDoc6(item.PathName, tipo,
                  (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

                var swModelDocExt = swModel.Extension;
                var swCustPropMgr = swModelDocExt.get_CustomPropertyManager(item.Configuracao);

                if (item.Referencia.StartsWith("Item da lista de corte")) {
                  item.ItensCorte[0].CodProduto = item.CodProduto;
                  bool boolstatus = swModel.Extension.SelectByID2(item.ItensCorte[0].NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

                  SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
                  Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
                  swCustPropMgr = swFeat.CustomPropertyManager;
                }

                swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, item.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                swModel.Save();

                if (index > 0)
                  Sw.App.CloseDoc(item.PathName);
              }

              DataGridViewRow row = dgv.Grid.Rows[index];

              row.DefaultCellStyle.ForeColor = row.DefaultCellStyle.SelectionForeColor = corSucesso;
              item.CadastrarErp = item.CadastrarAddin = false;
            }

            MsgBox.ShowWaitMessage("Salvando Todos...");

            swModel = (ModelDoc2)Sw.App.ActiveDoc;

            swModel.Save3(5, ref status, ref warnings);

            BtnCancel_Click(null, null);

            MsgBox.Show("Cadastro de produtos finalizado com sucesso", "Addin LM Projetos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        }));
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar tempalte\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
        //CarregarGrid();
      }
    }

    private static void CadastrarAddin(ContextoDados db, ProdutoErp item, string codigoNovo) {
      try {
        var produtoERP = new produto_erp {
          codigo_produto = Convert.ToInt64(codigoNovo),
          name = item.Name,
          descricao = item.Denominacao,
          codigo_componente = Convert.ToInt64(item.CodComponente),
          pathname = item.PathName,
          referencia = item.Referencia,
          configuracao = item.Configuracao,
        };

        db.produto_erp.Add(produtoERP);
        db.SaveChanges();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao cadastrar produto no LM Connect addin");
      }
    }

    private void BtnCancel_Click(object sender, EventArgs e) {
      btnCancel.Enabled = false;
      btnCarrProcess.Enabled = btnImportar.Enabled = !btnCancel.Enabled;
    }

    private void lmButton1_Click(object sender, EventArgs e) {
      System.Threading.Thread t = new System.Threading.Thread(() => { ExcluirTudo(); }) { IsBackground = true };
      t.Start();
    }

    private void ExcluirTudo() {
      try {
        Invoke(new MethodInvoker(async () => {
          long codReduz = 305011152;

          MsgBox.ShowWaitMessage("Excluindo Itens Genéricos...");
          while (codReduz < 305012000) {

          lmButton1.Text = $"Excluindo {codReduz}";
          lmButton1.Refresh();
            //await Api.GetOpsAsync();
            var excluido = await Api.ExcludeItemGenericoAsync(codReduz);

            codReduz++;
          }
          MsgBox.CloseWaitMessage();
          Toast.Success("Excluido com sucesso");
        }));
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar tempalte\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
        //CarregarGrid();
      }
    }

    private void Dgv_ProcurarTextChanged(object sender, EventArgs e) {
      CarregarGrid();
    }

    private void CarregarGrid() {
      dgv.CarregarGrid(_dadosDraw);

      try {
        MsgBox.ShowWaitMessage("Analisando Componentes...");

        using (ContextoDados db = new ContextoDados()) {
          System.Collections.IList list = dgv.Grid.Rows;
          for (int i = 0; i < list.Count; i++) {
            DataGridViewRow row = (DataGridViewRow)list[i];
            var item = row.DataBoundItem as ProdutoErp;

            if (string.IsNullOrEmpty(item.CodProduto)) {
              var prod = db.produto_erp.FirstOrDefault(x => x.name == item.Name && x.referencia == item.Referencia && x.configuracao == item.Configuracao);
              if (prod != null) {
                row.DefaultCellStyle.ForeColor = row.DefaultCellStyle.SelectionForeColor = corSucesso;
                item.CadastrarErp = item.CadastrarAddin = false;
                item.CodProduto = prod.codigo_produto.ToString();
                // atualizar props
                int status = 0;
                int warnings = 0;
                int tipo = item.PathName.EndsWith("SLDASM")
                ? (int)swDocumentTypes_e.swDocASSEMBLY
                : (int)swDocumentTypes_e.swDocPART;
                var swModel = Sw.App.OpenDoc6(item.PathName, tipo,
                  (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

                var swModelDocExt = swModel.Extension;
                var swCustPropMgr = swModelDocExt.get_CustomPropertyManager(item.Configuracao);

                if (item.Referencia.StartsWith("Item da lista de corte")) {
                  item.ItensCorte[0].CodProduto = item.CodProduto;
                  bool boolstatus = swModel.Extension.SelectByID2(item.ItensCorte[0].NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

                  SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
                  Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
                  swCustPropMgr = swFeat.CustomPropertyManager;
                }

                swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, item.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                swModel.Save();

                if (i > 0)
                  Sw.App.CloseDoc(item.PathName);
              } else {
                item.CadastrarErp = !item.CodComponente.StartsWith("10") && !item.CodComponente.StartsWith("20") && !item.CodComponente.StartsWith("30") && !item.CodComponente.StartsWith("40");

                if (item.CodComponente.StartsWith("10") || item.CodComponente.StartsWith("20") || item.CodComponente.StartsWith("30") || item.CodComponente.StartsWith("40")) {
                  item.CodProduto = item.CodComponente;
                }

                item.CadastrarAddin = true;
                row.DefaultCellStyle.ForeColor = row.DefaultCellStyle.SelectionForeColor = corErro;
              }
            } else {
              var cod = Convert.ToInt32(item.CodProduto);
              if (db.produto_erp.Any(x => x.codigo_produto == cod && x.name == item.Name && x.referencia == item.Referencia && x.configuracao == item.Configuracao)) {
                row.DefaultCellStyle.ForeColor = row.DefaultCellStyle.SelectionForeColor = corSucesso;
                item.CadastrarErp = item.CadastrarAddin = false;
              } else {
                row.DefaultCellStyle.ForeColor = row.DefaultCellStyle.SelectionForeColor = corErro;
                item.CadastrarErp = !item.CodComponente.StartsWith("10") && !item.CodComponente.StartsWith("20") && !item.CodComponente.StartsWith("30") && !item.CodComponente.StartsWith("40");
                item.CadastrarAddin = true;
              }
            }

            //if (!item.CadastrarErp && item.ItensCorte.Count > 1) {
            //  foreach (var itemCorte in item.ItensCorte) {
            //    if (string.IsNullOrEmpty(itemCorte.CodProduto)) {
            //      item.CadastrarErp = item.CadastrarAddin = true;
            //      row.DefaultCellStyle.ForeColor = row.DefaultCellStyle.SelectionForeColor = corErro;
            //    }
            //  }
            //}
          }
        }
      } catch (Exception ex) {
        Toast.Error("Erro ao formatar cores grid. \r\n" + ex.Message);
      } finally { MsgBox.CloseWaitMessage(); }
    }

    private void dgv_CellClick(object sender, DataGridViewCellEventArgs e) {
      if (dgv.Grid.CurrentRow == null)
        return;

      if (e.RowIndex != -1) {
        var item = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;
        int status = 0;
        int warnings = 0;
        var fileName3D = item.PathName;
        var fileName2D = item.PathName.Replace("SLDPRT", "SLDDRW").Replace("SLDASM", "SLDDRW");

        if (e.RowIndex != -1 && e.ColumnIndex == dgv.Grid.Columns["Img3D"].Index) {
          try {
            var tipo = item.PathName.ToUpper().EndsWith("SLDPRT") ? (int)swDocumentTypes_e.swDocPART : (int)swDocumentTypes_e.swDocASSEMBLY;
            if (File.Exists(fileName3D)) {
              ModelDoc2 swModel = Sw.App.OpenDoc6(fileName3D, tipo, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
              int errors = 0;
              Sw.App.ActivateDoc2(fileName3D, false, (int)errors);

              if (item.Referencia.StartsWith("Item da lista de corte")) {
                var swModelDocExt = swModel.Extension;

                item.ItensCorte[0].CodProduto = item.CodProduto;
                bool boolstatus = swModel.Extension.SelectByID2(item.ItensCorte[0].NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);
              }
            }
          } catch (Exception ex) {
            MsgBox.Show($"Erro ao abrir arquivo 3D\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        } else if (e.RowIndex != -1 && e.ColumnIndex == dgv.Grid.Columns["Img2D"].Index) {
          try {
            var tipo = (int)swDocumentTypes_e.swDocDRAWING;
            if (File.Exists(fileName2D)) {
              Sw.App.OpenDoc6(fileName2D, tipo, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
              int errors = 0;
              Sw.App.ActivateDoc2(fileName2D, false, (int)errors);
            }
          } catch (Exception ex) {
            MsgBox.Show($"Erro ao abrir arquivo 3D\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }
    }
  }
}
