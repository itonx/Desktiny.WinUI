using System.Collections.Generic;
using Windows.ApplicationModel.Resources.Core;

namespace Desktiny.WinUI.Services
{
    public interface ILanguageService
    {
        IReadOnlyList<string> GetSupportedAppLanguages();
        IReadOnlyList<string> GetUserMachineSupportedLanguages();
        string GetLangValue(string resourceKey);
    }


    public class LanguageService : ILanguageService
    {
        public IReadOnlyList<string> GetSupportedAppLanguages()
        {

            return Windows.Globalization.ApplicationLanguages.ManifestLanguages;
        }

        public IReadOnlyList<string> GetUserMachineSupportedLanguages()
        {
            return Windows.Globalization.ApplicationLanguages.Languages;
        }

        public string GetLangValue(string resourceKey)
        {
            return LangService.GetLangValue(resourceKey);
        }
    }

    public static class LangService
    {
        private static readonly string _assemblyName = typeof(LanguageService).Assembly.GetName().Name;
        private static readonly ResourceMap _desktinyResourceMap;
        private static readonly ResourceMap _runningAppResourceMap;

        static LangService()
        {
            _desktinyResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree(_assemblyName + "/" + Constants.Global.LANG_TREE);
            _runningAppResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree(Constants.Global.LANG_TREE);
        }

        public static string GetLangValue(string resourceKey)
        {
            string appLangValue = _runningAppResourceMap?.GetValue(resourceKey)?.ValueAsString ?? string.Empty;
            if (string.IsNullOrEmpty(appLangValue))
            {
                return _desktinyResourceMap.GetValue(resourceKey)?.ValueAsString ?? string.Empty;
            }

            return appLangValue;
        }

        public static Dictionary<string, string> GetLangOptions()
        {
            Dictionary<string, string> langOptions = new();
            foreach (var lang in Windows.Globalization.ApplicationLanguages.ManifestLanguages)
            {
                string langOptionValue = GetLangValue(string.Format(Constants.Global.LANG_KEY_FORMAT, lang.ToUpper()));
                langOptions.Add(lang, langOptionValue);
            }
            return langOptions;
        }
    }
}
