using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera: MonoBehaviour
{
    public float LookSensitivity = 3;

    Vector2 _mousePos;
    float _targetYRot;
    float _currTilt;

    public void UpdateCameraAngle(Vector2 delta)
    {
        _mousePos += delta*LookSensitivity;
    }

    void LateUpdate()
    {
        //handle cursor lock
        if (Input.GetKey(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;

        //if the screen isnt clicked, dont rotate the camera
        if (Cursor.lockState == CursorLockMode.None) return;
        
        //clamp in range 360 to prevent floating point precision
        _mousePos.x += 360;
        _mousePos.x %= 360;
        //clamp so camera cannot go upside down
        _mousePos.y = Mathf.Clamp(_mousePos.y, -89, 89);
        Quaternion rot = Quaternion.AngleAxis(_mousePos.x, Vector3.up);
        Quaternion up = Quaternion.AngleAxis(_mousePos.y, Vector3.left);

        transform.rotation = rot*up;
    }
}