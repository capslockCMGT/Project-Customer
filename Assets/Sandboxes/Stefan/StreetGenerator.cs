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
    void OnEnable()
    {
        _cityGenerator.TileCollapsed += OnTileCollapse;
    }
    void OnDisable()
    {
        _cityGenerator.TileCollapsed -= OnTileCollapse;
    }

    void OnTileCollapse(Tile tile, GameObject gameObject)
    {

    }

    void MakeRandomCrosswalks()
    {
        List<CellAndDir> neighbours = new();
        //connect sidewalks
        foreach (Cell cell in _cityGenerator.Cells)
        {
            neighbours.Clear();
            _cityGenerator.GetNeighbouringCellsAndDirections(cell.X, cell.Y, neighbours);

            foreach (CellAndDir neighbourData in neighbours)
            {
                TileConnections sidewalkManager = neighbourData.cell.WorldObj.GetComponent<TileConnections>();

                //sidewalkManager.get MyGrid.GetOppositeDir((int)neighbourData.dir);
            }
            
        }

        //instantiate random crosswalks
    }
}
