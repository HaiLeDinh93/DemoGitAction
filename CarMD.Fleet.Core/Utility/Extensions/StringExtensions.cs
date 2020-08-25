using System.Collections.Generic;

namespace CarMD.Fleet.Core.Utility.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        private static readonly Regex humps = new Regex("(?:^[a-zA-Z][^A-Z]*|[A-Z][^A-Z]*)");
        private static readonly Regex safe = new Regex(@"[^_\-a-zA-Z\d]+");
        public const string DEFAULT_SEPARATOR = ",";

        public static string CamelFriendly(this string camel)
        {
            if (String.IsNullOrWhiteSpace(camel))
                return "";

            var matches = humps.Matches(camel).OfType<Match>().Select(m => m.Value).ToArray();
            return matches.Any()
                       ? matches.Aggregate((a, b) => a + " " + b).TrimStart(' ')
                       : camel;
        }

        public static string Ellipsize(this string text, int characterCount)
        {
            return text.Ellipsize(characterCount, "&#160;&#8230;");
        }

        public static string Ellipsize(this string text, int characterCount, string ellipsis, bool wordBoundary = false)
        {
            if (String.IsNullOrWhiteSpace(text))
                return "";

            if (characterCount < 0 || text.Length <= characterCount)
                return text;

            var trimmed = Regex.Replace(text.Substring(0, characterCount), @"\s+\S*$", "");

            if (wordBoundary)
            {
                trimmed = Regex.Replace(trimmed + ".", @"\W*\w*$", "");
            }

            return trimmed + ellipsis;
        }

        public static string HtmlClassify(this string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return "";

            var friendlier = text.CamelFriendly();
            return Regex.Replace(friendlier, @"[^a-zA-Z]+", m => m.Index == 0 ? "" : "-").ToLowerInvariant();
        }

        public static string RemoveTags(this string html)
        {
            return String.IsNullOrEmpty(html)
                       ? ""
                       : Regex.Replace(html, "<[^<>]*>", "", RegexOptions.Singleline);
        }

        // not accounting for only \r (e.g. Apple OS 9 carriage return only new lines)
        public static string ReplaceNewLinesWith(this string text, string replacement)
        {
            return String.IsNullOrWhiteSpace(text)
                       ? ""
                       : Regex.Replace(text, @"(\r?\n)", replacement, RegexOptions.Singleline);
        }

        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static byte[] ToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length).
                Where(x => 0 == x % 2).
                Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).
                ToArray();
        }

        public static bool IsValidUrlSegment(this string segment)
        {
            // valid isegment from rfc3987 - http://tools.ietf.org/html/rfc3987#page-8
            // the relevant bits:
            // isegment    = *ipchar
            // ipchar      = iunreserved / pct-encoded / sub-delims / ":" / "@"
            // iunreserved = ALPHA / DIGIT / "-" / "." / "_" / "~" / ucschar
            // pct-encoded = "%" HEXDIG HEXDIG
            // sub-delims  = "!" / "$" / "&" / "'" / "(" / ")" / "*" / "+" / "," / ";" / "="
            // ucschar     = %xA0-D7FF / %xF900-FDCF / %xFDF0-FFEF / %x10000-1FFFD / %x20000-2FFFD / %x30000-3FFFD / %x40000-4FFFD / %x50000-5FFFD / %x60000-6FFFD / %x70000-7FFFD / %x80000-8FFFD / %x90000-9FFFD / %xA0000-AFFFD / %xB0000-BFFFD / %xC0000-CFFFD / %xD0000-DFFFD / %xE1000-EFFFD
            // 
            // rough blacklist regex == m/^[^/?#[]@"^{}|\s`<>]+$/ (leaving off % to keep the regex simple)

            return Regex.IsMatch(segment, @"^[^/?#[\]@""^{}|`<>\s]+$");
        }

        /// <summary>
        /// Generates a valid technical name.
        /// </summary>
        /// <remarks>
        /// Uses a white list set of chars.
        /// </remarks>
        public static string ToSafeName(this string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;

            name = RemoveDiacritics(name);
            name = safe.Replace(name, String.Empty);
            name = name.Trim();

            // don't allow non A-Z chars as first letter, as they are not allowed in prefixes
            while (name.Length > 0 && !IsLetter(name[0]))
            {
                name = name.Substring(1);
            }

            if (name.Length > 128)
                name = name.Substring(0, 128);

            return name;
        }

        /// <summary>
        /// Whether the char is a letter between A and Z or not
        /// </summary>
        public static bool IsLetter(this char c)
        {
            return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
        }


        public static string RemoveDiacritics(string name)
        {
            string stFormD = name.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char t in stFormD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static int? ToInt(this string number)
        {
            try
            {
                return int.Parse(number.Trim());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Split a string to an array of strings with default separator = DEFAULT_SEPARATOR
        /// Ex: "111, 222, 333" ==> ["111", "222", "333"]
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static List<string> ToList(this string val)
        {
            return ToList(val, DEFAULT_SEPARATOR);
        }

        /// <summary>
        /// Split a string to an array of strings.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> ToList(this string val, string separator)
        {
            if (string.IsNullOrEmpty(val))
                return new List<string>();

            return val.Split(separator.ToCharArray()).ToList();
        }

        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        public static string Left(this string s, int length)
        {
            if (s.Length > length)
                return s.Substring(0, length);

            return s;
        }

        /// <summary>
        /// Determines whether [is valid email address] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [is valid email address] [the specified s]; otherwise, <c>false</c>.</returns>
        public static bool IsValidEmailAddress(this string s)
        {
            const string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var re = new Regex(strRegex);
            return re.IsMatch(s);
        }

        public static string Substring(this string htmlString, string patternPrefix, string patternPostfix,
                                       bool? truncatePrefixPostfix = true, int? prefixLength = null,
                                       int? postfixLength = null)
        {
            string propertyString = null;

            var regex = new Regex(patternPrefix + @"[^/]*" + patternPostfix, RegexOptions.IgnoreCase);
            Match match = regex.Match(htmlString);
            if (!match.Success)
            {
                regex = new Regex(patternPrefix + @"[\S]*" + patternPostfix, RegexOptions.IgnoreCase);
                match = regex.Match(htmlString);

                if (!match.Success)
                {
                    regex = new Regex(patternPrefix + @"(.)*" + patternPostfix, RegexOptions.IgnoreCase);
                    match = regex.Match(htmlString);

                    if (!match.Success)
                    {
                        regex = new Regex(patternPrefix + @"[\S\s]*" + patternPostfix, RegexOptions.IgnoreCase);
                        match = regex.Match(htmlString);
                    }
                }
            }


            if (match.Success)
            {
                propertyString = match.Value;

                if (truncatePrefixPostfix.HasValue && truncatePrefixPostfix.Value)
                    propertyString = propertyString.Substring(prefixLength ?? patternPrefix.Length,
                                                              propertyString.Length -
                                                              (postfixLength ?? patternPostfix.Length) -
                                                              (prefixLength ?? patternPrefix.Length));
            }

            return propertyString;
        }

        public static string TrimRequestParam(this string val)
        {
            return string.IsNullOrEmpty(val) ? val : val.Trim();
        }


        public static List<string> CovertRequestParamToStringList(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return new List<string>();
            }
            var ids = val.Trim().Split(',');
            return ids.ToList();
        }

        public static Guid ParseGuid(this string id)
        {
            try
            {
                Guid gid;
                if (string.IsNullOrWhiteSpace(id))
                {
                    return Guid.Empty;
                }

                if (Guid.TryParse(id, out gid))
                {
                    return gid;
                }
            }
            catch
            {

            }
            return Guid.Empty;
        }

        public static List<Guid> CovertRequestParamToGuidList(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return new List<Guid>();
            }
            var ids = val.Trim().Split(',');
            var result = new List<Guid>();

            foreach (var item in ids)
            {
                var id = item.ParseGuid();
                if (id != Guid.Empty)
                {
                    result.Add(id);
                }
            }
            return result;
        }
    }
}