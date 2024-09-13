using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class GearShift : MonoBehaviour
{
    [SerializeField] Transform Renderer;
    [SerializeField] CarControlsHandler carHandler;
    private void Start()
    {
        var grab = GetComponent<GrabbableItem>();
        if (grab == null)
        {
            Debug.LogWarning($"gearshift {transform.name} does not have a grabbable and will not function.");
            Destroy(this);
            return;
        }

        if (carHandler == null)
        {
            Debug.LogWarning($"gearshift {transform.name} could not find a carControlsHandler and will not function.");
            Destroy(this);
            return;
        }

        if (Renderer == null)
            Debug.LogWarning($"gearshift {transform.name} doesnt have a renderer set. can you add it pls");

        grab.onPlayerInteract.AddListener(carHandler.ToggleCarReverse);
    }
}
