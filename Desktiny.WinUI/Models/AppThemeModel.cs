using Microsoft.UI.Xaml;
using System;

namespace Desktiny.WinUI.Models
{
    public class AppThemeModel
    {
        public string Name { get; set; }
        public ElementTheme ElementTheme { get; set; }
        public Uri ThemeResource { get; set; }
        public string Icon { get; set; }

        public AppThemeModel(ElementTheme appTheme)
        {
            ElementTheme = appTheme;
            ThemeResource = null;
            Name = string.Empty;
            Icon = string.Empty;
        }

        public AppThemeModel(string name, ElementTheme elementTheme, string themeResource, string icon)
        {
            Name = name;
            ElementTheme = elementTheme;
            string themeResourcePath = themeResource.StartsWith(Constants.Global.RESOURCE_PREFIX) ? themeResource : string.Concat(Constants.Global.RESOURCE_PREFIX, themeResource);
            ThemeResource = string.IsNullOrWhiteSpace(themeResource) ? null : new Uri(themeResourcePath);
            Icon = icon;
        }
    }
}
