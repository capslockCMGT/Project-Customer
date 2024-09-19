using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SafetyCreditsManager))]
public abstract class RuleChecker : MonoBehaviour
{
    [SerializeField] float _penaltyThreshhold = 3f;
    [SerializeField] float _penaltyCooldown = 3f;
    [SerializeField] int _deductionAmount = 5;
    public event Action RuleBroken;

    SafetyCreditsManager _creditsManager;
    float _penaltyAccumulation;
    float _cooldownTimer;

    protected void Awake()
    {
        _creditsManager = GetComponent<SafetyCreditsManager>();
        OnAwake();
    }
    
    protected virtual void OnAwake()
    {

    }

    protected void FixedUpdate()
    {
        _cooldownTimer += Time.deltaTime;
        //when you don't look at the road for a variable time, you get penalized once and then an immunity timer starts
        if (Condition())
        {
            _penaltyAccumulation = 0;
            return;
        }
        if (_cooldownTimer <= _penaltyCooldown) return;

        if(GetType() == typeof(PedestrianHitRule)) 
        {
        
        }

        _penaltyAccumulation += Time.deltaTime;

        if (_penaltyAccumulation >= _penaltyThreshhold)
        {
            _cooldownTimer = 0;
            _penaltyAccumulation = 0;
            _creditsManager.DeductCredits(_deductionAmount, DeductionName());
            RuleBroken?.Invoke();
        }
    }

    protected abstract bool Condition();
    protected abstract string DeductionName();
}
