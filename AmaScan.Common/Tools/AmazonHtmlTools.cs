using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AmaScan.Common.Tools
{
    public static class AmazonHtmlTools
    {
        public const string FALLBACK_IMAGE = "/Assets/Square44x44Logo.scale-200.png";

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

        public static string ExtractTextFromHtml(string html, string id)
        {
            Regex regex = new Regex(string.Format("<h2 .*? class=\"a-size-medium a-color-null .*?>(.*?)</h2>", id), RegexOptions.Singleline);
            var v = regex.Match(html);
            string s = v.Groups[1].ToString();
            string trimmedContent = s.Trim();
            trimmedContent = trimmedContent.Replace("  ", " ");
            return trimmedContent;
        }

        public static Uri ExtractImageUriFromHtml(string html)
        {
            // extract html region
            Regex regex = new Regex("<a class=\"a-link-normal a-text-normal\" .*?>(.*?)</a>", RegexOptions.Singleline);
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
    }
}
