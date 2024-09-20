using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public GameObject Car;
    Material Glow13;
    Rigidbody Rigidbody;
    void Start()
    {
        Glow13 = GetComponent<Renderer>().material;
        Rigidbody = Car.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Glow13.SetFloat("_Progress_Bar", (Rigidbody.velocity.x + Rigidbody.velocity.z) * 10 + 30);
    }
}
