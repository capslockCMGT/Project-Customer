using System;
using UnityEngine;

[RequireComponent (typeof(ItemGrabber))] 
public class NoHandsChecker : RuleChecker
{
    ItemGrabber _hands;

    protected override void OnAwake()
    {
        _hands = GetComponent<ItemGrabber>();
    }

    protected override bool Condition()
    {
        return _hands.GetRightHandItem() != null || _hands.GetLeftHandItem() != null;
    }
}
