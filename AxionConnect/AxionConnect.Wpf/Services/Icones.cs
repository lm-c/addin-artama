using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AxionConnect.Wpf.Services {
  /// <summary>
  /// Ícones de Assets/ (os mesmos PNGs do projeto WinForms), carregados uma vez e congelados
  /// para poderem ser usados por qualquer thread.
  /// </summary>
  public static class Icones {
    public static readonly ImageSource Montagem = Carregar("assembly");
    public static readonly ImageSource Peca = Carregar("part");
    public static readonly ImageSource ListaCorte = Carregar("weldmentcutlist");
    public static readonly ImageSource Chapa = Carregar("sheetmetal");
    public static readonly ImageSource Soldagem = Carregar("weldment");
    public static readonly ImageSource Biblioteca = Carregar("toolbox_item");
    public static readonly ImageSource Desenho = Carregar("draw");
    public static readonly ImageSource SemDesenho = Carregar("not_draw");
    public static readonly ImageSource Fantasma = Carregar("fantasma");
    public static readonly ImageSource Erro = Carregar("error");
    public static readonly ImageSource Alerta = Carregar("warning");
    public static readonly ImageSource ZoomAjustar = Carregar("zoomfit");
    public static readonly ImageSource Isometrico = Carregar("isometric");
    public static readonly ImageSource Proximo = Carregar("next");
    public static readonly ImageSource Ferramentas = Carregar("tools");

    private static ImageSource Carregar(string nome) {
      var img = new BitmapImage(new Uri($"pack://application:,,,/AxionConnect;component/Assets/{nome}.png"));
      img.Freeze();
      return img;
    }
  }
}
