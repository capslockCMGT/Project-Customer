using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianHitRule : RuleChecker
{
    TriggerDelegator _triggerDelegator;
    bool _pedestrianHit;

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
            _pedestrianHit = true;
        }

    }

    void TriggerExited(Collider other)
    {
        if(other.CompareTag("NPC"))
            _pedestrianHit = false;
    }

    protected override bool Condition()
    {
        return !_pedestrianHit;
    }

    protected override string DeductionName()
    {
        return "Pedestrian Killed";
    }

}
