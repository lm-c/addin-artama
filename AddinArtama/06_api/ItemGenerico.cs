using LmCorbieUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AddinArtama.Api;

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

    internal static async Task UpdateItemGenericoAsync(ProdutoErp produtoErp) {
      ItemGenerico itemGenerico = new ItemGenerico();
      try {
        itemGenerico = await Api.GetItemGenericoAsync(produtoErp.CodProduto);

        Api.MontarItemGenerico(produtoErp, itemGenerico);

        if (produtoErp.Fantasma) {
          itemGenerico.situacao = 0;
        }

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
          var errorMessage = ApiError.Parse(response.Content);
          throw new Exception($"Item: {produtoErp.Name}\n\nErro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        Toast.Error($"{ex.Message}");
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
          var errorMessage = ApiError.Parse(response.Content);
          throw new Exception($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        // Toast.Error($"Erro ao Cadastrar Produto: {ex.Message}");
        return false;
      }

    }

    internal static async Task<ItemGenerico> GetItemGenericoAsync(string codigo) {
      ItemGenerico _return = null;

      if (string.IsNullOrEmpty(codigo))
        return _return;

      try {
        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/{codigo}");
        var request = Api.CreateRequest(Method.GET);

        var response = await client.ExecuteAsync(request);


        if (response.IsSuccessful) {
          var responseData = response.Content;
          var jsonObject = JObject.Parse(responseData);

          // Acessar o array "data"
          var dataArray = jsonObject["data"] as JArray;

          var mascaraSaida = jsonObject["dadosSaida"]?["mascara"]?.ToString();
          var mascaraEntrada = jsonObject["dadosEntrada"]?["mascara"]?.ToString();

          _return = new ItemGenerico {
            codigo = jsonObject["codigo"]?.ToString(),
            nome = jsonObject["nome"]?.ToString(),
            refTecnica = jsonObject["refTecnica"]?.ToString(),
            unidadeMedida = jsonObject["unidadeMedida"]?.ToString(),
            classificacaoFiscal = jsonObject["classificacaoFiscal"]?.ToString(),

            finalidade = int.TryParse(jsonObject["finalidade"]?.ToString(), out var f) ? f : 0,
            origem = int.TryParse(jsonObject["origem"]?.ToString(), out var o) ? o : 0,
            tipo = int.TryParse(jsonObject["tipo"]?.ToString(), out var t) ? t : 0,
            procedencia = int.TryParse(jsonObject["procedencia"]?.ToString(), out var p) ? p : 0,

            pesoBruto = double.TryParse(jsonObject["dadosSaida"]?["pesoBruto"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var pb) ? pb : 0,
            pesoLiquido = double.TryParse(jsonObject["dadosSaida"]?["pesoLiquido"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var pl) ? pl : 0,
            mascaraSaida = mascaraSaida,

            mascaraEntrada = mascaraEntrada,
            pesoPadraoNBR = double.TryParse(jsonObject["dadosEntrada"]?["pesoPadraoNBR"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var ppn) ? ppn : 0,

            situacao = int.TryParse(jsonObject["dadosEntrada"]?["situacao"]?.ToString(), out var sit) ? sit : 1,
          };
        } else {
          var errorMessage = ApiError.Parse(response.Content);
          throw new Exception($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, $"Item código: {codigo} | Erro ao pesquisar item genérico");
      }

      return _return;
    }

    internal static async Task<List<ItemGenerico>> GetItemGenericoByNameAsync(string nomeItem) {
      List<ItemGenerico> _return = new List<ItemGenerico>();

      if (string.IsNullOrEmpty(nomeItem))
        return _return;

      try {
        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico?descricao={nomeItem}");
        var request = Api.CreateRequest(Method.GET);

        var response = await client.ExecuteAsync(request);


        if (response.IsSuccessful) {
          var responseData = response.Content;
          var jsonObject = JObject.Parse(responseData);

          // Acessar o array "data"
          var dataArray = jsonObject["data"] as JArray;
          if (dataArray != null) {
            foreach (var item in dataArray) {

              // Strings simples
              var codigo = item["codigo"]?.ToString();
              var nome = item["nome"]?.ToString();
              var refTecnica = item["refTecnica"]?.ToString();
              var unidadeMedida = item["unidadeMedida"]?.ToString();
              var classificacaoFiscal = item["classificacaoFiscal"]?.ToString();

              // Inteiros com TryParse
              var finalidade = int.TryParse(item["finalidade"]?.ToString(), out var f) ? f : 0;
              var origem = int.TryParse(item["origem"]?.ToString(), out var o) ? o : 0;
              var tipo = int.TryParse(item["tipo"]?.ToString(), out var t) ? t : 0;
              var procedencia = int.TryParse(item["procedencia"]?.ToString(), out var p) ? p : 0;

              // Dados de saída
              var pesoBruto = double.TryParse(item["dadosSaida"]?["pesoBruto"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var pb) ? pb : 0;
              var pesoLiquido = double.TryParse(item["dadosSaida"]?["pesoLiquido"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var pl) ? pl : 0;
              var mascaraSaida = item["dadosSaida"]?["mascara"]?.ToString();

              // Dados de entrada
              var mascaraEntrada = item["dadosEntrada"]?["mascara"]?.ToString();
              var pesoPadraoNBR = double.TryParse(item["dadosEntrada"]?["pesoPadraoNBR"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var ppn) ? ppn : 0;

              // Situação (se vier vazio → 1 como padrão)
              var situacao = int.TryParse(item["dadosEntrada"]?["situacao"]?.ToString(), out var sit) ? sit : 1;

              // Monta o objeto final
              _return.Add(new ItemGenerico {
                codigo = codigo,
                nome = nome,
                refTecnica = refTecnica,
                unidadeMedida = unidadeMedida,
                classificacaoFiscal = classificacaoFiscal,
                finalidade = finalidade,
                origem = origem,
                tipo = tipo,
                procedencia = procedencia,

                pesoBruto = pesoBruto,
                pesoLiquido = pesoLiquido,
                mascaraSaida = mascaraSaida,

                mascaraEntrada = mascaraEntrada,
                pesoPadraoNBR = pesoPadraoNBR,
                situacao = situacao,
              });
            }

          }
        } else {
          var errorMessage = ApiError.Parse(response.Content);
          throw new Exception($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, $"Item nome: {nomeItem} | Erro ao pesquisar item genérico");
      }

      return _return;
    }

    internal static void MontarItemGenerico(ProdutoErp produtoErp, ItemGenerico itemGenerico) {
      var codigo = produtoErp.CodComponente.StartsWith("10") || produtoErp.CodComponente.StartsWith("20") || produtoErp.CodComponente.StartsWith("40") ? produtoErp.CodComponente : produtoErp.Name;
      var denominacao = produtoErp.Denominacao;

      var separador = " - ";
      var maxComprimento = 50;

      int espacoParaNome = maxComprimento - (codigo.Length + separador.Length);

      // Corta o nome se necessário
      var nomeAjustado = denominacao.Length > espacoParaNome
          ? denominacao.Substring(0, espacoParaNome)
          : denominacao;

      string nomeParaErp = nomeAjustado + separador + codigo;

      if (produtoErp.NaoAlterarNomeERP) {
        nomeParaErp = denominacao.Length > maxComprimento ? denominacao.Substring(0, maxComprimento) : denominacao;
        nomeParaErp = !string.IsNullOrEmpty(itemGenerico.nome) ? itemGenerico.nome : nomeParaErp;
      }

      itemGenerico.refTecnica = produtoErp.Name;
      itemGenerico.nome = nomeParaErp?.Replace("\"", "\\\"") ?? string.Empty;
      itemGenerico.pesoBruto = produtoErp.PesoBruto;
      itemGenerico.pesoLiquido = produtoErp.PesoLiquido;
      itemGenerico.tipoDocumento = produtoErp.TipoComponente == TipoComponente.Montagem ? TipoDocumento.Montagem : TipoDocumento.Peca;
      itemGenerico.unidadeMedida = produtoErp.UnidadeMedida;

      produtoErp.ItemGenerico = itemGenerico;
    }

  }
}
