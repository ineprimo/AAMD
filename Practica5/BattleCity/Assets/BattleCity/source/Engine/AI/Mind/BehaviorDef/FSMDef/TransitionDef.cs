using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mind
{
    [System.Serializable]
    public class TransitionDef
    {
        [SerializeField]
        private string from;
        [SerializeField]
        private string to;
        [SerializeField]
        private string when;
        [SerializeField]
        private Transition.BehaviorFromStatus fromStatus = Transition.BehaviorFromStatus.NOT_IMPORTANT;
        [SerializeField]
        private bool inverseConditon=false;
        [SerializeField]
        private float prob=1f;
        [SerializeField]
        private StackedAction stacked;

        public string From
        {
            get
            {
                return from;
            }

            set
            {
                from = value;
            }
        }

        public string When
        {
            get
            {
                return when;
            }

            set
            {
                when = value;
            }
        }

        public string To
        {
            get
            {
                return to;
            }

            set
            {
                to = value;
            }
        }

        public Transition.BehaviorFromStatus FronmStatus
        {
            get
            {
                return fromStatus;
            }

            set
            {
                fromStatus = value;
            }
        }


        public StackedAction Stacked
        {
            get
            {
                return stacked;
            }

            set
            {
                stacked = value;
            }
        }

        public float Prob
        {
            get
            {
                return prob;
            }

            set
            {
                prob = value;
            }
        }

        public bool IsInverseCondition
        {
            get { return inverseConditon; }
            set { inverseConditon = value; }
        }
    }
}
