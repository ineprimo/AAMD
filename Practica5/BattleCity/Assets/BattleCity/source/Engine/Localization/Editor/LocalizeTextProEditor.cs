using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizeTextPro))]
[CanEditMultipleObjects]
public class LocalizeTextProEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LocalizeTextPro obj = (LocalizeTextPro)target;

        if (GUILayout.Button(new GUIContent("Refresh List")))
        {
            LocalizeInspectorStr.RefreshList();
        }
        

        if (obj.preferKey)
        {
            if (!LocalizeInspectorStr.ExistKey(obj.key))
                EditorGUILayout.HelpBox("Clave (" + obj.key + ") no encontrada ", MessageType.Warning);
            else
            {
                EditorGUILayout.HelpBox("La clave (" + obj.key + ") existe", MessageType.Info);
                obj.texto.key = obj.key;
                obj.dirty++;
            }
        }
    }
}
