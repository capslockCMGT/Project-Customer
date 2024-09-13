using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableItem : MonoBehaviour
{
    public Transform Renderer;
    public UnityEvent<PlayerController> onItemGrabbed;
    public UnityEvent<PlayerController> onItemReleased;
    public UnityEvent onPlayerInteract;

    private void Start()
    {
        if (Renderer == null)
            Renderer = this.transform;
    }

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
