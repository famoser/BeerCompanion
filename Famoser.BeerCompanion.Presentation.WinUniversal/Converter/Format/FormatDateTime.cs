using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Converter.Format
{
    public class FormatDateTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dt = (DateTime)value;
            if (dt - DateTime.Today < TimeSpan.FromDays(1))
                return "Heute um " + dt.ToString("hh:mm");
            if (dt > DateTime.Now - TimeSpan.FromDays(7))
                return "Letzter " + dt.ToString("dddd") + " um " + dt.ToString("hh:mm");
            return dt.ToString("dddd hh:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
