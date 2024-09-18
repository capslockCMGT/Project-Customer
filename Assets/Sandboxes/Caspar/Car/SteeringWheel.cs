using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class SteeringWheel : MonoBehaviour
{
    [SerializeField] Transform Renderer;
    [SerializeField] CarControlsHandler carHandler;
    [SerializeField] float _steeringSens = 25f;
    [SerializeField] int _honksBeforeCooldownStart;
    [SerializeField] float _honkCooldown;
    [SerializeField] SoundName _soundName;
    bool _canHonk = true;
    int _honks;

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

        if(Renderer != null) 
            carHandler.SteeringAngleChanged += (float fuckshit) => { Renderer.localRotation = Quaternion.AngleAxis(fuckshit * _steeringSens, Vector3.up); };
    }

    IEnumerator HonkTimer()
    {
        _honks = 0;
        _canHonk = false;
        yield return new WaitForSeconds(_honkCooldown);
        _canHonk = true;
    }
}
