using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerController controller;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

    }
    private void Start()
    {
        var index = playerInput.playerIndex;

        controller = FindObjectsOfType<PlayerController>().FirstOrDefault(p => p.Player == index);

    }

    public void OnGasPress(CallbackContext context)
    {
        if (controller != null)
            controller.OnGasPress(context);

    }

    public void OnBrakePress(CallbackContext context)
    {
        if (controller != null)
            controller.OnBrakePress(context);
    }

    public void OnMove(CallbackContext context)
    {
        if (controller != null) 
            controller.OnMove(context);
    }
    
    public void OnInteractLeft(CallbackContext context)
    {
        if (controller != null) 
            controller.OnInteractLeft(context);
    }

    public void OnInteractRight(CallbackContext context)
    {
        if (controller != null) 
            controller.OnInteractRight(context);
    }

    public void OnGrabRight(CallbackContext context)
    {
        if (controller != null) 
            controller.OnGrabRight(context);
    }

    public void OnGrabLeft(CallbackContext context)
    {
        if (controller != null) 
        controller.OnGrabLeft(context);
    }

    public void OnCameraMove(CallbackContext context)
    {
        if (controller != null) 
            controller.OnCameraMove(context);
    }

    public void OnItemControl(CallbackContext context)
    {
        if (controller != null)
            controller.OnItemControl(context);
    }
}
