using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class SteeringWheel : MonoBehaviour
{
    [SerializeField] Transform Renderer;
    [SerializeField] CarControlsHandler carHandler;
    private void Start()
    {
        var grab = GetComponent<GrabbableItem>();
        if(grab == null )
        {
            Debug.LogWarning("steering wheel does not have a grabbable and will not function.");
            Destroy(this);
            return;
        }

        if(carHandler == null)
        {
            Debug.LogWarning("steering wheel could not find a carControlsHandler and will not function.");
            Destroy(this);
            return;
        }

        if(Renderer == null)
        {
            Debug.LogWarning("steering wheel doesnt have a renderer set. can you add it pls 🥺");
        }

        grab.onItemGrabbed.AddListener((PlayerController addedController) => {
            if(addedController.PlayerCanSteer)
                addedController.UpdateLeftJoystick.AddListener(carHandler.UpdateSteeringAngle);
        });
        grab.onItemReleased.AddListener((PlayerController addedController) => {
            if(addedController.PlayerCanSteer)
                addedController.UpdateLeftJoystick.RemoveListener(carHandler.UpdateSteeringAngle);
        });

        if(Renderer != null) 
            carHandler.SteeringAngleChanged += (float fuckshit) => { Renderer.localRotation = Quaternion.AngleAxis(fuckshit * 30, Vector3.up); };
    }
}
