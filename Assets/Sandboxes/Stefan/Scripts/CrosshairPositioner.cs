using UnityEngine;

public class CrosshairPositioner : MonoBehaviour
{
    [SerializeField] RectTransform _p1Position;
    [SerializeField] RectTransform _p2Position;
    [SerializeField] Canvas _canvas;

    //void Awake()
    //{
        
    //}

    void FixedUpdate()
    {
        float centerOffset = _canvas.pixelRect.width / 4f;

        _p1Position.position = _canvas.transform.position + Vector3.left * centerOffset;
        _p2Position.position = _canvas.transform.position + Vector3.right * centerOffset;
    }
}
