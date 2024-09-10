using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableItem : MonoBehaviour
{
    public UnityEvent onItemGrabbed;
    public UnityEvent onPlayerInteract;
    public UnityEvent onItemReleased;
    private void Start()
    {
        if(GetComponent<Rigidbody>() == null)
        {
            Debug.LogWarning("Grabbable item does not have an attached rigidbody and will not work!");
            Destroy(this);
        }
    }
    public void Grab() => onItemGrabbed?.Invoke();
    public void PlayerInteract() => onPlayerInteract?.Invoke();
    public void Release() => onItemReleased?.Invoke();
}
