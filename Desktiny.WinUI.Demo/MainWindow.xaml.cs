using Desktiny.WinUI.Demo.ViewModels;
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
        private IList<AppThemeModel> themes = new List<AppThemeModel>()
        {
            new AppThemeModel("Light", ElementTheme.Light, "Resources/OverrideWinUITheme.xaml", "\uE793"),
            new AppThemeModel("Dark", ElementTheme.Dark, "Resources/OverrideWinUITheme.xaml", "\uF0CE"),
            new AppThemeModel("Neuromancer", ElementTheme.Dark, "Resources/NeuromancerTheme.xaml", "\uE950")
        };

        public MainWindow()
        {
            InitializeComponent();
            ThemeService.RegisterThemes(themes);
            var vm = new MainViewModel();
            this.WinstonContainer.DataContext = vm;
        }
    }
}
