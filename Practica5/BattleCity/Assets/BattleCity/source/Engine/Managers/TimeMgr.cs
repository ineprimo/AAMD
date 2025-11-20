using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Delegado que nos permite definir la interfaz del metodo al que vamso a llamar cuando se nos establezca una alarma
//el delegado recibe como parametro el tiempo que ha trasncurrido y el objeto que previamente sed le habia pasado.
public delegate void AlarmDelegate(float time, object data);

public struct DefferedCallID
{
	public int id;
	public int numParameters;

	public DefferedCallID(int i, int n)
    {
		id = i;
		numParameters = n;
    }
}

/// <summary>
/// Time mgr. PErmite deifnir alarmas, tanto de un unico tiempo como periodicas.
/// </summary>
public class TimeMgr : MonoBehaviour
{
	protected bool Contains(MonoBehaviour component, string name, ref TDataTimeMgr dataTimeMgr)
	{

		bool result = false;
		for(int i = 0 ; !result && i < m_alarms.Count; ++i)
		{
			TDataTimeMgr data = m_alarms[i];
			if(component.GetInstanceID() == data.m_component.GetInstanceID() && name == data.m_alarmName)
			{
				result = true;
				dataTimeMgr = data;
			}
		}
		return result;
	}

	public void CancelDeferredCall(DefferedCallID id)
    {
		if (id.numParameters == 0)
			CancelDeferred0(id.id);
		else if (id.numParameters == 1)
			CancelDeferred1(id.id);
		else if (id.numParameters == 2)
			CancelDeferred2(id.id);
		else if (id.numParameters == 3)
			CancelDeferred3(id.id);
		else
			Debug.LogError("Error, no existe un deferred call con " + id.numParameters + " parámetros");
	}

	protected void CancelDeferred0(int id)
    {
		for(int i = 0; i < m_deferredCall.Count; i++)
        {
			if (m_deferredCall[i].id == id)
            {
				m_deferredCall.RemoveAt(i);
				return;
			}
		}
    }

	protected void CancelDeferred1(int id)
	{
		for (int i = 0; i < m_deferredCall1.Count; i++)
		{
			if (m_deferredCall1[i].id == id)
			{
				m_deferredCall1.RemoveAt(i);
				return;
			}
		}
	}

	protected void CancelDeferred2(int id)
	{
		for (int i = 0; i < m_deferredCall2.Count; i++)
		{
			if (m_deferredCall2[i].id == id)
			{
				m_deferredCall2.RemoveAt(i);
				return;
			}
		}
	}

	protected void CancelDeferred3(int id)
	{
		for (int i = 0; i < m_deferredCall3.Count; i++)
		{
			if (m_deferredCall3[i].id == id)
			{
				m_deferredCall3.RemoveAt(i);
				return;
			}
		}
	}

	public DefferedCallID DeferredCall(MonoBehaviour com, float time, System.Action action)
    {
		DeferredCallTime dc = new DeferredCallTime(time, action, com);
		m_deferredCall.Add(dc);
		return new DefferedCallID(dc.id, 0);
	}

	public DefferedCallID DeferredCall(MonoBehaviour com, float time, System.Action<object> action, object p1)
	{
		DeferredCallTimeP1 dc = new DeferredCallTimeP1(time, action, com, p1);
		m_deferredCall1.Add(dc);
		return new DefferedCallID(dc.id, 1);
	}

	public DefferedCallID DeferredCall(MonoBehaviour com, float time, System.Action<object,object> action, object p1, object p2)
	{
		DeferredCallTimeP2 dc = new DeferredCallTimeP2(time, action, com, p1, p2);
		m_deferredCall2.Add(dc);
		return new DefferedCallID(dc.id, 2);
	}

	public DefferedCallID DeferredCall(MonoBehaviour com, float time, System.Action<object, object, object> action, object p1, object p2, object p3)
	{
		DeferredCallTimeP3 dc = new DeferredCallTimeP3(time, action, com, p1, p2, p3);
		m_deferredCall3.Add(dc);
		return new DefferedCallID(dc.id,3);
	}

	public void SetAlarm(MonoBehaviour component,string name, AlarmDelegate func, Object data, float time, bool periodic = false)
	{
		TDataTimeMgr alarm = default;
		if(!Contains(component,name,ref alarm))
		{
            TDataTimeMgr dataMgr = new()
            {
                m_component = component,
                m_alarmName = name,
                func = func,
                m_timeToExpired = time,
                m_time = time,
                periodic = periodic,
                data = data,
            };
            m_alarms.Add(dataMgr);
		}
		else
		{
			//Cambio los valores de la alarma
			alarm.m_timeToExpired = time;
			alarm.m_time = time;
			alarm.func = func;
			alarm.periodic = periodic;
		}
	}
	
	public void CancelAlarm(MonoBehaviour component,string name)
	{
		TDataTimeMgr alarm = default;
		if(Contains(component,name,ref alarm ))
		{
			m_alarms.Remove(alarm);
		}
	}
	
	public bool ExistAlarm(MonoBehaviour component,string name)
	{
		TDataTimeMgr alarm = default;
		return Contains(component,name,ref alarm);
	}
	
	public float GetTimeToExpired(MonoBehaviour component,string name)
	{
		float time = -1;
		TDataTimeMgr alarm = default;
		if(Contains(component,name,ref alarm ))
		{
			time = alarm.m_timeToExpired;
		}
		return time;
	}

	public bool IsGamePause
    {
		get;
		set;
    }



	void Update()
	{

		processDeferredCalls();
		ProcessTimeData();
		ProcessWaitList();
		ProcessWaiNoDatatList();
	}

	protected void processDeferredCalls()
    {
		int i = 0;
		while (i < m_deferredCall.Count)
		{
			DeferredCallTime data = m_deferredCall[i];
			if(data.component == null)
            {
				m_deferredCall.RemoveAt(i);
			}
			else if(data.component.gameObject.activeInHierarchy)
            {
				data.time = data.time - Time.deltaTime;
				if(data.time <= 0f)
                {
					data.action?.Invoke();
					m_deferredCall.RemoveAt(i);
				}
				else
                {
					m_deferredCall[i] = data;
					i++;
				}
			}
			else
				i++;
		}

		i = 0;
		while (i < m_deferredCall1.Count)
		{
			DeferredCallTimeP1 data = m_deferredCall1[i];
			if (data.component == null)
			{
				m_deferredCall1.RemoveAt(i);
			}
			else if (data.component.gameObject.activeInHierarchy)
			{
				data.time = data.time - Time.deltaTime;
				if (data.time <= 0f)
				{
					data.action?.Invoke(data.p1);
					m_deferredCall1.RemoveAt(i);
				}
				else
				{
					m_deferredCall1[i] = data;
					i++;
				}
			}
			else
            {
				if(IsGamePause)
					i++;
				else
					m_deferredCall1.RemoveAt(i);
			}
				
		}

		i = 0;
		while (i < m_deferredCall2.Count)
		{
			DeferredCallTimeP2 data = m_deferredCall2[i];
			if (data.component == null)
			{
				m_deferredCall2.RemoveAt(i);
			}
			else if (data.component.gameObject.activeInHierarchy)
			{
				data.time = data.time-Time.deltaTime;
				if (data.time <= 0f)
				{
					data.action?.Invoke(data.p1, data.p2);
					m_deferredCall2.RemoveAt(i);
				}
				else
				{
					m_deferredCall2[i] = data;
					i++;
				}
			}
			else
			{
				if (IsGamePause)
					i++;
				else
					m_deferredCall2.RemoveAt(i);
			}
		}

		i = 0;
		while (i < m_deferredCall3.Count)
		{
			DeferredCallTimeP3 data = m_deferredCall3[i];
			if (data.component == null)
			{
				m_deferredCall3.RemoveAt(i);
			}
			else if (data.component.gameObject.activeInHierarchy)
			{
				data.time = data.time - Time.deltaTime;
				if (data.time <= 0f)
				{
					data.action?.Invoke(data.p1, data.p2, data.p3);
					m_deferredCall3.RemoveAt(i);
				}
				else
				{
					m_deferredCall3[i] = data;
					i++;
				}
			}
			else
			{
				if (IsGamePause)
					i++;
				else
					m_deferredCall3.RemoveAt(i);
			}
		}
	}

	protected void ProcessTimeData()
    {
		int i = 0;
		while (i < m_alarms.Count)
		{
			TDataTimeMgr data = m_alarms[i];
			if (data.m_component != null)
			{
				if (data.m_component.gameObject.activeInHierarchy)
				{
					data.m_timeToExpired -= Time.deltaTime;
					if (data.m_timeToExpired <= 0f)
					{
						data.func(data.m_time - data.m_timeToExpired, data.data);
						if (data.periodic)
						{
							data.m_timeToExpired = data.m_time + data.m_timeToExpired;
							m_alarms[i] = data;
							i++;
						}
						else
						{
							m_alarms.RemoveAt(i);
						}
					}
					else
                    {
						m_alarms[i] = data;
						i++;
					}
						
				}
				else
				{
					if (IsGamePause)
						i++;
					else
						m_alarms.RemoveAt(i);
				}

			}
			else
				m_alarms.RemoveAt(i); // si el objeto no existe significa que se ha destruido por lo que no informamos de nada.
		}
	}
	protected void ProcessWaitList()
    {
		int i = 0;
		while (i < m_waitList.Count)
		{
			TWait w = m_waitList[i];
			if (w.component == null)
            {
				m_waitList.RemoveAt(i);
			}
			else
            {
				if (!w.IsPause() && w.component.gameObject.activeInHierarchy)
				{
					w.time -= Time.deltaTime;
					if (w.time <= 0f)
					{
						if (w.component != null)
						{
							w.action(w.parameter);
						}
						m_waitList.RemoveAt(i);
					}
					else
					{
						m_waitList[i] = w;
						i++;
					}
				}
				else if (!w.component.gameObject.activeInHierarchy)
				{
					if (!w.pauseWhenGamePause) // si esta marcado como 
					{
						m_waitList.RemoveAt(i);
					}
					else if (!w.IsPause())
					{
						m_waitList.RemoveAt(i);
					}
					else
						i++;
				}
			}
		}
	}

	protected void ProcessWaiNoDatatList()
	{
		int i = 0;
		while (i < m_waitNoDataList.Count)
		{
			TWaitNoData w = m_waitNoDataList[i];
			if (w.component == null)
            {
				m_waitNoDataList.RemoveAt(i);
			}
			else if (!w.IsPause() && w.component.gameObject.activeInHierarchy)
			{
				w.time -= Time.deltaTime;
				if (w.time <= 0f)
				{
					if (w.component != null)
					{
						w.action();
					}
					m_waitNoDataList.RemoveAt(i);
				}
				else
				{
					m_waitNoDataList[i] = w;
					i++;
				}
			}
			else if (!w.component.gameObject.activeInHierarchy)
			{
				if (!w.pauseWhenGamePause) // si esta marcado como 
				{
					m_waitNoDataList.RemoveAt(i);
				}
				else if (!w.IsPause())
				{
					m_waitNoDataList.RemoveAt(i);
				}
				else
					i++;
			}
		}
	}
	protected struct DeferredCallTime
    {
		public int id;
		public float time;
		public System.Action action;
		public MonoBehaviour component;

		public DeferredCallTime(float t, System.Action a, MonoBehaviour c)
        {
			id = Random.Range(int.MinValue, int.MaxValue);
			time = t;
			action = a;
			component = c;

		}
	}

	protected struct DeferredCallTimeP1
	{
		public int id;
		public float time;
		public System.Action<object> action;
		public MonoBehaviour component;
		public object p1;

		public DeferredCallTimeP1(float t, System.Action<object> a, MonoBehaviour c, object a_p1)
		{
			id = Random.Range(int.MinValue, int.MaxValue);
			time = t;
			action = a;
			component = c;
			p1 = a_p1;
		}
	}

	protected struct DeferredCallTimeP2
	{
		public int id;
		public float time;
		public System.Action<object,object> action;
		public MonoBehaviour component;
		public object p1;
		public object p2;

		public DeferredCallTimeP2(float t, System.Action<object,object> a, MonoBehaviour c, object a_p1, object a_p2)
		{
			id = Random.Range(int.MinValue, int.MaxValue);
			time = t;
			action = a;
			component = c;
			p1 = a_p1;
			p2 = a_p2;
		}
	}

	protected struct DeferredCallTimeP3
	{
		public int id;
		public float time;
		public System.Action<object, object, object> action;
		public MonoBehaviour component;
		public object p1;
		public object p2;
		public object p3;

		public DeferredCallTimeP3(float t, System.Action<object, object, object> a, MonoBehaviour c, object a_p1, object a_p2, object a_p3)
		{
			id = RandomID.GetID;
			time = t;
			action = a;
			component = c;
			p1 = a_p1;
			p2 = a_p2;
			p3 = a_p3;
		}
	}

	protected struct TDataTimeMgr
	{
		public MonoBehaviour m_component;
		public string m_alarmName;
		public AlarmDelegate func;
		public float m_timeToExpired;
		public float m_time;
		public bool periodic;
		public object data;
	}


	public int WaitForTime(MonoBehaviour c, float time, System.Action a, bool p)
	{
		Debug.Assert(c != null, "El componente no puede ser null");
		Debug.Assert(a != null, "La acción a invocar no puede ser null");
		Debug.Assert(time >= 0, "el tiempo no puede ser negativo");
        TWaitNoData w = new()
        {
            time = time,
            id = RandomID.GetID,
			action = a,
			component = c,
			pauseWhenGamePause=p,
		};
		m_waitNoDataList.Add(w);
		return w.id;
	}

	public int WaitForTime(MonoBehaviour c, float t, System.Action<object> a, object param, bool p)
    {
		Debug.Assert(c != null,"El componente no puede ser null");
		Debug.Assert(a != null, "La acción a invocar no puede ser null");
		Debug.Assert(t >= 0, "el tiempo no puede ser negativo");
        TWait w = new()
        {
            time = t,
            id = RandomID.GetID,
            action = a,
            component = c,
            parameter = param,
            pauseWhenGamePause = p
        };
        m_waitList.Add(w);
		return w.id;
	}
	public int CancelWaitForTime(int id)
    {
		int index = GetIndex(id);
		if(index != -1)
        {
			if (id > 0)
				m_waitList.RemoveAt(index);
			else if (id < 0)
				m_waitNoDataList.RemoveAt(index);
			else
				Debug.LogError("Error el valor de 0 indica no inicialización");
		}
		return 0;
	}

	protected int GetIndex(int id)
    {
		if (id > 0)
			return GetIndexData(id);
		else
			return GetIndexNoData(id);
	}

	protected int GetIndexData(int id)
    {
		for (int i = 0; i < m_waitList.Count; i++)
		{
			if (m_waitList[i].id == id)
			{
				return i;
			}
		}
		return -1;
	}

	protected int GetIndexNoData(int id)
    {
		for (int i = 0; i < m_waitNoDataList.Count; i++)
		{
			if (m_waitNoDataList[i].id == id)
			{
				return i;
			}
		}
		return -1;
	}
	
	public void PauseWaitForTime(int id)
    {
		int index = GetIndex(id);
		if (index != -1)
		{
			if(id >0)
				m_waitList[index].Pause();
			else
				m_waitNoDataList[index].Pause();
		}
	}

	public void ResumeWaitForTime(int id)
	{
		int index = GetIndex(id);
		if (index != -1)
		{
			if (id > 0)
				m_waitList[index].Resume();
			else
				m_waitNoDataList[index].Resume();
		}
	}

	public void PauseAllWaitsByPauseGame()
    {
		for(int i = 0; i < m_waitList.Count; i++)
        {
			if (m_waitList[i].pauseWhenGamePause)
				m_waitList[i].Pause();
		}

		for (int i = 0; i < m_waitNoDataList.Count; i++)
		{
			if (m_waitNoDataList[i].pauseWhenGamePause)
				m_waitNoDataList[i].Pause();
		}
	}

	public void ResumeAllWaitsByResumeGame()
	{
		for (int i = 0; i < m_waitList.Count; i++)
		{
			if (m_waitList[i].pauseWhenGamePause)
				m_waitList[i].Resume();
		}

		for (int i = 0; i < m_waitNoDataList.Count; i++)
		{
			if (m_waitNoDataList[i].pauseWhenGamePause)
				m_waitNoDataList[i].Resume();
		}
	}

	protected struct TWait
    {
		public int id;
		public float time;
		public System.Action<object> action;
		public object parameter;
		public MonoBehaviour component;
		public bool pauseWhenGamePause;
		private bool pause;

		public void Pause()
        {
			pause = true;
        }

		public void Resume()
        {
			pause = false;
        }

		public bool IsPause()
        {
			return pause;
        }
    }

	protected struct TWaitNoData
	{
		public int id;
		public float time;
		public System.Action action;
		public MonoBehaviour component;
		public bool pauseWhenGamePause;
		private bool pause;

		public void Pause()
		{
			pause = true;
		}

		public void Resume()
		{
			pause = false;
		}

		public bool IsPause()
		{
			return pause;
		}
	}

	List<DeferredCallTime> m_deferredCall = new List<DeferredCallTime>();
	List<DeferredCallTimeP1> m_deferredCall1 = new List<DeferredCallTimeP1>();
	List<DeferredCallTimeP2> m_deferredCall2 = new List<DeferredCallTimeP2>();
	List<DeferredCallTimeP3> m_deferredCall3 = new List<DeferredCallTimeP3>();
	List<TDataTimeMgr> m_alarms = new List<TDataTimeMgr>();
	List<TWait> m_waitList = new List<TWait>();
	List<TWaitNoData> m_waitNoDataList = new List<TWaitNoData>();
}
