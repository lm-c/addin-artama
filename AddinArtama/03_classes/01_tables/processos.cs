using LmCorbieUI;
using LmCorbieUI.Metodos.AtributosCustomizados;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Windows.Forms;

namespace AddinArtama {
  internal class processos {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataObjectField(true, false)]
    [LarguraColunaGrid(80)]
    [DisplayName("Código")]
    public int id { get; set; }

    [LarguraColunaGrid(120)]
    [DisplayName("Código Operação")]
    public int codigo_operacao { get; set; }

    [LarguraColunaGrid(120)]
    [DisplayName("Mascara da Máquina")]
    [StringLength(20)]
    public string mascara_maquina { get; set; }

    [LarguraColunaGrid(120)]
    [DisplayName("Centro de Custo")]
    [StringLength(20)]
    public string centro_custo { get; set; }

    [LarguraColunaGrid(80)]
    public bool ativo { get; set; }

    public static bool Salvar(processos processo) {
      try {
        using (ContextoDados db = new ContextoDados()) {
          if (processo.id == 0) {
            if (db.processos.Any(x => x.codigo_operacao == processo.codigo_operacao)) {
              Toast.Warning("Já existe um registro com este processo vinculado a uma máquina");
              return false;
            }

            db.processos.Add(processo);
            db.SaveChanges();

            Toast.Success("Processo Cadastrado com Sucesso!");
          } else {
            if (db.processos.Any((x => x.id != processo.id && x.codigo_operacao == processo.codigo_operacao))) {
              Toast.Warning("Já existe um registro com este processo vinculado a uma máquina");
              return false;
            }

            var modelAlt = db.processos.FirstOrDefault(x => x.id == processo.id);
            modelAlt.codigo_operacao = processo.codigo_operacao;
            modelAlt.mascara_maquina = processo.mascara_maquina;
            modelAlt.centro_custo = processo.centro_custo;
            modelAlt.ativo = processo.ativo;

            db.SaveChanges();

            Toast.Success("Processo Alterado com Sucesso!");
          }

          Processo.Carregar();
          return true;
        }
      } catch (Exception ex) {
        MsgBox.Show("Erro ao Salvar Processo.", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    public static List<processos> SelecionarTodos() {
      var _return = new List<processos>();

      try {
        using (ContextoDados db = new ContextoDados()) {
          _return = Enumerable.ToList(
           db.processos);
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Retornar Processos");
      }

      return _return;
    }

    public static List<W_Processo> SelecionarTodosRel() {
      var _return = new List<W_Processo>();

      try {
        using (ContextoDados db = new ContextoDados()) {
          Enumerable.ToList(
            from x in
           db.processos
            select new {
              x.id,
              x.codigo_operacao,
              x.mascara_maquina,
              x.centro_custo,
              x.ativo,
            })
            .ForEach(x => {
              var proc = Processo.ListaProcessos.FirstOrDefault(op => op.codOperacao == x.codigo_operacao);
              _return.Add(new W_Processo {
                Codigo = x.id,
                CodOperacao = x.codigo_operacao,
                DescrOperacao = proc.descrOperacao,
                MascaraMaquina = x.mascara_maquina,
                CentroCusto = x.centro_custo,
                Ativo = x.ativo,
              });
            });
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Retornar Processos");
      }

      return _return;
    }

    public static processos SelecionarProcesso(int id) {
      var _return = new processos();

      try {
        using (ContextoDados db = new ContextoDados()) {
          _return = Queryable.FirstOrDefault(
           db.processos.Where(x => x.id == id));
        }
      } catch (Exception ex) {
        LmException.ShowException(ex, "Erro ao Retornar Processo");
      }

      return _return;
    }
  }
}
