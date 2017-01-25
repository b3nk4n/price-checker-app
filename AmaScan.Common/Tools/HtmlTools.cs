using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AmaScan.Common.Tools
{
    public static class HtmlTools
    {
        public static string ExtractTextFromHtml(string html, string id)
        {
            Regex regex = new Regex(string.Format("<span id=\"{0}\" .*?>(.*?)</span>", id), RegexOptions.Singleline);
            var v = regex.Match(html);
            string s = v.Groups[1].ToString();
            string trimmedContent = s.Trim();
            return trimmedContent;
        }

        public static Uri ExtractImageUriFromHtml(string html)
        {
            // extract html region
            Regex regex = new Regex("<li class=\"swatchHoverExp a-hidden maintain-height\">(.*?)</li>", RegexOptions.Singleline);
            var v = regex.Match(html);
            string htmlInner = v.Groups[1].ToString();

            // extract img-src
            Regex regexInner = new Regex("src\\s*=\\s*\"(.+?)\"", RegexOptions.Singleline);
            var vInner = regexInner.Match(htmlInner);
            string sInner = vInner.Groups[1].ToString();

            string trimmedContent = sInner.Trim();
            return new Uri(trimmedContent, UriKind.Absolute);
        }
    }
}
