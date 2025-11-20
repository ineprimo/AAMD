using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

public class LocalizationMgr
{
    public const string ConfigurationPath = "Assets";
    public const string ConfigurationFileName = "LocalizationConfig";
    public const string ConfigurationFile = ConfigurationPath + "/" + ConfigurationFileName+".asset";
    private static LocalizationMgr _instance=null;
    public const Char CSV_FIELD_DELIMETER = '\t';

    static public event System.Action onReloadList;
    static public event System.Action onDestroy;


    private string _currentLanguage;
    private Dictionary<string, string> _dictionaryText;
    private string _fileLoaded;
    


    public string CurrentLanguage
    {
        get{
            return _currentLanguage;
        }
    }

    public static void Destroy()
    {
        _instance = null;

        if (onDestroy != null)
            onDestroy();
    }

    public static void Reload()
    {
        Destroy();
        _instance = Instance;

        if (onReloadList != null)
            onReloadList();
    }



    public static LocalizationMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LocalizationMgr();
            }

            return _instance;
        }
    }

    public List<string> GetKeyList()
    {
        return new List<string>(_dictionaryText.Keys);
    }


    public string Translate(string key, params object[] parameters)
    {
        if (key == "NONE")
            return "";
        if(!_dictionaryText.ContainsKey(key))
            throw new Exception("Translate: la key ("+key+") no se ha encontrado en el diccionario");

        string value = _dictionaryText[key];
        try
        {
            return String.Format(value, parameters);
        }
        catch(NullReferenceException e)
        {
            return value;
        }
        catch (FormatException e)
        {
            var pattern = @"{(.*?)}";
            var matches = Regex.Matches(value, pattern);
            if (matches.Count > parameters.Length && parameters.Length > 0)
                throw new FormatException(e.Message);
            return value;
        }
    }

    public string FiledLoaded
    {
        get { return _fileLoaded; }
    }

    public bool IsFileLoaded(string file)
    {
        if (_fileLoaded == null || file == null)
            return false;

        return _fileLoaded.Equals(file);
    }

    public bool KeyExist(string key)
    {
        return _dictionaryText.ContainsKey(key);
    }


    private LocalizationMgr()
    {
        _dictionaryText = new Dictionary<string, string>();
#if UNITY_EDITOR
        string[] assets = AssetDatabase.FindAssets("t:" + typeof(LocalizationConfig).Name);

        if(assets.Length != 1)
            Debug.LogError("Error al cargar los datos de configuración de localizacion "+ assets.Length);
        else
        {
            LocalizationConfig config = AssetDatabase.LoadAssetAtPath<LocalizationConfig>(AssetDatabase.GUIDToAssetPath(assets[0]));
            if (config.descriptionText == null)
                return;
            Configure(config.descriptionText.name, config.defaulLanguage, config.descriptionText.text);
        }
#endif
    }

    public void Configure(string fileLoaded, string currentLanguage, string fileContent)
    {
        _fileLoaded = fileLoaded;
        _currentLanguage = currentLanguage;
        _dictionaryText = CreateDictionary(fileContent);
    }


    private Dictionary<string,string> CreateDictionary(string fileContent)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        string[] lines = fileContent.Split('\n');
        //Evitamos la primera fila porque asumimos que es una cabecera con los nombres de los campos.
        for (int i = 1; i < lines.Length; ++i)
        {
            string line = lines[i];
            //string processLine = ChangeDelimiter(line, 0, ';');
            //string[] fields = processLine.Split(';');
            string[] fields = line.Split(CSV_FIELD_DELIMETER);

            string ID = fields[0]; //ID
            string text = fields.Length == 2 ? fields[1] : ""; //Text
            text = text.Trim();

            if (!dictionary.ContainsKey(ID))
                dictionary.Add(ID, text);

        }
        return dictionary;
    }

    private static string ChangeDelimiter(string line, int index, char delimiter)
    {
        int firstColon = line.IndexOf(",", index);
        if (firstColon >= 0)
        {
            int firstStrDelimiter = line.IndexOf("\"", index);
            if (firstStrDelimiter < 0 || firstStrDelimiter > firstColon)
            {
                char[] lineArray = line.ToCharArray();
                lineArray[firstColon] = delimiter;
                line = new string(lineArray);
                line = ChangeDelimiter(line, firstColon + 1, delimiter);
            }
            else
                line = ChangeDelimiter(line, line.IndexOf("\"", firstStrDelimiter + 1) + 1, delimiter);

        }
        return line;
    }


}
