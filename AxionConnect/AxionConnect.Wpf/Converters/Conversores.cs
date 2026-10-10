using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AxionConnect.Wpf.Converters {
  public class InverseBoolToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
      value is true ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
      throw new NotSupportedException();
  }

  public class InverseBoolConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is not true;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is not true;
  }

  /// <summary>True quando todos os valores são iguais (ex.: Tag do item de menu = tela aberta).</summary>
  public class IguaisConverter : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
      values.Length >= 2 && Equals(values[0]?.ToString(), values[1]?.ToString());

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
      throw new NotSupportedException();
  }

  public class NullToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
      value == null || (value is string s && s.Length == 0) ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
      throw new NotSupportedException();
  }
}

namespace AxionConnect.Wpf.Converters {
  /// <summary>
  /// Mostra numa célula WPF os ícones do ProdutoErp (CORE), que são System.Drawing.Bitmap.
  /// Cada Bitmap é convertido uma vez só (as grades repetem as mesmas imagens em muitas linhas).
  /// </summary>
  public class BitmapParaImagemConverter : System.Windows.Data.IValueConverter {
    static readonly System.Runtime.CompilerServices.ConditionalWeakTable<System.Drawing.Image, System.Windows.Media.ImageSource> cache = new();

    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) =>
      value is System.Drawing.Image imagem ? cache.GetValue(imagem, Converter) : null;

    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) =>
      throw new System.NotSupportedException();

    public static System.Windows.Media.ImageSource Converter(System.Drawing.Image imagem) {
      using (var ms = new System.IO.MemoryStream()) {
        imagem.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        ms.Position = 0;

        var bitmap = new System.Windows.Media.Imaging.BitmapImage();
        bitmap.BeginInit();
        bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
        bitmap.StreamSource = ms;
        bitmap.EndInit();
        bitmap.Freeze();
        return bitmap;
      }
    }
  }
}
