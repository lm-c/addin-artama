using System.ComponentModel;

namespace AddinArtama {
  internal class Z_Chapa {
    [Browsable(false)]
    [DataObjectField(true, false)]
    public int Id { get; set; }

    [DisplayName("Código")]
    public int CodigoChapa { get; set; }

    [DataObjectField(false, true)]
    [DisplayName("Descrição Chapa")]
    public string DescricaoChapa { get; set; }

    [DisplayName("Descrição Material")]
    public string DescricaoMaterial { get; set; }

    public bool Ativo { get; set; }
  }
}
