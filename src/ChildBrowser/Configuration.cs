﻿using System;
using System.Linq;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows;

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

            ResourceDictionary dict = new ResourceDictionary();
            switch (cultureInfo.Name)
            {
                case "ru-RU":
                    dict.Source = new Uri(string.Format("Resources/UITexts.{0}.xaml", cultureInfo.Name), UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("Resources/UITexts.xaml", UriKind.Relative);
                    break;
            }

            ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                          where d.Source != null && d.Source.OriginalString.StartsWith("Resources/UITexts.")
                                          select d).First();
            if (oldDict != null)
            {
                int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }

        private static CultureInfo GetCultureInfo(string language)
        {
            if (string.IsNullOrWhiteSpace(language)) return null;

            return CultureInfo.GetCultureInfo(language);
        }
    }
}
