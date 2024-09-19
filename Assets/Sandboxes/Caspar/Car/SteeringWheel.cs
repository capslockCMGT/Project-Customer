using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[SelectionBase]
public class SteeringWheel : MonoBehaviour
{
    [SerializeField] Transform Renderer;
    [SerializeField] CarControlsHandler carHandler;
    [SerializeField] CarController _carController;
    [SerializeField] float _steeringSens = 25f;
    [SerializeField] float _steeringAngle = 25f;
    [SerializeField] int _honksBeforeCooldownStart;
    [SerializeField] float _honkCooldown;
    [SerializeField] SoundName _soundName;

    [SerializeField] bool _canReposition;

    bool _canHonk = true;
    int _honks;
    float _currAngle;

    void Start()
    {
        var grab = GetComponent<GrabbableItem>();
        if(grab == null )
        {
            Debug.LogWarning("steering wheel does not have a grabbable and will not function.");
            Destroy(this);
            return;
        }

        if(carHandler == null)
        {
            Debug.LogWarning("steering wheel could not find a carControlsHandler and will not function.");
            Destroy(this);
            return;
        }

        if(Renderer == null)
            Debug.LogWarning("steering wheel doesnt have a renderer set. can you add it pls");

        grab.onItemGrabbed.AddListener((PlayerController addedController) => 
        {
            if(addedController.PlayerCanSteer)
                addedController.UpdateLeftJoystick.AddListener(carHandler.UpdateSteeringAngle);
        });
        grab.onItemReleased.AddListener((PlayerController addedController) => 
        {
            if(addedController.PlayerCanSteer)
                addedController.UpdateLeftJoystick.RemoveListener(carHandler.UpdateSteeringAngle);
        });

        grab.onPlayerInteract.AddListener((PlayerController addedController) =>
        {
            if (!_canHonk) return;
            Debug.Log("HONLK!!1!");
            SoundManager.Instance.PlaySound(_soundName);
            if (++_honks > _honksBeforeCooldownStart)
                StartCoroutine(HonkTimer());
        });

        if (Renderer != null) 
            carHandler.SteeringAngleChanged += (float input) => 
            {
                _currAngle += input * _steeringSens;
                _currAngle = Mathf.Clamp(_currAngle, -_steeringAngle, _steeringAngle);
                //goes back to 0 in increments
                if (_canReposition && input == 0 && _currAngle != 0)
                {
                    CenterWheel();
                }
                Renderer.localRotation = Quaternion.AngleAxis(_currAngle, Vector3.up);

                _carController.SetWheelAngle(_currAngle / _steeringAngle);
            };
    }

    void CenterWheel()
    {
        var prevAngle = _currAngle;
        _currAngle -= _steeringSens * Mathf.Sign(_currAngle);
        if (prevAngle * _currAngle < 0)
            _currAngle = 0;
    }

    IEnumerator HonkTimer()
    {
        _honks = 0;
        _canHonk = false;
        yield return new WaitForSeconds(_honkCooldown);
        _canHonk = true;
    }
}
