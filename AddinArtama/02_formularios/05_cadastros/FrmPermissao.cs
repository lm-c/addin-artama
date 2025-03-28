using LmCorbieUI;
using LmCorbieUI.LmForms;
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
  public partial class FrmPermissao : LmSingleForm {
    readonly TreeNode _tnSolucao;
    readonly string[] _spl;
    readonly bool verificarParent = false;
    readonly string _permissoesEntrada;
    private string _permissoesSaida;

    public FrmPermissao(string permiss) {
      InitializeComponent();

      if (!string.IsNullOrEmpty(permiss)) {
        _spl = permiss.Split('^');
        _permissoesEntrada = permiss;
      }

      ImageList il = new ImageList();
      il.Images.Add(((int)TipoPermissao.Menu).ToString(), Properties.Resources.trv_menu);                                 // 0
      il.Images.Add(((int)TipoPermissao.Formulario).ToString(), Properties.Resources.trv_form);                           // 1
      il.Images.Add(((int)TipoPermissao.Configuracao).ToString(), Properties.Resources.trv_permissao);                    // 2

      trvSolution.ImageList = il;
      _tnSolucao = trvSolution.Nodes.Add(PermissoesSistema.Solucao.ToString(), PermissoesSistema.Solucao.ObterDescricaoEnum());

      NovoNo(_tnSolucao, PermissoesSistema.AplicacaoProcesso);
      NovoNo(_tnSolucao, PermissoesSistema.PropsPersonalizadas);
      TreeNode tnDesenho = NovoNo(_tnSolucao, PermissoesSistema.Desenho);
      NovoNo(tnDesenho, PermissoesSistema.CriarAlterarDesenhos);
      NovoNo(tnDesenho, PermissoesSistema.AtualizarTemplatesDesenhos);
      TreeNode tnExportar = NovoNo(_tnSolucao, PermissoesSistema.Exportar);
      NovoNo(tnExportar, PermissoesSistema.ExportarPDF);
      NovoNo(tnExportar, PermissoesSistema.ExportarDXF);
      TreeNode tnCadastros = NovoNo(_tnSolucao, PermissoesSistema.Cadastros);
      NovoNo(tnCadastros, PermissoesSistema.UsuarioCad);
      NovoNo(tnCadastros, PermissoesSistema.PerfilUsuarioCad);
      NovoNo(tnCadastros, PermissoesSistema.SenhaRedefinir);

      TreeNode tnRelatorios = NovoNo(_tnSolucao, PermissoesSistema.Relatorios);
      NovoNo(tnRelatorios, PermissoesSistema.ProcessoFabricacao);
      NovoNo(tnRelatorios, PermissoesSistema.PackList);
      NovoNo(tnRelatorios, PermissoesSistema.PlanoPintura);
      NovoNo(tnRelatorios, PermissoesSistema.ManutencaoPinturaPackList);
      NovoNo(tnRelatorios, PermissoesSistema.ReportWorks);

      NovoNo(_tnSolucao, PermissoesSistema.Configuracao);

      _tnSolucao.ExpandAll();
      _tnSolucao.Checked = true;
      verificarParent = false;
      CheckTreeViewNode(_tnSolucao);
      verificarParent = true;

      tnDesenho.Collapse(true);
      tnCadastros.Collapse(true);
      tnRelatorios.Collapse(true);
    }

    private static TreeNode NovoNo(TreeNode value, PermissoesSistema perm) {
      return value.Nodes.Add(perm.ToString(), perm.ObterDescricaoEnum(), perm.ObterPermissao(), perm.ObterPermissao());
    }

    private void CheckTreeViewNode(TreeNode node) {
      foreach (TreeNode item in node.Nodes) {
        item.Checked = (_spl != null && _spl.Contains(((int)Enum.Parse(typeof(PermissoesSistema), item.Name)).ToString())) ? true : false;

        if (item.Nodes.Count > 0) {
          this.CheckTreeViewNode(item);
        }
      }
    }

    private void CheckParentTreeViewNode(TreeNode node, bool isChecked) {
      if (!isChecked) {
        foreach (TreeNode item in node.Nodes) {
          if (item.Checked)
            return;
        }
      }

      node.Checked = isChecked;

      if (node.Parent != null)
        CheckParentTreeViewNode(node.Parent, node.Checked);
    }

    private void TrvSolution_AfterSelect(object sender, TreeViewEventArgs e) {
      if (e.Node.Parent != null && verificarParent)
        CheckParentTreeViewNode(e.Node.Parent, e.Node.Checked);
    }

    //marca com filhos
    private void CheckTreeViewNode(TreeNode node, Boolean isChecked) {
      foreach (TreeNode item in node.Nodes) {
        item.Checked = isChecked;

        if (item.Nodes.Count > 0) {
          this.CheckTreeViewNode(item, isChecked);
        }
      }
    }

    private void VerificarItensSelecionados(TreeNode node) {
      foreach (TreeNode item in node.Nodes) {
        if (item.Checked)
          _permissoesSaida += (int)Enum.Parse(typeof(PermissoesSistema), item.Name) + "^";

        if (item.Nodes.Count > 0) {
          this.VerificarItensSelecionados(item);
        }
      }
    }

    private void BtnSalvar_Click(object sender, EventArgs e) {
      _permissoesSaida = string.Empty;

      VerificarItensSelecionados(_tnSolucao);

      if (_permissoesSaida.EndsWith("^"))
        _permissoesSaida = _permissoesSaida.Substring(0, _permissoesSaida.Length - 1);

      var frm = UcPainelTarefas.Instancia.pnlMain.Controls.OfType<FrmPerfil>().FirstOrDefault();

      if (frm != null) {
        frm.model.permissoes = _permissoesSaida;
        frm.BtnSalvar_Click(null, null);
      }

      Close();
    }

    private void BtnMarcar_Click(object sender, EventArgs e) {
      CheckTreeViewNode(_tnSolucao, true);
    }

    private void BtnDesmarcar_Click(object sender, EventArgs e) {
      CheckTreeViewNode(_tnSolucao, false);
    }

    private void BtnFechar_Click(object sender, EventArgs e) {

    }
  }
}
