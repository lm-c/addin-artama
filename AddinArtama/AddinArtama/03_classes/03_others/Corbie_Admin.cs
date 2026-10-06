using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddinArtama {
  internal class Corbie_Admin {
    public static List<string> PermissoesPerfil { get; set; }

    public static bool TemPermissao(PermissoesSistema permissao) {
      if (PermissoesPerfil == null)
        return true;

      return PermissoesPerfil.Contains(((int)permissao).ToString());
    }
  }
}
