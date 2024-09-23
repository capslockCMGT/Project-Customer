using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenToTutorial : MonoBehaviour
{
    [SerializeField] SoundName _soundName;
    public float SceneDuration = 12f;
    public float Timer;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySound(_soundName);
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > SceneDuration)
        {
            SceneManager.LoadScene("Tutorial Scene");
        }
    }
}
