using LmCorbieUI.Metodos.AtributosCustomizados;
using System.ComponentModel;

namespace AddinArtama {
  internal class W_Processo {
    [EhLink]
    [LarguraColunaGrid(80)]
    [DisplayName("Código")]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleRight)]
    public int Codigo { get; set; }

    [LarguraColunaGrid(150)]
    [DisplayName("Código Operacao")]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleRight)]
    public int CodOperacao { get; set; }

    [LarguraColunaGrid(450)]
    [DisplayName("Descrição Operação")]
    public string DescrOperacao { get; set; }

    [LarguraColunaGrid(120)]
    [DisplayName("Mascara da Máquina")]
    public string MascaraMaquina { get; set; }
    
    [LarguraColunaGrid(120)]
    [DisplayName("Centro de Custo")]
    public string CentroCusto { get; set; }

    [LarguraColunaGrid(80)]
    [DisplayName("Ativo")]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public bool Ativo { get; set; }
  }
}
