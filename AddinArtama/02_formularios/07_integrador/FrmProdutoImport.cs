using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using System.IO;
using System.Threading.Tasks;
using static AddinArtama.Api;

namespace AddinArtama {
  public partial class FrmProdutoImport : LmSingleForm {
    string _montageGeralNome = string.Empty;
    SortableBindingList<ProdutoErp> _produtos = new SortableBindingList<ProdutoErp>();

    Color corErro = Color.Red;
    Color corAlerta = Color.Blue;
    Color corSucesso = Color.Green;
    Color corFaltaOperacao = Color.Indigo;

    public FrmProdutoImport() {
      InitializeComponent();

      tbcOperacoes.SelectedIndex = 0;

      _produtos = new SortableBindingList<ProdutoErp>();
      dgv.MontarGrid<ProdutoErp>();
    }

    private void FrmProcessoAplicacao_Load(object sender, EventArgs e) {
      try {
        var lists = Processo.ListaProcessos
            .GroupBy(x => x.codOperacao)
            .Select(g => new Z_Padrao {
              Codigo = g.Key,
              Descricao = g.Key + " - " + g.First().descrOperacao
            })
            .ToList()
            .OrderBy(x => x.Codigo);

        txtOperacao.CarregarComboBox(lists);
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar tela");
      }
    }

    private void FrmProcessoAplicacao_Loaded(object sender, EventArgs e) {
    }

    private void BtnCarrProcess_Click(object sender, EventArgs e) {
      CarregarProcessosAsync();
    }

    private async Task CarregarProcessosAsync() {
      MsgBox.ShowWaitMessage("Lendo componentes da montagem...");
      try {
        if (Sw.App.ActiveDoc == null) {
          Toast.Warning("Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
          await Processo.Carregar();
          _produtos = new SortableBindingList<ProdutoErp>();

          _montageGeralNome = Path.GetFileNameWithoutExtension(swModel.GetPathName());

          _produtos = await ProdutoErp.GetComponentsAsync(trvProduto, _montageGeralNome);
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

        if (dgv.Grid.CurrentRow == null) {
          Toast.Info($"Nenhum produto selecionado");
          return;
        }

        if (_produtos.Count == 0) {
          Toast.Warning("Carregar Desenhos primeiro");
          return;
        }

        var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        if (produtoERP.TipoComponente == TipoComponente.ItemBiblioteca) {
          Toast.Warning("Recurso indisponivel para o componentes de biblioteca.");
          return;
        }

        //var possuiErro = false;
        //_produtos.Where(x => x.Nivel == produtoERP.Nivel || x.Nivel.StartsWith($"{produtoERP.Nivel}.")).ToList().ForEach(x => {
        //  if (x.Pendencias.Count > 0) {

        //    var msgPend = string.Empty;
        //    x.Pendencias.ForEach(y => {
        //      msgPend += $"- {y.ObterDescricaoEnum()}\r\n";
        //    });

        //    Toast.Warning($"{x.Name} [{x.Nivel}]\r\n{msgPend}");
        //    possuiErro = true;
        //  }
        //});

        //if (possuiErro) {
        //  return;
        //}

        System.Threading.Thread t = new System.Threading.Thread(() => { CadastrarNovo(produtoERP); }) { IsBackground = true };
        t.Start();
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao salvar..\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
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
            Toast.Warning($"Você está no primeiro componente");
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
          Toast.Warning($"Você já chegou no último componente");
          SalvarFechar(swModel);
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao avançar peça\n\n{ex.Message}", "Addin LM Projetos",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void BtnAtualizarProcesso_Click(object sender, EventArgs e) {
      AtualizarProcessos();
    }

    private void Dgv_RowIndexChanged(object sender, EventArgs e) {
      try {
        if (sender == null) return;

        //lblPecasProc.Text = $"Item {dgv.Grid.CurrentRow.Index + 1} de {dgv.Grid.RowCount} - {(((dgv.Grid.CurrentRow.Index + 1) * 100) / dgv.Grid.RowCount)}%";

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
        lblEspess.Text =
        lblCodMat.Text =
        lblDescMat.Text = string.Empty;

        ClearControls();
        var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        if (produtoERP.PathName.ToUpper().EndsWith(".SLDPRT"))
          Sw.App.OpenDoc6(produtoERP.PathName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, produtoERP.Configuracao, 0, 0);
        else
          Sw.App.OpenDoc6(produtoERP.PathName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, produtoERP.Configuracao, 0, 0);

        var swModel = (ModelDoc2)Sw.App.ActivateDoc2(Name: produtoERP.PathName, Silent: false, Errors: 0);
        if (swModel == null)
          return;

        swModel.ClearSelection2(true);
        swModel.ShowConfiguration2(produtoERP.Configuracao);

        if (produtoERP.ItensCorte != null && produtoERP.ItensCorte.Count == 1 && produtoERP.ItensCorte[0].Tipo == TipoListaMaterial.Chapa)
          ListaCorte.RefreshCutList(swModel, "", produtoERP.ItensCorte[0]);

        if (produtoERP.Referencia.StartsWith("Item da lista de corte")) {
          var swModelDocExt = swModel.Extension;

          bool boolstatus = swModel.Extension.SelectByID2(produtoERP.ItensCorte[0].NomeLista, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);
        }

        if (produtoERP.Pendencias.Count > 0) {

          var msgPend = string.Empty;
          produtoERP.Pendencias.ForEach(y => {
            msgPend += $"- {y.ObterDescricaoEnum()}\r\n";
          });

          Toast.Warning($"{produtoERP.Name} [{produtoERP.Nivel}]\r\n{msgPend}");
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
      txtSmLarg.Text = produtoErp.SobremetalLarg.ToString("#");
      txtSmCompr.Text = produtoErp.SobremetalCompr.ToString("#");
      lblPeso.Text = produtoErp.PesoBruto + " kg";
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
        lblCodMat.Text = codigo.ToString();
      }
    }

    private void GetProcess(ProdutoErp produtoErp) {
      try {
        ClearControls();
        string[] procs;

        if (!string.IsNullOrEmpty(produtoErp.Operacao)) {
          procs = produtoErp.Operacao?.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
        } else return;

        if (procs != null) {
          foreach (string prc in procs) {
            if (int.TryParse(prc, out int proc_id)) {
              var proc = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == proc_id);

              if (proc != null)
                CardInsert(proc);

              txtMaquina.SelectedValue = txtOperacao.SelectedValue = null;
              txtOperacao.Focus();
            }
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao retornar Processos\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void CadastrarNovo(ProdutoErp produtoErp) {
      try {
        Invoke(new MethodInvoker(async () => {
          MsgBox.ShowWaitMessage("Cadastrando Produtos...");

          using (ContextoDados db = new ContextoDados()) {
            ModelDoc2 swModel = default(ModelDoc2);
            var item = produtoErp;
            var nivelPai = item.Nivel;
            var startIndex = dgv.Grid.CurrentRow.Index;
            var nameItemPai = Path.GetFileNameWithoutExtension(item.PathName);

            int status = 0;
            int warnings = 0;

            var config = db.configuracao_api.FirstOrDefault();

            while (true) {
              int tipo = item.PathName.EndsWith("SLDASM")
              ? (int)swDocumentTypes_e.swDocASSEMBLY
              : (int)swDocumentTypes_e.swDocPART;

              if (item.CadastrarErp) {
                var itemGenerico = new Api.ItemGenerico();
                MontarItemGenerico(item, itemGenerico);

                var codigoNovo = await Api.DuplicarItemGenericoAsync(db, itemGenerico);

                if (!string.IsNullOrEmpty(codigoNovo)) {
                  item.CodProduto = codigoNovo;
                  if (item.CadastrarAddin) {
                    CadastrarAddin(db, item, codigoNovo);
                  } else {
                    AtualizarAddin(db, item);
                  }
                }
              } else {
                await Api.UpdateItemGenericoAsync(db, produtoErp);

                if (!string.IsNullOrEmpty(item.CodProduto)) {
                  if (item.CadastrarAddin) {
                    CadastrarAddin(db, item, item.CodProduto);
                  } else {
                    AtualizarAddin(db, item);
                  }
                }
              }

              if (item.CadastrarAddin || item.CadastrarErp) {
                swModel = Sw.App.OpenDoc6(item.PathName, tipo,
                  (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

                if (!swModel.IsOpenedReadOnly()) {
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

                  if (Path.GetFileNameWithoutExtension(item.PathName) != nameItemPai)
                    Sw.App.CloseDoc(item.PathName);
                }
              }

              dgv.Grid.Rows[startIndex].DefaultCellStyle.ForeColor = dgv.Grid.Rows[startIndex].DefaultCellStyle.SelectionForeColor = corSucesso;
              item.CadastrarErp = item.CadastrarAddin = false;

              startIndex++;
              if (startIndex >= dgv.Grid.Rows.Count)
                break;

              var prod = dgv.Grid.Rows[startIndex].DataBoundItem as ProdutoErp;
              if (prod.Nivel.Contains(nivelPai + "."))
                item = prod;
              else break;
            }

            // salvar engenharia
            MsgBox.ShowWaitMessage("Criando Engenharia de Produto...");
            var configApi = configuracao_api.Selecionar();
            TreeNode node = GetNodeByLevelPath(trvProduto, produtoErp.Nivel);
            await PercorrerTreeViewSalvarEngAsync(db, node, configApi);

            MsgBox.ShowWaitMessage("Salvando Todos...");

            swModel = (ModelDoc2)Sw.App.ActiveDoc;

            swModel.Save3(5, ref status, ref warnings);

            MsgBox.Show("Cadastro de produtos e engenharia finalizado com sucesso", "Addin LM Projetos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        }));
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao atualizar tempalte\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        // MsgBox.CloseWaitMessage();
        //CarregarGrid();
      }
    }

    private async Task PercorrerTreeViewSalvarEngAsync(ContextoDados db, TreeNode node, configuracao_api configApi) {
      try {
        var produtoErp = node.Tag as ProdutoErp;
        if (produtoErp != null) {
          if (produtoErp.TipoComponente != TipoComponente.ListaMaterial && produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
            var produto = _produtos.ToList().FirstOrDefault(x => produtoErp.Name == x.Name && produtoErp.Referencia == x.Referencia && produtoErp.Configuracao == x.Configuracao);

            if (produto != null) {
              produtoErp.CodProduto = produto.CodProduto;
              produtoErp.Denominacao = produto.Denominacao;
              produtoErp.SobremetalCompr = produto.SobremetalCompr;
              produtoErp.SobremetalLarg = produto.SobremetalLarg;
              produtoErp.Operacao = produto.Operacao;
            }

            var engenharia = new Engenharia {
              codEmpresa = configApi.codigoEmpresa,
              codProduto = produtoErp.CodProduto,
              tipoModulo = "S",
              codClassificacao = produtoErp.TipoComponente == TipoComponente.Montagem ? 3 : 4,
              nomeArquivoDesenhoEng = produtoErp.Name,
              descricaoProduto = $"{produtoErp.Denominacao} - {produtoErp.Name}",
            };

            if (node.Nodes.Count > 0) {
              System.Collections.IList list = node.Nodes;
              for (int i = 0; i < list.Count; i++) {
                int index = i + 1;
                TreeNode nodeFilho = (TreeNode)list[i];
                var itemFilho = nodeFilho.Tag as ProdutoErp;
                if (itemFilho != null) {
                  var classificacao = itemFilho.CodComponente.StartsWith("10") || itemFilho.CodComponente.StartsWith("20") ? 5 : itemFilho.TipoComponente == TipoComponente.Montagem ? 3 : 4;

                  var componenteEng = new ComponenteEng {
                    seqComponente = index,
                    codInsumo = itemFilho.CodProduto,
                    quantidade = itemFilho.Quantidade,
                    itemKanban = 0,
                    comprimento = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? 0 : itemFilho.ItensCorte[0].CxdCompr + itemFilho.SobremetalCompr,
                    largura = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? 0 : itemFilho.ItensCorte[0].CxdLarg + itemFilho.SobremetalLarg,
                    espessura = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? 0 : itemFilho.ItensCorte[0].CxdEspess,
                    percQuebra = 0,
                    codClassificacaoInsumo = classificacao, // 1 = produto, 3 = subconjunto, 4 = peças e 5 = insumo comprado
                  };
                  engenharia.componentes.Add(componenteEng);
                }
              }
            }

            var procs = produto.Operacao?.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
            if (procs != null && procs.Length > 0) {
              for (int seqOperacao = 1; seqOperacao <= procs.Length; seqOperacao++) {
                string proc = procs[seqOperacao - 1];

                var idProc = Convert.ToInt32(proc);
                var processo = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == idProc);
                if (processo == null) {
                  Toast.Warning($"Processo '{idProc}' não encontrado no Axion.");
                  continue;
                }
                var operacaoEng = new OperacaoEng {
                  seqOperacao = seqOperacao,
                  codOperacao = processo.codOperacao,
                  abreviaturaOperacao = processo.abreviatura,
                  numOperadores = 1,    // definido pelo cliente
                  codFaseOperacao = processo.faseProducao,
                  codMascaraMaquina = processo.mascaraMaquina.Replace(".", ""),
                  centroCusto = processo.centroCusto,
                  tempoPadraoOperacao = 1,  // definido pelo cliente
                  tempoPreparacaoOperacao = 0,   // definido pelo cliente
                };
                engenharia.operacoes.Add(operacaoEng);
              }
            }

            await Api.CadastrarEngenhariaAsync(db, engenharia);

            foreach (TreeNode nodeFilho in node.Nodes) {
              await PercorrerTreeViewSalvarEngAsync(db, nodeFilho, configApi);
            }
          }
        }
      } catch (Exception ex) {
        Toast.Error("Erro ao gerar Engenharia:\r\n" + ex.Message);
      }
    }

    private static void CadastrarAddin(ContextoDados db, ProdutoErp item, string codigoNovo) {
      try {
        var espessura = 0.0;
        var largura = 0.0;
        var comprimento = 0.0;
        var peso = 0.0;

        if (item.TipoComponente == TipoComponente.Peca && item.ItensCorte.Count == 1) {
          var itemCorte = item.ItensCorte[0];
          espessura = itemCorte.CxdEspess;
          largura = itemCorte.CxdLarg;
          comprimento = itemCorte.CxdCompr;
          peso = itemCorte.Massa;
        } else {
          peso = item.PesoLiquido;
        }

        var produtoERP = new produto_erp {
          codigo_produto = Convert.ToInt64(codigoNovo),
          name = item.Name,
          descricao = item.Denominacao,
          pathname = item.PathName,
          referencia = item.Referencia,
          configuracao = item.Configuracao,
          sobremetal_largura = item.SobremetalLarg,
          sobremetal_comprimento = item.SobremetalCompr,
          espessura = espessura,
          largura = largura,

          comprimento = comprimento,
          peso_liquido = peso,
          peso_bruto = item.PesoBruto,
          adicionado_por_id = usuario_alocados.model.usuario_id,
          adicionado_em = DateTime.Now,
          alterado_por_id = usuario_alocados.model.usuario_id,
          alterado_em = DateTime.Now,
        };

        if (produtoERP.descricao.Length > 70)
          produtoERP.descricao = produtoERP.descricao.Substring(0, 70);

        db.produto_erp.Add(produtoERP);
        db.SaveChanges();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao cadastrar produto no Axion");
      }
    }

    private static void AtualizarAddin(ContextoDados db, ProdutoErp item) {
      try {
        var espessura = 0.0;
        var largura = 0.0;
        var comprimento = 0.0;

        if (item.TipoComponente == TipoComponente.Peca && item.ItensCorte.Count == 1) {
          var itemCorte = item.ItensCorte[0];
          espessura = itemCorte.CxdEspess;
          largura = itemCorte.CxdLarg;
          comprimento = itemCorte.CxdCompr;
        }

        var codProd = Convert.ToInt64(item.CodProduto);
        var produtoERP = db.produto_erp.FirstOrDefault(x => x.codigo_produto == codProd);
        if (produtoERP == null) {
          LmException.ShowException(new Exception("Produto não encontrado no Axion"), "Erro ao atualizar produto no Axion");
          return;
        }
        // Atualiza os campos necessários
        produtoERP.name = item.Name;
        produtoERP.descricao = item.Denominacao;
        produtoERP.pathname = item.PathName;
        produtoERP.referencia = item.Referencia;
        produtoERP.configuracao = item.Configuracao;
        produtoERP.sobremetal_largura = item.SobremetalLarg;
        produtoERP.sobremetal_comprimento = item.SobremetalCompr;
        produtoERP.espessura = espessura;
        produtoERP.largura = largura;
        produtoERP.comprimento = comprimento;
        produtoERP.peso_liquido = item.PesoLiquido;
        produtoERP.peso_bruto = item.PesoBruto;
        produtoERP.alterado_por_id = usuario_alocados.model.usuario_id;
        produtoERP.alterado_em = DateTime.Now;
        db.SaveChanges();

      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao atualizar produto no Axion");
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

    private void TxtSmLarg_Leave(object sender, EventArgs e) {
      if (dgv.Grid.CurrentRow == null) {
        return;
      }

      var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;
      if (produtoERP != null && !string.IsNullOrEmpty(txtSmLarg.Text))
        produtoERP.SobremetalLarg = Convert.ToDouble(txtSmLarg.Text);
      else produtoERP.SobremetalLarg = 0;
    }

    private void TxtSmCompr_Leave(object sender, EventArgs e) {
      if (dgv.Grid.CurrentRow == null) {
        return;
      }

      var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;
      if (produtoERP != null && !string.IsNullOrEmpty(txtSmCompr.Text))
        produtoERP.SobremetalCompr = Convert.ToDouble(txtSmCompr.Text);
      else produtoERP.SobremetalCompr = 0;
    }

    private void ClearControls() {
      flpOperacoes.Controls.Clear();//.OfType<CardOperacao>().Where(x => x.Checked).ToList().ForEach(x => x.Checked = false);
    }

    private void FlpProcess_SizeChanged(object sender, EventArgs e) {
      try {
        flpOperacoes.Controls.OfType<CardOperacao>().ToList()
        .ForEach(x => {
          x.Size = new Size(flpOperacoes.Width - 9, x.Height);
        });
      } catch (Exception) {

      }
    }

    private void CarregarGrid() {
      dgv.CarregarGrid(_produtos);

      try {
        using (ContextoDados db = new ContextoDados()) {
          System.Collections.IList list = dgv.Grid.Rows;
          for (int i = 0; i < list.Count; i++) {
            DataGridViewRow row = (DataGridViewRow)list[i];
            var item = row.DataBoundItem as ProdutoErp;

            row.DefaultCellStyle.ForeColor = item.CadastrarErp
              ? row.DefaultCellStyle.SelectionForeColor = corErro
              : !item.CadastrarErp && item.CadastrarAddin
              ? row.DefaultCellStyle.SelectionForeColor = corAlerta
              : row.DefaultCellStyle.SelectionForeColor = corSucesso;

            if (string.IsNullOrEmpty(item.Operacao) && item.TipoComponente != TipoComponente.ItemBiblioteca) {
              item.ImgPendencia = Properties.Resources.warning;
              item.Pendencias.Add(PendenciasEngenharia.OperacaoNaoPossui);
            }
          }
        }
      } catch (Exception ex) {
        Toast.Error("Erro ao formatar cores grid. \r\n" + ex.Message);
      } finally { MsgBox.CloseWaitMessage(); }
    }

    private void AdicionarDescricaoTodasConfiguracoes() {
      try {
        if (dgv.Grid.CurrentRow == null) {
          Toast.Info($"Nenhum produto selecionado");
          return;
        }

        var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        ConfigurationManager swConfMgr = swModel.ConfigurationManager;
        var configName = swConfMgr.ActiveConfiguration.Name;
        var swModelDocExt = swModel.Extension;

        var swCustPropMngr = swModelDocExt.get_CustomPropertyManager(configName);

        swCustPropMngr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText, produtoERP.Denominacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        //var status = swModel.ShowConfiguration2(configName);
      } catch (Exception ex) {
        Toast.Warning("Falha ao Atualizar Denominação: \n" + ex.Message);
      }
    }

    private void TxtOperacao_SelectedValueChanched(object sender, EventArgs e) {
      try {
        txtMaquina.SelectedValue = null;
        if (txtOperacao.SelectedValue != null) {
          var idOp = (int)txtOperacao.SelectedValue;

          var lists = Processo.ListaProcessos
              .Where(x => x.codOperacao == idOp)
              .Select(x => new Z_Padrao {
                Codigo = x.codMaquina,
                Descricao = $"{x.codMaquina} - {x.descrMaquina}",
              })
              .ToList()
              .OrderBy(x => x.Codigo);

          txtMaquina.CarregarComboBox(lists);

          if (lists.Count() == 1)
            txtMaquina.SelectedValue = lists.FirstOrDefault().Codigo;
        } else {
          txtMaquina.CarregarComboBox(null);
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao selecionar Máquinas");
      }
    }

    private void BtnInserir_Click(object sender, EventArgs e) {
      try {
        if (dgv.Grid.CurrentRow == null) {
          Toast.Info($"Nenhum produto selecionado");
          return;
        }

        txtMaquina.CampoObrigatorio = txtOperacao.CampoObrigatorio = true;

        if (Controles.PossuiCamposInvalidos(lmPanelOP)) {
          txtMaquina.CampoObrigatorio = txtOperacao.CampoObrigatorio = false;
          return;
        }

        var idOp = (int)txtOperacao.SelectedValue;
        var idMq = (int)txtMaquina.SelectedValue;

        if (flpOperacoes.Controls.OfType<CardOperacao>().Any(x => ((Processo)x.Tag).codOperacao == idOp && ((Processo)x.Tag).codMaquina == idMq)) {
          Toast.Warning("Esta Operação com esta Máquina já foi inserida");
          return;
        }

        var proc = Processo.ListaProcessos.FirstOrDefault(x => x.codOperacao == idOp && x.codMaquina == idMq);

        CardInsert(proc);

        AtualizarProcessos();

        AdicionarDescricaoTodasConfiguracoes();

        txtMaquina.CampoObrigatorio = txtOperacao.CampoObrigatorio = false;
        txtMaquina.SelectedValue = txtOperacao.SelectedValue = null;
        txtOperacao.Focus();
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao inserir Operação ao Componente");
      }
    }

    private void AtualizarProcessos() {
      var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

      var operacoes = flpOperacoes.Controls
          .OfType<CardOperacao>()
          .Select(a => ((Processo)a.Tag).codOperacao);

      produtoERP.Operacao = string.Join("^", operacoes);

      var swModel = (ModelDoc2)Sw.App.ActiveDoc;

      var swModelDocExt = swModel.Extension;
      var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

      swCustPropMgr.Add3("PESO", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Mass\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

      if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
        swCustPropMgr.Add3("Material", (int)swCustomInfoType_e.swCustomInfoText, "\"SW-Material\"", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
      } else
        swCustPropMgr.Delete("Material");

      if (produtoERP.Referencia.StartsWith("Item da lista de corte")) {
        produtoERP.ItensCorte[0].Operacao = produtoERP.Operacao;
        ListaCorte.UpdateCutList(swModel, produtoERP.ItensCorte[0]);
      } else {
        swCustPropMgr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText, produtoERP.Operacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
      }

      if (!string.IsNullOrEmpty(produtoERP.Operacao)) {
        var list = produtoERP.Pendencias.Where(x => x == PendenciasEngenharia.OperacaoRevisar || x == PendenciasEngenharia.OperacaoNaoPossui);
        list.ToList().ForEach(x => { produtoERP.Pendencias.Remove(x); });

        if (!produtoERP.Pendencias.Any()) {
          produtoERP.ImgPendencia = new Bitmap(20, 20);
        }
      } else {
        if (!produtoERP.Pendencias.Contains(PendenciasEngenharia.OperacaoNaoPossui)) {
          produtoERP.Pendencias.Add(PendenciasEngenharia.OperacaoNaoPossui);
          produtoERP.ImgPendencia = Properties.Resources.warning;
        }
      }

      swModel.Save();
    }

    private void CardInsert(Processo proc) {


      CardOperacao card = new CardOperacao {
        Tag = proc,
        Width = flpOperacoes.Width - 9
      };

      card.SetText();

      card.CardExclude += Card_CardExclude;
      card.MouseDownCtrl += Card_MouseDownCtrl;

      flpOperacoes.Controls.Add(card);
    }

    private void Card_CardExclude(object sender, EventArgs e) {
      this.flpOperacoes.Controls.Remove((CardOperacao)sender);
      AtualizarProcessos();
    }

    private void Card_MouseDownCtrl(object sender, MouseEventArgs e) {
      ((CardOperacao)sender).DoDragDrop((CardOperacao)sender, DragDropEffects.Move);
    }

    private void FlpOperacoes_DragEnter(object sender, DragEventArgs e) {
      if (e.Data.GetDataPresent(typeof(CardOperacao)))
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void FlpOperacoes_DragDrop(object sender, DragEventArgs e) {
      try {
        var pt = new Point(e.X, e.Y);

        var control = (CardOperacao)e.Data.GetData(typeof(CardOperacao));

        Point mousePosition = flpOperacoes.PointToClient(pt);
        Control destination = flpOperacoes.GetChildAtPoint(mousePosition);

        if (destination == null) {
          pt = new Point(pt.X, pt.Y + 10);
          mousePosition = flpOperacoes.PointToClient(pt);
          destination = flpOperacoes.GetChildAtPoint(mousePosition);
        }

        int indexDestination = flpOperacoes.Controls.IndexOf(destination);
        if (flpOperacoes.Controls.IndexOf(control) < indexDestination)
          indexDestination--;

        flpOperacoes.Controls.SetChildIndex(control, indexDestination);

        AtualizarProcessos();
      } catch (Exception ex) {
        Toast.Error(ex.Message);
      }
    }

    TreeNode GetNodeByLevelPath(TreeView treeView, string path) {
      string[] levels = path.Split('.');
      TreeNode currentNode = null;

      for (int i = 0; i < levels.Length; i++) {
        int index = int.Parse(levels[i]) - 1; // Assume que "1.3" significa índice 0,2

        if (i == 0) {
          // Nível raiz
          if (treeView.Nodes.Count > index)
            currentNode = treeView.Nodes[index];
          else
            return null;
        } else {
          // Nível filho
          if (currentNode != null && currentNode.Nodes.Count > index)
            currentNode = currentNode.Nodes[index];
          else
            return null;
        }
      }

      return currentNode;
    }
  }
}
