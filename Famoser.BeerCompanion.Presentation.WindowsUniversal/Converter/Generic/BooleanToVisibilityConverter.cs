using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.Converter.Generic
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bo = (bool)value;
            if (parameter as string == "inverted")
                bo = !bo;
            if (bo)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
