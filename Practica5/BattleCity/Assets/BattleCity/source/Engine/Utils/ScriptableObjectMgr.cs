using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Utilidad para simplificar la carga y el almacenamiento de ScriptableObjects.
/// </summary>
public static class ScriptableObjectMgr
{
    public const string DEFAULT_SO_SAVE = "Assets/Resources/";

    public static T Load<T>(string name) where T : ScriptableObject
    {
        var t = Resources.Load<T>(name);
        if (t == null)
        {
            Debug.Log("Error to Load resource" + typeof(T) + " " + name + ".");
        }
        return t;
    }

    public static bool TryToLoad<T>(string name, out T o_t) where T : ScriptableObject
    {
        var t = Resources.Load<T>(name);
        o_t = t;
        return t != null;
    }

    public static bool Exist<T>(string name) where T : ScriptableObject
    {
        var t = Resources.Load<T>(name);
        return t != null;
    }
#if UNITY_EDITOR
    public static void Save<T>(T t, string path = "") where T : ScriptableObject
    {
        t.hideFlags = HideFlags.None;
        if (!AssetDatabase.Contains(t))
        {
            if (path != "")
                AssetDatabase.CreateAsset(t, path);
            else
                AssetDatabase.CreateAsset(t, DEFAULT_SO_SAVE + t.name + ".asset");
        }
        else
        {
            EditorUtility.SetDirty(t);
            AssetDatabase.SaveAssets();
        }
        AssetDatabase.SaveAssets();
    }
#endif
}
