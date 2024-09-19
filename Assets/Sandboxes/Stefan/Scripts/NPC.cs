using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour
{
    public Transform Sender { get; set; }
    public NavMeshAgent Agent { get; private set; }
    public Vector3 gixmo { get; set; }

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();    
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(gixmo, 2);
    }
}
