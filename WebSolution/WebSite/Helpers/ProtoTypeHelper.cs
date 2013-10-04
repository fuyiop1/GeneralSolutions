using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Helpers
{
    public static class ProtoTypeHelper
    {
        public static int TryToParseInt(string intString, int failValue = 0)
        {
            var result = 0;
            if (!int.TryParse(intString, out result))
            {
                result = failValue;
            }
            return result;
        }

        public static string AutoCompleteOuterUrl(this string val)
        {
            var result = string.Empty;
            if (string.IsNullOrWhiteSpace(val))
            {
                result = "javascript:;";
            }
            else
            {
                if (!val.StartsWith("http"))
                {
                    result = string.Format("http://{0}", val);
                }
                else
                {
                    result = val;
                }
            }
            return result;
        }

        public static string AutoProcessVidelUrl(this string val)
        {
            var result = val;
            var youtube = "youtube.com";
            var vimeo = "vimeo.com";

            if (!string.IsNullOrWhiteSpace(val))
            {
                if (val.Contains(youtube))
                {
                    var videoRouteValue = val.Split(new char[] { '&', '?' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(x => x.StartsWith("v="));
                    if (!string.IsNullOrWhiteSpace(videoRouteValue))
                    {
                        var videoRouteValueStrings = videoRouteValue.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (videoRouteValueStrings.Count() == 2)
                        {
                            var videoId = videoRouteValueStrings[1];
                            result = string.Format("http://www.youtube.com/embed/{0}", videoId);
                        }
                    }

                }
                else if (val.Contains(vimeo))
                {
                    result = val;
                    if (!val.Contains("player.vimeo.com/video") && !val.EndsWith("/"))
                    {
                        var videoId = val.Substring(val.LastIndexOf('/') + 1);
                        result = string.Format("http://player.vimeo.com/video/{0}", videoId);
                    }
                }
            }

            return result;
        }

        public static string ToStandardDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static bool IsRichTextEmpty(string richText)
        {
            var result = true;
            var emptyContentConditions = new string[] { "<br>", "<br />", "<p></p>", "<p>&nbsp;</p>" };
            if (richText != null && !emptyContentConditions.Contains(richText.Trim()))
            {
                result = false;
            }
            var htmlToText = new HtmlToTextHelper();
            if (!string.IsNullOrWhiteSpace(htmlToText.Convert(richText)))
            {
                result = false;
            }
            return result;
        }

    }
}