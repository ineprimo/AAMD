using Mind.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mind.Behaviors
{
    public abstract class FSM : Behavior
    {
        private Dictionary<string, Action> _states;
        private Dictionary<string, Condition> _conditions;
        private Dictionary<string, List<Transition>> _transitions;
        private Dictionary<string, bool> _finals;
        private string _initialState;
        private FSMDef _fsmDef;
        private Dictionary<Type, System.Action> _onStartCallbacks;
        private Dictionary<Type, System.Action> _onFinishCallbacks;
        private bool[] eventActivationsToCombinedConditions = new bool[3];
        private Stack<string> _currentStateStack;

        public FSMDef Definition
        {
            get { return _fsmDef; }
            set { _fsmDef = value; }
        }

        public override void SetComponent(Component component)
        {
            FSMParameters parameters = (FSMParameters)component;
            Definition = parameters.FSMDef;
            base.SetComponent(component);
        }

        public void OnStart<T>(System.Action action) where T : Action
        {
            Type t = typeof(T);
            if (_states.ContainsKey(t.Name))
            {
                if (_onStartCallbacks.ContainsKey(t))
                {
                    System.Action act = _onStartCallbacks[t];
                    act += action;
                }
                else
                {
                    System.Action act = null;
                    act += action;
                    _onStartCallbacks.Add(t, act);
                }
            }
        }

        public void OnFinish<T>(System.Action action) where T : Action
        {
            Type t = typeof(T);
            if (_states.ContainsKey(t.Name))
            {
                if (_onFinishCallbacks.ContainsKey(t))
                {
                    System.Action act = _onFinishCallbacks[t];
                    act += action;
                }
                else
                {
                    System.Action act = null;
                    act += action;
                    _onFinishCallbacks.Add(t, act);
                }
            }
        }

        public void OnStartCallDel<T>( System.Action action) where T : Action
        {
            Type t = typeof(T);
            if (_states.ContainsKey(t.Name))
            {
                if (_onStartCallbacks.ContainsKey(t))
                {
                    System.Action act = _onStartCallbacks[t];
                    act -= (System.Action) action;
                    if (act == null)
                        _onStartCallbacks.Remove(t);
                }
            }
        }

        public void OnFinishCallDel<T>( System.Action action) where T : Action
        {
            Type t = typeof(T);
            if (_states.ContainsKey(t.Name))
            {
                if (_onFinishCallbacks.ContainsKey(t))
                {
                    System.Action act = _onFinishCallbacks[t];
                    act -= action;
                    if (act == null)
                        _onFinishCallbacks.Remove(t);
                }
            }
        }

        public override void OnAwake()
        {
            _onStartCallbacks = new Dictionary<Type, System.Action>();
            _onFinishCallbacks = new Dictionary<Type, System.Action>();
            //Enlazar la blackboar privada mía con la externa. La idea es que el comportamiento puede tener parámetros de entrada
            //y de salida. Estos se escribiran en la pizarra que se le pasa por parámetro.
            //sin embargo el crea una pizarra interna para sus comportamientos.
            //Hay que gestionar esos parámetros de entrada y de salida.
            Blackboard = new Blackboard(ParentBehavior != null ? ParentBehavior.Blackboard : null);
            _currentStateStack = new Stack<string>();
            _finals = new Dictionary<string, bool>();
            StateDef[] states = _fsmDef.States;
            _states = new Dictionary<string, Action>();
            _conditions = new Dictionary<string, Condition>();
            _transitions = new Dictionary<string, List<Transition>>();
            for (int i = 0; i < states.Length; ++i)
            {
                if (MindMgr.Instance.ActionExist(states[i].Action))
                {
                    Action action = MindMgr.Instance.CreateActionByName(states[i].Action,GameObject, this);
                    SetLinkedParameters(action);
                    _states.Add(states[i].State, action);
                    if (states[i].Type == StateType.INITIAL)
                        _initialState = states[i].State;
                }
                else
                    Debug.LogError("No existe la implementación del estado "+ states[i].State);

            }

            _conditions.Add(typeof(EventActionFinished).Name, MindMgr.Instance.CreateConditionByType(typeof(EventActionFinished), null, null));
            _conditions.Add(typeof(EventActionFailure).Name, MindMgr.Instance.CreateConditionByType(typeof(EventActionFailure), null, null));
            _conditions.Add(typeof(EventActionSuccess).Name, MindMgr.Instance.CreateConditionByType(typeof(EventActionSuccess), null, null));

            TransitionDef[] transitions = _fsmDef.Transitions;
            for (int i = 0; i < transitions.Length; ++i)
            {
                if (MindMgr.Instance.ConditionExist(transitions[i].When))
                {
                    Condition condition = null;
                    if (_conditions.ContainsKey(transitions[i].When))
                        condition = _conditions[transitions[i].When];
                    else
                    {
                        condition = MindMgr.Instance.CreateConditionByName(transitions[i].When, GameObject, this);
                        _conditions.Add(transitions[i].When, condition);
                    }

                    SetLinkedParameters(condition);
                    if (transitions[i].From == Transition.ANY)
                    {
                        foreach (var statePair in _states)
                        {
                            if (!IsTheSameFromAndTo(statePair.Value.Name, transitions[i]))
                            {
                                Debug.LogWarning("+++ " + statePair.Value.Name);
                                AddTransition(statePair.Value.Name, transitions[i], condition);
                            }
                        }
                    }
                    else
                        AddTransition(transitions[i].From, transitions[i], condition);
                }
                else
                    Debug.LogError("No existe la implementación de la condicion " + transitions[i].When);

            }

            foreach(List<Transition> list in _transitions.Values)
            {
                foreach(Transition transition in list)
                {
                    transition.RecalculateProbability();
                }
            }

            if (_fsmDef.Perception != null && _fsmDef.Perception != "")
            {
                Perception = (Perception)MindMgr.Instance.CreateActionByName(_fsmDef.Perception, GameObject, this);
                if (Perception == null)
                    Debug.LogError("La percepción " + _fsmDef.Perception + " no existe ");
                SetLinkedParameters(Perception);
            }
            else
                Perception = null;

            foreach (Action actions in _states.Values)
            {
                actions.OnAwake();
            }

            if(Perception!=null)
                Perception.OnAwake();

            foreach (Condition condition in _conditions.Values)
            {
                condition.OnAwake();
            }
        }

        public string CurrentStateStack
        {
            get
            {
                return _currentStateStack.Peek();
            }
        }

        /*public FSM(FSMDef def,MonoBehaviour monoBehavior) : base(monoBehavior)
        {
            _currentStateStack = new Stack<string>();
            //_finder = finder;
            _finals = new Dictionary<string, bool>();

            StateDef[] states = def.States;
            string[] conditions = def.Conditions;*/
        /* _states = new Dictionary<string, Action>();
         _transitions = new Dictionary<string, List<Transition>>();

         string[] inputs = def.Inputs;
         if (inputs != null && inputs.Length > 0)
         {
             this.Inputs = new string[inputs.Length];
             for (int i = 0; i < inputs.Length; ++i)
             {
                 this.Inputs[i] = inputs[i];
             }
         }

         string[] outputs = def.Outputs;
         if (outputs != null && inputs.Length > 0)
         {
             for (int i = 0; i < outputs.Length; ++i)
             {
                 this.Outputs[i] = outputs[i];
             }
         }*/

        /*for (int i = 0; i < states.Length; ++i)
        {
            //if (MindMgr.Instance.ActivityExist(states[i].State))
            {
                /*ActivityDef activityDef = null;
                Action action = (Action)CreateActivity(states[i], out activityDef);
                if(action != null)
                {
                    _states.Add(states[i].State, action);
                    _finals.Add(states[i].State, states[i].Type == StateType.FINAL);
                    if (states[i].Type == StateType.INITIAL)
                        _initialState = states[i].State;

                    //TODO => pasar los parámetros
                    //action.Inputs = activityDef.Input;
                    //action.Outputs = activityDef.Output;
                }*/
        //}
        //else
        //Debug.LogError("No existe la implementación del estado "+ states[i].State);

        //}

        /*TransitionDef[] transitions = def.Transitions;
        for (int i = 0; i < transitions.Length; ++i)
        {*/
        /*ActivityDef activityDef = null;
        Condition condition = (Condition) CreateActivity(states[i], out activityDef);
        if (condition != null)
        {
            if (transitions[i].From == Transition.ANY)
            {
                foreach (var statePair in _states)
                {
                    if (!IsTheSameFromAndTo(statePair.Value.Name, transitions[i]))
                    {
                        Debug.LogWarning("+++ " + statePair.Value.Name);
                        AddTransition(statePair.Value.Name, transitions[i], condition);
                    }
                }
            }
            else
                AddTransition(transitions[i].From, transitions[i], condition);
        }*/
        //}

        //Generamos las probabilidades
        /*foreach(var transList in _transitions.Values)
        {
            for (int i = 0; i < transList.Count; ++i)
            {
                transList[i].RecalculateProbability();
            }
        }*/

        //}

        protected bool IsTheSameFromAndTo(string from, TransitionDef trans)
        {
            return trans.To == from;
        }

        protected void AddTransition(string from, TransitionDef transitionDef, Condition condition)
        {
            List<Transition> transitionList;
            if (_transitions.ContainsKey(from))
                transitionList = _transitions[from];
            else
            {
                transitionList = new List<Transition>();
                _transitions.Add(from, transitionList);
            }
            Transition tran = FindTransitionWhen(transitionList, condition.Name);
            if (tran == null)
            {
                tran = new Transition(transitionDef, condition);
                transitionList.Add(tran);
            }
            else
            {
                tran.AddTransitionDef(transitionDef);
            }

        }

        public Transition FindTransitionWhen(List<Transition> transitions, string conditionName)
        {
            Transition transition = null;
            for (int i = 0; transition == null && i < transitions.Count; ++i)
            {
                Transition tran = transitions[i];
                if(tran.Condition.Name == conditionName)
                {
                    transition = tran;
                }
            }

            return transition;
        }

        public Transition GetTransition(string from, string to)
        {
            Transition transition = null;
            if (_transitions.ContainsKey(from))
            {
                List<Transition> transitions = _transitions[from];
                for (int i = 0; transition == null && i < transitions.Count; ++i)
                {
                    Transition tran = transitions[i];
                    TransitionTo tto = tran.GetTransitionTo(to);
                    if(tto != null)
                    {
                        transition = tran;
                    }
                }
            }
            return transition;
        }

        /*public Activity CreateActivity(StateDef stateDef, out ActivityDef activityDef)
        {
            activityDef = null;
            var implementations = MindMgr.Instance.GetImplentations(stateDef.State);
            //if (implementations.Count > 1 && (stateDef.Implementation == null || stateDef.Implementation == ""))
                //Debug.LogError("hay multiples implementaciones de la acción " + stateDef.State + " y n podemos decidir cual instanciar ");
            //else
            {
                if (implementations.Count == 1)
                {
                    foreach (var adef in implementations.Values)
                        activityDef = adef;
                }
                else
                {
                    //System.Type t = System.Type.GetType(stateDef.Implementation);
                    //if (t != null && implementations.ContainsKey(t))
                        //activityDef = implementations[t];
                    //else
                        //Debug.LogError("La implementación  " + t + " del estad0 " + stateDef.State + " no existe ");

                }

                if (activityDef != null)
                {
                    //Activity activity = (Activity)Activator.CreateInstance(activityDef.Type);
                    //return activity;
                }
            }
            return null;
        }*/

        public override void OnAbort()
        {

        }

        public override void OnFinish()
        {
            if(Perception!=null)
                Perception.OnFinish();
        }

        public override void OnStart()
        {
            Debug.LogWarning("OnStart");
            //_currentState = _initialState;
            _currentStateStack.Clear();
            _currentStateStack.Push(_initialState);
            Action action = _states[_currentStateStack.Peek()];
            if (_onStartCallbacks.ContainsKey(action.GetType()))
                _onStartCallbacks[action.GetType()]();
            action.OnStart();
            if(Perception!=null)
                Perception.OnStart();
        }

        public override BehaviorStatus OnUpdate(float time)
        {
            BehaviorStatus status = BehaviorStatus.RUNNING;
            bool needRefreshEvents = false;

            if (Perception != null)
                Perception.OnUpdate(time);

            BehaviorStatus stateStatus = _states[_currentStateStack.Peek()].OnUpdate(time);
            eventActivationsToCombinedConditions[0] = false;
            eventActivationsToCombinedConditions[1] = false;
            eventActivationsToCombinedConditions[2] = false;
            if (stateStatus != BehaviorStatus.RUNNING) //ha terminado
            {
                FinishState();
                EventActionFinished eventFinish = GetEvent<EventActionFinished>(_conditions);
                eventFinish.Enable = true;
                eventActivationsToCombinedConditions[(int) Transition.BehaviorFromStatus.FINISH] = true;
                needRefreshEvents = true;
                if (stateStatus == BehaviorStatus.SUCCESS)
                {
                    EventActionSuccess eventSuccess = GetEvent<EventActionSuccess>(_conditions);
                    eventSuccess.Enable = true;
                    eventActivationsToCombinedConditions[(int)Transition.BehaviorFromStatus.SUCCESS] = true;
                }
                else
                {
                    EventActionFailure eventFailure = GetEvent<EventActionFailure>(_conditions);
                    eventFailure.Enable = true;
                    eventActivationsToCombinedConditions[(int)Transition.BehaviorFromStatus.FAILURE] = true;
                }
                if (_finals.ContainsKey(_currentStateStack.Peek()))
                {
                    status = stateStatus;
                }
            }


            string next;
            if (_transitions.ContainsKey(_currentStateStack.Peek()))
            {
                List<Transition> transitionList = _transitions[_currentStateStack.Peek()];
                bool stateChange = false;
                for (int i = 0; !stateChange && i < transitionList.Count; ++i)
                {
                    Transition transition = transitionList[i];
                    if (transition.Transit(out next, eventActivationsToCombinedConditions))
                    {
                        Debug.Log("Next state " + next);
                        if (next != null)
                        {
                            AbortState();
                            FinishState();
                            stateChange = true;
                            //Cambiando escena
                            if (transition.Stack == StackedAction.NONE)
                            {
                                _currentStateStack.Pop();
                                _currentStateStack.Push(next);
                            }
                            else if (transition.Stack == StackedAction.PUSH)
                            {
                                _currentStateStack.Push(next);
                            }
                            else if (transition.Stack == StackedAction.POP)
                            {
                                _currentStateStack.Pop();
                                if (next != null && next != "")
                                {
                                    _currentStateStack.Pop();
                                    _currentStateStack.Push(next);
                                }
                            }
                            Debug.Assert(_states.ContainsKey(_currentStateStack.Peek()), " Error no se encuentra " + _currentStateStack.Peek() + " en la lista de estados");
                            Action action = _states[_currentStateStack.Peek()];
                            if (_onStartCallbacks.ContainsKey(action.GetType()))
                                _onStartCallbacks[action.GetType()]();
                            action.OnStart();
                        }
                    }
                }
            }

            if(needRefreshEvents)
            {
                needRefreshEvents = false;
                GetEvent<EventActionFinished>(_conditions).Enable = false;
                GetEvent<EventActionSuccess>(_conditions).Enable = false;
                GetEvent<EventActionFailure>(_conditions).Enable = false;
            }

            return status;
        }

        public void FinishState()
        {
            Action action = _states[_currentStateStack.Peek()];
            if (_onFinishCallbacks.ContainsKey(action.GetType()))
                _onFinishCallbacks[action.GetType()]();
            action.OnFinish();
            if (Perception != null)
                Perception.OnFinish();
        }

        public void AbortState()
        {
            _states[_currentStateStack.Peek()].OnAbort();
            if (Perception != null)
                Perception.OnAbort();
        }

        protected void SetLinkedParameters(Activity activity)
        {
            if (activity != null)
            {
                System.Type linkedType = MindMgr.GetLinkedType(activity);
                if (linkedType != null)
                {
                    Component[] components = GameObject.GetComponents(linkedType);
                    Component selected = GetCorrectParameter(components, linkedType);
                    if (selected == null)
                        Debug.LogError("no hay ningun de los componentes del tipo "+ linkedType+" marcado para la FSM "+this.BehaviorName);
                    activity.SetComponent(selected);
                }
            }
            else
                Debug.LogError("SetLinkedParameters " + activity);
        }

        protected Component GetCorrectParameter(Component[] components, System.Type linkedType)
        {
            Component selected = null;
            bool printValues = false;
            string message = printValues ? "FMS States will be printed" : "FMS States won't be printed";

            Debug.Log(message);
            for (int i = 0; selected == null && i < components.Length; ++i)
            {
                //Debug.Log(components[i].GetType().Name);
                Parameters fsmParameter = (Parameters)components[i];
                if (fsmParameter.behaviorLinked == LinkedComponent.GetType().Name && linkedType == fsmParameter.GetType())
                    selected = fsmParameter;
            }
            return selected;
        }
    }
}
