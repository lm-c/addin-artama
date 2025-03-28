using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddinArtama {
  public partial class FrmSelecionarPerfil : LmSingleForm {
    internal SortableBindingList<W_UsuarioPerfil> _listaPerfis = new SortableBindingList<W_UsuarioPerfil>();

    public FrmSelecionarPerfil(SortableBindingList<W_UsuarioPerfil> listaPerfis) {
      InitializeComponent();

      var list = perfis.SelecionarPerfis(-1);

      foreach (var p in list) {
        bool adicionado = false;
        foreach (var item in listaPerfis) {
          if (item.Codigo == p.Codigo) {
            dgv.Rows.Add(true, p.Codigo, p.Descricao);
            adicionado = true;
            break;
          }
        }

        if (!adicionado)
          dgv.Rows.Add(false, p.Codigo, p.Descricao);
      }
    }

    private void BtnConfirmar_Click(object sender, EventArgs e) {
      foreach (DataGridViewRow r in dgv.Rows) {
        bool selecionado = Convert.ToBoolean(r.Cells["Sel"].Value);

        if (!selecionado) continue;

        int id = Convert.ToInt32(r.Cells["ID"].Value);
        string desc = Convert.ToString(r.Cells["Descricao"].Value);

        _listaPerfis.Add(new W_UsuarioPerfil { Codigo = id, Descricao = desc });
      }
      DialogResult = DialogResult.OK;
    }
  }
}
