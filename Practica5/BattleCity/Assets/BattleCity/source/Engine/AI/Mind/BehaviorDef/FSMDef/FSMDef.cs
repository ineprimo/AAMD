using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mind
{

    public enum StackedAction { NONE, PUSH, POP }
    public enum StateType { NORMAL, INITIAL, FINAL }

    [CreateAssetMenu(menuName = "Mind/FSM")]
    public class FSMDef : BehaviorDef
    {
        [SerializeField]
        private StateDef[] states;
        [SerializeField]
        private TransitionDef[] transitions;
        [SerializeField]
        private string perception;


        public string Perception
        {
            get { return perception; }
            set { perception = value; }
        }

        public StateDef[] States
        {
            get
            {
                return states;
            }

            set
            {
                states = value;
            }
        }


        public TransitionDef[] Transitions
        {
            get
            {
                return transitions;
            }

            set
            {
                transitions = value;
            }
        }
    }
}
