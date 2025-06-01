using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowButton : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    GameObject shownObj;

    public void OnInteract(Action<GameObject> toggle)
    {
        button.onClick.AddListener(() => toggle.Invoke(shownObj));
    }

    void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
