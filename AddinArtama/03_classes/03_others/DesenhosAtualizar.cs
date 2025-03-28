using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddinArtama
{
    public class DesenhosAtualizar
    {
        [DisplayName("-")]
        public bool Atualizar { get; set; }

        [DisplayName("Seq.")]
        public int Id { get; set; }

        [DisplayName("Nome Arquivo")]
        public string ShorName { get; set; }

        [Browsable(false)]
        public string PathName { get; set; }
    }
}
