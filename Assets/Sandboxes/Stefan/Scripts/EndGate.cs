using UnityEngine;

public class EndGate : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CarController>(out _))
        {
            GameManager.Instance.FinishLevel();
        }


    }
}
