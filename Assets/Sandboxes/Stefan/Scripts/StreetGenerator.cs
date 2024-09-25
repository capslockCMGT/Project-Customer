using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MyGrid))]
public class StreetGenerator : MonoBehaviour
{
    [SerializeField] NPC[] _npcPrefabs;
    [SerializeField] Transform _npcHolder;

    MyGrid _cityGenerator;

    void Awake()
    {
        _cityGenerator = GetComponent<MyGrid>();
    }

    void Start()
    {
        MakeRandomCrosswalks();
    }
    void OnEnable()
    {
        _cityGenerator.MapGenerated.AddListener(MakeRandomCrosswalks);
    }
    void OnDisable()
    {
        _cityGenerator.MapGenerated.RemoveListener(MakeRandomCrosswalks);
    }

    //void OnTileCollapse(Tile tile, GameObject gameObject)
    //{
    //    CrossManager crossPoint = gameObject.GetComponentInChildren<CrossManager>();
    //    if (crossPoint == null) return;

    //    Instantiate(_npcPrefab,crossPoint.transform.position,Quaternion.identity, _npcHolder);
    //}

    void MakeRandomCrosswalks()
    {
        GameObject newTileHolder = new("NpcHolder");
        Destroy(_npcHolder.gameObject);
        _npcHolder = newTileHolder.transform;
        _npcHolder.parent = transform;

        foreach (Cell cell in _cityGenerator.Cells)
        {
            CrossManager crossPoint = cell.WorldObj.GetComponentInChildren<CrossManager>();
            if (crossPoint == null) continue;

            Instantiate(_npcPrefabs[UnityEngine.Random.Range(0, _npcPrefabs.Length)], crossPoint.transform.position, Quaternion.identity, _npcHolder);
            Instantiate(_npcPrefabs[UnityEngine.Random.Range(0, _npcPrefabs.Length)], crossPoint.transform.position, Quaternion.identity, _npcHolder);
        }

        //instantiate random crosswalks
    }
}
