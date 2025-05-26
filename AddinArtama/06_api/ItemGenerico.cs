using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using static AddinArtama.Api;
using LmCorbieUI;
using System.Windows.Forms;

namespace AddinArtama {
  internal partial class Api {
    internal class ItemGenerico {
      public string Nome { get; set; }
      public string UnidadeMedida { get; set; }
      public double PesoBruto { get; set; }
      public double PesoLiquido { get; set; }
      public int Situacao { get; set; }
      public TipoDucumento Tipo { get; set; }
    }

    internal static async Task<string> CadasterItemGenericoAsync(ItemGenerico itemGenerico) {
      var configApi = configuracao_api.Selecionar();

      try {
        var tentativas = 1;

        var tipoMascara = string.Empty;
        var sequencial = configApi.sequencial += 1;
        if (itemGenerico.Tipo == TipoDucumento.Peca) {
          tipoMascara = configApi.tipo_peca;
        } else {
          tipoMascara = configApi.tipo_montagem;
        }

        /* inicio variaveis */
        var codigo = $"{configApi.grupo}{configApi.subgrupo}{configApi.familia}{sequencial:0000}";
        var mascara = $"{configApi.grupo}{configApi.subgrupo}{tipoMascara}{configApi.familia}{configApi.classificacao}{sequencial:0000}";
        var nome = itemGenerico.Nome;
        var unidadeMedida = itemGenerico.UnidadeMedida;
        var pesoBruto = itemGenerico.PesoBruto;
        var pesoLiquido = itemGenerico.PesoLiquido;
        var situacao = itemGenerico.Situacao;

        var classificacaoFiscal = configApi.classificacaoFiscal;
        var finalidade = configApi.finalidade;
        var origem = configApi.origem;
        var tipo = configApi.tipo;
        var procedencia = configApi.procedencia;
        var perComissao = configApi.perComissao;
        var perIPI = configApi.perIPI;
        var situacaoICMS = configApi.situacaoICMS;
        var codNatureza = configApi.codNatureza;
        var codigoICMS = configApi.codigoICMS;
        var codigoIPI = configApi.codigoIPI;
        var codContaContabil = configApi.codContaContabil;
        var tipoControleSaida = configApi.tipoControleSaida;
        /* fim variaveis */

        JObject jsonObject = new JObject();

        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/{codigo}");
        var request = Api.CreateRequest(Method.GET);
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          do {
            if (tentativas == 10) {
              MsgBox.Show($"Erro ao cadastrar item genérico, tente novamente mais tarde.", "Addin LM Projetos",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
              return string.Empty;
            }

            sequencial = configApi.sequencial += 1;
            codigo = $"{configApi.grupo}{configApi.subgrupo}{configApi.familia}{sequencial:0000}";
            mascara = $"{configApi.grupo}{configApi.subgrupo}{tipoMascara}{configApi.familia}{configApi.classificacao}{sequencial:0000}";

            client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/{codigo}");
            request = Api.CreateRequest(Method.GET);
            response = await client.ExecuteAsync(request);

            if (response.IsSuccessful) {
              tentativas++;
            } else break;
          } while (tentativas <= 10);
        }

        client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico");
        request = Api.CreateRequest(Method.PUT);

        var bodyObject = "" +
          "{" +
             $"\"codigo\": {codigo}," +
             $"\"nome\": \"{nome}\"," +
             $"\"unidadeMedida\": \"{unidadeMedida}\"," +
             $"\"classificacaoFiscal\": \"{classificacaoFiscal}\"," +
             $"\"finalidade\": {finalidade}," +
             $"\"origem\": {origem}," +
             $"\"tipo\": {tipo}," +
             $"\"procedencia\": {procedencia}," +
             $"\"mascaraReferencia\": {mascara}," +

             "\"dadosSaida\": {" +
               $"\"mascara\": \"{mascara}\"," +
               $"\"situacao\": {situacao}," +
               $"\"descricao\": \"{nome}\"," +
               $"\"perComissao\": {perComissao}," +
               $"\"perIPI\": {perIPI}," +
               $"\"pesoLiquido\": {pesoLiquido}," +
               $"\"pesoBruto\": {pesoBruto}," +
               $"\"situacaoICMS\": {situacaoICMS}," +
               $"\"codNatureza\": {codNatureza}" +
             "}," +

             "\"dadosEntrada\": {" +
               $"\"mascara\": \"{mascara}\"," +
               $"\"situacao\": {situacao}," +
               $"\"codNatureza\": {codNatureza}," +
               $"\"descricao\": \"{nome}\"," +
               $"\"codigoICMS\": {codigoICMS}," +
               $"\"codigoIPI\": {codigoIPI}," +
               $"\"codContaContabil\": \"{codContaContabil}\"," +
               $"\"tipoControleSaida\": {tipoControleSaida}," +
               $"\"codUnidMedida\": {unidadeMedida}" +
             "}" +

           "}";

        request.AddJsonBody(bodyObject);

        response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          jsonObject = JObject.Parse(responseData);
          configuracao_api.SalvarAsync(configApi).Wait();
          return codigo;
        } else {
          var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
          var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro ao Cadastrar Produto";
          throw new Exception($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
       Toast.Error($"Erro ao Cadastrar Produto: {ex.Message}");
        return string.Empty;
      }

    }

    internal static async Task<bool> ExcludeItemGenericoAsync(long codReduzido) {
      try {
        JObject jsonObject = new JObject();

        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/{codReduzido}");
        var request = Api.CreateRequest(Method.DELETE);
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          jsonObject = JObject.Parse(responseData);
          return true;
        } else {
          var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
          var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro ao Cadastrar Produto";
          throw new Exception($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        // Toast.Error($"Erro ao Cadastrar Produto: {ex.Message}");
        return false;
      }

    }
  }
}
