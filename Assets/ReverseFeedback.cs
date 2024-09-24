using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReverseFeedback : MonoBehaviour
{
    public Texture2D texture1;
    public Material Glow;
    bool _isReversed;
    [ColorUsageAttribute(true, true)]
    public Color _reverseColor;
    [ColorUsageAttribute(true, true)]
    public Color _reverseColorOff;
    [SerializeField] GrabbableItem _grabbable;


    void Start()
    {
        _grabbable.onPlayerInteract.AddListener((c) =>
        {
            _isReversed = !_isReversed;
            if (_isReversed)
            {
                Glow.SetColor("_Glow_Color", _reverseColor);
            }
            else
            {
                Glow.SetColor("_Glow_Color", _reverseColorOff);
            }
        });
    }

}
