using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CityGenerator : MonoBehaviour
{
    [SerializeField] float _time = .2f;
    [SerializeField] GameObject _prefab1;
    [SerializeField] GameObject _prefab2;
    [SerializeField] GameObject _prefab3;
    [SerializeField] GameObject _prefab4;
    [SerializeField] GameObject _prefab5;
    [SerializeField] GameObject _prefab6;

    readonly Tile[] _tiles = new Tile[6];
    MyGrid grid;
    float _timer = 0;

    void Awake()
    {
        grid = GetComponent<MyGrid>();
        //hardcoded the simetries.
        //The simetries are needed to save memory on repeating the tiles that after rotation
        //look the same

        _tiles[0] = new Tile( _prefab1,true, true, "ABA", "ABA", "ABA", "ABA");
        _tiles[1] = new Tile( _prefab2,true, false, "AAA", "ABA", "AAA", "ABA");
        _tiles[2] = new Tile( _prefab3,false, true, "AAA", "ABA", "ABA", "ABA");
        _tiles[3] = new Tile( _prefab4,false, false, "ABA", "ABA", "AAA", "AAA");
        _tiles[4] = new Tile( _prefab5,true, true, "AAA", "AAA", "AAA", "AAA");
        _tiles[5] = new Tile( _prefab6,false, false, "AAA", "AAA", "AAA", "ABA");

    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (grid.Done || _timer > _time) return;
        
        _timer = _time;
        Iterate();
    }

    void Iterate()
    {
        Cell lowestCell = grid.GetLeastEntropyCell();
        grid.CollapseCell(lowestCell);
        grid.Propagate(lowestCell);
    }
}
