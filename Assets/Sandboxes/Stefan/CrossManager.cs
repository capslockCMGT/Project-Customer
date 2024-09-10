using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
struct Range
{
    public float Min;
    public float Max;
}

public class CrossManager : MonoBehaviour
{
    [SerializeField] Range _randomCrossRange;
    [SerializeField] Transform _objective;

    float _currentRandomTime;
    float _timer;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        _timer += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsAgent(other.gameObject) && _timer < _currentRandomTime) return;
        ResetTimer();

        Physics.Raycast(_objective.position, -_objective.up,out RaycastHit hit, 10, 1<<LayerMask.NameToLayer("Walkable"));

        var navAgent = other.GetComponent<NavMeshAgent>();
        navAgent.SetDestination(hit.point);
    }

    void ResetTimer()
    {
        _timer = 0;
        _currentRandomTime = Random.Range(_randomCrossRange.Min, _randomCrossRange.Max);
    }

    bool IsAgent(GameObject obj)
    {
        return obj;
    }
}
