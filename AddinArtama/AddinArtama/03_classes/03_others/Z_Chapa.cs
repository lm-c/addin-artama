using LmCorbieUI.Metodos.AtributosCustomizados;
using System.ComponentModel;

namespace AddinArtama {
  internal class Z_Chapa {
    [DisplayName("Código Axion")]
    [DataObjectField(true, false)]
    [LarguraColunaGrid(120)]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public int Id { get; set; }

    [DisplayName("Espessura")]
    [LarguraColunaGrid(80)]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleRight)]
    public double? Espessura { get; set; }
    
    [DisplayName("Código")]
    [LarguraColunaGrid(120)]
    [AlinhamentoColunaGrid(System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter)]
    public int CodigoChapa { get; set; }

    [DataObjectField(false, true)]
    [DisplayName("Descrição Chapa")]
    [LarguraColunaGrid(350)]
    public string DescricaoChapa { get; set; }

    [DisplayName("Descrição Material")]
    [LarguraColunaGrid(200)]
    public string DescricaoMaterial { get; set; }

    [DisplayName("Ativo")]
    [LarguraColunaGrid(60)]
    public bool Ativo { get; set; }

    /// <summary>
    /// Padroniza a descrição para comparação: troca espaço não separável (comum ao colar do Excel/ERP)
    /// por espaço normal, junta espaços repetidos, tira espaços das pontas e ignora maiúsculas.
    /// </summary>
    public static string NormalizarDescricao(string texto) {
      if (string.IsNullOrEmpty(texto))
        return string.Empty;
      var limpo = System.Text.RegularExpressions.Regex.Replace(texto.Replace(' ', ' '), @"\s+", " ");
      return limpo.Trim().ToUpperInvariant();
    }
  }
}
