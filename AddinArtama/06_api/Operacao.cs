using LmCorbieUI;
using System;
using System.Collections.Generic;
using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace AddinArtama {
  internal partial class Api {
    internal class Operacao {
      public string Abreviatura { get; set; }
      public int CodOperacao { get; set; }
      public string Descricao { get; set; }
      public int? FaseProducao { get; set; } // Permite valores nulos
      public string Situacao { get; set; }
      public string Tipo { get; set; }
    }

    private class Response {
      public string continuationToken { get; set; }
      public List<Operacao> data { get; set; }
    }

    internal static async Task<List<Operacao>> GetOpsAsync() {
      var _return = new List<Operacao>();

      try {
        var client = Api.GetClient(modulo: "ppcppadrao", endpoint: "operacao?situacao=1&paginacao=1000");
        var request = Api.CreateRequest(Method.GET);
        request.AddHeader("empresa", "1");
        var response = await client.ExecuteAsync(request);


        if (response.IsSuccessful) {
          Response responseObj = JsonConvert.DeserializeObject<Response>(response.Content);

          _return = responseObj.data;

        } else {
          var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
          var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro desconhecido";
          Toast.Error($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }

      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar operações");
      }

      return _return;
    }
  }
}
