using System;
using System.Xml;

namespace Data
{
    public static class CharacterInfo
    {
        private static bool[,] FloatingCharacters;
        private static bool[,] NoShadowCharacters;

        static CharacterInfo()
        {
            LoadCharacterInfo(out FloatingCharacters, out NoShadowCharacters);
        }

        public static bool IsFloatingCharacter(int sheet, int index) =>
            sheet < FloatingCharacters.GetLength(0) && index < 8 && FloatingCharacters[sheet, index];

        public static bool IsNoShadowCharacter(int sheet, int index) =>
            sheet < NoShadowCharacters.GetLength(0) && index < 8 && NoShadowCharacters[sheet, index];

        public static void LoadCharacterInfo(out bool[,] floatingCharacters, out bool[,] noShadowCharacters)
        {
            floatingCharacters = new bool[20, 8];
            noShadowCharacters = new bool[20, 8];

            var languageDoc = new XmlDocument();
            languageDoc.Load($"data/data/CharacterInfo.xml");

            var characterNodes = languageDoc.SelectNodes("data/Character");
            foreach (XmlNode characterNode in characterNodes)
            {
                var sheet = int.Parse(characterNode?.Attributes["Sheet"]?.Value ?? "-1");
                var index = int.Parse(characterNode?.Attributes["Index"]?.Value ?? "-1");

                if (sheet == -1 || index == -1)
                {
                    throw new InvalidOperationException("Character info missing sheet and index.");
                }

                if (index >= 8)
                {
                    throw new InvalidOperationException("Invalid character index.");
                }

                if (sheet >= floatingCharacters.GetLength(0))
                {
                    var newFloating = new bool[sheet + 1, 8];
                    var newNoShadow = new bool[sheet + 1, 8];

                    for (var i = 0; i < floatingCharacters.GetLength(0); i++)
                    {
                        for (var ii = 0; ii <= 7; ii++)
                        {
                            newFloating[i, ii] = floatingCharacters[i, ii];
                            newNoShadow[i, ii] = noShadowCharacters[i, ii];
                        }
                    }

                    floatingCharacters = newFloating;
                    noShadowCharacters = newNoShadow;
                }

                var isFloating = bool.Parse(characterNode?.Attributes["IsFloating"]?.Value ?? "False");
                var noShadow = bool.Parse(characterNode?.Attributes["NoShadow"]?.Value ?? "False");

                floatingCharacters[sheet, index] = isFloating;
                noShadowCharacters[sheet, index] = noShadow;
            }
        }
    }
}