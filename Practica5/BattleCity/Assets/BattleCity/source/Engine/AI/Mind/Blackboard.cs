using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mind
{
    public class Blackboard
    {
        private Dictionary<string, object> _dictionary;
        private Blackboard _parent;

        public Blackboard(Blackboard parent)
        {
            _dictionary = new Dictionary<string, object>();
            _parent = parent;
        }

        public T Get<T>(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                object value = _dictionary[key];
                return (T)value;
            }
            else if(_parent != null)
                return _parent.Get<T>(key);
            else
            {
                Debug.LogError("El campo " + key + " No existe en la blackboard");
                return default(T);
            }
        }

        public void Set(string key, object val)
        {
            if (ExistInThisBlackboard(key))
                _dictionary[key] = val;
            else if (_parent != null)
                _parent.Set(key,val);
            else
                _dictionary.Add(key, val);
        }

        public bool ExistInHierarchy(string key)
        {
            if (ExistInThisBlackboard(key))
                return true;
            else
                return _parent != null ? _parent.ExistInHierarchy(key) : false;

        }

        public bool ExistInThisBlackboard(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public Blackboard GetFirstBlackboardContain(string key)
        {
            if (ExistInThisBlackboard(key))
                return this;
            else if (_parent != null)
                return _parent.GetFirstBlackboardContain(key);
            else
                return null;
        }

        public Blackboard Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public List<string> Keys
        {
            get
            {
                List<string> keyValues = new List<string>();
                foreach (var k in _dictionary.Keys)
                {
                    keyValues.Add(k);
                }
                return keyValues;
            }
        }

        public object this[string key]
        {
            get
            {
                return Get<object>(key);
            }
            set
            {
                Set(key, value);
            }
        }
    }
}
