using Desktiny.WinUI.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources.Core;
using Windows.Storage;

namespace Desktiny.WinUI.Behaviors
{
    public class ComboBoxChangeLanguageBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            LoadLanguages();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            SetDefaultLanguage();
            AssociatedObject.IsDropDownOpen = true;
            string settingsLang = GetLanguageFromSettings();
            AssociatedObject.SelectedItem = AssociatedObject.Items.FirstOrDefault(i => (i as ComboBoxItem).Tag.Equals(settingsLang));
            AssociatedObject.IsDropDownOpen = false;
        }

        private void LoadLanguages()
        {
            Dictionary<string, string> langOptions = LangService.GetLangOptions();
            foreach (var lang in langOptions)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = lang.Value,
                    Tag = lang.Key
                };
                AssociatedObject.Items.Add(item);
            }
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string settingsLang = GetLanguageFromSettings();
            string selectedLang = (AssociatedObject.SelectedItem as ComboBoxItem).Tag.ToString();

            if (settingsLang.Equals(selectedLang, StringComparison.InvariantCultureIgnoreCase)) return;

            Microsoft.Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = selectedLang;
            ResourceManager.Current.DefaultContext.QualifierValues["Language"] = selectedLang;
            SaveLanguageSettings(selectedLang);
            RefreshUI();
        }

        private void RefreshUI()
        {
            AppInstance.Restart("");
        }

        private string GetLanguageFromSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string lang = localSettings.Values[Desktiny.WinUI.Constants.Global.SETTINGS_LANG_KEY] as string;
            return lang;
        }

        private void SaveLanguageSettings(string lang)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[Desktiny.WinUI.Constants.Global.SETTINGS_LANG_KEY] = lang;
        }

        private void SetDefaultLanguage()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string lang = localSettings.Values[Desktiny.WinUI.Constants.Global.SETTINGS_LANG_KEY] as string;
            if (ExistsInAppManifest(lang)) return;

            string defaultLangOverride = Microsoft.Windows.Globalization.ApplicationLanguages.Languages.FirstOrDefault();
            SaveLanguageSettings(defaultLangOverride);
        }

        private bool ExistsInAppManifest(string lang)
        {
            return lang != null && Windows.Globalization.ApplicationLanguages.ManifestLanguages.Any(l => l.Equals(lang, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
