using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Converter.Generic
{
    public class ReferenceToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isnull = value != null;
            if (parameter as string == "inverted")
                isnull = !isnull;
            if (isnull)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
