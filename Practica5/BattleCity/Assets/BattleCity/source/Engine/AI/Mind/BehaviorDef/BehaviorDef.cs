using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mind
{
    public enum BehaviorType { FSM }
    public class BehaviorDef : ScriptableObject
    {
 
        [SerializeField]
        private BehaviorType type;

        public BehaviorType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
    }
}
