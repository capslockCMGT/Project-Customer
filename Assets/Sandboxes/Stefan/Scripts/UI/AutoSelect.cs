using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelect : MonoBehaviour
{
    void OnEnable()
    {
        SelectObject(gameObject);
    }

    public void SelectObject(GameObject go)
    {
        //EventSystem.current.SetSelectedGameObject(go);

    }

}
