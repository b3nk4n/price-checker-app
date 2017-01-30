using System;
using Windows.System.UserProfile;

namespace AmaScan.Common.Tools
{
    public static class AmazonUriTools
    {
        public const string BASE_URI_FORMAT = "https://www.amazon{0}/";

        public const string SEARCH_URI = "s/field-keywords=";

        public const string DEFAULT_DOMAIN = ".com";

        public static string GetBase()
        {
            AmazonRegion selectedRegion = (AmazonRegion)Enum.Parse(typeof(AmazonRegion), AppSettings.Region.Value);
            string domain;

            if (selectedRegion == AmazonRegion.Auto)
                domain = GetDomainOfRegionAuto();
            else
                domain = GetDomainOfRegion(selectedRegion);

            return string.Format(BASE_URI_FORMAT, domain);
        }

        public static Uri GetUri()
        {
            return new Uri(GetBase(), UriKind.Absolute);
        }

        public static Uri GetSearchUri(string searchTerm)
        {
            return new Uri(string.Format("{0}{1}{2}", GetBase(), SEARCH_URI, searchTerm), UriKind.Absolute);
        }

        private static string GetDomainOfRegionAuto()
        {
            var sysRegion = GlobalizationPreferences.HomeGeographicRegion.ToUpper();

            switch (sysRegion)
            {
                // amazon official
                case "DE":
                    return ".de";
                case "FR":
                    return ".fr";
                case "CA":
                    return ".ca";
                case "CN":
                    return ".cn";
                case "NL":
                    return ".nl";
                case "GB":
                    return ".co.uk";
                case "AU":
                    return ".com.au";
                case "IN":
                    return ".in";
                case "US":
                    return ".com";
                case "jp":
                    return ".co.jp";
                case "ES":
                    return ".es";
                case "MX":
                    return ".com.mx";   
                case "IT":
                    return ".it";
                case "BR":
                    return ".com.br";

                // other:
                case "CH":
                    return ".de";
                case "AT":
                    return ".de";
                case "PT":
                    return ".co.uk";
                case "NO":
                    return ".co.uk";
                case "SE":
                    return ".co.uk";
                case "CZ":
                    return ".co.uk";
                case "PL":
                    return ".co.uk";
                case "DK":
                    return ".co.uk";
                case "FI":
                    return ".co.uk";
                case "TR":
                    return ".co.uk";
                case "BE":
                    return ".co.uk";
                case "UA":
                    return ".co.uk";

                default:
                    return DEFAULT_DOMAIN;
            }
        }

        private static string GetDomainOfRegion(AmazonRegion region)
        {
            switch (region)
            {
                case AmazonRegion.Germany:
                    return ".de";
                case AmazonRegion.France:
                    return ".fr";
                case AmazonRegion.Canada:
                    return ".ca";
                case AmazonRegion.China:
                    return ".cn";
                case AmazonRegion.Netherlands:
                    return ".nl";
                case AmazonRegion.UnitedKingdom:
                    return ".co.uk";
                case AmazonRegion.Australia:
                    return ".com.au";
                case AmazonRegion.India:
                    return ".in";
                case AmazonRegion.USA:
                    return ".com";
                case AmazonRegion.Japan:
                    return ".co.jp";
                case AmazonRegion.Spain:
                    return ".es";
                case AmazonRegion.Mexico:
                    return ".com.mx";
                case AmazonRegion.Italy:
                    return ".it";
                case AmazonRegion.Brazil:
                    return ".com.br";
                default:
                    return DEFAULT_DOMAIN;
            }
        }
    }
}
