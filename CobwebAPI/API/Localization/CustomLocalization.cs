using I2.Loc;

namespace CobwebAPI.API.Localization;

public class CustomLocalization : Singleton<CustomLocalization>
{
    public void AddTerm(string term, string translation, string language = "English")
    {
        var source = this.GetLanguageSource(language, out var langIdx);

        var termData = source.ContainsTerm(term) ? source.GetTermData(term) : source.AddTerm(term, eTermType.Text);
        termData.Languages[langIdx] = translation;
        
        source.UpdateDictionary();
    }

    public void AddTerm(string term, IEnumerable<LanguageTranslation> translations)
    {
        var sources = new List<LanguageSourceData>();
        
        foreach (var translation in translations)
        {
            var source = this.GetLanguageSource(translation.Language, out var langIdx);

            if (!sources.Contains(source))
            {
                sources.Add(source);
            }

            this.SetTermTranslation(source, langIdx, term, translation.Translation);
        }

        foreach (var source in sources)
        {
            source.UpdateDictionary();
        }
    }

    public void AddTerms(IEnumerable<TermTranslation> translations, string language = "English")
    {
        var source = this.GetLanguageSource(language, out var langIdx);

        foreach (var translation in translations)
        {
            this.SetTermTranslation(source, langIdx, translation.Term, translation.Translation);
        }

        source.UpdateDictionary();
    }
    
    public void AddTerms(IEnumerable<TermLanguageTranslation> languageTranslations)
    {
        var sources = new Dictionary<string, (LanguageSourceData, int)>();

        foreach (var languageTranslation in languageTranslations)
        {
            foreach (var translation in languageTranslation.Translations)
            {
                if (!sources.TryGetValue(translation.Language, out var sourceInfo))
                {
                    sourceInfo = (this.GetLanguageSource(translation.Language, out var langIdx), langIdx);
                    sources[translation.Language] = sourceInfo;
                }

                this.SetTermTranslation(
                    sourceInfo.Item1,
                    sourceInfo.Item2,
                    languageTranslation.Term,
                    translation.Translation);
            }
        }

        foreach (var source in sources.Values.Select(sourceInfo => sourceInfo.Item1))
        {
            source.UpdateDictionary();
        }
    }

    private LanguageSourceData GetLanguageSource(string language, out int langIdx)
    {
        var source = this.GetLanguageSource(language);
        
        langIdx = source.GetLanguageIndex(language, false, false);

        return source;
    }
    
    private LanguageSourceData GetLanguageSource(string language)
    {
        var source = LocalizationManager.Sources.Find(source => source.GetLanguageIndex(language, false, false) != -1);
        if (source == null)
        {
            source = LocalizationManager.Sources[0];
            source.AddLanguage(language);
        }

        return source;
    }

    private void SetTermTranslation(LanguageSourceData source, int langIdx, string term, string translation)
    {
        var termData = source.ContainsTerm(term) ? source.GetTermData(term) : source.AddTerm(term, eTermType.Text);
        termData.Languages[langIdx] = translation;
    }
}