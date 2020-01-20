using Common;

namespace Services
{
    public interface ILanguageTranslationService
    {
        bool CanTranslate(string key);

        Dialogue Translate(string key);
    }
}