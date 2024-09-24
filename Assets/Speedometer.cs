using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public GameObject Car;
    Material Glow13;
    Rigidbody Rigidbody;
    float Speedx;
    float Speedz;
    void Start()
    {
        Glow13 = GetComponent<Renderer>().material;
        Rigidbody = Car.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Speedx = Rigidbody.velocity.x;
        Speedz = Rigidbody.velocity.z;

        if (Speedx > 0)
        {
            Speedx = Speedx * -1;
        }

        if (Speedz > 0)
        {
            Speedz = Speedz * -1;
        }

        Glow13.SetFloat("_Progress_Bar", (Speedz + Speedx) * 3 + 30);
    }
}
