using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelCollider[] frontWheelColliders = new WheelCollider[2];
    private void Start()
    {
        GetComponent<SteeringManager>().SteeringAngleChanged += OnSteer;
        if (frontWheelColliders[0] == null || frontWheelColliders[1] == null)
            Debug.LogWarning("front wheels were not properly set");
    }

    void OnSteer(float newAngle)
    {
        frontWheelColliders[0].steerAngle = newAngle*30f;
        frontWheelColliders[1].steerAngle = newAngle*30f;
    }
}
