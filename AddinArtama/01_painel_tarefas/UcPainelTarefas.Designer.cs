namespace AddinArtama {
  partial class UcPainelTarefas {
    /// <summary> 
    /// Variável de designer necessária.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Limpar os recursos que estão sendo usados.
    /// </summary>
    /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Código gerado pelo Designer de Componentes

    /// <summary> 
    /// Método necessário para suporte ao Designer - não modifique 
    /// o conteúdo deste método com o editor de código.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcPainelTarefas));
      this.msMenu = new LmCorbieUI.Controls.LmPanel();
      this.msCSW = new LmCorbieUI.Controls.LmMenuItem();
      this.msConfig = new LmCorbieUI.Controls.LmMenuItem();
      this.msRelatorio = new LmCorbieUI.Controls.LmMenuItem();
      this.msCadastro = new LmCorbieUI.Controls.LmMenuItem();
      this.msImprimir = new LmCorbieUI.Controls.LmMenuItem();
      this.msDesenho = new LmCorbieUI.Controls.LmMenuItem();
      this.msProperties = new LmCorbieUI.Controls.LmMenuItem();
      this.msProcess = new LmCorbieUI.Controls.LmMenuItem();
      this.msLogout = new LmCorbieUI.Controls.LmMenuItem();
      this.cmxToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.stpRodape = new LmCorbieUI.Controls.LmStatusStrip();
      this.lblVersao = new System.Windows.Forms.ToolStripStatusLabel();
      this.lblUsuario = new System.Windows.Forms.ToolStripStatusLabel();
      this.msMenuDesenho = new LmCorbieUI.Controls.LmDropdownMenu(this.components);
      this.msCriarDesenho = new System.Windows.Forms.ToolStripMenuItem();
      this.msAtualizarDesenho = new System.Windows.Forms.ToolStripMenuItem();
      this.msMenuExportar = new LmCorbieUI.Controls.LmDropdownMenu(this.components);
      this.msExportarPDF = new System.Windows.Forms.ToolStripMenuItem();
      this.msExportarDXF = new System.Windows.Forms.ToolStripMenuItem();
      this.msMenuRelatorio = new LmCorbieUI.Controls.LmDropdownMenu(this.components);
      this.msProcessoFabricacao = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
      this.msPackList = new System.Windows.Forms.ToolStripMenuItem();
      this.msPlanoPintura = new System.Windows.Forms.ToolStripMenuItem();
      this.msManutPacklistPlanoDePintura = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.msReportWorks = new System.Windows.Forms.ToolStripMenuItem();
      this.pnlMain = new LmCorbieUI.Controls.LmPanel();
      this.msMenuCadastro = new LmCorbieUI.Controls.LmDropdownMenu(this.components);
      this.msUsuarioCad = new System.Windows.Forms.ToolStripMenuItem();
      this.msPerfilCad = new System.Windows.Forms.ToolStripMenuItem();
      this.msMaterialCad = new System.Windows.Forms.ToolStripMenuItem();
      this.msMateriaPrimaCad = new System.Windows.Forms.ToolStripMenuItem();
      this.msRedefinirSenha = new System.Windows.Forms.ToolStripMenuItem();
      this.msMenuCSW = new LmCorbieUI.Controls.LmDropdownMenu(this.components);
      this.msCadastroProduto = new System.Windows.Forms.ToolStripMenuItem();
      this.msCadastroEngenhariaProduto = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.msConfiguracaoIntegracao = new System.Windows.Forms.ToolStripMenuItem();
      this.msMenu.SuspendLayout();
      this.stpRodape.SuspendLayout();
      this.msMenuDesenho.SuspendLayout();
      this.msMenuExportar.SuspendLayout();
      this.msMenuRelatorio.SuspendLayout();
      this.msMenuCadastro.SuspendLayout();
      this.msMenuCSW.SuspendLayout();
      this.SuspendLayout();
      // 
      // msMenu
      // 
      this.msMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(54)))), ((int)(((byte)(71)))));
      this.msMenu.Controls.Add(this.msCSW);
      this.msMenu.Controls.Add(this.msConfig);
      this.msMenu.Controls.Add(this.msRelatorio);
      this.msMenu.Controls.Add(this.msCadastro);
      this.msMenu.Controls.Add(this.msImprimir);
      this.msMenu.Controls.Add(this.msDesenho);
      this.msMenu.Controls.Add(this.msProperties);
      this.msMenu.Controls.Add(this.msProcess);
      this.msMenu.Controls.Add(this.msLogout);
      this.msMenu.Dock = System.Windows.Forms.DockStyle.Top;
      this.msMenu.IsPanelMenu = true;
      this.msMenu.Location = new System.Drawing.Point(0, 0);
      this.msMenu.Name = "msMenu";
      this.msMenu.Size = new System.Drawing.Size(317, 30);
      this.msMenu.TabIndex = 0;
      // 
      // msCSW
      // 
      this.msCSW.Dock = System.Windows.Forms.DockStyle.Left;
      this.msCSW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.msCSW.Image = ((System.Drawing.Image)(resources.GetObject("msCSW.Image")));
      this.msCSW.Location = new System.Drawing.Point(180, 0);
      this.msCSW.Name = "msCSW";
      this.msCSW.Size = new System.Drawing.Size(30, 30);
      this.msCSW.TabIndex = 13;
      this.msCSW.TabStop = false;
      this.msCSW.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.msCSW, "Integração ERP");
      this.msCSW.UseSelectable = true;
      this.msCSW.UseVisualStyleBackColor = false;
      this.msCSW.Click += new System.EventHandler(this.MsCSW_Click);
      // 
      // msConfig
      // 
      this.msConfig.Dock = System.Windows.Forms.DockStyle.Right;
      this.msConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.msConfig.Image = ((System.Drawing.Image)(resources.GetObject("msConfig.Image")));
      this.msConfig.Location = new System.Drawing.Point(257, 0);
      this.msConfig.Name = "msConfig";
      this.msConfig.Size = new System.Drawing.Size(30, 30);
      this.msConfig.TabIndex = 11;
      this.msConfig.TabStop = false;
      this.msConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.msConfig, "Configuração");
      this.msConfig.UseSelectable = true;
      this.msConfig.UseVisualStyleBackColor = false;
      this.msConfig.Click += new System.EventHandler(this.MsConfig_Click);
      // 
      // msRelatorio
      // 
      this.msRelatorio.Dock = System.Windows.Forms.DockStyle.Left;
      this.msRelatorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.msRelatorio.Image = ((System.Drawing.Image)(resources.GetObject("msRelatorio.Image")));
      this.msRelatorio.Location = new System.Drawing.Point(150, 0);
      this.msRelatorio.Name = "msRelatorio";
      this.msRelatorio.Size = new System.Drawing.Size(30, 30);
      this.msRelatorio.TabIndex = 10;
      this.msRelatorio.TabStop = false;
      this.msRelatorio.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.msRelatorio, "Relatórios");
      this.msRelatorio.UseSelectable = true;
      this.msRelatorio.UseVisualStyleBackColor = false;
      this.msRelatorio.Click += new System.EventHandler(this.MsRelatorio_Click);
      // 
      // msCadastro
      // 
      this.msCadastro.Dock = System.Windows.Forms.DockStyle.Left;
      this.msCadastro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.msCadastro.Image = ((System.Drawing.Image)(resources.GetObject("msCadastro.Image")));
      this.msCadastro.Location = new System.Drawing.Point(120, 0);
      this.msCadastro.Name = "msCadastro";
      this.msCadastro.Size = new System.Drawing.Size(30, 30);
      this.msCadastro.TabIndex = 9;
      this.msCadastro.TabStop = false;
      this.msCadastro.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.msCadastro, "Cadastros Gerais");
      this.msCadastro.UseSelectable = true;
      this.msCadastro.UseVisualStyleBackColor = false;
      this.msCadastro.Click += new System.EventHandler(this.MsCadastro_Click);
      // 
      // msImprimir
      // 
      this.msImprimir.Dock = System.Windows.Forms.DockStyle.Left;
      this.msImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.msImprimir.Image = ((System.Drawing.Image)(resources.GetObject("msImprimir.Image")));
      this.msImprimir.Location = new System.Drawing.Point(90, 0);
      this.msImprimir.Name = "msImprimir";
      this.msImprimir.Size = new System.Drawing.Size(30, 30);
      this.msImprimir.TabIndex = 5;
      this.msImprimir.TabStop = false;
      this.msImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.msImprimir, "Gerar PDFs");
      this.msImprimir.UseSelectable = true;
      this.msImprimir.UseVisualStyleBackColor = false;
      this.msImprimir.Click += new System.EventHandler(this.MsMenuExportar_Click);
      // 
      // msDesenho
      // 
      this.msDesenho.Dock = System.Windows.Forms.DockStyle.Left;
      this.msDesenho.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.msDesenho.Image = ((System.Drawing.Image)(resources.GetObject("msDesenho.Image")));
      this.msDesenho.Location = new System.Drawing.Point(60, 0);
      this.msDesenho.Name = "msDesenho";
      this.msDesenho.Size = new System.Drawing.Size(30, 30);
      this.msDesenho.TabIndex = 4;
      this.msDesenho.TabStop = false;
      this.msDesenho.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.msDesenho, "Desenhos");
      this.msDesenho.UseSelectable = true;
      this.msDesenho.UseVisualStyleBackColor = false;
      this.msDesenho.Click += new System.EventHandler(this.MsDesenho_Click);
      // 
      // msProperties
      // 
      this.msProperties.Dock = System.Windows.Forms.DockStyle.Left;
      this.msProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.msProperties.Image = ((System.Drawing.Image)(resources.GetObject("msProperties.Image")));
      this.msProperties.Location = new System.Drawing.Point(30, 0);
      this.msProperties.Name = "msProperties";
      this.msProperties.Size = new System.Drawing.Size(30, 30);
      this.msProperties.TabIndex = 3;
      this.msProperties.TabStop = false;
      this.msProperties.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.msProperties, "Propriedades Personalizadas");
      this.msProperties.UseSelectable = true;
      this.msProperties.UseVisualStyleBackColor = false;
      this.msProperties.Click += new System.EventHandler(this.MsProperties_Click);
      // 
      // msProcess
      // 
      this.msProcess.Dock = System.Windows.Forms.DockStyle.Left;
      this.msProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.msProcess.Image = ((System.Drawing.Image)(resources.GetObject("msProcess.Image")));
      this.msProcess.Location = new System.Drawing.Point(0, 0);
      this.msProcess.Name = "msProcess";
      this.msProcess.Size = new System.Drawing.Size(30, 30);
      this.msProcess.TabIndex = 1;
      this.msProcess.TabStop = false;
      this.msProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.msProcess, "Definir Processos");
      this.msProcess.UseSelectable = true;
      this.msProcess.UseVisualStyleBackColor = false;
      this.msProcess.Click += new System.EventHandler(this.MsProcess_Click);
      // 
      // msLogout
      // 
      this.msLogout.Dock = System.Windows.Forms.DockStyle.Right;
      this.msLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.msLogout.Image = ((System.Drawing.Image)(resources.GetObject("msLogout.Image")));
      this.msLogout.Location = new System.Drawing.Point(287, 0);
      this.msLogout.Name = "msLogout";
      this.msLogout.Size = new System.Drawing.Size(30, 30);
      this.msLogout.TabIndex = 12;
      this.msLogout.TabStop = false;
      this.msLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.cmxToolTip1.SetToolTip(this.msLogout, "Configurar Templates");
      this.msLogout.UseSelectable = true;
      this.msLogout.UseVisualStyleBackColor = false;
      this.msLogout.Click += new System.EventHandler(this.MsLogout_Click);
      // 
      // stpRodape
      // 
      this.stpRodape.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(54)))), ((int)(((byte)(71)))));
      this.stpRodape.Font = new System.Drawing.Font("Segoe UI", 8.25F);
      this.stpRodape.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      this.stpRodape.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblVersao,
            this.lblUsuario});
      this.stpRodape.Location = new System.Drawing.Point(0, 264);
      this.stpRodape.Name = "stpRodape";
      this.stpRodape.Size = new System.Drawing.Size(317, 22);
      this.stpRodape.SizingGrip = false;
      this.stpRodape.TabIndex = 23;
      this.stpRodape.Text = "Rodapé";
      // 
      // lblVersao
      // 
      this.lblVersao.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
      this.lblVersao.Name = "lblVersao";
      this.lblVersao.Size = new System.Drawing.Size(16, 17);
      this.lblVersao.Text = "*";
      // 
      // lblUsuario
      // 
      this.lblUsuario.Name = "lblUsuario";
      this.lblUsuario.Size = new System.Drawing.Size(12, 17);
      this.lblUsuario.Text = "*";
      // 
      // msMenuDesenho
      // 
      this.msMenuDesenho.IsMainMenu = false;
      this.msMenuDesenho.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msCriarDesenho,
            this.msAtualizarDesenho});
      this.msMenuDesenho.MenuItemHeight = 25;
      this.msMenuDesenho.MenuItemTextColor = System.Drawing.Color.Empty;
      this.msMenuDesenho.Name = "msMenuSistema";
      this.msMenuDesenho.NaoInverterCorImagem = false;
      this.msMenuDesenho.PrimaryColor = System.Drawing.Color.Empty;
      this.msMenuDesenho.Size = new System.Drawing.Size(253, 48);
      this.msMenuDesenho.Z_Teste = 0;
      // 
      // msCriarDesenho
      // 
      this.msCriarDesenho.Image = ((System.Drawing.Image)(resources.GetObject("msCriarDesenho.Image")));
      this.msCriarDesenho.Name = "msCriarDesenho";
      this.msCriarDesenho.Size = new System.Drawing.Size(252, 22);
      this.msCriarDesenho.Text = "Criar/Alterar Desenhos";
      this.msCriarDesenho.Click += new System.EventHandler(this.MsCriarDesenho_Click);
      // 
      // msAtualizarDesenho
      // 
      this.msAtualizarDesenho.Image = ((System.Drawing.Image)(resources.GetObject("msAtualizarDesenho.Image")));
      this.msAtualizarDesenho.Name = "msAtualizarDesenho";
      this.msAtualizarDesenho.Size = new System.Drawing.Size(252, 22);
      this.msAtualizarDesenho.Text = "Atualizar Templates dos Desenhos";
      this.msAtualizarDesenho.Click += new System.EventHandler(this.MsAtualizarDesenho_Click);
      // 
      // msMenuExportar
      // 
      this.msMenuExportar.IsMainMenu = false;
      this.msMenuExportar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msExportarPDF,
            this.msExportarDXF});
      this.msMenuExportar.MenuItemHeight = 25;
      this.msMenuExportar.MenuItemTextColor = System.Drawing.Color.Empty;
      this.msMenuExportar.Name = "msMenuSistema";
      this.msMenuExportar.NaoInverterCorImagem = false;
      this.msMenuExportar.PrimaryColor = System.Drawing.Color.Empty;
      this.msMenuExportar.Size = new System.Drawing.Size(175, 48);
      this.msMenuExportar.Z_Teste = 0;
      // 
      // msExportarPDF
      // 
      this.msExportarPDF.Image = ((System.Drawing.Image)(resources.GetObject("msExportarPDF.Image")));
      this.msExportarPDF.Name = "msExportarPDF";
      this.msExportarPDF.Size = new System.Drawing.Size(174, 22);
      this.msExportarPDF.Text = "Exportar PDF/DWG";
      this.msExportarPDF.Click += new System.EventHandler(this.MsExportar_Click);
      // 
      // msExportarDXF
      // 
      this.msExportarDXF.Image = ((System.Drawing.Image)(resources.GetObject("msExportarDXF.Image")));
      this.msExportarDXF.Name = "msExportarDXF";
      this.msExportarDXF.Size = new System.Drawing.Size(174, 22);
      this.msExportarDXF.Text = "Exportar DXF";
      this.msExportarDXF.Click += new System.EventHandler(this.MsExportarDXF_Click);
      // 
      // msMenuRelatorio
      // 
      this.msMenuRelatorio.IsMainMenu = false;
      this.msMenuRelatorio.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msProcessoFabricacao,
            this.toolStripMenuItem4,
            this.msPackList,
            this.msPlanoPintura,
            this.msManutPacklistPlanoDePintura,
            this.toolStripMenuItem1,
            this.msReportWorks});
      this.msMenuRelatorio.MenuItemHeight = 25;
      this.msMenuRelatorio.MenuItemTextColor = System.Drawing.Color.Empty;
      this.msMenuRelatorio.Name = "msMenuSistema";
      this.msMenuRelatorio.NaoInverterCorImagem = false;
      this.msMenuRelatorio.PrimaryColor = System.Drawing.Color.Empty;
      this.msMenuRelatorio.Size = new System.Drawing.Size(284, 126);
      this.msMenuRelatorio.Z_Teste = 0;
      // 
      // msProcessoFabricacao
      // 
      this.msProcessoFabricacao.Image = ((System.Drawing.Image)(resources.GetObject("msProcessoFabricacao.Image")));
      this.msProcessoFabricacao.Name = "msProcessoFabricacao";
      this.msProcessoFabricacao.Size = new System.Drawing.Size(283, 22);
      this.msProcessoFabricacao.Text = "Processo de Fabricação";
      this.msProcessoFabricacao.Click += new System.EventHandler(this.MsProcessoFabricacao_Click);
      // 
      // toolStripMenuItem4
      // 
      this.toolStripMenuItem4.Name = "toolStripMenuItem4";
      this.toolStripMenuItem4.Size = new System.Drawing.Size(280, 6);
      // 
      // msPackList
      // 
      this.msPackList.Image = ((System.Drawing.Image)(resources.GetObject("msPackList.Image")));
      this.msPackList.Name = "msPackList";
      this.msPackList.Size = new System.Drawing.Size(283, 22);
      this.msPackList.Text = "Pack List";
      this.msPackList.Click += new System.EventHandler(this.MsPackList_Click);
      // 
      // msPlanoPintura
      // 
      this.msPlanoPintura.Image = ((System.Drawing.Image)(resources.GetObject("msPlanoPintura.Image")));
      this.msPlanoPintura.Name = "msPlanoPintura";
      this.msPlanoPintura.Size = new System.Drawing.Size(283, 22);
      this.msPlanoPintura.Text = "Plano de Pintura";
      this.msPlanoPintura.Click += new System.EventHandler(this.MsPlanoPintura_Click);
      // 
      // msManutPacklistPlanoDePintura
      // 
      this.msManutPacklistPlanoDePintura.Image = ((System.Drawing.Image)(resources.GetObject("msManutPacklistPlanoDePintura.Image")));
      this.msManutPacklistPlanoDePintura.Name = "msManutPacklistPlanoDePintura";
      this.msManutPacklistPlanoDePintura.Size = new System.Drawing.Size(283, 22);
      this.msManutPacklistPlanoDePintura.Text = "Manutenção Packlist e Plano de Pintura";
      this.msManutPacklistPlanoDePintura.Click += new System.EventHandler(this.MsManutPacklistPlanoDePintura_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(280, 6);
      // 
      // msReportWorks
      // 
      this.msReportWorks.Image = ((System.Drawing.Image)(resources.GetObject("msReportWorks.Image")));
      this.msReportWorks.Name = "msReportWorks";
      this.msReportWorks.Size = new System.Drawing.Size(283, 22);
      this.msReportWorks.Text = "Report Works";
      this.msReportWorks.Click += new System.EventHandler(this.MsReportWorks_Click);
      // 
      // pnlMain
      // 
      this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(233)))));
      this.pnlMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlMain.IsPanelMenu = false;
      this.pnlMain.Location = new System.Drawing.Point(0, 30);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new System.Drawing.Size(317, 234);
      this.pnlMain.TabIndex = 24;
      // 
      // msMenuCadastro
      // 
      this.msMenuCadastro.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.msMenuCadastro.IsMainMenu = false;
      this.msMenuCadastro.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msUsuarioCad,
            this.msPerfilCad,
            this.msMaterialCad,
            this.msMateriaPrimaCad,
            this.msRedefinirSenha});
      this.msMenuCadastro.MenuItemHeight = 25;
      this.msMenuCadastro.MenuItemTextColor = System.Drawing.Color.Empty;
      this.msMenuCadastro.Name = "msMenuSistema";
      this.msMenuCadastro.NaoInverterCorImagem = false;
      this.msMenuCadastro.PrimaryColor = System.Drawing.Color.Empty;
      this.msMenuCadastro.Size = new System.Drawing.Size(232, 134);
      this.msMenuCadastro.Z_Teste = 0;
      // 
      // msUsuarioCad
      // 
      this.msUsuarioCad.Image = ((System.Drawing.Image)(resources.GetObject("msUsuarioCad.Image")));
      this.msUsuarioCad.Name = "msUsuarioCad";
      this.msUsuarioCad.Size = new System.Drawing.Size(231, 26);
      this.msUsuarioCad.Text = "Cadastro de Usuário";
      this.msUsuarioCad.Click += new System.EventHandler(this.MsUsuarioCad_Click);
      // 
      // msPerfilCad
      // 
      this.msPerfilCad.Image = ((System.Drawing.Image)(resources.GetObject("msPerfilCad.Image")));
      this.msPerfilCad.Name = "msPerfilCad";
      this.msPerfilCad.Size = new System.Drawing.Size(231, 26);
      this.msPerfilCad.Text = "Cadastro de Perfil do Usuário";
      this.msPerfilCad.Click += new System.EventHandler(this.MsPerfilCad_Click);
      // 
      // msMaterialCad
      // 
      this.msMaterialCad.Image = ((System.Drawing.Image)(resources.GetObject("msMaterialCad.Image")));
      this.msMaterialCad.Name = "msMaterialCad";
      this.msMaterialCad.Size = new System.Drawing.Size(231, 26);
      this.msMaterialCad.Text = "Cadastro de Material";
      this.msMaterialCad.Click += new System.EventHandler(this.MsMaterialCad_Click);
      // 
      // msMateriaPrimaCad
      // 
      this.msMateriaPrimaCad.Image = ((System.Drawing.Image)(resources.GetObject("msMateriaPrimaCad.Image")));
      this.msMateriaPrimaCad.Name = "msMateriaPrimaCad";
      this.msMateriaPrimaCad.Size = new System.Drawing.Size(231, 26);
      this.msMateriaPrimaCad.Text = "Cadastro de Matéria Prima";
      this.msMateriaPrimaCad.Click += new System.EventHandler(this.MsMateriaPrimaCad_Click);
      // 
      // msRedefinirSenha
      // 
      this.msRedefinirSenha.Image = ((System.Drawing.Image)(resources.GetObject("msRedefinirSenha.Image")));
      this.msRedefinirSenha.Name = "msRedefinirSenha";
      this.msRedefinirSenha.Size = new System.Drawing.Size(231, 26);
      this.msRedefinirSenha.Text = "Redefinir Senha";
      this.msRedefinirSenha.Click += new System.EventHandler(this.MsRedefinirSenha_Click);
      // 
      // msMenuCSW
      // 
      this.msMenuCSW.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.msMenuCSW.IsMainMenu = false;
      this.msMenuCSW.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msCadastroProduto,
            this.msCadastroEngenhariaProduto,
            this.toolStripMenuItem2,
            this.msConfiguracaoIntegracao});
      this.msMenuCSW.MenuItemHeight = 25;
      this.msMenuCSW.MenuItemTextColor = System.Drawing.Color.Empty;
      this.msMenuCSW.Name = "msMenuSistema";
      this.msMenuCSW.NaoInverterCorImagem = false;
      this.msMenuCSW.PrimaryColor = System.Drawing.Color.Empty;
      this.msMenuCSW.Size = new System.Drawing.Size(266, 88);
      this.msMenuCSW.Z_Teste = 0;
      // 
      // msCadastroProduto
      // 
      this.msCadastroProduto.Image = ((System.Drawing.Image)(resources.GetObject("msCadastroProduto.Image")));
      this.msCadastroProduto.Name = "msCadastroProduto";
      this.msCadastroProduto.Size = new System.Drawing.Size(265, 26);
      this.msCadastroProduto.Text = "Cadastro de Produto";
      this.msCadastroProduto.Click += new System.EventHandler(this.MsCadastroProduto_Click);
      // 
      // msCadastroEngenhariaProduto
      // 
      this.msCadastroEngenhariaProduto.Image = ((System.Drawing.Image)(resources.GetObject("msCadastroEngenhariaProduto.Image")));
      this.msCadastroEngenhariaProduto.Name = "msCadastroEngenhariaProduto";
      this.msCadastroEngenhariaProduto.Size = new System.Drawing.Size(265, 26);
      this.msCadastroEngenhariaProduto.Text = "Cadastro de Engenharia de Produto";
      this.msCadastroEngenhariaProduto.Click += new System.EventHandler(this.MsCadastroEngenhariaProduto_Click);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(262, 6);
      // 
      // msConfiguracaoIntegracao
      // 
      this.msConfiguracaoIntegracao.Image = ((System.Drawing.Image)(resources.GetObject("msConfiguracaoIntegracao.Image")));
      this.msConfiguracaoIntegracao.Name = "msConfiguracaoIntegracao";
      this.msConfiguracaoIntegracao.Size = new System.Drawing.Size(265, 26);
      this.msConfiguracaoIntegracao.Text = "Configuração de Integração";
      this.msConfiguracaoIntegracao.Click += new System.EventHandler(this.MsConfiguracaoIntegracao_Click);
      // 
      // UcPainelTarefas
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.pnlMain);
      this.Controls.Add(this.stpRodape);
      this.Controls.Add(this.msMenu);
      this.Name = "UcPainelTarefas";
      this.Size = new System.Drawing.Size(317, 286);
      this.Load += new System.EventHandler(this.UcPainelTarefas_Load);
      this.msMenu.ResumeLayout(false);
      this.stpRodape.ResumeLayout(false);
      this.stpRodape.PerformLayout();
      this.msMenuDesenho.ResumeLayout(false);
      this.msMenuExportar.ResumeLayout(false);
      this.msMenuRelatorio.ResumeLayout(false);
      this.msMenuCadastro.ResumeLayout(false);
      this.msMenuCSW.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.ToolTip cmxToolTip1;
    private System.Windows.Forms.ToolStripStatusLabel lblVersao;
    private System.Windows.Forms.ToolStripStatusLabel lblUsuario;
    private LmCorbieUI.Controls.LmDropdownMenu msMenuDesenho;
    private System.Windows.Forms.ToolStripMenuItem msCriarDesenho;
    private System.Windows.Forms.ToolStripMenuItem msAtualizarDesenho;
    private LmCorbieUI.Controls.LmDropdownMenu msMenuExportar;
    private System.Windows.Forms.ToolStripMenuItem msExportarPDF;
    private System.Windows.Forms.ToolStripMenuItem msExportarDXF;
    private LmCorbieUI.Controls.LmDropdownMenu msMenuRelatorio;
    private System.Windows.Forms.ToolStripMenuItem msProcessoFabricacao;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    private System.Windows.Forms.ToolStripMenuItem msPackList;
    private System.Windows.Forms.ToolStripMenuItem msPlanoPintura;
    private System.Windows.Forms.ToolStripMenuItem msManutPacklistPlanoDePintura;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem msReportWorks;
    private LmCorbieUI.Controls.LmMenuItem msProcess;
    private LmCorbieUI.Controls.LmMenuItem msConfig;
    private LmCorbieUI.Controls.LmMenuItem msRelatorio;
    private LmCorbieUI.Controls.LmMenuItem msCadastro;
    private LmCorbieUI.Controls.LmMenuItem msImprimir;
    private LmCorbieUI.Controls.LmMenuItem msDesenho;
    private LmCorbieUI.Controls.LmMenuItem msProperties;
    private LmCorbieUI.Controls.LmMenuItem msLogout;
    private LmCorbieUI.Controls.LmDropdownMenu msMenuCadastro;
    private System.Windows.Forms.ToolStripMenuItem msUsuarioCad;
    private System.Windows.Forms.ToolStripMenuItem msPerfilCad;
    private System.Windows.Forms.ToolStripMenuItem msMateriaPrimaCad;
    private System.Windows.Forms.ToolStripMenuItem msMaterialCad;
    private System.Windows.Forms.ToolStripMenuItem msRedefinirSenha;
    internal LmCorbieUI.Controls.LmPanel msMenu;
    internal LmCorbieUI.Controls.LmStatusStrip stpRodape;
    internal LmCorbieUI.Controls.LmPanel pnlMain;
    private LmCorbieUI.Controls.LmMenuItem msCSW;
    private LmCorbieUI.Controls.LmDropdownMenu msMenuCSW;
    private System.Windows.Forms.ToolStripMenuItem msCadastroProduto;
    private System.Windows.Forms.ToolStripMenuItem msCadastroEngenhariaProduto;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem msConfiguracaoIntegracao;
  }
}
