using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
public class ReadOnlyInspectorPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
