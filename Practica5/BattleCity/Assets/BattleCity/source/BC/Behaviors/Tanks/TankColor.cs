using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankColor : MonoBehaviour
{
    public Color colorTint;
    public MeshRenderer[] meshRenderer;
    public int[] material;
    private GameLogicListener gameLogicListener;
    // Start is called before the first frame update
    void Start()
    {
        gameLogicListener = GetComponent<GameLogicListener>();
        if(gameLogicListener.entityType == GameLogicListener.EntityType.AGENT)
        {
            GameLogic gameLogic = GameLogic.Get();
            if(gameLogic.IsMultyAgent)
            {
                colorTint=gameLogic.GetColor();
            }
        }


        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].materials[material[i]].color = colorTint;
        }


    }
}
