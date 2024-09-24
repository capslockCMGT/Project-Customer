using System;
using UnityEngine;

public class SafetyCreditsManager : MonoBehaviour
{
    public event Action<int, int, string> CreditsChanged;

    [field: SerializeField] public int SafetyCredits { get; private set; }
    [SerializeField] SoundName _sound;
    [SerializeField] CarController _car;
    [SerializeField] float _speedThreshHold = .01f;

    public void DeductCredits(int amount, string deductionName)
    {
        if (_car.CarVelocity.magnitude < _speedThreshHold) return;

        SafetyCredits -= amount;
        CreditsChanged?.Invoke(SafetyCredits, amount, deductionName);
        SoundManager.Instance.PlaySound(_sound);
    }
}
