using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicUpdate : MonoBehaviour
{
    [Tooltip("Game Object gráfico")]
    public GameObject _graphicGo;
    [Tooltip("Suavidad de la rotación")]
    public float _graphicRotationSmooth;
    [Tooltip("Suavidad de la rotación")]
    public Vector3 _rotation;
    public float right;
    public float left;
    public float up;
    public float down;

    private Vector3 _direction;
    
    public Vector3 Direction
    {
        get
        {
            return _direction;
        }

        set
        {
            _direction = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GraphicsUpdate(Time.deltaTime);
    }

    public Vector3 LookDirection()
    {
        return _direction == Vector3.zero ? Vector3.up :  _direction.normalized;
    }

    private void GraphicsUpdate(float time)
    {
        if (_direction.x > 0)
        {
            _graphicGo.transform.rotation = Quaternion.Lerp(_graphicGo.transform.rotation, Quaternion.Euler(_rotation*right), time * _graphicRotationSmooth);
        }
        else if (_direction.x < 0)
        {
            _graphicGo.transform.rotation = Quaternion.Lerp(_graphicGo.transform.rotation, Quaternion.Euler(_rotation*left), time * _graphicRotationSmooth);
            //_graphicGo.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (_direction.y > 0)
        {
            _graphicGo.transform.rotation = Quaternion.Lerp(_graphicGo.transform.rotation, Quaternion.Euler(_rotation * up), time * _graphicRotationSmooth);
        }
        else if (_direction.y < 0)
        {
            _graphicGo.transform.rotation = Quaternion.Lerp(_graphicGo.transform.rotation, Quaternion.Euler(_rotation * down), time * _graphicRotationSmooth);
        }

    }
}
