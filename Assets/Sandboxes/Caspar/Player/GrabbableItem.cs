using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableItem : MonoBehaviour
{
    public UnityEvent<PlayerController> onItemGrabbed;
    public UnityEvent<PlayerController> onItemReleased;

    List<PlayerController> _holdingPlayers = new();
    private void Start()
    {
        if(GetComponent<Rigidbody>() == null)
        {
            Debug.LogWarning("Grabbable item does not have an attached rigidbody and will not work!");
            Destroy(this);
        }
    }
    public void Grab(PlayerController controller)
    {
        onItemGrabbed?.Invoke(controller);
        _holdingPlayers.Add(controller);
    }
    public void Release(PlayerController controller)
    {
        onItemReleased?.Invoke(controller);
        _holdingPlayers.Remove(controller);
    }
}
