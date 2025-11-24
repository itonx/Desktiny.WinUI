using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktiny.WinUI.Demo.Enums;
using Desktiny.WinUI.Demo.Models;
using Desktiny.WinUI.Managers;
using Desktiny.WinUI.Models;
using Desktiny.WinUI.Services;
using System;

namespace Desktiny.WinUI.Demo.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private AppThemeModel _currentAppTheme;
        [ObservableProperty]
        private Enum _currentPage;

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
            EventManager.Subscribe<DestinyPageMessage>(this, OnDesktinyPageMessage);
        }

        private void OnDesktinyPageMessage(DestinyPageMessage message)
        {
            this.CurrentPage = message.DesktinyPage;
        }
    }
}
