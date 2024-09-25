using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeductionUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _totalCountMesh;
    [SerializeField] Image _deductionPanelPrefab;
    [SerializeField] Transform _deductionSignsHolder;
    [SerializeField] float _displayTime;


    SafetyCreditsManager _safetyCreditsManager;

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
        StartCoroutine(DisplayDeductionWarning(newValue, deductedAmount, message));
    }

    IEnumerator DisplayDeductionWarning(int newValue, int deductedAmount, string message)
    {
        Image panel = Instantiate(_deductionPanelPrefab, _deductionSignsHolder);
        TextMeshProUGUI txtMesh = panel.GetComponentInChildren<TextMeshProUGUI>();
        txtMesh.text = $"Lost {deductedAmount} safety points because: {message}!";

        UpdateTotalCount(newValue);
        yield return new WaitForSeconds(_displayTime);
        //fade
        Destroy(panel.gameObject);
    }

    IEnumerator GetSafetyManager()
    {
        while (_safetyCreditsManager == null)
        {
            yield return null;
            _safetyCreditsManager = GameManager.Instance.PlayerCar.GetComponentInChildren<SafetyCreditsManager>();
            _safetyCreditsManager.CreditsChanged += OnDeduction;

        }
        UpdateTotalCount(_safetyCreditsManager.SafetyCredits);

    }

    void UpdateTotalCount(int val)
    {
        _totalCountMesh.text = "Current credits: " + val + "$";
    }
}
