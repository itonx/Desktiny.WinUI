using CommunityToolkit.Mvvm.ComponentModel;
using Desktiny.WinUI.Models;
using Desktiny.WinUI.Services;
using Microsoft.UI.Xaml;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktiny.WinUI.Demo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<AppThemeModel> themes = new()
            {
                new AppThemeModel("Light", ElementTheme.Light, "Resources/OverrideWinUITheme.xaml", "\uE793"),
                new AppThemeModel("Dark", ElementTheme.Dark, "Resources/OverrideWinUITheme.xaml", "\uF0CE"),
                new AppThemeModel("Neuromancer", ElementTheme.Dark, "Resources/NeuromancerTheme.xaml", "\uE950")
            };
            ThemeService.RegisterThemes(themes);
            AppThemeModel theme = ThemeService.GetTheme();
            var vm = new MainViewModel();
            vm.CurrentAppTheme = theme;

            this.WinstonContainer.DataContext = vm;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            bool continueProcess = await DialogLang.ShowYesNoAsync("DialogTest/Text");
            this.YesNoResult.Text = continueProcess.ToString();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await DialogLang.ShowInformationAsync("DialogOKTest/Text");
        }
    }

    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private AppThemeModel _currentAppTheme;

        partial void OnCurrentAppThemeChanged(AppThemeModel value)
        {
            var v = value;
        }
    }
}
