using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDebugRender : MonoBehaviour {

    public Material[] _lineDebugMaterial;
    private List<Line> _lineList;

    void Start()
    {
        _lineList = new List<Line>();
    }

    public void OnPostRender()
    {
        //Example: http://answers.unity3d.com/questions/482128/draw-grid-lines-in-game-view.html
            //_lineDebugMaterial.SetPass(0);
            //GL.Begin(GL.LINES);

            //GL.Color(_mainColor);
            for (int i = 0; i < _lineList.Count; ++i)
            {
                Line line = _lineList[i];
                _lineDebugMaterial[line.MatId].SetPass(0);
                GL.Begin(GL.LINES);
                
                GL.Vertex3(line.From.x, line.From.y, line.From.z);
                GL.Vertex3(line.To.x, line.To.y, line.To.z);
                GL.End();
            }
            _lineList.Clear();

    }

    public void AddLine(Line l)
    {
        _lineList.Add(l);
    }
}
