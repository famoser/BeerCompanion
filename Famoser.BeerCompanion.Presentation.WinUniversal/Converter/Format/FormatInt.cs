using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Converter.Format
{
    public class FormatInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var defaultDigits = 2;
            var s = parameter as string;
            if (s != null && !int.TryParse(s, out defaultDigits))
                defaultDigits = 2;

            var str = "";
            var number = 1;
            for (int i = 0; i < defaultDigits; i++)
            {
                str += "0";
                number *= 10;
            }
            
            var val = (int)value;
            if (val < number)
                return val.ToString(str);
            return val.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
