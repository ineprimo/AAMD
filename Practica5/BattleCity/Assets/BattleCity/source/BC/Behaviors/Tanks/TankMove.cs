using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour
{
    public float speed;
    public Rigidbody _rigidbody;
    public GraphicUpdate graphicUpdate;
    public BoxCollider boxCollider;
    public string tankIdleSound;
    public string tanMoveSound;

    private Vector3 _moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        _moveDirection = Vector3.zero;
        graphicUpdate.Direction = _moveDirection;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //_characterController.Move(_moveDirection * speed * Time.deltaTime);
        Vector3 newPosition = this.transform.position + _moveDirection * speed * Time.fixedDeltaTime;
        Collider[] colliders = Physics.OverlapBox(newPosition + boxCollider.center, boxCollider.size*0.5f,Quaternion.identity,1 << Globals.GROUND_LAYER);
        if(colliders == null || colliders.Length == 0)
        {
            _rigidbody.MovePosition(newPosition);
        }
        
    }

    public void Move(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > 0.1f && Mathf.Abs(dir.y) > 0.1f)
            dir = Vector2.zero;
        _moveDirection = new Vector3(dir.x, dir.y, 0f);
        _moveDirection = _moveDirection.normalized;
        if(dir != Vector2.zero)
        {
            graphicUpdate.Direction = _moveDirection;
            GameMgr.Instance.GetServer<SoundMgr>().PlaySound(tanMoveSound);
        }
        else
            GameMgr.Instance.GetServer<SoundMgr>().PlaySound(tankIdleSound);


    }
}
