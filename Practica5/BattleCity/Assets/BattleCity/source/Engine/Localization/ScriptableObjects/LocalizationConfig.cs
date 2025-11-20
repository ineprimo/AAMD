using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[System.Serializable]
public class LocalizationFiles
{
    public string languages;
    public TextAsset text;
}

[CreateAssetMenu(fileName = "LocalizationConfig", menuName = "8Picaros/Config/LocalizationConfig")]
public class LocalizationConfig : ScriptableObject
{
    private void OnEnable()
    {
        hideFlags = HideFlags.None;
    }

    [Tooltip("Fichero csv con los textos")]
    public string defaulLanguage = "ESPAÑOL";

#if UNITY_EDITOR
    public TextAsset descriptionText;
#endif
    public LocalizationFiles[] availableLanguages;

    //Recursos
}
