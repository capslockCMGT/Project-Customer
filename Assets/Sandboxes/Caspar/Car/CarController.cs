using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

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
    [SerializeField] float CenterOfMassOffset = .2f;

    public Vector3 CarVelocity => _carRB.velocity;

    Rigidbody _carRB;
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

        controlsHandler.CarSpeedChanged += OnGas;
        controlsHandler.GearshiftReversed += () => { _inReverse = !_inReverse; };

        _carRB.centerOfMass -= Vector3.up*CenterOfMassOffset;
    }


    public void SetWheelAngle(float angle01)
    {
        //set the angle of the wheel's colliders
        foreach (Wheel wheel in Wheels)
            if (wheel.frontWheel)
            {
                wheel.collider.steerAngle = angle01 * MaxTurnAngle;
                //cilinder is oriented upwards by default. this is a little jank
                //who did this
                wheel.renderer.rotation = Quaternion.Euler(90, 0, 90 - angle01 * MaxTurnAngle);
            }
    }

    //expects an update every frame. more updates will make changing speed more erratic
    //dont care to make this update speed agnostic
    void OnGas(float input)
    {
        //a little messed up. a little evil
        //invert driving direction and inputs if were going in reverse
        float reversing = 1;
        float maxVel = MaximumSpeed;
        if (_inReverse)
        {
            reversing = -1;
            maxVel = MaxSpeedBackwards;
        }


        //if the car is already near maximum speed, power down the engine
        float currentSpeed = Vector3.Dot(_carRB.velocity,transform.forward) / maxVel;
        float engineStrength = EngineStrengthAtSpeed.Evaluate(currentSpeed);

        float torque = engineStrength*input;
        foreach (Wheel wheel in Wheels)
        {
            //if gas is hit or neutral, dont brake but gas
            if(input >= 0)
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
