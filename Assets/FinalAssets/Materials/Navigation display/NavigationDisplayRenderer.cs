using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationDisplayRenderer : MonoBehaviour
{
    [SerializeField] Material DisplayTexMat;
    [SerializeField] Texture2D RoadTexture;
    [SerializeField] Transform Car;

    [SerializeField][Range(0, 1)] float carOnMapDistance;
    [SerializeField] float mapScale = 1;

    Texture2D _mapAsTexture;
    Texture2D _routeAsTexture;
    Texture2D _magicRoadTexture;
    MyGrid _map;
    List<Cell> _currentRoute = null;

    readonly Color zero = new Color(0,0,0,0);
    public void Init(MyGrid grid)
    {
        _map = grid;

        if (_map == null || !_map.Done)
        {
            Debug.LogWarning("Help! NavigationDisplayRenderer *really* wanted a grid but it was null or not done!");
            return;
        }

        //saving bools as bytes... gotta love unity
        _mapAsTexture = new(_map.Cells.GetLength(0), _map.Cells.GetLength(1), TextureFormat.ARGB32, false, false);
        _mapAsTexture.wrapMode = TextureWrapMode.Clamp;
        _mapAsTexture.filterMode = FilterMode.Point;
        for (int x = 0; x < _mapAsTexture.width; x++)
            for (int y = 0; y < _mapAsTexture.height; y++)
                _mapAsTexture.SetPixel(x, y, GetConnections(x, y));
        _mapAsTexture.Apply();

        _routeAsTexture = new(_map.Cells.GetLength(0), _map.Cells.GetLength(1), TextureFormat.ARGB32, false, false);
        _routeAsTexture.wrapMode = TextureWrapMode.Clamp;
        _routeAsTexture.filterMode = FilterMode.Point;
        UpdateRoute();

        MakeHorribleMagicRoadTexture();

        DisplayTexMat.SetInteger("_Width", _map.Cells.GetLength(0));
        DisplayTexMat.SetInteger("_Height", _map.Cells.GetLength(1));
        DisplayTexMat.SetTexture("_MapTex", _mapAsTexture);
        DisplayTexMat.SetTexture("_RoadTex", _magicRoadTexture);
        DisplayTexMat.SetInteger("_DestX", _map.EndPos.x);
        DisplayTexMat.SetInteger("_DestY", _map.EndPos.y);
    }

    void FixedUpdate()
    {
        UpdateRoute();
    }
    private void Update()
    {
        Vector3 relPos = transform.position - _map.transform.position;
        relPos /= _map._size;
        var forward = Car.forward;
        Vector2 dir = new(forward.x, forward.z);
        dir.Normalize();
        dir *= mapScale;
        DisplayTexMat.SetVector("_PositionDirection", new Vector4(-relPos.z, relPos.x, -dir.x, -dir.y));
        DisplayTexMat.SetFloat("_CarOffset", -carOnMapDistance);
    }

    bool HasConnection(Tile tile, Tile otherTile)
    {
        //horrible. unoptimised. at least it works
        return tile.Neighbours.Contains(otherTile);
    }
    Color GetConnections(int x, int y)
    {
        Color res = zero;

        if (x + 1 < _map.Cells.GetLength(0))
            res.a = HasConnection(_map.Cells[x, y].CollapsedTile, _map.Cells[x + 1, y].CollapsedTile) ? 1 : 0;
        if (y + 1 < _map.Cells.GetLength(1))
            res.r = HasConnection(_map.Cells[x, y].CollapsedTile, _map.Cells[x, y + 1].CollapsedTile) ? 1 : 0;
        if (x - 1 >= 0)
            res.g = HasConnection(_map.Cells[x, y].CollapsedTile, _map.Cells[x - 1, y].CollapsedTile) ? 1 : 0;
        if (y - 1 >= 0) 
            res.b = HasConnection(_map.Cells[x, y].CollapsedTile, _map.Cells[x, y - 1].CollapsedTile) ? 1 : 0;

        return res;
    }
    void UpdateRoute()
    {
        Vector3 relPos = transform.position - _map.transform.position;
        relPos /= _map._size;
        Vector2Int pos = new((int)(relPos.x * _map.Cells.GetLength(0)), (int)(-relPos.z * _map.Cells.GetLength(1)));

        bool carIsOnRoute = false;
        if(_currentRoute != null) 
            foreach(var cell in _currentRoute)
                if(cell.X == pos.x && cell.Y == pos.y)
                    carIsOnRoute = true;
        if (carIsOnRoute) return;

        _currentRoute = _map.DoAstar(pos, _map.EndPos);

        for (int x = 0; x < _mapAsTexture.width; x++)
            for (int y = 0; y < _mapAsTexture.height; y++)
                _routeAsTexture.SetPixel(x, y, zero);

        if (_currentRoute == null)
            return;
        for (int i = 0; i < _currentRoute.Count; i++)
        {
            var cell = _currentRoute[i];
            //set to the same cell by default, so difference == 0 and thus no roads will be drawn between these
            Cell prevCell = cell;
            Cell nextCell = cell;
            if (i != 0)
                prevCell = _currentRoute[i - 1];
            if (i != _currentRoute.Count - 1)
                nextCell = _currentRoute[i + 1];

            bool top = cell.Y - prevCell.Y == -1 || cell.Y - nextCell.Y == -1;
            bool right = cell.X - prevCell.X == -1 || cell.X - nextCell.X == -1;
            bool bottom = cell.Y - prevCell.Y == 1 || cell.Y - nextCell.Y == 1;
            bool left = cell.X - prevCell.X == 1 || cell.X - nextCell.X == 1;

            _routeAsTexture.SetPixel(cell.Y, cell.X, new Color(
            (vile)right,
            (vile)bottom,
            (vile)left,
            (vile)top
                ));
        }
        _routeAsTexture.Apply();
        DisplayTexMat.SetTexture("_RouteTex", _routeAsTexture);
    }

    void MakeHorribleMagicRoadTexture()
    {
        _magicRoadTexture = new(RoadTexture.width, RoadTexture.height);
        _magicRoadTexture.wrapMode = TextureWrapMode.Clamp;
        _magicRoadTexture.filterMode = FilterMode.Point;

        for(int x = 0; x< RoadTexture.width; x++)
            for(int y = 0; y< RoadTexture.height; y++)
            {
                Vector2Int pos = new(x, y);
                _magicRoadTexture.SetPixel(x, y, new Color(
                    getFromDir(0,pos).r,
                    getFromDir(1,pos).r,
                    getFromDir(2,pos).r,
                    getFromDir(3,pos).r
                    ));
            }
        _magicRoadTexture.Apply();

        Color getFromDir(int dir, Vector2Int pos)
        {
            Vector2Int point = new();
            switch (dir)
            {
                default:
                    point = pos;
                    break;
                case 1:
                    point = new Vector2Int(pos.y, RoadTexture.height-pos.x);
                    break;
                case 2:
                    point = new Vector2Int(RoadTexture.width-pos.x, RoadTexture.height-pos.y);
                    break;
                case 3:
                    point = new Vector2Int(RoadTexture.width-pos.y, pos.x);
                    break;
            }

            return RoadTexture.GetPixel(point.x, point.y);
        }
    }

    private struct vile
    {
        public float dat;

        public static implicit operator vile(bool dat) => dat? new vile() { dat = 1} : new vile() { dat = 0};
        public static implicit operator float(vile gruh) => gruh.dat;
    }
}
