using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarControlsHandler : MonoBehaviour
{
    public event Action<float> SteeringAngleChanged;
    public event Action<float> CarSpeedChanged;
    public event Action GearshiftReversed;
    [SerializeField] SoundName engineSound;
    [SerializeField] SoundName accelerateSound;

    //int _steersReceived = 0;
    float _steerInput = 0;
    float _gasInput;

    int _engineSound = -1;

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
        SteeringAngleChanged?.Invoke(_steerInput );
        CarSpeedChanged?.Invoke(_gasInput);
        handleEngineSound();
    }

    void handleEngineSound()
    {
        if (_gasInput > .5f)
        {
            if (_engineSound != -1) return;
            _engineSound = SoundManager.Instance.PlaySound(accelerateSound);
        }
        else if(_engineSound != -1)
        {
            SoundManager.Instance.StopSound(_engineSound);
            _engineSound = -1;
        }
    }
}
