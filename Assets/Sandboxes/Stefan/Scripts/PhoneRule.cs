using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneRule : RuleChecker
{
    [SerializeField] Phone _phone;
    enum GrabState
    {
        False,
        ByPassenger,
        ByDriver
    }
    GrabState _state;

    private void OnEnable()
    {
        _phone.Grabbed += OnPhoneGrabbed;
    }

    private void OnPhoneGrabbed(PlayerController obj)
    {
        if(obj.Player == 0)
        {
            _state = GrabState.ByDriver;
        }
        else
        {
            _state = GrabState.ByPassenger;
        }
    }

    private void OnDisable()
    {
        _phone.Grabbed -= OnPhoneGrabbed;

    }
    protected override bool Condition()
    {
        bool cond = _state != GrabState.ByPassenger;
        _state = GrabState.False;
        return cond;
    }

    protected override string DeductionName()
    {
        return "Excessive phone noise";
    }

    
    void Update()
    {
        
    }
}
