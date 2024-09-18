using System;
using UnityEngine;

public class SafetyCreditsManager : MonoBehaviour
{
    public event Action<int, int, string> CreditsChanged;

    [field: SerializeField] public int SafetyCredits { get; private set; }

    public void DeductCredits(int amount, string deductionName)
    {
        SafetyCredits -= amount;
        CreditsChanged?.Invoke(SafetyCredits, amount, deductionName);
    }
}
