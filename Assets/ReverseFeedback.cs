using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseFeedback : MonoBehaviour
{
    public Texture2D texture1;
    public Material Glow;
    public float State;
    [ColorUsageAttribute(true, true)]
    public Color Color1;
    [ColorUsageAttribute(true, true)]
    public Color Color2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (State == 1)
        {
            Glow.SetColor("_Glow_Color", Color1);
        }
        else if (State == 2)
        {
            Glow.SetColor("_Glow_Color", Color2);
        }

    }
}
