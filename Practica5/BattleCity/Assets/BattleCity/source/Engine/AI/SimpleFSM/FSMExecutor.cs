using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMExecutor<T,E,S> : Tickable
                                    where T: MonoBehaviour
                                    where E : struct
                                    where S : struct
{

    private T _component;
    public S _stateName;
    private FiniteStateMachine<T,E,S> _fsm;
    private bool _isInicialized;
	// Use this for initialization
	public virtual void Start () {
        _component = GetComponent<T>();
        _fsm = new FiniteStateMachine<T,E,S>(_component, _stateName);
        CreateFSM(_fsm);
        _isInicialized = false;
    }

    public void Init()
    {
        FSMInit();
        _fsm.Init();
        _isInicialized = true;
    }

    // Update is called once per frame
    protected override void Tick(float time)
    {
        if (!_isInicialized)
            Init();
        _fsm.Update(time);
        FSMUpdate(_fsm);
    }

    protected abstract void CreateFSM(FiniteStateMachine<T, E, S> fsm);
    protected abstract void FSMInit();
    protected abstract void FSMUpdate(FiniteStateMachine<T, E, S> fsm);

    

    public void SendEvent(E e)
    {
        _fsm.SendEvent(e);
    }

    void OnDestroy()
    {
        _fsm.End();
    }
}
