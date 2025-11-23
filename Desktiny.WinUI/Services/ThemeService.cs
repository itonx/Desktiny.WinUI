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
        private static SortedDictionary<string, AppThemeModel> _appThemesDict = new();

        public static AppThemeModel GetTheme()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string savedTheme = (localSettings.Values[Constants.Global.SETTINGS_THEME_KEY] as string)?.ToLower() ?? string.Empty;
            if (string.IsNullOrEmpty(savedTheme)) return _appThemesDict.Count > 0 ? _appThemesDict.FirstOrDefault().Value : new AppThemeModel(ElementTheme.Default);

            AppThemeModel appTheme = _appThemesDict.TryGetValue(savedTheme.ToLower(), out AppThemeModel value) ? value : null;
            if (appTheme == null) return new(ElementTheme.Default);

            return appTheme;
        }

        public static AppThemeModel GetNextTheme(AppThemeModel currentAppTheme)
        {
            AppThemeModel nextAppTheme = null;
            bool lastFound = false;

            foreach (var appTheme in _appThemesDict.Values)
            {
                if (!lastFound && appTheme.Name.Equals(currentAppTheme?.Name, StringComparison.OrdinalIgnoreCase))
                {
                    lastFound = true;
                    continue;
                }

                if (lastFound)
                {
                    nextAppTheme = appTheme;
                    break;
                }
            }

            if (nextAppTheme == null) nextAppTheme = _appThemesDict.Values.Count > 0 ? _appThemesDict.Values.FirstOrDefault() : new(ElementTheme.Default);

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[Constants.Global.SETTINGS_THEME_KEY] = nextAppTheme?.Name;
            return nextAppTheme;
        }

        public static void RegisterThemes(IEnumerable<AppThemeModel> appThemes)
        {
            foreach (var appTheme in appThemes)
            {
                _appThemesDict.Add(appTheme.Name.ToLower(), appTheme);
            }
        }
    }
}
