using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MotionSpeedData
{
    public string id;
    public MotionSpeed motionSpeed;
}



public class MotionSpeedSelectorBase : MonoBehaviour
{
    public int defaultSpeed;
    public MotionSpeedData[] motionSpeedData;
    public MotionAccelerationSpeed customAccel;
    public MotionAccelArriveSpeed customAccelArrive;

    private Dictionary<string, MotionSpeed> _motionSpeedDict;
    // Start is called before the first frame update
    void Awake()
    {
        _motionSpeedDict = new Dictionary<string, MotionSpeed>();
        for (int i = 0; i < motionSpeedData.Length; i++)
        {
            _motionSpeedDict.Add(motionSpeedData[i].id, motionSpeedData[i].motionSpeed);
        }
    }

    public MotionAccelerationSpeed CustomMotionAccelerationSpeed
    {
        get
        {
            return customAccel;
        }
    }
    public MotionAccelArriveSpeed CustomMotionAccelArriveSpeed
    {
        get
        {
            return customAccelArrive;
        }
    }

    public MotionSpeed Default
    {
        get
        {
            return motionSpeedData[defaultSpeed].motionSpeed;
        }
    }

    public string DefaultID
    {
        get
        {
            return motionSpeedData[defaultSpeed].id;
        }
    }

    public List<string> GetAllIDs()
    {
        List<string> keyList = new List<string>(_motionSpeedDict.Keys);
        return keyList;
    }

    public bool ContainID(string index)
    {
        return _motionSpeedDict.ContainsKey(index);
    }

    public MotionSpeed this[string index]
    {
        get
        {
            if (!_motionSpeedDict.ContainsKey(index))
                return null;
            return _motionSpeedDict[index];
        }
    }
}