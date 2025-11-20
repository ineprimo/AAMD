using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LocalizationEditor : EditorWindow
{
    public const string defaultPath = "./Assets/Localization";
    private const int ID_COL = 0;
    private const int COMMENT_COL = 2;


    [MenuItem("Assets/Create/Create localization config")]
    public static void CreateAsset()
    {
        if (!Directory.Exists(LocalizationMgr.ConfigurationPath))
            Directory.CreateDirectory(LocalizationMgr.ConfigurationPath);
        ScriptableObject.CreateInstance<LocalizationConfig>();
        LocalizationConfig localizationConfig = ScriptableObject.CreateInstance<LocalizationConfig>();
        AssetDatabase.CreateAsset(localizationConfig, LocalizationMgr.ConfigurationFile);
        AssetDatabase.SaveAssets();
    }


    //[MenuItem("Assets/BuildConfiguration")]
    [MenuItem("8Picaros Utilities/Load language csv")]
    static void CreateWindow()
    {
        string path = EditorUtility.OpenFilePanel("Select the localization csv file", "Assets/Localization", "csv");
        if (path.Length != 0)
        {
            List<LanguagesKeyValue> commentList = new List<LanguagesKeyValue>();
            Dictionary<string, List<LanguagesKeyValue>> languageDictionary = new Dictionary<string, List<LanguagesKeyValue>>();

            string[] fileContentInLines = File.ReadAllLines(path);
            List<string> languages = ProcessFields(fileContentInLines[0]);
            for(int i = 0; i < languages.Count; ++i)
            {
                languageDictionary.Add(languages[i], new List<LanguagesKeyValue>());
            }

            for (int i = 1; i < fileContentInLines.Length; ++i)
            {
                string line = fileContentInLines[i];
                //line = ChangeDelimiter(line,0, ';');
                line = line.Trim();
                string[] fields = line.Split(LocalizationMgr.CSV_FIELD_DELIMETER);
                if (fields.Length > 1)
                {
                    if (fields.Length != languages.Count + 2)
                        Debug.LogWarning("El fichero de lenguaje csv " + path + " no es correcto, no tiene el mismo numero de campos que los especificados en la cabecera " + fields.Length + " line ("+line+")");

                    string ID = fields[ID_COL]; //ID
                    if (ID == "CUTSCENE/bane_title")
                        Debug.Log("Breakpoint");
                    string comment = fields.Length >= 3 ? fields[COMMENT_COL] : ""; //descripcion
                    commentList.Add(new LanguagesKeyValue(ID, comment));
                    for (int j = ID_COL + 1; j < fields.Length; ++j)
                    {
                        if (j != COMMENT_COL)
                        {
                            string field = fields[j];
                            field = field.Trim();
                            if (field.StartsWith("\"") && field.EndsWith("\""))
                            {
                                field = field.Substring(1, field.Length - 2);
                            }

                            List<LanguagesKeyValue> langList = languageDictionary[languages[j - ((j > COMMENT_COL)? 2 : 1)]];
                            langList.Add(new LanguagesKeyValue(ID, field));
                        }
                    }

                }// Son categorias que no necesitamos...
            }

            if(!Directory.Exists(defaultPath))
            {
                Directory.CreateDirectory(defaultPath);
            }
            //Ya tengo separados los lenguajes, ahora genero ficheros para cada lenguaje.
            foreach (var keyValue in languageDictionary)
            {
                string lan = keyValue.Key;
                List < LanguagesKeyValue > fileContent = keyValue.Value;
                string stringToWrite = "ID\t"+ lan+"\n";
                foreach(var entry in fileContent)
                {
                    stringToWrite += entry.Key + LocalizationMgr.CSV_FIELD_DELIMETER + entry.Value+"\n";
                }
                File.WriteAllText(defaultPath+"/" + lan + ".csv", stringToWrite);
            }

            //Genero el fichero de descripción

            string stringToWriteComment = "ID\tEXPLICACION\n";
            foreach (var entry in commentList)
            {
                stringToWriteComment += entry.Key + LocalizationMgr.CSV_FIELD_DELIMETER + entry.Value + "\n";
            }
            File.WriteAllText(defaultPath + "/Description.txt", stringToWriteComment);

        }

    }

    private static string ChangeDelimiter(string line,  int index, char delimiter)
    {
        int firstColon = line.IndexOf(LocalizationMgr.CSV_FIELD_DELIMETER, index);
        if (firstColon >= 0)
        {
            int firstStrDelimiter = line.IndexOf("\"", index);
            if (firstStrDelimiter < 0 || firstStrDelimiter > firstColon)
            {
                char[] lineArray = line.ToCharArray();
                lineArray[firstColon] = delimiter;
                line = new string(lineArray);
                line = ChangeDelimiter(line, firstColon+1, delimiter);
            }
            else
                line = ChangeDelimiter(line, line.IndexOf("\"", firstStrDelimiter+1)+1, delimiter);

        }
        return line;
    }

    private static List<string> ProcessFields(string line)
    {
        List<string> languages = new List<string>();
        string[] fields = line.Split(LocalizationMgr.CSV_FIELD_DELIMETER);

        if (fields.Length < 3)
            Debug.LogError("El fichero de idiomas debe tener al menos tres campos. ID, descripción y un idioma");
        else
        {
            for (int i = ID_COL+1; i < fields.Length; ++i)
            {
                //fields[i];
                if (i != COMMENT_COL)
                {
                    string lenguage = fields[i];
                    languages.Add(lenguage);
                }
            }
        }

        return languages;
    }
}
