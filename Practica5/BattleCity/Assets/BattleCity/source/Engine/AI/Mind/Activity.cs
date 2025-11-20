using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mind
{
    public abstract class Activity
    {
        private ActivityData _activityData;
        private GameObject _gameObject;
        private Component _linkedComponent;
        private Behavior _behavior;


        public Behavior ParentBehavior
        {
            get { return _behavior; }
        }

        public ActivityData ActivityData
        {
            get { return _activityData; }
        }

        public void Create(ActivityData activityData, GameObject go, Behavior bh)
        {
            _activityData = activityData;
            _gameObject = go;
            _behavior = bh;
        }

        public virtual void SetComponent(Component component)
        {
            _linkedComponent = component;
        }

        public T GetLinkedComponent<T>() where T : Component
        {
            return (T)_linkedComponent;
        }

        public Component LinkedComponent
        {
            get { return _linkedComponent; }
        }

        public T Input<T>(string name) 
        {
            Debug.Assert(_activityData.inputs.Contains(name), "El campo "+name+" no es input de de la Activity "+GetType().Name);
            return (T)_behavior.Blackboard[name];
        }

        public bool InputExistInHierarchy(string name)
        {
            Debug.Assert(_activityData.inputs.Contains(name), "El campo " + name + " no es input de de la Activity " + GetType().Name);
            return _behavior.Blackboard.ExistInHierarchy(name);
        }

        public GameObject GameObject
        {
            get { return _gameObject; }
        }

        public string Name
        {
            get { return _activityData.name; }
        }

        public abstract void OnAwake();
        public abstract void OnStart();

        protected T GetPerception<T>() where T : Perception
        {
            return (T)_behavior.Perception;
        }
    }
}
