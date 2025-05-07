using LmCorbieUI.Metodos.AtributosCustomizados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;
using System.IO;

namespace AddinArtama {
  internal class produto_erp {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    [DataObjectField(true, false)]
    public int id { get; set; }

    [DataObjectField(false, true)]
    public int codigo_produto { get; set; }
    
    [DataObjectField(false, true)]
    public int codigo_componente { get; set; }

    [StringLength(60)]
    public string descricao { get; set; }

    [StringLength(50)]
    public string name { get; set; }

    [StringLength(250)]
    public string pathname { get; set; }
    
    [StringLength(50)]
    public string referencia { get; set; } 

  }
}
