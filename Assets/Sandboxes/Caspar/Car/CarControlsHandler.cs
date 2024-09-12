using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarControlsHandler : MonoBehaviour
{
    public event Action<float> SteeringAngleChanged;
    public event Action<float> CarSpeedChanged;

    int _steersReceived = 0;
    float _steerInput = 0;

    public void UpdateGas(Vector2 playerInput)
    {
        CarSpeedChanged?.Invoke(playerInput.y);
    }

    public void UpdateSteeringAngle(Vector2 playerInput)
    {
        //tally up the steers received every frame, so multiple players can hold it
        _steersReceived++;
        _steerInput += playerInput.x;
    }

    private void Update()
    {
        if(_steersReceived != 0)
            SteeringAngleChanged?.Invoke(_steerInput / _steersReceived);
        _steerInput = 0;
        _steersReceived = 0;
    }
}
