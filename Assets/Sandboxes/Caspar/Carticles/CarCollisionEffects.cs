using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionEffects : MonoBehaviour
{
    [SerializeField] GameObject Effect;
    [SerializeField] float Strength;
    [SerializeField] SoundName[] DefaultCollisionSound;
    [SerializeField] SoundName[] CollideWithPedestrian;

    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            var col = collision.GetContact(i);
            if (col.impulse == Vector3.zero) continue;
            var obj = Instantiate(Effect, col.point, Quaternion.LookRotation(col.impulse));
            float collisionStrength = col.impulse.magnitude * .001f * Strength;

            if (collision.transform.CompareTag("NPC"))
                SoundManager.PlayRandomSound(CollideWithPedestrian, collisionStrength);
            else SoundManager.PlayRandomSound(DefaultCollisionSound, collisionStrength);
            
            var parts = obj.GetComponent<ParticleSystem>();
            if (parts != null)
            {
                var main = parts.main;
                main.startSize = .1f * collisionStrength;
            }
        }
    }
}
