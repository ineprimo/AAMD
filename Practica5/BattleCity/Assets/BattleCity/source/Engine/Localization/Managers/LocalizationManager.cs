using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField]
    private LocalizationConfig localizationConfig;
    private Dictionary<string, TextAsset> _languageDictionary;
    private string _default;
    //private HashSet<string> _languages;
    public void Configure(LocalizationConfig config)
    {
        localizationConfig = config;
        if (Application.isPlaying)
            DontDestroyOnLoad(this.gameObject);
        _default = localizationConfig.defaulLanguage;
        CreateLanguageDictionary(localizationConfig.availableLanguages);
        LocalizationMgr.Instance.Configure(_languageDictionary[localizationConfig.defaulLanguage].name, localizationConfig.defaulLanguage, _languageDictionary[localizationConfig.defaulLanguage].text);
    }




    public void ChangeLanguage(string lang)
    {
        if (!_languageDictionary.ContainsKey(lang))
        {
            Debug.LogWarning("ChangeLanguage: Lenguage " + lang + " no soportado");
            return;
        }

        if(LocalizationMgr.Instance.CurrentLanguage != lang)
        {
            
            LocalizationMgr.Instance.Configure(_languageDictionary[lang].name,lang, _languageDictionary[lang].text);
        }
    }

    public string Translate(string key)
    {
        return LocalizationMgr.Instance.Translate(key);
    }

    public string Translate(string key, params object[] parameters)
    {
        return LocalizationMgr.Instance.Translate(key, parameters);
    }

    public string GetCurrentLanguage()
    {
        return LocalizationMgr.Instance.CurrentLanguage;
    }

    protected void CreateLanguageDictionary(LocalizationFiles[] availableLanguages)
    {
        _languageDictionary = new Dictionary<string, TextAsset>();

        for (int i = 0; i < availableLanguages.Length;  ++i)
            _languageDictionary.Add(availableLanguages[i].languages, availableLanguages[i].text);
    }


    protected void OnDestroy()
    {
        LocalizationMgr.Destroy();
    }

}
