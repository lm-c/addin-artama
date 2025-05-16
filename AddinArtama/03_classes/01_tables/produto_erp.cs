using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AddinArtama {
  internal class produto_erp {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    [DataObjectField(true, false)]
    public int id { get; set; }

    [DataObjectField(false, true)]
    public long codigo_produto { get; set; }
    
    [DataObjectField(false, true)]
    public long codigo_componente { get; set; }

    [StringLength(60)]
    public string descricao { get; set; }

    [StringLength(50)]
    public string name { get; set; }

    [StringLength(250)]
    public string pathname { get; set; }
    
    [StringLength(50)]
    public string referencia { get; set; } 
    
    [StringLength(150)]
    public string configuracao { get; set; }  

  }
}
