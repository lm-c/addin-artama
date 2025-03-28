using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LmCorbieUI;
using RestSharp;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using LmCorbieUI.Metodos;

namespace AddinArtama {
  internal partial class Api {
    internal class ItemGenerico {
      public string Codigo { get; set; }
      public string Nome { get; set; }
      public string Descricao { get; set; }
      public string UnidadeMedida { get; set; }
      public string ClassificacaoFiscal { get; set; }
      //public string CodItemPai { get; set; }
      //public int? Tipo { get; set; }
      //public int? Finalidade { get; set; }
      public DadosEntrada DadosEntrada { get; set; }
      public DadosSaida DadosSaida { get; set; }
      //public DadosTecnicos DadosTecnicos { get; set; }
      //public List<SaldoEstoque> SaldoEstoque { get; set; }
    }

    internal class DadosEntrada {
      public string Mascara { get; set; }
      public string Descricao { get; set; }
      public int TipoControleSaida { get; set; }
      //public string UnidadeConsumo { get; set; }
      //public string Altura { get; set; }
      //public string Largura { get; set; }
      //public string Comprimento { get; set; }
      //public string CodigoFabricante { get; set; }
    }

    internal class DadosSaida {
      public string Mascara { get; set; }
      public string Descricao { get; set; }
      //public string Marca { get; set; }
      //public string PesoBruto { get; set; }
      //public string PesoLiquido { get; set; }
    }

    internal class DadosTecnicos {
      public string Composicao { get; set; }
      public string Gramatura { get; set; }
      public string Largura { get; set; }
      public string Rendimento { get; set; }
    }

    internal class SaldoEstoque {
      public string CodNatureza { get; set; }
      public string QtdSaldo { get; set; }
    }

    internal class DadosCustomizados {
      public string Campo { get; set; }
      public string Valor { get; set; }
    }

    internal static async Task<bool> CadasterItemGenericoAsync(item_generico_duplicacao itemGenerico) {
      JObject jsonObject = new JObject();

      try {
        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/{itemGenerico.codigo}");
        var request = Api.CreateRequest(Method.GET);
        request.AddHeader("empresa", "1");
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          jsonObject = JObject.Parse(responseData);

        }
      } catch (Exception ex) {
        throw new Exception($"Erro ao Buscar Item: {ex.Message}");
      }

      try {
        /* inicio variaveis */
        var codigo = string.Empty;// jsonObject["codigo"]?.ToString();
        var nome = itemGenerico.descricaoItem;
        var unidadeMedida = jsonObject["unidadeMedida"]?.ToString();
        var classificacaoFiscal = jsonObject["classificacaoFiscal"]?.ToString();
        var finalidade = jsonObject["finalidade"].ToObject<int>();
        var origem = jsonObject["origem"].ToObject<int>();
        var tipo = jsonObject["tipo"].ToObject<int>();
        var procedencia = jsonObject["procedencia"].ToObject<int>();
        var codigoBarras = jsonObject["codigoBarras"]?.ToString();
        var codigoBarraDun14 = jsonObject["codigoBarraDun14"]?.ToString();
        var mascaraReferencia = jsonObject["mascaraReferencia"]?.ToString();
        var controlaLote = jsonObject["controlaLote"].ToObject<int>();
        var controlaDataValidadeLote = jsonObject["controlaDataValidadeLote"].ToObject<int>();
        var controlaConcentracaoLote = jsonObject["controlaConcentracaoLote"].ToObject<int>();
        var selecaoLoteSemManutencao = jsonObject["selecaoLoteSemManutencao"].ToObject<int>();
        var contQuaFioLote = jsonObject["contQuaFioLote"].ToObject<int>();
        var codItemInter = jsonObject["codItemInter"].ToString();
        var refTecnica = jsonObject["refTecnica"]?.ToString();
        var compQuimLotes = jsonObject["compQuimLotes"].ToString();
        var loteFornec = jsonObject["loteFornec"].ToString();
        var codigoQualidade = jsonObject["codigoQualidade"].ToString();
        var controlaTemperatura = jsonObject["controlaTemperatura"].ToString();
        var codigoItemAlfaNumerico = jsonObject["codigoItemAlfaNumerico"]?.ToString();
        var formatoInicialCodProduto = jsonObject["formatoInicialCodProduto"]?.ToString();
        var referenciaImportacao = jsonObject["referenciaImportacao"]?.ToString();
        var volumeMetroCubItem = jsonObject["volumeMetroCubItem"].ToObject<double>();
        var loteCalculoVolume = jsonObject["loteCalculoVolume"].ToObject<double>();
        var arredondaOcupacaoVolumeLote = jsonObject["arredondaOcupacaoVolumeLote"].ToString();
        var tipoMaterial = jsonObject["tipoMaterial"]?.ToString();
        var controlFCI = jsonObject["controlFCI"].ToString();
        var unidadesPorEtiqueta = jsonObject["unidadesPorEtiqueta"].ToObject<double>();
        var metodoControleEstoque = jsonObject["metodoControleEstoque"].ToString();
        var flagReplicado = true;

        // dadosSaida
        var mascara_sai = "10.10.10.10.10";// jsonObject["dadosSaida"]["mascara"]?.ToString();
        var situacao_sai = 1;
        var marca_sai = jsonObject["dadosSaida"]["marca"]?.ToString();
        var descricao_sai = itemGenerico.descricaoCompleta;
        var perComissao_sai = jsonObject["dadosSaida"]["perComissao"].ToObject<double>();
        var perIPI_sai = jsonObject["dadosSaida"]["perIPI"].ToObject<double>();
        var pesoLiquido_sai = jsonObject["dadosSaida"]["pesoLiquido"].ToObject<double>();
        var pesoBruto_sai = jsonObject["dadosSaida"]["pesoBruto"].ToObject<double>();
        var situacaoICMS_sai = jsonObject["dadosSaida"]["situacaoICMS"].ToObject<int>();
        var tipoFaturamento_sai = jsonObject["dadosSaida"]["tipoFaturamento"].ToString();
        var unidadeConversao_sai = jsonObject["dadosSaida"]["unidadeConversao"]?.ToString();
        var unidadeVenda_sai = jsonObject["dadosSaida"]["unidadeVenda"]?.ToString();
        var ribana_sai = jsonObject["dadosSaida"]["ribana"].ToString();
        var tipoGrupo_sai = jsonObject["dadosSaida"]["tipoGrupo"]?.ToString();
        var codNatureza_sai = jsonObject["dadosSaida"]["codNatureza"].ToObject<int>();
        var qtdeEmbalagemPadrao_sai = jsonObject["dadosSaida"]["qtdeEmbalagemPadrao"].ToObject<double>();
        var alternativo_sai = jsonObject["dadosSaida"]["alternativo"]?.ToString();
        var codMarkup_sai = jsonObject["dadosSaida"]["codMarkup"].ToString();
        var observacao_sai = jsonObject["dadosSaida"]["observacao"]?.ToString();
        var valorIPIUnidade_sai = jsonObject["dadosSaida"]["valorIPIUnidade"].ToObject<double>();
        var juntoONU_sai = jsonObject["dadosSaida"]["juntoONU"].ToString();
        var codClasse_sai = jsonObject["dadosSaida"]["codClasse"].ToString();
        var subClasse_sai = jsonObject["dadosSaida"]["subClasse"].ToString();
        var classeItem_sai = jsonObject["dadosSaida"]["classeItem"].ToString();
        var grupoItem_sai = jsonObject["dadosSaida"]["grupoItem"].ToString();
        var familiaItem_sai = jsonObject["dadosSaida"]["familiaItem"].ToString();
        var tara_sai = jsonObject["dadosSaida"]["tara"].ToObject<double>();
        var reducaoIPI_sai = jsonObject["dadosSaida"]["reducaoIPI"].ToObject<double>();
        var codMensagem_sai = jsonObject["dadosSaida"]["codMensagem"].ToString();
        var diasGarantia_sai = jsonObject["dadosSaida"]["diasGarantia"].ToString();
        var justificativa_sai = jsonObject["dadosSaida"]["justificativa"]?.ToString();
        var dataDesativacao_sai = jsonObject["dadosSaida"]["dataDesativacao"]?.ToString();
        var dataReativacao_sai = jsonObject["dadosSaida"]["dataReativacao"]?.ToString();
        var codIBC_sai = jsonObject["dadosSaida"]["codIBC"].ToString();
        var qtdVen_sai = jsonObject["dadosSaida"]["qtdVen"].ToObject<double>();
        var pesIte_sai = jsonObject["dadosSaida"]["pesIte"].ToObject<double>();
        var codCli_sai = jsonObject["dadosSaida"]["codCli"]?.ToString();
        var EAN13_sai = jsonObject["dadosSaida"]["EAN13"]?.ToString();
        var rua_sai = jsonObject["dadosSaida"]["rua"].ToString();
        var gaveta_sai = jsonObject["dadosSaida"]["gaveta"].ToString();
        var codNomeEmb_sai = jsonObject["dadosSaida"]["codNomeEmb"].ToString();
        var codLab_sai = jsonObject["dadosSaida"]["codLab"].ToString();
        var disponivel45_sai = jsonObject["dadosSaida"]["disponivel45"].ToString();
        var disponivel46_sai = jsonObject["dadosSaida"]["disponivel45"].ToString();
        var seqHistMasc_sai = jsonObject["dadosSaida"]["seqHistMasc"].ToString();
        var alturaDimenEmbalagem_sai = jsonObject["dadosSaida"]["alturaDimenEmbalagem"].ToObject<double>();
        var comprDimenEmbalagem_sai = jsonObject["dadosSaida"]["comprDimenEmbalagem"].ToObject<double>();
        var larguraDimenEmbalagem_sai = jsonObject["dadosSaida"]["larguraDimenEmbalagem"].ToObject<double>();
        var alturaDimenProduto_sai = jsonObject["dadosSaida"]["alturaDimenProduto"].ToObject<double>();
        var comprDimenProduto_sai = jsonObject["dadosSaida"]["comprDimenProduto"].ToObject<double>();
        var larguraDimenProduto_sai = jsonObject["dadosSaida"]["larguraDimenProduto"].ToObject<double>();
        var unidadeTributavel_sai = jsonObject["dadosSaida"]["unidadeTributavel"]?.ToString();
        var fatorConversaoUnidade_sai = jsonObject["dadosSaida"]["fatorConversaoUnidade"].ToObject<double>();
        var diametroEmbalagem_sai = jsonObject["dadosSaida"]["diametroEmbalagem"].ToObject<double>();
        var diametroProduto_sai = jsonObject["dadosSaida"]["diametroProduto"].ToObject<double>();

        // dadosEntrada
        var mascara_ent = "10 40 10 70 10";// jsonObject["dadosEntrada"]["mascara"]?.ToString();
        var situacao_ent = 1;
        var codNatureza_ent = jsonObject["dadosEntrada"]["codNatureza"].ToObject<int>();
        var descricao_ent = itemGenerico.descricaoCompleta;
        var justificativa_ent = jsonObject["dadosEntrada"]["justificativa"]?.ToString();
        var dataDesativacao_ent = jsonObject["dadosEntrada"]["dataDesativacao"]?.ToString();
        var dataReativacao_ent = jsonObject["dadosEntrada"]["dataReativacao"]?.ToString();
        var finalidadeMaterial_ent = jsonObject["dadosEntrada"]["finalidadeMaterial"].ToObject<int>();
        var codigoICMS_ent = jsonObject["dadosEntrada"]["codigoICMS"].ToObject<int>();
        var codigoIPI_ent = jsonObject["dadosEntrada"]["codigoIPI"].ToObject<int>();
        var codContaContabil_ent = jsonObject["dadosEntrada"]["codContaContabil"]?.ToString();
        var tipoControleSaida_ent = jsonObject["dadosEntrada"]["tipoControleSaida"].ToObject<int>();
        var fatorConversao_ent = jsonObject["dadosEntrada"]["fatorConversao"]?.ToString();
        var codUnidMedida_ent = jsonObject["dadosEntrada"]["codUnidMedida"]?.ToString();
        var materiaPrimaCSM_ent = jsonObject["dadosEntrada"]["materiaPrimaCSM"].ToString();
        var quantidadeMateriaPrima_ent = jsonObject["dadosEntrada"]["quantidadeMateriaPrima"].ToString();
        var percentualConcentracaoMM_ent = jsonObject["dadosEntrada"]["percentualConcentracaoMM"].ToString();
        var fichaInspecaoLI_ent = jsonObject["dadosEntrada"]["fichaInspecaoLI"]?.ToString();
        var localFisicoFI_ent = jsonObject["dadosEntrada"]["localFisicoFI"]?.ToString();
        var classeToxicologicaFM_ent = jsonObject["dadosEntrada"]["classeToxicologicaFM"]?.ToString();
        var regMinAgricultura_ent = jsonObject["dadosEntrada"]["regMinAgricultura"].ToString();
        var multiplicador_ent = jsonObject["dadosEntrada"]["multiplicador"].ToObject<double>();
        var codigoFabricante_ent = jsonObject["dadosEntrada"]["codigoFabricante"].ToString();
        var comprimento_ent = jsonObject["dadosEntrada"]["comprimento"].ToObject<double>();
        var largura_ent = jsonObject["dadosEntrada"]["largura"].ToObject<double>();
        var altura_ent = jsonObject["dadosEntrada"]["altura"].ToObject<double>();
        var pesoPadraoNBR_ent = jsonObject["dadosEntrada"]["pesoPadraoNBR"].ToObject<double>();
        var UnidadeConsumo_ent = "";
        var fatorConversaoUniCompra_ent = jsonObject["dadosEntrada"]["fatorConversaoUniCompra"]?.ToString();
        var inativoParaCompra_ent = jsonObject["dadosEntrada"]["inativoParaCompra"].ToString();
        var pesoPadraoNBRDesc_ent = jsonObject["dadosEntrada"]["pesoPadraoNBRDesc"]?.ToString();
        var codNaturezaConsignada_ent = jsonObject["dadosEntrada"]["codNaturezaConsignada"].ToString();
        var qtdeCompra_ent = jsonObject["dadosEntrada"]["qtdeCompra"].ToObject<double>();
        var equipProtecIndivid_ent = jsonObject["dadosEntrada"]["equipProtecIndivid"].ToString();
        var regMinisterioSaude_ent = jsonObject["dadosEntrada"]["regMinisterioSaude"]?.ToString();
        var dataValidade_ent = jsonObject["dadosEntrada"]["dataValidade"]?.ToString();
        var denomComumBrasil_ent = jsonObject["dadosEntrada"]["denomComumBrasil"]?.ToString();
        var medidaEPI_ent = jsonObject["dadosEntrada"]["medidaEPI"]?.ToString();
        var tipcom_ent = jsonObject["dadosEntrada"]["tipcom"].ToString();
        var seqHistMasc_ent = jsonObject["dadosEntrada"]["seqHistMasc"].ToString();
        var espessuraMaterial_ent = jsonObject["dadosEntrada"]["espessuraMaterial"].ToObject<double>();
        var oriComp_ent = jsonObject["dadosEntrada"]["oriComp"].ToString();
        var estatistica_ent = jsonObject["dadosEntrada"]["estatistica"].ToString();
        var tipoProduto_ent = jsonObject["dadosEntrada"]["tipoProduto"].ToString();
        var referenciaFornec_ent = jsonObject["dadosEntrada"]["referenciaFornec"]?.ToString();
        var codItemReferenciaOutraEmpresa_ent = jsonObject["dadosEntrada"]["codItemReferenciaOutraEmpresa"]?.ToString();
        var codItemReduzidoOutraEmpresa_ent = jsonObject["dadosEntrada"]["codItemReduzidoOutraEmpresa"]?.ToString();
        var descrItemReduzOutraEmpresa_ent = jsonObject["dadosEntrada"]["descrItemReduzOutraEmpresa"]?.ToString();
        var diametroInterno_ent = jsonObject["dadosEntrada"]["diametroInterno"].ToObject<double>();
        var diametroExterno_ent = jsonObject["dadosEntrada"]["diametroExterno"].ToObject<double>();
        var densidade_ent = jsonObject["dadosEntrada"]["densidade"].ToObject<double>();
        var codServEfdReinf_ent = jsonObject["dadosEntrada"]["codServEfdReinf"].ToString();
        var motivo_ent = jsonObject["dadosEntrada"]["motivo"]?.ToString();

        /* fim variaveis */

        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico");
        var request = Api.CreateRequest(Method.PUT);
        request.AddHeader("empresa", "1");

        var bodyObject = "" +
          "{"+
             $"\"codigo\": {codigo}," +
             $"\"nome\": \"TESTE LEO\"," +
             $"\"unidadeMedida\": \"{unidadeMedida}\"," +
             $"\"classificacaoFiscal\": \"{classificacaoFiscal}\"," +
             $"\"finalidade\": {finalidade}," +
             $"\"origem\": {origem}," +
             $"\"tipo\": {tipo}," +
             $"\"procedencia\": {procedencia}," +
             $"\"codigoBarras\": \"{codigoBarras}\"," +
             $"\"codigoBarraDun14\": \"{codigoBarraDun14}\"," +
             $"\"mascaraReferencia\": \"{mascaraReferencia}\"," +
             $"\"controlaLote\": {controlaLote}," +
             $"\"controlaDataValidadeLote\": {controlaDataValidadeLote}," +
             $"\"controlaConcentracaoLote\": {controlaConcentracaoLote}," +
             $"\"selecaoLoteSemManutencao\": {selecaoLoteSemManutencao}," +
             $"\"contQuaFioLote\": {contQuaFioLote}," +
             $"\"codItemInter\": \"{codItemInter}\"," +
             $"\"refTecnica\": \"{refTecnica}\"," +
             $"\"compQuimLotes\": \"{compQuimLotes}\"," +
             $"\"loteFornec\": \"{loteFornec}\"," +
             $"\"codigoQualidade\": \"{codigoQualidade}\"," +
             $"\"controlaTemperatura\": \"{controlaTemperatura}\"," +
             $"\"codigoItemAlfaNumerico\": \"{codigoItemAlfaNumerico}\"," +
             $"\"formatoInicialCodProduto\": \"{formatoInicialCodProduto}\"," +
             $"\"referenciaImportacao\": \"{referenciaImportacao}\"," +
             $"\"volumeMetroCubItem\": {volumeMetroCubItem}," +
             $"\"loteCalculoVolume\": {loteCalculoVolume}," +
             $"\"arredondaOcupacaoVolumeLote\": \"{arredondaOcupacaoVolumeLote}\"," +
             $"\"tipoMaterial\": \"{tipoMaterial}\"," +
             $"\"controlFCI\": \"{controlFCI}\"," +
             $"\"unidadesPorEtiqueta\": {unidadesPorEtiqueta}," +
             $"\"metodoControleEstoque\": \"{metodoControleEstoque}\"," +
             $"\"flagReplicado\": {flagReplicado}," +
             "\"dadosSaida\": {" +
               $"\"mascara\": \"{mascara_sai}\"," +
               $"\"situacao\": {situacao_sai}," +
               $"\"marca\": \"{marca_sai}\"," +
               $"\"descricao\": \"{descricao_sai}\"," +
               $"\"perComissao\": {perComissao_sai}," +
               $"\"perIPI\": {perIPI_sai}," +
               $"\"pesoLiquido\": {pesoLiquido_sai}," +
               $"\"pesoBruto\": {pesoBruto_sai}," +
               $"\"situacaoICMS\": {situacaoICMS_sai}," +
               $"\"tipoFaturamento\": \"{tipoFaturamento_sai}\"," +
               $"\"unidadeConversao\": \"{unidadeConversao_sai}\"," +
               $"\"unidadeVenda\": \"{unidadeVenda_sai}\"," +
               $"\"ribana\": \"{ribana_sai}\"," +
               $"\"tipoGrupo\": \"{tipoGrupo_sai}\"," +
               $"\"codNatureza\": {codNatureza_sai}," +
               $"\"qtdeEmbalagemPadrao\": {qtdeEmbalagemPadrao_sai}," +
               $"\"alternativo\": \"{alternativo_sai}\"," +
               $"\"codMarkup\": \"{codMarkup_sai}\"," +
               $"\"observacao\": \"{observacao_sai}\"," +
               $"\"valorIPIUnidade\": {valorIPIUnidade_sai}," +
               $"\"juntoONU\": \"{juntoONU_sai}\"," +
               $"\"codClasse\": \"{codClasse_sai}\"," +
               $"\"subClasse\": \"{subClasse_sai}\"," +
               $"\"classeItem\": \"{classeItem_sai}\"," +
               $"\"grupoItem\": \"{grupoItem_sai}\"," +
               $"\"familiaItem\": \"{familiaItem_sai}\"," +
               $"\"tara\": {tara_sai}," +
               $"\"reducaoIPI\": {reducaoIPI_sai}," +
               $"\"codMensagem\": \"{codMensagem_sai}\"," +
               $"\"diasGarantia\": \"{diasGarantia_sai}\"," +
               $"\"justificativa\": \"{justificativa_sai}\"," +
               $"\"dataDesativacao\": \"{dataDesativacao_sai}\"," +
               $"\"dataReativacao\": \"{dataReativacao_sai}\"," +
               $"\"codIBC\": \"{codIBC_sai}\"," +
               $"\"qtdVen\": {qtdVen_sai}," +
               $"\"pesIte\": {pesIte_sai}," +
               $"\"codCli\": \"{codCli_sai}\"," +
               $"\"EAN13\": \"{EAN13_sai}\"," +
               $"\"rua\": \"{rua_sai}\"," +
               $"\"gaveta\": \"{gaveta_sai}\"," +
               $"\"codNomeEmb\": \"{codNomeEmb_sai}\"," +
               $"\"codLab\": \"{codLab_sai}\"," +
               $"\"disponivel45\": \"{disponivel45_sai}\"," +
               $"\"disponivel46\": \"{disponivel46_sai}\"," +
               $"\"seqHistMasc\": \"{seqHistMasc_sai}\"," +
               $"\"alturaDimenEmbalagem\": {alturaDimenProduto_sai}," +
               $"\"comprDimenEmbalagem\": {comprDimenEmbalagem_sai}," +
               $"\"larguraDimenEmbalagem\": {larguraDimenEmbalagem_sai}," +
               $"\"alturaDimenProduto\": {alturaDimenProduto_sai}," +
               $"\"comprDimenProduto\": {comprDimenProduto_sai}," +
               $"\"larguraDimenProduto\": {larguraDimenProduto_sai}," +
               $"\"unidadeTributavel\": \"{unidadeTributavel_sai}\"," +
               $"\"fatorConversaoUnidade\": {fatorConversaoUnidade_sai}," +
               $"\"diametroEmbalagem\": {diametroEmbalagem_sai}," +
               $"\"diametroProduto\": {diametroProduto_sai}," +
               "itensPorCliente\": [" +
                 // "{" +
                 //  $"\"codCliente\": 9999999,"+
                 // $"\"codItemCliente\": "string""+
                 // "}" +
               "]"+
             "},"+
             "\"dadosEntrada\": {" +
               $"\"mascara\": \"{mascara_ent}\"," +
               $"\"situacao\": {situacao_ent}," +
               $"\"codNatureza\": {codNatureza_ent}," +
               $"\"descricao\": \"{descricao_ent}\"," +
               $"\"justificativa\": \"{justificativa_ent}\"," +
               $"\"dataDesativacao\": \"{dataDesativacao_ent}\"," +
               $"\"dataReativacao\": \"{dataReativacao_ent}\"," +
               $"\"finalidadeMaterial\": {finalidadeMaterial_ent}," +
               $"\"codigoICMS\": {codigoICMS_ent}," +
               $"\"codigoIPI\": {codigoIPI_ent}," +
               $"\"codContaContabil\": \"{codContaContabil_ent}\"," +
               $"\"tipoControleSaida\": {tipoControleSaida_ent}," +
               $"\"fatorConversao\": \"{fatorConversao_ent}\"," +
               $"\"codUnidMedida\": \"{codUnidMedida_ent}\"," +
               $"\"materiaPrimaCSM\": \"{materiaPrimaCSM_ent}\"," +
               $"\"quantidadeMateriaPrima\": \"{quantidadeMateriaPrima_ent}\"," +
               $"\"percentualConcentracaoMM\": \"{percentualConcentracaoMM_ent}\"," +
               $"\"fichaInspecaoLI\": \"{fichaInspecaoLI_ent}\"," +
               $"\"localFisicoFI\": \"{localFisicoFI_ent}\"," +
               $"\"classeToxicologicaFM\": \"{classeToxicologicaFM_ent}\"," +
               $"\"regMinAgricultura\": \"{regMinAgricultura_ent}\"," +
               $"\"multiplicador\": {multiplicador_ent}," +
               $"\"codigoFabricante\": \"{codigoFabricante_ent}\"," +
               $"\"comprimento\": {comprimento_ent}," +
               $"\"largura\": {largura_ent}," +
               $"\"altura\": {altura_ent}," +
               $"\"pesoPadraoNBR\": {pesoPadraoNBR_ent}," +
               $"\"UnidadeConsumo\": \"{UnidadeConsumo_ent}\"," +
               $"\"fatorConversaoUniCompra\": \"{fatorConversaoUniCompra_ent}\"," +
               $"\"inativoParaCompra\": \"{inativoParaCompra_ent}\"," +
               $"\"pesoPadraoNBRDesc\": \"{pesoPadraoNBRDesc_ent}\"," +
               $"\"codNaturezaConsignada\": \"{codNaturezaConsignada_ent}\"," +
               $"\"qtdeCompra\": {qtdeCompra_ent}," +
               $"\"equipProtecIndivid\": \"{equipProtecIndivid_ent}\"," +
               $"\"regMinisterioSaude\": \"{regMinisterioSaude_ent}\"," +
               $"\"dataValidade\": \"{dataValidade_ent}\"," +
               $"\"denomComumBrasil\": \"{denomComumBrasil_ent}\"," +
               $"\"medidaEPI\": \"{medidaEPI_ent}\"," +
               $"\"tipcom\": {tipcom_ent}," +
               $"\"seqHistMasc\": \"{seqHistMasc_ent}\"," +
               $"\"espessuraMaterial\": {espessuraMaterial_ent}," +
               $"\"oriComp\": {oriComp_ent}," +
               $"\"estatistica\": \"{estatistica_ent}\"," +
               $"\"tipoProduto\": \"{tipoProduto_ent}\"," +
               $"\"referenciaFornec\": \"{referenciaFornec_ent}\"," +
               $"\"codItemReferenciaOutraEmpresa\": \"{codItemReferenciaOutraEmpresa_ent}\"," +
               $"\"codItemReduzidoOutraEmpresa\": \"{codItemReduzidoOutraEmpresa_ent}\"," +
               $"\"descrItemReduzOutraEmpresa\": \"{descrItemReduzOutraEmpresa_ent}\"," +
               $"\"diametroInterno\": {diametroInterno_ent}," +
               $"\"diametroExterno\": {diametroExterno_ent}," +
               $"\"densidade\": \"{densidade_ent}\"," +
               $"\"codServEfdReinf\": \"{codServEfdReinf_ent}\"," +
               $"\"motivo\": \"{motivo_ent}\"" +
             "}," +
             "\"dadosQualidade\": [" +
             //  {
             //    "tipoControle": 0," +
             //    "codComprador": 999," +
             //    "fornecedoresHomologados": [" +
             //      {" +
             //        "codFornecedor": 99999," +
             //        "statusPreferencial": true," +
             //        "codQualForn": 99999," +
             //        "situacaoHomologacao": 1" +
             //      }" +
             //    ]" +
             //  }" +
             //]," +
             //"DadosCustomizados": [" +
             //  {" +
             //    "campo": "string"," +
             //    "valor": "string"" +
             //  }" +
             "]" +
           "}";

        request.AddJsonBody(bodyObject);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          jsonObject = JObject.Parse(responseData);
          return true;
        } else {
          var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
          var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro ao Duplicar Item";
          throw new Exception($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        throw new Exception($"Erro ao Duplicar Item: {ex.Message}");
      }

    }

    internal static async Task<bool> DuplicateItemGenericoAsync(item_generico_duplicacao itemGenerico) {
      try {
        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/duplicar");
        var request = Api.CreateRequest(Method.PUT);
        request.AddHeader("empresa", "1");

        var bodyObject = new {
          tipoModulo = "E",
          descricaoItem = itemGenerico.descricaoItem,
          descricaoCompleta = itemGenerico.descricaoCompleta,
          nivelMascaraEntrada = itemGenerico.nivelMascaraEntrada,
          nivelMascaraSaida = itemGenerico.nivelMascaraSaida,
          pesoLiquido = itemGenerico.pesoLiquido,
          pesoBruto = itemGenerico.pesoBruto,
          unidadeMedida = itemGenerico.unidadeMedida,
          classificacaoOrigem = itemGenerico.classificacaoOrigem,
          classificacaoFinalidade = itemGenerico.classificacaoFinalidade,
          tipoControleSaida = itemGenerico.tipoControleSaida,
          classificacaoFiscal = itemGenerico.classificacaoFiscal,
          localizacaoEntrada = itemGenerico.localizacaoEntrada,
          localizacaoSaida = itemGenerico.localizacaoSaida,
          DadosCustomizados = new List<object> { new {
            campo = "",
            valor = ""
          }
        }
        };

        request.AddJsonBody(bodyObject);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          var jsonObject = JObject.Parse(responseData);
          return true;
        } else {
          var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
          var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro ao Duplicar Item";
          throw new Exception($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar item genérico");
        return false;
      }

    }

    internal static async Task<bool> DuplicaItemImportacaoAsync(item_generico_duplicacao itemGenerico) {
      JObject jsonObject = new JObject();
      
      try {
        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/{itemGenerico.codigo}");
        var request = Api.CreateRequest(Method.GET);
        request.AddHeader("empresa", "1");
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          jsonObject = JObject.Parse(responseData);

        }
      } catch (Exception ex) {
        throw new Exception($"Erro ao Buscar Item: {ex.Message}");
      }

      try {
        /* inicio variaveis */
        var codigo = string.Empty;
        var nome = itemGenerico.descricaoItem;
        var unidadeMedida = itemGenerico.unidadeMedida;
        var classificacaoFiscal = itemGenerico.classificacaoFiscal;

        var perIPI_sai = jsonObject["dadosSaida"]["perIPI"].ToObject<double>();
        var pesoLiquido_sai = itemGenerico.pesoLiquido;
        var pesoBruto_sai = itemGenerico.pesoBruto;

        var descricao_ent = itemGenerico.descricaoCompleta;

        var codContaContabil_ent = jsonObject["dadosEntrada"]["codContaContabil"]?.ToString();

        /* fim variaveis */

        var client = Api.GetClient(modulo: "ppcppadrao", endpoint: $"duplicarItemImportacao");
        var request = Api.CreateRequest(Method.PUT);
        request.AddHeader("empresa", "1");

        var bodyObject = "" +
          "{" +
             $"\"tipoModulo\": \"E\"," +
             $"\"codigoItem\": {codigo}," +
             $"\"descricaoItem\": \"{nome}\"," +
             $"\"descricaoCompleta\": \"{descricao_ent}\"," +
             $"\"nivelMascaraEntrada\": \"10.40.10.70.10\"," +
             $"\"nivelMascaraSaida\": \"10.40.10.70.10\"," +
             $"\"pesoLiquido\": {pesoLiquido_sai}," +
             $"\"pesoBruto\": {pesoBruto_sai}," +
             $"\"unidadeMedida\": \"{unidadeMedida}\"," +
             $"\"classificacaoOrigem\": 0," +
             $"\"classificacaoFinalidade\": 0," +
             $"\"tipoControleSaida\": 0," +
             $"\"classificacaoFiscal\": \"{classificacaoFiscal}\"," +
             $"\"localizacaoEntrada\": 0," +
             $"\"localizacaoSaida\": 0," +
             $"\"codContaContabil\": \"{codContaContabil_ent}\"," +
             $"\"perIPI\": {perIPI_sai}," +
             "\"dadosQualidade\": []" +
             "\"DadosCustomizados\": []" +
           "}";

        request.AddJsonBody(bodyObject);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          jsonObject = JObject.Parse(responseData);
          return true;
        } else {
          var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
          var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro ao Duplicar Item";
          throw new Exception($"Erro: {response.StatusCode}\r\n{errorMessage}");
        }
      } catch (Exception ex) {
        throw new Exception($"Erro ao Duplicar Item: {ex.Message}");
      }

    }

    internal static async Task<ItemGenerico> GetItemGenericoAsync(string codigo) {
      ItemGenerico _return = null;

      try {
        var client = Api.GetClient(modulo: "itens", endpoint: $"itemGenerico/{codigo}");
        var request = Api.CreateRequest(Method.GET);
        request.AddHeader("empresa", "1");
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) {
          var responseData = response.Content;
          var jsonObject = JObject.Parse(responseData);

          _return = new ItemGenerico {
            Codigo = jsonObject["codigo"]?.ToString(),
            Nome = jsonObject["nome"]?.ToString(),
            UnidadeMedida = jsonObject["unidadeMedida"]?.ToString(),
            ClassificacaoFiscal = jsonObject["classificacaoFiscal"]?.ToString(),
            //Finalidade = jsonObject["finalidade"]?.ToObject<int>(),

            // Dados de Entrada
            DadosEntrada = new DadosEntrada {
              Mascara = jsonObject["dadosEntrada"]["mascara"]?.ToString(),
              Descricao = jsonObject["dadosEntrada"]["descricao"]?.ToString(),
              TipoControleSaida = jsonObject["dadosEntrada"]["tipoControleSaida"].ToObject<int>(),
              //Altura = jsonObject["dadosEntrada"]["altura"]?.ToString(),
              //Largura = jsonObject["dadosEntrada"]["largura"]?.ToString(),
              //Comprimento = jsonObject["dadosEntrada"]["comprimento"]?.ToString(),
              //CodigoFabricante = jsonObject["dadosEntrada"]["codigoFabricante"]?.ToString(),
            },

            // Dados de Saída
            DadosSaida = new DadosSaida {
              Mascara = jsonObject["dadosSaida"]["mascara"]?.ToString(),
              Descricao = jsonObject["dadosSaida"]["descricao"]?.ToString(),
              //PesoBruto = jsonObject["dadosSaida"]["pesoBruto"]?.ToString(),
              //PesoLiquido = jsonObject["dadosSaida"]["pesoLiquido"]?.ToString(),
              //Marca = jsonObject["dadosSaida"]["marca"]?.ToString()
            },

            //// Dados Técnicos
            //DadosTecnicos = new DadosTecnicos {
            //  Composicao = jsonObject["dadosTecnicos"]["composicao"]?.ToString(),
            //  Gramatura = jsonObject["dadosTecnicos"]["gramatura"]?.ToString(),
            //  Largura = jsonObject["dadosTecnicos"]["largura"]?.ToString(),
            //  Rendimento = jsonObject["dadosTecnicos"]["rendimento"]?.ToString()
            //},

            //SaldoEstoque = jsonObject["saldoEstoque"]?.ToObject<List<SaldoEstoque>>()
          };

        }
        //else {
        //  var errorResponse = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(response.Content);
        //  var errorMessage = errorResponse?.FirstOrDefault()?.mensagem ?? "Erro desconhecido";

        //  Toast.Error($"Erro: {response.StatusCode}\r\n{errorMessage}");
        //}
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao carregar item genérico");
      }

      return _return;
    }

  }
}
