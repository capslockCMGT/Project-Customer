using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
    GrabbableItem grab;
    private void Start()
    {
        grab = GetComponent<GrabbableItem>();
        if(grab == null )
        {
            Debug.LogWarning("steering wheel does not have a grabbable and will not function.");
            Destroy(this);
            return;
        }

    }
}
