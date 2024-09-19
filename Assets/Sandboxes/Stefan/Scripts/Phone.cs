using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GrabbableItem))]
public class Phone : MonoBehaviour
{
    GrabbableItem _grababble;
    [SerializeField] GameObject _callState;
    [SerializeField] GameObject _talkState;
    [SerializeField] GameObject _idleState;
    [SerializeField] bool _inspectorCall;
    [SerializeField] float _callTime = 10;

    void Awake()
    {
        _grababble = GetComponent<GrabbableItem>();    
    }

    void OnEnable()
    {
        _grababble.onPlayerInteract.AddListener(OnInteract);
    }

    void OnDisable()
    {
        _grababble.onPlayerInteract.RemoveListener(OnInteract);
    }

    void FixedUpdate()
    {
        if (_inspectorCall)
        {
            _inspectorCall = false;
            StartCall();
        }
    }

    public void StartCall()
    {
        _idleState.SetActive(false);
        _callState.SetActive(true);
    }

    void OnInteract(PlayerController controller)
    {
        if (!_callState.activeInHierarchy) return;
        
        bool isDriver = controller.Player == 0;

        _callState.SetActive(false);

        if (isDriver)
        {
            _idleState.SetActive(true);
            //turn off phone call 

        }
        else
        {
            //accept phone call and start talking
            StartCoroutine(Talk());
        }
    }

    IEnumerator Talk()
    {
        _talkState.SetActive(true);
        yield return new WaitForSeconds(_callTime);
        _talkState.SetActive(false);
        _idleState.SetActive(true);
    }    
}
