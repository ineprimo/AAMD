using Mind.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mind
{
    public abstract class Behavior : Action
    {
        private string _name;
        private Perception _perception;
        private Blackboard _blackboard;
        private MonoBehaviour _monoBehavior;

        public string BehaviorName { get { return _name; } set { _name = value; } }
        public Perception Perception { get { return _perception; } set { _perception = value; } }
        public Blackboard Blackboard { get { return _blackboard; } set { _blackboard = value; } }



        public static T GetEvent<T>(Dictionary<string, Condition> keyValuePairs) where T : MindEvent
        {
            return (T)keyValuePairs[typeof(T).Name];
        }
    }
}
