using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] MyGrid _grid;
    [SerializeField] GameObject _carPrefab;
    public static GameManager Instance { get; private set; } 

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
        GameObject carInst = Instantiate(_carPrefab, startCell.WorldObj.transform.position, Quaternion.AngleAxis(startCell.WorldObj.transform.rotation.eulerAngles.y - 90, Vector3.up));
        //shoot a ray from above to spawn the car exactly on road
        //Vector3 startCenter = ;
        //Physics.Raycast(startCenter + Vector3.up * 50,Vector3.down, out RaycastHit hit, 55);

        //carInst.transform.position = startCenter;

        carInst.GetComponentInChildren<NavigationDisplayRenderer>().Init(_grid);
    }

    public void FinishLevel()
    {
        //show UI
        //make darkening transition etc
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
