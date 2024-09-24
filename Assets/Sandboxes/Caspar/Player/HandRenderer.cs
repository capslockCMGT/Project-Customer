using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRenderer : MonoBehaviour
{
    [SerializeField] Transform Renderer;
    Transform Empty;

    private void Start()
    {
        if(Renderer == null)
        {
            Debug.LogWarning("hey i want my renderer set please");
            Destroy(this);
            return;
        }
    }
    public void Grab(Transform grabbedObject, Vector3 worldPos)
    {
        GenerateEmpty();
        Empty.parent = grabbedObject;
        Empty.position = worldPos;
    }

    private void LateUpdate()
    {
        GenerateEmpty();
        Renderer.position = Empty.position;
    }

    public void LetGo()
    {
        GenerateEmpty();
        Empty.parent = transform;
        Empty.position = transform.position;
    }

    void GenerateEmpty()
    {
        if (Empty != null) return;
        Empty = new GameObject().transform;
        Empty.parent = transform;
        Empty.position = transform.position;
    }
}
