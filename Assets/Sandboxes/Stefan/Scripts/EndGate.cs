using UnityEngine;

public class EndGate : MonoBehaviour
{
    bool _done;
    void OnTriggerEnter(Collider other)
    {
        if (_done) return;

        if(other.CompareTag("Car"))
        {
            GameManager.Instance.GameOver(true, false);
            _done = true;
        }


    }
}
