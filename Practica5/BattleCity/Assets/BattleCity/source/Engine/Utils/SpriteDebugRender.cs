using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDebugRender : MonoBehaviour {

    public SpriteRenderer[] _sprites;

    private bool _showDebug;
	// Use this for initialization


    public bool DebugEnable
    {
        set
        {
            _showDebug = value;

            for (int i = 0; i < _sprites.Length; ++i)
            {
                _sprites[i].enabled = _showDebug;
            }
        }

        get
        {
            return _showDebug;
        }
    }
}
