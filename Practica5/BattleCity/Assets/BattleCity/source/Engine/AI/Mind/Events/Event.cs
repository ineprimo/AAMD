using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mind.Events
{
    public abstract class MindEvent : Condition 
    {
        
        //private Hash128 _id;
        private bool _enable = false;
        public bool Enable
        {
            get
            {
                return _enable;
            }

            set
            {
                _enable = value;
            }
        }

        /*public Hash128 ID
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }*/

        public override bool Check()
        {
            bool e = _enable;
            _enable = false;
            return e;
        }

        public override void OnStart()
        {

        }

        public override void OnAwake()
        {
            
        }

    }
}
