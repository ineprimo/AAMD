using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Reflection;
using System;
//using System.Xml.XmlNodeList;

public delegate object DeserializeMethod(string sType, string value);

public abstract class StorageMgr
{
	public StorageMgr( DeserializeMethod ds)
	{
        Deserializer = ds;
		ProcessAttributes();
	}
	
	
	public Dictionary<string, StorageKey> CreateSerialized(string name)
    {
		Dictionary<string, StorageKey> serialize = new Dictionary<string, StorageKey>();
		m_serializedCustom.Add(name, serialize);
		return serialize;
	}

	public bool ExistSerialezed(string name)
	{
		return m_serializedCustom.ContainsKey(name);
	}

	public Dictionary<string, StorageKey> GetSerialized(string name, bool forceToCreate)
    {
		if (forceToCreate && !m_serializedCustom.ContainsKey(name))
			CreateSerialized(name);
		return m_serializedCustom[name];

	}


	public void ShowDebug(string section)
	{
		StorageKey sk = m_serialized[section];
		Debug.Log("--------------"+section+"---------------------");
		if(sk == null)
		{
			Debug.Log("NULL");
		}
		else
		{
			sk.ShowDebug();
		}
		Debug.Log("-----------------------------------");
	}
			
	//Application.persistentDataPath
	//Removes all keys and values from the preferences. Use with caution.
	public void DeleteAll()
	{
		m_serialized.Clear();
	}
	
	//Removes key and its corresponding value from the preferences.
	public void Delete(string section)
	{
		if(m_serialized.ContainsKey(section))
			m_serialized.Remove(section);
	}
	
	public void Delete(string section, string key)
	{
		if(m_serialized.ContainsKey(section))
		{
			StorageKey sk = m_serialized[section];
			sk.Delete(key);
		}
	}

	public void SetInSerialized(string name, string section, string key, object a_value)
    {
		Dictionary<string, StorageKey> serialized = GetSerialized(name,true);
		if (!serialized.ContainsKey(section))
		{
			serialized.Add(section, new StorageKey(m_permitedType));
		}
		serialized[section].Set(key, a_value);
	}

	public void Set(string section, string key, object a_value)
	{
		if(!m_serialized.ContainsKey(section))
		{
			m_serialized.Add(section,new StorageKey(m_permitedType));
		}
		m_serialized[section].Set(key,a_value);
	}

	public void SetArray(string section, string key, string[] a_value)
	{
		string val = "";
		for (int i = 0; i < a_value.Length - 1; i++)
			val += a_value[i] + "#";
		if(a_value.Length > 0)
			val += a_value[a_value.Length - 1];
		m_serialized[section].Set(key, val);
	}

	public string[] GetArray(string section, string key)
	{
		string va = Get<string>(section, key);
		if(va == "")
        {
			return new string[0];
		}
		else
        {
			string[] vaArr = va.Split("#");
			return vaArr;
		}
	}

	public bool IsExist(string section, string key)
    {
        if(m_serialized.ContainsKey(section))
        {
            StorageKey sk = m_serialized[section];
            return sk.ContainsKey(key);
        }
        return false;
    }

	public T GetInSerialized<T>(string name, string section, string key)
	{
		Dictionary<string, StorageKey> serialized = GetSerialized(name,true);
		Assert.AbortIfNot(serialized.ContainsKey(section), "Section " + section + " not exist");
		StorageKey sk = serialized[section];
		Assert.AbortIfNot(sk.ContainsKey(key), "The Key " + key + " in section " + section + " not exist");
		return sk.Get<T>(key);
	}

	public T Get<T>(string section, string key)
	{
		// TODO 2: Alamcenamos una Key en el diccionario m_serialized (Solución 002 B)
		Assert.AbortIfNot(m_serialized.ContainsKey(section),"Section "+section+" not exist");
		StorageKey sk = m_serialized[section];
		Assert.AbortIfNot(sk.ContainsKey(key),"The Key "+key+" in section "+section+" not exist");
		return sk.Get<T>(key);
	}
	
	
	public bool Contains(string section)
	{
		return m_serialized.ContainsKey(section);
	}
	
	public bool Contains( string section, string key)
	{
		bool result = m_serialized.ContainsKey(section);
		if(result)
		{
			StorageKey sk = m_serialized[section];
			result = sk.ContainsKey(key);
		}
		return result;
	}

	public T LoadMetadata<T>(string dataFileName)
    {
		string fileFullName = Application.persistentDataPath + "/" + dataFileName;
		string data =  System.IO.File.ReadAllText(fileFullName);
		return JsonUtility.FromJson<T>(data);
	}

	public void StoreMetadata<T>(string dataFileName, T metadata) 
    {
		string fileFullName = Application.persistentDataPath + "/" + dataFileName;

		string data = JsonUtility.ToJson(metadata);
		
		System.IO.File.WriteAllText(fileFullName, data);
	}

	public void StoreSerialized(string name, string dataFileName)
    {
		string fileFullName = Application.persistentDataPath + "/" + dataFileName;
		Dictionary<string, StorageKey> serialized = GetSerialized(name,true);
		_Store(serialized, fileFullName);
	}


	public void Store(string dataFileName)
	{
		string fileFullName = Application.persistentDataPath+"/"+ dataFileName;

		_Store(m_serialized, fileFullName);
	}

	private void _Store(Dictionary<string, StorageKey> serialized, string fileFullName)
    {
		string data = "<save>\n";
		foreach (KeyValuePair<string, StorageKey> sectionPair in serialized)
		{
			StorageKey sk = sectionPair.Value;
			data += "\t<section name=\"" + sectionPair.Key + "\">\n";
			List<KeyValuePair<string, object>> pairList = sk.GetList();
			foreach (KeyValuePair<string, object> pairStrObj in pairList)
			{
				System.Type type = pairStrObj.Value.GetType();
				string strType = type.ToString();
				string strValue = pairStrObj.Value.ToString();
				data += "\t\t<field name=\"" + pairStrObj.Key + "\" type=\"" + strType + "\" value=\"" + strValue + "\"/>\n";
			}
			data += "\t</section>\n";
		}
		data += "</save>";
		System.IO.File.WriteAllText(fileFullName, data);
	}
	
	public long GetSize(string file)
	{
		string fileFullName = Application.persistentDataPath+"/"+file;
		System.IO.FileInfo info = new System.IO.FileInfo(fileFullName);
		if(!info.Exists)
			return -1;
		else
			return info.Length;
	}
	
	/******************************** FILE *****************************/
	
	public void Store(string fileName, string data)
	{
		string fileFullName = Application.persistentDataPath+"/"+fileName;
		System.IO.File.WriteAllText(fileFullName,data);
	}
	
	
	public void Append(string fileName, string data)
	{
		string fileFullName = Application.persistentDataPath+"/"+fileName;
		System.IO.StreamWriter file = new System.IO.StreamWriter(fileFullName, true);
		file.Write(data);
		file.Close();
	}
	
	/******************VOLATILE**********************************/

	
	public void DeleteVolatile(string section)
	{
		if(m_nonserialized.ContainsKey(section))
			m_nonserialized.Remove(section);
	}
	
	public void DeleteVolatile(string section, string key)
	{
		if(m_nonserialized.ContainsKey(section))
		{
			m_nonserialized[section].Remove(key);
		}

		if (m_nonserialized[section].Count == 0)
			m_nonserialized.Remove(section);
	}
	
	public void DeleteAllVolatile()
	{
		m_nonserialized.Clear();
	}
	
	public void SetVolatile(string section, string key, object a_value)
	{
		if(!m_nonserialized.ContainsKey(section))
		{
			m_nonserialized.Add(section,new Dictionary<string, object>());
		}
		m_nonserialized[section][key] = a_value;
	}
	
	
	public T GetVolatile<T>(string section, string key, bool deleteWenGet = false)
	{
		Assert.AbortIfNot(m_nonserialized.ContainsKey(section),"Section "+section+" not exist");
		Dictionary<string, object> dictionary = m_nonserialized[section];
		Assert.AbortIfNot(dictionary.ContainsKey(key),"The Key "+key+" in section "+section+" not exist");
		T v = (T)dictionary[key];
		if (deleteWenGet)
			DeleteVolatile(section, key);
		return v;
	}
	
	
	public bool ContainsVolatile(string section)
	{
		return m_nonserialized.ContainsKey(section);
	}
	
	public bool ContainsVolatile( string section, string key)
	{
		bool result = m_nonserialized.ContainsKey(section);
		if(result)
		{
			result = m_nonserialized[section].ContainsKey(key);
		}
		return result;
	} 
	
	
	
	/*public T GetUnserialized<T>(string section, string key)
	{
		
	}*/
	
	public bool ExistsFile(string dataFileName)
	{
		string fileFullName = Application.persistentDataPath + "/" + dataFileName;
		return System.IO.File.Exists(fileFullName);
	}

	public void Erase(string dataFileName)
    {
		string fileFullName = Application.persistentDataPath + "/" + dataFileName;
		System.IO.File.Delete(fileFullName);
	}
	
	public void LoadSerialized(string name, string dataFileName)
    {
		string fileFullName = Application.persistentDataPath + "/" + dataFileName;
		Dictionary<string, StorageKey> serialized = GetSerialized(name,true);
		_Load(serialized, fileFullName);
	}

	private void _Load(Dictionary<string, StorageKey> serialized, string fileFullName)
    {
		serialized.Clear();
		XmlDocument root = new XmlDocument();
		root.Load(fileFullName);
		XmlNodeList sectionsList = root.GetElementsByTagName("section");
		for (int i = 0; i < sectionsList.Count; ++i)
		{
			XmlNode section = sectionsList[i];
			string name = section.Attributes["name"].Value;

			StorageKey sk = new StorageKey(m_permitedType);
			serialized.Add(name, sk);

			List<XmlNode> fieldList = XMLUtils.GetNodes(section, "field");
			for (int j = 0; j < fieldList.Count; ++j)
			{
				XmlNode fieldnode = fieldList[j];
				string strType = fieldnode.Attributes["type"].Value;
				string strValuej = fieldnode.Attributes["value"].Value;
				string fieldName = fieldnode.Attributes["name"].Value;

				sk.Set(fieldName, Deserializer(strType, strValuej));
			}
		}
	}
	
	public void Load(string dataFileName)
	{
		
		string fileFullName = Application.persistentDataPath + "/" + dataFileName;
		_Load(m_serialized, fileFullName);
		Debug.Log("Partida cargada!");
	}
	
	public static DeserializeMethod Deserializer= null;

    protected void ProcessAttributes()
	{
        m_permitedType = new List<Type>();
        int count = 0;
		System.Reflection.MemberInfo info = GetType();
		object[] attributes = info.GetCustomAttributes(true);
		for(int i = 0; i < attributes.Length; ++i)
		{
			if(attributes[i].GetType() == typeof (AllowedTypeToStorage))
			{
				//TODO 1: Añadir a la lista de atributos permitidos. (solucion 001)
				AllowedTypeToStorage allowedType = attributes[i] as AllowedTypeToStorage;
                m_permitedType.Add(allowedType.m_type);
				
				count++;
			}
		}
		
		Assert.AbortIfNot(count > 0,"Error: It Must be defined a PermitedTypes Attributes");
	}
	
	List<System.Type> m_permitedType;
	protected Dictionary<string,StorageKey> m_serialized = new Dictionary<string, StorageKey>();
	protected Dictionary<string,Dictionary<string, StorageKey>> m_serializedCustom = new Dictionary<string,Dictionary<string, StorageKey>>();
	protected Dictionary<string,Dictionary<string,object>> m_nonserialized = new Dictionary<string, Dictionary<string, object>>();
	

/*GetFloat 	Returns the value corresponding to key in the preference file if it exists.
GetInt 	Returns the value corresponding to key in the preference file if it exists.
GetString 	Returns the value corresponding to key in the preference file if it exists.
HasKey 	Returns true if key exists in the preferences.
Save 	Writes all modified preferences to disk.
SetFloat 	Sets the value of the preference identified by key.
SetInt 	Sets the value of the preference identified by key.
SetString 	Sets th*/
}
