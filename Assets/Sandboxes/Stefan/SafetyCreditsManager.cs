using System;
using UnityEngine;

public class SafetyCreditsManager : MonoBehaviour
{
    public event Action<int> CreditsChanged;

    [field: SerializeField] public int SafetyCredits { get; private set; }

    public void DeductCredits(int amount)
    {
        SafetyCredits -= amount;
        CreditsChanged?.Invoke(SafetyCredits);
    }
}
