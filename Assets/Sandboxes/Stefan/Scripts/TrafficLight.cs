using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] GameObject _pedestrianRedLight;
    [SerializeField] GameObject _pedestrianGreenLight;
    [SerializeField] GameObject _carRedLight;
    [SerializeField] GameObject _carYellowLight;
    [SerializeField] GameObject _carGreenLight;
    [SerializeField] GameObject _pedestrianRedLight2;
    [SerializeField] GameObject _pedestrianGreenLight2;
    [SerializeField] GameObject _carRedLight2;
    [SerializeField] GameObject _carYellowLight2;
    [SerializeField] GameObject _carGreenLight2;
    [SerializeField] float _lightSwitchTime = .4f;
    Coroutine _togglingCoroutine;

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
        if (child.parent != null)
            return GetTopParent(child.parent);
        return child;
    }

    IEnumerator Cooldown()
    {
        _cooldownOver = false;
        yield return new WaitForSeconds(_deductionCooldown);
        _cooldownOver = true;
    }

    public void ToggleLights(bool toRed)
    {
        if (_togglingCoroutine != null)
            StopCoroutine(_togglingCoroutine);
        _togglingCoroutine = StartCoroutine(ToggleCarLights(toRed));
    }

    IEnumerator ToggleCarLights(bool toRed)
    {
        TurnOffAllLights();
        _carYellowLight.SetActive(true);
        _carYellowLight2.SetActive(true);

        yield return new WaitForSeconds(_lightSwitchTime);
        _pedestrianGreenLight.SetActive(toRed);
        _pedestrianRedLight.SetActive(!toRed);

        _carYellowLight.SetActive(false);
        _carGreenLight.SetActive(!toRed);
        _carRedLight.SetActive(toRed);

        _pedestrianGreenLight2.SetActive(toRed);
        _pedestrianRedLight2.SetActive(!toRed);

        _carYellowLight2.SetActive(false);
        _carGreenLight2.SetActive(!toRed);
        _carRedLight2.SetActive(toRed);
    }

    void TurnOffAllLights()
    {
        _pedestrianGreenLight.SetActive(false);
        _pedestrianRedLight.SetActive(false);
        _pedestrianGreenLight2.SetActive(false);
        _pedestrianRedLight2.SetActive(false);

        _carGreenLight.SetActive(false);
        _carYellowLight.SetActive(false);
        _carRedLight.SetActive(false);

        _carGreenLight2.SetActive(false);
        _carYellowLight2.SetActive(false);
        _carRedLight2.SetActive(false);
    }
}
