using System;
using UnityEngine;

public class TileConnections : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<TileConnections>(out var otherConection))
            return;

        
    }



}
