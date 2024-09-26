using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject Menu;
    
    static bool _paused = false;
    static GameObject _menu;

    private void Awake()
    {
        if (_menu == null)
            _menu = Menu;
    }
    public void TogglePause()
    {
        if (GameManager.IsGameOver) return;
        _paused = !_paused;
        Time.timeScale = _paused ? 0 : 1;
        _menu?.SetActive(_paused);
    }
}
