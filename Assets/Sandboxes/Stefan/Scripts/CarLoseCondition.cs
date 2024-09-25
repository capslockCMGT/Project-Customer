using UnityEngine;
using UnityEngine.UIElements;

public class CarLoseCondition : MonoBehaviour
{
    [SerializeField] float _penaltyThreshhold = 3f;

    SafetyCreditsManager _safetyCreditsManager;
    float _penaltyAccumulation;
    bool _done = false;
    private void Awake()
    {
        _safetyCreditsManager = GetComponentInChildren<SafetyCreditsManager>();
    }

    void FixedUpdate()
    {
        if(_safetyCreditsManager.SafetyCredits < 0 && !_done)
        {
            _done = true;
            OnDeductionToZero();
        }
        if (_done || NotLoseCondition())
        {
            _penaltyAccumulation = 0;
            return;
        }

        _penaltyAccumulation += Time.deltaTime;

        _done = _penaltyAccumulation >= _penaltyThreshhold;

        if(_done)
            OnCarFlip();

    }

    void OnCarFlip()
    {
        GameManager.Instance.GameOver(false, false);
    }

    void OnDeductionToZero()
    {
        GameManager.Instance.GameOver(false, true);
    }

    bool NotLoseCondition()
    {
        return Vector3.Dot(transform.up, Vector3.up) > 0;
    }
}
