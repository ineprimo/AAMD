using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Enemy : FSMExecutor<Enemy> {

    public int _life;
    public float _speed;
    public Transform _initPoint;
    public Transform _endPoint;
    public float _stoppingDistance;
    public float _AttackTime;
    public float _visionDistance;

    private GameObject _target;

    protected override void FSMStart(FiniteStateMachine<Enemy> fsm)
    {

        fsm.AddState(new WanderState(this), true);
        fsm.AddState(new AttackState(this), false);
        fsm.AddTransition("WanderState", "AttackState", "PLAYER_VISTO");
        fsm.AddTransition("AttackState", "WanderState", "PLAYER_DEJADO_VER");
    }


    protected override void FSMUpdate(FiniteStateMachine<Enemy> fsm)
    {
        RaycastHit hitInfo;
        Ray r = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (fsm.CurrentState == "WanderState")
        {
            if (Physics.Raycast(r, out hitInfo, _visionDistance))
            {
                Target = hitInfo.collider.gameObject;
                if (hitInfo.collider.gameObject.tag == "Player")
                    fsm.Emmit("PLAYER_VISTO");
            }
        }
        else if (fsm.CurrentState == "AttackState")
        {
            bool noVisto = false;
            if (Physics.Raycast(r, out hitInfo, _visionDistance))
            {
                if (hitInfo.collider.gameObject.tag != "Player")
                    noVisto = true;
            }
            else
                noVisto = true;

            if(noVisto)
            {
                fsm.Emmit("PLAYER_DEJADO_VER");
                Target = null;
            }
        }
    }


    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

}

public class WanderState : StateBehavior<Enemy>
{
    private Transform _target;
    private CharacterController _controller;
    private int _point = 0;
    public WanderState(Enemy executor) : base(executor)
    {

    }

    public override void Init()
    {
        base.Init();
        _target = Component._initPoint;
        _point = 0;
        _controller = Component.GetComponent<CharacterController>();
    }

    public override void Update()
    {
        base.Update();



        Vector3 direction = _target.position - Component.transform.position;

        if (direction.sqrMagnitude < (Component._stoppingDistance * Component._stoppingDistance))
        {
            if (_point == 0)
            {
                _target = Component._endPoint;
                _point = 1;
            }
            else
            {
                _target = Component._initPoint;
                _point = 0;
            }
        }

        direction = direction.normalized;

        _controller.Move(new Vector3(direction.x * Component._speed * Time.deltaTime, 0, 0));

    }
}

public class AttackState : StateBehavior<Enemy>
{
    private float _time;
    public AttackState(Enemy executor) : base(executor)
    {

    }

    public override void Init()
    {
        base.Init();
        Attack();
        _time = Component._AttackTime;
    }

    public override void Update()
    {
        base.Update();
        _time -= Time.deltaTime;
        if (_time <= 0f)
        {
            Attack();
            _time = Component._AttackTime;
        }
    }

    private void Attack()
    {
        Debug.LogWarning("Atacando a " + Component.Target);
    }
}
*/  