using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search, System.StringComparison.Ordinal);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static bool HasJapaneseCharacters(this string text)
        {
            var charArray = text.ToCharArray();
            return charArray.Any(IsJapaneseCharacter);
        }

        public static bool IsJapaneseCharacter(this char c)
        {
            var isHiragana = c >= 0x3040 && c <= 0x309F;
            var isKatakana = c >= 0x30A0 && c <= 0x30FF;
            var isKanji = c >= 0x4E00 && c <= 0x9FBF;

            return isHiragana || isKatakana || isKanji;
        }

        public static Func<string, string> CreateFormatInverter(this string format, IEnumerable<string> conflictCheckStrings)
        {
            var dummyString = new StringBuilder("#!#!#");
            while (conflictCheckStrings.Any(f => f.Contains(dummyString.ToString())))
            {
                dummyString.Append("!#");
            }
            var substitutedFormat = string.Format(format, dummyString.ToString());
            var formatPrefixLength = substitutedFormat.IndexOf(dummyString.ToString(), StringComparison.InvariantCulture);
            var formatSuffixLength = substitutedFormat.Length - dummyString.Length - formatPrefixLength;

            return new Func<string, string>(key =>
            {
                return key.Substring(formatPrefixLength, key.Length - formatSuffixLength - formatPrefixLength);
            });
        }
    }
}
