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
        if (npc.Sender == null)
        {
            SetDestinationToCrossPoint(_randomObjectives[Random.Range(0, _randomObjectives.Count)], npc);
            return;
        }

        if(_randomObjectives.Count == 1)
        {
            SetDestinationToCrossPoint(_randomObjectives[0], npc);
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

    //in this function I am merging two connection points into one to have less colliders
    //because the deleted objects are referenced in the "_randomObjectives" list, I need to replace the future deleted objects with the one that will remain
    //example:
    //===sidewalk======connectionA connectionA=========sidewalk========
    //===sidewalk======connectionB connectionB=========sidewalk========
    //one pair of connections will be romoved:
    //===sidewalk======connectionA =========sidewalk========
    //===sidewalk======connectionB =========sidewalk========
    //in this case, the right sidewalks need to have the references of the connections from the
    //333left sidewalks
    void Connect(CrossManager otherConnection)
    {
        if(_connected) return;

        TransferObjectivesFrom(otherConnection);
        //to prevent the other script from removing this script
        otherConnection._connected = true;
        //search the connection that we will be deleting, so I can replace it with the one with this script
        foreach (CrossManager connection in otherConnection.transform.parent.GetComponentsInChildren<CrossManager>(true))
        {
            int toBeDeletedConnectionIndex = connection._randomObjectives.IndexOf(otherConnection.transform);
            if (toBeDeletedConnectionIndex == -1) continue;

            connection._randomObjectives[toBeDeletedConnectionIndex] = transform;
        }

        Destroy(otherConnection.gameObject);
    }

    void SetDestinationToCrossPoint(Transform crossPoint, NPC npc)
    {
        if(crossPoint == null) return;
        npc.Sender = transform;
        //doing a spherecast because some connections are not alligned properly so a simple ray just misses the sidewalk
        Physics.SphereCast(crossPoint.position + Vector3.up * 11,5, -Vector3.up, out RaycastHit hit, 20, 1 << LayerMask.NameToLayer("Walkable"));
        //Physics.Raycast(crossPoint.position + Vector3.up * 11, -Vector3.up, out RaycastHit hit, 20, 1 << LayerMask.NameToLayer("Walkable"));
        npc.gixmo = hit.point;
        //Debug.Log("Setting destonation to " + crossPoint.gameObject.name + "pos: " + hit.point);

        npc.Agent.SetDestination(hit.point);
    }

}
