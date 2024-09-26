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
    [SerializeField] float _callTime = 10;
    [SerializeField] Range _callRange;
    [SerializeField] SoundName _callSound;
    [SerializeField] SoundName _talkSound;

    public event Action<PlayerController> Grabbed;
    public bool CanCall { get; set; } = true;
    int _callSoundID = -1;

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

    public void StartCall()
    {
        if(_callSoundID != -1)
        {
            SoundManager.Instance.StopSound( _callSoundID );
        }

        _idleState.SetActive(false);
        _talkState.SetActive(false);
        _callSoundID = SoundManager.Instance.PlaySound(_callSound);
        
        _callState.SetActive(true);
    }

    void OnInteract(PlayerController controller)
    {
        if (!_callState.activeInHierarchy || _talkState.activeInHierarchy) return;
        
        bool isDriver = controller.Player == 0;

        RemoveAllStates();

        if (isDriver)
        {
            _idleState.SetActive(true);
            
        }
        else
        {
            if (_callSoundID != -1)
                SoundManager.Instance.StopSound(_callSoundID);
            _callSoundID = -1;
            StartCoroutine(Talk());
        }

        Grabbed?.Invoke(controller);
    }

    IEnumerator Talk()
    {
        RemoveAllStates();
        _talkState.SetActive(true);
        
        SoundManager.Instance.PlaySound(_talkSound);
        Debug.Log("Talking");
        yield return new WaitForSeconds(_callTime);
        RemoveAllStates();
        _idleState.SetActive(true);
    }    

    IEnumerator RandomCall()
    {
        while(CanCall)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_callRange.Min, _callRange.Max));
            if (_talkState.activeInHierarchy) continue;

            StartCall();
        }

    }

    void RemoveAllStates()
    {
        _callState.SetActive(false);
        _idleState.SetActive(false);
        _talkState.SetActive(false);
    }
}
