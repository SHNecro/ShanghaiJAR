using System.Linq;

namespace NSShanghaiEXE.ExtensionMethods
{
    public static class StringExtensionMethods
    {
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
    }
}
