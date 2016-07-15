using System;
using Windows.UI.Xaml.Data;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Converter.Format
{
    public class FormatDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dub = (double) value;
            var decim = 2;
            if (parameter != null && !int.TryParse((string) parameter, out decim))
                decim = 2;

            return Math.Round(dub, decim).ToString("00.00");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
