using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlsHandler : MonoBehaviour
{
    public event Action<float> SteeringAngleChanged;
    public event Action<float> CarSpeedChanged;

    //temporary controls - both actions are expected to be between -1, 1 at all times
    private void Update()
    {
        float steeringAngle = Input.GetAxis("Horizontal");
        SteeringAngleChanged(steeringAngle);

        float gas = Input.GetAxis("Vertical");
        CarSpeedChanged(gas);
    }
}
