using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public int Player = 0;
    [field:SerializeField] public bool PlayerCanSteer { get; private set; } = true;
    [SerializeField] CarControlsHandler CarIfDriver;
    
    public UnityEvent<Vector2> UpdateLeftJoystick;
    public UnityEvent<Vector2> UpdateRightJoystick;

    public UnityEvent LeftTriggerPressed;
    public UnityEvent RightTriggerPressed;
    public UnityEvent LeftTriggerReleased;
    public UnityEvent RightTriggerReleased;

    private void Start()
    {
        if (Player >= Gamepad.all.Count || Gamepad.all[Player] == null)
            Debug.LogWarning($"not epicore! player {Player} totes doesnt exist yo!");

        var grabber = GetComponent<ItemGrabber>();
        if (grabber == null) Debug.LogWarning("controller couldnt find an itemgrabber. thats fair. i am hardcoding this though, so it wont work. FUCK you.");
        else
        {
            //automatically add the playercontroller to grab on input.
            LeftTriggerPressed.AddListener( () => { grabber.TryGrabReleaseItem(true, this); });
            RightTriggerPressed.AddListener( () => { grabber.TryGrabReleaseItem(false, this); });
        }

        if(CarIfDriver != null)
        {
            UpdateLeftJoystick.AddListener(CarIfDriver.UpdateGas);
        }
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
