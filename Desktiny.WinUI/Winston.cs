using Desktiny.WinUI.Behaviors;
using Desktiny.WinUI.Extensions;
using Desktiny.WinUI.Models;
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
            Winston winContainer = d as Winston;
            winContainer.SetApptheme((AppThemeModel)e.NewValue);
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
            if (EnableTitleBarThemeButtons)
            {
                try
                {
                    string defaultThemeKey = this.AppTheme.ElementTheme.GetThemeKeyFromDefault();

                    ResourceDictionary lastDictionary = Application.Current.Resources.GetCustomResource().GetThemeDictionary(this.AppTheme.ElementTheme.ToString());

                    var lastDictionaryKeys = lastDictionary
                        .Keys
                        .Select((value, index) => new { Key = value, Value = index })
                        .ToDictionary(i => i.Key, i => i.Value);

                    Application.Current.Resources.GetResourceByType(typeof(XamlControlsResources)).GetThemeDictionary(defaultThemeKey)
                        .TryGetValue(Constants.Global.TITLE_BAR_BUTTON_BACKGROUND_KEY, out object resourceValue);

                    if (resourceValue is SolidColorBrush backgroundValue)
                    {
                        lastDictionary.TryGetValue(Constants.Global.TITLE_BAR_BUTTON_BACKGROUND_KEY, out object customBackground);
                        SolidColorBrush customBackgroundValue = lastDictionaryKeys.ContainsKey(Constants.Global.TITLE_BAR_BUTTON_BACKGROUND_KEY) ? customBackground as SolidColorBrush : null;

                        Color color = customBackgroundValue != null && customBackgroundValue.Color != backgroundValue.Color ? customBackgroundValue.Color : backgroundValue.Color;
                        this.AppWindow.TitleBar.ButtonBackgroundColor = color;
                        this.AppWindow.TitleBar.ButtonInactiveBackgroundColor = color;
                    }
                }
                finally
                {
                    this.AppWindow.TitleBar.ButtonHoverBackgroundColor = this.AppTheme.ElementTheme == ElementTheme.Light ? Color.FromArgb(50, 0, 0, 0) : Color.FromArgb(50, 255, 255, 255);
                    this.AppWindow.TitleBar.ButtonHoverForegroundColor = this.AppTheme.ElementTheme == ElementTheme.Light ? Colors.Black : Colors.White;
                    this.AppWindow.TitleBar.ButtonForegroundColor = this.AppTheme.ElementTheme == ElementTheme.Light ? Colors.Black : Colors.White;
                }
            }
        }

        private void SetApptheme(AppThemeModel appThemeModel)
        {
            if (appThemeModel == null) return;
            Grid container = this.GetWindowContainer();
            if (container == null) return;
            ResourceDictionary lastDictionary = Application.Current.Resources.MergedDictionaries.LastOrDefault(d => d.Source.AbsoluteUri.Contains("Theme.xaml"));
            ElementTheme switchTo = appThemeModel.ElementTheme == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;

            if (_removeLastThemeResource != null && (bool)_removeLastThemeResource && lastDictionary != null && lastDictionary.Source != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(lastDictionary);
            }

            _removeLastThemeResource = appThemeModel.ThemeResource != null;
            if (appThemeModel.ThemeResource != null)
            {
                ResourceDictionary resourceTheme = new ResourceDictionary()
                {
                    Source = appThemeModel.ThemeResource
                };
                Application.Current.Resources.MergedDictionaries.Add(resourceTheme);
            }

            container.RequestedTheme = switchTo;
            container.RequestedTheme = appThemeModel.ElementTheme;
        }

        private void SetWindows()
        {
            MainWindow = WindowHelper.CurrentMainWindow;
            AppWindow = WindowHelper.CurrentAppWindow;
        }
    }
}
