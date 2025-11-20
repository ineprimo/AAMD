using Mind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionRunning : MonoBehaviour
{
    public KeyCode key;
    public string _conditionName;
    public MonoBehaviour _componentLinked;

    private Condition _condition;
    private Blackboard blackboard;

    private void Awake()
    {
        blackboard = new Blackboard(null);
        TestingBehavior behavior = new TestingBehavior();
        behavior.Blackboard = blackboard;

        if (_componentLinked == null)
            _condition = MindMgr.Instance.CreateConditionByName(_conditionName,this.gameObject, behavior);
        else
            _condition = MindMgr.Instance.CreateConditionByLinked(_componentLinked, this.gameObject, behavior);

        if (_condition != null)
        {
            System.Type linkedType = MindMgr.GetLinkedType(_condition);
            if (linkedType != null)
            {
                _condition.SetComponent(GetComponent(linkedType));
            }
            _condition.OnAwake();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            _condition.OnStart();
            Debug.Log("Condicion " + _condition.GetType().Name + " se cumple " + _condition.Check());
        }
    }
}
