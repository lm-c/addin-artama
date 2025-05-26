using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using LmCorbieUI;
using System.Windows.Forms;

namespace AddinArtama {
  internal partial class Api {
    internal class Engenharia {
      public int codEmpresa { get; set; }
      public string codProduto { get; set; }
      public string descricaoProduto { get; set; } = "";
      public string narrativaLinha1 { get; set; } = "";
      public string narrativaLinha2 { get; set; } = "";
      public string narrativaLinha3 { get; set; } = "";
      public string narrativaLinha4 { get; set; } = "";
      public string tipoModulo { get; set; } = "E";
      public int codClassificacao { get; set; }
      public int statusEngenharia { get; set; }
      public string nomeArquivoDesenhoEng { get; set; }
      public bool engenhariaFantasma { get; set; }
      public string descEngenhariaFantasma { get; set; }
      public string loteProjecaoCusto { get; set; }
      public List<ComponenteEng> componentes = new List<ComponenteEng>();
      public List<OperacaoEng> operacoes = new List<OperacaoEng>();
    }

    internal class ComponenteEng {
      public int seqComponente { get; set; }
      public string codInsumo { get; set; }
      public double quantidade { get; set; }
      public int itemKanban { get; set; }
      public double comprimento { get; set; }
      public double largura { get; set; }
      public double espessura { get; set; }
      public double percQuebra { get; set; }
      public int codClassificacaoInsumo { get; set; }
      public string codPEInsumo { get; set; }
      public int seqOperacaoConsumo { get; set; }
    }

    internal class OperacaoEng {
      public int seqOperacao { get; set; }
      public int codOperacao { get; set; }
      public string abreviaturaOperacao { get; set; }
      public double numOperadores { get; set; }
      public int codFaseOperacao { get; set; }
      public string codMascaraMaquina { get; set; }
      public string centroCusto { get; set; }
      public double tempoPadraoOperacao { get; set; }
      public double tempoPreparacaoOperacao { get; set; }
    }

    internal static async Task<bool> CadastrarEngenhariaAsync(Engenharia engenharia) {
      var configApi = configuracao_api.Selecionar();

      try {
        JObject jsonObject = new JObject();

        var client = Api.GetClient(modulo: "ppcppadrao", endpoint: $"pendenciaEngenharia");
        var request = Api.CreateRequest(Method.PUT);

        var bodyObject = "" +
          "{" +
             $"\"codEmpresa\": {engenharia.codEmpresa}," +
             $"\"codProduto\": \"{engenharia.codProduto}\"," +
             $"\"narrativaLinha1\": \"{engenharia.narrativaLinha1}\"," +
             $"\"narrativaLinha2\": \"{engenharia.narrativaLinha2}\"," +
             $"\"narrativaLinha3\": \"{engenharia.narrativaLinha3}\"," +
             $"\"narrativaLinha4\": \"{engenharia.narrativaLinha4}\"," +
             $"\"tipoModulo\": \"{engenharia.tipoModulo}\"," +
             $"\"codClassificacao\": {engenharia.codClassificacao}," +
             $"\"nomeArquivoDesenhoEng\": \"{engenharia.nomeArquivoDesenhoEng}\"," +
             $"\"engenhariaFantasma\": {engenharia.engenhariaFantasma.ToString().ToLower()}," +
             $"\"descEngenhariaFantasma\": \"{engenharia.descEngenhariaFantasma}\"," +

             "\"componentes\": [";

        foreach (var componente in engenharia.componentes) {
          bodyObject += "{" +
              $"\"seqComponente\": {componente.seqComponente}," +
              $"\"codInsumo\": \"{componente.codInsumo}\"," +
              $"\"quantidade\": {componente.quantidade}," +
              $"\"itemKanban\": {componente.itemKanban}," +
              $"\"comprimento\": {componente.comprimento}," +
              $"\"largura\": {componente.largura}," +
              $"\"espessura\": {componente.espessura}," +
              $"\"percQuebra\": {componente.percQuebra}," +
              $"\"codClassificacaoInsumo\": {componente.codClassificacaoInsumo}," +
              //$"\"codPEInsumo\": \"{componente.codPEInsumo}\"," +
              $"\"seqOperacaoConsumo\": {componente.seqOperacaoConsumo}" +
              "},";
        }

        bodyObject = bodyObject.TrimEnd(',') + "]," +

        "\"operacoes\": [";
        foreach (var operacao in engenharia.operacoes) {
          bodyObject += "{" +
              $"\"seqOperacao\": {operacao.seqOperacao}," +
              $"\"codOperacao\": {operacao.codOperacao}," +
              $"\"abreviaturaOperacao\": \"{operacao.abreviaturaOperacao}\"," +
              $"\"numOperadores\": {operacao.numOperadores}," +
              $"\"codFaseOperacao\": {operacao.codFaseOperacao}," +
              $"\"codMascaraMaquina\": \"{operacao.codMascaraMaquina}\"," +
              $"\"centroCusto\": {operacao.centroCusto}," +
              $"\"tempoPadraoOperacao\": {operacao.tempoPadraoOperacao.ToString().Replace(",",".")}," +
              $"\"tempoPreparacaoOperacao\": {operacao.tempoPreparacaoOperacao.ToString().Replace(",", ".")}" +
              "},";
        }
        bodyObject = bodyObject.TrimEnd(',') + "]" +
          "}";

        request.AddJsonBody(bodyObject);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          jsonObject = JObject.Parse(responseData);
          configuracao_api.SalvarAsync(configApi).Wait();
          return true;
        } else {
          var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
          var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro ao Cadastrar Engenharia";
          throw new Exception($"{response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        throw new Exception($"{ex.Message}");
      }

    }

    internal static async Task<Engenharia> GetEngenhariaAsync(string codigo) {
      Engenharia _return = null;

      try {
        var client = Api.GetClient(modulo: "ppcppadrao", endpoint: $"engenharia/{codigo}");
        var request = Api.CreateRequest(Method.GET);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          var jsonObject = JObject.Parse(responseData);

          _return = new Engenharia {
            descricaoProduto = jsonObject["descricao"]?.ToString(),
            narrativaLinha1 = jsonObject["narrativa"]?.ToString(),
            codClassificacao = jsonObject["codClassificacao"]?.ToObject<int>() ?? 0,
            statusEngenharia = jsonObject["statusEngenharia"]?.ToObject<int>() ?? 0,

            componentes = jsonObject["componentes"]?.Select(c => new ComponenteEng {
              seqComponente = c["seqComponente"]?.ToObject<int>() ?? 0,
              codInsumo = c["codItem"]?.ToString(),
              quantidade = c["quantidade"]?.ToObject<double>() ?? 0,
              //itemKanban = c["kanban"]?.ToObject<int>() ?? 0,
              //centroCusto = c["centroCusto"]?.ToString(),
              comprimento = c["comprimento"]?.ToObject<double>() ?? 0,
              largura = c["largura"]?.ToObject<double>() ?? 0,
              espessura = c["espessura"]?.ToObject<double>() ?? 0,
              percQuebra = c["percQuebra"]?.ToObject<double>() ?? 0,
              codClassificacaoInsumo = c["codClassificacao"]?.ToObject<int>() ?? 0,
              seqOperacaoConsumo = c["seqOperacional"]?.ToObject<int>() ?? 0,
            }).ToList() ?? new List<ComponenteEng>(),

            operacoes = jsonObject["operacoes"]?.Select(o => new OperacaoEng {
              seqOperacao = o["seqOperacao"]?.ToObject<int>() ?? 0,
              codOperacao = o["codOperacao"]?.ToObject<int>() ?? 0,
              abreviaturaOperacao = o["abreviaturaOperacao"]?.ToString(),
              numOperadores = o["numOpradores"]?.ToObject<double>() ?? 0,
              codFaseOperacao = o["faseProducao"]?.ToObject<int>() ?? 0,
              codMascaraMaquina = o["mascMaquina"]?.ToString(),
              //codLinhaProducao = o["codLinha"]?.ToObject<int>() ?? 0,
              tempoPadraoOperacao = o["tempoPadrao"]?.ToObject<double>() ?? 0,
              tempoPreparacaoOperacao = o["tempoPreparacao"]?.ToObject<double>() ?? 0,
              //centroCusto = o["centroCusto"]?.ToString(),
            }).ToList() ?? new List<OperacaoEng>()
          };

        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar item genérico");
      }

      return _return;
    }
  }
}
