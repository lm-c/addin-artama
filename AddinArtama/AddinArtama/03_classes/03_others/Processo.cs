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

namespace AddinArtama {
  internal class Processo {
    [DisplayName("Código Axion")]
    [LarguraColunaGrid(120)]
    public int codAxion { get; set; }

    [DisplayName("Cód. Oper.")]
    [LarguraColunaGrid(120)]
    [DataObjectField(true, false)]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public int codOperacao { get; set; }

    [Browsable(false)]
    [DisplayName("Abrev.")]
    [LarguraColunaGrid(80)]
    public string abreviatura { get; set; }

    [DisplayName("Descrição Operação")]
    [LarguraColunaGrid(200)]
    [DataObjectField(false, true)]
    public string descrOperacao { get; set; }

    [DisplayName("Cód. Máq.")]
    [LarguraColunaGrid(100)]
    public int? codMaquina { get; set; }

    [DisplayName("Descrição Máquina")]
    [LarguraColunaGrid(200)]
    public string descrMaquina { get; set; }

    [DisplayName("Másc. Máqu.")]
    [LarguraColunaGrid(100)]
    public string mascaraMaquina { get; set; }

    [DisplayName("Tipo")]
    [LarguraColunaGrid(100)]
    public TipoOperacao tipoOperacao { get; set; }

    [DisplayName("Centro de Custo")]
    [LarguraColunaGrid(100)]
    public string centroCusto { get; set; }

    [Browsable(false)]
    [DisplayName("Fase Prod.")]
    [LarguraColunaGrid(100)]
    public int faseProducao { get; set; }

    public static List<Processo> ListaProcessos { get; set; } = new List<Processo>();
    public static List<Api.Operacao> ListaOperacoesERP { get; set; } = new List<Api.Operacao>();
    public static List<Api.Maquina> ListaMaquinasERP { get; set; } = new List<Api.Maquina>();

    public static async Task Carregar() {
      try {
        ListaProcessos = new List<Processo>();

        ListaOperacoesERP = await Api.GetOpsAsync();
        ListaMaquinasERP = await Api.GetMaquinasAsync();
        var procs = processos.SelecionarTodos();

        foreach (var processo in procs) {
          var operacao = ListaOperacoesERP.FirstOrDefault(x => x.codOperacao == processo.codigo_operacao);
          var maquina = ListaMaquinasERP.FirstOrDefault(x => x.codMaquina == processo.codigo_maquina);

          if (operacao == null)
            continue;

          ListaProcessos.Add(new Processo {
            codAxion = processo.id,
            codOperacao = operacao.codOperacao,
            abreviatura = operacao.abreviatura,
            descrOperacao = operacao.descricao.Replace("\\", "_").Replace("/", "_"),
            descrMaquina = maquina?.descricao.Replace("\\", "_").Replace("/", "_"),
            mascaraMaquina = maquina?.mascara,
            codMaquina = maquina?.codMaquina,
            tipoOperacao = operacao.tipo == "Interno" ? TipoOperacao.Interna : TipoOperacao.Externa,
            centroCusto = maquina?.centroCusto,
            faseProducao = operacao.faseProducao ?? 0, // Default para 0 se for nulo
          });
        }
        ListaProcessos = ListaProcessos.OrderByDescending(x => x.descrOperacao).ToList();
      } catch (Exception ex) {
        Toast.Error("Erro ao Selecionar Processos");
      }
    }

  }
}
