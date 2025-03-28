using LmCorbieUI;
using System;
using System.Collections.Generic;
using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace AddinArtama {
  internal partial class Api {
    internal class MascaraEntrada {
      public int sequencialNivel { get; set; }
      public string estruturaNivel { get; set; }
      public string descricaoNivel { get; set; }
      public int? tamanho { get; set; } // Permite valores nulos
      public List<DadosCustomizados> dadosCustomizados { get; set; }
    }

    private class ResponseMascara {
      public string continuationToken { get; set; }
      public List<MascaraEntrada> data { get; set; }
    }

    internal static async Task<List<MascaraEntrada>> GetMascarasAsync() {
      var _return = new List<MascaraEntrada>();

      try {
        var continuationToken = "";
        do {
          var client = Api.GetClient(modulo: "itens", endpoint: $"nivelMascara?{continuationToken}tipoMascara=E");
          var request = Api.CreateRequest(Method.GET);
          request.AddHeader("empresa", "1");
          var response = await client.ExecuteAsync(request);


          if (response.IsSuccessful) {
            ResponseMascara responseObj = JsonConvert.DeserializeObject<ResponseMascara>(response.Content);

            var token = responseObj.continuationToken;
            if (!string.IsNullOrEmpty(token))
              continuationToken = $"continuationToken={token}";

            _return.AddRange(responseObj.data);

          } else {
            var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
            var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro desconhecido";
            Toast.Error($"Erro: {response.StatusCode}\r\n{errorMessage}");
          }
        } while (!string.IsNullOrEmpty(continuationToken));


      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar operações");
      }

      return _return;
    }

  }
}
