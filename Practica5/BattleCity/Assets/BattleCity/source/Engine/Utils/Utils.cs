using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
//using UnityEditor;
public class _
{
    public static float sqr(float x) { return x* x; }
    public static float sum(float x) { return x + x; }

    public static void div(ref float[] x, float d)
    {
        for (int i = 0; i < x.Length; i++)
        {
            x[i] /= d;
        }
    }

    public static void sum(ref float[] x, float d)
    {
        for (int i = 0; i < x.Length; i++)
        {
            x[i] += d;
        }
    }

    public static void minus(ref float[] x, float d)
    {
        for (int i = 0; i < x.Length; i++)
        {
            x[i] -= d;
        }
    }

    public static void mul(ref float[] x, float d)
    {
        for (int i = 0; i < x.Length; i++)
        {
            x[i] *= d;
        }
    }

    public static void mul(float d,ref float[] x)
    {
        for (int i = 0; i < x.Length; i++)
        {
            x[i] *= d;
        }
    }

    public static void div(float d, ref float[] x)
    {
        for (int i = 0; i < x.Length; i++)
        {
            x[i] /= d;
        }
    }

    public static bool CNumber(float a, float n, float precission = 0.01f)
    {
        return Between(a, n-precission, n+precission);
    }
    public static bool CZero(float a, float precission = 0.01f)
    {
        return Between(a, -precission, precission);
    }

    public static bool Between(float a, float left, float right)
    {
        return a > left && a < right;
    }

    public static void sum(float d, ref float[] x)
    {
        for (int i = 0; i < x.Length; i++)
        {
            x[i] += d;
        }
    }

    public static void minus(float d, ref float[] x)
    {
        for (int i = 0; i < x.Length; i++)
        {
            x[i] -= d;
        }
    }

    public static float[] mul(float d, float[] x)
    {
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] * d;
        }
        return r;
    }

    public static float[] div(float d, float[] x)
    {
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] / d;
        }
        return r;
    }

    public static float[] sum(float d, float[] x)
    {
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] + d;
        }
        return r;
    }

    public static float[] minus(float d, float[] x)
    {
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] - d;
        }
        return r;
    }


    public static float[] mul(float[] x, float[] y)
    {
        if (x.Length != y.Length)
        {
            UnityEngine.Debug.LogError("x and y must be the same size");
            return null;
        }
            
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] * y[i];
        }
        return r;
    }

    public static float[] div(float[] x, float[] y)
    {
        if (x.Length != y.Length)
        {
            UnityEngine.Debug.LogError("x and y must be the same size");
            return null;
        }
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] / y[i];
        }
        return r;
    }

    public static float[] sum(float[] x, float[] y)
    {
        if (x.Length != y.Length)
        {
            UnityEngine.Debug.LogError("x and y must be the same size");
            return null;
        }
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] + y[i];
        }
        return r;
    }

    public static float[] minus(float[] x, float[] y)
    {
        if (x.Length != y.Length)
        {
            UnityEngine.Debug.LogError("x and y must be the same size");
            return null;
        }
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] - y[i];
        }
        return r;
    }

    public static float[] AcumulateProb(float[] x)
    {
        if (x == null || x.Length == 0)
            return x;
        float[] r = new float[x.Length];
        float s = sum(x);
        div(ref x, s);
        r[0] = x[0];
        for (int i = 1; i < x.Length; i++)
        {
            r[i] = x[i] + r[i - 1];
        }
        return r;
    }


    public static float[] mul(float[] x, float d)
    {
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] * d;
        }
        return r;
    }

    public static float[] div(float[] x, float d)
    {
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] / d;
        }
        return r;
    }

    public static float[] sum(float[] x, float d)
    {
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] + d;
        }
        return r;
    }

    public static float[] minus(float[] x, float d)
    {
        float[] r = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            r[i] = x[i] - d;
        }
        return r;
    }

    public static float sum(float[] x) 
    {
        float s = 0f;
        for(int i = 0; i < x.Length; i++)
        {
            s += x[i];
        }
        return s; 
    }

    public static float minus(float x) { return x - x; }

    public static float minus(float[] x)
    {
        float s = 0f;
        for (int i = 0; i < x.Length; i++)
        {
            s -= x[i];
        }
        return s;
    }
    public static bool par(int x) { return (x % 2) == 0; }
}
public class Utils
{
    [System.Serializable]
    public class SerializePairStr
    {
        public string key1;
        public string key2;
    }

    [System.Serializable]
    public class SerializeIntIDValue
    {
        public int hashID;
        public string val;
    }

    public static T SelectRandom<T>(T[] t , out int pos) where T : Component
    {
        pos = UnityEngine.Random.Range(0, t.Length);
        return t[pos];
    }

    public static void Destroy<T>(HashSet<GameObject> exceptions) where T : Component
    {
        T[] projectiles = UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None);
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (exceptions != null)
            {
                if (!exceptions.Contains(projectiles[i].gameObject))
                {
                    GameMgr.Instance.GetSpawnerMgr().DestroyGameObject(projectiles[i].gameObject);
                }
            }
            else
                GameMgr.Instance.GetSpawnerMgr().DestroyGameObject(projectiles[i].gameObject);
        }
    }

    public static void DestroyByTag(string tag, HashSet<GameObject> exceptions)
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < projectiles.Length; i++)
        {
            if(exceptions != null)
            {
                if(!exceptions.Contains(projectiles[i]))
                {
                    GameMgr.Instance.GetSpawnerMgr().DestroyGameObject(projectiles[i]);
                }
            }
            else
                GameMgr.Instance.GetSpawnerMgr().DestroyGameObject(projectiles[i]);
        }
    }

    

    public static string ConvertIntArrayToString(int[] arr,string delimeterToken="@")
    {
        string finalStr = "";
        for(int i = 0; i < arr.Length; i++)
        {
            string s = arr[i].ToString();
            finalStr += s + delimeterToken;
        }
        finalStr=finalStr.Remove(finalStr.Length - 1);
        return finalStr;
    }

    public static int[] ConvertStringToIntArray(string strArr, string delimeterToken = "@")
    {
        string[] tokens = strArr.Split(delimeterToken);
        int[] arr = new int[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
        {
            arr[i] = int.Parse(tokens[i]);
        }
        return arr;
    }

    public static string ConvertStringArrayToString(string[] arr, string delimeterToken = "@")
    {
        string finalStr = "";
        for (int i = 0; i < arr.Length; i++)
        {
            string s = arr[i].ToString();
            finalStr += s + delimeterToken;
        }
        if(finalStr != "")
            finalStr = finalStr.Remove(finalStr.Length - 1);
        return finalStr;
    }


    public static string[] ConvertStringToStringArray(string strArr, string delimeterToken = "@")
    {
        string[] tokens = strArr.Split(delimeterToken);
        string[] arr = new string[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
        {
            arr[i] = tokens[i];
        }
        return arr;
    }

    public static string ConvertFloatArrayToString(float[] arr, string delimeterToken = "@")
    {
        string finalStr = "";
        for (int i = 0; i < arr.Length; i++)
        {
            string s = arr[i].ToString();
            finalStr += s + delimeterToken;
        }
        finalStr = finalStr.Remove(finalStr.Length - 1);
        return finalStr;
    }

    public static float[] ConvertStringToFloatArray(string strArr, string delimeterToken = "@")
    {
        string[] tokens = strArr.Split(delimeterToken);
        float[] arr = new float[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
        {
            arr[i] = float.Parse(tokens[i], System.Globalization.CultureInfo.InvariantCulture);
        }
        return arr;
    }


    public static string ConvertBoolArrayToString(bool[] arr, string delimeterToken = "@")
    {
        string finalStr = "";
        for (int i = 0; i < arr.Length; i++)
        {
            string s = arr[i].ToString();
            finalStr += s + delimeterToken;
        }
        finalStr = finalStr.Remove(finalStr.Length - 1);
        return finalStr;
    }

    public static bool[] ConvertstringToBoolArray(string strArr, string delimeterToken = "@")
    {
        string[] tokens = strArr.Split(delimeterToken);
        bool[] arr = new bool[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
        {
            arr[i] = bool.Parse(tokens[i]);
        }
        return arr;
    }

    public static string TranslateUnityTypes(System.Type t)
    {
        if (t == typeof(System.Single))
            return "float";
        if(t == typeof(System.Int32))
            return "int";

        if (t == typeof(System.Boolean))
            return "boolean";

        return t.ToString();
    }

    public static void ApplyPhysicLayerInDepth(GameObject go, int layer)
    {
        if (go == null)
            return;
        go.layer = layer;
        for(int i = 0; i < go.transform.childCount; i++)
        {
            if(go.transform.GetChild(i) != null)
                ApplyPhysicLayerInDepth(go.transform.GetChild(i).gameObject, layer);
        }
    }

    public static Vector3 RotateVector2ExeYZ(Vector3 v3, float ro)
    {
        Vector3 res = Vector3.zero;
        ro = ro * Mathf.Deg2Rad;

        res.x = v3.x;
        res.y = v3.y * Mathf.Cos(ro) - v3.z * Mathf.Sin(ro);
        res.z = v3.y * Mathf.Sin(ro) + v3.z * Mathf.Cos(ro);
        return res;
    }

    public static Vector3 RotateVector2(Vector2 v2, float ro, float z)
    {
        ro = ro * Mathf.Deg2Rad;
        Vector3 res = Vector2.zero;
        res.x = v2.x * Mathf.Cos(ro) - v2.y * Mathf.Sin(ro);
        res.y = v2.x * Mathf.Sin(ro) + v2.y * Mathf.Cos(ro);
        res.z = z;
        return res;
    }
    public Vector2 RotateVector2(Vector2 v2, float ro)
    {
        Vector2 res = Vector2.zero;
        res.x = v2.x * Mathf.Cos(ro) - v2.y * Mathf.Sin(ro);
        res.y = v2.x * Mathf.Sin(ro) + v2.y * Mathf.Cos(ro);
        return res;
    }
    public enum GroupingType { VERTICAL, HORIZONTAL, BOTH }

    /*public static GameObject LoadPrefabFromFile(string path)
    {
        path += ".prefab";

        GameObject toInstantiate = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

        return toInstantiate;
    }*/



    public static void DebugPosition(Vector3 position, float scale, string name)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.transform.position = position;
        sphere.name = name;
    }

    public static MonoBehaviour[] GetAllComponents(GameObject go)
    {
        MonoBehaviour[] children = go.GetComponentsInChildren<MonoBehaviour>();
        return children;
    }


    public static T GetAttribute<T>(object[] attributtes) where T : System.Attribute
    {
        for (int i = 0; i < attributtes.Length; ++i)
        {
            if (attributtes[i].GetType().Equals(typeof(T)))
                return (T)attributtes[i];
        }
        return null;
    }

    public static List<T> GetAllCustomAttribute<T>(GameObject go) where T : System.Attribute
    {
        MonoBehaviour[] components = GetAllComponents(go);
        List<T> attrList = new List<T>();
        for (int i = 0; i < components.Length; i++)
        {
            object[] attributtes = components[i].GetType().GetCustomAttributes(true);
            T atribute = GetAttribute<T>(attributtes);
            if (atribute != null)
            {
                attrList.Add(atribute);
            }
        }
        return attrList;
    }

    public static List<MonoBehaviour> GetComponentsWithCustomAttribute<T>(GameObject go) where T : System.Attribute
    {
        MonoBehaviour[] components = GetAllComponents(go);
        List<MonoBehaviour> componentList = new List<MonoBehaviour>();
        for (int i = 0; i < components.Length; i++)
        {
            object[] attributtes = components[i].GetType().GetCustomAttributes(true);
            T atribute = GetAttribute<T>(attributtes);
            if (atribute != null)
            {
                componentList.Add(components[i]);
            }
        }
        return componentList;
    }

    public static T[] GetAllCustomAttributeArr<T>(GameObject go) where T : System.Attribute
    {
        List<T> attrList = GetAllCustomAttribute<T>(go);
        return attrList.ToArray();
    }

    public static List<T> GetFieldCustomAttributes<T>(MonoBehaviour component) where T : System.Attribute
    {
        List<T> attrList = new List<T>();
        FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < fields.Length; ++i)
        {
            FieldInfo fieldInfo = fields[i];
            object[] attributtes = fieldInfo.GetCustomAttributes(true);
            T fieldTileMapAsign = Utils.GetAttribute<T>(attributtes);
            if(fieldTileMapAsign != null)
                attrList.Add(fieldTileMapAsign);
        }
        return attrList;
    }

    public static List<Pair<T, FieldInfo>> GetFieldCustomAttributesPair<T>(MonoBehaviour component) where T : System.Attribute
    {
        List<Pair<T,FieldInfo >> attrList = new List<Pair<T, FieldInfo>>();
        FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < fields.Length; ++i)
        {
            FieldInfo fieldInfo = fields[i];
            object[] attributtes = fieldInfo.GetCustomAttributes(true);
            T fieldTileMapAsign = Utils.GetAttribute<T>(attributtes);
            if (fieldTileMapAsign != null)
                attrList.Add(new Pair<T, FieldInfo>(fieldTileMapAsign, fieldInfo));
        }
        return attrList;
    }

    public static List<T> GetFieldCustomAttributes<T>(ref List<T> attrList, MonoBehaviour component) where T : System.Attribute
    {
        if (attrList == null)
        {
            attrList = new List<T>();
        }
        FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < fields.Length; ++i)
        {
            FieldInfo fieldInfo = fields[i];
            object[] attributtes = fieldInfo.GetCustomAttributes(true);
            T fieldTileMapAsign = Utils.GetAttribute<T>(attributtes);
            if (fieldTileMapAsign != null)
                attrList.Add(fieldTileMapAsign);
        }
        return attrList;
    }

    public static T[] GetFieldCustomAttributesArr<T>(MonoBehaviour component) where T : System.Attribute
    {
        List<T> attrList = GetFieldCustomAttributes<T>(component);
        return attrList.ToArray();
    }


    public static void GetAllCustomAttribute<T>(ref List<T> attrList,GameObject go) where T : System.Attribute
    {
        if(attrList == null)
        {
            attrList = new List<T>();
        }
        MonoBehaviour[] components = GetAllComponents(go);
        for (int i = 0; i < components.Length; i++)
        {
            object[] attributtes = components[i].GetType().GetCustomAttributes(true);
            T atribute = GetAttribute<T>(attributtes);
            if (atribute != null)
            {
                attrList.Add(atribute);
            }
        }
    }

    public static void RevertPrefabIfIsPossible(GameObject go)
    {
#if UNITY_EDITOR
        if (PrefabUtility.IsPartOfAnyPrefab(go))
        {
            GameObject prefabRoot = PrefabUtility.GetNearestPrefabInstanceRoot(go);
            try
            {
                if(PrefabUtility.HasPrefabInstanceAnyOverrides(prefabRoot,false))
                    UnityEngine.Debug.LogError("Cuidado, hemos revertido el prefab de " + prefabRoot +
                        " este prefab no debe tener cambios ya que serán revertidos en la exportación final. Los cambios en el prefab peuden ser sintoma de que se " +
                        "ha guardado una escena parcialmente, revierte a mano este prefab para evitar mostrar este error en lo sucesivo");
                PrefabUtility.RevertPrefabInstance(prefabRoot, InteractionMode.AutomatedAction);
                    
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
#endif
    }

    public static bool UnpackPrefabIfIsPossible(GameObject go)
    {
        bool unpack = false;
#if UNITY_EDITOR
        if (PrefabUtility.IsPartOfAnyPrefab(go))
        {
            GameObject prefabRoot = PrefabUtility.GetNearestPrefabInstanceRoot(go);
            try
            {
                PrefabUtility.UnpackPrefabInstance(prefabRoot, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                unpack = true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Error al intentar hacer un unpack de "+go.name);
                unpack = false;
            }
        }
#endif
        return unpack;
    }



    public static bool ArrivedAtPoint(Vector3 entity, Vector3 destination, float threshold)
    {
        return ArrivedAtPoint((destination - entity).normalized, entity, destination, threshold);
    }



    public static bool ArrivedAtPoint(Vector3 moveDirection, Vector3 entity, Vector3 destination, float threshold)
    {
        Vector3 vectorDist = destination - entity;

        if(vectorDist.sqrMagnitude < threshold * threshold)
        {
            return true;
        }
        else
        {
            Vector3 simulatePoint = entity + moveDirection * 2*threshold;
            Vector3 vectorDistSimulate = destination - simulatePoint;

            if (vectorDistSimulate.sqrMagnitude < vectorDist.sqrMagnitude)
                return false;
            else // se ha pasado del punto porque la distancia es mayor.
            {
                return true;
            }
        }
    }

    /*public static bool ArrivedAtPoint(Vector3 moveDirection, Vector3 entity, Vector3 destination, float threshold)
    {
        Vector3 vectorDist = destination - entity;
        UnityEngine.Debug.Log("[ArrivedAtPoint] " + moveDirection);
        if (vectorDist.sqrMagnitude < threshold * threshold)
        {
            return true;
        }
        else
        {
            Vector3 simulatePoint = entity + moveDirection * 2*threshold;
            Vector3 vectorDistSimulate = simulatePoint - entity;
            UnityEngine.Debug.Log("[ArrivedAtPoint] vectorDistSimulate " + vectorDistSimulate.sqrMagnitude + " vectorDist "+ vectorDist.sqrMagnitude);
            if (vectorDistSimulate.sqrMagnitude < vectorDist.sqrMagnitude)
                return false;
            else // se ha pasado del punto porque la distancia es mayor.
            {
                return true;
            }
        }
    }*/

    public static Vector3 IncPosition(Vector3 position, float xs, float ys)
    {
        return new Vector3(position.x + xs, position.y + ys, position.z);
    }

    public static Vector3 IncPosition(Vector3 position, float xs, float ys, float zs)
    {
        return new Vector3(position.x + xs, position.y + ys, position.z + zs);
    }

    /*public static bool HaveArrivedAtPoint(Vector3 entity, Vector3 destination, float threshold)
    {
        Vector3 vectorDist = destination - entity;
        return vectorDist.sqrMagnitude < threshold * threshold;
    }*/


    public static Vector3 GetDirection(Transform transform1, Transform transform2)
    {
        return (transform1.position - transform2.position);
    }

    public static Vector3 GetDirectionNormalized(Transform transform1, Transform transform2)
    {
        return GetDirection(transform1, transform2).normalized;
    }


    public static T GetSecureComponent<T>(MonoBehaviour m, ref T ob) where T : Component
    {
        if (ob == null)
            ob = m.GetComponent<T>();
        return ob;
    }

    public static GameObject[] GetPlayers()
    {
        return GameObject.FindGameObjectsWithTag(Globals.PlayerTag);
    }

    public static T GetSecureStaticGet<T>(ref T t, string tag) where T : MonoBehaviour
    {
        if (t == null)
            t = Utils.CreateStaticGet<T>(tag);
        return t;
    }

    public static T GetSecureComponent<T>(ref T ob) where T : Component
    {
        if (ob == null)
            ob = GameObject.FindFirstObjectByType<T>();
        return ob;
    }

    public static void ThrowEventOneShot(ref System.Action action)
    {
        action?.Invoke();
        action = null;
    }

    public static Color ConvertStringColorToUnityColor(string color)
    {
        string c = color.ToLower();
        switch (c)
        {
            case "white":
                return Color.white;
            case "yellow":
                return Color.yellow;
            case "grey":
                return Color.grey;
            case "magenta":
                return Color.magenta;
            case "cyan":
                return Color.cyan;
            case "red":
                return Color.red;
            case "blue":
                return Color.blue;
            case "green":
                return Color.green;
            default:
                return Color.black;
        }
    }

    public static bool CheckGameObjectIsIn(Collider[] colliders, GameObject go)
    {
        bool found = false;
        for(int i=0; !found && i < colliders.Length; ++i)
        {
            found = colliders[i].gameObject == go;
        }
        return found;
    }

    public static float LinearRangeInv(float v, float min, float max)
    {
        float a = 1 / (max - min);
        float b = -a * min;
        return Rect(v, a, b);
    }
    public static float Rect(float x, float a,float b)
    {
        return a * x + b;
    }

    public static Vector2 V3ToV2(Vector3 position)
    {
        return new Vector2(position.x, position.y);
    }

    public static Vector2 V3ToV2InZY(Vector3 position)
    {
        return new Vector2(position.z, position.y);
    }

    public static Vector2 V2ToV3InZY(Vector2 position, float x)
    {
        return new Vector3(x,position.y, position.x);
    }

    public static float LinearRange(float value, float max, float min)
    {
        return value * (max - min) + min;
    }

     public static bool IsOverlappingCapsule(Vector3 position, Vector3 center, float height, float radius, params int[] layer)
    {
        Collider[] colliders = Utils.OverlapCapsule(position, center, height, radius, layer);
        return colliders != null && colliders.Length > 0;
    }


    public static T GetNearestGeneric<T>(T[] targets, GameObject gameobject, out float sqrDistance) where T : Component
    {
        sqrDistance = float.PositiveInfinity;
        if (targets == null || targets.Length < 1)
            return null;

        T nearest = targets[0];
        sqrDistance = Vector3.SqrMagnitude(targets[0].transform.position - gameobject.transform.position);
        for (int i = 1; i < targets.Length; ++i)
        {
            float sqrDistanceLocal = Vector3.SqrMagnitude(targets[i].transform.position - gameobject.transform.position);
            if (sqrDistanceLocal < sqrDistance)
            {
                sqrDistance = sqrDistanceLocal;
                nearest = targets[i];
            }
        }

        return nearest;
    }
    public static Transform GetNearest(GameObject[] targets, GameObject gameobject)
    {
        if (targets == null || targets.Length < 1)
            return null;

        Transform nearest = targets[0].transform;
        float sqrDistance = Vector3.SqrMagnitude(targets[0].transform.position - gameobject.transform.position);
        for (int i = 1; i < targets.Length; ++i)
        {
            float sqrDistanceLocal = Vector3.SqrMagnitude(targets[i].transform.position - gameobject.transform.position);
            if (sqrDistanceLocal < sqrDistance)
            {
                sqrDistance = sqrDistanceLocal;
                nearest = targets[i].transform;
            }
        }

        return nearest;
    }

    public static Transform GetNearestPoint(List<Transform> targets, Vector3 currentPosition, HashSet<Transform> ignored)
    {
        if (targets == null || targets.Count < 1)
            return null;

        float sqrDistanceNeares =float.PositiveInfinity;
        int nearest = -1;
        for (int i = 0; i < targets.Count; ++i)
        {
            if(ignored == null || !ignored.Contains(targets[i]))
            {
                float sqrDistanceLocal = Vector3.SqrMagnitude(targets[i].position - currentPosition);
                if (sqrDistanceLocal < sqrDistanceNeares)
                {
                    sqrDistanceNeares = sqrDistanceLocal;
                    nearest = i;
                }
            }
        }
        if (nearest >= 0)
            return targets[nearest];
        else
            return null;
    }

    /// <summary>
    /// Nos da el indice de colliders donde se encuentra el primer collider del tipo layer
    /// </summary>
    /// <param name="colliders">array de colliders a consultar</param>
    /// <param name="layer">el identificador de la layer</param>
    /// <returns></returns>
    public static int GetColliderLayerIndex(Collider[] colliders, int layer, bool collisionWithTriggers = false)
    {
        int index = -1;
        for(int i =0; index < 0 && i < colliders.Length; ++i)
        {
            if (colliders[i].gameObject.layer == layer && colliders[i].isTrigger == collisionWithTriggers)
                index = i;
        }
        return index;
    }

    public static int GetColliderTagIndex(Collider[] colliders, string t, bool collisionWithTriggers = false)
    {
        int index = -1;
        for (int i = 0; index < 0 && i < colliders.Length; ++i)
        {
            if (colliders[i].gameObject.tag == t && colliders[i].isTrigger == collisionWithTriggers)
                index = i;
        }
        return index;
    }

    public static Collider[] GetOverlappingCapsule(Vector3 position, Vector3 center, float height, float radius, params int[] layer)
    {
        Collider[] colliders = Utils.OverlapCapsule(position, center, height, radius, layer);
        return colliders;
    }

    public static int MakeLayerMask(params int[] layer)
    {
        int mask = 0;
        for (int i = 0; i < layer.Length; ++i)
        {
            mask |= (1 << layer[i]);
        }
        return mask;
    }

    public static Vector3 CalcMidelPoint(Vector3 position1, Vector3 position2)
    {
        Vector3 dir = position1 - position2;
        dir = dir / 2;
        return position1 + dir;
    }

    public static Collider[] OverlapCapsule(Vector3 position, Vector3 center, float height, float radius, params int[] layer)
    {
        Vector3 p1 = position + center + Vector3.up * - height * 0.5F;
        Vector3 p2 = p1 + Vector3.up * height;
        int mask = MakeLayerMask(layer);
        return Physics.OverlapCapsule(p1, p2, radius, mask);
    }

    public static Collider[] OverlapSphere(Vector3 position, float radius, params int[] layer)
    {
        int mask = MakeLayerMask(layer);
        return Physics.OverlapSphere(position, radius, mask);
    }

    public static T OverlapSphere<T>(Vector3 position, float radius, params int[] layer) where T : MonoBehaviour
    {
        int mask = MakeLayerMask(layer);
        Collider[] colliders = Physics.OverlapSphere(position, radius, mask);
        for (int i = 0; i < colliders.Length; i++)
        {
            T t = colliders[i].GetComponent<T>();
            if (t != null)
                return t;
        }
        return null;
    }

    public static Collider OverlapSphereTag(Vector3 position, float radius, string tag, params int[] layer)
    {
        int mask = MakeLayerMask(layer);
        Collider[] colliders = Physics.OverlapSphere(position, radius, mask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == tag)
                return colliders[i];
        }
        return null;
    }

    public static bool SphereRaycast(Vector3 position, float radius, Vector3 direction, float dist, params int[] layer)
    {
        int mask = MakeLayerMask(layer);
        Ray r = new Ray(position, direction);
        return Physics.SphereCast(r, radius, dist, mask);
    }


    public static T CreateStaticGet<T>(string tag) where T : MonoBehaviour
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
        if (gos == null || gos.Length != 1)
        {
            return null;
        }
        else
        {
            return gos[0].GetComponent<T>();
        }
    }

    public static T CreateStaticGet<T>() where T : MonoBehaviour
    {
        T gos = GameObject.FindFirstObjectByType<T>();
        return gos;
    }






    public static Vector3 CalcCentroide<T>(List<T> goList, out Vector3 size) where T : Component
    {
        Vector3 centroid = Vector3.zero;
        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        float yMin = float.MaxValue;
        float yMax = float.MinValue;
        float zMin = float.MaxValue;
        float zMax = float.MinValue;
        for (int i = 0; i < goList.Count; ++i)
        {
            centroid = centroid + goList[i].transform.position;
            if (goList[i].transform.position.x < xMin)
                xMin = goList[i].transform.position.x;
            if (goList[i].transform.position.x > xMax)
                xMax = goList[i].transform.position.x;

            if (goList[i].transform.position.y < yMin)
                yMin = goList[i].transform.position.y;
            if (goList[i].transform.position.y > yMax)
                yMax = goList[i].transform.position.y;

            if (goList[i].transform.position.z < zMin)
                zMin = goList[i].transform.position.z;
            if (goList[i].transform.position.z > zMax)
                zMax = goList[i].transform.position.z;
        }

        size.x = xMax - xMin;
        size.y = yMax - yMin;
        size.z = zMax - zMin;

        centroid = centroid / goList.Count;
        return centroid;
    }

    

    

    public static Vector3 CalcCentroide(List<GameObject> goList, out Vector3 size)
    {
        Vector3 centroid = Vector3.zero;
        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        float yMin = float.MaxValue;
        float yMax = float.MinValue;
        float zMin = float.MaxValue;
        float zMax = float.MinValue;
        for (int i = 0; i < goList.Count; ++i)
        {
            centroid = centroid + goList[i].transform.position;
            if (goList[i].transform.position.x < xMin)
                xMin = goList[i].transform.position.x;
            if (goList[i].transform.position.x > xMax)
                xMax = goList[i].transform.position.x;

            if (goList[i].transform.position.y < yMin)
                yMin = goList[i].transform.position.y;
            if (goList[i].transform.position.y > yMax)
                yMax = goList[i].transform.position.y;

            if (goList[i].transform.position.z < zMin)
                zMin = goList[i].transform.position.z;
            if (goList[i].transform.position.z > zMax)
                zMax = goList[i].transform.position.z;
        }

        size.x = xMax - xMin;
        size.y = yMax - yMin;
        size.z = zMax - zMin;

        centroid = centroid / goList.Count;
        return centroid;
    }

    public static Vector3 CalcCentroide(List<Vector3> vecList, out Vector3 size)
    {
        Vector3 centroid = Vector3.zero;
        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        float yMin = float.MaxValue;
        float yMax = float.MinValue;
        float zMin = float.MaxValue;
        float zMax = float.MinValue;
        for (int i = 0; i < vecList.Count; ++i)
        {
            centroid = centroid + vecList[i];
            if (vecList[i].x < xMin)
                xMin = vecList[i].x;
            if (vecList[i].x > xMax)
                xMax = vecList[i].x;

            if (vecList[i].y < yMin)
                yMin = vecList[i].y;
            if (vecList[i].y > yMax)
                yMax = vecList[i].y;

            if (vecList[i].z < zMin)
                zMin = vecList[i].z;
            if (vecList[i].z > zMax)
                zMax = vecList[i].z;
        }

        size.x = xMax - xMin;
        size.y = yMax - yMin;
        size.z = zMax - zMin;

        centroid = centroid / vecList.Count;
        return centroid;
    }

    public static Vector2 CalcCentroide(List<GameObject> goList, out Vector2 size)
    {
        Vector2 centroid = Vector2.zero;
        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        float yMin = float.MaxValue;
        float yMax = float.MinValue;

        for (int i = 0; i < goList.Count; ++i)
        {
            centroid = centroid + (Vector2)goList[i].transform.position;
            if (goList[i].transform.position.x < xMin)
                xMin = goList[i].transform.position.x;
            if (goList[i].transform.position.x > xMax)
                xMax = goList[i].transform.position.x;

            if (goList[i].transform.position.y < yMin)
                yMin = goList[i].transform.position.y;
            if (goList[i].transform.position.y > yMax)
                yMax = goList[i].transform.position.y;

        }

        size.x = (xMax - xMin) +1;
        size.y = (yMax - yMin) +1;

        centroid = centroid / goList.Count;
        return centroid;
    }

    public static Vector2 CalcCentroide(out Vector2 size, params Vector3[] goList)
    {
        Vector2 centroid = Vector2.zero;
        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        float yMin = float.MaxValue;
        float yMax = float.MinValue;

        for (int i = 0; i < goList.Length; ++i)
        {
            centroid = centroid + (Vector2)goList[i];
            if (goList[i].x < xMin)
                xMin = goList[i].x;
            if (goList[i].x > xMax)
                xMax = goList[i].x;

            if (goList[i].y < yMin)
                yMin = goList[i].y;
            if (goList[i].y > yMax)
                yMax = goList[i].y;

        }

        size.x = xMax - xMin;
        size.y = yMax - yMin;

        centroid = centroid / goList.Length;
        return centroid;
    }

    public static List<Vector3> FindPositionsNear(List<Vector3> tilePositions, int tileSizeX, int tileSizeY, GroupingType groupingType)
    {
        List<Vector3> tileLine = new List<Vector3>();
        tileLine.Add(tilePositions[0]);
        tilePositions.RemoveAt(0);
        for (int i = 0; i < tileLine.Count; ++i)
        {
            int cellx = (int)tileLine[i].x / tileSizeX;
            int celly = (int)tileLine[i].y / tileSizeY;

            int j = 0;
            while (j < tilePositions.Count)
            {
                bool next = true;
                if (tilePositions[j] != tileLine[i])
                {
                    int nextCellx = (int)tilePositions[j].x / tileSizeX;
                    int nextCelly = (int)tilePositions[j].y / tileSizeY;
                    bool isAttachable = false;

                    if (groupingType == GroupingType.BOTH)
                        isAttachable = ((Mathf.Abs(cellx - nextCellx) == 1) && (Mathf.Abs(celly - nextCelly) == 0) ||
                            (Mathf.Abs(cellx - nextCellx) == 0) && (Mathf.Abs(celly - nextCelly) == 1));
                    else if (groupingType == GroupingType.HORIZONTAL)
                        isAttachable = (Mathf.Abs(cellx - nextCellx) == 1) && (Mathf.Abs(celly - nextCelly) == 0);
                    else if (groupingType == GroupingType.HORIZONTAL)
                        isAttachable = (Mathf.Abs(cellx - nextCellx) == 0) && (Mathf.Abs(celly - nextCelly) == 1);

                    if (isAttachable)
                    {
                        tileLine.Add(tilePositions[j]);
                        tilePositions.RemoveAt(j);
                        next = false;
                    }
                }
                if (next)
                    ++j;
            }
        }
        return tileLine;
    }

    public static int CircularInt(int value, int max)
    {
        int pos = value % max;
        if (pos < 0)
            return max + pos;
        else
            return pos;
    }

    public static List<Pair<GameObject, GameObject>> FindGraphicInPosition<T>(T[] parentElement, Vector3 size, AttachableToPhysic[] atachableObjects) where T : Component
    {
        List<Pair<GameObject, GameObject>> graphicelements = new List<Pair<GameObject, GameObject>>();
        for (int i = 0; i < parentElement.Length; ++i)
        {
            Vector3 pos = parentElement[i].transform.position;
            bool found = false;
            for(int j=0; j < atachableObjects.Length && !found; ++j)
            {
                float xDist = Mathf.Abs(pos.x - atachableObjects[j].transform.position.x);
                float yDist = Mathf.Abs(pos.y - atachableObjects[j].transform.position.y);
                if ((xDist+yDist) < 0.05f)
                {
                    found = true;
                    graphicelements.Add(new Pair<GameObject, GameObject>(parentElement[i].gameObject, atachableObjects[j].gameObject));
                }
            }
        }
        return graphicelements;
    }

    public static List<GameObject> FindPlatformsNear(List<GameObject> tileMovileList, int tileSizeX, int tileSizeY, GroupingType groupingType)
    {
        List<GameObject> platform = new List<GameObject>();
        platform.Add(tileMovileList[0]);
        tileMovileList.RemoveAt(0);
        for (int i = 0; i < platform.Count; ++i)
        {
            int cellx = (int)platform[i].transform.position.x / tileSizeX;
            int celly = (int)platform[i].transform.position.y / tileSizeY;

            int j = 0;
            while (j < tileMovileList.Count)
            {
                bool next = true;
                if (tileMovileList[j] != platform[i])
                {
                    int nextCellx = (int)tileMovileList[j].transform.position.x / tileSizeX;
                    int nextCelly = (int)tileMovileList[j].transform.position.y / tileSizeY;
                    bool isAttachable = false;

                    if (groupingType == GroupingType.BOTH)
                        isAttachable = ((Mathf.Abs(cellx - nextCellx) == 1) && (Mathf.Abs(celly - nextCelly) == 0) ||
                            (Mathf.Abs(cellx - nextCellx) == 0) && (Mathf.Abs(celly - nextCelly) == 1));
                    else if (groupingType == GroupingType.HORIZONTAL)
                        isAttachable = (Mathf.Abs(cellx - nextCellx) == 1) && (Mathf.Abs(celly - nextCelly) == 0);
                    else if (groupingType == GroupingType.HORIZONTAL)
                        isAttachable = (Mathf.Abs(cellx - nextCellx) == 0) && (Mathf.Abs(celly - nextCelly) == 1);

                    if (isAttachable)
                    {
                        platform.Add(tileMovileList[j]);
                        tileMovileList.RemoveAt(j);
                        next = false;
                    }
                }
                if (next)
                    ++j;
            }
        }
        return platform;
    }

    public static bool GetRaycastCollision(Vector3 initialPosition, Vector3 direction, float distance, out RaycastHit hit, int layerMask, string tag = "")
    {
        if (Physics.Raycast(new Ray(initialPosition, direction), out hit, distance,layerMask))
        {
            if(tag != "")
            {
                if (hit.collider.gameObject.tag == tag)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    public static GameObject SphereCastGetFirstGameObjectInDirection(Vector3 initialPosition,Vector3 direction, float distance, string tag)
    {
        RaycastHit hit;
        if(Physics.SphereCast(initialPosition,0.45f, direction, out hit,distance))
        {
            if (hit.collider.gameObject.tag == tag)
                return hit.collider.gameObject;
            else
                return null;
        }
        return null;
    }

    public static List<GameObject> SphereCastGetAllGameObjectInDirection(Vector3 initialPosition, Vector3 direction, float distance, string tag)
    {
        List<GameObject> goList = null;
        RaycastHit[] hits = Physics.SphereCastAll(initialPosition, 1f, direction, distance);
        if(hits != null)
        {
            for(int i = 0; i < hits.Length; ++i)
            {
                RaycastHit hit = hits[i];
                if (hit.collider.gameObject.tag == tag)
                {
                    if(goList == null)
                        goList = new List<GameObject>();
                    goList.Add(hit.collider.gameObject);
                }
            }
        }

        return goList;
    }

    public static GameObject GetFirstGameObjectInDirection(Vector3 initialPosition, Vector3 direction, float distance, int layer)
    {
        RaycastHit hit;
        if (Physics.Raycast(initialPosition, direction, out hit, distance, 1 << layer))
            return hit.collider.gameObject;
        return null;
    }

    public static List<GameObject> CombineMeshesWithFaces(GameObject source, Transform parentDestination, bool staticOnly, int vertexSize, int[] layers)
    {
        List<GameObject> gameObjectList = new List<GameObject>();
        GraphicPackable[] graphicsPackages = source.GetComponentsInChildren<GraphicPackable>();
        List<MeshFilter> meshFiltersList = new List<MeshFilter>();

        for(int c = 0; c < graphicsPackages.Length; ++c)
        {
            MeshFilter[] meshFilters = graphicsPackages[c].GetComponentsInChildren<MeshFilter>();
            foreach(MeshFilter mesh in meshFilters)
            {
                meshFiltersList.Add(mesh);
            }
        }

        Dictionary<int, CombineMeshInfo> combineInit = new Dictionary<int, CombineMeshInfo>();

        for (int c = 0; c < layers.Length; ++c)
        {
            combineInit.Add(layers[c], new CombineMeshInfo(meshFiltersList.Count));
        }

        int i = 0;
        while (i < meshFiltersList.Count)
        {
            GraphicPackable gp = meshFiltersList[i].transform.parent.GetComponent<GraphicPackable>();
            if (gp != null)
            {
                CombineMeshInfo meshInfo = combineInit[gp.LayerID];

                if (staticOnly)
                {
                    if (meshFiltersList[i].gameObject.isStatic)
                        CheckAndCombine(gameObjectList, meshFiltersList, meshInfo, i,
                            vertexSize, parentDestination);
                }
                else
                    CheckAndCombine(gameObjectList, meshFiltersList, meshInfo, i,
                        vertexSize, parentDestination);
            }
            ++i;
        }

        foreach (var keyValue in combineInit)
        {
            if (keyValue.Value._index > 0)
            {
                CombineMeshInfo meshInfo = keyValue.Value;
                Array.Resize<CombineInstance>(ref meshInfo._combineInstances, meshInfo._index);
                Combine(gameObjectList, meshInfo._material, meshInfo._combineInstances, parentDestination, meshInfo._position / meshInfo._index);
            }
        }

        return gameObjectList;
    }

    /**
     * public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes = true, 
     * bool useMatrices = true, bool hasLightmapData = false); 
    * Parameters
    *  - combine Descriptions of the Meshes to combine.
    *  - mergeSubMeshes Defines whether Meshes should be combined into a single sub-Mesh.
    *  - useMatrices Defines whether the transforms supplied in the CombineInstance array should be used or ignored.

        Description: Combines several Meshes into this Mesh. Combining Meshes is useful for performance optimization.
        
    If mergeSubMeshes is true, all the Meshes are combined to a single sub-Mesh. Otherwise, each Mesh goes into a different sub-Mesh. If all Meshes share the same Material, set this to true.

    If useMatrices is true, the transform matrices in CombineInstance structs are used. Otherwise, they are ignored.

    Set hasLightmapData to true to transform the input Mesh lightmap UV data by the lightmap scale offset data in CombineInstance structs. The Meshes must share the same lightmap texture.
        **/
    public static List<GameObject> CombineMeshes(GameObject source, Transform parentDestination, bool staticOnly, int vertexSize, int[] layers)
    {
        List<GameObject> gameObjectList = new List<GameObject>();
        MeshFilter[] meshFilters = source.GetComponentsInChildren<MeshFilter>();
        Dictionary<int, CombineMeshInfo> combineInit = new Dictionary<int, CombineMeshInfo>();//new CombineInstance[meshFilters.Length];
        
        for(int c = 0; c < layers.Length; ++c)
        {
            combineInit.Add(layers[c],new CombineMeshInfo(meshFilters.Length));
        }


        int i = 0;
        while (i < meshFilters.Length)
        {
            GraphicPackable gp = meshFilters[i].GetComponent<GraphicPackable>();
            if (gp != null)
            {

                CombineMeshInfo meshInfo = combineInit[gp.LayerID];
                
                if (staticOnly)
                {
                    if (meshFilters[i].gameObject.isStatic)
                        CheckAndCombine(gameObjectList, meshFilters, meshInfo, i,
                            vertexSize, parentDestination );
                }
                else
                    CheckAndCombine(gameObjectList, meshFilters, meshInfo, i,
                        vertexSize, parentDestination);
            }
            ++i;
        }
        foreach (var keyValue in combineInit)
        {
            if (keyValue.Value._index > 0)
            {
                CombineMeshInfo meshInfo = keyValue.Value;
                Array.Resize<CombineInstance>(ref meshInfo._combineInstances, meshInfo._index);
                Combine(gameObjectList, meshInfo._material, meshInfo._combineInstances, parentDestination, meshInfo._position / meshInfo._index);
            }
        }
        
        return gameObjectList;
    }

    public static void CombineMeshes(List<MeshFilter> meshFilters, Transform transform)
    {
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];

        int i = 0;
        while (i < meshFilters.Count)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            combine[i].subMeshIndex = i;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);
    }


    private static void CheckAndCombine(List<GameObject> gameObjectList, List<MeshFilter> meshFilters, CombineMeshInfo meshInfo,
        int i, int vertexSize, Transform parentDestination)
    {
        meshInfo._vertexAcum += meshFilters[i].mesh.vertexCount;
        meshInfo._position += meshFilters[i].transform.position;
        meshInfo._material = meshFilters[i].GetComponent<MeshRenderer>().material;
        MeshCombine(i, meshInfo._index, meshInfo._combineInstances, meshFilters);
        meshInfo._index++;

        if (meshInfo._vertexAcum > vertexSize)
        {
            System.Diagnostics.Debug.Assert(meshInfo._vertexAcum < 65500, "No se puede combinar mallas de mas de 65.500 vertices");
            System.Diagnostics.Debug.Assert(meshInfo._index > 0, "No hay mallas que combinar");
            Array.Resize<CombineInstance>(ref meshInfo._combineInstances, meshInfo._index);
            Combine(gameObjectList, meshInfo._material, meshInfo._combineInstances, parentDestination, meshInfo._position / meshInfo._index);
            meshInfo._combineInstances = new CombineInstance[meshFilters.Count - i];
            meshInfo._vertexAcum = 0;
            meshInfo._position = Vector3.zero;
            meshInfo._index = 0;
        }

    }

    private static void CheckAndCombine(List<GameObject> gameObjectList,MeshFilter[] meshFilters, CombineMeshInfo meshInfo,
        int i, int vertexSize, Transform parentDestination)
    {
        meshInfo._vertexAcum += meshFilters[i].mesh.vertexCount;
        meshInfo._position += meshFilters[i].transform.position;
        meshInfo._material = meshFilters[i].GetComponent<MeshRenderer>().material;
        MeshCombine(i, meshInfo._index, meshInfo._combineInstances, meshFilters);
        meshInfo._index++;

        if (meshInfo._vertexAcum > vertexSize)
        {
            System.Diagnostics.Debug.Assert(meshInfo._vertexAcum < 65500, "No se puede combinar mallas de mas de 65.500 vertices");
            System.Diagnostics.Debug.Assert(meshInfo._index > 0, "No hay mallas que combinar");
            Array.Resize<CombineInstance>(ref meshInfo._combineInstances, meshInfo._index);
            Combine(gameObjectList, meshInfo._material, meshInfo._combineInstances, parentDestination, meshInfo._position / meshInfo._index);
            meshInfo._combineInstances = new CombineInstance[meshFilters.Length - i];
            meshInfo._vertexAcum = 0;
            meshInfo._position = Vector3.zero;
            meshInfo._index = 0;
        }
  
    }

    public static void CombineMesh(GameObject go, MeshFilter[] meshFilters, Material material)
    {
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        MeshRenderer meshRender = go.AddComponent<MeshRenderer>();

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        meshFilter.sharedMesh = mesh;
        meshRender.material = material;
        go.transform.gameObject.SetActive(true);
        i = 0;
        mesh.UploadMeshData(true);
        while (i < meshFilters.Length)
        {
            UnityEngine.Object.Destroy(meshFilters[i].gameObject);
            i++;
        }
        //go.SetActive(true);
    }
    private static void Combine(List<GameObject> gameObjectList, Material material, CombineInstance[] combineInit, Transform parentDestination, Vector3 position, string GraphicPacketRefTag = "")
    {
        GameObject go = new GameObject("GrapchiTilePacket " + gameObjectList.Count);
        go.transform.position = position;
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        
        go.transform.position = Vector3.zero;
        go.GetComponent<MeshFilter>().mesh = new Mesh();
        go.GetComponent<MeshFilter>().mesh.CombineMeshes(combineInit);
        go.GetComponent<MeshRenderer>().material = material;
        go.gameObject.SetActive(true);
        gameObjectList.Add(go);
        GameObject goRef = new GameObject("GrapchiTilePacketRef " + gameObjectList.Count);
        goRef.transform.position = position;
        if(GraphicPacketRefTag != "")
            goRef.tag = GraphicPacketRefTag;
        go.transform.parent = goRef.transform;
        goRef.transform.parent = parentDestination;
    }


    private static void MeshCombine(int i, int j, CombineInstance[] combine, List<MeshFilter> meshFilters)
    {
        combine[j].mesh = meshFilters[i].sharedMesh;
        combine[j].transform = meshFilters[i].transform.localToWorldMatrix;
        GameObject.Destroy(meshFilters[i].gameObject);
    }

    private static void MeshCombine(int i, int j,CombineInstance[] combine, MeshFilter[] meshFilters)
    {
        combine[j].mesh = meshFilters[i].sharedMesh;
        combine[j].transform = meshFilters[i].transform.localToWorldMatrix;
        GameObject.Destroy(meshFilters[i].gameObject);
    }


    public static bool VectorsAreOppisite(Vector3 v1, Vector3 v2, float threshold)
    {
        float f = Vector3.Dot(v1.normalized, v2.normalized);
        return ((Mathf.Abs(f) - 1f) < threshold && f < 0);
    }

    public static bool VectorsAreOppisite(Vector2 v1, Vector2 v2, float threshold)
    {
        float f = Vector2.Dot(v1.normalized, v2.normalized);
        return ((Mathf.Abs(f) - 1f) < threshold && f < 0);
    }

    public static bool VectorsAreParallel(Vector3 v1, Vector3 v2, float threshold)
    {
        float f = Vector3.Dot(v1.normalized, v2.normalized);
        return (1f-Mathf.Abs(f)) < threshold;
    }

    public static bool VectorsAreParallel(Vector2 v1, Vector2 v2, float threshold)
    {
        float f = Vector2.Dot(v1.normalized, v2.normalized);
        return (1f-Mathf.Abs(f)) < threshold;
    }


    public static bool IsInArray(string tag, string [] arr)
	{
		bool found = false;
		for(int i = 0; !found && i < arr.Length; ++i )
		{
			if( tag == arr[i])
			{
				found = true;
			}
		}
		
		return found;
	}

    

    /*
float* CrossProduct(float *a, float *b)
{
	float Product[3];

        //Cross product formula 
	Product[0] = (a[1] * b[2]) - (a[2] * b[1]);
	Product[1] = (a[2] * b[0]) - (a[0] * b[2]);
	Product[2] = (a[0] * b[1]) - (a[1] * b[0]);

	return Product;
}
*/

    public static bool IsTheSameSign(float a, float b)
    {
        return Mathf.Sign(a) == Mathf.Sign(b);
    }


    public static float Manhattan(float tox, float toy, float fromx, float fromy)
    {
        float dX = Mathf.Abs(tox - fromx);
        float dY = Mathf.Abs(toy - fromy);
        return dX + dY;
    }

    public static void CopyTiledParameters(GameObject go, Dictionary<string,object> parameters)
    {
        List<MonoBehaviour> componentList = Utils.GetComponentsWithCustomAttribute<ClassTileMapAsignableAttributeAttribute>(go);
        for (int i = 0; i < componentList.Count; i++)
        {
            object[] attributtes = componentList[i].GetType().GetCustomAttributes(true);
            ClassTileMapAsignableAttributeAttribute classTileMapAsign = Utils.GetAttribute<ClassTileMapAsignableAttributeAttribute>(attributtes);
            if (classTileMapAsign != null)
            {
                CopyAttributes(go.name, componentList[i], parameters);
            }
        }
    }

    public static void CopyAttributes(string name, MonoBehaviour c, Dictionary<string, object> parameters)
    {
        FieldInfo[] fields = c.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < fields.Length; ++i)
        {
            FieldInfo fieldInfo = fields[i];
            object[] attributtes = fieldInfo.GetCustomAttributes(true);
            FieldTileMapAsignableAttributeAttribute fieldTileMapAsign = Utils.GetAttribute<FieldTileMapAsignableAttributeAttribute>(attributtes);
            if (fieldTileMapAsign != null)
            {
                if (!parameters.ContainsKey(fieldTileMapAsign.Name) && fieldTileMapAsign.Required)
                    UnityEngine.Debug.LogError(" La propiedad requeridad " + fieldTileMapAsign.Name + " no ha sido cargada correctamente del mapa");


                if (fieldInfo.FieldType.IsEnum)
                {
                    //si no se queda con el valor por defecto que tenga
                    if (parameters.ContainsKey(fieldTileMapAsign.Name))
                    {
                        string val = (string) parameters[fieldTileMapAsign.Name];
                        object objVal = Enum.Parse(fieldInfo.FieldType, val, true);
                        fieldInfo.SetValue(c, objVal);
                    }
                }
                else
                {
                    //si no se queda con el valor por defecto que tenga
                    if (parameters.ContainsKey(fieldTileMapAsign.Name))
                    {
                        object v = parameters[fieldTileMapAsign.Name];

                        UnityEngine.Debug.Log("Plataforma " + name + " " + fieldTileMapAsign.Name + ":" + v);

                        fieldInfo.SetValue(c, v);
                    }
                }
            }
        }
    }

    public static Collider GetColliderAbove(Transform transform, float distance, int layerMask, bool debugRay)
    {
        return GetColliderDirection(transform, Vector3.down, distance, layerMask, debugRay);
    }
    public static Collider GetColliderCeil(Transform transform, float distance, int layerMask, bool debugRay)
    {
        return GetColliderDirection(transform, Vector3.up, distance, layerMask, debugRay);
    }

    public static Collider GetColliderRight(Transform transform, float distance, int layerMask, bool debugRay)
    {
        return GetColliderDirection(transform, Vector3.right, distance, layerMask, debugRay);
    }

    public static Collider GetColliderLeft(Transform transform, float distance, int layerMask, bool debugRay)
    {
        return GetColliderDirection(transform, Vector3.left, distance, layerMask, debugRay);
    }

    public static Collider GetColliderDirection(Transform transform, Vector3 direction, float distance,int layerMask, bool debugRay)
    {
        RaycastHit hit;
        if (debugRay)
            UnityEngine.Debug.DrawRay(transform.position, direction * distance, Color.yellow);
        if (Physics.Raycast(transform.position, direction, out hit, distance, layerMask))
        {
            return hit.collider;
        }
        return null;
    }

    public static Collider GetColliderDirection(Transform transform, Vector3 direction, float distance, int layerMask, bool debugRay, out RaycastHit hit)
    {
        if (debugRay)
            UnityEngine.Debug.DrawRay(transform.position, direction * distance, Color.yellow);
        if (Physics.Raycast(transform.position, direction, out hit, distance, layerMask,QueryTriggerInteraction.Ignore))
        {
            return hit.collider;
        }
        return null;
    }

    

    public static void ExplosionForce(float multiplier, float explosionForce, Transform transform)
    {
        float r = 10 * multiplier;
        var cols = Physics.OverlapSphere(transform.position, r);
        var rigidbodies = new List<Rigidbody>();
        foreach (var col in cols)
        {
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
            {
                rigidbodies.Add(col.attachedRigidbody);
            }
        }
        foreach (var rb in rigidbodies)
        {
            rb.AddExplosionForce(explosionForce * multiplier, transform.position, r, 1 * multiplier, ForceMode.Impulse);
        }
    }



    

   
    public static void CleanPath(List<TileData> path)
    {
        if (path.Count < 2)
            return;
        Vector2 pI2 = new Vector2(path[1].X, path[1].Y);
        Vector2 pI1 = new Vector2(path[0].X, path[0].Y);
        Vector2 direction = pI2 - pI1;
        direction = direction.normalized;
        int firtsDiagonal = 0;
        int lastDiagonal = 0;
        bool diagonal = false;
        int i = 2;
        while (i < (path.Count - 1))
        {
            Vector2 p2 = new Vector2(path[i].X, path[i].Y);
            Vector2 p1 = new Vector2(path[i - 1].X, path[i - 1].Y);
            Vector2 newDir = p2 - p1;
            newDir = newDir.normalized;
            if (Mathf.Abs(newDir.x - direction.x) < 0.01f && Mathf.Abs(newDir.y - direction.y) < 0.01f)
            {
                if (diagonal)
                {

                    lastDiagonal = i - 2;
                    //Debug.Log("firtsDiagonal " + path[firtsDiagonal].ToString() + " lastDiagonal " + path[lastDiagonal].ToString());
                    diagonal = false;
                    TileData tdInit = path[firtsDiagonal];
                    TileData tdEnd = path[lastDiagonal];
                    if (Utils.Manhattan(tdInit.X, tdInit.Y, tdEnd.X, tdEnd.Y) >= 1f)
                    {
                        int num = lastDiagonal - firtsDiagonal - 1;
                        for (int j = 0; j < num; ++j)
                        {
                            path.RemoveAt(firtsDiagonal + 1);
                            i--;
                        }
                    }
                }
                path.RemoveAt(i - 1);
            }
            else
            {
                //me muevo en diagonal. Si sigo moviendo en diagonal, puedo aproximar hasta que deje de moverme en diagonal
                direction = newDir;
                if (!diagonal)
                {
                    firtsDiagonal = i - 1;
                    diagonal = true;
                }
                ++i;
            }
        }
    }

    public static float Manhattan(Vector2 from, Vector2 to)
    {
        float dX = Mathf.Abs(to.x - from.x);
        float dY = Mathf.Abs(to.y - from.y);
        return dX + dY;
    }

    protected float Euclidean(Vector2 from, Vector2 to)
    {
        float dX = to.x - from.x;
        float dY = to.y - from.y;
        return Mathf.Sqrt(dX * dX + dY * dY);
    }
}
