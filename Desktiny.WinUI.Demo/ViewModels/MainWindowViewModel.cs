using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktiny.WinUI.Demo.Enums;
using Desktiny.WinUI.EventMessages;
using Desktiny.WinUI.Managers;
using Desktiny.WinUI.Models;
using Desktiny.WinUI.Services;

namespace Desktiny.WinUI.Demo.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private AppThemeModel _currentAppTheme;
        [ObservableProperty]
        private DesktinyPages _currentPage;

        public MainViewModel()
        {
            CurrentAppTheme = ThemeService.GetTheme();
            CurrentPage = DesktinyPages.Welcome;
            RegisterEvents();
        }

        [RelayCommand]
        private void GoToDemosPage()
        {
            CurrentPage = DesktinyPages.Demos;
        }

        [RelayCommand]
        private void GoToBuyMeACoffeePage()
        {
            CurrentPage = DesktinyPages.BuyMeACoffee;
        }

        private void RegisterEvents()
        {
            EventManager.Subscribe<EnumNavigation>(this, OnDesktinyPageMessage);
        }

        private void OnDesktinyPageMessage(EnumNavigation enumNavigation)
        {
            this.CurrentPage = (DesktinyPages)enumNavigation.Page;
        }
    }
}
