using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.InputSystem.InputAction;

public abstract class PlayerInputHandlerBase : MonoBehaviour
{
    private PlayerInput playerInput;

    public PlayerInput PlayerInput
    {
        get
        {
            return playerInput;
        }
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        _Start();
    }

    protected abstract void _Start();
}
