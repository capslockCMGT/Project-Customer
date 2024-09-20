using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GrabbableItem))]
public class PopUpAd : MonoBehaviour
{
    GrabbableItem item;
    private void Awake()
    {
        item = GetComponent<GrabbableItem>();
        item.onItemGrabbed.AddListener(controller => {
            Transform personHands = transform.GetChild(0);
            personHands.position = controller.transform.position;
            personHands.parent = controller.transform;

            Destroy(gameObject);
        });
    }


}
