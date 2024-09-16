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
    bool _connected;

    public void TransferObjectivesFrom(CrossManager fromCrossPoint)
    {
        _randomObjectives = fromCrossPoint._randomObjectives.Concat(_randomObjectives).ToList();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NPC>(out var npc))
        {
            SendNPC(npc);
        }else if(other.TryGetComponent<CrossManager>(out var otherConenction))
        {
            Connect(otherConenction);
        }

        
    }

    void SendNPC(NPC npc)
    {
        UpdateObjectives();

        if (npc.Sender == null)
        {
            SetDestinationToCrossPoint(_randomObjectives[Random.Range(0, _randomObjectives.Count)], npc);
            return;
        }

        List<Transform> shufledObjectives = new(_randomObjectives);
        shufledObjectives.Shuffle();

        foreach (Transform randomObjective in shufledObjectives)
        {
            if (randomObjective == npc.Sender) continue;
            SetDestinationToCrossPoint(randomObjective, npc);
            break;
        }
    }


    void Connect(CrossManager otherConnection)
    {
        if(_connected) return;

        TransferObjectivesFrom(otherConnection);
        otherConnection._connected = true;
        Destroy(otherConnection.gameObject);
    }

    void UpdateObjectives()
    {
        _randomObjectives.RemoveAll(o => o == null);
    }

    void SetDestinationToCrossPoint(Transform crossPoint, NPC npc)
    {

        npc.Sender = transform;

        Physics.Raycast(crossPoint.position + Vector3.up * 11, -Vector3.up, out RaycastHit hit, 20, 1 << LayerMask.NameToLayer("Walkable"));
        npc.gixmo = hit.point;
        Debug.Log("Setting destonation to " + crossPoint.gameObject.name + "pos: " + hit.point);

        npc.Agent.SetDestination(hit.point);
    }

}
