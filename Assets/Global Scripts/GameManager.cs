using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    [SerializeField] MyGrid _grid;
    [SerializeField] GameObject _carPrefab;
    [SerializeField] Image _gameOverPanel;
    
    List<PlayerInput> _playerInputs = new List<PlayerInput>(2);   
    public static GameManager Instance { get; private set; } 
    public GameObject PlayerCar { get; private set; }

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

    void Start()
    {
        OnMapGenerated();    
    }

    void OnDisable()
    {
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
        Physics.Raycast(startCenter + Vector3.up * 20,Vector3.down, out RaycastHit hit, 25);

        GameObject carInst = Instantiate(_carPrefab, hit.point + Vector3.up * 5, Quaternion.AngleAxis(startCell.WorldObj.transform.rotation.eulerAngles.y - 90, Vector3.up));
        PlayerCar = carInst;
        carInst.GetComponentInChildren<NavigationDisplayRenderer>().Init(_grid);
    }

    public void GameOver()
    {
        _gameOverPanel.gameObject.SetActive(true);
        foreach (var input in _playerInputs)
        {
            input.DeactivateInput();
        }
        foreach (FirstPersonCamera camController in PlayerCar.GetComponentsInChildren<FirstPersonCamera>())
        {
            camController.enabled = false;
        }
        Cursor.lockState = CursorLockMode.None;
    }

    public void FinishLevel()
    {
        //show UI
        //make darkening transition etc
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

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
}
