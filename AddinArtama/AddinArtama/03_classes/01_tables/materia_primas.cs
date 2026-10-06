using LmCorbieUI;
using LmCorbieUI.Metodos.AtributosCustomizados;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddinArtama {
  internal class materia_primas {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataObjectField(true, false)]
    [LarguraColunaGrid(80)]
    [DisplayName("Código")]
    public int id { get; set; }

    [LarguraColunaGrid(80)]
    [DisplayName("Código")]
    public int codigo { get; set; }

    [DataObjectField(false, true)]
    [Required(ErrorMessage = "Campo \"Descrição\" é Obrigatório!")]
    [StringLength(250)]
    [LarguraColunaGrid(500)]
    [DisplayName("Descrição")]
    public string descricao { get; set; }

    public TipoMateriaPrima tipo_materia_prima { get; set; }

    public double? espessura { get; set; } = null;

    public double? largura { get; set; } = null;

    public double? comprimento { get; set; } = null;

    [Required(ErrorMessage = "Material é Obrigatório!"), Browsable(false)]
    public int material_id { get; set; }
    [ForeignKey("material_id"), Browsable(false), NaoVerificarAlteracao]
    public materiais material { get; set; }

    [LarguraColunaGrid(80)]
    public bool ativo { get; set; }

    public static bool Salvar(materia_primas materiaPrima) {
      try {
        using (ContextoDados db = new ContextoDados()) {
          if (materiaPrima.id == 0) {
            if (db.materia_primas.Any(x => x.descricao == materiaPrima.descricao)) {
              Toast.Warning("Já existe uma matéria prima cadastrada com esta descrição");
              return false;
            }

            db.materia_primas.Add(materiaPrima);
            db.SaveChanges();

            Toast.Success("Matéria Prima Cadastrada com Sucesso!");
          } else {
            if (db.materia_primas.Any((x => x.descricao == materiaPrima.descricao && x.id != materiaPrima.id))) {
              Toast.Warning("Já existe uma matéria prima cadastrada com esta descrição");
              return false;
            }

            var modelAlt = db.materia_primas.FirstOrDefault(x => x.id == materiaPrima.id);
            modelAlt.espessura = materiaPrima.espessura;
            modelAlt.codigo = materiaPrima.codigo;
            modelAlt.descricao = materiaPrima.descricao;
            modelAlt.material_id = materiaPrima.material_id;
            modelAlt.ativo = materiaPrima.ativo;

            db.SaveChanges();
            Toast.Success("Matéria Prima Alterada com Sucesso!");
          }
          return true;
        }
      } catch (Exception ex) {
        MsgBox.Show("Erro ao Salvar Material.", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    public static List<Z_Chapa> Selecionar(bool? ativo = null, double? espessura = null) {
      List<Z_Chapa> _return = new List<Z_Chapa>();

      using (ContextoDados db = new ContextoDados()) {
        var condicoes = string.Empty;
        var valores = new object[10];
        short pos = 0;

        condicoes += $"id > @{pos} && ";
        valores[pos] = 0;
        pos++;
        
        condicoes += $"tipo_materia_prima == @{pos} && ";
        valores[pos] = TipoMateriaPrima.Chapa;
        pos++;

        if (ativo != null) {
          condicoes += $"Ativo == @{pos} && ";
          valores[pos] = ativo.Value;
          pos++;
        }

        if (espessura != null) {
          condicoes += $"Espessura >= @{pos} && ";
          valores[pos] = (espessura - 0.3);
          pos++;

          condicoes += $"Espessura <= @{pos} && ";
          valores[pos] = (espessura + 0.3);
          pos++;

        }

        condicoes = condicoes.Substring(0, condicoes.Length - 3);

        var chapas = Enumerable.ToList(
          from x in db.materia_primas.Where(condicoes, valores).OrderBy(x => x.espessura)
          select new {
            x.id,
            x.codigo,
            x.descricao,
            x.espessura,
            x.ativo,
            material = x.material.descricao,
          });

        foreach (var chapa in chapas) {
          _return.Add(new Z_Chapa {
            Id = chapa.id,
            Espessura = chapa.espessura,
            CodigoChapa = chapa.codigo,
            DescricaoChapa = chapa.descricao,
            DescricaoMaterial = chapa.material,
            Ativo = chapa.ativo
          });
        }
      }

      return _return;
    }

    public static Z_Chapa Selecionar(int id) {
      var _return = new Z_Chapa();

      try {
        using (ContextoDados db = new ContextoDados()) {
          var chapa = Queryable.FirstOrDefault(
          from x in db.materia_primas.OrderBy(x => x.espessura)
              .Where(x => x.id == id)
          select new {
            x.id,
            x.codigo,
            x.descricao,
            x.espessura,
            x.ativo,
            material = x.material.descricao,
          });

          _return = (new Z_Chapa {
            Id = chapa.id,
            Espessura = chapa.espessura,
            CodigoChapa = chapa.codigo,
            DescricaoChapa = chapa.descricao,
            DescricaoMaterial = chapa.material,
            Ativo = chapa.ativo
          });
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Retornar Descrição Processo");
      }

      return _return;
    }

    public static materia_primas SelecionarMateriaPrima(int id) {
      var _return = new materia_primas();

      try {
        using (ContextoDados db = new ContextoDados()) {
          _return = Queryable.FirstOrDefault(
           db.materia_primas.Where(x => x.id == id));
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Retornar Descrição Processo");
      }

      return _return;
    }
  }
}
