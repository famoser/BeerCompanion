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
            var str = parameter is string ? (string)parameter : "";
            if (value != null)
            {
                var dt = (DateTime) value;
                if (dt - DateTime.Today > TimeSpan.Zero)
                    return "Heute um " + dt.ToString("HH:mm") + str;
                if (DateTime.Today - dt < TimeSpan.FromDays(1))
                    return "Gestern um " + dt.ToString("HH:mm") + str;
                if (DateTime.Today - dt < TimeSpan.FromDays(7))
                    return "Letzter " + dt.ToString("dddd") + " um " + dt.ToString("HH:mm") + str;
                return dt.ToString("dddd hh:mm") + str;
            }
            return "unbekanntes Datum";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
