/*using Mind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFSM : MonoBehaviour
{
    public enum States { WANDER, SEEK, ATTACK}
    private States state;
    private Blackboard blackboard;
    // Start is called before the first frame update
    void Start()
    {
        state = States.WANDER;
        blackboard = new Blackboard(null);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case States.WANDER:
                state = Wander(Time.deltaTime);
                break;
            case States.SEEK:
                state = Seek(Time.deltaTime);
                break;
            case States.ATTACK:
                state = Attak(Time.deltaTime);
                break;
        }
    }
    protected States Wander(float dt)
    {
        //Do something...
        if (blackboard.Get<bool>("Target seen"))
            return States.SEEK;
        return States.WANDER;
    }

    protected States Seek(float dt)
    {
        //Do something...
        if (blackboard.Get<bool>("InRange"))
            return States.ATTACK;
        return States.SEEK;
    }
    protected States Attak(float dt)
    {
        //Do something...
        if (!blackboard.Get<bool>("Target  seen") || blackboard.Get<bool>("Dead Target"))
            return States.WANDER;
        return States.ATTACK;
    }
}

public abstract class State
{
    private GameObject gameObject;
    private HFSM fsm;
    public State() {}
    public void Init(GameObject g, HFSM f)
    {
        gameObject = g;
        fsm = f;
    }
    //Esto es llamado la primera vez que se ejecuta el estado.
    public abstract void Enter();
    //Es llamado por cada Update
    public abstract void Update(float dt);
    public abstract void Exit();
}

public abstract class Transition
{
    private GameObject gameObject;
    private HFSM fsm;
    public Transition() { }
    public void Init(GameObject g, HFSM f)
    {
        gameObject = g;
        fsm = f;
    }
    public abstract void Enter();
    public abstract bool Check();
    public abstract State NexState();
}

public abstract class HFSM : MonoBehaviour
{
    private State current;
    private Blackboard blackboard;
    Dictionary<State, Transition[]> fsmStructure;
    public void InitHFSM(State initial)
    {
        current = initial;
        blackboard = new Blackboard(null);
        fsmStructure = new Dictionary<State, Transition[]>();
        current.Enter();
    }

    private void Update()
    {
        current.Update(Time.deltaTime);
        Transition[] transitions = fsmStructure[current];
        bool changState = false;
        for(int i = 0; !changState && i< transitions.Length; i++)
        {
            Transition t = transitions[i];
            if(t.Check())
            {
                current.Exit();
                current = t.NexState();
                current.Enter();
                changState = true;
            }
        }
    }
    public HFSM AddState(State s, Transition[] transitions )
    {
        s.Init(this.gameObject, this);
        for(int i = 0; i < transitions.Length; i++)
        {
            transitions[i].Init(this.gameObject, this);
        }
        fsmStructure.Add(s, transitions);
        return this;
    }
}*/
