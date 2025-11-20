using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Clase que almacena alamcena los tipos de datos que guardamos en el almacenamiento.
public class StorageKey 
{
	
	public StorageKey(List<System.Type> allowedType)
	{
		m_allowedType = allowedType;
	}
	
	public void Delete(string key)
	{
		m_data.Remove(key);
	}
	
	public void Set(string key, object data)
	{
		if(!AllowedType(data.GetType()))
            Debug.LogError(data.GetType()+" Is forbidden");
		m_data[key] = data;
	}
	
	public string GetValueToString(string key)
	{
		Assert.AbortIfNot(m_data.ContainsKey(key),"the key "+key+" is not exist");
		return m_data[key].ToString();
	}
	
	public System.Type GetContentType(string key)
	{
		Assert.AbortIfNot(m_data.ContainsKey(key),key+" Is not exists");
		object obj = m_data[key];
		return obj.GetType();
	}
	
	public bool ContainsKey(string key)	
	{
		return m_data.ContainsKey(key);
	}
	
	public T Get<T>(string key)
	{
		Assert.AbortIfNot(AllowedType(typeof(T)),typeof(T)+" Is not permited");
		bool exist = m_data.ContainsKey(key);
		Assert.AbortIfNot(exist,key+" Is not exists");
		T obj = (T) m_data[key];
		return obj;
	}
	
	public void ShowDebug()
	{
		foreach (KeyValuePair<string,object> pair in m_data)
		{
			Debug.Log(pair.Key+" = "+pair.Value);
		}
	}
	
	public List<KeyValuePair<string,object>> GetList()
	{
		List<KeyValuePair<string,object>> list = new List<KeyValuePair<string,object>>();
		foreach (KeyValuePair<string,object> pair in m_data)
		{
			list.Add(pair);
		}
		return list;
	}

    public bool AllowedType(System.Type type)
    {
        return m_allowedType.Exists(x => x == type);
    }

    protected Dictionary<string,object> m_data = new Dictionary<string, object>();
	List<System.Type> m_allowedType;
	
}
