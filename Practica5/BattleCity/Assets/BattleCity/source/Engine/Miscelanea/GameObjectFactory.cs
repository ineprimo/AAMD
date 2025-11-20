using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public interface IFactory
{
    /// <summary>
    /// Max objects used at the same time
    /// </summary>
    int MaxObjectsUsedAtTime { get; }

    /// <summary>
    /// Is Factory Initialized?
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Internal list of Used objects
    /// </summary>
    List<GameObject> UsedObjects { get; }

    /// <summary>
    /// Initialize the Factory
    /// </summary>
    /// <param name="parent">Parent to attach new GameObjects</param>
    /// <param name="prefab">Prefab to clone</param>
    /// <param name="capacity">Initial size</param>
    /// <returns>True if success</returns>
    bool Initialize(Transform parent, GameObject prefab, int capacity);

    /// <summary>
    /// Deinitialize the Factory
    /// </summary>
    /// <returns></returns>
    bool Deinitialize();

    /// <summary>
    /// Get a new Gameobject
    /// </summary>
    /// <returns></returns>
    GameObject Get();

    /// <summary>
    /// Create and Get a new object
    /// </summary>
    /// <param name="forceCreation"></param>
    /// <returns></returns>
    GameObject Get(bool forceCreation);

    /// <summary>
    /// Release the GameObject. Use it when finish the use
    /// </summary>
    /// <param name="o"></param>
    void Release(GameObject o);

}
    
public class GameObjectFactory : IFactory
{
    GameObject _prefab;
    Transform _parent;
    List<GameObject> _used = new List<GameObject>();
    List<GameObject> _notUsed = new List<GameObject>();

    public List<GameObject> UsedObjects { get { return _used; } }


    bool _initialized = false;
    public bool IsInitialized { get { return _initialized; } }

    int _maxUsedAtTime = int.MaxValue;
    public int MaxObjectsUsedAtTime { get { return _maxUsedAtTime; } set { _maxUsedAtTime = value; } }

    public bool Initialize(Transform parent, GameObject prefab, int capacity)
    {
        if (!_initialized)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < capacity; ++i)
            {
                Get(true);
            }
            _ReleaseAll();
            _initialized = true;
        }
        return _initialized;
    }
    public bool Deinitialize()
    {
        if (_initialized)
        {
            _ReleaseAll();
            while (_notUsed.Count > 0)
            {
                GameObject.DestroyImmediate(_notUsed[0]);
                _notUsed.RemoveAt(0);
            }
            _initialized = false;
        }
        return !_initialized;
    }
    public GameObject Get()
    {
        return Get(false);
    }
    public GameObject Get(bool forceCreation)
    {
        if (forceCreation) return _Instantiate();
        if (_used.Count < MaxObjectsUsedAtTime)
        {
            if (_notUsed.Count > 0)
            {
                GameObject res = _notUsed[_notUsed.Count - 1];
                _notUsed.RemoveAt(_notUsed.Count - 1);
                _used.Add(res);
                _Activate(res);
                return res;
            }
            else return _Instantiate();
        }
        else return null;
    }
    public void Release(GameObject o)
    {
        if (o != null && _used.Remove(o))
        {
            _Deactivate(o);
            _notUsed.Add(o);

        }
    }



    void _ReleaseAll()
    {
        while (_used.Count > 0)
        {
            if (_used[0] != null)
            {
                _Deactivate(_used[0]);
                _notUsed.Add(_used[0]);
            }
            _used.RemoveAt(0);
        }
    }
    GameObject _Instantiate()
    {
        GameObject o = GameObject.Instantiate(_prefab) as GameObject;
        o.transform.parent = _parent;
        _used.Add(o);
        o.name = _prefab.name + "_" + _used.Count;
        return o;
    }
    void _Activate(GameObject o)
    {
        o.SetActive(true);
    }
    void _Deactivate(GameObject o)
    {            
        o.SetActive(false);
    }


}
public class PoolFactory<T> where T : class, new()
{
    //GameObject _prefab;
    //Transform _parent;
    List<T> _used = new List<T>();
    List<T> _notUsed = new List<T>();

    public List<T> UsedObjects { get { return _used; } }

    public int CountUsed { get { return _used.Count; } }
    public int CountNotUsed { get { return _notUsed.Count; } }


    bool _initialized = false;
    public bool IsInitialized { get { return _initialized; } }

    int _maxUsedAtTime = int.MaxValue;
    public int MaxObjectsUsedAtTime { get { return _maxUsedAtTime; } set { _maxUsedAtTime = value; } }

    public bool Initialize(int capacity)
    {
        if (!_initialized)
        {
            for (int i = 0; i < capacity; ++i)
            {
                Get(true);
            }
            _ReleaseAll();
            _initialized = true;
        }
        return _initialized;
    }
    public bool Deinitialize()
    {
        if (_initialized)
        {
            _ReleaseAll();
            while (_notUsed.Count > 0)
            {
                //GC... is your Turn
                _notUsed[0] = null;
                _notUsed.RemoveAt(0);
            }
            _initialized = false;
        }
        return !_initialized;
    }
    public T Get()
    {
        return Get(false);
    }
    public T Get(bool forceCreation)
    {
        if (forceCreation) return _Instantiate();
        if (_used.Count < MaxObjectsUsedAtTime)
        {
            if (_notUsed.Count > 0)
            {
                T res = _notUsed[0];
                _notUsed.RemoveAt(0);
                _used.Add(res);
                return res;
            }
            else return _Instantiate();
        }
        else return null;
    }
    public void Release(T o)
    {
        if (_used.Remove(o))
        {
            _notUsed.Add(o);
        }
    }



    void _ReleaseAll()
    {
        while (_used.Count > 0)
        {
            _notUsed.Add(_used[0]);
            _used.RemoveAt(0);
        }
    }
    T _Instantiate()
    {
        T o = new T();
        _used.Add(o);
        return o;
    }
}


