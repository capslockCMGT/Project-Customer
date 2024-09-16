using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(MyGrid))]
public class StreetGenerator : MonoBehaviour
{
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

    //}

    void MakeRandomCrosswalks()
    {
        

        //instantiate random crosswalks
    }
}
