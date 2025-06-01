using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowButton : MonoBehaviour
{
    [SerializeField]
    GameObject shownObj;

    public void OnInteract(Action<GameObject> toggle)
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => toggle.Invoke(shownObj));
    }

    void OnDisable()
    {
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
