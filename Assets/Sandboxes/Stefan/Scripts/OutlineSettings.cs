using UnityEngine;

public class OutlineSettings : MonoBehaviour
{
    public float OutlineWidth = 8f;
    public Color OutlineColor = Color.black;
    public Outline.Mode OutlineMode = Outline.Mode.OutlineVisible;

    public static OutlineSettings Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

    }

}
