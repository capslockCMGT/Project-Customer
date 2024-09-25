using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRenderer : MonoBehaviour
{
    [SerializeField] Transform Renderer;
    [SerializeField] Mesh Gripping;
    [SerializeField] Mesh Relaxed;
    Transform Empty;
    MeshFilter hand;

    private void Start()
    {
        if(Renderer == null)
        {
            Debug.LogWarning("hey i want my renderer set please");
            Destroy(this);
            return;
        }
        hand = Renderer.GetComponentInChildren<MeshFilter>();
    }
    public void Grab(Transform grabbedObject, Vector3 worldPos, Vector3 normal)
    {
        GenerateEmpty();
        Empty.parent = grabbedObject;
        Empty.position = worldPos;
        Empty.rotation = Quaternion.LookRotation(-normal);
        Renderer.gameObject.SetActive(true);
    }

    public void Interact()
    {
        hand.mesh = Gripping;
        StartCoroutine(InteractAnimation());
    }
    IEnumerator InteractAnimation()
    {
        yield return new WaitForSeconds(.2f);
        hand.mesh = Relaxed;
    }

    private void LateUpdate()
    {
        GenerateEmpty();
        Renderer.position = Empty.position;
        Renderer.rotation = Empty.rotation;
    }

    public void LetGo()
    {
        GenerateEmpty();
        Empty.parent = transform;
        Empty.position = transform.position;
        Renderer.gameObject.SetActive(false);
    }

    void GenerateEmpty()
    {
        if (Empty != null) return;
        Empty = new GameObject().transform;
        Empty.parent = transform;
        Empty.position = transform.position;
    }
}
