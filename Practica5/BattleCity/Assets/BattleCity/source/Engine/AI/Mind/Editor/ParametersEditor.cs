using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(Parameters), true)]
public class ParametersEditor : Editor
{
    SerializedProperty behaviorLinked;
    

    void OnEnable()
    {
        // Setup the SerializedProperties
        behaviorLinked = serializedObject.FindProperty("behaviorLinked");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Parameters parameters = (Parameters)target;
        FSMParameters[] fsmParameters = parameters.gameObject.GetComponents<FSMParameters>();
        if (fsmParameters.Length > 0)
        {
            string[] values = new string[fsmParameters.Length];
            int index = -1;

            for (int i = 0; i < fsmParameters.Length; ++i)
            {
                if (fsmParameters[i].GetType() != parameters.GetType())
                    values[i] = fsmParameters[i].GetType().Name;
                else
                    values[i] = "<none>";

                if (values[i] == behaviorLinked.stringValue)
                {
                    index = i;
                }
            }
            if (index < 0)
                index = 0;
            index = EditorGUILayout.Popup("Petenece a",index, values);
            behaviorLinked.stringValue = values[index];
        }
        else
            behaviorLinked.stringValue = "<none>";
        serializedObject.ApplyModifiedProperties();
        DrawDefaultInspector();
    }
}
