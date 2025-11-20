using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mind
{
    public class Transition
    {
        public enum BehaviorFromStatus { FINISH = 0, SUCCESS = 1, FAILURE = 2, NOT_IMPORTANT = 3 }
        public const string ANY = "ANY";
        private string _from;
        private Condition _condition;
        private bool _inverseCondition;
        private List<TransitionTo> _transitions;
        private BehaviorFromStatus _fromStatus;
        private float _acum;
        private StackedAction _stacked;

        public Transition(TransitionDef def, Condition cond)
        {
            _transitions = new List<TransitionTo>();
            _from = def.From;
            Condition = cond;
            _stacked = def.Stacked;
            _fromStatus = def.FronmStatus;
            _inverseCondition = def.IsInverseCondition;
            _transitions.Add(new TransitionTo(float.MinValue, float.MaxValue, def.Prob, def.To));
        }

        public void AddTransitionDef(TransitionDef def)
        {
            _transitions.Add(new TransitionTo(float.MinValue, float.MaxValue, def.Prob, def.To));
        }

        public void RecalculateProbability()
        {
            _acum = 0f;
            for (int i = 0; i < _transitions.Count; ++i)
            {
                float aux = _acum;
                _acum += _transitions[i].Prob;
                //(float min, float max, float pro, string to)
                _transitions[i].Min = aux;
                _transitions[i].Max = _acum;
            }
        }

        public TransitionTo GetTransitionTo(string to)
        {
            TransitionTo found = null;
            for (int i = 0; found == null && i < _transitions.Count; ++i)
            {
                TransitionTo tTo = _transitions[i];
                if (tTo.To == to)
                    found = _transitions[i];
            }

            return found;
        }

        public List<TransitionTo> GetTransitionToList()
        {
            return _transitions;
        }

        public bool Transit(out string next, bool[] combinedEvents)
        {
            bool found = false;
            next = null;
            bool conditionResult = Condition.Check();
            bool needTransit = _inverseCondition ? !conditionResult : conditionResult;
            if (needTransit && (_fromStatus == BehaviorFromStatus.NOT_IMPORTANT || combinedEvents[(int)_fromStatus]))
            {
                if (_transitions.Count == 1 || _acum == 0f) // si no tengo probabilidad acumulada es que tengo dos condicines que llevan al mismo sitio con que una se cumpla transita.
                {
                    found = true;
                    next = _transitions[0].To;
                }
                else
                {
                    float f = Random.Range(0, _acum);
                    if (f == _acum)
                    {
                        next = _transitions[_transitions.Count-1].To;
                        if (next == null)
                            Debug.LogError("TO is null " + Condition.Name);
                    }
                    else
                    {
                        for (int i = 0; !found && i < _transitions.Count; ++i)
                        {

                            if (_transitions[i].CheckProbability(f))
                            {
                                found = true;
                                next = _transitions[i].To;
                                if (next == null)
                                    Debug.LogError("TO is null " + Condition.Name);
                            }
                        }
                    }
                }
            }
            return found;
        }

        public StackedAction Stack
        {
            get { return _stacked; }
        }

        public Condition Condition
        {
            get
            {
                return _condition;
            }

            set
            {
                _condition = value;
            }
        }

        protected StackedAction ConverTo(string stack)
        {
            if (stack.ToLower().CompareTo("push") == 0)
                return StackedAction.PUSH;
            else if (stack.ToLower().CompareTo("pop") == 0)
                return StackedAction.POP;
            return StackedAction.NONE;
        }
    }
}
