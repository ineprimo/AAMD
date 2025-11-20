using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualMap : MonoBehaviour
{
    private Image[] images;
    public Sprite spriteBrick;
    public Sprite spriteCommandCenter;
    public Sprite spriteSemiBrekable;
    public Sprite spriteSemiUnbrekable;
    public Sprite spriteUnbrekable;

    private bool waitForStart;
    private bool startCompleted=false;
    private Map _map;
    // Start is called before the first frame update
    void Start()
    {
        images = new Image[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            images[i] = this.transform.GetChild(i).GetComponent<Image>();
        }
        startCompleted = true;
    }

    // Update is called once per frame
    public void UpdateGrid(Map map)
    {
        if(images == null || images.Length == 0)
        {
            waitForStart = true;
            _map = map;
        }
        else
        {
            _UpdateGrid(map);
        }
    }

    protected void _UpdateGrid(Map map)
    {
        for (int i = 0; i < this.images.Length; i++)
        {
            if (map[i] == TankPerception.INPUT_TYPE.NOTHING)
            {
                images[i].color = Color.white;
                images[i].sprite = null;
            }
            else if (map[i] == TankPerception.INPUT_TYPE.BRICK)
                images[i].sprite = spriteBrick;
            else if (map[i] == TankPerception.INPUT_TYPE.COMMAND_CENTER)
                images[i].sprite = spriteCommandCenter;
            else if (map[i] == TankPerception.INPUT_TYPE.SEMI_BREKABLE)
                images[i].sprite = spriteSemiBrekable;
            else if (map[i] == TankPerception.INPUT_TYPE.SEMI_UNBREKABLE)
                images[i].sprite = spriteSemiUnbrekable;
            else if (map[i] == TankPerception.INPUT_TYPE.UNBREAKABLE)
                images[i].sprite = spriteUnbrekable;
        }
    }

    private void Update()
    {
        if(waitForStart && startCompleted)
        {
            _UpdateGrid(_map);
        }
    }
}
