using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour {

    public GameObject _plane;
    public bool _showGrid;
    public Vector2 _gridSize;
    public Vector3 _startPoint;
    public float _step;
    public Material _lineMaterial;


    
    private Color _mainColor = new Color(0f,1f,0f,1f);

    // Use this for initialization
    /*void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}*/

    public void OnPostRender()
    {
        //Example: http://answers.unity3d.com/questions/482128/draw-grid-lines-in-game-view.html
        _lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        if(_showGrid)
        {
            //GL.Color(_mainColor);
            for (float j = 0f; j <= _gridSize.y; j += _step)
            {
                // X
                GL.Vertex3(_startPoint.x, _startPoint.y+j, _startPoint.z);
                GL.Vertex3(_startPoint.x + _gridSize.y, _startPoint.x+j, _startPoint.z);
            }


            for(float i = 0; i <= _gridSize.x; i += _step)
            {
                GL.Vertex3(_startPoint.x + i, _startPoint.y, _startPoint.z);
                GL.Vertex3(_startPoint.x + i, _startPoint.y+_gridSize.y, _startPoint.z);
            }


        }
        GL.End();
    }
}
