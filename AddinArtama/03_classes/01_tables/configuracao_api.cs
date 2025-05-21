using LmCorbieUI.Metodos.AtributosCustomizados;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using LmCorbieUI;
using System.Linq;
using System.Windows.Forms;
using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace AddinArtama {
  internal class configuracao_api {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataObjectField(true, false)]
    [LarguraColunaGrid(80)]
    [DisplayName("Código")]
    public int id { get; set; }

    /// <summary>
    /// 3 - Materiais acabados - nivel 1 da máscara
    /// </summary>
    [Browsable(false)]
    [StringLength(1)]
    public string grupo { get; set; }

    /// <summary>
    /// 05 - Rotina importação addin - nivel 2 da máscara
    /// </summary>
    [Browsable(false)]
    [StringLength(2)]
    public string subgrupo { get; set; }

    /// <summary>
    /// 01 - Rotina importação addin - usado para peças - nivel 3 da máscara
    /// </summary>
    [Browsable(false)]
    [StringLength(2)]
    public string tipo_peca { get; set; }

    /// <summary>
    /// 01 - Rotina importação addin - usado para montagem - nivel 3 da máscara
    /// </summary>
    [Browsable(false)]
    [StringLength(2)]
    public string tipo_montagem { get; set; }

    /// <summary>
    /// 01 - Rotina importação addin - nivel 4 da máscara
    /// </summary>
    [Browsable(false)]
    [StringLength(2)]
    public string familia { get; set; }

    /// <summary>
    /// 01 - Rotina importação addin - usado para peças - nivel 5 da máscara (usar no código reduzido aumentar quando acabado sequencial)
    /// </summary>
    [Browsable(false)]
    [StringLength(2)]
    public string classificacao { get; set; }
    
    /// <summary>
    /// último sequencial usado para o código reduzido (Quando alterado nivel 5, deve ser zerado
    /// </summary>
    [Browsable(false)]
    public int sequencial { get; set; }

    [Browsable(false)]
    public int classificacaoFiscal { get; set; }

    [Browsable(false)]
    public int finalidade { get; set; }

    [Browsable(false)]
    public int origem { get; set; }

    [Browsable(false)]
    public int tipo { get; set; }
    
    [Browsable(false)]
    public int procedencia { get; set; }

    [Browsable(false)]
    public double perComissao { get; set; }

    [Browsable(false)]
    public double perIPI { get; set; }

    [Browsable(false)]
    public int situacaoICMS { get; set; }
    
    [Browsable(false)]
    public int codNatureza { get; set; }
    
    [Browsable(false)]
    public int codigoICMS { get; set; }
    
    [Browsable(false)]
    public int codigoIPI { get; set; }

    [Browsable(false)]
    [StringLength(10)]
    public string codContaContabil { get; set; }

    [Browsable(false)]
    public int tipoControleSaida { get; set; }
    
    // api config
    [Browsable(false)]
    public int codigoEmpresa { get; set; }

    [Browsable(false)]
    [StringLength(250)]
    public string endereco { get; set; }

    [Browsable(false)]
    [StringLength(500)]
    public string token { get; set; }

    public static async Task<bool> SalvarAsync(configuracao_api configuracao) {
      try {
        using (ContextoDados db = new ContextoDados()) {
          if (configuracao.id == 0) {

            db.configuracao_api.Add(configuracao);
            db.SaveChanges();
            return true;
          } else {
            var modelAlt = db.configuracao_api.FirstOrDefault(x => x.id == configuracao.id);
            modelAlt.grupo = configuracao.grupo;
            modelAlt.subgrupo = configuracao.subgrupo;
            modelAlt.tipo_peca = configuracao.tipo_peca;
            modelAlt.tipo_montagem = configuracao.tipo_montagem;
            modelAlt.familia = configuracao.familia;
            modelAlt.classificacao = configuracao.classificacao;
            modelAlt.sequencial = configuracao.sequencial;
            modelAlt.classificacaoFiscal = configuracao.classificacaoFiscal;
            modelAlt.finalidade = configuracao.finalidade;
            modelAlt.origem = configuracao.origem;
            modelAlt.tipo = configuracao.tipo;
            modelAlt.procedencia = configuracao.procedencia;
            modelAlt.perComissao = configuracao.perComissao;
            modelAlt.perIPI = configuracao.perIPI;
            modelAlt.situacaoICMS = configuracao.situacaoICMS;
            modelAlt.codNatureza = configuracao.codNatureza;
            modelAlt.codigoICMS = configuracao.codigoICMS;
            modelAlt.codigoIPI = configuracao.codigoIPI;
            modelAlt.codContaContabil = configuracao.codContaContabil;
            modelAlt.tipoControleSaida = configuracao.tipoControleSaida;
            modelAlt.codigoEmpresa = configuracao.codigoEmpresa;
            modelAlt.endereco = configuracao.endereco;
            modelAlt.token = configuracao.token;

            db.SaveChanges();
            return true;
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Salvar Configuração API.\r\n{ex.Message}", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    public static configuracao_api Selecionar() {
      var _return = new configuracao_api();

      try {
        using (ContextoDados db = new ContextoDados()) {
          _return = Queryable.FirstOrDefault(db.configuracao_api);
        }
      } catch (Exception ex) {
        Toast.Warning("Erro ao selecionar Configuração API.\n" +
            "-------------------------------------\n" +
            "" + ex.Message);
        _return = null;
      }

      return _return;
    }
  }
}
