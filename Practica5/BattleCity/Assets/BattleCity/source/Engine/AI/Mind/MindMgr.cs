using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Mind
{
    public class ConditionData : ActivityData
    {
        public ConditionData(string n, Type t, string[] inp) : base(n,t,inp){}
    }

    public  class ActionData : ActivityData
    {
        public HashSet<string> outputs;

        public ActionData(string n, Type t, string[] inp, string[] outp) : base(n,t,inp)
        {
            outputs = new HashSet<string>();
            if (outp != null)
            {
                for (int i = 0; i < outp.Length; ++i)
                {
                    outputs.Add(outp[i]);
                }
            }
        }
    }

    public abstract class ActivityData
    {
        public string name;
        public Type type;
        public HashSet<string> inputs;

        public ActivityData(string n, Type t, string[] inp)
        {
            name = n;
            type = t;
            inputs = new HashSet<string>();

            if (inp != null)
            {
                for (int i = 0; i < inp.Length; ++i)
                {
                    inputs.Add(inp[i]);
                }
            }
        }
    }

    public class MindMgr
    {
        private static MindMgr _instance = null;

        private Dictionary<string, ActivityData> _activityDictionary;
        private Dictionary<System.Type, string> _linkedDictionary;
        //private Dictionary<string, Dictionary<Type,ActivityDef>> _activityDictionary;

        public static MindMgr Instance
        {
            get {
                if (_instance == null)
                    _instance = new MindMgr();
                return _instance;
            }
        }

        static IEnumerable<System.Type> GetTypesWithAttribute<T>(Assembly assembly) where T : Attribute
        {
            foreach (System.Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(T), true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        public Type GetActivityType(string activityName)
        {
            if(_activityDictionary.ContainsKey(activityName))
            {
                ActivityData activity = _activityDictionary[activityName];
                return activity.type;
            }
            return null;
        }

        public static Type GetLinkedType(Type t)
        {
            ActivityDescriptionAttribute atributes = t.GetCustomAttribute<ActivityDescriptionAttribute>(false);
            if(atributes != null)
            {
                return atributes.LinkedTo;
            }
            return null;
        }

        public static Type GetLinkedType(Activity activity)
        {
            return GetLinkedType(activity.GetType());
            
        }


        private MindMgr()
        {
            _activityDictionary = new Dictionary<string, ActivityData>();
            _linkedDictionary = new Dictionary<Type, string>();
            foreach (var t in GetTypesWithAttribute<ActivityDescriptionAttribute>(GetType().Assembly))
            {

                object[] objects = t.GetCustomAttributes(typeof(ActivityDescriptionAttribute), false);
                if (objects.Length == 1)
                {
                    ActivityDescriptionAttribute activityName = (ActivityDescriptionAttribute)objects[0];
                    if (!_activityDictionary.ContainsKey(t.Name))
                    {
                        ActivityData activityData = null;
                        string[] inputs = GetInputsInHierarchy(t,activityName);

                        if (activityName is ActionDescriptionAttribute)
                        {
                            string[] outputs = GetOutputsInHierarchy(t, activityName);
                            activityData = new ActionData(t.Name, t, inputs, outputs);
                        }
                        else
                            activityData = new ConditionData(t.Name, t, inputs);

                        _activityDictionary.Add(activityData.name, activityData);
                    }
                    else
                        Debug.LogError("El nombre de la actividad " + t.Name + " está duplicado");

                    //Esta linkado el tipo a algo
                    Type linkedType = GetLinkedType(t);
                    if(linkedType!=null)
                    {
                        _linkedDictionary.Add(linkedType, t.Name);
                    }
                }

            }
        }

        //((ActionDescriptionAttribute)activityName).Output
        public string[] GetInputsInHierarchy(Type t, ActivityDescriptionAttribute activityName)
        {
            List<string> inputList = new List<string>();
            // Sustituir por llamada a todos los de la jerarquía
            object[] objects = t.BaseType.GetCustomAttributes(typeof(ActivityDescriptionAttribute), false);
            if (objects.Length == 1)
            {
                ActivityDescriptionAttribute activityNameBase = (ActivityDescriptionAttribute)objects[0];
                if(activityNameBase.Input != null)
                {
                    for (int i = 0; i < activityNameBase.Input.Length; ++i)
                    {
                        inputList.Add(activityNameBase.Input[i]);
                    }
                }
            }
            if (activityName.Input != null)
            {
                for (int i = 0; i < activityName.Input.Length; ++i)
                {
                    inputList.Add(activityName.Input[i]);
                }
            }

            return inputList.ToArray();
        }

        public string[] GetOutputsInHierarchy(Type t, ActivityDescriptionAttribute activityName)
        {
            List<string> inputList = new List<string>();
            // Sustituir por llamada a todos los de la jerarquía
            object[] objects = t.BaseType.GetCustomAttributes(typeof(ActivityDescriptionAttribute), false);
            if (objects.Length == 1)
            {
                ActivityDescriptionAttribute activityNameBase = (ActivityDescriptionAttribute)objects[0];
                ActionDescriptionAttribute ActionBaseName = (ActionDescriptionAttribute)activityNameBase;
                if(ActionBaseName.Output != null)
                {
                    for (int i = 0; i < ActionBaseName.Output.Length; ++i)
                    {
                        inputList.Add(ActionBaseName.Output[i]);
                    }
                }
            }
            ActionDescriptionAttribute ActionName = (ActionDescriptionAttribute)activityName;
            if(ActionName.Output != null)
            {
                for (int i = 0; i < ActionName.Output.Length; ++i)
                {
                    inputList.Add(ActionName.Output[i]);
                }
            }


            return inputList.ToArray();
        }

        public Action CreateActionByLinked(Type t, GameObject go, Behavior bh)
        {
            if (_linkedDictionary.ContainsKey(t))
            {
                string activityName = _linkedDictionary[t];
                return CreateActionByName(activityName,go, bh);
            }
            else
                Debug.LogError("el componente " + t.GetType().Name + " No está vinculado a ninguna Activity");
            return null;
        }

        public Action CreateActionByLinked(Component component, GameObject go, Behavior bh)
        {
            Type t = component.GetType();
            return CreateActionByLinked(t, go, bh);
        }

        public Action CreateActionByType(System.Type t, GameObject go, Behavior bh)
        {
            return CreateActionByName(t.Name,go, bh);
        }

        public Action CreateAction<T>(GameObject go, Behavior bh)
        {
            return CreateActionByName(typeof(T).Name,go, bh);
        }

        public Action CreateActionByName(string name, GameObject go, Behavior bh)
        {
            if (_activityDictionary.ContainsKey(name))
            {
                ActivityData activityData = _activityDictionary[name];
                if (activityData is ActivityData)
                {
                    Action action = (Action)Activator.CreateInstance(activityData.type);
                    action.Create(activityData,go, bh);
                    return action;
                }
                else
                    Debug.LogError("LA actividad " + name + " no es una acción");
            }
            else
                Debug.LogError("La actividad " + name + " no existe");
            return null;
        }

        public Condition CreateConditionByLinked(Type t, GameObject go, Behavior bh)
        {
            if (_linkedDictionary.ContainsKey(t))
            {
                string activityName = _linkedDictionary[t];
                return CreateConditionByName(activityName,go, bh);
            }
            else
                Debug.LogError("el componente " + t.GetType().Name + " No está vinculado a ninguna Activity");
            return null;
        }

        public Condition CreateConditionByLinked(Component component, GameObject go, Behavior bh)
        {
            return CreateConditionByLinked(component.GetType(),go, bh);
        }

        public Condition CreateConditionByType(System.Type t, GameObject go, Behavior bh)
        {
            return CreateConditionByName(t.Name,go, bh);
        }

        public Condition CreateCondition<T>(GameObject go, Behavior bh)
        {
            return CreateConditionByName(typeof(T).Name,go, bh);
        }

        public Condition CreateConditionByName(string name, GameObject go, Behavior bh)
        {
            if (_activityDictionary.ContainsKey(name))
            {
                ActivityData activityData = _activityDictionary[name];
                if (activityData is ConditionData)
                {
                    Condition condition = (Condition)Activator.CreateInstance(activityData.type);
                    condition.Create(activityData,go, bh);
                    return condition;
                }
                else
                    Debug.LogError("La actividad " + name + " no es una condición");
            }
            else
                Debug.LogError("La actividad " + name + " no existe");
            return null;
        }



        /*public Dictionary<Type, ActivityDef> GetImplentations(string activityName)
        {
            return _activityDictionary[activityName];
        }*/

        public bool ActivityExist(string activityName)
        {
            return _activityDictionary.ContainsKey(activityName);
        }

        public bool ActionExist(string activityName)
        {
            if(_activityDictionary.ContainsKey(activityName))
            {
                return typeof(ActionData).IsAssignableFrom(_activityDictionary[activityName].GetType());
            }
            return false;
        }

        public bool ConditionExist(string activityName)
        {
            if (_activityDictionary.ContainsKey(activityName))
            {
                return typeof(ConditionData).IsAssignableFrom(_activityDictionary[activityName].GetType());
            }
            return false;
        }

        /*public bool ImplementationExist(string activityName, Type t)
        {
            if( _activityDictionary.ContainsKey(activityName))
            {
                Dictionary<Type, ActivityDef> keyValuePairs = GetImplentations(activityName);
                return keyValuePairs.ContainsKey(t);
            }

            return false;
        }

        public ActivityDef GetImplentation(string activityName, Type t)
        {
            Dictionary<Type, ActivityDef> keyValuePairs = GetImplentations(activityName);
            return keyValuePairs[t];
        }*/
    }
}
