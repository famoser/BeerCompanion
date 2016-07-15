using System;
using Windows.UI.Xaml.Data;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Converter.Generic
{
    public class BooleanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bo = (bool) value;
            if (bo)
                return 1;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
