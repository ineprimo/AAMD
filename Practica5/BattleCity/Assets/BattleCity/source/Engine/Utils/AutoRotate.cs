using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3andSpace rotateDegreesPerSecond;
    public bool disableOnStart;
    protected Quaternion initialRotation;

    public void Init()
    {
        initialRotation = this.transform.rotation;
        enabled = true;
    }

    public void Init(float x,float y, float z)
    {
        initialRotation = this.transform.rotation;
        rotateDegreesPerSecond.value = new Vector3(x,y,z);
        enabled = true;
    }

    public void Init(Vector3 v)
    {
        initialRotation = this.transform.rotation;
        rotateDegreesPerSecond.value = v;
        enabled = true;
    }

    public void End()
    {
        this.transform.rotation = initialRotation;
        enabled = false;
    }

    private void Start()
    {
        if (disableOnStart)
            this.enabled = false;
    }

    private void OnDisable()
    {
        if(disableOnStart)
            this.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        float deltaTime = Time.deltaTime;
        transform.Rotate(rotateDegreesPerSecond.value * deltaTime, rotateDegreesPerSecond.space);
    }


    [Serializable]
    public class Vector3andSpace
    {
        public Vector3 value;
        public Space space = Space.Self;
    }
}
