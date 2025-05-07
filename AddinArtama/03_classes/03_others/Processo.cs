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
    [DisplayName("Código")]
    [LarguraColunaGrid(100)]
    [DataObjectField(true, false)]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public int codOperacao { get; set; }

    [DisplayName("Abreviatura")]
    [LarguraColunaGrid(120)]
    [DataObjectField(false, true)]
    public string abreviatura { get; set; }

    [DisplayName("Descrição")]
    [LarguraColunaGrid(500)]
    [DataObjectField(false, true)]
    public string descricao { get; set; }

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

        var operacoes = await Api.GetOpsAsync();
        foreach (var operacao in operacoes) {
          ListaProcessos.Add(new Processo {
            codOperacao = operacao.CodOperacao,
            abreviatura = operacao.Abreviatura,
            descricao = operacao.Descricao.Replace("\\", "_").Replace("/", "_"),
            tipo = operacao.Tipo,
            faseProducao = operacao.FaseProducao ?? 0 // Default para 0 se for nulo
          });
        }
        ListaProcessos = ListaProcessos.OrderByDescending(x => x.tipo).ThenBy(x => x.descricao).ToList();
      } catch (Exception ex) {
        Toast.Error("Erro ao Selecionar Processos");
      }
    }
  }

  // Adicione uma classe para modelar a resposta JSON

}
