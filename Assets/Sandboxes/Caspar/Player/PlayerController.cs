using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


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

    public UnityEvent LeftBumperPressed;
    public UnityEvent RightBumperPressed;
    public UnityEvent LeftBumperReleased;
    public UnityEvent RightBumperReleased;

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
            LeftBumperPressed.AddListener( () => { grabber.TryInteractWithItem(true); });
            RightBumperPressed.AddListener( () => { grabber.TryInteractWithItem(false); });
        }

        if(CarIfDriver != null)
        {
            UpdateLeftJoystick.AddListener(CarIfDriver.UpdateGas);
        }
    }

    public void OnMove(CallbackContext context)
    {
        //Vector2 val = context.ReadValue<Vector2>();

    }

    public void OnInteractLeft(CallbackContext context)
    {
        //bool interacted = context.action.triggered;

    }

    public void OnInteractRight(CallbackContext context)
    {
        //bool interacted = context.action.triggered;

    }

    public void OnGrabRight(CallbackContext context)
    {
        //bool interacted = context.action.triggered;

    }

    public void OnGrabLeft(CallbackContext context)
    {
        //bool interacted = context.action.triggered;

    }

    public void OnCameraMove(CallbackContext context)
    {
        //Vector2 val = context.ReadValue<Vector2>();

    }
}
