using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject[] bricks;
    protected BrickContainer parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public BrickContainer Parent
    {
        get
        {
            return parent;
        }

        set
        {
            parent = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break(float force, Transform t, float rad)
    {
        for(int i = 0; i < bricks.Length; i++)
        {
            GameObject go = GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(bricks[i], this.transform.position, Quaternion.identity,null);
            Rigidbody rb = go.GetComponent<Rigidbody>();
            if(rb !=null)
            {
                rb.AddExplosionForce(force, t.position, rad);
            }
        }
        Parent?.ChildDestroy(this);
        Destroy(this.gameObject);
    }
}
