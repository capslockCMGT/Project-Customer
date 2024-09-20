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

    [field: SerializeField] public bool ChangeToDynamicWhileGrabbing {  get; private set; }

    void Start()
    {
        if (Renderer == null)
            Renderer = this.transform;

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
