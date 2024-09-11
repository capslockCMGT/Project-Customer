using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public int Player = 0;
    
    public UnityEvent<Vector2> UpdateLeftJoystick;
    public UnityEvent<Vector2> UpdateRightJoystick;

    public UnityEvent LeftTriggerPressed;
    public UnityEvent RightTriggerPressed;
    public UnityEvent LeftTriggerReleased;
    public UnityEvent RightTriggerReleased;

    private void Start()
    {
        if (Player >= Gamepad.all.Count || Gamepad.all[Player] == null)
            Debug.LogWarning($"not epicore! Player {Player} totes doesnt exist yo!");
    }
    private void Update()
    {
        //return if the player is invalid
        if (Player >= Gamepad.all.Count || Gamepad.all[Player] == null) return;

        var pad = Gamepad.all[Player];
        UpdateLeftJoystick?.Invoke(pad.leftStick.ReadValue());
        UpdateRightJoystick?.Invoke(pad.rightStick.ReadValue());

        if (pad.leftTrigger.wasPressedThisFrame) LeftTriggerPressed?.Invoke();
        if (pad.rightTrigger.wasPressedThisFrame) RightTriggerPressed?.Invoke();
        if (pad.leftTrigger.wasReleasedThisFrame) LeftTriggerReleased?.Invoke();
        if (pad.rightTrigger.wasReleasedThisFrame) RightTriggerReleased?.Invoke();
    }
}
