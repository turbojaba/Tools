﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tools
{
    public class BoolToVisibilityConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value != null && ((bool)value) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}