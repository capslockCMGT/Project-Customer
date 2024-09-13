using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemGrabber : MonoBehaviour
{
    [SerializeField] Rigidbody Car;
    [SerializeField] Transform LeftHandRenderer;
    [SerializeField] Transform RightHandRenderer;

    [SerializeField] float MaxHandReach = 3;
    [SerializeField] float HoldingItemDistance = 1;
    [SerializeField] float ThrowForce = 10;
    [SerializeField] float HoldingForceStrength = 5;
    [SerializeField] float DistanceBetweenHands = .3f;
    [SerializeField] bool DisableGravity = false;

    class grabbedItem
    {
        public GrabbableItem grabbable;
        public Rigidbody itemRB;
    }

    grabbedItem _leftHand;
    grabbedItem _rightHand;

    private void Start()
    {
        if(Car == null) Car = GetComponent<Rigidbody>();
        if (Car == null) Debug.Log($"couldnt find car for '{gameObject.name}' so velocity is relative to the world");
        else if (Car.interpolation == RigidbodyInterpolation.None) Debug.Log("car's interpolation is set to None, movement of grabbed objects may look janky");
    }

    public void FixedUpdate()
    {
        TryHoldItemToPosition(_leftHand, transform.position + transform.forward*HoldingItemDistance - transform.right*DistanceBetweenHands*.5f);
        TryHoldItemToPosition(_rightHand, transform.position + transform.forward*HoldingItemDistance + transform.right*DistanceBetweenHands*.5f);
    }

    void TryHoldItemToPosition(grabbedItem heldItem, Vector3 position)
    {
        if (heldItem == null) return;
        var rb = heldItem.itemRB;
        if (rb == null || rb.isKinematic) return;

        Vector3 rbPos = rb.position;
        Vector3 posDifference = rbPos - position;
        Vector3 posDifferenceNormalized = posDifference.normalized;

        Vector3 carVelocity = Vector3.zero;
        if (Car != null) carVelocity = Car.velocity;

        //dot between the velocity and point direction, between -1, 1 for how correct the direction is
        float dirCorrectness = Vector3.Dot(posDifferenceNormalized, rb.velocity-carVelocity);
        dirCorrectness = Mathf.Clamp(dirCorrectness, -1, 1);

        //arbitrary function to get the strength of the centering force
        float centeringForce = posDifference.magnitude - dirCorrectness*posDifference.magnitude;
        centeringForce *= HoldingForceStrength;

        if (centeringForce < 0) centeringForce = 0;

        rb.velocity -= carVelocity;
        rb.velocity *= .5f + .25f*dirCorrectness;
        rb.velocity += carVelocity;
        rb.AddForce(-posDifferenceNormalized * centeringForce, ForceMode.Force);
        rb.angularVelocity *= .9f;
    }
    public void TryInteractWithItem(bool leftHand)
    {
        grabbedItem hand = leftHand ? _leftHand : _rightHand;
        if (hand == null) return;
        hand.grabbable.onPlayerInteract?.Invoke();
    }
    public void TryGrabReleaseItem(bool leftHand, PlayerController controller)
    {
        //the hand were working with to make code more agnostic
        grabbedItem workingHand;
        Transform workingHandRenderer;
        if (leftHand)
        {
            workingHand = _leftHand;
            workingHandRenderer = LeftHandRenderer;
        }
        else
        {
            workingHand = _rightHand;
            workingHandRenderer = RightHandRenderer;
        }

        if(workingHand == null)
        {
            //try pickikng up the item in the middle of the view
            Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hitinfo, MaxHandReach);

            if (hitinfo.transform == null) return;

            var grabbable = hitinfo.transform.GetComponent<GrabbableItem>();
            Debug.Log(hitinfo.transform.name);
            if (grabbable == null) return;

            //if theres a grabbable item, set it
            workingHand = new grabbedItem() { 
                grabbable = grabbable, 
                itemRB = hitinfo.rigidbody, 
            };

            grabbable.Grab(controller);
            workingHandRenderer.position = hitinfo.point;
            workingHandRenderer.parent = grabbable.Renderer;

            if (DisableGravity && hitinfo.rigidbody != null)
                hitinfo.rigidbody.useGravity = false;

            if (hitinfo.rigidbody != null && hitinfo.rigidbody.interpolation == RigidbodyInterpolation.None) 
                Debug.Log($"grabbed rigidbody '{hitinfo.rigidbody.gameObject.name}' has interpolation set to None, movement may look janky");
        }
        else
        {
            //add a force to the thrown object and release it
            if (workingHand.itemRB != null)
            {
                workingHand.itemRB.AddForce(transform.forward * ThrowForce, ForceMode.Impulse);
                if (DisableGravity)
                    workingHand.itemRB.useGravity = true;
            }
            workingHand.grabbable.Release(controller);
            workingHand = null;

            workingHandRenderer.position = transform.position;
            workingHandRenderer.parent = transform;
        }

        //set the hand back to the altered working hand - set to a new object if grabbed, or set to null if released
        if (leftHand)
            _leftHand = workingHand;
        else _rightHand = workingHand;
    }
}