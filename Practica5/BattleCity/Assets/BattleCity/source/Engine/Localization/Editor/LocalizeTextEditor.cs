using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizeText))]
[CanEditMultipleObjects]
public class LocalizeTextEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button(new GUIContent("Refresh List")))
        {
            LocalizeInspectorStr.RefreshList();
        }
    }
}
