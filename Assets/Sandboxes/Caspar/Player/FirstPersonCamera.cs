using UnityEngine;

[SelectionBase]
public class FirstPersonCamera: MonoBehaviour
{
    public float LookSensitivity = 3;
    [SerializeField] Transform _renderer;

    Vector2 _mousePos;

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

        transform.localRotation = rot*up;
        _renderer.localRotation = rot;
    }
}