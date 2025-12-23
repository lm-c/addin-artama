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
using System.Collections.Generic;
using System.Linq.Dynamic.Core;

namespace AddinArtama {
  public partial class FrmProdutoImport : LmSingleForm {
    string _montageGeralNome = string.Empty;
    SortableBindingList<ProdutoErp> _produtos = new SortableBindingList<ProdutoErp>();
    TreeView _arvoreCompleta = new TreeView();
    bool _canceladoComErro = false;

    Color corErro = Color.Red;
    Color corAlerta = Color.Blue;
    Color corSucesso = Color.Green;

    public FrmProdutoImport() {
      InitializeComponent();

      tbcOperacoes.SelectedIndex = 0;

      ImageList il = new ImageList();
      il.Images.Add(0.ToString(), Properties.Resources.assembly);
      il.Images.Add(1.ToString(), Properties.Resources.part);
      il.Images.Add(2.ToString(), Properties.Resources.weldmentcutlist);
      il.Images.Add(3.ToString(), Properties.Resources.sheetmetal);
      il.Images.Add(4.ToString(), Properties.Resources.weldment);
      il.Images.Add(5.ToString(), Properties.Resources.toolbox_item);

      trvProduto.ImageList = il;
      trvProduto.ItemHeight = 21;

      _produtos = new SortableBindingList<ProdutoErp>();
      dgv.MontarGrid<ProdutoErp>();
    }

    private void FrmProcessoAplicacao_Load(object sender, EventArgs e) {

    }

    private void FrmProcessoAplicacao_Loaded(object sender, EventArgs e) {
    }

    private void BtnCarrProcess_Click(object sender, EventArgs e) {
      //if (_trabalhando) {
      //  _trabalhando = false;
      //  btnSalvar.Enabled = true;
      //  btnCarrProcess.Image = Properties.Resources.carregar;
      //  btnCarrProcess.Text = " Carregar Processos";
      //  return;
      //}

      CarregarProcessosAsync();
    }

    private void CarregarProcessosAsync() {
      Invoke(new MethodInvoker(async () => {
        //MsgBox.ShowWaitMessage("Lendo componentes da montagem...");
        try {

          ptbMaterialError.Visible = false;

          if (Sw.App.ActiveDoc == null) {
            Toast.Warning("Sem documentos abertos");
            return;
          }

          if (dgv.Grid.RowCount > 0) {
            var result = MsgBox.Show("Já existem componentes carregados. Deseja substituir os dados atuais?",
              "Addin LM Projetos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
              return;
          }

          var swModel = (ModelDoc2)Sw.App.ActiveDoc;

          if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
            _arvoreCompleta.Nodes.Clear();
            trvProduto.Nodes.Clear();

            await Loader.ShowDuringOperation(async (progress) => {
              progress.Report("Limpando cache e dados antigos");

              // Limpar todos os caches para evitar duplicação de códigos no ERP
              ProdutoErp.ClearAllCaches();

              await Processo.Carregar();
              _produtos = new SortableBindingList<ProdutoErp>();

              _montageGeralNome = Path.GetFileNameWithoutExtension(swModel.GetPathName());

              if (_montageGeralNome.Length > 50)
                _montageGeralNome = _montageGeralNome.Substring(0, 50);

              _produtos = new SortableBindingList<ProdutoErp>();
              UIThreadHelper.Invoke(dgv, () => {
                dgv.CarregarGrid(_produtos);
              });
            });

            UIThreadHelper.Invoke(btnCarrProcess, () => {
              btnCarrProcess.Enabled = btnSalvar.Enabled = false;
            });

            await Loader.ShowDuringOperation(async (progress) => {
              progress.Report("Iniciando leitura Componentes");
              _produtos = await ProdutoErp.GetComponentsAsync(_arvoreCompleta);
            });

            UIThreadHelper.Invoke(btnCarrProcess, () => {
              btnCarrProcess.Enabled = btnSalvar.Enabled = true;
            });

            await Loader.ShowDuringOperation(async (progress) => {
              progress.Report("Carregando Grid...");
              CarregarGrid();
            });
          } else {
            Toast.Warning("Comando apenas para Peças e Montagens");
          }
        } catch (Exception ex) {
          MsgBox.Show($"Erro ao Carregar Componentes..\n\n{ex.Message}", "Addin LM Projetos",
              MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
          MsgBox.CloseWaitMessage();
        }
      }));
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

        if (_produtos.Any(x => string.IsNullOrEmpty(x.Denominacao))) {
          Toast.Warning("Favor preencher todas as Denominações");
          return;
        }

        var produtoERP = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        if (produtoERP.TipoComponente == TipoComponente.ItemBiblioteca) {
          Toast.Warning("Recurso indisponivel para o componentes de biblioteca.");
          return;
        }

        if (!string.IsNullOrEmpty(dgv.txtProcurar.Text)) {
          Toast.Warning("Remova o filtro do Grid para salvar Produtos.");
          return;
        }

        var possuiErro = false;
        _produtos.Where(x => x.Nivel == produtoERP.Nivel || x.Nivel.StartsWith($"{produtoERP.Nivel}.")).ToList().ForEach(x => {
          if (x.Pendencias.Where(y => y.EhPendenciaCritica()).ToList().Count > 0) {
            var msgPend = string.Empty;
            x.Pendencias.Where(y => y.EhPendenciaCritica()).ToList().ForEach(y => {
              if (y.EhPendenciaCritica())
                msgPend += $"- {y.ObterDescricaoEnum()}\r\n";
            });

            Toast.Warning($"{x.Name} [{x.Nivel}]\r\n{msgPend}");
            possuiErro = true;
          }
        });

        if (possuiErro) {
          return;
        }

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
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao avançar peça\n\n{ex.Message}", "Addin LM Projetos",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
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
      //txtMaquina.CampoObrigatorio = txtOperacao.CampoObrigatorio = false;
      //txtMaquina.SelectedValue = txtOperacao.SelectedValue = null;

      //txtTempoOperacao.Text = tempoPadro;
      //txtNumeroOperadores.Text = numOperadorPadrao.ToString();

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
        lblPesoBrut.Text = "0,000Kg";
        lblPesoNbr.Text = "0,000Kg";
        lblEspess.Text =
        lblCodMat.Text =
        lblCodigoProduto.Text =
        lblDescMat.Text = string.Empty;

        var produtoErp = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;

        if (produtoErp.PathName.ToUpper().EndsWith(".SLDPRT"))
          Sw.App.OpenDoc6(produtoErp.PathName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, produtoErp.Configuracao, 0, 0);
        else
          Sw.App.OpenDoc6(produtoErp.PathName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, produtoErp.Configuracao, 0, 0);

        var swModel = (ModelDoc2)Sw.App.ActivateDoc2(Name: produtoErp.PathName, Silent: false, Errors: 0);
        if (swModel == null)
          return;

        swModel.ClearSelection2(true);
        swModel.ShowConfiguration2(produtoErp.Configuracao);

        if (produtoErp.ItemCorte != null)
          ListaCorte.RefreshCutList(swModel, "", produtoErp.ItemCorte);

        if (produtoErp.Referencia.StartsWith("Item da lista de corte")) {
          var swModelDocExt = swModel.Extension;

          bool boolstatus = swModel.Extension.SelectByID2(produtoErp.Referencia, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);
        }

        AtualizarInfoAlerta(produtoErp);

        AtualizarInformacoes(produtoErp);
        TreeNode node = GetNodeByLevelPath(_arvoreCompleta, produtoErp.Nivel);
        TreeNode clonedNode = (TreeNode)node.Clone();
        trvProduto.Nodes.Clear();
        trvProduto.Nodes.Add(clonedNode);
        clonedNode.ExpandAll();
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Atualizar Dados\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void AtualizarInfoAlerta(ProdutoErp produtoErp) {
      if (produtoErp.Pendencias.Where(y => y.EhPendenciaCritica()).ToList().Count > 0) {
        var msgPend = string.Empty;
        produtoErp.Pendencias.Where(y => y.EhPendenciaCritica()).ToList().ForEach(y => {
          msgPend += $"- {y.ObterDescricaoEnum()}\r\n";
        });

        UIThreadHelper.Invoke(lblWarning, () => {
          lblWarning.Visible = true;
          lblWarning.Text = msgPend.Trim();
          lblWarning.ForeColor = Color.Red;
          lblWarning.Refresh();
        });
      } else if (produtoErp.Pendencias.Where(y => !y.EhPendenciaCritica()).ToList().Count > 0) {
        var msgPend = string.Empty;
        produtoErp.Pendencias.Where(y => !y.EhPendenciaCritica()).ToList().ForEach(y => {
          msgPend += $"- {y.ObterDescricaoEnum()}\r\n";
        });

        UIThreadHelper.Invoke(lblWarning, () => {
          lblWarning.Visible = true;
          lblWarning.Text = msgPend.Trim();
          lblWarning.ForeColor = Color.DarkGoldenrod;
          lblWarning.Refresh();
        });
      } else {
        UIThreadHelper.Invoke(lblWarning, () => {
          lblWarning.Visible = false;
        });
      }
    }

    private void AtualizarInformacoes(ProdutoErp produtoErp) {
      txtDescricao.Text = produtoErp.Denominacao;
      txtSmLarg.Text = produtoErp.SobremetalLarg.ToString("#");
      txtSmCompr.Text = produtoErp.SobremetalCompr.ToString("#");
      lblPesoBrut.Text = produtoErp.PesoBruto + " kg";
      lblPesoNbr.Text = produtoErp.PesoPadraoNBR + " kg";
      lblCodigoProduto.Text = produtoErp.CodProduto;

      if (produtoErp.ItemCorte != null && produtoErp.TipoComponente == TipoComponente.Peca || produtoErp.Referencia.StartsWith("Item da lista de corte")) {
        var espess = produtoErp.ItemCorte.Espessura;
        var largur = produtoErp.ItemCorte.Largura + produtoErp.SobremetalLarg;
        var compri = produtoErp.ItemCorte.Comprimento + produtoErp.SobremetalCompr;
        var descricMaterial = produtoErp.ItemCorte.Denominacao;
        var tipo = produtoErp.ItemCorte.Tipo;
        var codigo = produtoErp.ItemCorte.Codigo;

        lblEspess.Text = tipo == TipoListaMaterial.Chapa ? $"{espess}x{largur}x{compri}" : $"{compri}";
        lblDescMat.Text = produtoErp.ItemCorte.Denominacao;
        lblCodMat.Text = codigo.ToString();

        ptbMaterialError.Visible = produtoErp.Pendencias.Contains(PendenciasEngenharia.MateriaErrado);
      }
    }

    private void CadastrarNovo(ProdutoErp produtoErp) {
      Invoke(new MethodInvoker(async () => {
        var nomeProduto = string.Empty;
        try {
          _canceladoComErro = false;

          btnSalvar.Enabled = btnCarrProcess.Enabled = false;

          await Loader.ShowDuringOperation(async (progress) => {
            progress.Report("Iniciando Cadastro de3 Produtos...");
            using (ContextoDados db = new ContextoDados()) {
              ModelDoc2 swModel = default(ModelDoc2);
              var nivelPai = produtoErp.Nivel;
              var startIndex = dgv.Grid.CurrentRow.Index;
              var nameItemPai = Path.GetFileNameWithoutExtension(produtoErp.PathName);

              int status = 0;
              int warnings = 0;

              var config = db.configuracao_api.FirstOrDefault();

              while (true) {
                if (!Loader._isWorking) {
                  MsgBox.Show($"Cadastro de produtos cancelado pelo usuário.",
                    "Ação não Permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  //Toast.Info("Cadastro de produtos cancelado pelo usuário.");
                  return;
                }

                nomeProduto = produtoErp.Name;

                // imprimir label 
                progress.Report($"Cadastrando produto '{nomeProduto}'!");

                await SalvarProdutoAsync(produtoErp, db, swModel, startIndex, nameItemPai, status, warnings);

                startIndex++;
                if (startIndex >= dgv.Grid.Rows.Count)
                  break;

                if (startIndex < dgv.Grid.FirstDisplayedScrollingRowIndex ||
                    startIndex > dgv.Grid.FirstDisplayedScrollingRowIndex + dgv.Grid.DisplayedRowCount(false) - 1) {
                  UIThreadHelper.Invoke(dgv.Grid, () => {
                    dgv.Grid.FirstDisplayedScrollingRowIndex = startIndex;
                  });
                }

                var prod = dgv.Grid.Rows[startIndex].DataBoundItem as ProdutoErp;

                if (prod.Nivel.Contains(nivelPai + "."))
                  produtoErp = prod;
                else break;
              }

              // salvar engenharia
              progress.Report("Criando Engenharia de Produto...");
              var configApi = configuracao_api.Selecionar();

              await PercorrerTreeViewSalvarEngAsync(db, trvProduto.Nodes[0], configApi, progress);

              //MsgBox.ShowWaitMessage("Salvando Todos...");

              swModel = (ModelDoc2)Sw.App.ActiveDoc;

              swModel.Save3(5, ref status, ref warnings);
            }
          });

          if (!_canceladoComErro)
            Toast.Success("Cadastro de produtos e engenharia finalizado com sucesso");
          else
            MsgBox.Show("Cadastro de produtos e engenharia finalizado com Erro", "Addin LM Projetos",
              MessageBoxButtons.OK, MessageBoxIcon.Error);

          UIThreadHelper.Invoke(lblCodigoProduto, () => {
            lblCodigoProduto.Text = produtoErp.CodProduto;
            btnSalvar.Enabled = btnCarrProcess.Enabled = true;
          });
        } catch (Exception ex) {
          // dgv.RowIndexChanged += Dgv_RowIndexChanged;
          MsgBox.Show($"Erro ao cadastrar produto\n\nItem: {nomeProduto}\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);

          btnSalvar.Enabled = btnCarrProcess.Enabled = true;
        } finally {
        }
      }));
    }

    private async Task SalvarProdutoAsync(ProdutoErp produtoErp, ContextoDados db, ModelDoc2 swModel, int startIndex, string nameItemPai, int status, int warnings) {

      int tipo = produtoErp.PathName.EndsWith("SLDASM")
      ? (int)swDocumentTypes_e.swDocASSEMBLY
      : (int)swDocumentTypes_e.swDocPART;

      //if (produtoErp.ItemGenerico == null) {
      if (string.IsNullOrEmpty(produtoErp.CodProduto)) {
        var itemGenerico = new Api.ItemGenerico();
        MontarItemGenerico(produtoErp, itemGenerico);

        if (produtoErp.Fantasma) {
          itemGenerico.situacao = 0;
        }

        var codigoNovo = await Api.DuplicarItemGenericoAsync(db, itemGenerico);

        if (!string.IsNullOrEmpty(codigoNovo)) {
          produtoErp.CodProduto = codigoNovo;
        }
      } else {
        if (produtoErp.TipoComponente != TipoComponente.ItemBiblioteca && produtoErp.TipoComponente != TipoComponente.ListaMaterial) {
          await Api.UpdateItemGenericoAsync(produtoErp);
        }
      }

      var jaCadastradoAddin = long.TryParse(produtoErp.CodProduto, out long codprod)
         && db.produto_erp.Any(x => x.codigo_produto == codprod);

      if (!jaCadastradoAddin) {
        CadastrarAddin(db, produtoErp);
      } else {
        AtualizarAddin(db, produtoErp);
      }

      if (produtoErp.ItemGenerico == null) {
        swModel = Sw.App.OpenDoc6(produtoErp.PathName, tipo,
          (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

        if (!swModel.IsOpenedReadOnly()) {
          var swModelDocExt = swModel.Extension;
          var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

          if (produtoErp.Referencia.StartsWith("Item da lista de corte")) {
            //produtoErp.ItemCorte.CodProduto = produtoErp.CodProduto; // talvez precise disso
            bool boolstatus = swModel.Extension.SelectByID2(produtoErp.Referencia, "SUBWELDFOLDER", 0, 0, 0, false, 0, null, 0);

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
            Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, 0);
            swCustPropMgr = swFeat.CustomPropertyManager;
          }

          swCustPropMgr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodProduto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

          swModel.Save();

          if (Path.GetFileNameWithoutExtension(produtoErp.PathName) != nameItemPai)
            Sw.App.CloseDoc(produtoErp.PathName);
        }
      }

      produtoErp.Pendencias = new List<PendenciasEngenharia>();
      produtoErp.ImgPendencia = new Bitmap(20, 20);

      AtualizarInfoAlerta(produtoErp);

      dgv.Grid.Rows[startIndex].DefaultCellStyle.ForeColor = dgv.Grid.Rows[startIndex].DefaultCellStyle.SelectionForeColor = corSucesso;
    }

    private async Task PercorrerTreeViewSalvarEngAsync(ContextoDados db, TreeNode node, configuracao_api configApi, IProgress<string> progress) {
      try {
        if (_canceladoComErro) {
          // Toast.Black("Cadastro de engenharia cancelado.");
          return;
        }

        if (!Loader._isWorking) {
          //Toast.Info("Cadastro de produtos cancelado pelo usuário.");
          _canceladoComErro = true;
          return;
        }

        var produtoErp = node.Tag as ProdutoErp;

        if (produtoErp != null) {
          progress.Report($"Gerando Engenharia '{produtoErp.Name}'");

          if (produtoErp.TipoComponente != TipoComponente.ListaMaterial && produtoErp.TipoComponente != TipoComponente.ItemBiblioteca) {
            foreach (TreeNode nodeFilho in node.Nodes) {
              await PercorrerTreeViewSalvarEngAsync(db, nodeFilho, configApi, progress);
            }

            var produto = _produtos.ToList().FirstOrDefault(x => produtoErp.Name == x.Name && produtoErp.Referencia == x.Referencia && produtoErp.Configuracao == x.Configuracao);

            if (produto != null) {
              produtoErp.CodProduto = produto.CodProduto;
              produtoErp.Denominacao = produto.Denominacao;
              //produtoErp.SobremetalCompr = produto.SobremetalCompr;
              //produtoErp.SobremetalLarg = produto.SobremetalLarg;
              produtoErp.Operacoes = produto.Operacoes;
              produtoErp.UnidadeMedida = produto.UnidadeMedida;
              produtoErp.PesoLiquido = produto.PesoLiquido;
              produtoErp.PesoBruto = produto.PesoBruto;
              produtoErp.PesoPadraoNBR = produto.PesoPadraoNBR;
            }

            var engenharia = new Engenharia {
              codEmpresa = configApi.codigoEmpresa,
              codProduto = produtoErp.CodProduto,
              tipoModulo = "S",
              codClassificacao = produtoErp.TipoComponente == TipoComponente.Montagem ? 3 : 4,
              nomeArquivoDesenhoEng = produtoErp.Name,
              descricaoProduto = $"{produtoErp.Denominacao} - {produtoErp.Name}",
              engenhariaFantasma = produtoErp.Fantasma,
              descEngenhariaFantasma = produtoErp.Fantasma ? "Engenharia Fantasma" : "",
            };

            if (node.Nodes.Count > 0) {
              System.Collections.IList list = node.Nodes;
              for (int i = 0; i < list.Count; i++) {
                int index = i + 1;
                TreeNode nodeFilho = (TreeNode)list[i];
                var itemFilho = nodeFilho.Tag as ProdutoErp;

                if (itemFilho != null) {
                  var produtoFilho = _produtos.ToList().FirstOrDefault(x => itemFilho.Name == x.Name && itemFilho.Referencia == x.Referencia && itemFilho.Configuracao == x.Configuracao);
                  if (produtoFilho != null) {
                    itemFilho.UnidadeMedida = produtoFilho.UnidadeMedida;
                    itemFilho.PesoBruto = produtoFilho.PesoBruto;
                    itemFilho.PesoLiquido = produtoFilho.PesoLiquido;
                    itemFilho.SobremetalCompr = produtoFilho.SobremetalCompr;
                    itemFilho.SobremetalLarg = produtoFilho.SobremetalLarg;
                    if (itemFilho.ItemCorte != null)
                      itemFilho.ItemCorte.unidadeMedida = produtoFilho.ItemCorte.unidadeMedida;
                  }

                  var classificacao = itemFilho.CodComponente.StartsWith("10") || itemFilho.CodComponente.StartsWith("20") ? 5 : itemFilho.TipoComponente == TipoComponente.Montagem ? 3 : 4;

                  double qtd = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? itemFilho.Quantidade : 1;
                  double espessura = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? 0 : itemFilho.ItemCorte.Espessura;
                  double larg = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? 0 : itemFilho.ItemCorte.Largura + itemFilho.SobremetalLarg;
                  double compr = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? itemFilho.Comprimento : itemFilho.ItemCorte.Comprimento + itemFilho.SobremetalCompr;
                  string um = itemFilho.TipoComponente != TipoComponente.ListaMaterial ? itemFilho.UnidadeMedida : itemFilho.ItemCorte.unidadeMedida;

                  if (itemFilho.TipoComponente == TipoComponente.ListaMaterial) {
                    switch (um) {
                      case "M":
                      qtd = (compr / 1000); // para cabo de aço
                      break;
                      case "M2":
                      qtd = (larg / 1000) * (compr / 1000);
                      break;
                      case "KG":
                      qtd = produtoErp.PesoBruto;
                      break;
                      default:
                      break;
                    }
                  } else if (itemFilho.TipoComponente == TipoComponente.ItemBiblioteca && compr > 0) {
                    switch (itemFilho.UnidadeMedida) {
                      case "M":
                      qtd = (compr / 1000) * qtd; // para cabo de aço
                      break;
                      case "M2":
                      qtd = (larg / 1000) * (compr / 1000);
                      break;
                      case "KG":
                      qtd = produtoErp.PesoBruto;
                      break;
                      case "L":
                      case "LT":
                      qtd = compr * qtd;
                      break;
                      default:
                      break;
                    }
                  }

                  if (qtd == 0) {
                    _canceladoComErro = true;
                    MsgBox.Show($"Não é possível cadastrar engenharia com quantidade '0' para o item: {itemFilho.Name}.", "Addin LM Projetos",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // não pode cadastrar engenharia com quantidade 0
                  }

                  var componenteEng = new ComponenteEng {
                    seqComponente = index,
                    codInsumo = itemFilho.CodProduto,
                    quantidade = qtd,
                    itemKanban = 0,
                    comprimento = compr,
                    largura = larg,
                    espessura = espessura,
                    percQuebra = 0,
                    codClassificacaoInsumo = classificacao, // 1 = produto, 3 = subconjunto, 4 = peças e 5 = insumo comprado
                  };
                  engenharia.componentes.Add(componenteEng);
                }
              }
            }

            if (produtoErp.Operacoes != null && produtoErp.Operacoes.Count > 0) {
              for (int seqOperacao = 1; seqOperacao <= produtoErp.Operacoes.Count; seqOperacao++) {
                var proc = produtoErp.Operacoes[seqOperacao - 1];

                var processo = Processo.ListaProcessos.FirstOrDefault(x => x.codAxion == proc.processo_id);
                if (processo == null) {
                  Toast.Warning($"Processo '{proc.processo_id}' não encontrado no Axion.");
                  continue;
                }
                var operacaoEng = new OperacaoEng {
                  seqOperacao = seqOperacao,
                  codOperacao = processo.codOperacao,
                  abreviaturaOperacao = processo.abreviatura,
                  numOperadores = proc.qtd_operador,
                  codFaseOperacao = processo.faseProducao,
                  codMascaraMaquina = processo.mascaraMaquina.Replace(".", ""),
                  centroCusto = processo.centroCusto,
                  tempoPadraoOperacao = proc.tempo.FormatarHoraDouble(),
                  tempoPreparacaoOperacao = 0,
                  tipoOperacao = processo.tipoOperacao,
                };
                engenharia.operacoes.Add(operacaoEng);
              }
            }

            var eng = await Api.GetEngenhariaAsync(engenharia.codProduto);

            if (eng == null || eng.statusEngenharia == StatusEngenharia.EmDesenvolvimento) {
              if (eng != null && engenharia.codProduto.StartsWith("4"))
                engenharia.descricaoProduto = eng.descricaoProduto;

              await Api.CadastrarEngenhariaAsync(db, engenharia);
            }
          }
        }
      } catch (Exception ex) {
        _canceladoComErro = true;
        Toast.Error($"Erro ao gerar Engenharia Item: {((ProdutoErp)node.Tag).Name}\n\n{ex.Message}");

      }
    }

    private static void CadastrarAddin(ContextoDados db, ProdutoErp item) {
      try {
        var espessura = 0.0;
        var largura = 0.0;
        var comprimento = 0.0;

        if (item.TipoComponente == TipoComponente.Peca && item.ItemCorte != null) {
          var itemCorte = item.ItemCorte;
          espessura = itemCorte.Espessura;
          largura = itemCorte.Largura;
          comprimento = itemCorte.Comprimento;
        }

        var descricao = item.Denominacao.Length <= 70 ? item.Denominacao : item.Denominacao.Substring(0, 70);
        var referencia = item.Referencia.Length <= 50 ? item.Referencia : item.Referencia.Substring(0, 50);

        var produtoERP = new produto_erp {
          codigo_produto = Convert.ToInt64(item.CodProduto),
          codigo_componente = item.CodComponente,
          tipo_componente = item.TipoComponente,
          name = item.Name,
          descricao = descricao,
          pathname = item.PathName,
          referencia = referencia,
          configuracao = item.Configuracao,
          sobremetal_largura = item.SobremetalLarg,
          sobremetal_comprimento = item.SobremetalCompr,
          espessura = espessura,
          largura = largura,
          comprimento = comprimento,
          peso_liquido = item.PesoLiquido,
          peso_bruto = item.PesoBruto,
          fantasma = item.Fantasma,
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
        LmException.ShowException(ex, $"Item: {item.Name} | Erro ao cadastrar");
      }
    }

    private static void AtualizarAddin(ContextoDados db, ProdutoErp item) {
      try {
        var espessura = 0.0;
        var largura = 0.0;
        var comprimento = 0.0;

        if (item.TipoComponente == TipoComponente.Peca && item.ItemCorte != null) {
          var itemCorte = item.ItemCorte;
          espessura = itemCorte.Espessura;
          largura = itemCorte.Largura;
          comprimento = itemCorte.Comprimento;
        }

        var codProd = Convert.ToInt64(item.CodProduto);
        var produtoErp = db.produto_erp.FirstOrDefault(x => x.codigo_produto == codProd);
        if (produtoErp == null) {
          LmException.ShowException(new Exception("Produto não encontrado no Axion"), "Erro ao atualizar produto no Axion");
          return;
        }

        var descricao = item.Denominacao.Length <= 70 ? item.Denominacao : item.Denominacao.Substring(0, 70);

        // Atualiza os campos necessários
        produtoErp.name = item.Name;
        produtoErp.descricao = descricao;
        produtoErp.pathname = item.PathName;
        produtoErp.referencia = item.Referencia;
        produtoErp.configuracao = item.Configuracao;
        produtoErp.sobremetal_largura = item.SobremetalLarg;
        produtoErp.sobremetal_comprimento = item.SobremetalCompr;
        produtoErp.espessura = espessura;
        produtoErp.largura = largura;
        produtoErp.comprimento = comprimento;
        produtoErp.peso_liquido = item.PesoLiquido;
        produtoErp.peso_bruto = item.PesoBruto;
        produtoErp.fantasma = item.Fantasma;
        produtoErp.alterado_por_id = usuario_alocados.model.usuario_id;
        produtoErp.alterado_em = DateTime.Now;
        db.SaveChanges();

      } catch (Exception ex) {
        LmException.ShowException(ex, $"Item: {item.Name} | Erro ao atualizar");
      }
    }

    private void TxtDenominacao_Leave(object sender, EventArgs e) {
      if (dgv.Grid.CurrentRow == null) {
        return;
      }

      var produtoErp = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;
      if (produtoErp != null)
        produtoErp.Denominacao = txtDescricao.Text;
    }

    private void TxtSmLarg_Leave(object sender, EventArgs e) {
      if (dgv.Grid.CurrentRow == null) {
        return;
      }

      var produtoErp = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;
      if (produtoErp != null && !string.IsNullOrEmpty(txtSmLarg.Text)) {
        var sm = Convert.ToDouble(txtSmLarg.Text);
        if (sm > 500) {
          Toast.Warning("Sobremetal não pode ser maior que 500!");
          txtSmLarg.Text = string.Empty;
          produtoErp.SobremetalLarg = 0;
          txtSmLarg.Focus();
        } else
          produtoErp.SobremetalLarg = sm;
      } else produtoErp.SobremetalLarg = 0;

      ProdutoErp.CalcularPeso(ref produtoErp);

      AtualizarInformacoes(produtoErp);
    }

    private void TxtSmCompr_Leave(object sender, EventArgs e) {
      if (dgv.Grid.CurrentRow == null) {
        return;
      }

      var produtoErp = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;
      if (produtoErp != null && !string.IsNullOrEmpty(txtSmCompr.Text)) {
        var sm = Convert.ToDouble(txtSmCompr.Text);
        if (sm > 500) {
          Toast.Warning("Sobremetal não pode ser maior que 500!");
          txtSmCompr.Text = string.Empty;
          produtoErp.SobremetalCompr = 0;
          txtSmCompr.Focus();
        } else
          produtoErp.SobremetalCompr = sm;
      } else produtoErp.SobremetalCompr = 0;

      ProdutoErp.CalcularPeso(ref produtoErp);

      AtualizarInformacoes(produtoErp);
    }

    private void CarregarGrid() {
      dgv.CarregarGrid(_produtos);

      //VerCoresGrid();
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
        Toast.Warning("Falha ao Atualizar Denominação: \n" + ex.Message);
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

    private void PtbMaterialError_VisibleChanged(object sender, EventArgs e) {
      tmr.Enabled = ptbMaterialError.Visible;
    }

    private bool useFirstImage = true;
    private void Tmr_Tick(object sender, EventArgs e) {
      ptbMaterialError.Image = useFirstImage ? Properties.Resources.error : Properties.Resources.warning;
      useFirstImage = !useFirstImage;
      //tmr.Interval = useFirstImage ? 1100 : 2500;
    }

    private void PtbMaterialError_Click(object sender, EventArgs e) {
      try {
        var produtoErp = dgv.Grid.CurrentRow.DataBoundItem as ProdutoErp;
        var espMat = produtoErp.ItemCorte.Espessura;
        var list = materia_primas.Selecionar(ativo: true, espMat);
        var defatlList = list.ToList().Select(x => new DefaultObject { Codigo = x.CodigoChapa, Descricao = x.DescricaoChapa }).ToList();
        var chapaDesc = MsgBox.InputBox("Selecione a chapa correta", lmValueType: LmCorbieUI.Design.LmValueType.ComboBox, itens: defatlList);
        var mat = list.FirstOrDefault(x => x.DescricaoChapa == chapaDesc);

        if (mat != null) {
          var swModel = (ModelDoc2)Sw.App.ActiveDoc;
          var swPart = (PartDoc)swModel;

          var swModelDocExt = swModel.Extension;
          var swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");

          if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
            var cutList = produtoErp.ItemCorte;
            cutList.Codigo = mat.CodigoChapa;
            cutList.Denominacao = mat.DescricaoChapa;
            cutList.Material = mat.DescricaoMaterial;
            ListaCorte.UpdateCutList(swModel, cutList);

            AtualizarInfoAlerta(produtoErp);

            AtualizarInformacoes(produtoErp);

            ProdutoErp.CalcularPeso(ref produtoErp);

            lblCodMat.Text = mat.CodigoChapa.ToString();
            lblDescMat.Text = mat.DescricaoChapa;
          }

          ptbMaterialError.Visible = false;

          swModel.Save();

          ProdutoErp.RemoverPendencia(produtoErp, PendenciasEngenharia.MateriaErrado);

          Toast.Success("Atualizado com sucesso!");
        }

      } catch (Exception ex) {
        Toast.Error(ex.Message);
      }
    }

    private void LblCodigoProduto_Click(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(lblCodigoProduto.Text)) {
        Clipboard.SetText(lblCodigoProduto.Text);
        MsgBox.ShowToolTip(lblCodigoProduto, "Código copiado para área de transferência!");
      }
    }

    private void Dgv_ProcurarTextChanged(object sender, EventArgs e) {
      CarregarGrid();
    }

    private void Dgv_Sorted(object sender, EventArgs e) {
      //VerCoresGrid();
    }
  }
}
