using LmCorbieUI.Wpf.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Wpf = LmCorbieUI.Wpf;

namespace AddinArtama {
  /// <summary>Templates de desenho (formatos de folha) e listas de material (substitui o FrmConfigTemplate).</summary>
  public partial class WindowConfigTemplate : Wpf.LmWindow {
    templates model = new templates();
    bool novo;

    /// <summary>Campo do template → (descrição do formato, campo do nome da folha).</summary>
    readonly Dictionary<LmTextBox, (string formato, LmTextBox folha)> formatos;

    public WindowConfigTemplate() {
      InitializeComponent();

      formatos = new Dictionary<LmTextBox, (string, LmTextBox)> {
        [txtA4R] = ("A4 Retrato", lblA4R),
        [txtA4P] = ("A4 Paisagem", lblA4P),
        [txtA3] = ("A3", lblA3),
        [txtA2] = ("A2", lblA2),
        [txtA1] = ("A1", lblA1),
        [txtA0] = ("A0", lblA0),
      };

      Loaded += (s, e) => Carregar();
    }

    private void Carregar() {
      try {
        model = templates.model;

        if (model != null) {
          novo = false;

          txtA4R.Text = model.formato_a4r;
          txtA4P.Text = model.formato_a4p;
          txtA3.Text = model.formato_a3;
          txtA2.Text = model.formato_a2;
          txtA1.Text = model.formato_a1;
          txtA0.Text = model.formato_a0;
          lblA4R.Text = model.template_a4r;
          lblA4P.Text = model.template_a4p;
          lblA3.Text = model.template_a3;
          lblA2.Text = model.template_a2;
          lblA1.Text = model.template_a1;
          lblA0.Text = model.template_a0;
          txtListaMotagem.Text = model.lista_montagem;
          txtListaSoldagem.Text = model.lista_soldagem;
        } else {
          novo = true;
          model = new templates();
        }
      } catch (Exception ex) {
        Msg.Erro(ex, "Erro ao Carregar Dados");
      }
    }

    private void BtnSalvar_Click(object sender, RoutedEventArgs e) {
      model.formato_a4r = txtA4R.Text;
      model.formato_a4p = txtA4P.Text;
      model.formato_a3 = txtA3.Text;
      model.formato_a2 = txtA2.Text;
      model.formato_a1 = txtA1.Text;
      model.formato_a0 = txtA0.Text;
      model.template_a4r = lblA4R.Text;
      model.template_a4p = lblA4P.Text;
      model.template_a3 = lblA3.Text;
      model.template_a2 = lblA2.Text;
      model.template_a1 = lblA1.Text;
      model.template_a0 = lblA0.Text;
      model.lista_montagem = txtListaMotagem.Text;
      model.lista_soldagem = txtListaSoldagem.Text;

      if (novo && model.id == 0 && templates.Salvar(model)) {
        novo = false;
        Wpf.MsgBox.Show("Salvo com Sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
      } else if (!novo && templates.Alterar(model)) {
        Wpf.MsgBox.Show("Alterado com Sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    /// <summary>Escolhe o template (.drwdot); o nome da folha (.slddrt) acompanha o nome do arquivo.</summary>
    private void TxtFormato_BotaoF7Click(object sender, RoutedEventArgs e) {
      var txt = (LmTextBox)sender;
      var (formato, folha) = formatos[txt];

      txt.Text = SelecionarArquivo($"Selecionar Formato de Folha {formato}", "Templates Desenho|*.drwdot|All files|*.*", "drwdot");
      if (!string.IsNullOrEmpty(txt.Text))
        folha.Text = Path.GetFileNameWithoutExtension(txt.Text) + ".slddrt";
    }

    private void TxtListaMotagem_BotaoF7Click(object sender, RoutedEventArgs e) {
      var arquivo = SelecionarArquivo("Selecionar Lista de Montagem", "Lista Montagem|*.sldbomtbt|All files|*.*", "sldbomtbt");
      if (!string.IsNullOrEmpty(arquivo))
        txtListaMotagem.Text = arquivo;
    }

    private void TxtListaSoldagem_BotaoF7Click(object sender, RoutedEventArgs e) {
      var arquivo = SelecionarArquivo("Selecionar Lista de Corte/Soldagem", "Lista Corte/Soldagem|*.sldwldtbt|All files|*.*", "sldwldtbt");
      if (!string.IsNullOrEmpty(arquivo))
        txtListaSoldagem.Text = arquivo;
    }

    private string SelecionarArquivo(string titulo, string filtro, string extensao) {
      var ofd = new OpenFileDialog {
        RestoreDirectory = true,
        Title = titulo,
        Filter = filtro,
        DefaultExt = extensao,
      };

      return ofd.ShowDialog(this) == true ? ofd.FileName : string.Empty;
    }
  }
}
