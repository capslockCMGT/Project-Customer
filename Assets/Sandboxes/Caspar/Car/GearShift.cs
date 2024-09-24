using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(GrabbableItem))]
public class GearShift : MonoBehaviour
{
    [SerializeField] Transform Renderer;
    [SerializeField] CarControlsHandler carHandler;
    [SerializeField] float PassengerSetCooldown = 10f;

    private void Start()
    {
        var grab = GetComponent<GrabbableItem>();

        if (carHandler == null)
        {
            Debug.LogWarning($"gearshift {transform.name} could not find a carControlsHandler and will not function.");
            Destroy(this);
            return;
        }

        if (Renderer == null)
            Debug.LogWarning($"gearshift {transform.name} doesnt have a renderer set. can you add it pls");

        grab.onPlayerInteract.AddListener((PlayerController controller) => {
            if (controller.SetGearshiftCooldown > 0)
                return;

            //waahhhh hardcoding
            if(controller.Player == 1)
                controller.SetGearshiftCooldown = PassengerSetCooldown;

            carHandler.ToggleCarReverse(controller);
            });
    }
}
