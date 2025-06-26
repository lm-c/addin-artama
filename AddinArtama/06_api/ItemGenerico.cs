﻿using System;
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
      public string codigo { get; set; }
      public string nome { get; set; }
      public string refTecnica { get; set; }
      public string mascaraEntrada { get; set; }
      public string mascaraSaida { get; set; }
      public string classificacaoFiscal { get; set; } 
      public string unidadeMedida { get; set; }
      public int finalidade { get; set; }
      public int origem { get; set; }
      public int tipo { get; set; }
      public int procedencia { get; set; }
      public double pesoBruto { get; set; }
      public double pesoLiquido { get; set; }
      public double pesoPadraoNBR { get; set; }
      public int situacao { get; set; }
      public TipoDocumento tipoDocumento { get; set; }
    }

    internal static async Task UpdateItemGenericoAsync(ContextoDados db, ProdutoErp produtoErp) {
      ItemGenerico itemGenerico = new ItemGenerico();
      try {
        itemGenerico = await Api.GetItemGenericoAsync(produtoErp.CodProduto);

        Api.MontarItemGenerico(produtoErp, itemGenerico);
      
        JObject jsonObject = new JObject();

        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/{itemGenerico.codigo}");
        var request = Api.CreateRequest(Method.PUT);
        var response = await client.ExecuteAsync(request);

        var bodyObject = "" +
            "{" +
                $"\"nome\": \"{itemGenerico.nome}\"," +
                $"\"unidadeMedida\": \"{itemGenerico.unidadeMedida}\"," +
                $"\"classificacaoFiscal\": \"{itemGenerico.classificacaoFiscal}\"," +
                $"\"finalidade\": {itemGenerico.finalidade}," +
                $"\"origem\": {itemGenerico.origem}," +
                $"\"tipo\": {itemGenerico.tipo}," +
                $"\"procedencia\": {itemGenerico.procedencia}," +

              "\"dadosSaida\": {" +
                    $"\"mascara\": \"{itemGenerico.mascaraSaida}\"," +
                    $"\"descricao\": \"{itemGenerico.nome}\"," +
                    $"\"pesoLiquido\": {itemGenerico.pesoLiquido.ToString().Replace(",", ".")}," +
                    $"\"pesoBruto\": {itemGenerico.pesoBruto.ToString().Replace(",", ".")}," +
                    $"\"situacao\": {itemGenerico.situacao}" +
                "}," +
              "\"dadosEntrada\": {" +
                    $"\"mascara\": \"{itemGenerico.mascaraEntrada}\"," +
                    $"\"situacao\": {itemGenerico.situacao}," +
                    $"\"descricao\": \"{itemGenerico.nome}\"" +
                    //$"\"justificaiva\": \"string\"," +
                    //$"\"dataDesativacao\": \"string\"," +
                    //$"\"dataReativacao\": \"string\"," +
              "}" +
            "}";

        request.AddJsonBody(bodyObject);

        response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          jsonObject = JObject.Parse(responseData);
        } else {
          var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
          var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro ao Alterar Item";
          throw new Exception($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        Toast.Error($"Erro ao Alterar Item {produtoErp.Name}: {ex.Message}");
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

    internal static async Task<ItemGenerico> GetItemGenericoAsync(string codigo) {
      ItemGenerico _return = null;

      try {
        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/{codigo}");
        var request = Api.CreateRequest(Method.GET);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          var jsonObject = JObject.Parse(responseData);

          _return = new ItemGenerico {
            codigo = jsonObject["codigo"]?.ToString(),
            nome = jsonObject["nome"]?.ToString(),
            refTecnica = jsonObject["refTecnica"]?.ToString(),
            unidadeMedida = jsonObject["unidadeMedida"]?.ToString(),
            classificacaoFiscal = jsonObject["classificacaoFiscal"]?.ToString(),
            finalidade = jsonObject["finalidade"]?.ToObject<int>() ?? 0,
            origem = jsonObject["origem"]?.ToObject<int>() ?? 0,
            tipo = jsonObject["tipo"]?.ToObject<int>() ?? 0,
            procedencia = jsonObject["procedencia"]?.ToObject<int>() ?? 0,
            
            pesoBruto = jsonObject["dadosSaida"]["pesoBruto"]?.ToObject<double>() ?? 0,
            pesoLiquido = jsonObject["dadosSaida"]["pesoLiquido"]?.ToObject<double>() ?? 0,
            mascaraSaida = jsonObject["dadosSaida"]["mascara"]?.ToString(),

            mascaraEntrada = jsonObject["dadosEntrada"]["mascara"]?.ToString(),
            pesoPadraoNBR = jsonObject["dadosEntrada"]["pesoPadraoNBR"]?.ToObject<double>() ?? 0,
            situacao = jsonObject["dadosEntrada"]["situacao"]?.ToObject<int>() ?? 0,
          };
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar item genérico");
      }

      return _return;
    }

    internal static void MontarItemGenerico(ProdutoErp produtoErp, ItemGenerico itemGenerico) {
      var codigo = produtoErp.CodComponente.StartsWith("10") || produtoErp.CodComponente.StartsWith("20") || produtoErp.CodComponente.StartsWith("40") ? produtoErp.CodComponente  : produtoErp.Name;
      var denominacao  = produtoErp.Denominacao;

      var separador = " - ";
      var maxComprimento = 50;

      int espacoParaNome = maxComprimento - (codigo.Length + separador.Length);

      // Corta o nome se necessário
      var nomeAjustado = denominacao.Length > espacoParaNome
          ? denominacao.Substring(0, espacoParaNome)
          : denominacao;

      string nomeParaErp = nomeAjustado + separador + codigo;

      itemGenerico.refTecnica = produtoErp.Name;
      itemGenerico.nome = nomeParaErp?.Replace("\"", "\\\"") ?? string.Empty;
      itemGenerico.pesoBruto = produtoErp.PesoBruto;
      itemGenerico.pesoLiquido = produtoErp.PesoLiquido;
      itemGenerico.tipoDocumento = produtoErp.TipoComponente == TipoComponente.Montagem || produtoErp.ItensCorte.Count > 1 ? TipoDocumento.Montagem : TipoDocumento.Peca;
      itemGenerico.unidadeMedida = produtoErp.UnidadeMedida;
    }

  }
}
