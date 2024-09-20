using System.Collections;
using UnityEngine;
using System.Linq;

public class PopUpAdsSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _ads;
    [SerializeField] Range _spawnTimeRange;
    Transform[] _randomSpots;

    Coroutine _coroutine;

    void Start()
    {
        StartSpawning();    
        _randomSpots = GetComponentsInChildren<Transform>();
    }
    
    public void StartSpawning()
    {
        _coroutine = StartCoroutine(Timer());
    }

    public void StopSpawning()
    {
        StopCoroutine(_coroutine);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(Random.Range(_spawnTimeRange.Min, _spawnTimeRange.Max));

        PutAdOnSpot(Random.Range(0, _randomSpots.Length));

        _coroutine = StartCoroutine(Timer());
    }

    void PutAdOnSpot(int index)
    {
        Transform spot = _randomSpots[index];

        if (spot.childCount > 0) return;

        Instantiate(_ads[Random.Range(0, _ads.Length)], Vector3.zero, Quaternion.identity, spot);

    }
}
