using Desktiny.WinUI.Models;
using Desktiny.WinUI.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Desktiny.WinUI
{
    public sealed class ThemeButton : Button
    {
        public static readonly DependencyProperty AppThemeProperty = DependencyProperty.Register(
            nameof(AppTheme),
            typeof(AppThemeModel),
            typeof(ThemeButton),
            new PropertyMetadata(new AppThemeModel(ElementTheme.Default)));

        public AppThemeModel AppTheme
        {
            get { return (AppThemeModel)GetValue(AppThemeProperty); }
            set { SetValue(AppThemeProperty, value); }
        }

        public ThemeButton()
        {
            this.DefaultStyleKey = typeof(ThemeButton);
            this.Click += ThemeButton_Click;
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            this.AppTheme = ThemeService.GetNextTheme(this.AppTheme);
        }
    }
}
