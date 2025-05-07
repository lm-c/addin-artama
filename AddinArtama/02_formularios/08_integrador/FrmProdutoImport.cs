using System;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace AddinArtama {
  public partial class FrmProdutoImport : LmSingleForm {
    public SldWorks swApp = new SldWorks();
    ModelDoc2 swModel = default(ModelDoc2);
    ModelDocExtension swModelDocExt;
    CustomPropertyManager swCustPropMngr = default(CustomPropertyManager);

    string _montagemPrincipal = string.Empty;
    int _posAtualItemCorte = 0;
    long novo_cadcont = 1;

    public FrmProdutoImport() {
      InitializeComponent();
    }

    private void FrmProdutoImport_Loaded(object sender, EventArgs e) {
      Invoke(new MethodInvoker(() => {

      }));
    }

    private void BtnCarrProcess_Click(object sender, EventArgs e) {
      MsgBox.ShowWaitMessage("Lendo componentes da montagem...");
      try {
        if (swApp.ActiveDoc == null) {
          Toast.Warning("Sem documentos abertos");
          return;
        }

        swModel = (ModelDoc2)swApp.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
          _montagemPrincipal = swModel.GetPathName().ToLower();

          TreeComponent.GetComponents(trvProduto);
          trvProduto.TopNode = trvProduto.Nodes[0];
          TrvProduto_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(trvProduto.Nodes[0], MouseButtons.Left, 1, 0, 0));
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

    private void BtnSalvar_Click(object sender, EventArgs e) {
      try {
        if (swApp.ActiveDoc == null) {
          Toast.Warning("Sem documentos abertos.");
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
          Toast.Warning("Comando apenas para Peças e Montagens.");
          return;
        }

        var swConfMgr = swModel.ConfigurationManager;
        var configName = swConfMgr.ActiveConfiguration.Name;
        string defConfig = configName;

        //swCustPropMngr = swModelDocExt.get_CustomPropertyManager(defConfig);
        //swCustPropMngr.Add3("sgl_DescricaoEspecifica", (int)swCustomInfoType_e.swCustomInfoText, descr, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        if (trvProduto.SelectedNode != null) {
          trvProduto.SelectedNode.ForeColor = Color.Black;
          trvProduto.SelectedNode.ToolTipText = string.Empty;
        }

        swModel.Save();

        Toast.Success("Sucesso!");
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao salvar..\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void BtnImportar_Click(object sender, EventArgs e) {
      long priCod = 999999999;
      var atualizandoEstrutura = false;

      try {
      //  MsgBox.ShowWaitMessage("Verificando pendências...");
      //  List<TreeComponent> list = new List<TreeComponent>();
      //  List<long> importados = new List<long>();
      //  var compSemProps = "";
      //  var compSemSalvar = "";
      //  var listaVistos = new List<string>();
      //  PercorrerTreeViewIgnorandoRepetido(trvProduto.Nodes, list);

      //  using (var conn = ConexaoPgSql.GetConexao()) {
      //    conn.Open();
      //    using (var transaction = conn.BeginTransaction()) {
      //      try {
      //        // Verificar Materia Prima
      //        using (var cmd = conn.CreateCommand()) {
      //          foreach (var item in list) {
      //            if (item.tipoComponente != TipoComponente.ListaMaterial &&
      //              (item.grupo == null ||
      //              item.subGrupo == null ||
      //              string.IsNullOrEmpty(item.um1) ||
      //              string.IsNullOrEmpty(item.operacao))) {
      //              compSemProps += $"{Path.GetFileName(item.pathName)} | ";
      //            } else if (item.tipoComponente == TipoComponente.ListaMaterial && !compSemSalvar.Contains(item.codigo)) {
      //              cmd.Parameters.Clear();

      //              cmd.CommandText = "SELECT codigo_produto FROM vw_produto WHERE codigo_produto = @codigo_produto ";
      //              cmd.Parameters.AddWithValue("@codigo_produto", item.codigo);

      //              using (var dr = cmd.ExecuteReader()) {
      //                if (!dr.Read()) {
      //                  compSemSalvar += $"{Path.GetFileName(item.codigo)} | ";
      //                }
      //              }
      //            }
      //          }
      //        }

      //        if (!string.IsNullOrEmpty(compSemProps)) {
      //          compSemProps = $"Os items abaixo, estão sem o grupo e subgrupo definido, preencha-os para prosseguir." +
      //            $"<sep>" +
      //            $"{compSemProps.Substring(0, compSemProps.Length - 3)}";
      //          MsgBox.Show(compSemProps, "Addin LM Projetos",
      //            MessageBoxButtons.OK, MessageBoxIcon.Warning);
      //          conn.Close();
      //          return;
      //        }

      //        if (!string.IsNullOrEmpty(compSemSalvar)) {
      //          compSemSalvar = $"As Materias Primas abaixo, ainda não foram salvas, salve-as no ERP para prosseguir." +
      //            $"<sep>" +
      //            $"{compSemSalvar.Substring(0, compSemSalvar.Length - 3)}";
      //          MsgBox.Show(compSemSalvar, "Addin LM Projetos",
      //            MessageBoxButtons.OK, MessageBoxIcon.Warning);
      //          conn.Close();
      //          return;
      //        }

      //        // Salvar Estrutura
      //        novo_cadcont = priCod = 1;
      //        using (var cmd = conn.CreateCommand()) {
      //          MsgBox.ShowWaitMessage("Importando Estrutura...");
      //          cmd.CommandText = "SELECT cadcont FROM cadireta ORDER BY cadcont DESC LIMIT 1;";
      //          using (var dr = cmd.ExecuteReader()) {
      //            if (dr.Read()) {
      //              novo_cadcont = priCod = dr.GetInt64(dr.GetOrdinal("cadcont")) + 1;
      //            }
      //          }

      //          if (ImportacaoProjeto.ImportacaoEmAndamento(out string cod, out string desc)) {
      //            MsgBox.Show($"O Projeto '{cod} - {desc}' está em sendo importado neste momento, aguarde alguns minutos e tente novamente.",
      //              "Importação em Andamento", MessageBoxButtons.OK, MessageBoxIcon.Information);
      //            return;
      //          }

      //          ImportacaoProjeto.model = ImportacaoProjeto.Selecionar(list[0].codigo);

      //          atualizandoEstrutura = ImportacaoProjeto.model != null;

      //          if (ImportacaoProjeto.model == null) {
      //            ImportacaoProjeto.model = new ImportacaoProjeto {
      //              Codigo = list[0].codigo,
      //              Descricao = list[0].descricao,
      //              CadContIni = priCod,
      //              ImportacaoConcluida = false,
      //            };
      //          }

      //          ImportacaoProjeto.Salvar();

      //          PercorrerTreeViewSalvarCadireta(cmd, trvProduto.Nodes, importados);
      //        }

      //        if (atualizandoEstrutura) {
      //          List<long> itensExixtentes = new List<long>();
      //          using (var cmd = conn.CreateCommand()) {
      //            MsgBox.ShowWaitMessage("Removendo Obsoletos...");
      //            cmd.CommandText = "" +
      //              "SELECT cadcont " +
      //              "FROM cadireta " +
      //              "WHERE cadcont >= @cadcontIni AND cadcont <= @cadcontFim " +
      //              "ORDER BY cadcont ASC;";
      //            cmd.Parameters.AddWithValue("@cadcontIni", ImportacaoProjeto.model.CadContIni);
      //            cmd.Parameters.AddWithValue("@cadcontFim", ImportacaoProjeto.model.CadContFim);

      //            using (var dr = cmd.ExecuteReader()) {
      //              while (dr.Read()) {
      //                itensExixtentes.Add(dr.GetInt64(dr.GetOrdinal("cadcont")));
      //              }
      //            }

      //            cmd.Parameters.Clear();

      //            string clausulaWherePro = string.Empty;
      //            string clausulaWhereDim = string.Empty;
      //            string clausulaWhereEst = string.Empty;

      //            foreach (var id in itensExixtentes) {
      //              if (!importados.Contains(id)) {
      //                clausulaWherePro += $"cadpcont = @cadContPro{id} OR ";
      //                cmd.Parameters.AddWithValue($"@cadContPro{id}", id);

      //                clausulaWhereDim += $"caddcont = @cadContDim{id} OR ";
      //                cmd.Parameters.AddWithValue($"@cadContDim{id}", id);

      //                clausulaWhereEst += $"cadcont = @cadContEst{id} OR ";
      //                cmd.Parameters.AddWithValue($"@cadContEst{id}", id);
      //              }
      //            }

      //            if (!string.IsNullOrEmpty(clausulaWherePro)) {
      //              clausulaWherePro = clausulaWherePro.Substring(0, clausulaWherePro.Length - 4);
      //              cmd.CommandText = "" +
      //               $"DELETE " +
      //               $"FROM cadproce " +
      //               $"WHERE {clausulaWherePro}";
      //              cmd.ExecuteNonQuery();
      //            }

      //            if (!string.IsNullOrEmpty(clausulaWhereDim)) {
      //              clausulaWhereDim = clausulaWhereDim.Substring(0, clausulaWhereDim.Length - 4);
      //              cmd.CommandText = "" +
      //               $"DELETE " +
      //               $"FROM cadiredi " +
      //               $"WHERE {clausulaWhereDim}";
      //              cmd.ExecuteNonQuery();
      //            }

      //            if (!string.IsNullOrEmpty(clausulaWhereEst)) {
      //              clausulaWhereEst = clausulaWhereEst.Substring(0, clausulaWhereEst.Length - 4);
      //              cmd.CommandText = "" +
      //               $"DELETE " +
      //               $"FROM cadireta " +
      //               $"WHERE {clausulaWhereEst}";
      //              cmd.ExecuteNonQuery();
      //            }
      //          }
      //        }

      //        transaction.Commit();

      //        ImportacaoProjeto.model.CadContFim = novo_cadcont - 1;
      //        ImportacaoProjeto.model.ImportacaoConcluida = true;
      //        ImportacaoProjeto.Salvar();

      //        Toast.Success("Estrutura Importada com Sucesso!!");
      //      } catch (Exception ex1) {
      //        transaction.Rollback();
      //        throw ex1;
      //      }
      //    }
      //  }
      //} catch (Exception ex) {
      //  LmException.ShowException(ex, "Erro ao Importar Estrutura");

      //  try {
      //    if (!atualizandoEstrutura)
      //      ImportacaoProjeto.Excluir();
      //  } catch (Exception ex1) {
      //    LmException.ShowException(ex1, "Erro ao Limpar Importação parcial");
      //  }
      } finally {
        MsgBox.CloseWaitMessage();
      }
    }

    private void PercorrerTreeViewIgnorandoRepetido(TreeNodeCollection nodes, List<TreeComponent> list) {
      foreach (TreeNode node in nodes) {
        TreeComponent comp = (TreeComponent)node.Tag;
        if (!list.Any(x => x.codigo.Equals(comp?.codigo))) {
          if (node.Tag != null) {
            list.Add(comp);
          }

          // Chamar recursivamente para percorrer os nós filhos
          if (node.Nodes.Count > 0) {
            PercorrerTreeViewIgnorandoRepetido(node.Nodes, list);
          }
        }
      }
    }

    private void BtnClose_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void PercorrerTreeViewSalvarCadireta( TreeNodeCollection nodes, List<long> importados) {
      foreach (TreeNode node in nodes) {
        long exist_cadcont = 0;
        var agora = DateTime.Now;

        if (node.Level != 0) {
          TreeComponent compPai = (TreeComponent)node.Parent.Tag;
          TreeComponent compFil = (TreeComponent)node.Tag;

          importados.Add(exist_cadcont == 0 ? novo_cadcont : exist_cadcont);

          // Salvar CADIRETA
        
          // Salvar CADIREDI
          }

          // Salvar CADPROCE



        // Chamar recursivamente para percorrer os nós filhos
        if (node.Nodes.Count > 0) {
          PercorrerTreeViewSalvarCadireta( node.Nodes, importados);
        }
        }
      }
    

    private void TrvProduto_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
      TreeComponent treeComp = (TreeComponent)e.Node.Tag;
      trvProduto.SelectedNode = e.Node;
      if (treeComp != null && treeComp?.tipoComponente != TipoComponente.ListaMaterial) {
        try {
          var pathName = treeComp.pathName;
          var confgName = treeComp.configName;
          swModel = (ModelDoc2)swApp.ActiveDoc;

          if (swModel != null && swModel.GetPathName().ToLower() != _montagemPrincipal) {
            swModel.ShowNamedView("*Isométrica");
            swModel.ViewZoomtofit();

            swModel.Save();
            swApp.CloseDoc(swModel.GetPathName());
          }

          if (treeComp.tipoComponente == TipoComponente.Peca)
            swApp.OpenDoc6(treeComp.pathName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
          else
            swApp.OpenDoc6(treeComp.pathName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);

          swModel = (ModelDoc2)swApp.ActivateDoc2(Name: treeComp.pathName, Silent: false, Errors: 0);

        } catch (Exception ex) {
          LmException.ShowException(ex, "Erro ao atualizar dados Componente");
        }
      }
    }

    private void TrvProduto_BeforeSelect(object sender, TreeViewCancelEventArgs e) {
      try {
        trvProduto.SelectedNode.BackColor = trvProduto.BackColor;
        //trvProduto.SelectedNode.ForeColor = trvProduto.ForeColor; 
      } catch (Exception ex) {
        // LmException.ShowException(ex, "Erro eventos Antes de Selecionar");
      }
    }

    private void TrvProduto_AfterSelect(object sender, TreeViewEventArgs e) {
      try {
        e.Node.BackColor = Color.FromArgb(0, 120, 215);
        //e.Node.ForeColor = Color.White; 
      } catch (Exception ex) {
        // LmException.ShowException(ex, "Erro eventos Após de Selecionar");
      }
    }

    private void TrvProduto_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e) {
      if (!string.IsNullOrEmpty(e.Node.ToolTipText))
        MsgBox.ShowToolTip(pnlControl, e.Node.ToolTipText, tempoExibicao: 3);
    }
  }
}
