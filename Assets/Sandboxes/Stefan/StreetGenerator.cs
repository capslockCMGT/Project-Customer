using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(MyGrid))]
public class StreetGenerator : MonoBehaviour
{
    [SerializeField] NPC _npcPrefab;
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
    //void OnEnable()
    //{
    //    _cityGenerator.TileCollapsed += OnTileCollapse;
    //}
    //void OnDisable()
    //{
    //    _cityGenerator.TileCollapsed -= OnTileCollapse;
    //}

    //void OnTileCollapse(Tile tile, GameObject gameObject)
    //{
    //    CrossManager crossPoint = gameObject.GetComponentInChildren<CrossManager>();
    //    if (crossPoint == null) return;

    //    Instantiate(_npcPrefab,crossPoint.transform.position,Quaternion.identity, _npcHolder);
    //}

    void MakeRandomCrosswalks()
    {
        foreach (Cell cell in _cityGenerator.Cells)
        {
            CrossManager crossPoint = cell.WorldObj.GetComponentInChildren<CrossManager>();
            if (crossPoint == null) continue ;

            Instantiate(_npcPrefab, crossPoint.transform.position, Quaternion.identity, _npcHolder);
        }

        //instantiate random crosswalks
    }
}
