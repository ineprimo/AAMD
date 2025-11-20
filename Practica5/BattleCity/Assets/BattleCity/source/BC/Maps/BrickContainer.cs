using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BrickContainer : MonoBehaviour
{

    public abstract void ChildDestroy(Breakable b);
}
