using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Aplica Projetista/Desenhista e datas nas propriedades dos arquivos (substitui o FrmFileProperties).</summary>
  public partial class ViewFileProperties : Wpf.LmView {
    /// <summary>Valores da tela lidos antes de atualizar (a atualização roda fora da thread da tela).</summary>
    sealed class Dados {
      public string Projetista, DataProjeto, Desenhista, DataDesenho, NomeInicio;
      public bool PorPasta, PorNome;
    }

    string pastaProjeto = "";
    string nameShort = "";
    List<string> lista = new List<string>();

    public ViewFileProperties() {
      InitializeComponent();

      Loaded += (s, e) => CarregarUsuarios();

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Propriedades Personalizadas",
        "Esta tela grava, de uma vez, quem projetou e quem desenhou — e as datas — nas propriedades dos arquivos do SolidWorks.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Aguardar("1. Abra a peça ou a montagem",
        "No SolidWorks, abra (e deixe ativa) a peça ou a montagem que vai receber as propriedades.\n\n" +
        "Regra: com uma PEÇA ativa, só ela é atualizada. Com uma MONTAGEM ativa, são atualizados a montagem e os componentes dela que passarem no filtro. " +
        "Com um desenho ativo, nada é aplicado.",
        ViewDesenho.TemModeloAtivo);

      yield return Wpf.LmPassoAssistente.Info("2. Projetista e data do projeto",
        "Escolha quem projetou e informe a data do projeto.\n\n" +
        "Regras: a lista traz os usuários ativos e já vem com o usuário logado. " +
        "Os valores são gravados nas propriedades \"Projetista\" e \"Data do Projeto\".",
        cmbProjetista, dtpDataProjeto);

      yield return Wpf.LmPassoAssistente.Info("3. Desenhista e data do desenho",
        "Escolha quem fez o desenho e informe a data.\n\n" +
        "Regra: os valores são gravados nas propriedades \"Desenhista\" e \"Data do Desenho\".",
        cmbDesenhista, dtpDataDesenho);

      yield return Wpf.LmPassoAssistente.Info("4. Quais componentes atualizar",
        "Vale só quando uma montagem está ativa. Escolha o filtro dos componentes:\n\n" +
        "• Na Mesma Pasta da Montagem: atualiza os componentes salvos na pasta da montagem (incluindo as subpastas). Peças de outras pastas, como biblioteca, ficam de fora.\n\n" +
        "• Nome Começa Com: atualiza só os componentes cujo nome de arquivo começa com o texto informado.\n\n" +
        "Regra: componentes suprimidos ou excluídos da lista de materiais são ignorados, junto com os filhos deles.",
        grpAtualizar);

      yield return Wpf.LmPassoAssistente.Info("5. Aplicar",
        "Grava as quatro propriedades e SALVA cada arquivo atualizado. Em montagens grandes pode demorar; o andamento aparece na tela e pode ser cancelado.\n\n" +
        "Regras: o valor que já existia na propriedade é substituído. Cada componente é atualizado uma única vez, mesmo que apareça várias vezes na montagem.",
        btnAplicar);

      yield return Wpf.LmPassoAssistente.Info("Pronto!",
        "Depois de aplicar, o projetista, o desenhista e as datas ficam gravados em cada arquivo atualizado.\n\n" +
        "Para rever este passo a passo, clique no \"?\" no topo da tela.");
    }

    #endregion

    private void CarregarUsuarios() {
      try {
        if (cmbProjetista.ItemsSource != null)
          return;

        var dados = usuarios.Selecionar(ativo: true);

        cmbProjetista.ItemsSource = dados;
        cmbDesenhista.ItemsSource = dados;
        cmbProjetista.SelectedValue = usuario_alocados.model.usuario.id;
        cmbDesenhista.SelectedValue = usuario_alocados.model.usuario.id;
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao carregar usuários");
      }
    }

    private Dados LerTela() => new Dados {
      Projetista = cmbProjetista.Text,
      DataProjeto = dtpDataProjeto.Text,
      Desenhista = cmbDesenhista.Text,
      DataDesenho = dtpDataDesenho.Text,
      NomeInicio = txtNome.Text,
      PorPasta = rdbPasta.IsChecked == true,
      PorNome = rdbNome.IsChecked == true,
    };

    private async void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      try {
        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        var swModel = (ModelDoc2)Sw.App.ActiveDoc;
        var dados = LerTela();

        string pathName = swModel.GetPathName();

        pastaProjeto = Path.GetDirectoryName(pathName);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          using (var worker = new LmBackgroundWorker()) {
            await worker.RunAsync("Iniciando atualização Componentes...", async (progress, token) => {
              AddPropriedades(swModel, dados);
              AtualizarPropriedades(dados, progress, token);

              progress.Report(("Concluído!", 0, 0));
            });
          }

          lista.Clear();

          if (dados.PorPasta)
            Wpf.MsgBox.Show($"Os componentes que estão na pasta\n\n{pastaProjeto}\n\nForam Atualizados com Sucesso.", "Sucesso",
                MessageBoxButton.OK, MessageBoxImage.Information);
          else if (dados.PorNome)
            Wpf.MsgBox.Show($"Os componentes que o nome começa com\n\n{dados.NomeInicio}\n\nForam Atualizados com Sucesso.", "Sucesso",
                MessageBoxButton.OK, MessageBoxImage.Information);
        } else if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART) {
          AddPropriedades(swModel, dados);
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao atualizar componentes");
      }
    }

    private void AtualizarPropriedades(Dados dados, IProgress<(string message, int current, int max)> progress, CancellationToken token) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        ConfigurationManager swConfMgr;
        Configuration swConf;
        Component2 swRootComp;

        swConfMgr = swModel.ConfigurationManager;
        swConf = swConfMgr.ActiveConfiguration;
        swRootComp = swConf.GetRootComponent3(true);

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          TraverseComponent(swRootComp, 1, dados, progress, token);
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao atualizar propriedades");
      }
    }

    private void TraverseComponent(Component2 swComp, long nLevel, Dados dados, IProgress<(string message, int current, int max)> progress, CancellationToken token) {
      try {
        object[] vChildComp;

        Component2 swChildComp;

        vChildComp = (object[])swComp.GetChildren();

        for (int i = 0; i < vChildComp.Length; i++) {
          if (token.IsCancellationRequested) {
            Wpf.Toast.Info("Operação cancelada pelo usuário.");
            return;
          }

          swChildComp = (Component2)vChildComp[i];
          bool supress = swChildComp.IsSuppressed();
          bool exclude = swChildComp.ExcludeFromBOM;
          string refConfig = swChildComp.ReferencedConfiguration;

          var swModel = (ModelDoc2)swChildComp.GetModelDoc2();
          if (swModel == null) continue;

          string pathName = swModel.GetPathName();

          nameShort = Path.GetFileNameWithoutExtension(pathName);
          progress.Report(($"Adicionando propriedades.\r\n\r\n{nLevel} - {nameShort} - {pathName}", 0, 0));
          bool readOnly = swModel.IsOpenedReadOnly();

          if (supress == false && exclude == false) {
            bool canAdd = true;

            foreach (string s in lista) {
              if (s == nameShort) {
                canAdd = false;
                break;
              }
            }

            if (canAdd == true) {
              if (dados.PorPasta && pathName.Contains(pastaProjeto)) {
                AddPropriedades(swModel, dados);
                lista.Add(nameShort);
              } else if (dados.PorNome && nameShort.StartsWith(dados.NomeInicio.Trim())) {
                AddPropriedades(swModel, dados);
                lista.Add(nameShort);
              }
            }

            TraverseComponent(swChildComp, nLevel + 1, dados, progress, token);
          }
        }
      } catch (Exception ex) {
        Msg.Erro(ex, $"Erro ao carregar componente [{nameShort}]");
      }
    }

    private void AddPropriedades(ModelDoc2 swModel, Dados dados) {
      try {
        var swModelDocExt = swModel.Extension;
        var swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

        swCustPropMgr.Add3("Projetista", (int)swCustomInfoType_e.swCustomInfoText, dados.Projetista, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        swCustPropMgr.Add3("Data do Projeto", (int)swCustomInfoType_e.swCustomInfoDate, dados.DataProjeto, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        swCustPropMgr.Add3("Desenhista", (int)swCustomInfoType_e.swCustomInfoText, dados.Desenhista, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
        swCustPropMgr.Add3("Data do Desenho", (int)swCustomInfoType_e.swCustomInfoDate, dados.DataDesenho, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        swModel.Save();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao adicionar propriedade");
      }
    }
  }
}
