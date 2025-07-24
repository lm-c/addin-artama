using LmCorbieUI.Metodos.AtributosCustomizados;
using LmCorbieUI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Ubiety.Dns.Core;
using Newtonsoft.Json;
using System.Linq;
using LmCorbieUI.Metodos;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;
using System.IO;
using static AddinArtama.Api;
using System.Globalization;
using System.Data.Entity.Infrastructure;

namespace AddinArtama {
  internal class ProcessoNaoSeriado {
    [DisplayName("Código Axion")]
    [LarguraColunaGrid(120)]
    public int Id { get; set; }

    [DisplayName("Cód. Oper.")]
    [LarguraColunaGrid(120)]
    [DataObjectField(true, false)]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public int Codigo { get; set; }

    [Browsable(false)]
    [DisplayName("Abrev.")]
    [LarguraColunaGrid(80)]
    public string abreviatura { get; set; }

    [DisplayName("Descrição Operação")]
    [LarguraColunaGrid(200)]
    [DataObjectField(false, true)]
    public string Descricao { get; set; }

    [DisplayName("Tipo Sequência")]
    [LarguraColunaGrid(100)]
    public TipoSequencia TipoSequencia { get; set; }

    [Browsable(false)]
    public bool GerarDxf { get; set; }

    [Browsable(false)]
    public bool ImprimirFilhos { get; set; }

    public static List<ProcessoNaoSeriado> ListaProcessos { get; set; } = new List<ProcessoNaoSeriado>();

    public static async Task Carregar() {
      try {
        ListaProcessos = new List<ProcessoNaoSeriado>();

        using (ContextoDados db = new ContextoDados()) {
          var procs = db.processos_nao_seriado.OrderBy(x => x.tipo_sequencia).ThenBy(x => x.codigo).ToList();

          foreach (var processo in procs) {

            ListaProcessos.Add(new ProcessoNaoSeriado {
              Id = processo.id,
              Codigo = processo.codigo,
              Descricao = processo.descricao,
              TipoSequencia = (TipoSequencia)processo.tipo_sequencia,
              GerarDxf = processo.gerar_dxf,
              ImprimirFilhos = processo.imprimir_filhos,
            });
          }
        }

      } catch (Exception ex) {
        Toast.Error("Erro ao Selecionar Processos");
      }
    }

    public static async Task<SortableBindingList<ProdutoErp>> GetProdutos() {
      var _listaProduto = new List<ProdutoErp>();

      try {
        using (ContextoDados db = new ContextoDados()) {

          var swModel = (ModelDoc2)Sw.App.ActiveDoc;

          ModelDocExtension swModelDocExt;
          CustomPropertyManager swCustPropMngr = default(CustomPropertyManager);

          var pathName = swModel.GetPathName();
          var name = Path.GetFileNameWithoutExtension(pathName);

          ConfigurationManager swConfMgr;
          Configuration swConf;
          //Component2 swRootComp;

          swModelDocExt = swModel.Extension;
          swConfMgr = swModel.ConfigurationManager;
          swConf = swConfMgr.ActiveConfiguration;
          //swRootComp = swConf.GetRootComponent3(true);

          var tipo = swModel.GetType();

          ProdutoErp produtoErp = new ProdutoErp {
            Nivel = "1",
            PathName = pathName,
            Name = name,
            Referencia = name,
            TipoComponente = pathName.ToUpper().EndsWith("SLDASM") ? TipoComponente.Montagem : TipoComponente.Peca,
          };

          var desenhoExiste = File.Exists(pathName.ToUpper().Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW");
          produtoErp.Img3D = produtoErp.TipoComponente == TipoComponente.Peca ? Properties.Resources.part : Properties.Resources.assembly;
          produtoErp.Img2D = desenhoExiste ? Properties.Resources.draw : Properties.Resources.not_draw;

          string valOut;
          string resolvedValOut;

          swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");

          swCustPropMngr.Get2("Código Produto", out valOut, out resolvedValOut);
          produtoErp.CodProduto = resolvedValOut;
          swCustPropMngr.Get2("Denominação", out valOut, out resolvedValOut);
          produtoErp.Denominacao = resolvedValOut;  
          swCustPropMngr.Get2("Componente", out valOut, out resolvedValOut);
          produtoErp.CodComponente = resolvedValOut;
          swCustPropMngr.Get2("Massa (kg)", out valOut, out resolvedValOut);
          double.TryParse(resolvedValOut, NumberStyles.Any, CultureInfo.InvariantCulture, out double massa);
          produtoErp.PesoLiquido = Math.Round(massa, 4);
          produtoErp.PesoBruto = produtoErp.PesoLiquido;
          swCustPropMngr.Get2("OPERAÇÃO", out valOut, out resolvedValOut);
          var ops = resolvedValOut;
          swCustPropMngr.Get2("Fantasma", out valOut, out resolvedValOut);
          produtoErp.Fantasma = !string.IsNullOrEmpty(resolvedValOut) && resolvedValOut.ToLower() == "sim";

          if (produtoErp.Fantasma)
            produtoErp.ImgFantasma = Properties.Resources.fantasma;

          if (pathName.ToUpper().Contains("BIBLIOTECA") ||
            produtoErp.CodComponente.StartsWith("10") ||
            produtoErp.CodComponente.StartsWith("20")) {
            Toast.Warning("Recurso indisponivel para o componentes de biblioteca.");
            return new SortableBindingList<ProdutoErp>();
          }

          swCustPropMngr = swModelDocExt.get_CustomPropertyManager(swConf.Name);

          if (string.IsNullOrEmpty(produtoErp.CodComponente)) {
            swCustPropMngr.Get2("Componente", out valOut, out resolvedValOut);
            produtoErp.CodComponente = resolvedValOut;
          }

          if (string.IsNullOrEmpty(produtoErp.Denominacao)) {
            swCustPropMngr.Get2("Denominação", out valOut, out resolvedValOut);
            produtoErp.Denominacao = resolvedValOut;
          }

          if (produtoErp.Name.StartsWith("40") && produtoErp.Name.Contains(produtoErp.CodComponente))
            produtoErp.CodComponente = produtoErp.Name.Split(' ')[0];

          if (string.IsNullOrEmpty(produtoErp.CodProduto) && produtoErp.CodComponente.StartsWith("40")) {
            swCustPropMngr = swModelDocExt.get_CustomPropertyManager("");
            swCustPropMngr.Add3("Código Produto", (int)swCustomInfoType_e.swCustomInfoText, produtoErp.CodComponente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
            produtoErp.CodProduto = produtoErp.CodComponente;
          }

          produtoErp.Quantidade = 1;

          produtoErp.Configuracao = swConf.Name;

          PegarOperacoes(_listaProduto, produtoErp, ops);

          if (tipo == (int)swDocumentTypes_e.swDocASSEMBLY) {
            _listaProduto.Add(produtoErp);

            // Inserir lista de material e pegar dados
            string templateGeral = $"{Application.StartupPath}\\01 - Addin LM\\ListaCompleta.sldbomtbt";
            int BomTypeGeral = (int)swBomType_e.swBomType_Indented;
            int NumberingType = (int)swNumberingType_e.swNumberingType_Detailed;
            bool DetailedCutList = false;
            var swBOMAnnotationGeral = swModelDocExt.InsertBomTable3(templateGeral, 0, 1, BomTypeGeral, swConf.Name, false, NumberingType, DetailedCutList);
            await PegaDadosListaGeralAsync(db, swBOMAnnotationGeral, _listaProduto);
            ListaCorte.ExcluirLista(swModel);
          } else {
            int status = 0;
            int warnings = 0;
            int errors = 0;

            var itensCorte = ListaCorte.GetCutList(swModel, produtoErp.PathName);

            if (produtoErp.TipoComponente == TipoComponente.Peca) {
              for (int indiceLista = 0; indiceLista < itensCorte.Count; indiceLista++) {
                ListaCorte itemCorte = itensCorte[indiceLista];
                var indiceNome = indiceLista + 1;
                var sufixo = itensCorte.Count > 1 ? $" - P{indiceNome}" : "";

                // add soldagem como peça
                var item = new ProdutoErp {
                  PathName = produtoErp.PathName,
                  Name = produtoErp.Name + sufixo,
                  Denominacao = produtoErp.Denominacao,
                  Referencia = itemCorte.NomeLista,
                  CodComponente = produtoErp.CodComponente,
                  CodProduto = itemCorte.CodProduto,
                  Img3D = Properties.Resources.part,
                  Img2D = produtoErp.Img2D,
                  TipoComponente = TipoComponente.Peca,
                  Nivel = produtoErp.Nivel + "." + indiceNome,
                  Configuracao = produtoErp.Configuracao,
                  Quantidade = itemCorte.Quantidade,
                  PesoBruto = itemCorte.Massa,
                  PesoLiquido = itemCorte.Massa,
                  ItemCorte = itemCorte,
                };

                PegarOperacoes(_listaProduto, item, itemCorte.Operacao);

                if (produtoErp.Name.Length + sufixo.Length > 50)
                  produtoErp.Name = produtoErp.Name.Substring(0, 50 - sufixo.Length) + sufixo;

                _listaProduto.Add(item);
              }
            } else
              _listaProduto.Add(produtoErp);
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao carregar componentes.\n\n{ex.Message}", "Addin LM Projetos",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      } finally {
        MsgBox.CloseWaitMessage();
      }
      return new SortableBindingList<ProdutoErp>(_listaProduto);
    }

    private static async Task PegaDadosListaGeralAsync(ContextoDados db, BomTableAnnotation swBOMAnnotation, List<ProdutoErp> _listaProduto) {
      string nameShort = "";
      try {
        int status = 0;
        int warnings = 0;
        string[] vModelPathNames = null;
        string strItemNumber = null;
        string strPartNumber = null;
        var swTableAnnotation = (TableAnnotation)swBOMAnnotation;

        int lStartRow = 1;

        if (!(swTableAnnotation.TitleVisible == false)) {
          lStartRow = 2;
        }

        var swBOMFeature = swBOMAnnotation.BomFeature;

        await Loader.ShowDuringOperation(
            "Iniciando leitura da tabela...",
            (progress2) => {
              var total = swTableAnnotation.TotalRowCount;
              for (int i = lStartRow; i < swTableAnnotation.TotalRowCount; i++) {
                if (!Loader._isWorking) {
                  _listaProduto = new List<ProdutoErp>();
                  MsgBox.Show("Operação cancelada pelo usuário.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  return Task.FromResult("Cancelado");
                }

                // rotoina ler lista
                vModelPathNames = (string[])swBOMAnnotation.GetModelPathNames(i, out strItemNumber, out strPartNumber);

                if (vModelPathNames != null) {
                  var produtoErp = new ProdutoErp();
                  string ptNm = vModelPathNames[0];

                  produtoErp.PathName = ptNm;
                  produtoErp.Name = nameShort = Path.GetFileNameWithoutExtension(produtoErp.PathName);
                  if (produtoErp.Name.Length > 50)
                    produtoErp.Name = produtoErp.Name.Substring(0, 50);

                  progress2.Report(($"Lendo componentes... \n\n{nameShort}", i, total));

                  if (!File.Exists(produtoErp.PathName))
                    continue;

                  var qtd = Convert.ToInt32(swTableAnnotation.get_Text(i, 1));

                  produtoErp.TipoComponente = ptNm.ToUpper().EndsWith("SLDPRT") ? TipoComponente.Peca : TipoComponente.Montagem;
                  var nivel = swTableAnnotation.get_Text(i, 0).Trim();
                  produtoErp.Nivel = "1." + nivel;
                  produtoErp.Quantidade = qtd;
                  produtoErp.CodComponente = swTableAnnotation.get_Text(i, 3).Trim();
                  produtoErp.CodProduto = swTableAnnotation.get_Text(i, 4).Trim();
                  produtoErp.Denominacao = swTableAnnotation.get_Text(i, 6);
                  produtoErp.Configuracao = swTableAnnotation.get_Text(i, 11);
                  var ops = swTableAnnotation.get_Text(i, 12);
                  produtoErp.Referencia = produtoErp.Name;
                  var massa = swTableAnnotation.get_Text(i, 13);
                  produtoErp.PesoLiquido = !string.IsNullOrEmpty(massa) ? Convert.ToDouble(massa.Replace(".", ",")) : 0;
                  produtoErp.PesoBruto = produtoErp.PesoLiquido;
                  var fantasma = swTableAnnotation.get_Text(i, 14);
                  produtoErp.Fantasma = !string.IsNullOrEmpty(fantasma) && fantasma.ToLower() == "sim";

                  if (produtoErp.Fantasma)
                    produtoErp.ImgFantasma = Properties.Resources.fantasma;

                  produtoErp.TipoComponente = ptNm.ToUpper().Contains("BIBLIOTECA") ||
                    produtoErp.CodComponente.StartsWith("10") ||
                    produtoErp.CodComponente.StartsWith("20") ||
                    produtoErp.CodComponente.StartsWith("30")
                    ? TipoComponente.ItemBiblioteca
                    : ptNm.ToUpper().EndsWith("SLDPRT")
                    ? TipoComponente.Peca : TipoComponente.Montagem;

                  if (produtoErp.TipoComponente == TipoComponente.ItemBiblioteca)
                    continue;

                  int swTipo = ptNm.ToUpper().EndsWith("SLDPRT") ? (int)swDocumentTypes_e.swDocPART : (int)swDocumentTypes_e.swDocASSEMBLY;

                  PegarOperacoes(_listaProduto, produtoErp, ops);

                  var swModel = Sw.App.OpenDoc6(produtoErp.PathName,
                  swTipo, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

                  var itensCorte = ListaCorte.GetCutList(swModel, produtoErp.PathName);

                  var desenhoExiste = File.Exists(ptNm.ToUpper().Substring(0, produtoErp.PathName.Length - 6) + "SLDDRW");
                  produtoErp.Img3D = produtoErp.TipoComponente == TipoComponente.ItemBiblioteca
                    ? Properties.Resources.toolbox_item
                    : produtoErp.TipoComponente == TipoComponente.Peca
                    ? Properties.Resources.part
                    : Properties.Resources.assembly;
                  produtoErp.Img2D = desenhoExiste ? Properties.Resources.draw : Properties.Resources.not_draw;

                  if (produtoErp.TipoComponente == TipoComponente.Peca) {
                    for (int indiceLista = 0; indiceLista < itensCorte.Count; indiceLista++) {
                      ListaCorte itemCorte = itensCorte[indiceLista];
                      var indiceNome = indiceLista + 1;
                      var sufixo = itensCorte.Count > 1 ? $" - P{indiceNome}" : "";

                      var item = new ProdutoErp {
                        PathName = produtoErp.PathName,
                        Name = produtoErp.Name + sufixo,
                        Denominacao = produtoErp.Denominacao,
                        Referencia = itemCorte.NomeLista,
                        CodComponente = produtoErp.CodComponente,
                        CodProduto = itemCorte.CodProduto,
                        Img3D = Properties.Resources.part,
                        Img2D = produtoErp.Img2D,
                        TipoComponente = TipoComponente.Peca,
                        Nivel = produtoErp.Nivel + "." + indiceNome,
                        Configuracao = produtoErp.Configuracao,
                        Quantidade = itemCorte.Quantidade,
                        PesoLiquido = itemCorte.Massa,
                        PesoBruto = itemCorte.Massa,
                        ItemCorte = itemCorte
                      };

                      PegarOperacoes(_listaProduto, item, itemCorte.Operacao);

                      if (!_listaProduto.Any(x => x.Name == item.Name && x.Referencia == item.Referencia && x.Configuracao == item.Configuracao))
                        _listaProduto.Add(item);
                    }
                  } else if (!_listaProduto.Any(x => x.Name == produtoErp.Name && x.Referencia == produtoErp.Referencia && x.Configuracao == produtoErp.Configuracao))
                    _listaProduto.Add(produtoErp);


                  Sw.App.CloseDoc(produtoErp.PathName);
                }
              }
              return Task.FromResult("concluído");
            },
            100
        );
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao pegar dados da Lista\n\nItem: {nameShort}\n\n{ex.Message}", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void PegarOperacoes(List<ProdutoErp> _listaProduto, ProdutoErp produtoErp, string ops) {
      if (!string.IsNullOrEmpty(ops)) {
        var splOps = ops.Split('/');
        foreach (var op in splOps) {
          if (int.TryParse(op, out int opId)) {
            var processo = ProcessoNaoSeriado.ListaProcessos.FirstOrDefault(x => x.Codigo == opId);

            if (processo == null)
              continue;

            produtoErp.Operacoes.Add(new produto_erp_operacao {
              qtd_operador = 1,
              processo_id = processo.Codigo,
              name = produtoErp.Name,
              referencia = produtoErp.Referencia,
              sequencia = _listaProduto.Count + 1
            });
          }
        }
      }
    }
  }
}
