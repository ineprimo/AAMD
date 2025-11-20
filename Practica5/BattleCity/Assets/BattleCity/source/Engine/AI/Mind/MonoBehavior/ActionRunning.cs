using Mind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingBehavior : Behavior
{
    public override void OnAbort()
    {
    }

    public override void OnAwake()
    {

    }

    public override void OnFinish()
    {

    }

    public override void OnStart()
    {

    }

    public override BehaviorStatus OnUpdate(float time)
    {
        return BehaviorStatus.SUCCESS;
    }
}

public class ActionRunning : MonoBehaviour
{
    public string _actionName;
    public MonoBehaviour _componentLinked;

    private Action _action;
    private Blackboard blackboard;


    private void Awake()
    {
        blackboard = new Blackboard(null);
        TestingBehavior behavior = new TestingBehavior();
        behavior.Blackboard = blackboard;
        if (_componentLinked == null)
            _action = MindMgr.Instance.CreateActionByName(_actionName,this.gameObject, behavior);
        else
            _action = MindMgr.Instance.CreateActionByLinked(_componentLinked, this.gameObject, behavior);


        if (_action != null)
        {
            System.Type linkedType = MindMgr.GetLinkedType(_action);
            if (linkedType != null)
            {
                _action.SetComponent(GetComponent(linkedType));
            }
            _action.OnAwake();
        }
        else
            Debug.LogError("no se ha podido crear la acción _actionName");
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        _action.OnStart();
    }

    private void OnDisable()
    {
        _action.OnFinish();
    }

    // Update is called once per frame
    void Update()
    {
        BehaviorStatus status = _action.OnUpdate(Time.deltaTime);
        if (status != BehaviorStatus.RUNNING)
        {
            this.enabled = false;
            Debug.Log("Action " + _actionName + " ha terminado con valor " + status + " Desactivamos la ejecución del ejecutor");
        }
    }
}
