using Desktiny.WinUI.Behaviors;
using Desktiny.WinUI.Models;
using Desktiny.WinUI.Services;
using Desktiny.WinUI.Tools;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Xaml.Interactivity;
using System.Linq;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktiny.WinUI
{
    public sealed class Winston : ContentControl
    {
        public static readonly DependencyProperty MainWindowProperty = DependencyProperty.Register(
            "MainWindow",
            typeof(Window),
            typeof(Winston),
            new PropertyMetadata(null));

        public Window MainWindow
        {
            get { return (Window)GetValue(MainWindowProperty); }
            set { SetValue(MainWindowProperty, value); }
        }

        public static readonly DependencyProperty AppWindowProperty = DependencyProperty.Register(
            "AppWindow",
            typeof(AppWindow),
            typeof(Winston),
            new PropertyMetadata(null));

        public AppWindow AppWindow
        {
            get { return (AppWindow)GetValue(AppWindowProperty); }
            set { SetValue(AppWindowProperty, value); }
        }

        public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register(
            "TitleBar",
            typeof(TitleBar),
            typeof(Winston),
            new PropertyMetadata(null, OnTitleBarChanged));

        private static void OnTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            Winston winContainer = d as Winston;
            EnableCustomAppTitleBarBehavior autoSizeAppTitleBarCaptionsBehavior = new();
            Interaction.GetBehaviors(winContainer).Add(autoSizeAppTitleBarCaptionsBehavior);
        }

        public TitleBar TitleBar
        {
            get { return (TitleBar)GetValue(TitleBarProperty); }
            set { SetValue(TitleBarProperty, value); }
        }

        public static readonly DependencyProperty MaximizeAtStartupProperty = DependencyProperty.Register(
            "MaximizeAtStartup",
            typeof(bool),
            typeof(Winston),
            new PropertyMetadata(false, OnMaximizeAtStartupChanged));

        private static void OnMaximizeAtStartupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue) return;
            Winston winContainer = d as Winston;
            MaximizeWindowAtStartupBehavior maximizeWindowAtStartupBehavior = new();
            Interaction.GetBehaviors(winContainer).Add(maximizeWindowAtStartupBehavior);
        }

        public bool MaximizeAtStartup
        {
            get { return (bool)GetValue(MaximizeAtStartupProperty); }
            set { SetValue(MaximizeAtStartupProperty, value); }
        }

        public static readonly DependencyProperty AppThemeProperty = DependencyProperty.Register(
            nameof(AppTheme),
            typeof(AppThemeModel),
            typeof(Winston),
            new PropertyMetadata(new AppThemeModel(ElementTheme.Default), OnAppThemeChanged));

        private static void OnAppThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Winston winContainer)
            {
                return;
            }

            AppThemeModel nextTheme = e.NewValue as AppThemeModel ?? ThemeService.GetTheme();
            if (nextTheme == null)
            {
                return;
            }

            winContainer.SetApptheme(nextTheme);
        }

        public AppThemeModel AppTheme
        {
            get { return (AppThemeModel)GetValue(AppThemeProperty); }
            set { SetValue(AppThemeProperty, value); }
        }

        private bool? _removeLastThemeResource = null;

        public static readonly DependencyProperty IsNocturneVisibleProperty = DependencyProperty.Register(
            "IsNocturneVisible",
            typeof(bool),
            typeof(Winston),
            new PropertyMetadata(false));

        public bool IsNocturneVisible
        {
            get { return (bool)GetValue(IsNocturneVisibleProperty); }
            set { SetValue(IsNocturneVisibleProperty, value); }
        }

        public static readonly DependencyProperty NocturneContentProperty = DependencyProperty.Register(
            "NocturneContent",
            typeof(object),
            typeof(Winston),
            new PropertyMetadata(null));

        public object NocturneContent
        {
            get { return GetValue(NocturneContentProperty); }
            set { SetValue(NocturneContentProperty, value); }
        }

        public static readonly DependencyProperty IconPathProperty = DependencyProperty.Register(
            "IconPath",
            typeof(ImageSource),
            typeof(Winston),
            new PropertyMetadata(null));

        public ImageSource IconPath
        {
            get { return (ImageSource)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        public static readonly DependencyProperty EnableTitleBarThemeButtonsProperty = DependencyProperty.Register(
            "EnableTitleBarThemeButtons",
            typeof(bool),
            typeof(Winston),
            new PropertyMetadata(false));

        public bool EnableTitleBarThemeButtons
        {
            get { return (bool)GetValue(EnableTitleBarThemeButtonsProperty); }
            set { SetValue(EnableTitleBarThemeButtonsProperty, value); }
        }

        public Winston()
        {
            this.DefaultStyleKey = typeof(Winston);
            this.Loaded += WinContainer_Loaded;
        }

        private void WinContainer_Loaded(object sender, RoutedEventArgs e)
        {
            SetWindows();
            Grid container = this.GetWindowContainer();
            container.ActualThemeChanged += Container_ActualThemeChanged;
            SetApptheme(AppTheme);
            if (IconPath is BitmapImage bitmapImage)
            {
                this.AppWindow.SetIcon(bitmapImage.UriSource.LocalPath);
            }
            ChangeTitleBarButtonStyle();
        }

        private void Container_ActualThemeChanged(FrameworkElement sender, object args)
        {
            ChangeTitleBarButtonStyle();
        }

        private void ChangeTitleBarButtonStyle()
        {
            if (!EnableTitleBarThemeButtons)
            {
                return;
            }

            ElementTheme activeTheme = this.AppTheme?.ElementTheme == ElementTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;

            if (this.TitleBar.Background is SolidColorBrush titleBarBackground)
            {
                this.AppWindow.TitleBar.ButtonBackgroundColor = titleBarBackground.Color;
                this.AppWindow.TitleBar.ButtonInactiveBackgroundColor = titleBarBackground.Color;
            }
            this.AppWindow.TitleBar.ButtonHoverBackgroundColor = activeTheme == ElementTheme.Light ? Color.FromArgb(50, 0, 0, 0) : Color.FromArgb(50, 255, 255, 255);
            this.AppWindow.TitleBar.ButtonHoverForegroundColor = activeTheme == ElementTheme.Light ? Colors.Black : Colors.White;
            this.AppWindow.TitleBar.ButtonForegroundColor = activeTheme == ElementTheme.Light ? Colors.Black : Colors.White;
        }

        private void SetApptheme(AppThemeModel appThemeModel)
        {
            Grid container = this.GetWindowContainer();
            if (container == null)
            {
                return;
            }

            AppThemeModel themeToApply = NormalizeTheme(appThemeModel);
            if (themeToApply == null)
            {
                return;
            }

            try
            {
                ApplyThemeDictionary(themeToApply);
            }
            catch
            {
                themeToApply = CreateBuiltInTheme(themeToApply.ElementTheme == ElementTheme.Dark ? ElementTheme.Dark : ElementTheme.Light);

                try
                {
                    ApplyThemeDictionary(themeToApply);
                }
                catch
                {
                    _removeLastThemeResource = false;
                }
            }

            ElementTheme switchTo = themeToApply.ElementTheme == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;
            container.RequestedTheme = switchTo;
            container.RequestedTheme = themeToApply.ElementTheme;
        }

        private AppThemeModel NormalizeTheme(AppThemeModel appThemeModel)
        {
            if (appThemeModel != null && (appThemeModel.ElementTheme == ElementTheme.Light || appThemeModel.ElementTheme == ElementTheme.Dark))
            {
                return appThemeModel;
            }

            AppThemeModel fallbackTheme = ThemeService.GetTheme();
            if (fallbackTheme != null && (fallbackTheme.ElementTheme == ElementTheme.Light || fallbackTheme.ElementTheme == ElementTheme.Dark))
            {
                return fallbackTheme;
            }

            ElementTheme osTheme = Application.Current?.RequestedTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;
            return CreateBuiltInTheme(osTheme);
        }

        private void ApplyThemeDictionary(AppThemeModel appThemeModel)
        {
            ResourceDictionary lastDictionary = Application.Current.Resources.MergedDictionaries.LastOrDefault(d =>
                d?.Source?.AbsoluteUri?.Contains("Theme.xaml") == true);

            if (_removeLastThemeResource == true && lastDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(lastDictionary);
            }

            _removeLastThemeResource = appThemeModel.ThemeResource != null;
            if (appThemeModel.ThemeResource != null)
            {
                ResourceDictionary resourceTheme = new()
                {
                    Source = appThemeModel.ThemeResource
                };
                Application.Current.Resources.MergedDictionaries.Add(resourceTheme);
            }
        }

        private static AppThemeModel CreateBuiltInTheme(ElementTheme elementTheme)
        {
            string name = elementTheme == ElementTheme.Dark ? "Dark" : "Light";
            return new AppThemeModel(elementTheme)
            {
                Name = name,
                Icon = string.Empty
            };
        }

        private void SetWindows()
        {
            MainWindow = WindowHelper.CurrentMainWindow;
            AppWindow = WindowHelper.CurrentAppWindow;
        }
    }
}
