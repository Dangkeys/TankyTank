using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;
[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> MoveEvent;
    private PlayerControls playerControls;
    private void OnEnable() {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.SetCallbacks(this);
        }
        playerControls.Player.Enable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if(context.performed)
            PrimaryFireEvent?.Invoke(true);
        else if(context.canceled)
            PrimaryFireEvent?.Invoke(false);

    }
}
