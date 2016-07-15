using System;
using Windows.UI.Xaml.Data;

namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.Converter.MainPage.BeerCounter
{
    public class CounterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var number = (int) value;
            if (number < 1000)
                return number.ToString("0000");
            return number.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
