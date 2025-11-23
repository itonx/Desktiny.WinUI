using Desktiny.WinUI.Models;
using Microsoft.UI.Xaml.Data;
using System;

namespace Desktiny.WinUI.Converters
{
    public class ThemeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            AppThemeModel currentAppTheme = (AppThemeModel)value;
            return currentAppTheme.Icon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
