using LmCorbieUI.Metodos.AtributosCustomizados;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static AddinArtama.Api;
using System.Collections.Generic;
using LmCorbieUI;
using System.Linq;
using System.Windows.Forms;
using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace AddinArtama {
  internal class item_generico_duplicacao {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataObjectField(true, false)]
    [LarguraColunaGrid(80)]
    [DisplayName("Código")]
    public int id { get; set; }

    [LarguraColunaGrid(80)]
    [StringLength(120)]
    [DisplayName("Código Item")]
    public string codigo { get; set; }

    [DataObjectField(false, true)]
    [Required(ErrorMessage = "Campo \"Descrição\" é Obrigatório!")]
    [StringLength(250)]
    [LarguraColunaGrid(500)]
    [DisplayName("Descrição")]
    public string descricao { get; set; }

    [StringLength(5000)]
    [LarguraColunaGrid(500)]
    [DisplayName("Observações")]
    public string observacao { get; set; }

    /* Enviado para duplicação */
    [Browsable(false)]
    public string tipoModulo { get; set; } = "E";

    [Browsable(false)]
    public string descricaoItem { get; set; } // denominação do Item

    [Browsable(false)]
    public string descricaoCompleta { get; set; } // denominação do Item

    [Browsable(false)]
    public string nivelMascaraEntrada { get; set; } // dadosEntrada.mascara

    [Browsable(false)]
    public string nivelMascaraSaida { get; set; } // dadosSaida.mascara

    [Browsable(false)]
    public double pesoLiquido { get; set; }

    [Browsable(false)]
    public double pesoBruto { get; set; }

    [Browsable(false)]
    public string unidadeMedida { get; set; }

    [Browsable(false)]
    public int classificacaoOrigem { get; set; }

    [Browsable(false)]
    public int classificacaoFinalidade { get; set; }

    [Browsable(false)]
    public int tipoControleSaida { get; set; }

    [Browsable(false)]
    public int classificacaoFiscal { get; set; }

    [Browsable(false)]
    public int localizacaoEntrada { get; set; }

    [Browsable(false)]
    public int localizacaoSaida { get; set; }

    [Browsable(false)]
    public List<DadosCustomizados> dadosCustomizados = new List<DadosCustomizados>();

    public static async Task<bool> SalvarAsync(item_generico_duplicacao material) {
      try {
        using (ContextoDados db = new ContextoDados()) {
          if (material.id == 0) {

            if (db.item_generico_duplicacao.Any(x => x.descricao == material.descricao)) {
              Toast.Warning("Já existe um Item cadastradao com esta descrição");
              return false;
            }

            // selecionar item API
            await SelItemAPI(material.codigo, material);

            db.item_generico_duplicacao.Add(material);
            db.SaveChanges();
            return true;
          } else {
            if (db.item_generico_duplicacao.Any((x => x.descricao == material.descricao && x.id != material.id))) {
              Toast.Warning("Já existe um Item cadastrado com esta descrição");
              return false;
            }

            var modelAlt = db.item_generico_duplicacao.FirstOrDefault(x => x.id == material.id);
            modelAlt.codigo = material.codigo;
            modelAlt.descricao = material.descricao;
            modelAlt.observacao = material.observacao;

            // selecionar item API
            await SelItemAPI(material.codigo, modelAlt);

            db.SaveChanges();
            return true;
          }
        }
      } catch (Exception ex) {
        MsgBox.Show($"Erro ao Salvar Item Duplicação.\r\n{ex.Message}", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    private static async Task SelItemAPI(string codigo, item_generico_duplicacao modelAlt) {
      var temp = await Api.GetItemGenericoAsync(codigo);
      if (temp != null) {
        modelAlt.descricaoItem = temp.Nome;
        modelAlt.descricaoCompleta = temp.DadosEntrada.Descricao;
        modelAlt.nivelMascaraEntrada = temp.DadosEntrada.Mascara;
        modelAlt.nivelMascaraSaida = temp.DadosSaida.Mascara;
        modelAlt.unidadeMedida = temp.UnidadeMedida;
        modelAlt.classificacaoOrigem = 0;
        modelAlt.classificacaoFinalidade = 0;
        modelAlt.tipoControleSaida = temp.DadosEntrada.TipoControleSaida;
        modelAlt.classificacaoFiscal = !string.IsNullOrEmpty(temp.ClassificacaoFiscal) ? Convert.ToInt32(temp.ClassificacaoFiscal) : 0;
        modelAlt.localizacaoEntrada = 0;
        modelAlt.localizacaoSaida = 0;
      } else {
        throw new Exception("'Código Item ERP' não cadastrado no Consistem");
      }
    }

    public static void Excluir(int item_id) {
      try {
        using (ContextoDados db = new ContextoDados()) {
          db.item_generico_duplicacao.Remove(db.item_generico_duplicacao.FirstOrDefault(x => x.id == item_id));
          db.SaveChanges();
        }
      } catch (Exception ex) {
        MsgBox.Show("Erro ao Excluir Usuario.", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public static List<item_generico_duplicacao> Selecionar(bool? ativo = null) {
      var _return = new List<item_generico_duplicacao>();

      try {
        using (ContextoDados db = new ContextoDados()) {
          var condicoes = string.Empty;
          var valores = new object[10];
          short pos = 0;

          condicoes += $"Id > @{pos} && ";
          valores[pos] = 0;
          pos++;

          if (ativo != null) {
            condicoes += $"Ativo == @{pos} && ";
            valores[pos] = ativo.Value;
            pos++;
          }
          condicoes = condicoes.Substring(0, condicoes.Length - 3);

          _return = Enumerable.ToList(db.item_generico_duplicacao.Where(condicoes, valores).OrderBy(x => x.descricao));
        }
      } catch (Exception ex) {
        Toast.Warning("Erro ao selecionar item duplicação.\n" +
            "-------------------------------------\n" +
            "" + ex.Message);
        _return = new List<item_generico_duplicacao>();
      }

      return _return;
    }

    public static item_generico_duplicacao Selecionar(int id) {
      var _return = new item_generico_duplicacao();

      try {
        using (ContextoDados db = new ContextoDados()) {
          _return = Queryable.FirstOrDefault(db.item_generico_duplicacao.Where(x => x.id == id));
        }
      } catch (Exception ex) {
        Toast.Warning("Erro ao selecionar item duplicação.\n" +
            "-------------------------------------\n" +
            "" + ex.Message);
        _return = null;
      }

      return _return;
    }
  }
}
