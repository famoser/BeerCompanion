using System;
using Windows.UI.Xaml.Data;

namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.Converter.Format
{
    public class FormatDateTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = parameter is string ? (string)parameter : "";
            var divider = " um ";
            if (str == "short")
            {
                divider = ", ";
                str = "";
            }
            if (value != null)
            {
                var dt = (DateTime) value;
                if (dt - DateTime.Today > TimeSpan.Zero)
                    return "Heute"+ divider + dt.ToString("HH:mm") + str;
                if (DateTime.Today - dt < TimeSpan.FromDays(1))
                    return "Gestern" + divider + dt.ToString("HH:mm") + str;
                if (DateTime.Today - dt < TimeSpan.FromDays(7))
                    return "Letzter " + dt.ToString("dddd") + divider + dt.ToString("HH:mm") + str;
                return dt.ToString("dddd dd.MM.yy hh:mm");
            }
            if (divider == ", ")
                return "unbekannt";
            return "unbekanntes Datum";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
