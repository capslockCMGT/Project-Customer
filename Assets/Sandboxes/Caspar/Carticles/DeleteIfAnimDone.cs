using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteIfAnimDone : MonoBehaviour
{
    ParticleSystem _particles;
    AudioSource _audioSource;
    void Start()
    {
        _particles = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (_particles != null && _particles.IsAlive())
            return;
        if (_audioSource != null && _audioSource.isPlaying)
            return;
        
        Destroy(gameObject);
    }
}
