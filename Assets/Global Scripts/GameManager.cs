using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] SoundName[] _bgMusic;
    [SerializeField] bool _startMusic;
    [SerializeField] bool _stopMusic;

    [SerializeField] MyGrid _grid;
    [SerializeField] GameObject _carPrefab;
    [SerializeField] CanvasGroup _fadeScreen;
    [SerializeField] float _fadeScreenTime = .8f;
    [SerializeField] GameObject _passengerGameOver;
    [SerializeField] GameObject _driverGameOver;
    [SerializeField] GameObject _passengerWin;
    [SerializeField] GameObject _driverWin;
    [SerializeField] bool _gameOverTest;
    [SerializeField] bool _lockCursorOnStart = true;

    readonly List<PlayerInput> _playerInputs = new(2);
    public static GameManager Instance { get; private set; }
    [field: SerializeField] public GameObject PlayerCar { get; private set; }

    int _currentMusic = -1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    void Update()
    {
        if(_gameOverTest)
        {
            _gameOverTest = false;
            GameOver(false,true);
        }

        if (_stopMusic)
        {
            _stopMusic = false;
            StopMusic();
        }
    }

    void Start()
    {
        if (_grid != null)
        {
            OnMapGenerated();
            PlayerCar.GetComponentInChildren<NavigationDisplayRenderer>().Init(_grid);
        }


        if(_lockCursorOnStart)
            Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(Utils.DoFadeOut(_fadeScreen, _fadeScreenTime, 0));

        if (_startMusic && _bgMusic.Length != 0)
            PlayRandomMusic();
    }

    public void StopMusic()
    {
        if (_currentMusic != -1)
            SoundManager.Instance.StopSound(_currentMusic);

        _currentMusic = -1;
    }

    void PlayRandomMusic()
    {
        _currentMusic = SoundManager.PlayRandomSound(_bgMusic/*, PlayRandomMusic*/);
    }

    void OnDisable()
    {
        StopMusic();
        Instance = null;
    }

    void OnMapGenerated()
    {
        SpawnCar();
    }

    public void SpawnCar()
    {
        Cell startCell = _grid.Cells[_grid.StartPos.y, _grid.StartPos.x];
        //shoot a ray from above to spawn the car exactly on road
        Vector3 startCenter = startCell.WorldObj.transform.position;
        Physics.Raycast(startCenter + Vector3.up * 20, Vector3.down, out RaycastHit hit, 25);

        GameObject carInst = Instantiate(_carPrefab, hit.point + Vector3.up * 5, Quaternion.AngleAxis(startCell.WorldObj.transform.rotation.eulerAngles.y - 90, Vector3.up));
        PlayerCar = carInst;

    }
    /// <summary>
    /// Spawns the game over panel
    /// </summary>
    /// <param name="driverState">is true if driver won, and false if driver lost</param>
    /// <param name="passengerState">is true if passenger won, and false if passenger lost</param>
    public void GameOver(bool driverState, bool passengerState)
    {
        foreach (var input in _playerInputs)
        {
            input.DeactivateInput();
        }
        foreach (FirstPersonCamera camController in PlayerCar.GetComponentsInChildren<FirstPersonCamera>())
        {
            camController.enabled = false;
        }

        if(driverState)
            _driverWin.SetActive(true);
        else
            _driverGameOver.SetActive(true);

        if(passengerState)
            _passengerWin.SetActive(true);
        else
            _passengerGameOver.SetActive(true);
    }

    public void FinishLevel()
    {
        _fadeScreen.gameObject.SetActive(true);
        StartCoroutine(Utils.DoFadeIn(_fadeScreen, _fadeScreenTime, 0, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1)));

    }

    public void ReloadLevel()
    {
        Debug.Log("Reload");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnInputAdd(PlayerInput input)
    {
        _playerInputs.Add(input);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
