using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject Menu;
    bool _paused = false;
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.P)) return;
        TogglePause();
    }
    public void TogglePause()
    {
        _paused = !_paused;
        Time.timeScale = _paused ? 0 : 1;
        Menu.SetActive(_paused);
    }
}
