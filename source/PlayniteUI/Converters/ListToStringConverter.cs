﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PlayniteUI
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return string.Join(", ", ((List<string>)value).ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringVal = (string)value;

            if (string.IsNullOrEmpty(stringVal))
            {
                return null;
            }
            else
            {
                return stringVal.Split(new char[] { ',' }).SkipWhile(a => string.IsNullOrEmpty(a.Trim())).Select(a => a.Trim()).ToList();
            }
        }
    }
}
