using System.Collections.Generic;
using UnityEngine;

public enum NeighbourDir
{
    Up,
    Right,
    Down,
    Left,
}

public class Tile 
{
    //sockets are the edges of tiles divided into 3 parts. These three parts are then checked with other tiles to see if they can be connected
    //A is empty
    //B is road
    public Cell ParentCell; //maybeChangeLater?

    public string[] Sockets { get; }
    public bool SymetryHorizontal { get; }
    public bool SymetryVertical { get; }
    public float Rotation { get; private set; }
    public GameObject Prefab { get; }

    public List<Tile> Neighbours { get; } = new List<Tile>();

    public Tile(GameObject prefab, bool horizontalSymetry, bool verticalSymetry, params string[] sockets)
    {
        Rotation = 0;
        Prefab = prefab;
        Sockets = sockets;
        SymetryHorizontal = horizontalSymetry;
        SymetryVertical = verticalSymetry;
    }

    public Tile(GameObject prefab, float rotation, bool horizontalSymetry, bool verticalSymetry, params string[] sockets) 
    {
        Rotation = rotation;
        Sockets = sockets;
        Prefab = prefab;
        SymetryHorizontal = horizontalSymetry;
        SymetryVertical = verticalSymetry;
    }

    public void Rotate()
    {
        string lastSocket = Sockets[^1];
        for (int i = Sockets.Length - 1; i >= 1; i--)
        {
            Sockets[i] = Sockets[i - 1];
        }

        Sockets[0] = lastSocket;

        Rotation += 90;
    }

    public Tile Clone()
    {
        Tile nt = new(Prefab, Rotation, SymetryHorizontal, SymetryVertical, (string[])Sockets.Clone());
        return nt;
    }
}