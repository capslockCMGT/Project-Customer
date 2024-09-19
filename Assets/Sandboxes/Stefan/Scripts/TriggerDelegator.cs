using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDelegator : MonoBehaviour
{
    public event Action<Collider> TriggerEnter;
    public event Action<Collider> TriggerExit;

    void OnTriggerEnter(Collider other)
    {
        TriggerEnter?.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke(other);
    }

}
