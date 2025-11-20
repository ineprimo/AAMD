using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//M: MonoBehavior. E: Enum Event; S: Enum State
public abstract class StateBehavior<T,E,S>   where T : MonoBehaviour
                                    where E : struct
                                    where S : struct
{
    private T _component;
    private FiniteStateMachine<T,E,S> _executor;
    private S _stateID;

    private Dictionary<E, S> _transition;

    public StateBehavior(T component, S id)
    {
        _stateID = id;
        _component = component;
        _transition = new Dictionary<E, S>();
    }

    public S StateID
    {
        get { return _stateID; }
    }

    public void AddExecutor(FiniteStateMachine<T,E,S> ex)
    {
        _executor = ex;
    }

    public virtual void SendEvent(E a_event)
    {
        _executor.SendEvent(a_event);
    }

    public bool Transite(E a_event, out S a_state)
    {
        a_state = default(S);
        if (_transition.ContainsKey(a_event))
        {
            a_state = _transition[a_event];
            return true;
        }
        return false;
    }

    public S this[E e]
    {
        get { return _transition[e]; }
    }

    public S GetState(E e)
    {
        return _transition[e];
    }

    public void AddTransition(E a_event, S state)
    {
        if (_transition.ContainsKey(a_event))
            _transition[a_event] = state;
        else
            _transition.Add(a_event,state);
    }

    public T Component
    {
        get { return _component; }
    }

    public abstract void Init();

    public abstract void Update(float time);

    public abstract void End();
}
