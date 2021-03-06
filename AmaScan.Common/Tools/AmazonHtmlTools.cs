using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UWPCore.Framework.Storage;

namespace AmaScan.Common.Tools
{
    public static class AmazonHtmlTools
    {
        public const string FALLBACK_IMAGE = IOConstants.APPX_SCHEME + "/Assets/Images/fallback.png";

        public static string ExtractTextFromHtmlInDetailPage(string html, string id)
        {
            Regex regex = new Regex(string.Format("<span id=\"{0}\" .*?>(.*?)</span>", id), RegexOptions.Singleline);
            var v = regex.Match(html);
            string s = v.Groups[1].ToString();
            string trimmedContent = s.Trim();
            trimmedContent = trimmedContent.Replace("  ", " ");
            return trimmedContent;
        }

        public static Uri ExtractImageUriFromHtmlInDetailPage(string html)
        {
            // extract html region
            Regex regex = new Regex("<li class=\"image item itemNo0 maintain-height selected\">(.*?)</li>", RegexOptions.Singleline);
            var v = regex.Match(html);
            string htmlInner = v.Groups[1].ToString();

            // extract img-src
            Regex regexInner = new Regex("src\\s*=\\s*\"(.+?)\"", RegexOptions.Singleline);
            var vInner = regexInner.Match(htmlInner);
            string sInner = vInner.Groups[1].ToString();

            string trimmedContent = sInner.Trim();

            if (trimmedContent != string.Empty)
                return new Uri(trimmedContent, UriKind.Absolute);

            return new Uri(FALLBACK_IMAGE, UriKind.Relative);
        }

        public static string ExtractTextFromHtml(string html)
        {
            try
            {
                Regex regex = new Regex("<h2 .*? class=\"a-size-medium a-color-null .*?>(.*?)</h2>", RegexOptions.Singleline);
                var v = regex.Match(html);
                string s = v.Groups[1].ToString();
                string trimmedContent = s.Trim();
                trimmedContent = trimmedContent.Replace("  ", " ");

                if (trimmedContent == string.Empty)
                {
                    Regex regexMobile = new Regex("<h5 class=\"sx-title\"><span class=\"a-size-base a-color-base\">(.*?)</span></h5>", RegexOptions.Singleline);
                    var vMobile = regexMobile.Match(html);
                    string sMobile = vMobile.Groups[1].ToString();
                    trimmedContent = sMobile.Trim();
                    trimmedContent = trimmedContent.Replace("  ", " ");
                }
                trimmedContent = trimmedContent.Replace("&amp;Amp;", "&");
                trimmedContent = trimmedContent.Replace("&amp;", "&");

                return trimmedContent;
            }
            catch (Exception) { }

            return string.Empty;
        }

        public static Uri ExtractImageUriFromHtml(string html)
        {
            try
            {
                // extract html region
                Regex regex = new Regex("<a class=\"a-link-normal a-text-normal\" .*?>(.*?)</a>", RegexOptions.Singleline);
                var v = regex.Match(html);
                string htmlInner = v.Groups[1].ToString();

                if (htmlInner == string.Empty)
                {
                    // retry for mobile:
                    //img class="sx-product-image"
                    Regex regexMobile = new Regex("<div class=\"sx-table-image-holder\">(.*?)</div>", RegexOptions.Singleline);
                    var vMobile = regexMobile.Match(html);
                    htmlInner = vMobile.Groups[1].ToString();
                }

                // extract img-src
                Regex regexInner = new Regex("src\\s*=\\s*\"(.+?)\"", RegexOptions.Singleline);
                var vInner = regexInner.Match(htmlInner);
                string sInner = vInner.Groups[1].ToString();

                if (sInner.StartsWith("data:"))
                {
                    // use different (non-dynamic) link:
                    regexInner = new Regex("data-search-image-source-set=\"(.*?) 1x,", RegexOptions.Singleline);
                    vInner = regexInner.Match(htmlInner);
                    sInner = vInner.Groups[1].ToString();
                }

                string trimmedContent = sInner.Trim();

                if (trimmedContent != string.Empty)
                    return new Uri(trimmedContent, UriKind.Absolute);
            }
            catch (Exception) { }
           
            // fallback image
            return new Uri(FALLBACK_IMAGE, UriKind.Absolute);
        }
    }
}
