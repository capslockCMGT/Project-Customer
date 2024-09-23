using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] MyGrid _cityGenerator;
    [SerializeField] Image _blackScreen;
    [SerializeField] Color _screenColor = Color.black;
    private void Awake()
    {
        _cityGenerator.MapGenerated.AddListener(OnMapGeneration);
        _blackScreen.color = _screenColor;
    }
    private void OnMapGeneration()
    {
        _blackScreen.color = new Color(1, 1, 1, 0);
    }
}

