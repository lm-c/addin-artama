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
          var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
          var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro ao Alterar Item";
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

      if (string.IsNullOrEmpty(codigo))
        return _return;

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
              var codigo = item["codigo"]?.ToString();
              var nome = item["nome"]?.ToString();
              var refTecnica = item["refTecnica"]?.ToString();
              var unidadeMedida = item["unidadeMedida"]?.ToString();
              var classificacaoFiscal = item["classificacaoFiscal"]?.ToString();
              var finalidade = item["finalidade"]?.ToObject<int>() ?? 0;
              var origem = item["origem"]?.ToString();
              var tipo = item["tipo"]?.ToObject<int>() ?? 0;
              var procedencia = item["procedencia"]?.ToString();

              var pesoBruto = item["dadosSaida"]["pesoBruto"]?.ToObject<double>() ?? 0;
              var pesoLiquido = item["dadosSaida"]["pesoLiquido"]?.ToObject<double>() ?? 0;
              var mascaraSaida = item["dadosSaida"]["mascara"]?.ToString();

              var mascaraEntrada = item["dadosEntrada"]["mascara"]?.ToString();
              var pesoPadraoNBR = item["dadosEntrada"]["pesoPadraoNBR"]?.ToObject<double>() ?? 0;
              var situacao = item["dadosEntrada"]["situacao"]?.ToString() ?? "";

              var itemGenerico = new ItemGenerico {
                codigo = codigo,
                nome = nome,
                refTecnica = refTecnica,
                unidadeMedida = unidadeMedida,
                classificacaoFiscal = classificacaoFiscal,
                finalidade = finalidade,
                origem = !string.IsNullOrEmpty( origem) ? int.Parse(origem) : 0,
                tipo = tipo,
                procedencia = !string.IsNullOrEmpty(procedencia) ? int.Parse(procedencia) : 0,

                pesoBruto = pesoBruto,
                pesoLiquido = pesoLiquido,
                mascaraSaida = mascaraSaida,

                mascaraEntrada = mascaraEntrada,
                pesoPadraoNBR = pesoPadraoNBR,
                situacao = !string.IsNullOrEmpty(situacao) ? int.Parse(situacao) : 1,
                              };
              _return.Add(itemGenerico);
            }
          }
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

      if(produtoErp.NaoAlterarNomeERP) {
        nomeParaErp = denominacao.Length > maxComprimento ? denominacao.Substring(0, maxComprimento) : denominacao;
        nomeParaErp = !string.IsNullOrEmpty(itemGenerico.nome) ? itemGenerico.nome : nomeParaErp;
      }

      itemGenerico.refTecnica = produtoErp.Name;
      itemGenerico.nome = nomeParaErp?.Replace("\"", "\\\"") ?? string.Empty;
      itemGenerico.pesoBruto = produtoErp.PesoBruto;
      itemGenerico.pesoLiquido = produtoErp.PesoLiquido;
      itemGenerico.tipoDocumento = produtoErp.TipoComponente == TipoComponente.Montagem ? TipoDocumento.Montagem : TipoDocumento.Peca;
      itemGenerico.unidadeMedida = produtoErp.UnidadeMedida;
    }

  }
}
