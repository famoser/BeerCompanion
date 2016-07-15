using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Converter.Generic
{
    class HexToSolidColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var hex = (string)value;
            if (hex.Length == 6)
                return new SolidColorBrush(Color.FromArgb(255,
                    System.Convert.ToByte(hex.Substring(0, 2), 16),
                    System.Convert.ToByte(hex.Substring(2, 2), 16),
                    System.Convert.ToByte(hex.Substring(4, 2), 16)));
            return new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
