using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] SoundName[] _honkSounds;

    [SerializeField] bool _canReposition;

    bool _canHonk = true;
    int _honks;
    float _currAngle;

    List<PlayerController> _playersHolding = new();

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

        //this sucks ass.
        grab.onItemGrabbed.AddListener((PlayerController addedController) => 
        {
            if (!addedController.PlayerCanSteer) return;

            addedController.UpdateLeftJoystick.AddListener( carHandler.UpdateSteeringAngle );
            _playersHolding.Add( addedController );
        });
        grab.onItemReleased.AddListener((PlayerController addedController) =>
        {
            if (!addedController.PlayerCanSteer) return;

            _playersHolding.Remove( addedController );

            //unity events will allow you to add duplicates of a function, but when you try to remove one, it will remove ALL.
            //List<> does not have this quirk - so it will be the one to make sure nothing goes wrong here.
            if(!_playersHolding.Contains(addedController))
                addedController.UpdateLeftJoystick.RemoveListener( carHandler.UpdateSteeringAngle );
        });

        grab.onPlayerInteract.AddListener((PlayerController addedController) =>
        {
            if (!_canHonk) return;

            SoundManager.PlayRandomSound(_honkSounds);
            if (++_honks > _honksBeforeCooldownStart)
                StartCoroutine(HonkTimer());
        });

        if (Renderer != null) 
            carHandler.SteeringAngleChanged += (float input) => 
            {
                _currAngle += input * _steeringSens * Time.deltaTime;
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
        _currAngle -= _steeringSens * Mathf.Sign(_currAngle) * Time.deltaTime;
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
