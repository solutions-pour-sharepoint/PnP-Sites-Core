using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Utilities
{
    public static class PnPHttpUtility
    {
        //Code copied from Microsoft.SharePoint.Utilities.SPHttpUtility 
        internal static readonly string[] HTMLData;

        static PnPHttpUtility()
        {
            //Code copied from Microsoft.SharePoint.Utilities.SPHttpUtility 
            HTMLData = new string[] { "", "&quot;", "&amp;", "&#39;", "&lt;", "&gt;", " ", "<br />", "&#160;", "<b>", "<i>", "<u>", "</b>", "</i>", "</u>", "<wbr />" };
        }

        //Code copied from Microsoft.SharePoint.Utilities.SPHttpUtility 
        /// <summary>
        /// Converts an HTML string from a Windows SharePoint Services rich text field to plain text.
        /// </summary>
        /// <param name="html">An HTML string that contains the contents of a Windows SharePoint Services rich text field.</param>
        /// <param name="maxLength">A 32-bit integer representing the maximum desired length of the returned string, or -1 to specify no maximum length.</param>
        /// <returns>A plain-text string version of the string.</returns>        
        public static string ConvertSimpleHtmlToText(string html, int maxLength)
        {
            return HtmlDecodeCore(html, maxLength, null);
        }

        //Code copied from Microsoft.SharePoint.Utilities.SPHttpUtility
        internal static string HtmlDecodeCore(string html, int maxLength, IList<string> tagsToRetain)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }
            if (maxLength == 0)
            {
                return string.Empty;
            }
            var builder = new StringBuilder();
            var currentPosition = 0;
            var startIndex = 0;
            while ((currentPosition < html.Length) && ((maxLength < 0) || (builder.Length < maxLength)))
            {
                var ch = html[currentPosition];
                switch (ch)
                {
                    case '&':
                    case '<':
                        {
                            var length = currentPosition - startIndex;
                            var flag = false;
                            if ((maxLength > -1) && ((builder.Length + length) >= maxLength))
                            {
                                flag = true;
                                length = maxLength - builder.Length;
                            }
                            if (length > 0)
                            {
                                builder.Append(html.Substring(startIndex, length));
                            }
                            if (flag)
                            {
                                return builder.ToString();
                            }
                            break;
                        }
                }
                switch (ch)
                {
                    case '&':
                        {
                            builder.Append(ProceedToEndOfHtmlString(html, ref currentPosition));
                            startIndex = currentPosition;
                            continue;
                        }
                    case '<':
                        {
                            builder.Append(ProceedToEndOfTag(html, tagsToRetain, ref currentPosition));
                            startIndex = currentPosition;
                            continue;
                        }
                }
                currentPosition++;
            }
            if ((maxLength < 0) || ((maxLength - builder.Length) >= (html.Length - startIndex)))
            {
                builder.Append(html.Substring(startIndex));
            }
            else
            {
                var num4 = maxLength - builder.Length;
                if (num4 > 0)
                {
                    builder.Append(html.Substring(startIndex, num4));
                }
            }
            return builder.ToString();
        }

        //Code copied from Microsoft.SharePoint.Utilities.SPHttpUtility 
        internal static string ProceedToEndOfHtmlString(string html, ref int currentPosition)
        {
            var ch = html[currentPosition];
            var num = currentPosition;
            while ((ch != ';') && (num < (html.Length - 1)))
            {
                ch = html[++num];
            }
            var str = string.Empty;
            switch (html.Substring(currentPosition, (num - currentPosition) + 1))
            {
                case "&quot;":
                    str = "\"";
                    break;

                case "&amp;":
                    str = "&";
                    break;

                case "&#39;":
                    str = "'";
                    break;

                case "&lt;":
                    str = "<";
                    break;

                case "&gt;":
                    str = ">";
                    break;

                case "&#160;":
                    str = " ";
                    break;
            }
            currentPosition = num + 1;
            return str;
        }

        //Code copied from Microsoft.SharePoint.Utilities.SPHttpUtility 
        internal static string ProceedToEndOfTag(string html, IList<string> tagsToRetain, ref int currentPosition)
        {
            var ch = html[currentPosition];
            var num = currentPosition;
            while ((ch != '>') && (num < (html.Length - 1)))
            {
                ch = html[++num];
            }
            var str = html.Substring(currentPosition, (num - currentPosition) + 1);
            var flag = str.EndsWith("/>", StringComparison.Ordinal);
            var index = str.IndexOf(' ');
            if (index == -1)
            {
                index = str.IndexOf('>');
            }
            var item = str.Substring(1, index - 1);
            var targetCloseTag = "</" + item + ">";
            var str4 = string.Empty;
            if (str == HTMLData[7])
            {
                str4 = "\n";
            }
            if (string.IsNullOrEmpty(str4) && (tagsToRetain != null) && tagsToRetain.Contains(item))
            {
                if (flag)
                {
                    str4 = str;
                    currentPosition = num + 1;
                    return str4;
                }
                var startIndex = num + 1;
                ProceedToEndOfCloseTag(targetCloseTag, html, ref currentPosition);
                return str + html.Substring(startIndex, currentPosition - startIndex);
            }
            if (!flag && ((str == "<style>") || str.Contains("display:none")))
            {
                ProceedToEndOfCloseTag(targetCloseTag, html, ref currentPosition);
                return str4;
            }
            currentPosition = num + 1;
            return str4;
        }

        //Code copied from Microsoft.SharePoint.Utilities.SPHttpUtility 
        private static void ProceedToEndOfCloseTag(string targetCloseTag, string html, ref int currentPosition)
        {
            var length = targetCloseTag.Length;
            while (currentPosition < (html.Length - 1))
            {
                int num2;
                currentPosition = num2 = currentPosition + 1;
                if ((html[num2] == '<') && ((currentPosition + length) < html.Length) && targetCloseTag.Equals(html.Substring(currentPosition, length)))
                {
                    currentPosition += targetCloseTag.Length;
                    return;
                }
            }
        }
    }
}
