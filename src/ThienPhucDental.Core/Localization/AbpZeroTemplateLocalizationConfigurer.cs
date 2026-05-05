using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ThienPhucDental.Localization
{
    public static class ThienPhucDentalLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ThienPhucDentalConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ThienPhucDentalLocalizationConfigurer).GetAssembly(),
                        "ThienPhucDental.Localization.ThienPhucDental"
                    )
                )
            );
        }
    }
}