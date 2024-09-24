using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;


public class PlayerController : MonoBehaviour
{
    public int Player = 0;
    [field: SerializeField] public bool PlayerCanSteer { get; private set; } = true;
    [SerializeField] float SteeringStrength = 1f;
    [SerializeField] CarControlsHandler CarIfDriver;

    public UnityEvent<float> ItemControl;
    public UnityEvent<Vector2> UpdateLeftJoystick;
    public UnityEvent<Vector2> UpdateRightJoystick;

    public UnityEvent GasPressed;
    public UnityEvent BrakePressed;
    public UnityEvent ItemInteract;
    public UnityEvent ItemGrab;
    public UnityEvent OptionPress;

    Vector2 _rightJoystickValue;
    ItemGrabber _grabber;

    void Awake()
    {
        _grabber = GetComponent<ItemGrabber>();

    }

    public void OnMove(CallbackContext context)
    {
        //Debug.Log($"Tryina move, value: {val} carIfDriver: " + CarIfDriver);
        //i ate a line here. yum!
        Vector2 val = context.ReadValue<Vector2>();
        val.x *= SteeringStrength;
        UpdateLeftJoystick?.Invoke(val);

    }

    public void OnGasPress(CallbackContext context)
    {
        bool clickEntered = context.action.triggered;
        GasPressed?.Invoke();
        if (CarIfDriver == null) return;
        CarIfDriver.UpdateGas(clickEntered ? Vector2.up : Vector2.zero);
    }

    public void OnBrakePress(CallbackContext context)
    {
        bool clickEntered = context.action.triggered;
        BrakePressed?.Invoke();
        if (CarIfDriver == null) return;
        CarIfDriver.UpdateGas(clickEntered ? Vector2.down : Vector2.zero);

    }

    public void OnInteractLeft(CallbackContext context)
    {
        bool clickEntered = context.action.triggered;
        if (clickEntered)
            _grabber.TryInteractWithItem(true, this);
        ItemInteract?.Invoke();
    }

    public void OnInteractRight(CallbackContext context)
    {
        bool clickEntered = context.action.triggered;
        if (clickEntered)
            _grabber.TryInteractWithItem(false, this);
        ItemInteract?.Invoke();
    }

    public void OnGrabRight(CallbackContext context)
    {
        bool interacted = context.action.triggered;
        if (interacted)
            _grabber.TryGrabReleaseItem(false, this);
        ItemGrab?.Invoke();

    }

    public void OnGrabLeft(CallbackContext context)
    {
        bool clickEntered = context.action.triggered;
        if (clickEntered)
            _grabber.TryGrabReleaseItem(true, this);
        ItemGrab?.Invoke();
    }

    public void OnCameraMove(CallbackContext context)
    {
        _rightJoystickValue = context.ReadValue<Vector2>();
    }

    //for keyboards to have continuous input
    void FixedUpdate()
    {
        UpdateRightJoystick?.Invoke(_rightJoystickValue);

    }

    public void OnItemControl(CallbackContext context)
    {
        Vector2 val = context.ReadValue<Vector2>();
        ItemControl?.Invoke(val.y);
    }

    public void OnOptionsPress(CallbackContext context)
    {
        bool interacted = context.action.triggered;
        if (interacted)
            OptionPress?.Invoke();
    }
}
