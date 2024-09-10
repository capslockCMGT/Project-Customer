using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringManager : MonoBehaviour
{
    public event Action<float> SteeringAngleChanged;

    private void Update()
    {
        float steeringAngle = Input.GetAxis("Horizontal");
        SteeringAngleChanged(steeringAngle);
    }
}
