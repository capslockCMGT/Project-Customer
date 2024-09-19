using System;
using UnityEngine;

[RequireComponent (typeof(ItemGrabber))] 
public class NoHandsChecker : RuleChecker
{
    ItemGrabber _hands;
    //CarController _carController;

    protected override void OnAwake()
    {
        _hands = GetComponent<ItemGrabber>();
    }

    protected override bool Condition()
    {
        return /*_carController.CarVelocity.magnitude > .1f */ _hands.GetRightHandItem() != null || _hands.GetLeftHandItem() != null;
    }

    protected override string DeductionName()
    {
        return "No hands on wheel";
    }
}
