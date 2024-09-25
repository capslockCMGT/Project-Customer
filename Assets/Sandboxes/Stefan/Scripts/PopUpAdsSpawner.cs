using System.Collections;
using System.Linq;
using UnityEngine;

public class PopUpAdsSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _ads;
    [SerializeField] Range _spawnTimeRange;
    [SerializeField] float _activationCooldown;
    [SerializeField] SoundName _popUpSound;
    Transform[] _randomSpots;

    float _time;


    void Start()
    {
        _randomSpots = GetComponentsInChildren<Transform>();
    }
    
    public void PutAdOnRandomSpot()
    {
        if (Time.time - _time < _activationCooldown) return;

        _time = Time.time;
        Transform[] openSpots = _randomSpots.Where(s => s.childCount == 0).ToArray();
        if (openSpots.Length == 0) return;

        Instantiate(_ads[Random.Range(0, _ads.Length)], openSpots[Random.Range(0, openSpots.Length)]);
        SoundManager.Instance.PlaySound(_popUpSound);
    }
}
