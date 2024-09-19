using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TrafficLight : MonoBehaviour
{
    [SerializeField] bool _useFixedTime;
    [SerializeField] float _fixedActivationTime;
    [SerializeField] Range _randomActivationRange;
    [SerializeField] float _redActiveTime;
    [SerializeField] UnityEvent GreenEnter;
    [SerializeField] UnityEvent RedEnter;
    [SerializeField] int _creditViolation;
    [SerializeField] float _deductionCooldown = 3;

    float _currentRandomTime;
    bool _carCanCross;
    bool _cooldownOver = true;

    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (true)
        {
            ResetTimer();
            GreenEnter?.Invoke();
            _carCanCross = false;
            yield return new WaitForSeconds(_currentRandomTime);
            RedEnter.Invoke();
            _carCanCross = true;
            yield return new WaitForSeconds(_redActiveTime);
        }
    }

    void ResetTimer()
    {
        _currentRandomTime = _useFixedTime ? _fixedActivationTime : Random.Range(_randomActivationRange.Min, _randomActivationRange.Max);
    }

    void OnTriggerEnter(Collider other)
    {
        Transform parent = GetTopParent(other.transform);
        if (!parent.CompareTag("Car") || !_carCanCross || !_cooldownOver) return;

        SafetyCreditsManager creditManager = parent.GetComponentInChildren<SafetyCreditsManager>();
        Debug.Log("passed a red light");
        creditManager.DeductCredits(_creditViolation, "Red light cross");
        StartCoroutine(Cooldown());
    }

    Transform GetTopParent(Transform child)
    {
        if(child.parent != null)
            return GetTopParent(child.parent);
        return child;
    }

    IEnumerator Cooldown()
    {
        _cooldownOver = false;
        yield return new WaitForSeconds(_deductionCooldown);
        _cooldownOver = true;
    }
}
