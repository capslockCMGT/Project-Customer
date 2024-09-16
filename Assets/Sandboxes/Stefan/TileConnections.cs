using System;
using UnityEngine;
[Serializable]
public struct ConnectionPair
{
    public CrossManager A;
    public CrossManager B;
}

public class TileConnections : MonoBehaviour
{
    [SerializeField] ConnectionPair _pairUp;
    [SerializeField] ConnectionPair _pairRight;
    [SerializeField] ConnectionPair _pairDown;
    [SerializeField] ConnectionPair _pairLeft;

    ConnectionPair[] _pairs;

    void Awake()
    {
        _pairs = new ConnectionPair[4] { _pairUp, _pairRight, _pairDown, _pairLeft };
    }

    public void Connect(TileConnections otherConnection, NeighbourDir direction)
    {
        switch (direction)
        {
            case NeighbourDir.Up:
                MergePairs(_pairUp, otherConnection._pairDown);
                otherConnection._pairDown = _pairUp;
                otherConnection._pairs[2] = _pairUp;
                break;
            case NeighbourDir.Right:
                MergePairs(_pairRight, otherConnection._pairLeft);
                otherConnection._pairLeft = _pairRight;
                otherConnection._pairs[3] = _pairRight;

                break;
            case NeighbourDir.Down:
                MergePairs(_pairDown, otherConnection._pairUp);
                otherConnection._pairUp = _pairDown;
                otherConnection._pairs[0] = _pairDown;

                break;
            case NeighbourDir.Left:
                MergePairs(_pairLeft, otherConnection._pairRight);
                otherConnection._pairRight = _pairLeft;
                otherConnection._pairs[1] = _pairLeft;

                break;
        }

    }

    void MergePairs(ConnectionPair myPair, ConnectionPair otherPair)
    {
        myPair.A.TransferObjectivesFrom(otherPair.A);
        myPair.B.TransferObjectivesFrom(otherPair.B);

        Destroy(otherPair.A.gameObject);
        Destroy(otherPair.B.gameObject);
    }

    public void UpdateDirections()
    {
        int upIndex = GetWorldUpIndex();

        Debug.Assert(upIndex >= 0);

        //I'm modulating by 4 to cycle back when out of bounds
        _pairUp = _pairs[upIndex];
        _pairRight = _pairs[(upIndex + 1) % 4]; 
        _pairDown = _pairs[(upIndex + 2) % 4];
        _pairLeft = _pairs[(upIndex + 3) % 4];
        
    }

    

}
