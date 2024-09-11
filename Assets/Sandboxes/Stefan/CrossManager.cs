using UnityEngine;

[System.Serializable]
struct Range
{
    public float Min;
    public float Max;
}

public class CrossManager : MonoBehaviour
{
    [SerializeField] Transform _mainObjective;
    [SerializeField] Transform[] _randomObjectives;
    [SerializeField] CrossManager IgnoreFrom;

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<NPC>(out var npc)) return;

        if(IgnoreFrom == null)
        {
            SetDestinationToCrossPoint(_mainObjective.transform, npc);
            return;
        }

        bool senderShouldBeIgnored = ShouldIgnore(npc);

        if (!senderShouldBeIgnored || IgnoreFrom.transform != _mainObjective)
        {
            SetDestinationToCrossPoint(_mainObjective.transform, npc);
            return;
        }

        Transform[] shufledObjectives = _randomObjectives.Clone() as Transform[];
        shufledObjectives.Shuffle();

        foreach (Transform randomObjective in shufledObjectives)
        {
            if (senderShouldBeIgnored && randomObjective == IgnoreFrom.transform) continue;
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
        return npc.Sender == IgnoreFrom.gameObject.name;
    }

    //bool ShouldIgnore(Transform objective)
    //{
    //    return objective.gameObject.name == IgnoreFrom.gameObject.name;
    //}
}
