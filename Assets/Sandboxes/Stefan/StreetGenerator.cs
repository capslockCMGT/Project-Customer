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
        List<CellAndDir> neighbours = new();
        //connect sidewalks
        foreach (Cell cell in _cityGenerator.Cells)
        {
            if (!cell.WorldObj.TryGetComponent<TileConnections>(out var mySideWalkManager)) continue;
            
            neighbours.Clear();
            _cityGenerator.GetNeighbouringCellsAndDirections(cell.X, cell.Y, neighbours);

            mySideWalkManager.UpdateDirections();
            foreach (CellAndDir neighbourData in neighbours)
            {
                //check if tiles are connected
                if (!neighbourData.cell.CollapsedTile.Neighbours.Contains(cell.CollapsedTile)) continue;
                if (!neighbourData.cell.WorldObj.TryGetComponent<TileConnections>(out var neighbourSidewalkManager)) continue;
                
                //connect
                neighbourSidewalkManager.UpdateDirections();
                neighbourSidewalkManager.Connect(mySideWalkManager, neighbourData.dir);

            }
            
        }

        //instantiate random crosswalks
    }
}
