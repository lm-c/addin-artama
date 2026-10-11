using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Remove as propriedades PINTURA e CHECK dos componentes da montagem (substitui o FrmManutProps).</summary>
  public partial class ViewManutProps : Wpf.LmView {
    public ViewManutProps() {
      InitializeComponent();

      DefinirAssistente(RoteiroAssistente);
    }

    #region Assistente (botão "?")

    /// <summary>Passo a passo da tela, com a regra de negócio de cada ação.</summary>
    private IEnumerable<Wpf.LmPassoAssistente> RoteiroAssistente() {
      yield return Wpf.LmPassoAssistente.Info("Manutenção Packlist e Plano de Pintura",
        "Esta tela faz uma limpeza: apaga, de todos os componentes de uma montagem, as propriedades \"PINTURA\" e \"CHECK\" — " +
        "as mesmas que o Plano de Pintura e o Pack List usam para separar os itens em volumes.\n\n" +
        "Use quando quiser refazer essa separação do zero.\n\n" +
        "Vou mostrar o caminho passo a passo. Você pode sair a qualquer momento com o X ou a tecla Esc.");

      yield return Wpf.LmPassoAssistente.Aguardar("1. Abra a montagem",
        "No SolidWorks, abra (e deixe ativa) a montagem do projeto.\n\n" +
        "Regra: esta tela só funciona com uma MONTAGEM ativa.",
        ViewFormatosAtualizar.TemMontagemAtiva);

      yield return Wpf.LmPassoAssistente.Info("2. O que será removido",
        "As propriedades ficam na aba \"Específico da configuração\" das Propriedades do arquivo de cada componente.",
        lblInfo);

      yield return Wpf.LmPassoAssistente.Info("3. Iniciar a manutenção",
        "Percorre todos os componentes da montagem, remove as duas propriedades e SALVA cada arquivo.\n\n" +
        "Regras:\n" +
        "• Não há como desfazer: depois, os valores precisam ser preenchidos de novo.\n" +
        "• Componentes suprimidos ou excluídos da lista de materiais são ignorados, junto com os filhos deles.\n" +
        "• Cada componente é tratado uma única vez, mesmo que apareça várias vezes na montagem.\n\n" +
        "Ao terminar, aparece a mensagem de conclusão.",
        btnIniciar);
    }

    #endregion

    private async void BtnIniciar_Click(object sender, RoutedEventArgs e) {
      try {
        var swModel = (ModelDoc2)Sw.App.ActiveDoc;

        if (Sw.App.ActiveDoc == null) {
          Wpf.Toast.Info($"Sem documentos abertos");
          return;
        }

        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) {
          ConfigurationManager swConfMgr;
          Configuration swConf;
          Component2 swRootComp;

          swConfMgr = swModel.ConfigurationManager;
          swConf = swConfMgr.ActiveConfiguration;
          swRootComp = swConf.GetRootComponent3(true);

          var nomesVisitados = new List<string>();

          using (var worker = new LmBackgroundWorker()) {
            await worker.RunAsync("Efetuando Manutenção. Aguarde......", (progress, token) => {
              progress.Report(("Efetuando Manutenção. Aguarde......", 0, 0));
              TraverseComponent(swRootComp, nomesVisitados, 1);
              return Task.CompletedTask;
            });
          }

          Wpf.MsgBox.Show("Concluido com Sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        } else {
          Wpf.MsgBox.Show($"Comando apenas para Montagens", "Addin LM Projetos", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao carregar componentes");
      }
    }

    private void TraverseComponent(Component2 swComp, List<string> nomesVisitados, long nLevel) {
      string nameShort = "";
      try {
        object[] vChildComp;

        Component2 swChildComp;

        vChildComp = (object[])swComp.GetChildren();

        for (int i = 0; i < vChildComp.Length; i++) {
          swChildComp = (Component2)vChildComp[i];
          bool supress = swChildComp.IsSuppressed();
          bool exclude = swChildComp.ExcludeFromBOM;
          string refConfig = swChildComp.ReferencedConfiguration;

          var swModel = (ModelDoc2)swChildComp.GetModelDoc2();
          if (swModel == null) continue;

          string pathName = swModel.GetPathName();

          nameShort = Path.GetFileNameWithoutExtension(pathName).ToUpper();
          if (!nomesVisitados.Contains(nameShort)) {
            if (supress == false && exclude == false) {
              ModelDocExtension swModelDocExt = default(ModelDocExtension);
              swModelDocExt = swModel.Extension;
              var swCustPropMgr = swModelDocExt.get_CustomPropertyManager(refConfig);
              swCustPropMgr.Delete2("PINTURA");
              swCustPropMgr.Delete2("CHECK");
              swModel.Save();

              nomesVisitados.Add(nameShort);

              TraverseComponent(swChildComp, nomesVisitados, nLevel + 1);
            } else {
              nomesVisitados.Add(nameShort);
            }
          }
        }
      } catch (Exception ex) {
        Msg.Erro(ex, $"Erro ao carregar componente. + [{nameShort}]");
      }
    }

    private void PegaDadosLista(BomTableAnnotation swBOMAnnotation) {
      try {
        string[] vModelPathNames = null;
        string strItemNumber = null;
        string strPartNumber = null;
        var swTableAnnotation = (TableAnnotation)swBOMAnnotation;
        var comps = new List<string>();

        int lStartRow = 1;

        if (!(swTableAnnotation.TitleVisible == false)) {
          lStartRow = 2;
        }

        var swBOMFeature = swBOMAnnotation.BomFeature;
        var totalRowsCount = swTableAnnotation.TotalRowCount;
        ModelDoc2 swModel = default(ModelDoc2);

        for (int i = lStartRow; i < totalRowsCount; i++) {
          double perc = (i / (double)totalRowsCount) * 100;

          Dispatcher.Invoke(() => lblInfo.Text = $"{Math.Round(perc, 1):0.0}%");

          vModelPathNames = (string[])swBOMAnnotation.GetModelPathNames(i, out strItemNumber, out strPartNumber);
          string ptNm = vModelPathNames[0];

          var codMat = swTableAnnotation.get_Text(i, 2);

          if (string.IsNullOrEmpty(codMat)) {
            var configName = swTableAnnotation.get_Text(i, 14);

            var descricao = Path.GetFileNameWithoutExtension(ptNm).ToUpper();

            if (!comps.Any(x => x == Path.GetFileNameWithoutExtension(ptNm).ToUpper())) {
              comps.Add(descricao);

              int status = 0;
              int warnings = 0;
              swModel = Sw.App.OpenDoc6(ptNm,
                  Path.GetExtension(ptNm).ToUpper() == "SLDPRT" ? (int)swDocumentTypes_e.swDocPART : (int)swDocumentTypes_e.swDocASSEMBLY,
                   (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);

              if (swModel != null) {
                var swModelDocExt = swModel.Extension;

                var swCustPropMgr = swModelDocExt.get_CustomPropertyManager(configName);
                swCustPropMgr.Delete2("PINTURA");
                swCustPropMgr.Delete2("CHECK");
                swModel.Save();
                Sw.App.CloseDoc(ptNm);
              }
            }
          }
        }

        Dispatcher.Invoke(() => lblInfo.Text = "Concluido");

        swModel = (ModelDoc2)Sw.App.ActiveDoc;
        swModel.Save();
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao pegar dados da Lista Plano Pintura");
      }
    }
  }
}
