using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LocalizationConfig))]
public class LocalizationConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.DrawDefaultInspector();
        EditorGUILayout.Space();
#if UNITY_EDITOR_OSX
        EditorGUILayout.HelpBox("Debes guardar (Command+S) para que se refresquen los cambios en el localizador", MessageType.Warning);
#else
        EditorGUILayout.HelpBox("Debes guardar (Control+S) para que se refresquen los cambios en el localizador", MessageType.Warning);
#endif
        //this.serializedObject();
    }
}
