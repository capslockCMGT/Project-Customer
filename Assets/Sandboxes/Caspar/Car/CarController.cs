using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Serializable] 
    public struct Wheel
    {
        public WheelCollider collider;
        public Transform renderer;
        public bool frontWheel;
    }
    [SerializeField] Wheel[] Wheels = new Wheel[4];
    [SerializeField] AnimationCurve EngineStrengthAtSpeed;
    [SerializeField] float MaximumSpeed = 30;
    [SerializeField] float MaxSpeedBackwards = 20;
    [SerializeField] float EngineStrength = 500;
    [SerializeField] float BrakeStrength = 5000;
    [SerializeField][Range(0f, 1f)] float FrontWheelBrakeStrengthMultiplier = .5f;
    [SerializeField] float MaxTurnAngle = 30;

    public Vector3 CarVelocity => _carRB.velocity;

    Rigidbody _carRB;
    float _targetSpeed = 0;
    bool _inReverse = false;
    private void Start()
    {
        var controlsHandler = GetComponent<CarControlsHandler>();

        _carRB = GetComponent<Rigidbody>();

        if(EngineStrengthAtSpeed == null || controlsHandler == null)
        {
            Debug.LogWarning("CarController was not set up properly - some essential values were null.");
            Destroy(this);
        }

        controlsHandler.SteeringAngleChanged += OnSteer;
        controlsHandler.CarSpeedChanged += OnGas;
        controlsHandler.GearshiftReversed += () => { _inReverse = !_inReverse; };
    }

    void OnSteer(float newAngle)
    {
        //set the angle of the wheel's colliders
        foreach (Wheel wheel in Wheels)
            if (wheel.frontWheel)
            {
                wheel.collider.steerAngle = newAngle * MaxTurnAngle;
                //cilinder is oriented upwards by default. this is a little jank
                //who did this
                wheel.renderer.rotation = Quaternion.Euler(90, 0, 90 - newAngle * MaxTurnAngle);
            }
    }

    //expects an update every frame. more updates will make changing speed more erratic
    //dont care to make this update speed agnostic
    void OnGas(float gas)
    {
        //a little messed up. a little evil
        //invert driving direction and inputs if were going in reverse
        float reversing = 1;
        float maxVel = MaximumSpeed;
        if(_inReverse)
        {
            gas = -gas;
            reversing = -1;
            maxVel = MaxSpeedBackwards;
        }

        //targetSpeed is between 0-1, defining the speed at which the car is trying to cruise
        _targetSpeed += gas * Time.deltaTime;
        _targetSpeed = Mathf.Clamp01(_targetSpeed);

        //if the car is already near maximum speed, power down the engine
        float currentSpeed = Vector3.Dot(_carRB.velocity,transform.forward) / maxVel;
        float engineStrength = EngineStrengthAtSpeed.Evaluate(currentSpeed);

        float torque = engineStrength*_targetSpeed;
        foreach (Wheel wheel in Wheels)
        {
            //if gas is hit or neutral, dont brake but gas
            if(gas >= 0)
            {
                wheel.collider.motorTorque = torque*EngineStrength*reversing;
                wheel.collider.brakeTorque = 0;
            }
            else
            {
                wheel.collider.motorTorque = 0;
                wheel.collider.brakeTorque = BrakeStrength;
                if (wheel.frontWheel)
                    wheel.collider.brakeTorque *= FrontWheelBrakeStrengthMultiplier;
            }
        }
    }
}
