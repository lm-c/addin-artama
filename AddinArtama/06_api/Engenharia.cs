using LmCorbieUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static AddinArtama.Api;

namespace AddinArtama {
  internal partial class Api {
    internal class Engenharia {
      public int codEmpresa { get; set; }
      public string codProduto { get; set; }
      public string descricaoProduto { get; set; } = "";
      public string tipoModulo { get; set; } = "E";
      public string tipoEngenharia { get; set; }
      public int codClassificacao { get; set; }
      public string nomeArquivoDesenhoEng { get; set; }
      public bool engenhariaFantasma { get; set; }
      public string descEngenhariaFantasma { get; set; }
      public StatusEngenharia statusEngenharia { get; set; }
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
      public string centroCusto { get; set; }
    }

    internal class OperacaoEng {
      public int seqOperacao { get; set; }
      public int codOperacao { get; set; }
      public string abreviaturaOperacao { get; set; }
      public double numOperadores { get; set; }
      public int codFaseOperacao { get; set; }
      public string codMascaraMaquina { get; set; }
      public int codLinhaProducao { get; set; }
      public double tempoPadraoOperacao { get; set; }
      public double tempoPreparacaoOperacao { get; set; }
      public string centroCusto { get; set; }
      public TipoOperacao tipoOperacao { get; set; }
    }

    internal static async Task<bool> CadastrarEngenhariaAsync(ContextoDados db, Engenharia engenharia) {
      try {
        JObject jsonObject = new JObject();

        var client = Api.GetClient(modulo: "ppcppadrao", endpoint: $"pendenciaEngenharia");
        var request = Api.CreateRequest(Method.PUT);
        var centroCusto = engenharia.operacoes.Count > 0 ? engenharia.operacoes[0].centroCusto : string.Empty;        
        
        var bodyObject = "" +

          "{" +
             $"\"codEmpresa\": {engenharia.codEmpresa}," +
             $"\"tipoModulo\": \"{engenharia.tipoModulo}\"," +
             $"\"codClassificacao\": {engenharia.codClassificacao}," +
             $"\"nomeArquivoDesenhoEng\": \"{engenharia.nomeArquivoDesenhoEng}\"," +
             $"\"codProduto\": \"{engenharia.codProduto}\"," +
             $"\"engenhariaFantasma\": {engenharia.engenhariaFantasma.ToString().ToLower()}," +
             $"\"descEngenhariaFantasma\": \"{engenharia.descEngenhariaFantasma}\"," +

             "\"componentes\": [";

        foreach (var componente in engenharia.componentes) {
          bodyObject += "{" +
              $"\"seqComponente\": {componente.seqComponente}," +
              $"\"codInsumo\": \"{componente.codInsumo}\"," +
              $"\"quantidade\": {componente.quantidade.ToString().Replace(",", ".")}," +
              $"\"comprimento\": {componente.comprimento.ToString().Replace(",", ".")}," +
              $"\"largura\": {componente.largura.ToString().Replace(",", ".")}," +
              $"\"espessura\": {componente.espessura.ToString().Replace(",", ".")}," +
              $"\"percQuebra\": {componente.percQuebra.ToString().Replace(",", ".")}," +
              $"\"codClassificacaoInsumo\": {componente.codClassificacaoInsumo}," +
              $"\"itemKanban\": {componente.itemKanban}" +
              "},";
        }

        bodyObject = bodyObject.TrimEnd(',') + "]," +

        "\"operacoes\": [";

        foreach (var operacao in engenharia.operacoes) {
          bodyObject += "{" +
              $"\"seqOperacao\": {operacao.seqOperacao}," +
              $"\"codOperacao\": {operacao.codOperacao}," +
              $"\"numOperadores\": {operacao.numOperadores}," +
              $"\"codFaseOperacao\": {operacao.codFaseOperacao}," +

              (operacao.tipoOperacao == TipoOperacao.Externa
              ? $"\"maquina\": \"{operacao.codMascaraMaquina}\","                 // utiliza tela de configuação de processo/máquina (Customizada)
              : $"\"codMascaraMaquina\": \"{operacao.codMascaraMaquina}\",") +

              $"\"tempoPadraoOperacao\": {operacao.tempoPadraoOperacao.ToString().Replace(",", ".")}," +
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

          return true;
        } else {
          var errorMessage = ApiError.Parse(response.Content);
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

          var codClassificacao = jsonObject["codClassificacao"]?.ToString();
          var statusEngenharia = jsonObject["statusEngenharia"]?.ToString();

          _return = new Engenharia {
            descricaoProduto = SafeToString(jsonObject["descricao"]),
            codClassificacao = SafeToInt(jsonObject["codClassificacao"]),
            statusEngenharia = (StatusEngenharia)(SafeToInt(jsonObject["statusEngenharia"]) == 0
                            ? (int)StatusEngenharia.EmDesenvolvimento
                            : SafeToInt(jsonObject["statusEngenharia"])),
            tipoEngenharia = SafeToString(jsonObject["tipoEngenharia"]),

            componentes = jsonObject["componentes"]?.Select(c => new ComponenteEng {
              seqComponente = SafeToInt(c["seqComponente"]),
              codInsumo = SafeToString(c["codItem"]),
              quantidade = SafeToDouble(c["quantidade"]),
              centroCusto = SafeToString(c["centroCusto"]),
              comprimento = SafeToDouble(c["comprimento"]),
              largura = SafeToDouble(c["largura"]),
              espessura = SafeToDouble(c["espessura"]),
              percQuebra = SafeToDouble(c["percQuebra"]),
              codClassificacaoInsumo = SafeToInt(c["codClassificacao"])
            }).ToList() ?? new List<ComponenteEng>(),

            operacoes = jsonObject["operacoes"]?.Select(o => new OperacaoEng {
              seqOperacao = SafeToInt(o["seqOperacao"]),
              codOperacao = SafeToInt(o["codOperacao"]),
              abreviaturaOperacao = SafeToString(o["abreviaturaOperacao"]),
              numOperadores = SafeToDouble(o["numOpradores"]),
              codFaseOperacao = SafeToInt(o["faseProducao"]),
              codMascaraMaquina = SafeToString(o["mascMaquina"]),
              tempoPadraoOperacao = SafeToDouble(o["tempoPadrao"]),
              tempoPreparacaoOperacao = SafeToDouble(o["tempoPreparacao"]),
              centroCusto = SafeToString(o["centroCusto"])
            }).ToList() ?? new List<OperacaoEng>()
          };
        } else {
          var errorMessage = ApiError.Parse(response.Content);
          if(errorMessage == $"Engenharia {codigo} não cadastrada!") {
            return null;
          }
          throw new Exception($"{response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, $"Erro ao retornar engenharia: {codigo}");
      }

      return _return;
    }

    internal static async Task<string> DuplicarItemGenericoAsync(ContextoDados db, ItemGenerico itemGenerico) {
      var configApi = configuracao_api.Selecionar();

      try {

        var mascara = itemGenerico.tipoDocumento == TipoDocumento.Peca ? configApi.mascara_peca : configApi.mascara_conjunto;

        /* inicio variaveis */
        var codigo = string.Empty;
        var mascara_ent = mascara;
        var mascara_sai = mascara;
        var descricao = itemGenerico.nome ?? string.Empty;
        var pesoBruto = itemGenerico.pesoBruto;
        var pesoLiquido = itemGenerico.pesoLiquido;
        var unidadeMedida = itemGenerico.unidadeMedida;
        /* fim variaveis */

        JObject jsonObject = new JObject();

        var client = Api.GetClient(modulo: "ppcppadrao", endpoint: $"duplicarItemImportacao");
        var request = Api.CreateRequest(Method.PUT);
        var response = await client.ExecuteAsync(request);

        var bodyObject = "" +
          "{" +
             $"\"tipoModulo\": \"E\"," +
             $"\"codigoItem\": null," +
             $"\"descricaoItem\": \"{descricao}\"," +
             $"\"descricaoCompleta\": \"{descricao}\"," +
             $"\"nivelMascaraEntrada\": \"{mascara_ent}\"," +
             $"\"nivelMascaraSaida\": \"{mascara_sai}\"," +
             $"\"pesoLiquido\": {pesoLiquido.ToString().Replace(",", ".")}," +
             $"\"pesoBruto\": {pesoBruto.ToString().Replace(",", ".")}," +
             $"\"unidadeMedida\": \"{unidadeMedida}\"" +
             
           "}";

        request.AddJsonBody(bodyObject);

        response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          jsonObject = JObject.Parse(responseData);
          codigo = (string)jsonObject["codigo"];

          if (codigo.EndsWith("9999")) {
            var splitMascEnt = mascara_ent.Split('.');
            splitMascEnt[splitMascEnt.Length - 1] = (Convert.ToInt32(splitMascEnt[splitMascEnt.Length - 1]) + 1).ToString("00");

            if (itemGenerico.tipoDocumento == TipoDocumento.Peca) {
              configApi.mascara_peca = string.Join(".", splitMascEnt);
            } else {
              configApi.mascara_conjunto = string.Join(".", splitMascEnt);
            }

            db.SaveChanges();
          }

          return codigo;
        } else {
          var errorMessage = ApiError.Parse(response.Content);
          throw new Exception($"Erro ao Duplicar Item: {itemGenerico.refTecnica}\n\nErro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        Toast.Error($"{ex.Message}");
        return string.Empty;
      }

    }

    #region Private metodos

    private static int SafeToInt(JToken token) {
      if (token == null) return 0;
      if (int.TryParse(token.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
        return value;
      return 0;
    }

    private static double SafeToDouble(JToken token) {
      if (token == null) return 0;
      if (double.TryParse(token.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
        return value;
      return 0;
    }

    private static string SafeToString(JToken token) {
      return token?.ToString() ?? string.Empty;
    }

    #endregion

  }
}
