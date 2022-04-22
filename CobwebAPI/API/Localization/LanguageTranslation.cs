namespace CobwebAPI.API.Localization;

public readonly struct LanguageTranslation
{
    public readonly string Language;
    public readonly string Translation;

    public LanguageTranslation(string language, string translation)
    {
        this.Language = language;
        this.Translation = translation;
    }
}