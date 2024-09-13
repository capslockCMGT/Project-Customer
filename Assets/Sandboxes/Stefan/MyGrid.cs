using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.AI.Navigation;
using UnityEditor.Experimental.GraphView;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    const int ROTATIONS = 4;
    [SerializeField] int columns = 10;
    public float _size = 100f;
    [SerializeField] int _objectiveMinDistance = 10;

    [SerializeField] GameObject _prefab1;
    [SerializeField] GameObject _prefab2;
    [SerializeField] GameObject _prefab3;
    [SerializeField] GameObject _prefab4;
    [SerializeField] GameObject _prefab5;
    [SerializeField] GameObject _prefab6;
    
    [SerializeField] GameObject _cellVisual;

    [SerializeField] GameObject _start;
    public GameObject _end;

    [SerializeField] bool _visualizeCell;

    [SerializeField] Material _pathMat;

    public bool Done { get; private set; }
    public Cell[,] Cells { get; private set; }

    int collapsedTileCount;
    float _cellWidth;
    Tile _startTile;
    Tile _endTile;
    readonly Tile[] _tiles = new Tile[6];

    public Vector2Int StartPos { get; private set; }
    public Vector2Int EndPos { get; private set; }
    readonly AStar _aStar = new();

    void Awake()
    {
        //hardcoded the simetries.
        //The simetries are needed to save memory on repeating the tiles that after rotation
        //look the same
        //up right down left
        _tiles[0] = new Tile(_prefab1, true, true, "ABA", "ABA", "ABA", "ABA");
        _tiles[1] = new Tile(_prefab2, true, false, "AAA", "ABA", "AAA", "ABA");
        _tiles[2] = new Tile(_prefab3, false, true, "AAA", "ABA", "ABA", "ABA");
        _tiles[3] = new Tile(_prefab4, false, false, "ABA", "ABA", "AAA", "AAA");
        _tiles[4] = new Tile(_prefab5, true, true, "AAA", "AAA", "AAA", "AAA");
        _tiles[5] = new Tile(_prefab6, false, false, "AAA", "AAA", "AAA", "ABA");

        _startTile = new Tile(_start, false, false, "AAA", "AAA", "AAA", "ABA");
        _endTile = new Tile(_end, false, false, "AAA", "AAA", "AAA", "ABA");

        for (int i = 0; i < Random.Range(1,5); i++)
            _startTile.Rotate();
        for (int i = 0; i < Random.Range(1, 5); i++)
            _endTile.Rotate();

        List<Tile> rotatedTiles = GenerateRotateTileStates(_tiles);
        var statesList = _tiles.Concat(rotatedTiles);

        int safety = 10;
        List<Cell> path;
        do
        {
            Debug.Log("recreate map");
            GenerateMap(statesList);
            path = DoAstar(StartPos, EndPos);
        } while (--safety > 0 && (path == null || path.Count == 0));
        VisualizeAStar(path);


        GetComponent<NavMeshSurface>().BuildNavMesh();

        //if there is no solution, redo the algo
    }

    void GenerateMap(IEnumerable<Tile> rotatedTiles)
    {
        Cells = new Cell[columns, columns];
        _cellWidth = _size / columns;
        GenerateGrid(columns, rotatedTiles);
        FillGridEdgesWithEmptyTiles(columns);
        //need to first check what rotations of end and start are allowed
        (StartPos, EndPos) = PutRandomStartAndEnd();

        while (!Done)
        {
            Iterate();
        }

        ConnectTilesB();
    }

    //void VisualizeConnectionNumbers()
    //{
    //    foreach (Cell cell in Cells)
    //    {
    //        TextMeshPro txt = Instantiate(_countMeshrePrefab, cell.WorldObj.transform.position + Vector3.up * 12, Quaternion.AngleAxis(90, Vector3.right));
    //        txt.text = cell.CollapsedTile.Neighbours.Count.ToString();
    //    }
    //}

    public List<Cell> DoAstar(Vector2Int startPos, Vector2Int endPos)
    {
        return _aStar.Find(Cells[startPos.y, startPos.x], Cells[endPos.y, endPos.x]);
    }

    public void VisualizeAStar(List<Cell> path)
    {
        if(path == null) return;
        List<GameObject> roads = new();

        //visualising path
        foreach (Cell pathPoint in path)
        {
            pathPoint.WorldObj.FindGameObjectInChildWithTag("road", roads);

            foreach (GameObject road in roads)
            {
                road.GetComponent<MeshRenderer>().material = _pathMat;
            }
        }
    }

    void Iterate()
    {
        Cell lowestCell = GetLeastEntropyCell();
        CollapseCell(lowestCell);
        Propagate(lowestCell);
    }

    void FillGridEdgesWithEmptyTiles(int columns)
    {
        int emptyTileIndex = _tiles.Length - 2;//hard coded to be empty tile

        for (int y = 0; y < columns; y++)//I know I can make a faster loop but meh
            for (int x = 0; x < columns; x++)
            {
                bool isOnEdge = x == 0 || y == 0 || x == Cells.GetLength(1) - 1 || y == Cells.GetLength(0) - 1;

                if (!isOnEdge) continue;

                PrePlaceTile(x, y, _tiles[emptyTileIndex]);
            }
    }

    (Vector2Int, Vector2Int) PutRandomStartAndEnd()
    {
        Vector2Int randomStart;
        Vector2Int randomEnd;
        float manhatanDistance;

        int maxTries = 20;
        do
        {
            //exclude edges because they will already be blanck
            randomEnd = new(Random.Range(2, columns - 3), Random.Range(2, columns-3));
            randomStart = new(Random.Range(2, columns - 3), Random.Range(2, columns-3));
            manhatanDistance = randomStart.GetManhattanDistance(randomEnd);
            maxTries--;
        } while (_objectiveMinDistance > manhatanDistance && maxTries > 0);
        if (maxTries <= 0) Debug.Log("Did not Find close end");

        //get neighbour tiles
        //get random rotation of cap tile
        //if it faces a wall, then continue
        //if not then collapse and return
        //naaah, not gonna do that, too much work

        PrePlaceTile(randomStart.x, randomStart.y, _startTile);
        PrePlaceTile(randomEnd.x, randomEnd.y, _endTile);
        return (randomStart, randomEnd);
    }

    void PrePlaceTile(int gridX, int gridY, Tile tile)
    {
        Cell cell = Cells[gridY, gridX];
        CollapseCellWithTile(cell, tile);
        Propagate(cell);
    }

    public Cell GetLeastEntropyCell()
    {
        Cell min = null;
        foreach (Cell cell in Cells)
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
            for (int x = 0; x < columns; x++)
            {
                if (_visualizeCell)
                {
                    var inst = Instantiate(_cellVisual, transform.position + new Vector3(x * _cellWidth + .5f * _cellWidth, 0, -y * _cellWidth - .5f * _cellWidth), Quaternion.identity);
                    inst.transform.localScale = new Vector3(_cellWidth - .2f, .2f, _cellWidth - .2f);
                }
                Cells[y, x] = new Cell(x, y, new List<Tile>(statesList));
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
        tile.ParentCell = cell;
        //The Y direction is negative because I initialy programed the algorithm in GXPengine and I can't bother
        //to figure out how to write the algorithm with positive y
        var inst = Instantiate(tile.Prefab, transform.position + new Vector3(cell.X * _cellWidth + .5f * _cellWidth, 0, -cell.Y * _cellWidth - .5f * _cellWidth), Quaternion.AngleAxis(tile.Rotation, transform.up), transform);
        inst.transform.localScale = Vector3.one * _cellWidth;
        cell.WorldObj = inst;

        if (++collapsedTileCount >= Cells.Length) Done = true;
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
            neighbours.Add(new CellAndDir(Cells[y, x - 1], NeighbourDir.Left));
        if (x + 1 < Cells.GetLength(1))
            neighbours.Add(new CellAndDir(Cells[y, x + 1], NeighbourDir.Right));
        if (y - 1 >= 0)
            neighbours.Add(new CellAndDir(Cells[y - 1, x], NeighbourDir.Up));
        if (y + 1 < Cells.GetLength(0))
            neighbours.Add(new CellAndDir(Cells[y + 1, x], NeighbourDir.Down));
        return neighbours;
    }


    void ConnectTilesB()
    {
        foreach (Cell cell in Cells)
        {
            int x = cell.X;
            int y = cell.Y;
            Tile currentTile = cell.CollapsedTile;

            if (x - 1 >= 0)
                ConnectTiles(Cells[y, x - 1], currentTile, (int)NeighbourDir.Left);
            if (x + 1 < Cells.GetLength(1))
                ConnectTiles(Cells[y, x + 1], currentTile, (int)NeighbourDir.Right);
            if (y - 1 >= 0)
                ConnectTiles(Cells[y - 1, x], currentTile, (int)NeighbourDir.Up);
            if (y + 1 < Cells.GetLength(0))
                ConnectTiles(Cells[y + 1, x], currentTile, (int)NeighbourDir.Down);

            
        }

        static void ConnectTiles(Cell neighbour, Tile currentTile, int dir)
        {
            string mySockets = currentTile.Sockets[dir];
            Tile neighbourTile = neighbour.CollapsedTile;

            string otherSockets = neighbourTile.Sockets[(dir + 2) % 4];
            if (otherSockets == "AAA") return;

            if (otherSockets == mySockets)//canConnect
            {
                if (!neighbourTile.Neighbours.Contains(currentTile))
                    neighbourTile.Neighbours.Add(currentTile);
                if (!currentTile.Neighbours.Contains(neighbourTile))
                    currentTile.Neighbours.Add(neighbourTile);
            }
        }
    }

    #region BFS version

    //void ConnectAllTiles(int startX, int startY)
    //{
    //    HashSet<Cell> _visitedNodes = new HashSet<Cell>();
    //    Queue<Cell> _queue = new Queue<Cell>();

    //    Cell startCell = Cells[startY, startX];

    //    if (startCell.CollapsedTile == null) return;

    //    _queue.Enqueue(startCell);
    //    _visitedNodes.Add(startCell);

    //    while (_queue.Count > 0)
    //    {
    //        Cell currentCell = _queue.Dequeue();
    //        Tile currentTile = currentCell.CollapsedTile;
    //        int x = currentCell.X;
    //        int y = currentCell.Y;
    //        if (x - 1 >= 0)
    //            ConnectTiles(Cells[y, x - 1], currentTile, (int)NeighbourDir.Left);
    //        if (x + 1 < Cells.GetLength(1))
    //            ConnectTiles(Cells[y, x + 1], currentTile, (int)NeighbourDir.Right);
    //        if (y - 1 >= 0)
    //            ConnectTiles(Cells[y - 1, x], currentTile, (int)NeighbourDir.Up);
    //        if (y + 1 < Cells.GetLength(0))
    //            ConnectTiles(Cells[y + 1, x], currentTile, (int)NeighbourDir.Down);

    //    }


    //    void ConnectTiles(Cell neighbourCell, Tile currentTile, int dir)
    //    {

    //        Tile neighbourTile = neighbourCell.CollapsedTile;
    //        string currentSockets = currentTile.Sockets[dir];

    //        if (currentSockets != "AAA")//I know that it can connect, I should change it so I take advantage of the fact
    //        {
    //            if (!neighbourTile.Neighbours.Contains(currentTile))
    //                neighbourTile.Neighbours.Add(currentTile);
    //            if (!currentTile.Neighbours.Contains(neighbourTile))
    //                currentTile.Neighbours.Add(neighbourTile);
    //        }
    //        if (_visitedNodes.Contains(neighbourCell)) return;

    //        _queue.Enqueue(neighbourCell);
    //        _visitedNodes.Add(neighbourCell);
    //    }
    //}

    //bool CanConnect(string[] socketsA, string socketsB, int dir)
    //{
    //    return socketsA[(dir + 2) % 4] == socketsB;
    //}
    #endregion

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
