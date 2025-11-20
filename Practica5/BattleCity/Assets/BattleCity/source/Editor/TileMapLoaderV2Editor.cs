using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TileMapLoaderV2))]
[CanEditMultipleObjects]
public class TileMapLoaderV2Editor : Editor
{
    //C:\Users\masterAdmin\Documents\Unity\BattleCity\Assets\BattleCity\scenes\Finales
    public const string PATH = "Assets/BattleCity/scenes/Finales";
    private bool foldout = false;
    SerializedProperty _path;
    void OnEnable()
    {
        _path = serializedObject.FindProperty("_scenePath");
        foldout = false;
    }
    public override void OnInspectorGUI()
    {
        var scriptInstance = target as TileMapLoaderV2;
        bool sceneGenerated = false;
        Color c = GUI.contentColor;
        GUI.contentColor = new Color(1f,0.5f,0f);
        if (GUILayout.Button(new GUIContent("Export Scene","Exportamos la escena y calculamos el nuevo hash")))
        {
            GUI.contentColor = c;
            Debug.Log("Exportamos la escena");
            scriptInstance.ResetScene();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(scriptInstance.SubSceneController.GetComponent<ExportVersion>());
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }
            scriptInstance.LoadMapAndGenerateLevel(scriptInstance.exportInPrefabMode);
            sceneGenerated=scriptInstance.GenerateScene();
            /*if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }*/
            //scriptInstance.CreatingSubScene()
        }
        GUI.contentColor = Color.yellow;
        if (GUILayout.Button(new GUIContent("Export Scene if exist changes", "Exportamos la escena y calculamos el nuevo hash")))
        {
            GUI.contentColor = c;
            if(scriptInstance.IsTiledChanged())
            {
                Debug.Log("Exportamos la escena");
                scriptInstance.ResetScene();
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(scriptInstance.SubSceneController.gameObject);
                    EditorUtility.SetDirty(target);
                    serializedObject.ApplyModifiedProperties();
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                }
                scriptInstance.LoadMapAndGenerateLevel(scriptInstance.exportInPrefabMode);
                sceneGenerated = scriptInstance.GenerateScene();
                /*if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                    serializedObject.ApplyModifiedProperties();
                }*/
            }
            else
            {
                EditorUtility.DisplayDialog("Export info", "La escena no ha cambiado", "OK");
            }
            //scriptInstance.CreatingSubScene()
        }
        GUI.contentColor = c;
        foldout = EditorGUILayout.Foldout(foldout, "Solo para programación");
        if(foldout)
        {
            if (GUILayout.Button(new GUIContent("Save hash", "Guarda el hash del tiled para ver si es necesario recargar")))
            {
                string hash = scriptInstance.SaveHash();
                Debug.Log("Guardamos el hash " + hash);
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                    serializedObject.ApplyModifiedProperties();
                }
            }

            if (!sceneGenerated && GUILayout.Button("Select Scene Path Directory"))
            {
                string folder = EditorUtility.OpenFolderPanel("Save level scene", _path.stringValue, PATH);
                serializedObject.Update();
                //corregimos para que no meta rutas absolutas
                string[] folderSplit = folder.Split("Assets");
                _path.stringValue = "Assets" + folderSplit[1];
                serializedObject.ApplyModifiedProperties();
            }
        }
        

        if (!sceneGenerated)
        {
            
            DrawDefaultInspector();
        }
    }
}
