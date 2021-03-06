﻿using System;
using System.Collections;
using Windows.UI.Xaml.Data;
using Famoser.BeerCompanion.Presentation.WindowsUniversal.Converter.Format;

namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.Converter.Collection
{
    public class CountItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var coll = value as IList;
            var conv = new FormatInt();
            return conv.Convert(coll?.Count ?? 0, null, parameter ?? "1", null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
