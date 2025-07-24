using LmCorbieUI;
using LmCorbieUI.Controls;
using LmCorbieUI.Design;
using LmCorbieUI.LmForms;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AddinArtama {
  [ComVisible(true)]
  [ProgId(SWTASKPANE_PROGID)]
  public partial class UcPainelTarefas : LmUserControl {
    public const string SWTASKPANE_PROGID = "AddinArtama.SWTaskPane.Plugin";

    static UcPainelTarefas instancia;

    public static UcPainelTarefas Instancia {
      get {
        if (instancia == null)
          instancia = new UcPainelTarefas();

        return instancia;
      }
    }

    public UcPainelTarefas() {
      InitializeComponent();
      
      OcultarControles();

      if (ConexaoMySql.Database.StartsWith("teste"))
        LmCor.CorPrimaria = Color.Red;

      //LmCor.CorSecundaria = ValorPredefinido.model.CorSecundaria.StringToColor();

      // Forçar o uso de TLS 1.2 (ou versões anteriores se necessário)
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      // Ignorar erros de certificado SSL
      ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

      string imagePath = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\01 - Addin LM\LogoArtama.png";

      if (File.Exists(imagePath)) {
        pnlMain.BackgroundImage = Image.FromFile(imagePath);
      }
    }

    private void UcPainelTarefas_Load(object sender, EventArgs e) {
      instancia = this;

      AttControls(this);

      FrmLogin frm = new FrmLogin();
      AbrirFormFilho(frm);
    }

    internal void AbrirFormFilho(Form frm) {
      try {
        if (!pnlMain.Controls.ContainsKey(frm.Name)) {
          frm.Dock = System.Windows.Forms.DockStyle.Fill;
          frm.TopLevel = false;
          frm.Parent = pnlMain;
          frm.Show();

          if (frm is LmSingleForm)
            ((LmSingleForm)frm).Movimentar = false;

          pnlMain.Controls.Add(frm);
          frm.BringToFront();
        } else {
          pnlMain.Controls[frm.Name].BringToFront();
        }
      } catch (Exception ex) {
        Toast.Error("Erro ao Abrir Tela.\r\n" + ex.ToString());
      }
    }

    private void MsProcess_Click(object sender, EventArgs e) {
      FrmMateriaPrimaApl frm = new FrmMateriaPrimaApl();
      AbrirFormFilho(frm);
    }

    private void MsProperties_Click(object sender, EventArgs e) {
      FrmFileProperties frm = new FrmFileProperties();
      AbrirFormFilho(frm);
    }

    private void MsDesenho_Click(object sender, EventArgs e) {
      msMenuDesenho.Show((LmMenuItem)sender, ((LmMenuItem)sender).Width, ((LmMenuItem)sender).Height);
    }

    private void MsCriarDesenho_Click(object sender, EventArgs e) {
      var frm = new FrmDesenho();
      AbrirFormFilho(frm);
    }

    private void MsAtualizarDesenho_Click(object sender, EventArgs e) {
      var frm = new FrmFormatosAtualizar();
      AbrirFormFilho(frm);
    }

    private void MsMenuExportar_Click(object sender, EventArgs e) {
      msMenuExportar.Show((LmMenuItem)sender, ((LmMenuItem)sender).Width, ((LmMenuItem)sender).Height);
    }

    private void MsExportar_Click(object sender, EventArgs e) {
      var frm = new FrmExportarPDF();
      AbrirFormFilho(frm);
    }

    private void MsExportarDXF_Click(object sender, EventArgs e) {
      var frm = new FrmExportarDXF();
      AbrirFormFilho(frm);
    }

    private void MsCadastro_Click(object sender, EventArgs e) {
      msMenuCadastro.Show((LmMenuItem)sender, ((LmMenuItem)sender).Width, ((LmMenuItem)sender).Height);
    }

    private void MsUsuarioCad_Click(object sender, EventArgs e) {
      FrmUsuarioCad frm = new FrmUsuarioCad();
      AbrirFormFilho(frm);
    }

    private void MsPerfilCad_Click(object sender, EventArgs e) {
      FrmPerfil frm = new FrmPerfil();
      AbrirFormFilho(frm);
    }

    private void MsMaterialCad_Click(object sender, EventArgs e) {
      FrmMaterialCad frm = new FrmMaterialCad();
      AbrirFormFilho(frm);
    }

    private void MsMateriaPrimaCad_Click(object sender, EventArgs e) {
      FrmMateriaPrimaCad frm = new FrmMateriaPrimaCad();
      AbrirFormFilho(frm);
    }

    private void MsProcessoCad_Click(object sender, EventArgs e) {
      FrmProcessoCad frm = new FrmProcessoCad();
      AbrirFormFilho(frm);
    }

    private void MsRedefinirSenha_Click(object sender, EventArgs e) {
      FrmAlterarSenha frm = new FrmAlterarSenha();
      AbrirFormFilho(frm);
    }

    private void MsRelatorio_Click(object sender, EventArgs e) {
      msMenuRelatorio.Show((LmMenuItem)sender, ((LmMenuItem)sender).Width, ((LmMenuItem)sender).Height);
    }

    private void MsProcessoFabricacao_Click(object sender, EventArgs e) {
      var frm = new FrmProcessoFabricacao();
      AbrirFormFilho(frm);
    }

    private void MsPackList_Click(object sender, EventArgs e) {
      var frm = new FrmPackList();
      AbrirFormFilho(frm);
    }

    private void MsPlanoPintura_Click(object sender, EventArgs e) {
      var frm = new FrmPlanoPintura();
      AbrirFormFilho(frm);
    }

    private void MsManutPacklistPlanoDePintura_Click(object sender, EventArgs e) {
      var frm = new FrmManutProps();
      AbrirFormFilho(frm);
    }

    private void MsReportWorks_Click(object sender, EventArgs e) {
      var frm = new FrmReportWorks();
      frm.Show();
    }

    private void MsCSW_Click(object sender, EventArgs e) {
      msMenuCSW.Show((LmMenuItem)sender, ((LmMenuItem)sender).Width, ((LmMenuItem)sender).Height);
    }

    private void MsCadastroProduto_Click(object sender, EventArgs e) {
      var frm = new FrmProdutoImport();
      AbrirFormFilho(frm);
    }

    private void MsConfiguracaoProcesso_Click(object sender, EventArgs e) {
      var frm = new FrmProcessoCad();
      AbrirFormFilho(frm);
    }

    private void MsCadastroEngenhariaProduto_Click(object sender, EventArgs e) {
      Toast.Warning("Em Desenvolvmento!");
    }

    private void MsConfiguracaoIntegracao_Click(object sender, EventArgs e) {
      FrmConfigIntegrador frm = new FrmConfigIntegrador();
      frm.ShowDialog();
    }

    private void MsConfig_Click(object sender, EventArgs e) {
      FrmConfigTemplate frm = new FrmConfigTemplate();
      frm.ShowDialog();
    }

    private void MsLogout_Click(object sender, EventArgs e) {
      try {
        usuario_alocados.Deslogar();

        var frmLogin = pnlMain.Controls.OfType<FrmLogin>().FirstOrDefault();
        frmLogin.Visible = true;

        OcultarControles();

        foreach (var frm in pnlMain.Controls.OfType<LmSingleForm>().ToList()) {
          if (frm.Name != "FrmLogin") {
            frm.Close();
          }
        }
      } catch (Exception ex) {
        Toast.Error("Erro ao Fazer Logout");
      }
    }

    internal void ConfigurarPermissoes() {
      try {
        if (usuario_alocados.model.usuario_id == -1)
          return;

        Corbie_Admin.PermissoesPerfil = usuarios.SelecionarPermissoes(usuario_alocados.model.usuario_id);

        //Menu Controle
        msProcess.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.AplicacaoProcesso);

        msProperties.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.PropsPersonalizadas);

        msDesenho.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.Desenho);
        msCriarDesenho.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.CriarAlterarDesenhos);
        msAtualizarDesenho.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.AtualizarTemplatesDesenhos);

        msImprimir.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.Exportar);
        msExportarPDF.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.ExportarPDF);
        msExportarDXF.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.ExportarDXF);

        msCadastro.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.Cadastros);
        msUsuarioCad.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.UsuarioCad);
        msPerfilCad.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.PerfilUsuarioCad);
        msMaterialCad.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.MaterialCad);
        msMateriaPrimaCad.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.MateriaPrimaCad);
        msRedefinirSenha.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.SenhaRedefinir);

        msRelatorio.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.Relatorios);
        msProcessoFabricacao.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.ProcessoFabricacao);
        msPackList.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.PackList);
        msPlanoPintura.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.PlanoPintura);
        msManutPacklistPlanoDePintura.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.ManutencaoPinturaPackList);
        msReportWorks.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.ReportWorks);

        msCSW.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.menuIntegracao);
        msCadastroProduto.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.produtoCad);
        msConfiguracaoProcesso.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.processoConfig);
        msConfiguracaoIntegracao.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.IntegracaoConfig);

        msConfig.Visible = Corbie_Admin.TemPermissao(PermissoesSistema.Configuracao);
      } catch (Exception ex) {
        MsgBox.Show("Erro Crítico!!!\nSuas Permissões do sistema precisa ser redefinida.",
            "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
        LmException.ShowException(ex, "Erro Permissão.", true);
      }
    }

    internal void AttControls(Control control) {
      AtualizarRecursivo(control);

      ptbPaint.BackColor = LmCor.CorPrimaria;
      PtbPaint_MouseLeave(ptbPaint, EventArgs.Empty);
      ptbPaint.Refresh();

      UcPainelTarefas.Instancia.Refresh();
      UcPainelTarefas.Instancia.Invalidate();
    }

    private void AtualizarRecursivo(Control control) {
      foreach (var item in control.Controls) {
        if (item is LmMenuItem menuItem) {
          menuItem.AplicarStilo();
          menuItem.Refresh();
        }

        if (item is LmButton btn) {
          btn.AplicarStilo();
          btn.Refresh();
        }

        if (control.Controls.Count > 0)
          AtualizarRecursivo((Control)item);
      }
    }

    internal void Carregarrodape(string version, string user) {
      lblVersao.Text = version;
      lblUsuario.Text = user;
    }

    internal void MostrarControles() {
      msMenu.Visible = stpRodape.Visible = /*ptbPaint.Visible =*/ true;
    }

    private void OcultarControles() {
      msMenu.Visible = stpRodape.Visible = ptbPaint.Visible = false;
    }

    private void PtbPaint_MouseEnter(object sender, EventArgs e) {
      ptbPaint.Image = LmCor.CorPrimaria.IsDarkColor() 
        ? ptbPaint.Image.ApplyColor(Color.White.Escurecer(66) )
        : ptbPaint.Image.ApplyColor(Color.Black.Clarear(66));
    }

    private void PtbPaint_MouseLeave(object sender, EventArgs e) {
      ptbPaint.Image = LmCor.CorPrimaria.IsDarkColor() 
        ? ptbPaint.Image.ApplyColor(Color.White)
        : ptbPaint.Image.ApplyColor(Color.Black);
    }

    private void PtbPaint_Click(object sender, EventArgs e) {
      ColorDialog cld = new ColorDialog {
        FullOpen = true,
        Color = LmCor.CorPrimaria,
      };

      if (cld.ShowDialog() == DialogResult.OK) {

        // MsgBox.ShowWaitMessage("Aplicando Tema, Aguarde...");
        LmCor.CorPrimaria = cld.Color;
        // ValorPredefinido.model.CorPrimaria = cld.Color.ColorToString();

        AttControls(this);

        Toast.Success("Tema novo aplicado!");
        MsgBox.CloseWaitMessage();
      }
    }
  }
}
