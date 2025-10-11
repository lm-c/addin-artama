﻿using LmCorbieUI;
using LmCorbieUI.Metodos.AtributosCustomizados;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AddinArtama {
  internal partial class Api {
    internal class Maquina {
      public string centroCusto { get; set; }
      public string codAtivo { get; set; }

      [DisplayName("Código Máquina")]
      [LarguraColunaGrid(150)]
      [DataObjectField(true, false)]
      public int codMaquina { get; set; }

      public string descReduzida { get; set; }

      [DisplayName("Descrição Máquina")]
      [LarguraColunaGrid(150)]
      [DataObjectField(false, true)]
      public string descricao { get; set; }
      public string mascara { get; set; }
    }

    private class ResponseMaq {
      public string continuationToken { get; set; }
      public List<Maquina> data { get; set; }
    }

    internal static async Task<List<Maquina>> GetMaquinasAsync() {
      var _return = new List<Maquina>();

      try {
        var continuationToken = "";
        do {
          var client = Api.GetClient(modulo: "ppcmmanutencao", endpoint: $"maquina?situacao=1&{continuationToken}");
          var request = Api.CreateRequest(Method.GET);
          var response = await client.ExecuteAsync(request);


          if (response.IsSuccessful) {
            ResponseMaq responseObj = JsonConvert.DeserializeObject<ResponseMaq>(response.Content);

            var token = responseObj.continuationToken;
            if (!string.IsNullOrEmpty(token))
              continuationToken = $"continuationToken={token}";
            else
              continuationToken = "";

            _return.AddRange(responseObj.data);

          } else {
            var errorMessage = ApiError.Parse(response.Content);
            Toast.Error($"Erro: {response.StatusCode}\r\n{errorMessage}");
            break;
          }
        } while (!string.IsNullOrEmpty(continuationToken));
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar operações");
      }

      return _return;
    }
  }
}
