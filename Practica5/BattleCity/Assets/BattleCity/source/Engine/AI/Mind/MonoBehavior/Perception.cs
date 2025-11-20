using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mind
{
    public abstract class Perception : Action
    {

        public override BehaviorStatus OnUpdate(float time)
        {
            Perceive(time);
            return BehaviorStatus.RUNNING;
        }

        public abstract void Perceive(float time);


        public override void OnAbort()
        {

        }

    }
}
