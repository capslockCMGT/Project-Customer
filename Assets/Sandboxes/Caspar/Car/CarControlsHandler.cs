using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlsHandler : MonoBehaviour
{
    public event Action<float> SteeringAngleChanged;
    public event Action<float> CarSpeedChanged;

    //temporary controls - both actions are expected to be between -1, 1 at all times
    public void UpdateGas(Vector2 playerInput)
    {
        CarSpeedChanged(playerInput.x);
    }

    public void UpdateSteeringAngle(Vector2 playerInput)
    {
        SteeringAngleChanged(playerInput.y);
    }
}
