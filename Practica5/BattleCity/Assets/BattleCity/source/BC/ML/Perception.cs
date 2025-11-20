using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionInfo
{
    public bool detected;
    public RaycastHit hit;
    public PerceptionInfo()
    {
        detected = false;
        hit = default;
    }
}
public class Perception : MonoBehaviour
{
    public float[] rays;
    public float[] distance;
    public int layer;
    public float heightOffset;

    private PerceptionInfo[] perceptionInfo;
    // Start is called before the first frame update
    void Start()
    {
        perceptionInfo = new PerceptionInfo[rays.Length];
        for (int i = 0; i < perceptionInfo.Length; i++)
            perceptionInfo[i] = new PerceptionInfo();
    }

    Vector3 RotateTowardsUp(Vector3 start, float angle)
    {
        // if you know start will always be normalized, can skip this step
        start.Normalize();

        Vector3 axis = Vector3.Cross(start, Vector3.right);

        // handle case where start is colinear with up
        if (axis == Vector3.zero) axis = Vector3.right;

        return Quaternion.AngleAxis(angle, axis) * start;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rays.Length; i++)
        {
            RaycastHit hit;
            Vector3 direction = RotateTowardsUp(transform.forward, rays[i]);
            Ray ray = new Ray(this.transform.position + Vector3.up* heightOffset, direction);
            bool collision = Physics.Raycast(ray, out hit, distance[i], 1 << layer);
            perceptionInfo[i].detected= collision;
            if (collision)
            {
                Debug.DrawRay(ray.origin,ray.direction* distance[i], Color.red);
                perceptionInfo[i].hit = hit;
            }
            else
                Debug.DrawRay(ray.origin, ray.direction* distance[i], Color.yellow);
        }
    }

    public PerceptionInfo[] Perceptions
    {
        get
        {
            return perceptionInfo;
        }
    }

}
