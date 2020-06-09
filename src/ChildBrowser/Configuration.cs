using System.Configuration;
using System.Globalization;
using System.Threading;

namespace ChildBrowser
{
    internal static class Configuration
    {
        private const string LanguageKey = "Language";

        private static readonly CultureInfo _defaultCulture;
        private static readonly CultureInfo _defaultUiCulture;

        static Configuration()
        {
            _defaultCulture = Thread.CurrentThread.CurrentCulture;
            _defaultUiCulture = Thread.CurrentThread.CurrentUICulture;
        }

        public static string Language
        {
            get
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[LanguageKey] == null)
                {
                    return null;
                }
                else
                {
                    return settings[LanguageKey].Value;
                }
            }
            set
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                value = GetCultureInfo(value) != null ? value : null;

                if (settings[LanguageKey] == null)
                {
                    settings.Add(LanguageKey, value);
                }
                else
                {
                    settings[LanguageKey].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
        }

        public static void SetCurrentCulture()
        {
            var cultureInfo = GetCultureInfo(Language);

            if(cultureInfo == null)
            {
                Thread.CurrentThread.CurrentCulture = _defaultCulture;
                Thread.CurrentThread.CurrentUICulture = _defaultUiCulture;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
        }

        private static CultureInfo GetCultureInfo(string language)
        {
            if (string.IsNullOrWhiteSpace(language)) return null;

            return CultureInfo.GetCultureInfo(language);
        }
    }
}
