using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : PlayerInputControllerBase
{
    public TankMove tankMove;
    public TankFire tankFire;

    protected Vector2 lastDir;
    protected bool fire;
    public void OnMove(Vector2 dir)
    {
        if (!enabled)
            return;
        lastDir = dir;
        tankMove.Move(dir);
    }

    public void Fire(bool b)
    {
        if (!enabled)
            return;
        fire = b;
        if(b)
            tankFire.Fire();
    }

    internal PerceptionBase.ACTION_TYPE GetLastInput()
    {
        return ConvertDirectToAction(lastDir);
    }

    internal static PerceptionBase.ACTION_TYPE ConvertDirectToAction(Vector2 lastDir)
    {
        if (Mathf.Abs(lastDir.x) < 0.01f && Mathf.Abs(lastDir.y) <= 0.01)
            return PerceptionBase.ACTION_TYPE.NOTHING;

        if (Mathf.Abs(lastDir.y) > Mathf.Abs(lastDir.x))
        {
            //vertical
            if (lastDir.y > 0f)
                return PerceptionBase.ACTION_TYPE.MOVE_UP;
            else
                return PerceptionBase.ACTION_TYPE.MOVE_DOWN;
        }
        else
        {
            //horizontal
            if (lastDir.x > 0f)
                return PerceptionBase.ACTION_TYPE.MOVE_RIGHT;
            else
                return PerceptionBase.ACTION_TYPE.MOVE_LEFT;
        }
    }
}
