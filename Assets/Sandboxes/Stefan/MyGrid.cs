using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class Cell
{
    public List<Tile> Possibilities { get; }
    public int X { get; }
    public int Y { get; }
    public Tile CollapsedTile { get; set; }
    public Cell(int x, int y, List<Tile> possibilities)
    {
        Possibilities = possibilities;
        X = x;
        Y = y;
    }

}

public class MyGrid : MonoBehaviour
{
    const int ROTATIONS = 4;
    [SerializeField] int columns = 10;
    [SerializeField] float _size = 100f;
    [SerializeField] GameObject _prefab1;
    [SerializeField] GameObject _prefab2;
    [SerializeField] GameObject _prefab3;
    [SerializeField] GameObject _prefab4;
    [SerializeField] GameObject _prefab5;
    [SerializeField] GameObject _prefab6;
    [SerializeField] GameObject _cellVisual;

    [SerializeField] bool _visualizeCell;

    public bool Done { get; private set; }
    
    int collapsedTileCount;
    float _cellWidth;
    Cell[,] _cells;

    readonly Tile[] _tiles = new Tile[6];

    void Awake()
    {
        //hardcoded the simetries.
        //The simetries are needed to save memory on repeating the tiles that after rotation
        //look the same

        _tiles[0] = new Tile(_prefab1, true, true, "ABA", "ABA", "ABA", "ABA");
        _tiles[1] = new Tile(_prefab2, true, false, "AAA", "ABA", "AAA", "ABA");
        _tiles[2] = new Tile(_prefab3, false, true, "AAA", "ABA", "ABA", "ABA");
        _tiles[3] = new Tile(_prefab4, false, false, "ABA", "ABA", "AAA", "AAA");
        _tiles[4] = new Tile(_prefab5, true, true, "AAA", "AAA", "AAA", "AAA");
        _tiles[5] = new Tile(_prefab6, false, false, "AAA", "AAA", "AAA", "ABA");

        _cells = new Cell[columns, columns];
        List<Tile> rotatedTiles = GenerateRotateTileStates(_tiles);
        _cellWidth = _size / columns;
        var statesList = _tiles.Concat(rotatedTiles);
        GenerateGrid(columns, statesList);
        FillGridEdgesWithEmptyTiles(columns, _tiles);
    }

    void Start()
    {
        while (!Done)
        {
            Iterate();
        }
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    void Iterate()
    {
        Cell lowestCell = GetLeastEntropyCell();
        CollapseCell(lowestCell);
        Propagate(lowestCell);
    }

    public Cell GetLeastEntropyCell()
    {
        Cell min = null;
        foreach (Cell cell in _cells)
        {
            if (cell.CollapsedTile != null) continue;//is a collapsed cell
            min ??= cell;

            if (cell.Possibilities.Count < min.Possibilities.Count)
                min = cell;
        }
        return min;
    }

    void GenerateGrid(int columns, IEnumerable<Tile> statesList)
    {
        for (int y = 0; y < columns; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if(_visualizeCell)
                {
                    var inst = Instantiate(_cellVisual, transform.position + new Vector3(x * _cellWidth + .5f * _cellWidth, 0, -y * _cellWidth - .5f * _cellWidth), Quaternion.identity);
                    inst.transform.localScale = new Vector3(_cellWidth - .2f, .2f, _cellWidth - .2f);
                }
                _cells[y, x] = new Cell(x, y, new List<Tile>(statesList));
            }
        }
    }

    void FillGridEdgesWithEmptyTiles(int columns, Tile[] allTiles)
    {
        for (int y = 0; y < columns; y++)
            for (int x = 0; x < columns; x++)
            {
                bool isOnEdge = x == 0 || y == 0 || x == _cells.GetLength(1) - 1 || y == _cells.GetLength(0) - 1;
                
                if (!isOnEdge) continue;

                Cell cell = _cells[y, x];
                int emptyTileIndex = allTiles.Length - 2;//hard coded to be empty tile
                CollapseCellWithTile(cell, allTiles[emptyTileIndex]);
                Propagate(cell);
            }
    }

    List<Tile> GenerateRotateTileStates(Tile[] unrotatedTiles)
    {
        List<Tile> rotatedTiles = new List<Tile>();

        foreach (Tile tile in unrotatedTiles)
            for (int i = 1; i < ROTATIONS; i++)
            {
                //the odd/even checks are because the symetric versions of tiles will be one array slot appart
                //(180 degrees since every array slot is a 90 degree rotation) 
                if ((tile.SymetryHorizontal && i % 2 == 0) || (tile.SymetryVertical && i % 2 != 0))
                    continue;

                Tile newTile = tile.Clone();
                for (int J = 0; J < i; J++)
                    newTile.Rotate();

                rotatedTiles.Add(newTile);
            }

        return rotatedTiles;
    }

    public void CollapseCell(Cell cell)
    {
        if (cell.Possibilities.Count == 0)
        {
            Done = true;
            return;
        }
        
        int randomIndex = GetRandomPossibility(cell);
        Tile prototype = cell.Possibilities[randomIndex];
        CollapseCellWithTile(cell, prototype);
    }

    int GetRandomPossibility(Cell cell)
    {
        //if there are more than 1 possibility, chose everything except cap tile
        //else choose cap tile
        //I don't want the city to have dead ends unless there are no other possible configurations
        int randomIndex = Random.Range(0, cell.Possibilities.Count);
        if (cell.Possibilities.Count > 1)
        {
            while (IsCapTile(randomIndex)) randomIndex = Random.Range(0, cell.Possibilities.Count);

        }
        return randomIndex;

        bool IsCapTile(int index)
        {
            return cell.Possibilities[index].Prefab.name == "Cap";
        }
    }

    void CollapseCellWithTile(Cell cell, Tile prototype)
    {
        cell.Possibilities.Clear();
        cell.CollapsedTile = prototype.Clone();
        Tile tile = cell.CollapsedTile;
        //The Y direction is negative because I initialy programed the algorithm in GXPengine and I can't bother
        //to figure out how to write the algorithm with positive y
        var inst = Instantiate(tile.Prefab, transform.position + new Vector3(cell.X * _cellWidth +.5f* _cellWidth, 0, -cell.Y * _cellWidth - .5f * _cellWidth), Quaternion.AngleAxis(tile.Rotation ,transform.up), transform);
        inst.transform.localScale = Vector3.one * _cellWidth;

        if (++collapsedTileCount >= _cells.Length) Done = true;
    }

    void Propagate(Cell cell)
    {
        foreach (CellAndDir val in GetNeighbouringCellsAndDirections(cell.X, cell.Y))
        {
            Cell neighbour = val.cell;
            if (neighbour.CollapsedTile != null) continue;

            int dir = (int)val.dir;

            string mySockets = cell.CollapsedTile.Sockets[dir];
            //constrain
            for (int i = 0; i < neighbour.Possibilities.Count; i++)
            {
                List<Tile> possibilities = neighbour.Possibilities;
                Tile possibility = possibilities[i];
                //the modulo operation is to overlap values, the addition to two is because the opposite side of cell is 2 array slots appart
                if (possibility.Sockets[(dir + 2) % 4] != mySockets)
                    possibilities.RemoveAt(i--);
            }
        }
    }

    List<CellAndDir> GetNeighbouringCellsAndDirections(int x, int y)
    {
        List<CellAndDir> neighbours = new();
        //I'm checking the bounds of the array
        if (x - 1 >= 0)
            neighbours.Add(new CellAndDir(_cells[y, x - 1], NeighbourDir.Left));
        if (x + 1 < _cells.GetLength(1))
            neighbours.Add(new CellAndDir(_cells[y, x + 1], NeighbourDir.Right));
        if (y - 1 >= 0)
            neighbours.Add(new CellAndDir(_cells[y - 1, x], NeighbourDir.Up));
        if (y + 1 < _cells.GetLength(0))
            neighbours.Add(new CellAndDir(_cells[y + 1, x], NeighbourDir.Down));
        return neighbours;
    }

    readonly struct CellAndDir
    {
        public readonly Cell cell;
        public readonly NeighbourDir dir;

        public CellAndDir(Cell cell, NeighbourDir dir)
        {
            this.cell = cell;
            this.dir = dir;
        }
    }
}
