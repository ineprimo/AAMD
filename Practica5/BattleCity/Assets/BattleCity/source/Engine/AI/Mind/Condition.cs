using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mind
{
    public abstract class Condition : Activity
    {
        public abstract bool Check();
    }
}
