using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionEffects : MonoBehaviour
{
    [SerializeField] GameObject Effect;
    [SerializeField] float Strength;
    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            var col = collision.GetContact(i);
            var obj = Instantiate(Effect, col.point, Quaternion.LookRotation(col.impulse));
            
            var parts = obj.GetComponent<ParticleSystem>();
            if (parts != null)
            {
                var main = parts.main;
                main.startSize = col.impulse.magnitude*.0001f*Strength;
            }
        }
    }
}
