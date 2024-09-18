using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture2D texture1;
    public Texture2D texture2;
    public Material Glow;
    public float Timer;
    public float CD;
    public float State;
    [ColorUsageAttribute(true, true)]
    public Color Color1;
    [ColorUsageAttribute(true, true)]
    public Color Color2;
    void Start()
    {
        State = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= CD && State == 2)
        {
            Glow.SetTexture("_MainTex", texture1);
            Glow.SetColor("_Glow_Color", Color1);
            Timer = 0;
            State = 1;
        }
        else if (Timer >= CD && State == 1)
        {
            Glow.SetTexture("_MainTex", texture2);
            Glow.SetColor("_Glow_Color", Color2);
            Timer = 0;
            State = 2;
        }
    }
}
