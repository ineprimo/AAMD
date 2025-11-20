using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class TankInput : PlayerInputHandlerBase
{
    public PlayerInputController playerInputController;
    public Record record;
    // Start is called before the first frame update
    protected override void _Start()
    {

    }

    public void Move(CallbackContext ctx)
    {
        if (ctx.performed)
            playerInputController.OnMove(ctx.ReadValue<Vector2>());
        else if (ctx.started)
            playerInputController.OnMove(ctx.ReadValue<Vector2>());
        else if (ctx.canceled)
            playerInputController.OnMove(Vector2.zero);
    }

    public void Fire(CallbackContext ctx)
    {
        if (ctx.performed)
            playerInputController.Fire(true);
        else if (ctx.canceled)
            playerInputController.Fire(false);
    }

    public void Reset(CallbackContext ctx)
    {
        if (ctx.started)
        {
            GameLogic.Get().SetGameOver();
            if(GameLogic.Get().gameMode == GameLogic.GAME_MODE.IA_DRIVEN)
                record.SaveInGameData("AI_testing");
        }
            
    }

    public void Sound(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SoundMgr sound = GameMgr.Instance.SoundMgr;
            if (sound.IsMute)
                sound.Unmute();
            else
                sound.Mute();
        }
    }

    public void TankAgentDamageHack(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            List<GameObject> agents = GameLogic.Get().Agents;
            if(agents != null && agents.Count > 0)
            {
                agents[0].GetComponent<Health>().TakeDamage(1, this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
