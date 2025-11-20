using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine<T,E,S> : StateBehavior<T, E, S>   where T : MonoBehaviour
                                                            where E : struct
                                                            where S : struct
{
    private Dictionary<S, StateBehavior<T,E,S>> _states;
    private List<E> _events;
    

    private S _currentState;

    public FiniteStateMachine(T executor, S state) : base(executor, state)
    {
        _states = new Dictionary<S, StateBehavior<T,E,S>>();
        _events = new List<E>();
    }

    public override void Init()
    {
        _states[_currentState].Init();
    }

    public override void Update(float time)
    {
        _states[_currentState].Update(time);
        ProcessEvents();
    }

    public override void End()
    {

    }

    public S CurrentState
    {
        get { return _currentState; }
    }

    public void AddState(StateBehavior<T,E,S> state, bool init = false)
    {
        if (_states.Count == 0 || init)
            _currentState = state.StateID;

        _states.Add(state.StateID, state);
        state.AddExecutor(this);

    }

    public bool AddTransition(S o, S des, E a_event)
    {
        StateBehavior<T,E,S> state;
        bool result = false;
        if(_states.TryGetValue(o,out state))
        {
            if (_states.ContainsKey(des))
            {
                state.AddTransition(a_event, des);
                result = true;
            }
        }
        return result;
    }

    public override void SendEvent(E a_event)
    {
        _events.Add(a_event);
    }

    protected void ProcessEvents()
    {
        for(int i = 0; i < _events.Count; ++i)
        {
            E evn = _events[i];
            S newState;
            if (_states[_currentState].Transite(evn,out newState))
            {
                _states[_currentState].End();
                _currentState = newState;
                _states[_currentState].Init();
            }
        }
        _events.Clear();
    }
}
