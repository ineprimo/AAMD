using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



/// <summary>
/// Clase creada para, dada una lista de claves obtenida del LocalizeMgr, crear un array de GUIContent y así pintarse
/// en el editor para que el usuario escoja entre las opciones la clave deseada.
/// </summary>
[System.Serializable]
public class LocalizeInspectorStr
{
    /// <summary>
    /// Array de GUIContents donde se guardan las diferentes claves del localizador para mostrarlas en el dropdown del editor
    /// </summary>
    private static GUIContent[] _guiContent = null;

    /// <summary>
    /// Devuelve si la lista de claves es nula
    /// </summary>
    public static bool GuiContentIsNull
    {
        get { return _guiContent == null; }
    }

    /// <summary>
    /// Getter del array de GUIContents de las claves
    /// </summary>
    public static GUIContent[] GUIContentArray
    {
        get { return _guiContent; }
    }

    /// <summary>
    /// Función llamada cuando el LocalizationMgr llama a su evento "onDestroy"
    /// </summary>
    public static void OnDestroyManager()
    {
        _guiContent = null;
    }

    /// <summary>
    /// Función usada para actualizar el array de GUIContents con la lista de claves del LocalizationMgr
    /// También es llamada por el evento "onRefresh" del LocalizationMgr
    /// </summary>
    public static void RefreshList() {
        List<string> list = LocalizationMgr.Instance.GetKeyList();

        _guiContent = new GUIContent[list.Count+1];

        string key = "";
        _guiContent[0] = new GUIContent("NONE", "");
        for (int i = 0; i < list.Count; ++i)
        {
            key = list[i];
            _guiContent[i+1] = new GUIContent(key, LocalizationMgr.Instance.Translate(key));
        }
    }

    public static bool ExistKey(string s)
    {

        for(int i = 0; i < _guiContent.Length; i++)
        {
            if (_guiContent[i].text == s)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Clave a traducir por el localizador
    /// </summary>
    public string key;

    /// <summary>
    /// Constructor de la clase
    /// </summary>
    /// <param name="key">Clave del localizador</param>
    public LocalizeInspectorStr(string key = null)
    {
        this.key = key;
    }


}
