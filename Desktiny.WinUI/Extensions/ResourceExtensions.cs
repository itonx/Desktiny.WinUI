using Desktiny.WinUI.Tools;
using Microsoft.UI.Xaml;
using System;
using System.Linq;

namespace Desktiny.WinUI.Extensions
{
    public static class ResourceExtensions
    {
        public static ResourceDictionary GetCustomResource(this ResourceDictionary resourceDictionary)
        {
            return resourceDictionary
                .MergedDictionaries
                .LastOrDefault(d => d.Source.AbsoluteUri.Contains("Theme.xaml"));
        }

        public static ResourceDictionary GetResourceByType(this ResourceDictionary resourceDictionary, Type type)
        {
            return resourceDictionary
                .MergedDictionaries
                .FirstOrDefault(d => d.GetType() == type);
        }

        public static ResourceDictionary GetThemeDictionary(this ResourceDictionary resourceDictionary, string themeKey)
        {
            return (ResourceDictionary)resourceDictionary.ThemeDictionaries[themeKey];
        }

        public static string GetThemeKeyFromDefault(this ElementTheme elementTheme)
        {
            bool isDarkModeDefault = PInvoke.UseDarkMode;
            return elementTheme == ElementTheme.Light ?
                isDarkModeDefault ? "Light" : "Default" :
                isDarkModeDefault ? "Default" : "Dark";
        }
    }
}
