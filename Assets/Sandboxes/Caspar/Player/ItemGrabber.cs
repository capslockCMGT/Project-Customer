using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    [SerializeField] Rigidbody Car;
    [SerializeField] HandRenderer LeftHandRenderer;
    [SerializeField] HandRenderer RightHandRenderer;


    [SerializeField] float ThrowForce = 10;
    [SerializeField] float HoldingForceStrength = 5;
    [SerializeField] float DistanceBetweenHands = .3f;
    [SerializeField] bool DisableGravity = false;

    [SerializeField, Range(.1f, 7)] float _minHandReach = .1f;
    [SerializeField, Range(.1f, 7)] float _maxHandReach = 3;
    [SerializeField] float _itemReachChangeSpeed = 1f;

    public float HoldingItemDistance { get; private set; } = 1;

    class GrabbedItem
    {
        public GrabbableItem grabbable;
        public Rigidbody itemRB;
        public Outline outline;
    }

    GrabbedItem _leftHand;
    GrabbedItem _rightHand;

    PlayerController _playerController;
    float _currentItemDistanceInput;

    Outline _lastLeftHandOutline;
    Outline _lastRightHandOutline;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.ItemControl.AddListener((input) => _currentItemDistanceInput = input);
        if (Car == null && !TryGetComponent(out Car)) Debug.Log($"couldnt find car for '{gameObject.name}' so velocity is relative to the world");
        else if (Car.interpolation == RigidbodyInterpolation.None) Debug.Log("car's interpolation is set to None, movement of grabbed objects may look janky");
    }

    public void FixedUpdate()
    {
        //in case I destroy after grabbing
        if (_leftHand != null && _leftHand.grabbable == null)
            _leftHand = null;
        if (_rightHand != null && _rightHand.grabbable == null)
            _rightHand = null;

        ChangeItemDistance(_currentItemDistanceInput);
        TryHoldItemToPosition(_leftHand, transform.position + transform.forward * HoldingItemDistance - .5f * DistanceBetweenHands * transform.right);
        TryHoldItemToPosition(_rightHand, transform.position + transform.forward * HoldingItemDistance + .5f * DistanceBetweenHands * transform.right);

        OutlineLookedAtItem(_leftHand, ref _lastLeftHandOutline);
        OutlineLookedAtItem(_rightHand, ref _lastRightHandOutline);
    }

    void OutlineLookedAtItem(GrabbedItem heldItem, ref Outline previousFrameOutline)
    {
        if (previousFrameOutline != null)
        {   //remove outline if not looking (called for every frame when you're not looking)
            previousFrameOutline.enabled = false;
            previousFrameOutline = null;
        }
        if (heldItem != null)
        {
            SetGrabOutline(heldItem.outline);
            previousFrameOutline = heldItem.outline;
            return;
        }

        if (!Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hitinfo, _maxHandReach))
            return;

        if (!hitinfo.transform.CompareTag("Interactable")) return;

        Outline outline = hitinfo.transform.GetComponent<Outline>();
        //if other person is grabbing it, don't color it to look color
        if (outline.enabled == true) return;

        SetLookOutline(outline);
        previousFrameOutline = outline;
    }

    void SetGrabOutline(Outline outline)
    {

        outline.OutlineColor = OutlineSettings.Instance.OutlineGrabColor;
        outline.OutlineWidth = OutlineSettings.Instance.OutlineWidth;
        outline.OutlineMode = OutlineSettings.Instance.OutlineMode;
        outline.enabled = true;

    }

    void SetLookOutline(Outline outline)
    {
        outline.OutlineColor = OutlineSettings.Instance.OutlineLookColor;
        outline.OutlineWidth = OutlineSettings.Instance.OutlineWidth;
        outline.OutlineMode = OutlineSettings.Instance.OutlineMode;
        outline.enabled = true;
    }

    void ChangeItemDistance(float amount)
    {
        HoldingItemDistance = Mathf.Clamp(HoldingItemDistance + amount * Time.deltaTime * _itemReachChangeSpeed, _minHandReach, _maxHandReach);
    }

    void TryHoldItemToPosition(GrabbedItem heldItem, Vector3 position)
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
        float dirCorrectness = Vector3.Dot(posDifferenceNormalized, rb.velocity - carVelocity);
        dirCorrectness = Mathf.Clamp(dirCorrectness, -1, 1);

        //arbitrary function to get the strength of the centering force
        float centeringForce = posDifference.magnitude - dirCorrectness * posDifference.magnitude;
        centeringForce *= HoldingForceStrength;

        if (centeringForce < 0) centeringForce = 0;

        rb.velocity -= carVelocity;
        rb.velocity *= .5f + .25f * dirCorrectness;
        rb.velocity += carVelocity;
        rb.AddForce(-posDifferenceNormalized * centeringForce, ForceMode.Force);
        rb.angularVelocity *= .9f;
    }
    public void TryInteractWithItem(bool leftHand, PlayerController controller)
    {
        GrabbedItem hand = leftHand ? _leftHand : _rightHand;
        if (hand == null) return;
        hand.grabbable.onPlayerInteract?.Invoke(controller);

        HandRenderer h = leftHand ? LeftHandRenderer : RightHandRenderer;
        h.Interact();
    }
    public void TryGrabReleaseItem(bool leftHand, PlayerController controller)
    {
        //the hand were working with to make code more agnostic
        GrabbedItem workingHand;
        HandRenderer workingHandRenderer;
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

        if (workingHand == null)
        {
            //try pickikng up the item in the middle of the view
            //ignore car collider
            if (!Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hitinfo, _maxHandReach))
                return;

            if (!hitinfo.transform.TryGetComponent<GrabbableItem>(out var grabbable)) return;

            //if theres a grabbable item, set it
            workingHand = new GrabbedItem()
            {
                grabbable = grabbable,
                itemRB = hitinfo.rigidbody,
                outline = grabbable.GetComponent<Outline>(),
            };

            if (grabbable.ChangeToDynamicWhileGrabbing)
            {
                workingHand.itemRB.isKinematic = false;
                workingHand.itemRB.velocity = Car.velocity;
            }

            workingHandRenderer.Grab(grabbable.Renderer, hitinfo.point, hitinfo.normal);

            if (DisableGravity && hitinfo.rigidbody != null)
                hitinfo.rigidbody.useGravity = false;

            if (hitinfo.rigidbody != null && hitinfo.rigidbody.interpolation == RigidbodyInterpolation.None && !hitinfo.rigidbody.isKinematic)
                Debug.Log($"grabbed rigidbody '{hitinfo.rigidbody.gameObject.name}' has interpolation set to None, movement may look janky");
            grabbable.Grab(controller);
        }
        else if(workingHand.grabbable != null)
        {
            //add a force to the thrown object and release it
            if (workingHand.itemRB != null)
            {
                workingHand.itemRB.AddForce(transform.forward * ThrowForce, ForceMode.Impulse);
                if (DisableGravity)
                    workingHand.itemRB.useGravity = true;
            }
            //if (workingHand.grabbable.ChangeToDynamicWhileGrabbing)
            //    workingHand.itemRB.isKinematic = true;
            workingHand.grabbable.Release(controller);

            workingHand = null;
            workingHandRenderer.LetGo();
        }

        //set the hand back to the altered working hand - set to a new object if grabbed, or set to null if released
        if (leftHand)
            _leftHand = workingHand;
        else _rightHand = workingHand;
    }
   
    public GameObject GetLeftHandItem()
    {
        if (_leftHand == null) return null;

        return _leftHand.grabbable.gameObject;

    }

    public GameObject GetRightHandItem()
    {
        if (_rightHand == null) return null;
        return _rightHand.grabbable.gameObject;

    }
}