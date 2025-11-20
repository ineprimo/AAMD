using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mind;
using Mind.Behaviors;


public abstract class FSMRunning<T> : Tickable, IBehaviorExecutor where T : FSM
{
    protected FSM _fsm;

    public FSM FSM
    {
        get { return _fsm; }
    }

    public void ResumeBehavior()
    {
        this.enabled = true;
    }

    public void PauseBehavior()
    {
        this.enabled = false;
    }

    private void Awake()
    {
        _fsm = (FSM) MindMgr.Instance.CreateActionByType(typeof(T), this.gameObject, null);
        if (_fsm != null)
        {
            System.Type linkedType = MindMgr.GetLinkedType(_fsm);
            if (linkedType != null)
            {
                Component[] components = GetComponents(linkedType);
                if (components == null || components.Length == 0)
                    Debug.LogError("No se ha encontrado los parámetros de tipo "+ linkedType + " en el GameOBject");
                Component selected = GetParameter(components);
                if (selected == null)
                    Debug.LogError("Debes marcar la máquina de estados de ejecución como <none>");
                _fsm.SetComponent(selected);
            }
            else
                Debug.LogError("No se ha definido parámetros para la FSM "+ typeof(T) + " y son necesarios para poder ejecutarlo");

            _fsm.OnAwake();
        }
        else
            Debug.LogError("no se ha podido crear la acción _actionName");
    }

    protected Component GetParameter(Component[] components)
    {
        Component selected = null;
        for(int i = 0; selected == null && i < components.Length; ++i)
        {
            FSMParameters fsmParameter = (FSMParameters)components[i];
            if (fsmParameter.behaviorLinked == "<none>")
                selected = fsmParameter;
        }
        return selected;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _fsm.OnStart();
    }


    protected override void Tick(float time)
    {
        BehaviorStatus status = _fsm.OnUpdate(time);
        if (status != BehaviorStatus.RUNNING)
        {
            _fsm.OnFinish();
            Debug.Log("la FSM " + _fsm.BehaviorName + " ha terminado con valor " + status);
        }
    }
}
