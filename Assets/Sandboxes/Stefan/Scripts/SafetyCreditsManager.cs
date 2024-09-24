using System;
using UnityEngine;

public class SafetyCreditsManager : MonoBehaviour
{
    public event Action<int, int, string> CreditsChanged;

    [field: SerializeField] public int SafetyCredits { get; private set; }
    [SerializeField] SoundName _deductionSound;

    public void DeductCredits(int amount, string deductionName)
    {
        SafetyCredits -= amount;
        CreditsChanged?.Invoke(SafetyCredits, amount, deductionName);
        SoundManager.Instance.PlaySound(_deductionSound);
    }
}
