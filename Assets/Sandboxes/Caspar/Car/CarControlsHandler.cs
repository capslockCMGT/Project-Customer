using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarControlsHandler : MonoBehaviour
{
    public event Action<float> SteeringAngleChanged;
    public event Action<float> CarSpeedChanged;
    public event Action GearshiftReversed;
    [SerializeField] SoundName engineSound;

    //int _steersReceived = 0;
    float _steerInput = 0;
    float _gasInput;


    private void Start()
    {
        SoundManager.Instance.PlaySound(engineSound);
    }
    public void ToggleCarReverse(PlayerController controller)
    {
        GearshiftReversed.Invoke();
    }

    public void UpdateGas(Vector2 playerInput)
    {
        _gasInput = playerInput.y;

    }

    public void UpdateSteeringAngle(Vector2 playerInput)
    {
        //tally up the steers received every frame, so multiple players can hold it
        //_steersReceived++;
        _steerInput = playerInput.x;
    }

    private void Update()
    {
        //if(_steersReceived != 0)
        SteeringAngleChanged?.Invoke(_steerInput );
        CarSpeedChanged?.Invoke(_gasInput);

        //_steersReceived = 0;
    }
}
