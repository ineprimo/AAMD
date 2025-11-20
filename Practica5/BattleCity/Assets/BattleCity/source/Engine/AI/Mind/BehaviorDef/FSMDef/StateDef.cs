using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mind
{
    [System.Serializable]
    public class StateDef
    {
        [SerializeField]
        private string state;
        [SerializeField]
        private StateType type;
        [SerializeField]
        private string action;

        public string State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        public StateType Type
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

        public string Action
        {
            get {
                if (action == null || action == "")
                    return State;
                return action;
            }
            set { action = value; }
        }
    }
}
