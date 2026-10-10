using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AddinArtama {
  /// <summary>Converte imagens WinForms (ex.: Properties.Resources) para uso nas telas WPF.</summary>
  internal static class ImagemWpf {
    public static ImageSource Converter(Image imagem) {
      if (imagem == null)
        return null;

      using (var ms = new MemoryStream()) {
        imagem.Save(ms, ImageFormat.Png);
        ms.Position = 0;

        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.StreamSource = ms;
        bitmap.EndInit();
        bitmap.Freeze();

        return bitmap;
      }
    }
  }

  /// <summary>Converter de binding: mostra numa célula WPF uma propriedade Image/Bitmap das classes (ex.: DrawExport.Img3D).</summary>
  public class BitmapParaImagemConverter : IValueConverter {
    // Grades grandes repetem a mesma imagem em muitas linhas: converte cada instância uma vez só.
    static readonly ConditionalWeakTable<Image, ImageSource> cache = new ConditionalWeakTable<Image, ImageSource>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (!(value is Image imagem))
        return null;

      return cache.GetValue(imagem, ImagemWpf.Converter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotSupportedException();
    }
  }
}
