using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TrafficLight : MonoBehaviour
{
    [SerializeField] bool _useFixedTime;
    [SerializeField] float _fixedActivationTime;
    [SerializeField] Range _randomActivationRange;
    [SerializeField] float _redActiveTime;
   // [SerializeField] CrossManager[] _crossWalks;
    [SerializeField] UnityEvent GreenEnter;
    [SerializeField] UnityEvent RedEnter;

    float _currentRandomTime;

    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while(true)
        {
            ResetTimer();
            GreenEnter?.Invoke();
            yield return new WaitForSeconds(_currentRandomTime);
            RedEnter.Invoke();
            yield return new WaitForSeconds(_redActiveTime);
        }
    }

    void ResetTimer()
    {
        _currentRandomTime = _useFixedTime ? _fixedActivationTime : Random.Range(_randomActivationRange.Min, _randomActivationRange.Max);
    }

}
