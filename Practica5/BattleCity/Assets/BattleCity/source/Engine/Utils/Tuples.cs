using UnityEngine;
using System.Collections;

public struct TPair<T,Q> 
{
	public T First;
	public Q Second;
	
	public TPair(T t,Q q)
	{
		First = t;
		Second = q;
	}
}

public struct TTerna<T,Q,S>
{
	public T First;
	public Q Second;
	public S Third;
	public TTerna(T t,Q q, S s)
	{
		First = t;
		Second = q;
		Third = s;
	}
}

public class Pair<T,Q> 
{
	public T First;
	public Q Second;
	
	public Pair(T t,Q q)
	{
		First = t;
		Second = q;
	}
	
	public Pair()
	{
		First = default(T);
		Second = default(Q);
	}
}

public class Terna<T,Q,S> : Pair<T,Q>
{
	public S Third;
	public Terna(T t,Q q, S s) : base(t,q)
	{
		Third = s;
	}
	
	public Terna() : base()
	{
		Third = default(S);
	}
}
