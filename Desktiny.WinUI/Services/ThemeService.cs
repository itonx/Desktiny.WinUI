using Desktiny.WinUI.Models;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace Desktiny.WinUI.Services
{
    public static class ThemeService
    {
        private static SortedDictionary<string, AppThemeModel> _appThemesDict = new(StringComparer.OrdinalIgnoreCase);

        public static AppThemeModel GetTheme()
        {
            EnsureThemesRegistered();

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string savedTheme = (localSettings.Values[Constants.Global.SETTINGS_THEME_KEY] as string)?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(savedTheme)
                && _appThemesDict.TryGetValue(savedTheme, out AppThemeModel savedAppTheme)
                && IsValidTheme(savedAppTheme))
            {
                return savedAppTheme;
            }

            AppThemeModel osTheme = GetThemeByElementTheme(GetOsElementTheme());
            if (osTheme != null)
            {
                return osTheme;
            }

            AppThemeModel firstTheme = _appThemesDict.Values.FirstOrDefault(IsValidTheme);
            return firstTheme ?? CreateBuiltInTheme(ElementTheme.Light);
        }

        public static AppThemeModel GetNextTheme(AppThemeModel currentAppTheme)
        {
            EnsureThemesRegistered();

            try
            {
                List<AppThemeModel> themes = _appThemesDict.Values.Where(IsValidTheme).ToList();
                if (themes.Count == 0)
                {
                    AppThemeModel fallbackTheme = GetTheme();
                    SaveThemeName(fallbackTheme);
                    return fallbackTheme;
                }

                if (currentAppTheme == null)
                {
                    AppThemeModel fallbackTheme = GetTheme();
                    SaveThemeName(fallbackTheme);
                    return fallbackTheme;
                }

                int currentThemeIndex = themes.FindIndex(theme =>
                    theme.Name.Equals(currentAppTheme.Name, StringComparison.OrdinalIgnoreCase));

                if (currentThemeIndex < 0)
                {
                    AppThemeModel fallbackTheme = GetTheme();
                    SaveThemeName(fallbackTheme);
                    return fallbackTheme;
                }

                AppThemeModel nextAppTheme = themes[(currentThemeIndex + 1) % themes.Count];
                SaveThemeName(nextAppTheme);
                return nextAppTheme;
            }
            catch
            {
                AppThemeModel fallbackTheme = GetTheme();
                SaveThemeName(fallbackTheme);
                return fallbackTheme;
            }
        }

        public static void RegisterThemes(IEnumerable<AppThemeModel> appThemes)
        {
            SortedDictionary<string, AppThemeModel> rebuiltThemes = new(StringComparer.OrdinalIgnoreCase);

            if (appThemes != null)
            {
                foreach (AppThemeModel appTheme in appThemes)
                {
                    if (appTheme == null || string.IsNullOrWhiteSpace(appTheme.Name))
                    {
                        continue;
                    }

                    rebuiltThemes[appTheme.Name.Trim()] = appTheme;
                }
            }

            _appThemesDict = rebuiltThemes;
            EnsureThemesRegistered();
        }

        private static void EnsureThemesRegistered()
        {
            if (_appThemesDict.Values.Any(IsValidTheme))
            {
                return;
            }

            _appThemesDict.Clear();
            _appThemesDict["Light"] = CreateBuiltInTheme(ElementTheme.Light);
            _appThemesDict["Dark"] = CreateBuiltInTheme(ElementTheme.Dark);
        }

        private static AppThemeModel GetThemeByElementTheme(ElementTheme elementTheme)
        {
            return _appThemesDict.Values.FirstOrDefault(theme =>
                IsValidTheme(theme) && theme.ElementTheme == elementTheme);
        }

        private static ElementTheme GetOsElementTheme()
        {
            ApplicationTheme osTheme = Application.Current?.RequestedTheme ?? ApplicationTheme.Light;
            return osTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;
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

        private static bool IsValidTheme(AppThemeModel appTheme)
        {
            if (appTheme == null || string.IsNullOrWhiteSpace(appTheme.Name))
            {
                return false;
            }

            return appTheme.ElementTheme == ElementTheme.Light || appTheme.ElementTheme == ElementTheme.Dark;
        }

        private static void SaveThemeName(AppThemeModel appTheme)
        {
            if (appTheme == null || string.IsNullOrWhiteSpace(appTheme.Name))
            {
                return;
            }

            ApplicationData.Current.LocalSettings.Values[Constants.Global.SETTINGS_THEME_KEY] = appTheme.Name;
        }
    }
}
