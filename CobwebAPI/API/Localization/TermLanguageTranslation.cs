namespace CobwebAPI.API.Localization;

public readonly struct TermLanguageTranslation
{
    public readonly string Term;
    public readonly LanguageTranslation[] Translations;

    public TermLanguageTranslation(string term, IEnumerable<LanguageTranslation> translations)
    {
        this.Term = term;
        this.Translations = translations.ToArray();
    }
}