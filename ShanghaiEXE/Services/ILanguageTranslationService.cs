using Common;
using System;
using System.Drawing;

namespace Services
{
    public interface ILanguageTranslationService
    {
        bool CanTranslate(string key);

        Dialogue Translate(string key);

        Tuple<string, Rectangle> GetLocalizedSprite(string key);

        string GetFontOverride();
    }
}