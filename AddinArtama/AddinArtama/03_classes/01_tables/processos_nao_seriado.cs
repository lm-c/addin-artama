using LmCorbieUI.Metodos.AtributosCustomizados;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddinArtama {
  internal class processos_nao_seriado {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataObjectField(true, false)]
    [LarguraColunaGrid(80)]
    [DisplayName("Código")]
    public int id { get; set; }

    [LarguraColunaGrid(150)]
    [DisplayName("Código Operação")]
    public int codigo { get; set; }

    [LarguraColunaGrid(400)]
    [StringLength(250)]
    [DisplayName("Descriçao")]
    public string descricao { get; set; }

    [LarguraColunaGrid(120)]
    [DisplayName("Tipo de Sequencia")]
    public int tipo_sequencia { get; set; }

    [Browsable(false)]
    public bool gerar_dxf { get; set; }

    [Browsable(false)]
    public bool imprimir_filhos { get; set; }

  }
}
