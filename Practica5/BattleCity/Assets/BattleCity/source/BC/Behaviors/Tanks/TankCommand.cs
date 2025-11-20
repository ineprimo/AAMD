using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCommand : MonoBehaviour
{
    public TankMove tankMove;
    public TankFire tankFire;

    private Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tankMove.Move(moveDirection);
    }

    public void MoveUp()
    {
        moveDirection = Vector2.up;

    }

    public void MoveDown()
    {
        moveDirection = Vector2.down;
    }

    public void MoveLeft()
    {
        moveDirection = Vector2.left;
    }

    public void MoveRight()
    {
        moveDirection = Vector2.right;
    }

    public void MoveStop()
    {
        moveDirection = Vector2.zero;
    }

    public void Fire()
    {
        tankFire.Fire();
    }

    public void RunCommand(int action)
    {
        if (action == (int)TankPerception.ACTION_TYPE.NOTHING)
        {
            MoveStop();
        }
        else if (action == (int)TankPerception.ACTION_TYPE.MOVE_UP)
        {
            MoveUp();
        }
        else if (action == (int)TankPerception.ACTION_TYPE.MOVE_DOWN)
        {
            MoveDown();
        }
        else if (action == (int)TankPerception.ACTION_TYPE.MOVE_LEFT)
        {
            MoveLeft();
        }
        else if (action == (int)TankPerception.ACTION_TYPE.MOVE_RIGHT)
        {
            MoveRight();
        }
    }

    public void RunCommand(bool fire)
    {
        if (fire)
            Fire();
    }

    public void RunCommand(int action, bool fire)
    {
        RunCommand(action);
        RunCommand(fire);
    }
}
