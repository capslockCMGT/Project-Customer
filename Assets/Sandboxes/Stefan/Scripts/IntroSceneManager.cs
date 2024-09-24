using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] SoundName _introSound;

    void Start()
    {
        SoundManager.Instance.PlaySound(_introSound, OnIntroSoundEnd);    
    }

    void OnIntroSoundEnd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
