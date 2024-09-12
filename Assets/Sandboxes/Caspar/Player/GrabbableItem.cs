using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableItem : MonoBehaviour
{
    public UnityEvent<PlayerController> onItemGrabbed;
    public UnityEvent<PlayerController> onItemReleased;

    List<PlayerController> _holdingPlayers = new();
    public void Grab(PlayerController controller)
    {
        Debug.Log($"i ({transform.name}) just got grabbed (:");
        onItemGrabbed?.Invoke(controller);
        _holdingPlayers.Add(controller);
    }
    public void Release(PlayerController controller)
    {
        onItemReleased?.Invoke(controller);
        _holdingPlayers.Remove(controller);
    }
}
