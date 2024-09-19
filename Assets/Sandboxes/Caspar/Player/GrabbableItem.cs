using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class GrabbableItem : MonoBehaviour
{
    public Transform Renderer;
    public UnityEvent<PlayerController> onItemGrabbed;
    public UnityEvent<PlayerController> onItemReleased;
    public UnityEvent<PlayerController> onPlayerInteract;

    List<PlayerController> _holdingPlayers = new();
    Outline _outline;

    void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    void Start()
    {
        if (Renderer == null)
            Renderer = this.transform;
    }

    public void Grab(PlayerController controller)
    {
        Debug.Log($"i ({transform.name}) just got grabbed (:");
        onItemGrabbed?.Invoke(controller);
        _holdingPlayers.Add(controller);

        _outline.enabled = true;
    }

    public void Release(PlayerController controller)
    {
        onItemReleased?.Invoke(controller);
        _holdingPlayers.Remove(controller);
        _outline.enabled = false;
    }
}
