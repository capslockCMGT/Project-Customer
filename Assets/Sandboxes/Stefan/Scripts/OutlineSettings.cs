using UnityEngine;

public class OutlineSettings : MonoBehaviour
{
    public float OutlineWidth = 8f;
    public Color OutlineGrabColor = Color.black;
    public Color OutlineLookColor = Color.yellow;
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
