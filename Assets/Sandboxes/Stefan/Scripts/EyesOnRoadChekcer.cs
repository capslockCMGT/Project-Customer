using System;
using UnityEngine;

public class EyesOnRoadChekcer : RuleChecker
{
    [SerializeField] protected LayerMask _frontScreenMask;

    protected override bool Condition()
    {

        return Physics.Raycast(transform.position, transform.forward, 10, _frontScreenMask);
    }

    protected override string DeductionName()
    {
        return "Eyes not on road";
    }
}
