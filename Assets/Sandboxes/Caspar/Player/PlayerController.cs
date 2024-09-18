using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;


public class PlayerController : MonoBehaviour
{
    public int Player = 0;
    [field:SerializeField] public bool PlayerCanSteer { get; private set; } = true;
    [SerializeField] CarControlsHandler CarIfDriver;
    
    public UnityEvent<Vector2> UpdateLeftJoystick;
    public UnityEvent<Vector2> UpdateRightJoystick;


    ItemGrabber _grabber;

    void Awake()
    {
        _grabber = GetComponent<ItemGrabber>();

    }

    public void OnMove(CallbackContext context)
    {
        //Debug.Log($"Tryina move, value: {val} carIfDriver: " + CarIfDriver);
        if (CarIfDriver == null) return;
        Vector2 val = context.ReadValue<Vector2>();

        CarIfDriver.UpdateGas(val);
        UpdateLeftJoystick?.Invoke(val);
    }

    public void OnInteractLeft(CallbackContext context)
    {
        bool clickEntered = context.action.triggered;
        if(clickEntered)
            _grabber.TryInteractWithItem(true,this);
    }

    public void OnInteractRight(CallbackContext context)
    {
        bool clickEntered = context.action.triggered;
        if(clickEntered)
            _grabber.TryInteractWithItem(false,this);
    }

    public void OnGrabRight(CallbackContext context)
    {
        bool interacted = context.action.triggered;
        if(interacted)
            _grabber.TryGrabReleaseItem(false, this);

    }

    public void OnGrabLeft(CallbackContext context)
    {
        bool clickEntered = context.action.triggered;
        if (clickEntered)
            _grabber.TryGrabReleaseItem(true, this);

    }

    public void OnCameraMove(CallbackContext context)
    {
        Vector2 val = context.ReadValue<Vector2>();
        UpdateRightJoystick?.Invoke(val);
    }
}
