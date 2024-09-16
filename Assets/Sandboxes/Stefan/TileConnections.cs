using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConnections : MonoBehaviour
{
    public struct ConnectionPair
    {
        public CrossManager A;
        public CrossManager B;
    }

    [SerializeField] ConnectionPair _pairUp;
    [SerializeField] ConnectionPair _pairRight;
    [SerializeField] ConnectionPair _pairDown;
    [SerializeField] ConnectionPair _pairLeft;

    ConnectionPair[] _pairs;

    void Awake()
    {
        _pairs = new ConnectionPair[4] { _pairUp, _pairRight, _pairDown, _pairLeft };
    }

    public ConnectionPair GetConnection(NeighbourDir direction)
    {
        UpdateDirections();


        return direction switch
        {
            NeighbourDir.Up => _pairUp,
            NeighbourDir.Right => _pairRight,
            NeighbourDir.Down => _pairDown,
            NeighbourDir.Left => _pairLeft,
            _ => new(),
        };
    }

    void UpdateDirections()
    {
        int upIndex = GetWorldUpIndex();

        Debug.Assert(upIndex >= 0);

        //I'm modulating by 4 to cycle back when out of bounds
        _pairUp = _pairs[upIndex];
        _pairRight = _pairs[(upIndex + 1) % 4]; 
        _pairDown = _pairs[(upIndex + 2) % 4];
        _pairLeft = _pairs[(upIndex + 3) % 4];
        
    }

    //returns in what array position is the world up pair
    int GetWorldUpIndex()
    {
        for (int i = 0; i < _pairs.Length; i++)
        {
            //getting the difference from center to check if point is in up direction relative to world coordinates
            //i'm checking if value is negative because the up direction is flipped in the grid system
            bool isAbove = _pairs[i].A.transform.position.z - transform.position.z < 0;
            if (isAbove) 
                return i;
        }
        return -1;
    }

}
