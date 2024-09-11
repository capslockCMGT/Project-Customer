
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
    public string[] Sockets { get; }
    public bool SymetryHorizontal { get; }
    public bool SymetryVertical { get; }
    public float Rotation { get; private set; }
    public GameObject Prefab { get; }

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
        this.Rotation = rotation;
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