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

namespace AddinArtama {
  internal class Processo {
    [DisplayName("Código Operação")]
    [LarguraColunaGrid(120)]
    [DataObjectField(true, false)]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public int codOperacao { get; set; }

    [DisplayName("Abreviatura")]
    [LarguraColunaGrid(120)]
    public string abreviatura { get; set; }

    [DisplayName("Descrição Operação")]
    [LarguraColunaGrid(200)]
    [DataObjectField(false, true)]
    public string descrOperacao { get; set; }
    
    [DisplayName("Código Máquina")]
    [LarguraColunaGrid(120)]
    public int codMaquina { get; set; }

    [DisplayName("Descrição Máquina")]
    [LarguraColunaGrid(150)]
    [DataObjectField(false, true)]
    public string descrMaquina { get; set; }
    
    [DisplayName("Mascara Máquina")]
    [LarguraColunaGrid(120)]
    [DataObjectField(false, true)]
    public string mascaraMaquina { get; set; }

    [DisplayName("Tipo")]
    [LarguraColunaGrid(100)]
    public string tipo { get; set; }

    [DisplayName("Fase produção")]
    [LarguraColunaGrid(120)]
    public int faseProducao { get; set; }

    public static List<Processo> ListaProcessos { get; set; } = new List<Processo>();

    public static async void Carregar() {
      try {
        /* teste pegar item genérico */
        //var item = await Api.GetItemGenericoAsync("14");
        /* teste pegar mascaras */
        //var mascaras = await Api.GetMascarasAsync();
        /* teste duplicar item */
        //Api.ItemGenerico item = new Api.ItemGenerico();
        //using (ContextoDados db = new ContextoDados()) {
        //  item.Nome = "TESTE LEO CADASTRO ADDIN";
        //  item.UnidadeMedida = "CJ";
        //  item.Tipo = TipoDucumento.Montagem;
        //  item.PesoBruto = 12.5;
        //  item.PesoLiquido = 11.33;

        //  Api.CadasterItemGenericoAsync(item);
        //}

        var maquinas = await Api.GetMaquinasAsync();
        var operacoes = await Api.GetOpsAsync();
        var procs = processos.SelecionarTodos();

        foreach (var processo in procs) {
          var operacao = operacoes.FirstOrDefault(x => x.CodOperacao == processo.codigo_operacao);
          var maquina = maquinas.FirstOrDefault(x => x.codMaquina == processo.codigo_maquina);
          ListaProcessos.Add(new Processo {
            codOperacao = operacao.CodOperacao,
            abreviatura = operacao.Abreviatura,
            descrOperacao = operacao.Descricao.Replace("\\", "_").Replace("/", "_"),
            descrMaquina = maquina.descReduzida.Replace("\\", "_").Replace("/", "_"),
            mascaraMaquina = maquina.mascara,
            codMaquina = maquina.codMaquina,
            tipo = operacao.Tipo,
            faseProducao = operacao.FaseProducao ?? 0 // Default para 0 se for nulo
          });
        }
        ListaProcessos = ListaProcessos.OrderByDescending(x => x.tipo).ThenBy(x => x.descrOperacao).ToList();
      } catch (Exception ex) {
        Toast.Error("Erro ao Selecionar Processos");
      }
    }
  }

  // Adicione uma classe para modelar a resposta JSON

}
