using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputControllerBase : MonoBehaviour
{
    public enum INPUT_TYPE { KEYBOARD = 0, GAMEPAD = 1 }

    private Dictionary<INPUT_TYPE, PlayerInputHandlerBase> _playerInputHandler;
    public Gamepad GetGamePad()
    {
        if (GotThisInputHandler(INPUT_TYPE.GAMEPAD))
        {
            PlayerInputHandlerBase inputHandler = GetInputHandler(INPUT_TYPE.GAMEPAD);
            Gamepad gamePad = inputHandler.PlayerInput.GetDevice<Gamepad>();
            return gamePad;
        }
        else
            return null;
    }

    public bool GotThisInputHandler(INPUT_TYPE type)
    {
        return _playerInputHandler.ContainsKey(type);
    }

    public PlayerInputHandlerBase GetInputHandler(INPUT_TYPE type)
    {
        return _playerInputHandler[type];
    }
}
