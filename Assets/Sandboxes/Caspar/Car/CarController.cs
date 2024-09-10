using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] float EngineStrength = 50;
    [SerializeField] float BrakeStrength = 150;
    [SerializeField]
    [Range(0f, 1f)] float FrontWheelBrakeStrengthMultiplier = .5f;

    Rigidbody carRB;
    float targetSpeed = 0;
    private void Start()
    {
        var controlsHandler = GetComponent<CarControlsHandler>();

        carRB = GetComponent<Rigidbody>();

        if(carRB == null || EngineStrengthAtSpeed == null || controlsHandler == null)
        {
            Debug.LogWarning("CarController was not set up properly - some essential values were null.");
            Destroy(this);
        }

        controlsHandler.SteeringAngleChanged += OnSteer;
        controlsHandler.CarSpeedChanged += OnGas;
    }

    void OnSteer(float newAngle)
    {
        //set the angle of the wheel's colliders
        foreach(Wheel wheel in Wheels)
            if(wheel.frontWheel)
                wheel.collider.steerAngle = newAngle*30;
    }

    void OnGas(float gas)
    {
        gas *= Time.deltaTime;
        
        //targetSpeed is between 0-1, defining the speed at which the car is trying to cruise
        targetSpeed += gas;
        targetSpeed = Mathf.Clamp01(targetSpeed);

        //if the car is already near maximum speed, power down the engine
        float currentSpeed = Vector3.Dot(carRB.velocity,transform.forward) / MaximumSpeed;
        float engineStrength = EngineStrengthAtSpeed.Evaluate(currentSpeed);

        float torque = engineStrength*targetSpeed;
        foreach(Wheel wheel in Wheels)
        {
            //if gas is hit or neutral, dont brake but gas
            if(gas >= 0)
            {
                wheel.collider.motorTorque = torque*EngineStrength;
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
