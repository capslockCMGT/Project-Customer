using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianHitRule : RuleChecker
{
    TriggerDelegator _triggerDelegator;
    HashSet<Collider> _pedestrianHitList = new();
    [SerializeField] SoundName[] _pedestrianHitSound;

    protected override void OnAwake()
    {
        _triggerDelegator = GetComponentInParent<TriggerDelegator>();

        _triggerDelegator.TriggerEnter += TriggerEntered;
        _triggerDelegator.TriggerExit += TriggerExited;
    }

    void OnDisable()
    {
        _triggerDelegator.TriggerEnter -= TriggerEntered;
        _triggerDelegator.TriggerExit -= TriggerExited;
    }

    void TriggerEntered(Collider other)
    {
        if(other.CompareTag("NPC"))
        {
            _pedestrianHitList.Add(other);
            SoundManager.PlayRandomSound(_pedestrianHitSound);
        }

    }

    void TriggerExited(Collider other)
    {
        if(other.CompareTag("NPC"))
        {
            _pedestrianHitList.Remove(other);
        }
    }

    protected override bool Condition()
    {
        return _pedestrianHitList.Count == 0;
    }

    protected override string DeductionName()
    {
        return "Pedestrian Killed";
    }

}
