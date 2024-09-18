using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DeductionUI : MonoBehaviour
{
    [SerializeField] Image _bg;
    [SerializeField] TextMeshProUGUI _textMesh;
    [SerializeField] float _displayTime;

    SafetyCreditsManager _safetyCreditsManager;
    Coroutine _currentSign;

    void OnEnable()
    {
        StartCoroutine(GetSafetyManager());
    }

    void OnDisable()
    {
        _safetyCreditsManager.CreditsChanged -= OnDeduction;
    }

    void OnDeduction(int newValue, int deductedAmount, string message)
    {
        if (_currentSign != null)
            StopCoroutine(_currentSign);
        _currentSign = StartCoroutine(DisplayDeductionWarning(newValue, deductedAmount, message));
    }

    IEnumerator DisplayDeductionWarning(int newValue, int deductedAmount, string message)
    {
        _bg.gameObject.SetActive(true);
        _textMesh.text = $"Lost {deductedAmount} safety points because: {message}!";
        yield return new WaitForSeconds(_displayTime);
        //fade
        _bg.gameObject.SetActive(false);
    }

    IEnumerator GetSafetyManager()
    {
        while(_safetyCreditsManager == null) 
        {
            yield return null;
            _safetyCreditsManager = GameManager.Instance.PlayerCar.GetComponentInChildren<SafetyCreditsManager>();
            _safetyCreditsManager.CreditsChanged += OnDeduction;

        }
    }
}
