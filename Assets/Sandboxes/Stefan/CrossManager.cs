using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
struct Range
{
    public float Min;
    public float Max;
}

public class CrossManager : MonoBehaviour
{
    [SerializeField] List<Transform> _randomObjectives;
    [SerializeField] Transform _ignoreFrom;

    public void TransferObjectivesFrom(CrossManager fromCrossPoint)
    {
        _randomObjectives = fromCrossPoint._randomObjectives.Concat(_randomObjectives).ToList();
    }

    public void SetIgnoreTransform(Transform ignore)
    {
        _ignoreFrom = ignore;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<NPC>(out var npc)) return;

        bool senderShouldBeIgnored = ShouldIgnore(npc);

        if(_ignoreFrom == null || !senderShouldBeIgnored)
        {
            SetDestinationToCrossPoint(_randomObjectives[Random.Range(0,_randomObjectives.Count)], npc);
            return;
        }

        List<Transform> shufledObjectives = new(_randomObjectives);
        shufledObjectives.Shuffle();

        foreach (Transform randomObjective in shufledObjectives)
        {
            if (randomObjective == _ignoreFrom.transform) continue;
            SetDestinationToCrossPoint(randomObjective, npc);
            break;
        }
    }


    void SetDestinationToCrossPoint(Transform crossPoint, NPC npc)
    {
        Debug.Log("Setting destonation to " + crossPoint.gameObject.name);

        npc.Sender = gameObject.name;

        Physics.Raycast(crossPoint.position, -crossPoint.up, out RaycastHit hit, 10, 1 << LayerMask.NameToLayer("Walkable"));
        npc.gixmo = hit.point;

        npc.Agent.SetDestination(hit.point);
    }

    

    bool ShouldIgnore(NPC npc)
    {
        return npc.Sender == _ignoreFrom.gameObject.name;
    }

    //bool ShouldIgnore(Transform objective)
    //{
    //    return objective.gameObject.name == IgnoreFrom.gameObject.name;
    //}
}
