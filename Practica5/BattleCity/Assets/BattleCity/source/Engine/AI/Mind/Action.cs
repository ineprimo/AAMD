using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mind
{
    public enum BehaviorStatus { RUNNING = 0, SUCCESS, FAILURE }
    public abstract class Action : Activity
    {
        public abstract BehaviorStatus OnUpdate(float time);
        public abstract void OnFinish();
        public abstract void OnAbort();

        public void Output(string name, object value)
        {
            Debug.Assert(((ActionData)ActivityData).outputs.Contains(name), "El campo "+name+" no es output de de la acción " + GetType().Name);
            ParentBehavior.Blackboard[name]=value;
        }
    }
}