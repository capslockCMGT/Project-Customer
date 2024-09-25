using System;
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
    [SerializeField] Range _callRange;
    [SerializeField] SoundName _callSound;
    [SerializeField] SoundName _talkSound;

    public event Action<PlayerController> Grabbed;
    public bool CanCall { get; set; } = true;

    void Awake()
    {
        _grababble = GetComponent<GrabbableItem>();
        StartCoroutine(RandomCall());
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
        _talkState.SetActive(false);
        SoundManager.Instance.PlaySound(_callSound);
        
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

        Grabbed?.Invoke(controller);
    }

    IEnumerator Talk()
    {
        _talkState.SetActive(true);
        SoundManager.Instance.PlaySound(_callSound);

        yield return new WaitForSeconds(_callTime);
        _talkState.SetActive(false);
        _idleState.SetActive(true);
    }    

    IEnumerator RandomCall()
    {
        while(CanCall)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_callRange.Min, _callRange.Max));
            StartCall();
        }

    }
}
