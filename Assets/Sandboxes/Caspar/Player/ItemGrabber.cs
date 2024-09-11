using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    [SerializeField] Rigidbody Car;
    [SerializeField] float MaxHandReach = 3;
    [SerializeField] float HoldingItemDistance = 1;
    [SerializeField] float ThrowForce = 10;
    [SerializeField] float HoldingForceStrength = 5;
    [SerializeField] bool disableGravity = false;

    class grabbedItem
    {
        public GrabbableItem grabbable;
        public Rigidbody itemRB;
    }

    grabbedItem _leftHand;
    grabbedItem _rightHand;

    void temporaryControls()
    {
        if (Input.GetMouseButtonDown(0))
            TryGrabReleaseItem(true);
        if (Input.GetMouseButtonDown(1))
            TryGrabReleaseItem(false);
    }
    private void Start()
    {
        if(Car == null) Car = GetComponent<Rigidbody>();
        if (Car == null) Debug.Log("couldnt find car so velocity is relative to the world");
    }
    public void Update()
    {
        temporaryControls();
    }

    public void FixedUpdate()
    {
        TryHoldItemToPosition(_leftHand, transform.position + transform.forward*HoldingItemDistance);
    }

    void TryHoldItemToPosition(grabbedItem heldItem, Vector3 position)
    {
        if (heldItem == null) return;
        var rb = heldItem.itemRB;
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
    }

    public void TryGrabReleaseItem(bool leftHand)
    {
        //the hand were working with to make code more agnostic
        grabbedItem workingHand;
        if (leftHand)
            workingHand = _leftHand;
        else workingHand = _rightHand;

        if(workingHand == null)
        {
            //try pickikng up the item in the middle of the view
            Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hitinfo, MaxHandReach);
            if (hitinfo.rigidbody == null) return;
            var grabbable = hitinfo.transform.GetComponent<GrabbableItem>();
            if (grabbable == null) return;

            //if theres a grabbable item, set it
            workingHand = new grabbedItem() { 
                grabbable = grabbable, 
                itemRB = hitinfo.rigidbody, 
            };
            grabbable.Grab();
            if(disableGravity)
                hitinfo.rigidbody.useGravity = false;
        }
        else
        {
            //add a force to the thrown object and release it
            workingHand.itemRB.AddForce(transform.forward*ThrowForce,ForceMode.Impulse);
            if(disableGravity)
                workingHand.itemRB.useGravity = true;
            workingHand.grabbable.Release();
            workingHand = null;
        }

        //set the hand back to the altered working hand - set to a new object if grabbed, or set to null if released
        if (leftHand)
            _leftHand = workingHand;
        else _rightHand = workingHand;
    }
    public void TryInteractWithItem(bool leftHand)
    {
        //player interaction - activate the grabbable's custom functionality
        if (leftHand) {
            if (_leftHand != null) _leftHand.grabbable.PlayerInteract();
        } else if (_rightHand != null) _rightHand.grabbable.PlayerInteract();
    }
}
