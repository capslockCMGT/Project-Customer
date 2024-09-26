using UnityEngine;

public class EndGate : MonoBehaviour
{
    bool _done;
    [SerializeField] bool _isTutorial;

    void OnTriggerEnter(Collider other)
    {
        if (_done) return;

        if(other.CompareTag("Car"))
        {
            if (_isTutorial)
                GameManager.Instance.FinishLevel();
            else
                GameManager.Instance.GameOver(true, false);
            _done = true;
        }


    }
}
