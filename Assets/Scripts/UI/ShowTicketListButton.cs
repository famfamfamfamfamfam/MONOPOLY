using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowTicketListButton : MonoBehaviour
{
    [SerializeField]
    GameObject ticketList;

    public void OnInteract(Action<GameObject> toggle)
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => toggle.Invoke(ticketList));
    }

    void OnDisable()
    {
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
