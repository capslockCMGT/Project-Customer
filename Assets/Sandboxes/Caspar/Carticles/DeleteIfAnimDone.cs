using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteIfAnimDone : MonoBehaviour
{
    ParticleSystem _particles;
    
    void Start()
    {
        _particles = GetComponent<ParticleSystem>();

        if (_particles == null)
            Destroy(this);
    }

    void FixedUpdate()
    {
        if(_particles != null && !_particles.IsAlive())
            Destroy(gameObject);
    }
}
