using System;
using System.Globalization;

namespace AmaScan.Common.Tools
{
    public static class AmazonUriTools
    {
        public const string BASE_URI_FORMAT = "https://www.amazon{0}/";

        public const string SEARCH_URI = "s/field-keywords=";

        public const string DEFAULT_DOMAIN = ".com";

        public static string GetBase()
        {
            return string.Format(BASE_URI_FORMAT, GetCultureDomain());
        }

        public static Uri GetUri()
        {
            return new Uri(GetBase(), UriKind.Absolute);
        }

        public static Uri GetSearchUri(string searchTerm)
        {
            return new Uri(string.Format("{0}{1}{2}", GetBase(), SEARCH_URI, searchTerm), UriKind.Absolute);
        }

        private static string GetCultureDomain()
        {
            var ci = CultureInfo.CurrentCulture;
            switch (ci.TwoLetterISOLanguageName)
            {
                case "de":
                    return ".de";
                case "fr":
                    if (ci.Name == "fr-CA")
                        return ".ca";
                    else
                        return ".fr";
                case "nl":
                    return ".nl";
                case "en":
                    if (ci.Name == "en-GB")
                        return ".co.uk";
                    else if (ci.Name == "en-AU")
                        return ".com.au";
                    else
                        return ".com";
                case "jp":
                    return ".jp";
                case "es":
                    return ".es";
                case "it":
                    return ".it";
                case "hi": // hi-IN
                    return ".in";
                default:
                    return DEFAULT_DOMAIN;
            }
        }
    }
}
