namespace CobwebAPI.API.Localization;

public readonly struct TermTranslation
{
    public readonly string Term;
    public readonly string Translation;

    public TermTranslation(string term, string translation)
    {
        this.Term = term;
        this.Translation = translation;
    }
}