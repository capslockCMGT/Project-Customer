using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour
{
    public Transform Sender { get; set; }
    public NavMeshAgent Agent { get; private set; }
    //public Vector3 gixmo { get; set; }

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();    
    }

    public void Die()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = new();

        var col = GetComponent<Collider>();
        col.isTrigger = false;
        Destroy(this);
        Destroy(Agent);
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(gixmo, 2);
    //}
}
